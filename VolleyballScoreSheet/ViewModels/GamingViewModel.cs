using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Regions;
using Prism.Services.Dialogs;
using Reactive.Bindings;

namespace VolleyballScoreSheet.ViewModels
{
    public class GamingViewModel : INavigationAware
    {
        public ReactiveProperty<DataTable> LeftTeamPlayer { get; set; } = new ReactiveProperty<DataTable>(new DataTable());
        public ReactiveProperty<DataTable> RightTeamPlayer { get; set; } = new ReactiveProperty<DataTable>(new DataTable());

        public ReactiveProperty<string> SetDisplay { get; set; } = new();
        public ReactiveProperty<string> LeftSideTeamName { get; set; } = new ReactiveProperty<string>();
        public ReactiveProperty<string> RightSideTeamName { get; set; } = new ReactiveProperty<string>();
        public ReactiveProperty<int> LeftSidePoints { get; set; } = new ReactiveProperty<int>();
        public ReactiveProperty<int> RightSidePoints { get; set; } = new ReactiveProperty<int>();
        public ReactiveCommand LeftSidePointAdd { get; } = new ReactiveCommand();
        public ReactiveCommand RightSidePointAdd { get; } = new ReactiveCommand();
        public ReactiveProperty<int> LeftSideSets { get; set; } = new ReactiveProperty<int>();
        public ReactiveProperty<int> RightSideSets { get; set; } = new ReactiveProperty<int>();
        public ReactiveProperty<int> LeftSideTimeOuts { get; } = new ReactiveProperty<int>();
        public ReactiveProperty<int> RightSideTimeOuts { get; } = new ReactiveProperty<int>();
        public ReactiveCommand LeftSideTimeOutCommand { get; } = new ReactiveCommand();
        public ReactiveCommand RightSideTimeOutCommand { get; } = new ReactiveCommand();
        public ReactiveProperty<string> LeftSideTimeOutDispley { get; } = new ReactiveProperty<string>("TimeOut 0");
        public ReactiveProperty<string> RightSideTimeOutDispley { get; } = new ReactiveProperty<string>("TimeOut 0");
        public ReactiveCommand MemberRegistrationCommand { get; } = new ReactiveCommand();
        public ReactiveProperty<int[]> LeftSideLotation { get; set; } = new();
        public ReactiveProperty<int[]> RightSideLotation { get; set; } = new();


        public bool IsLatestPointATeam { get; set; }
        private static int[] Rotation(int[] Team)
        {
            (Team[0], Team[1], Team[2], Team[3], Team[4], Team[5]) = (Team[1], Team[2], Team[3], Team[4], Team[5], Team[0]);
            return Team;
        }
        public void LeftSidePoint()
        {
            LeftSidePoints.Value++;
            if (IsLatestPointATeam == false)
            {
                LeftSideLotation.Value = Rotation(LeftSideLotation.Value);
                LeftSideLotation.ForceNotify();
            }
            if (LeftSidePoints.Value >= 25 && LeftSidePoints.Value-RightSidePoints.Value>=2)
            {
                //ゲームセット
                LeftSideSets.Value++;
                LeftSidePoints.Value = 0;
                RightSidePoints.Value = 0;
                if (_game.GetCurrentSet().ATeamRightSide)
                {
                    //BTeam
                    _game.BTeamSet++;
                }
                else
                {
                    //ATeam
                    _game.ATeamSet++;
                }
                Navigate("BeforeMatch");
            }
            IsLatestPointATeam = true;
        }
        public void RightSidePoint()
        {
            RightSidePoints.Value++;
            if (IsLatestPointATeam == true)
            {
                RightSideLotation.Value = Rotation(RightSideLotation.Value);
                RightSideLotation.ForceNotify();
            }
            if (RightSidePoints.Value >= 25 && RightSidePoints.Value-LeftSidePoints.Value>=2)
            {
                //ゲームセット
                RightSideSets.Value++;
                LeftSidePoints.Value = 0;
                RightSidePoints.Value = 0;
                if (_game.GetCurrentSet().ATeamRightSide)
                {
                    //ATeam
                    _game.ATeamSet++;
                }
                else
                {
                    //BTeam
                    _game.BTeamSet++;
                }
                Navigate("BeforeMatch");
            }
            IsLatestPointATeam = false;
        }
        public void LeftSideTimeOut()
        {
            LeftSideTimeOuts.Value++;
            LeftSideTimeOutDispley.Value = $"TimeOut {2 -LeftSideTimeOuts.Value}";
        }
        public void RightSideTimeOut()
        {
            RightSideTimeOuts.Value++;
            RightSideTimeOutDispley.Value = $"TimeOut {2 -RightSideTimeOuts.Value}";
        }

