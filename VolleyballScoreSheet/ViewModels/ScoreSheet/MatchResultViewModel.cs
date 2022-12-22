using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using Unity.Injection;
using VolleyballScoreSheet.Model;
using VolleyballScoreSheet.Views;
using Wpf.Ui.Interop.WinDef;

namespace VolleyballScoreSheet.ViewModels.ScoreSheet
{
    public class MatchResultViewModel : BindableBase
    {
        private readonly Game _game;
        public MatchResultViewModel(Game game)
        {
            _game = game;
            ATeamName = _game.ATeam.Value.Name.Value;
            BTeamName = _game.BTeam.Value.Name.Value;

            for (int i = 0; i < _game.Set.Value; i++)
            {
                ASet[i] = new()
                {
                    Timeouts= _game.ATeam.Value.Sets[i].TimeOuts.Value,
                    Substitutions = _game.ATeam.Value.Sets[i].Substitutions.Value + _game.ATeam.Value.Sets[i].SubstitutionDetails.Where(x => x.ExceptionalSubstitution == true).Count(),

                    Points = _game.ATeam.Value.Sets[i].Points.Value,
                };
                BSet[i] = new()
                {
                    Timeouts= _game.BTeam.Value.Sets[i].TimeOuts.Value,
                    Substitutions = _game.BTeam.Value.Sets[i].Substitutions.Value + _game.BTeam.Value.Sets[i].SubstitutionDetails.Where(x => x.ExceptionalSubstitution == true).Count(),

                    Points = _game.BTeam.Value.Sets[i].Points.Value,
                };

            }

            SetDuration[0] = TimeSpan(_game.History.Histories.Value.Where(x => x.Command1=="WSA" || x.Command1=="WSB").Select(x => x.DateTime).FirstOrDefault()
                , _game.History.Histories.Value.FirstOrDefault(x => x.Command1 == "S1"));

            SetDuration[1] = TimeSpan(_game.History.Histories.Value.Where(x => x.Command1=="WSA" || x.Command1=="WSB").Select(x => x.DateTime).Skip(1).FirstOrDefault()
                , _game.History.Histories.Value.FirstOrDefault(x => x.Command1 == "S2"));

            SetDuration[2] = TimeSpan(_game.History.Histories.Value.Where(x => x.Command1=="WSA" || x.Command1=="WSB").Select(x => x.DateTime).Skip(2).FirstOrDefault()
                , _game.History.Histories.Value.FirstOrDefault(x => x.Command1 == "S3"));

        }
        public static TimeSpan? TimeSpan(DateTime dt1, History? hisotry)
        {
            if (hisotry is null) return null;
            var dt2 = hisotry.DateTime;

            var d1 = new DateTime(dt1.Year, dt1.Month, dt1.Day, dt1.Hour, dt1.Minute, 0);
            var d2 = new DateTime(dt2.Year, dt2.Month, dt2.Day, dt2.Hour, dt2.Minute, 0);

            return d1 - d2;
        }

        public string ATeamName { get; set; }
        public string BTeamName { get; set; }
        public Set[] ASet { get; set; } = new Set[3];
        public Set[] BSet { get; set; } = new Set[3];
        public TimeSpan?[] SetDuration { get; set; } = new TimeSpan?[3];
        public class Set
        {
            public int? Timeouts { get; set; }
            public int? Substitutions { get; set; }
            public int? Win { get; set; }
            public int? Points { get; set; }
        }
    }
}
