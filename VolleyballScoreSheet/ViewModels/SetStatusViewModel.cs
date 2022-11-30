using Prism.Commands;
using Prism.Mvvm;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using VolleyballScoreSheet.Model;

namespace VolleyballScoreSheet.ViewModels
{
    public class SetStatusViewModel : BindableBase
    {
        Game _game;
        public SetStatusViewModel(Game game)
        {
            _game=game;

            ATeamName = _game.ToReactivePropertyAsSynchronized(x => x.ATeam.Value.Name.Value);
            BTeamName = _game.ToReactivePropertyAsSynchronized(x => x.BTeam.Value.Name.Value);
                                                                           
            ATeamSets = _game.ToReactivePropertyAsSynchronized(x => x.ATeam.Value.WinSets.Value);
            BTeamSets = _game.ToReactivePropertyAsSynchronized(x => x.BTeam.Value.WinSets.Value);

            ATeam = _game.ToReactivePropertyAsSynchronized(x => x.ATeam.Value);
            BTeam = _game.ToReactivePropertyAsSynchronized(x => x.BTeam.Value);

            ATeamPoints = new(new int[_game.Rule.SetCount]);
            BTeamPoints = new(new int[_game.Rule.SetCount]);

            EndText = _game.ToReactivePropertyAsSynchronized(x => x.EndButtonText.Value);
            EndCommand.Subscribe(_ => _game.EndSet());

            DataTime = Observable.Interval(TimeSpan.FromSeconds(1)).Select(x=>DateTime.Now.ToString("HH:mm")).ToReactiveProperty()!;
            SetCount = _game.ToReactivePropertyAsSynchronized(x => x.Set.Value);
        }
        public ReactiveProperty<string> DataTime { get; set; } = new(DateTime.Now.ToString("HH:mm"));
        public ReactiveProperty<string> EndText { get; set; } = new();
        public ReactiveCommand EndCommand { get; set; } = new();

        public ReactiveProperty<int> SetCount { get; set; }
        public ReactiveProperty<string> ATeamName { get; set; }
        public ReactiveProperty<string> BTeamName { get; set; }
        public ReactiveProperty<int> ATeamSets { get; set; } = new();
        public ReactiveProperty<int> BTeamSets { get; set; } = new();
        public ReactiveProperty<int[]> ATeamPoints { get; set; } 
        public ReactiveProperty<int[]> BTeamPoints { get; set; }
        public ReactiveProperty<Team> ATeam { get; set; } 
        public ReactiveProperty<Team> BTeam { get; set; } 
    }
}