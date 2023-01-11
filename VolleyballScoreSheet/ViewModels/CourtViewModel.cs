using Prism.Commands;
using Prism.Mvvm;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;

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

            ATeam.Value.Color.Subscribe(x =>
            {
                if (_game.isATeamLeft.Value)
                {
                    LeftTeamColor.Value=x;
                }
                else
                {
                    RightTeamColor.Value=x;
                }
            });
            BTeam.Value.Color.Subscribe(x =>
            {
                if (_game.isATeamLeft.Value)
                {
                    RightTeamColor.Value=x;
                }
                else
                {
                    LeftTeamColor.Value=x;
                }
            });

            ATeam.Value.Rotation.Subscribe(x =>
            {
                if (_game.isATeamLeft.Value)
                {
                    LeftSideRotation.Value = x;
                    LeftSideRotation.ForceNotify();
                }
                else
                {
                    RightSideRotation.Value = x;
                    RightSideRotation.ForceNotify();
                }
            });
            BTeam.Value.Rotation.Subscribe(x =>
            {
                if (_game.isATeamLeft.Value)
                {
                    RightSideRotation.Value = x;
                    RightSideRotation.ForceNotify();
                }
                else
                {
                    LeftSideRotation.Value = x;
                    LeftSideRotation.ForceNotify();
                }
            });

            _game.isATeamLeft.Subscribe(_ =>
            {
                ATeam.Value.Rotation.ForceNotify();
                BTeam.Value.Rotation.ForceNotify();

                ATeam.Value.Color.ForceNotify();
                BTeam.Value.Color.ForceNotify();
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
