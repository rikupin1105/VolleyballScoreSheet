using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
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

            _game.ATeam.Subscribe(x =>
            {
                foreach (var item in x.Sets)
                {
                    ASet.Add(new()
                    {
                        Timeouts= item.TimeOuts.Value,
                        Substitutions = item.Substitutions.Value,

                        Points = item.Points.Value,
                    });
                }
            });

            foreach (var item in _game.BTeam.Value.Sets)
            {
                BSet.Add(new()
                {
                    Timeouts= item.TimeOuts.Value,
                    Substitutions = item.Substitutions.Value,

                    Points = item.Points.Value,
                });
            }
        }

        public string ATeamName { get; set; }
        public string BTeamName { get; set; }
        public List<Set> ASet { get; set; } = new();
        public List<Set> BSet { get; set; } = new();
        public class Set
        {
            public int Timeouts { get; set; }
            public int Substitutions { get; set; }
            public int Win { get; set; }
            public int Points { get; set; }
        }
    }
}
