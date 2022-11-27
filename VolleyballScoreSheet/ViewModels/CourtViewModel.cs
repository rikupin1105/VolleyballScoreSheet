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
    public class CourtViewModel : BindableBase
    {
        readonly Game _game;
        public CourtViewModel(Game game)
        {
            _game=game;

            ATeam = _game.ToReactivePropertyAsSynchronized(x => x.ATeam.Value);
            BTeam = _game.ToReactivePropertyAsSynchronized(x => x.BTeam.Value);

            LeftTeamColor = _game.ToReactivePropertyAsSynchronized(x=>x.LeftTeam.Color.Value);
            RightTeamColor = _game.ToReactivePropertyAsSynchronized(x=>x.RightTeam.Color.Value);

            ATeam.Value.Sets[^1].Rotation.Subscribe(x=> 
            {
                LeftSideRotation.Value = x;
                LeftSideRotation.ForceNotify();
            });

            BTeam.Value.Sets[^1].Rotation.Subscribe(x =>
            {
                RightSideRotation.Value = x;
                RightSideRotation.ForceNotify();
            });

            isLeftServe = _game.ToReactivePropertyAsSynchronized(x => x.isLeftServe.Value);

            //表示
            DisplayRotation = _game.ToReactivePropertyAsSynchronized(x => x.DisplayRotation.Value);
        }
        //表示
        public ReactiveProperty<bool> DisplayRotation { get; set; }

        public ReactiveProperty<Team> ATeam { get; set; }
        public ReactiveProperty<Team> BTeam { get; set; }

        public static ReactiveProperty<bool> isLeftServe { get; set; }
        public ReactiveProperty<string> LeftTeamColor { get; set; } = new();
        public ReactiveProperty<string> RightTeamColor { get; set; } = new();
        public static ReactiveProperty<int[]> LeftSideRotation { get; set; } = new();
        public static ReactiveProperty<int[]> RightSideRotation { get; set; } = new();
    }
}
