using System;
using System.Collections.Generic;
using System.Data;
using Prism.Regions;
using Prism.Services.Dialogs;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using VolleyballScoreSheet.Model;

namespace VolleyballScoreSheet.ViewModels
{
    public class GamingViewModel : BinableBase
    {
        private readonly Game _game;
        public GamingViewModel(Game game)
        {
            _game = game;

            


            ATeam = _game.ToReactivePropertyAsSynchronized(x => x.ATeam.Value);
            BTeam = _game.ToReactivePropertyAsSynchronized(x => x.BTeam.Value);

            ATeam.Value.Points.Subscribe(x =>
            {
                if (_game.isATeamLeft.Value)
                {
                    LeftSidePoints.Value=x;
                }
                else
                {
                    RightSidePoints.Value=x;
                }
            });
            BTeam.Value.Points.Subscribe(x =>
            {
                if (_game.isATeamLeft.Value)
                {
                    RightSidePoints.Value=x;
                }
                else
                {
                    LeftSidePoints.Value=x;
                }
            });

            ATeam.Value.Timeouts.Subscribe(x =>
            {
                if (_game.isATeamLeft.Value)
                    LeftSideTimeOuts.Value=x;
                else
                    RightSideTimeOuts.Value=x;
            });
            BTeam.Value.Timeouts.Subscribe(x =>
            {
                if (_game.isATeamLeft.Value)
                    RightSideTimeOuts.Value=x;
                else
                    LeftSideTimeOuts.Value=x;
            });

            ATeam.Value.Substitutions.Subscribe(x =>
            {
                if (_game.isATeamLeft.Value)
                    LeftSideSubstitutions.Value=x;
                else
                    RightSideSubstitutions.Value=x;
            });
            BTeam.Value.Substitutions.Subscribe(x =>
            {
                if (_game.isATeamLeft.Value)
                    RightSideSubstitutions.Value=x;
                else
                    LeftSideSubstitutions.Value=x;
            });

            ATeam.Value.Name.Subscribe(x =>
            {
                if (_game.isATeamLeft.Value)
                {
                    LeftSideTeamName.Value=x;
                }
                else
                {
                    RightSideTeamName.Value=x;
                }
            });
            BTeam.Value.Name.Subscribe(x =>
            {
                if (_game.isATeamLeft.Value)
                {
                    RightSideTeamName.Value=x;
                }
                else
                {
                    LeftSideTeamName.Value=x;
                }
            });

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


            LeftSidePointAdd.Subscribe(_ => _game.PointAdd(true));
            RightSidePointAdd.Subscribe(_ => _game.PointAdd(false));

            RequestTimeOutCommand.Subscribe(_ => _game.RequestTimeOut());
            UndoCommand.Subscribe(_ => _game.Undo());

            UndoEnable = _game.ToReactivePropertyAsSynchronized(x => x.UndoEnable.Value);
            IsEnablePoint = _game.ToReactivePropertyAsSynchronized(x => x.IsEnablePoint.Value);
            IsEnableTimeout = _game.ToReactivePropertyAsSynchronized(x => x.IsEnableTimeout.Value);

            DebugMessage = _game.ToReactivePropertyAsSynchronized(x => x.Debug.Value);

            _game.isATeamLeft.Subscribe(_ =>
            {
                ATeam.Value.Points.ForceNotify();
                BTeam.Value.Points.ForceNotify();

                ATeam.Value.Timeouts.ForceNotify();
                BTeam.Value.Timeouts.ForceNotify();

                ATeam.Value.Substitutions.ForceNotify();
                BTeam.Value.Substitutions.ForceNotify();

                ATeam.Value.Name.ForceNotify();
                BTeam.Value.Name.ForceNotify();

                ATeam.Value.Color.ForceNotify();
                BTeam.Value.Color.ForceNotify();
            });
        }
        //public ReactiveProperty<DataTable> LeftTeamPlayer { get; set; } = new ReactiveProperty<DataTable>(new DataTable());
        //public ReactiveProperty<DataTable> RightTeamPlayer { get; set; } = new ReactiveProperty<DataTable>(new DataTable());
        //public ReactiveProperty<bool> UndoEnable { get; private set; } = Game.Instance.ObserveProperty(x => x.UndoEnable).ToReactiveProperty();




        //コマンド
        public ReactiveCommand UndoCommand { get; set; } = new();

        public ReactiveProperty<bool> UndoEnable { get; set; }
        public ReactiveProperty<bool> IsEnablePoint { get; set; }
        public ReactiveProperty<bool> IsEnableTimeout { get; set; }


        public ReactiveProperty<Team> ATeam { get; set; }
        public ReactiveProperty<Team> BTeam { get; set; }

        //デバッグ
        public ReactiveProperty<string> DebugMessage { get; private set; }

        //左右情報
        public ReactiveProperty<string> LeftSideTeamName { get; set; } = new();
        public ReactiveProperty<string> RightSideTeamName { get; set; } = new();
        public ReactiveProperty<int> LeftSidePoints { get; set; } = new();
        public ReactiveProperty<int> RightSidePoints { get; set; } = new();
        public ReactiveProperty<string> RightTeamColor { get; set; } = new();
        public ReactiveProperty<string> LeftTeamColor { get; set; } = new();


        public ReactiveCommand LeftSidePointAdd { get; } = new ReactiveCommand();
        public ReactiveCommand RightSidePointAdd { get; } = new ReactiveCommand();


        //タイムアウト
        public ReactiveCommand RequestTimeOutCommand { get; } = new();
        public ReactiveProperty<int> LeftSideTimeOuts { get; } = new();
        public ReactiveProperty<int> RightSideTimeOuts { get; } = new();

        ////サブスティテューション
        //public ReactiveCommand LeftSideSubstitutionCommand { get; } = new();
        //public ReactiveCommand RightSideSubstitutionCommand { get; } = new();
        //public ReactiveProperty<string> LeftSideSubstitutionDisplay { get; } = new("Substitution 0");
        //public ReactiveProperty<string> RightSideSubstitutionDisplay { get; } = new("Substitution 0");
        public ReactiveProperty<int> LeftSideSubstitutions { get; } = new();
        public ReactiveProperty<int> RightSideSubstitutions { get; } = new();



        //public ReactiveProperty<int[]> LeftSideRotation { get; set; } = new();
        //public ReactiveProperty<int[]> RightSideRotation { get; set; } = new();

        //public ReactiveProperty<bool> LeftServe { get; } = new();

    }
}
