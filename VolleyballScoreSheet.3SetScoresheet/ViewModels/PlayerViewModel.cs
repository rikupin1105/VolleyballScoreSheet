using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace VolleyballScoreSheet._3SetScoresheet.ViewModels
{
    public class PlayerViewModel : BindableBase
    {
        private readonly Game _game;
        public PlayerViewModel(Game game)
        {
            _game = game;

            ATeamName = _game.ATeam.Value.Name.Value;
            BTeamName = _game.BTeam.Value.Name.Value;

            foreach (var item in _game.ATeam.Value.Players.OrderBy(x => x.Id).ToList())
            {
                ATeamPlayer.Add(new()
                {
                    Id = item.Id,
                    Name = item.Name,
                    IsLibero = item.IsLibero
                });
            }
            for (int i = 0; i < 14 - ATeamPlayer.Count; i++)
            {
                ATeamPlayer.Add(new()
                {
                    Id = null,
                    Name = null,
                    IsLibero = null
                });
            }

            foreach (var item in _game.BTeam.Value.Players.OrderBy(x => x.Id).ToList())
            {
                BTeamPlayer.Add(new()
                {
                    Id = item.Id,
                    Name = item.Name,
                    IsLibero = item.IsLibero
                });
            }
            for (int i = 0; i < 14 - BTeamPlayer.Count; i++)
            {
                BTeamPlayer.Add(new()
                {
                    Id = null,
                    Name = null,
                    IsLibero = null
                });
            }

            foreach (var item in _game.ATeam.Value.Players.Where(x => x.IsLibero).OrderBy(x => x.Id).ToList())
            {
                ATeamLibero.Add(new()
                {
                    Id = item.Id,
                    Name = item.Name,
                    IsLibero = item.IsLibero
                });
            }
            for (int i = 0; i < 2 - ATeamLibero.Count; i++)
            {
                ATeamPlayer.Add(new()
                {
                    Id = null,
                    Name = null,
                    IsLibero = null
                });
            }

            foreach (var item in _game.BTeam.Value.Players.Where(x => x.IsLibero).OrderBy(x => x.Id).ToList())
            {
                BTeamLibero.Add(new()
                {
                    Id = item.Id,
                    Name = item.Name,
                    IsLibero = item.IsLibero
                });
            }
            for (int i = 0; i < 2 - BTeamLibero.Count; i++)
            {
                BTeamPlayer.Add(new()
                {
                    Id = null,
                    Name = null,
                    IsLibero = null
                });
            }

            if (_game.CoinToss.CoinTossCompleted)
            {
                if (_game.CoinToss.ATeamLeftSide)
                {
                    ATeamA = 'A';
                    BTeamA = 'B';
                }
                else
                {
                    ATeamA = 'B';
                    BTeamA = 'A';
                }
            }
        }
        public char ATeamA { get; set; }
        public char BTeamA { get; set; }
        public string ATeamName { get; set; }
        public string BTeamName { get; set; }
        public List<PlayerForScoresheet> ATeamPlayer { get; set; } = new();
        public List<PlayerForScoresheet> BTeamPlayer { get; set; } = new();
        public List<PlayerForScoresheet> ATeamLibero { get; set; } = new();
        public List<PlayerForScoresheet> BTeamLibero { get; set; } = new();

    }
    public class PlayerForScoresheet
    {

        public int? Id { get; set; }
        public string? Name { get; set; }
        public bool? IsLibero { get; set; }
    }
}
