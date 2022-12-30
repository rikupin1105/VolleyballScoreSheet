using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace VolleyballScoreSheet.Model
{
    public class Roaster : BinableBase
    {
        private readonly Game _game;
        public ObservableCollection<Player> ATeamPlayers { get; set; } = new();
        public ObservableCollection<Player> BTeamPlayers { get; } = new();
        public ReactiveCommand AlertCommand = new();

        public ReactiveProperty<Player> InputPlayerA { get; set; } = new();
        public ReactiveProperty<Player> InputPlayerB { get; set; } = new();
        public bool IsError { get; set; } = false;


        public Roaster(Game game)
        {
            _game = game;

            InputPlayerA.Value = new();
            InputPlayerB.Value = new();
        }
        public void PlayerLoad(bool isATeam, string FileName)
        {
            var reader = new StreamReader(FileName);
            var json = reader.ReadToEnd();
            reader.Dispose();

            List<Player>? players = JsonSerializer.Deserialize<List<Player>>(json);

            if (players is null) return;

            if (isATeam)
            {
                ATeamPlayers.Clear();
                ATeamPlayers.AddRange(players);
            }
            else
            {
                BTeamPlayers.Clear();
                BTeamPlayers.AddRange(players);
            }
        }
        public void PlayerAdd(bool isATeam)
        {
            if (isATeam)
            {
                if (InputPlayerA.Value.Id is null) return;

                if (ATeamPlayers.Count >= 14)
                {
                    //14人以上は登録できません
                    AlertCommand.Execute("14人以上は登録できません。");
                    return;
                }
                if (ATeamPlayers.Select(x => x.Id).Contains(InputPlayerA.Value.Id))
                {
                    AlertCommand.Execute($"{InputPlayerA.Value.Id}番はすでに登録されています。");
                    return;
                }

                ATeamPlayers.Add(InputPlayerA.Value);
                InputPlayerA.Value = new();
            }
            else
            {
                if (InputPlayerB.Value.Id is null) return;

                if (BTeamPlayers.Count >= 14)
                {
                    //14人以上は登録できません
                    AlertCommand.Execute("14人以上は登録できません。");
                    return;
                }
                if (BTeamPlayers.Select(x => x.Id).Contains(InputPlayerB.Value.Id))
                {
                    AlertCommand.Execute($"{InputPlayerB.Value.Id}番はすでに登録されています。");
                    return;
                }

                BTeamPlayers.Add(InputPlayerB.Value);
                InputPlayerB.Value = new();
            }
        }
        public void PlayerDelete(bool isATeam, Player player)
        {
            if (isATeam)
            {
                ATeamPlayers.Remove(player);
            }
            else
            {
                BTeamPlayers.Remove(player);
            }
        }
        public void PlayerEdit(bool isATeam, Player player, int playerId)
        {
            var teamPlayer = ATeamPlayers;

            if (!isATeam)
            {
                teamPlayer = BTeamPlayers;
            }

            if (player.Id == playerId)
            {
                //番号が一緒
                var index = teamPlayer.IndexOf(teamPlayer.First(x => x.Id==playerId));
                teamPlayer[index] = player;
            }
            else
            {
                //番号が変わっている
                if (teamPlayer.Select(x => x.Id).Contains(player.Id))
                {
                    AlertCommand.Execute($"すでに{player.Id}番は登録されています。");
                }
                else
                {
                    var index = teamPlayer.IndexOf(teamPlayer.First(x => x.Id==playerId));
                    teamPlayer[index] = player;
                }
            }
        }
        public void Validate()
        {
            if (ATeamPlayers.Count == 0 || BTeamPlayers.Count == 0)
            {
                IsError = true;
                AlertCommand.Execute("選手を登録してください");
                return;
            }

            if (ATeamPlayers.Count > 14 || BTeamPlayers.Count > 14)
            {
                IsError = true;
                AlertCommand.Execute("14人以上は登録できません");
                return;
            }

            if (ATeamPlayers.Count(x => !x.IsLibero) < 6 || BTeamPlayers.Count(x => !x.IsLibero) < 6)
            {
                IsError = true;
                AlertCommand.Execute("リベロ以外のプレイヤーが6人以上必要です");
                return;
            }

            if (ATeamPlayers.Count(x => x.IsCaptain) >= 2 || BTeamPlayers.Count(x => x.IsCaptain) >= 2)
            {
                IsError = true;
                AlertCommand.Execute("チームキャプテンは1人にしてください");
                return;
            }

            if (ATeamPlayers.Count(x => x.IsLibero) >= 3 || BTeamPlayers.Count(x => x.IsLibero) >= 3)
            {
                IsError = true;
                AlertCommand.Execute("リベロは2人まで登録できます");
                return;
            }

            if (ATeamPlayers.Count(x => x.IsLibero) == 1 && ATeamPlayers.Count > 12 || BTeamPlayers.Count(x => x.IsLibero) == 1 && BTeamPlayers.Count > 12)
            {
                //リベロが一人のとき12人までしか登録できない

                IsError = true;
                AlertCommand.Execute("プレイヤーが12人以上の場合、リベロは2人登録してください");
                return;

            }


            foreach (var player in ATeamPlayers)
            {
                _game.ATeam.Value.Players.Add(new(player));
            }

            foreach (var player in BTeamPlayers)
            {
                _game.BTeam.Value.Players.Add(new(player));
            }

            IsError = false;
        }
        public class Player
        {
            public Player(int? id, string? name, bool isLibero = false, bool isCaptain = false)
            {
                Id=id;
                Name=name;
                IsLibero=isLibero;
                IsCaptain=isCaptain;
            }
            public Player() { }
            public int? Id { get; set; }
            public string? Name { get; set; }
            public bool IsLibero { get; set; }
            public bool IsCaptain { get; set; }
        }
    }
}
