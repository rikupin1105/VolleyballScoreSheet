using System;
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

            LeftSideTeamName = _game.ToReactivePropertyAsSynchronized(x => x.LeftTeam.Name.Value);
            RightSideTeamName = _game.ToReactivePropertyAsSynchronized(x => x.RightTeam.Name.Value);

            ATeam = _game.ToReactivePropertyAsSynchronized(x => x.ATeam.Value);
            BTeam = _game.ToReactivePropertyAsSynchronized(x => x.BTeam.Value);

            ATeam.Value.Sets[^1].Point.Subscribe(x =>
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
            BTeam.Value.Sets[^1].Point.Subscribe(x =>
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

            ATeam.Value.Sets[^1].TimeOut.Subscribe(x =>
            {
                if (_game.isATeamLeft.Value)
                    LeftSideTimeOuts.Value=x;
                else
                    RightSideTimeOuts.Value=x;
            });
            BTeam.Value.Sets[^1].TimeOut.Subscribe(x =>
            {
                if (_game.isATeamLeft.Value)
                    RightSideTimeOuts.Value=x;
                else
                    LeftSideTimeOuts.Value=x;
            });

            ATeam.Value.Sets[^1].Substitution.Subscribe(x =>
            {
                if (_game.isATeamLeft.Value)
                    LeftSideSubstitutions.Value=x;
                else
                    RightSideSubstitutions.Value=x;
            });
            BTeam.Value.Sets[^1].Substitution.Subscribe(x =>
            {
                if (_game.isATeamLeft.Value)
                    RightSideSubstitutions.Value=x;
                else
                    LeftSideSubstitutions.Value=x;
            });


            ATeam.Value.Sets[^1].Point.Subscribe(x => { LeftSidePoints.Value=x; });
            //RightSidePoints = _game.ToReactivePropertyAsSynchronized(x => x.GetCurrentSet().Value.RightPoint.Value);



            LeftSidePointAdd.Subscribe(_ => _game.PointAdd(true));
            RightSidePointAdd.Subscribe(_ => _game.PointAdd(false));

            RequestTimeOutCommand.Subscribe(_ => _game.RequestTimeOut());
            UndoCommand.Subscribe(_ => _game.Undo());
            
            RightTeamColor = _game.ToReactivePropertyAsSynchronized(x => x.RightTeam.Color.Value);
            LeftTeamColor = _game.ToReactivePropertyAsSynchronized(x => x.LeftTeam.Color.Value); ;
        }
        //public ReactiveProperty<DataTable> LeftTeamPlayer { get; set; } = new ReactiveProperty<DataTable>(new DataTable());
        //public ReactiveProperty<DataTable> RightTeamPlayer { get; set; } = new ReactiveProperty<DataTable>(new DataTable());
        //public ReactiveProperty<bool> UndoEnable { get; private set; } = Game.Instance.ObserveProperty(x => x.UndoEnable).ToReactiveProperty();

        


        //コマンド
        public ReactiveCommand UndoCommand { get; set; } = new();
        
        public ReactiveProperty<Team> ATeam { get; set; }
        public ReactiveProperty<Team> BTeam { get; set; }

        ////デバッグ
        public ReactiveProperty<string> DebugMessage { get; private set; }

        ////左右情報
        public ReactiveProperty<string> LeftSideTeamName { get; set; }
        public ReactiveProperty<string> RightSideTeamName { get; set; }
        public ReactiveProperty<int> LeftSidePoints { get; set; } = new();
        public ReactiveProperty<int> RightSidePoints { get; set; } = new();
        public ReactiveProperty<string> RightTeamColor { get; set; }
        public ReactiveProperty<string> LeftTeamColor { get; set; }


        public ReactiveCommand LeftSidePointAdd { get; } = new ReactiveCommand();
        public ReactiveCommand RightSidePointAdd { get; } = new ReactiveCommand();


        //タイムアウト
        public ReactiveCommand RequestTimeOutCommand { get; } = new();
        public ReactiveProperty<int> LeftSideTimeOuts { get; } = new();
        public ReactiveProperty<int> RightSideTimeOuts { get; } = new();
        //public ReactiveCommand LeftSideTimeOutCommand { get; } = new ReactiveCommand();
        //public ReactiveCommand RightSideTimeOutCommand { get; } = new ReactiveCommand();

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
