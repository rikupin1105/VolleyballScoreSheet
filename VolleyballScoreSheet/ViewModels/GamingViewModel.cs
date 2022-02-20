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
        public ReactiveProperty<int[]> LeftSideLotation { get; set; } = new();
        public ReactiveProperty<int[]> RightSideLotation { get; set; } = new();
        public ReactiveProperty<int> LeftServeBallOpacity { get; } = new(0);
        public ReactiveProperty<int> RightServeBallOpacity { get; } = new(0);
        public ReactiveCommand LeftSideSubstitutionCommand { get; } = new();
        public ReactiveCommand RightSideSubstitutionCommand { get; } = new();
        public ReactiveProperty<string> LeftSideSubstitutionDisplay { get; } = new("Substitution 0");
        public ReactiveProperty<string> RightSideSubstitutionDisplay { get; } = new("Substitution 0");
        public ReactiveProperty<int> LeftSideSubstitutions { get; } = new();
        public ReactiveProperty<int> RightSideSubstitutions { get; } = new();


        public bool IsLatestPointLeftTeam { get; set; }
        private static int[] Rotation(int[] Team)
        {
            (Team[0], Team[1], Team[2], Team[3], Team[4], Team[5]) = (Team[1], Team[2], Team[3], Team[4], Team[5], Team[0]);
            return Team;
        }
        public void LeftSidePoint()
        {
            LeftSidePoints.Value++;
            if (IsLatestPointLeftTeam == false)
            {
                LeftSideLotation.Value = Rotation(LeftSideLotation.Value);
                LeftSideLotation.ForceNotify();
            }

            if (_game.Sets.Count == _game.SetCount)
            {
                //ファイナルセットの場合
                if (LeftSidePoints.Value >= _game.FinalSetToWinPoint && RightSidePoints.Value-LeftSidePoints.Value>=2)
                {
                    //ゲーム終わり
                    LeftSideSets.Value++;
                }
                if (LeftSidePoints.Value == _game.FinalSetCoutChangePoint && LeftSidePoints.Value > RightSidePoints.Value)
                {
                    //ファイナルセットのコートチェンジ
                    (LeftSidePoints.Value, RightSidePoints.Value)
                        =(RightSidePoints.Value, LeftSidePoints.Value);

                    (LeftSideSets.Value, RightSideSets.Value)
                        =(RightSideSets.Value, LeftSideSets.Value);

                    (LeftSideTeamName.Value, RightSideTeamName.Value) =
                        (RightSideTeamName.Value, LeftSideTeamName.Value);

                    (LeftTeamPlayer.Value, RightTeamPlayer.Value) =
                         (RightTeamPlayer.Value, LeftTeamPlayer.Value);

                    LeftServeBallOpacity.Value = 0;
                    RightServeBallOpacity.Value = 100;
                    IsLatestPointLeftTeam = false;
                }
            }
            else if (LeftSidePoints.Value >= _game.ToWinPoint && LeftSidePoints.Value - RightSidePoints.Value >= 2)
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

                var set = new Set();
                set.ATeamServer = !_game.GetCurrentSet().ATeamServer;
                if (_game.CourtChangeEnable)
                {
                    //コートチェンジを行う
                    set.ATeamRightSide = !_game.GetCurrentSet().ATeamRightSide;
                }
                _game.CreateSet(set);


                Navigate("BeforeMatch");
            }
            else
            {
                LeftServeBallOpacity.Value = 100;
                RightServeBallOpacity.Value = 0;
                IsLatestPointLeftTeam = true;
            }
        }
        public void RightSidePoint()
        {
            RightSidePoints.Value++;
            if (IsLatestPointLeftTeam == true)
            {
                RightSideLotation.Value = Rotation(RightSideLotation.Value);
                RightSideLotation.ForceNotify();
            }

            if (_game.Sets.Count == _game.SetCount)
            {
                //ファイナルセットの場合
                if (RightSidePoints.Value >= _game.FinalSetToWinPoint && RightSidePoints.Value-LeftSidePoints.Value>=2)
                {
                    //ゲーム終わり
                    RightSideSets.Value++;
                }
                if (RightSidePoints.Value == _game.FinalSetCoutChangePoint && RightSidePoints.Value > LeftSidePoints.Value)
                {
                    //ファイナルセットのコートチェンジ
                    (LeftSidePoints.Value, RightSidePoints.Value)
                        =(RightSidePoints.Value, LeftSidePoints.Value);

                    (LeftSideSets.Value, RightSideSets.Value)
                        =(RightSideSets.Value, LeftSideSets.Value);

                    (LeftSideTeamName.Value, RightSideTeamName.Value) =
                        (RightSideTeamName.Value, LeftSideTeamName.Value);

                    (LeftTeamPlayer.Value, RightTeamPlayer.Value) =
                         (RightTeamPlayer.Value, LeftTeamPlayer.Value);

                    LeftServeBallOpacity.Value = 100;
                    RightServeBallOpacity.Value = 0;
                    IsLatestPointLeftTeam = true;
                }
            }
            else if (RightSidePoints.Value >= _game.ToWinPoint && RightSidePoints.Value-LeftSidePoints.Value>=2)
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

                var set = new Set();
                set.ATeamServer = !_game.GetCurrentSet().ATeamServer;
                if (_game.CourtChangeEnable)
                {
                    //コートチェンジを行う
                    set.ATeamRightSide = !_game.GetCurrentSet().ATeamRightSide;
                }
                _game.CreateSet(set);


                Navigate("BeforeMatch");
            }
            else
            {
                LeftServeBallOpacity.Value = 0;
                RightServeBallOpacity.Value = 100;
                IsLatestPointLeftTeam = false;
            }

        }
        public void LeftSideTimeOut()
        {
            LeftSideTimeOuts.Value++;
            LeftSideTimeOutDispley.Value = $"TimeOut {LeftSideTimeOuts.Value}";
        }
        public void RightSideTimeOut()
        {
            RightSideTimeOuts.Value++;
            RightSideTimeOutDispley.Value = $"TimeOut {RightSideTimeOuts.Value}";
        }
        public void LeftSideSubstitution()
        {
            if (LeftSideSubstitutions.Value >= 6)
            {
                _dialogService.ShowDialog("ExceptionalSubstitution", new DialogParameters(), (result) =>
                {
                    if (result.Result == ButtonResult.No)
                    {
                        //不当な要求
                        return;
                    }
                    if (result.Result==ButtonResult.Cancel)
                    {
                        //閉じる
                        return;
                    }
                    if (result.Result == ButtonResult.Retry)
                    {
                        //例外的な選手交代
                        _dialogService.ShowDialog("Substitution", new DialogParameters
                        {
                            {
                                "Team",LeftSideTeamName.Value
                            }
                        },
                       (result) =>
                       {
                           if (result.Result == ButtonResult.Cancel)
                           {

                           }
                           else
                           {
                               result.Parameters.TryGetValue("Out", out int outMember);
                               result.Parameters.TryGetValue("In", out int inMember);

                               LeftSideLotation.Value[Array.IndexOf(LeftSideLotation.Value, outMember)] = inMember;
                               LeftSideLotation.ForceNotify();

                               LeftSideSubstitutions.Value++;
                               LeftSideSubstitutionDisplay.Value = $"Substitution {LeftSideSubstitutions.Value}";
                           }
                       }, "AlertWindow");
                    }
                }, "AlertWindow");
            }
            else
            {
                _dialogService.ShowDialog("Substitution", new DialogParameters
                {
                    {
                        "Team",LeftSideTeamName.Value
                    }
                },
                (result) =>
                {
                    if (result.Result == ButtonResult.Cancel)
                    {

                    }
                    else
                    {
                        result.Parameters.TryGetValue("Out", out int outMember);
                        result.Parameters.TryGetValue("In", out int inMember);

                        LeftSideLotation.Value[Array.IndexOf(LeftSideLotation.Value, outMember)] = inMember;
                        LeftSideLotation.ForceNotify();

                        LeftSideSubstitutions.Value++;
                        LeftSideSubstitutionDisplay.Value = $"Substitution {LeftSideSubstitutions.Value}";
                    }
                }, "AlertWindow");
            }
        }
        public void RightSideSubstitution()
        {
            if (RightSideSubstitutions.Value >= 6)
            {
                _dialogService.ShowDialog("ExceptionalSubstitution", new DialogParameters(), (result) =>
                {
                    if (result.Result == ButtonResult.No)
                    {
                        //不当な要求
                        return;
                    }
                    if (result.Result==ButtonResult.Cancel)
                    {
                        //閉じる
                        return;
                    }
                    if (result.Result == ButtonResult.Retry)
                    {
                        //例外的な選手交代
                        _dialogService.ShowDialog("Substitution", new DialogParameters
                        {
                            {
                                "Team",RightSideTeamName.Value
                            }
                        },
                       (result) =>
                       {
                           if (result.Result == ButtonResult.Cancel)
                           {

                           }
                           else
                           {
                               result.Parameters.TryGetValue("Out", out int outMember);
                               result.Parameters.TryGetValue("In", out int inMember);

                               RightSideLotation.Value[Array.IndexOf(RightSideLotation.Value, outMember)] = inMember;
                               RightSideLotation.ForceNotify();

                               RightSideSubstitutions.Value++;
                               RightSideSubstitutionDisplay.Value = $"Substitution {RightSideSubstitutions.Value}";
                           }
                       }, "AlertWindow");
                    }
                }, "AlertWindow");
            }
            else
            {
                _dialogService.ShowDialog("Substitution", new DialogParameters
                {
                    {
                        "Team",LeftSideTeamName.Value
                    }
                },
                (result) =>
                {
                    if (result.Result == ButtonResult.Cancel)
                    {

                    }
                    else
                    {
                        result.Parameters.TryGetValue("Out", out int outMember);
                        result.Parameters.TryGetValue("In", out int inMember);

                        LeftSideLotation.Value[Array.IndexOf(LeftSideLotation.Value, outMember)] = inMember;
                        LeftSideLotation.ForceNotify();

                        LeftSideSubstitutions.Value++;
                        LeftSideSubstitutionDisplay.Value = $"Substitution {LeftSideSubstitutions.Value}";
                    }
                }, "AlertWindow");
            }
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
            LeftSideSubstitutionCommand.Subscribe(_ => LeftSideSubstitution());
            RightSideSubstitutionCommand.Subscribe(_ => RightSideSubstitution());


            SetDisplay.Value = $"SET {_game.Sets.Count}";

            LeftTeamPlayer.Value.Clear();
            LeftTeamPlayer.Value.Columns.Add("Number");
            LeftTeamPlayer.Value.Columns.Add("Name");

            RightTeamPlayer.Value.Clear();
            RightTeamPlayer.Value.Columns.Add("Number");
            RightTeamPlayer.Value.Columns.Add("Name");

            if (_game.Sets[^1].ATeamRightSide)
            {
                RightSideLotation.Value = _game.Sets[^1].ATeamRotation;
                LeftSideLotation.Value = _game.Sets[^1].BTeamRotation;

                LeftSideTeamName.Value = _game.BTeam;
                RightSideTeamName.Value = _game.ATeam;

                RightSideSets.Value = _game.ATeamSet;
                LeftSideSets.Value = _game.BTeamSet;

                if (_game.Sets[^1].ATeamServer)
                {
                    RightServeBallOpacity.Value = 100;
                    LeftServeBallOpacity.Value = 0;
                    IsLatestPointLeftTeam = false;
                }
                else
                {
                    RightServeBallOpacity.Value = 0;
                    LeftServeBallOpacity.Value = 100;
                    IsLatestPointLeftTeam = true;
                }

                foreach (var item in _game.ATeamPlayers)
                {
                    var row1 = RightTeamPlayer.Value.NewRow();
                    row1[0] = item.Id;
                    row1[1] = item.Name;
                    RightTeamPlayer.Value.Rows.Add(row1);
                }
                foreach (var item in _game.BTeamPlayers)
                {
                    var row2 = LeftTeamPlayer.Value.NewRow();
                    row2[0] = item.Id;
                    row2[1] = item.Name;
                    LeftTeamPlayer.Value.Rows.Add(row2);
                }
            }
            else
            {
                RightSideLotation.Value = _game.Sets[^1].BTeamRotation;
                LeftSideLotation.Value = _game.Sets[^1].ATeamRotation;

                LeftSideTeamName.Value=_game.ATeam;
                RightSideTeamName.Value=_game.BTeam;

                LeftSideSets.Value = _game.ATeamSet;
                RightSideSets.Value = _game.BTeamSet;

                if (_game.Sets[^1].ATeamServer)
                {
                    RightServeBallOpacity.Value = 0;
                    LeftServeBallOpacity.Value = 100;
                    IsLatestPointLeftTeam = true;
                }
                else
                {
                    RightServeBallOpacity.Value = 100;
                    LeftServeBallOpacity.Value = 0;
                    IsLatestPointLeftTeam = false;
                }

                foreach (var item in _game.ATeamPlayers)
                {
                    var row1 = LeftTeamPlayer.Value.NewRow();
                    row1[0] = item.Id;
                    row1[1] = item.Name;
                    LeftTeamPlayer.Value.Rows.Add(row1);
                }
                foreach (var item in _game.BTeamPlayers)
                {
                    var row2 = RightTeamPlayer.Value.NewRow();
                    row2[0] = item.Id;
                    row2[1] = item.Name;
                    RightTeamPlayer.Value.Rows.Add(row2);
                }
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
