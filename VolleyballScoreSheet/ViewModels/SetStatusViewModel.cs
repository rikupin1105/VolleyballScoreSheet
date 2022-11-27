using Prism.Commands;
using Prism.Mvvm;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

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

            EndText = _game.ToReactivePropertyAsSynchronized(x => x.EndButtonText.Value);
            EndCommand.Subscribe(_ => _game.EndSet());
        }
        public ReactiveProperty<string> EndText { get; set; } = new();
        public ReactiveCommand EndCommand { get; set; } = new();

        public ReactiveProperty<string> ATeamName { get; set; }
        public ReactiveProperty<string> BTeamName { get; set; }
        public ReactiveProperty<int> ATeamSets { get; set; } 
        public ReactiveProperty<int> BTeamSets { get; set; } 

        //public ReactiveProperty<string[]> ATeamPoint()
        //{
        //    var array = new string[_game.Rule.SetCount];
        //    for (int i = 0; i < array.Length; i++)
        //    {
        //        if (_game.Sets[i]==null)
        //        {
        //            array[i] = "-";
        //        }
        //        else
        //        {
        //            array[i] = _game.ATeam.Sets[i].Point.Value.ToString();
        //        }
        //    }
        //    return new ReactiveProperty<string[]>(array);
        //}
    }
}