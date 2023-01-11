using Microsoft.Win32;
using Prism.Services.Dialogs;
using Reactive.Bindings;
using Reactive.Bindings.ObjectExtensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace VolleyballScoreSheet.Shared
{
    public class Substitution
    {
        public List<Player> OncourtMember;
        public List<Player> OffCourtMember;
        public Player OutMember;
        public Player InMember;
        public string TeamName;
        public bool isATeam;
        public ReactiveCommand SubstitutionCountNotifyCommand { get; } = new();
        public ReactiveCommand SubstitutionOverSixTimeCommand { get; } = new();
        public ReactiveCommand SubstitutionCommand { get; } = new();
        private readonly Game _game;
        public Substitution(Game game)
        {
            _game = game;
        }
        public void DoSubstitution(bool isLeft, Player OutPlayer, Player InPlayer)
        {
            bool isA;
            if (isLeft)
            {
                isA = _game.isATeamLeft.Value;
            }
            else
            {
                isA = !_game.isATeamLeft.Value;
            }

            if (isA)
            {
                _game.ATeam.Value.Substitution(InPlayer.Id, OutPlayer.Id, _game.ATeam.Value.Sets[^1].Points.Value, _game.BTeam.Value.Sets[^1].Points.Value);
                _game.History.HistoryAdd($"SubstitutionA", $"{InPlayer.Id},{OutPlayer.Id}");


                if (_game.ATeam.Value.Sets[^1].Substitutions.Value == 5)
                {
                    SubstitutionCountNotifyCommand.Execute(5);
                }
                else if (_game.ATeam.Value.Sets[^1].Substitutions.Value == 6)
                {
                    SubstitutionCountNotifyCommand.Execute(6);
                }

            }
            else
            {
                _game.BTeam.Value.Substitution(InPlayer.Id, OutPlayer.Id, _game.BTeam.Value.Sets[^1].Points.Value, _game.ATeam.Value.Sets[^1].Points.Value);
                _game.History.HistoryAdd($"SubstitutionB", $"{InPlayer.Id},{OutPlayer.Id}");
                if (_game.BTeam.Value.Sets[^1].Substitutions.Value == 5)
                {
                    SubstitutionCountNotifyCommand.Execute(5);
                }
                else if (_game.BTeam.Value.Sets[^1].Substitutions.Value == 6)
                {
                    SubstitutionCountNotifyCommand.Execute(6);
                }
            }
        }
        public void OutMemberSelectionChanged(bool isLeft, int player)
        {
            Team team;
            if (isLeft)
            {
                team = _game.LeftTeam;
            }
            else
            {
                team = _game.RightTeam;
            }

            OutMember = team.Players.First(x => x.Id==player);

            var 選手交代で出たことある人リスト = team.Players.Where(x => team.Sets[^1].SubstitutionDetails.Select(x => x.Out).Contains(x.Id));
            var 選手交代で入ったことある人リスト = team.Players.Where(x => team.Sets[^1].SubstitutionDetails.Select(x => x.In).Contains(x.Id));
            var onCourtPlayer = team.Players.Where(x => team.Sets[^1].Rotation.Value.Contains(x.Id));
            var liberos = team.Players.Where(x => x.IsLibero);
            var disqualifiedPlayer = team.Players.Where(x => x.IsDisqualified);
            var 例外的な選手交代で出た人リスト = team.Players
               .Where(x => team.Sets[^1].SubstitutionDetails.Where(x => x.ExceptionalSubstitution == true)
               .Select(x => x.Out).Contains(x.Id));


            var 例外的な選手交代で入った人リスト = team.Players
                .Where(x => team.Sets[^1].SubstitutionDetails.Where(x => x.ExceptionalSubstitution == true)
                .Select(x => x.In).Contains(x.Id));

            var 選手交代で入れる人リスト = team.Players
                .Except(liberos)
                .Except(onCourtPlayer)
                .Except(選手交代で出たことある人リスト)
                .Except(disqualifiedPlayer)
                .OrderBy(x => x.Id)
                .ToArray();

            var 選手交代で下がれる人リスト = onCourtPlayer
               .Except(選手交代で出たことある人リスト)
               .Except(例外的な選手交代で入った人リスト)
               .OrderBy(x => x.Id).ToList();



            if (選手交代で入ったことある人リスト.Contains(OutMember))
            {

                //再入場
                var a = team.Sets[^1].SubstitutionDetails
                    .Where(x => x.In == OutMember.Id)
                    .Select(x => x.Out);
                OffCourtMember = team.Players.Where(x => a.Contains(x.Id)).ToList();

                InMember = OffCourtMember.First();
            }
            else if (!選手交代で入れる人リスト.Any())
            {
                OffCourtMember = new();
                return;
            }
            else
            {
                OffCourtMember = 選手交代で入れる人リスト
                     .Except(選手交代で出たことある人リスト).ToList();
            }

            OncourtMember = 選手交代で下がれる人リスト;
        }
        public void SubstitutionOpenDialog(bool isLeft, int? outPlayer = null)
        {
            Team team;
            if (isLeft)
            {
                team = _game.LeftTeam;
            }
            else
            {
                team = _game.RightTeam;
            }

            var 選手交代で出たことある人リスト = team.Players.Where(x => team.Sets[^1].SubstitutionDetails.Select(x => x.Out).Contains(x.Id)).ToList();
            var 選手交代で入ったことある人リスト = team.Players.Where(x => team.Sets[^1].SubstitutionDetails.Select(x => x.In).Contains(x.Id)).ToList();
            var onCourtPlayer = team.Players.Where(x => team.Sets[^1].Rotation.Value.Contains(x.Id)).ToList();
            var liberos = team.Players.Where(x => x.IsLibero).ToList();
            var disqualifiedPlayer = team.Players.Where(x => x.IsDisqualified).ToList();

            var 例外的な選手交代で出た人リスト = team.Players
                .Where(x => team.Sets[^1].SubstitutionDetails.Where(x => x.ExceptionalSubstitution == true)
                .Select(x => x.Out).Contains(x.Id));

            var 選手交代で入れる人リスト = team.Players.ToList()
                .Except(liberos)
                .Except(onCourtPlayer)
                .Except(選手交代で入ったことある人リスト)
                .Except(disqualifiedPlayer)
                .Except(例外的な選手交代で出た人リスト)
                .OrderBy(x => x.Id).ToList();


            var 例外的な選手交代で入った人リスト = team.Players
                .Where(x => team.Sets[^1].SubstitutionDetails.Where(x => x.ExceptionalSubstitution == true)
                .Select(x => x.In).Contains(x.Id));


            var 選手交代で下がれる人リスト = onCourtPlayer
                .Except(選手交代で出たことある人リスト)
                .Except(例外的な選手交代で入った人リスト)
                .OrderBy(x => x.Id).ToList();

            var offCourtPlayers = team.Players.Select(x => x.Id)
                .Except(team.Sets[^1].Rotation.Value).ToArray();


            OncourtMember = 選手交代で下がれる人リスト;
            OffCourtMember = 選手交代で入れる人リスト;

            if (outPlayer is not null)
            {
                OutMember = team.Players.First(x => x.Id == (int)outPlayer);
            }
        }

        public bool CanSubstitutedLegally(bool isA, Player OutPlayer)
        {
            var sub = new Substitution(_game);

            Team team;
            if (isA)
            {
                team = _game.ATeam.Value;
            }
            else
            {
                team = _game.BTeam.Value;
            }

            var 選手交代で出たことある人リスト = team.Players.Where(x => team.Sets[^1].SubstitutionDetails.Select(x => x.Out).Contains(x.Id));
            var 選手交代で入ったことある人リスト = team.Players.Where(x => team.Sets[^1].SubstitutionDetails.Select(x => x.In).Contains(x.Id));
            var onCourtPlayer = team.Players.Where(x => team.Sets[^1].Rotation.Value.Contains(x.Id));
            var liberos = team.Players.Where(x => x.IsLibero);
            var disqualifiedPlayer = team.Players.Where(x => x.IsDisqualified);

            var 例外的な選手交代で入った人リスト = team.Players
                .Where(x => team.Sets[^1].SubstitutionDetails.Where(x => x.ExceptionalSubstitution == true)
                .Select(x => x.In).Contains(x.Id));

            var 選手交代で入れる人リスト = team.Players
                .Except(liberos)
                .Except(onCourtPlayer)
                .Except(選手交代で入ったことある人リスト)
                .Except(disqualifiedPlayer)
                .OrderBy(x => x.Id)
                .ToArray();

            var 選手交代で下がれる人リスト = onCourtPlayer
               .Except(選手交代で出たことある人リスト)
               .Except(例外的な選手交代で入った人リスト)
               .OrderBy(x => x.Id).ToList();

            if (!選手交代で下がれる人リスト.Contains(OutPlayer))
            {
                //正規の選手交代が出来ない
                return false;
            }

            if (選手交代で入れる人リスト.Count() == 0)
            {
                //正規の選手交代が出来ない
                return false;
            }

            if (team.Sets[^1].Substitutions.Value == 6)
            {
                //正規の選手交代が出来ない
                return false;
            }

            if (!選手交代で入れる人リスト.Except(選手交代で出たことある人リスト).Any())
            {
                //正規の選手交代が出来ない
                return false;
            }

            return true;
        }
        public bool CanExceptionalSubstitution(bool isA)
        {
            var sub = new Substitution(_game);

            Team team;
            if (isA)
            {
                team = _game.ATeam.Value;
            }
            else
            {
                team = _game.BTeam.Value;
            }

            var コート外の選手 = team.Players.Select(x => x.Id).Except(team.Sets[^1].Rotation.Value).ToArray();
            var リベロ除く = コート外の選手.Except(team.Players.Where(x => x.IsLibero == true).Select(x => x.Id).ToList()).ToArray();
            var 退場選手除く = リベロ除く.Except(team.Players.Where(x => x.IsExplusion[_game.Set.Value]).Select(x => x.Id).ToList()).ToArray();
            var 失格選手除く = 退場選手除く.Except(team.Players.Where(x => x.IsDisqualified).Select(x => x.Id).ToList()).ToArray();
            var 例外的な選手交代をしたことある人を除く = 失格選手除く.Except(team.Players.Where(x => x.IsExceptionalSubstituted).Select(x => x.Id).ToList()).ToArray();

            if (例外的な選手交代をしたことある人を除く.Length == 0)
            {
                return false;
            }
            return true;
        }
        public void LegallySubstitution(bool isLeft)
        {
            Team team;
            if (isLeft)
            {
                team = _game.LeftTeam;
            }
            else
            {
                team = _game.RightTeam;
            }

            if (team.Sets[^1].Substitutions.Value == 6)
            {
                //回数超え
                SubstitutionOverSixTimeCommand.Execute();
            }
            else
            {
                SubstitutionCommand.Execute(isLeft);
            }
        }
    }
}