        private readonly IRegionManager _regionManager;
        private readonly Game _game;
        private readonly IDialogService _dialogService;
        public GamingViewModel(IRegionManager regionManager, Game game, IDialogService dialogService)
        {
            _regionManager = regionManager;
            _game = game;
            _dialogService = dialogService;
            LeftSidePointAdd.Subscribe(_ => LeftSidePoint());
            RightSidePointAdd.Subscribe(_ => RightSidePoint());
            LeftSideTimeOutCommand.Subscribe(_ => LeftSideTimeOut());
            RightSideTimeOutCommand.Subscribe(_ => RightSideTimeOut());
            MemberRegistrationCommand.Subscribe(_ => Navigate("RosterA"));

            SetDisplay.Value = $"SET {_game.Sets.Count}";

            LeftSideSets.Value = _game.ATeamSet;
            RightSideSets.Value = _game.BTeamSet;
            LeftSideTeamName.Value = _game.ATeam;
            RightSideTeamName.Value = _game.BTeam;

            LeftTeamPlayer.Value.Clear();
            LeftTeamPlayer.Value.Columns.Add("Number");
            LeftTeamPlayer.Value.Columns.Add("Name");

            foreach (var item in _game.ATeamPlayers)
            {
                var row1 = LeftTeamPlayer.Value.NewRow();
                row1[0] = item.Id;
                row1[1] = item.Name;
                LeftTeamPlayer.Value.Rows.Add(row1);
            }

            RightTeamPlayer.Value.Clear();
            RightTeamPlayer.Value.Columns.Add("Number");
            RightTeamPlayer.Value.Columns.Add("Name");
            foreach (var item in _game.BTeamPlayers)
            {
                var row2 = RightTeamPlayer.Value.NewRow();
                row2[0] = item.Id;
                row2[1] = item.Name;
                RightTeamPlayer.Value.Rows.Add(row2);
            }


            if (_game.Sets[^1].ATeamRightSide)
            {
                RightSideLotation.Value = _game.Sets[^1].ATeamRotation;
                LeftSideLotation.Value = _game.Sets[^1].BTeamRotation;

                LeftSideTeamName.Value=_game.BTeam;
                RightSideTeamName.Value=_game.ATeam;
            }
            else
            {
                RightSideLotation.Value = _game.Sets[^1].BTeamRotation;
                LeftSideLotation.Value = _game.Sets[^1].ATeamRotation;

                LeftSideTeamName.Value=_game.ATeam;
                RightSideTeamName.Value=_game.BTeam;
            }

            if (_game.Sets[^1].ATeamServer)
            {
                IsLatestPointATeam = true;
            }
            else
            {
                IsLatestPointATeam = false;
            }
        }
        private void Navigate(string navigatePath)
        {
            if (navigatePath != null)
                _regionManager.RequestNavigate("ContentRegion", navigatePath);
        }

        public void OnNavigatedTo(NavigationContext navigationContext) { }
        public bool IsNavigationTarget(NavigationContext navigationContext) => false;
        public void OnNavigatedFrom(NavigationContext navigationContext) { }
    }
}
