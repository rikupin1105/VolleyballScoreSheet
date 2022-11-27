using Prism.Commands;
using Prism.Mvvm;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace VolleyballScoreSheet.ViewModels
{
    public class RequestTimeOutViewModel : BindableBase
    {
        private readonly Game _game;
        public RequestTimeOutViewModel(Game game)
        {
            _game = game;

            LeftTeamName = _game.ToReactivePropertyAsSynchronized(x => x.LeftTeam.Name.Value);
            RightTeamName = _game.ToReactivePropertyAsSynchronized(x => x.RightTeam.Name.Value);

            RightTeamColor = _game.ToReactivePropertyAsSynchronized(x => x.RightTeam.Color.Value);
            LeftTeamColor = _game.ToReactivePropertyAsSynchronized(x => x.LeftTeam.Color.Value);

            DisplayRequestTimeOut = _game.ToReactivePropertyAsSynchronized(x => x.DisplayRequestTimeOut.Value);
        }
        //表示
        public ReactiveProperty<bool> DisplayRequestTimeOut { get; set; }

        public ReactiveProperty<string> LeftTeamName { get; set; } 
        public ReactiveProperty<string> RightTeamName { get; set; } 
        public ReactiveProperty<string> LeftTeamColor { get; set; } 
        public ReactiveProperty<string> RightTeamColor { get; set; }

    }
}
