﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using Prism.Regions;
using Prism.Services.Dialogs;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using Unity.Policy;
using VolleyballScoreSheet.ViewModels.Card;

namespace VolleyballScoreSheet.ViewModels
{
    public class GamingViewModel : BinableBase
    {
        private readonly Game _game;
        private readonly IDialogService _dialogService;
        private Substitution _substitution;
        public GamingViewModel(Game game, IDialogService dialogService)
        {
            _game = game;
            _dialogService = dialogService;
            _substitution = new Substitution(game);

            ATeam = _game.ToReactivePropertyAsSynchronized(x => x.ATeam.Value);
            BTeam = _game.ToReactivePropertyAsSynchronized(x => x.BTeam.Value);

            ScoresheetCommand.Subscribe(_ =>
            {
                new Views.ScoreSheetWindow().Show();
            });

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

            ATeam.Value.StartingLineUp.Subscribe(x =>
            {
                if (_game.isATeamLeft.Value)
                {
                    LeftTeamStartingLineUp.Value=x;
                }
                else
                {
                    RightTeamStartingLineUp.Value=x;
                }
            });
            BTeam.Value.StartingLineUp.Subscribe(x =>
            {
                if (_game.isATeamLeft.Value)
                {
                    RightTeamStartingLineUp.Value=x;
                }
                else
                {
                    LeftTeamStartingLineUp.Value=x;
                }
            });

            ATeam.Value.Substitutioned.Subscribe(x =>
            {
                if (_game.isATeamLeft.Value)
                {
                    LeftTeamSubstitutioned.Value=x;
                    LeftTeamSubstitutioned.ForceNotify();
                }
                else
                {
                    RightTeamSubstitutioned.Value=x;
                    RightTeamSubstitutioned.ForceNotify();
                }
            });
            BTeam.Value.Substitutioned.Subscribe(x =>
            {
                if (_game.isATeamLeft.Value)
                {
                    RightTeamSubstitutioned.Value=x;
                    RightTeamSubstitutioned.ForceNotify();
                }
                else
                {
                    LeftTeamSubstitutioned.Value=x;
                    LeftTeamSubstitutioned.ForceNotify();
                }
            });

            ATeam.Value.isReturn.Subscribe(x =>
            {
                if (_game.isATeamLeft.Value)
                {
                    LeftTeamIsReturn.Value=x;
                    LeftTeamIsReturn.ForceNotify();
                }
                else
                {
                    RightTeamIsReturn.Value=x;
                    RightTeamIsReturn.ForceNotify();
                }
            });
            BTeam.Value.isReturn.Subscribe(x =>
            {
                if (_game.isATeamLeft.Value)
                {
                    RightTeamIsReturn.Value=x;
                    RightTeamIsReturn.ForceNotify();
                }
                else
                {
                    LeftTeamIsReturn.Value=x;
                    LeftTeamIsReturn.ForceNotify();
                }
            });

            LeftSidePointAdd.Subscribe(_ => _game.PointAdd(true));
            RightSidePointAdd.Subscribe(_ => _game.PointAdd(false));

            RequestTimeOutCommand.Subscribe(_ => _game.RequestTimeOut());
            UndoCommand.Subscribe(_ => _game.Undo());

            UndoEnable = _game.ToReactivePropertyAsSynchronized(x => x.UndoEnable.Value);
            IsEnablePoint = _game.ToReactivePropertyAsSynchronized(x => x.IsEnablePoint.Value);
            IsEnableTimeout = _game.ToReactivePropertyAsSynchronized(x => x.IsEnableTimeout.Value);
            IsEnableSubstitution = _game.ToReactivePropertyAsSynchronized(x => x.IsEnableSubstitution.Value);

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

                ATeam.Value.StartingLineUp.ForceNotify();
                BTeam.Value.StartingLineUp.ForceNotify();
            });

            _game.FinalSetCourtChangeNotifyCommand.Subscribe(_ =>
            {
                _dialogService.ShowDialog("NotificationDialog", new DialogParameters
                {
                    {"Title","通知" },
                    { "Message",$"コートチェンジを行ってください。"},
                    {"ButtonText","OK" }
                }, res =>
                {

                }, "AlertWindow");
            });

            LeftSubstitutionCommand.Subscribe(_ =>
            {
                _substitution.LegallySubstitution(true);
            });

            RightSubstitutionCommand.Subscribe(_ =>
            {
                _substitution.LegallySubstitution(false);
            });

            _substitution.SubstitutionCommand.Subscribe(isLeft =>
            {
                _dialogService.ShowDialog("Substitution", new DialogParameters
                {
                    {"Side",isLeft}
                }, res => { });
            });

            

            CardCommand.Subscribe(_ =>
            {
                _dialogService.ShowDialog("Card", new DialogParameters
                {

                }, res =>
                {

                }, "AlertWindow");
            });

            ExceptionalSubstitutionCommand.Subscribe(_ =>
            {
                bool isA = false;
                Team team = new Team("", "");
                Substitution sub = new(_game);
                Player outPlayer = new(0, "");

                _dialogService.ShowDialog("SelectTeam", new DialogParameters
                {
                    { "ExceptionalSubstitution", true}
                }, res =>
                {
                    res.Parameters.TryGetValue("isA", out bool IsA);

                    isA = IsA;
                    if (isA)
                    {
                        team = _game.ATeam.Value;
                    }
                    else
                    {
                        team =_game.BTeam.Value;
                    }

                }, "AlertWindow");

                _dialogService.ShowDialog("SelectPlayerAndStaff", new DialogParameters
                {
                    {"Message","交代する選手を選択してください。" },
                    {"isA" , isA },
                    {"SelectEnum", SelectEnum.OnCourtPlayer }
                }, res =>
                {
                    res.Parameters.TryGetValue("Mark", out string mark);

                    outPlayer = team.Players.First(x => x.Id == int.Parse(mark));

                    //sub.ExceptionalSubstitution(isA, outMember);

                }, "AlertWindow");

                if (sub.CanSubstitutedLegally(isA, outPlayer))
                {
                    _dialogService.ShowDialog("NotificationDialog", new DialogParameters()
                    {
                        {"Title","注意" },
                        { "Message",$"正規の選手交代が可能です。\n通常の選手交代を行ってください。"},
                        {"ButtonText","OK" }
                    }, res =>
                    {
                        if (res.Result == ButtonResult.OK)
                        {
                            //選手交代を表示
                            _dialogService.ShowDialog("Substitution", new DialogParameters
                            {
                                {"isA", isA},
                                {"OutPlayer", outPlayer.Id}
                            }, res =>
                            {

                            }, "AlertWindow");
                        }
                    });
                }
                else
                {
                    if (sub.CanExceptionalSubstitution(isA))
                    {
                        var コート外の選手 = team.Players.Select(x => x.Id).Except(team.Sets[^1].Rotation.Value).ToArray();
                        var リベロ除く = コート外の選手.Except(team.Players.Where(x => x.IsLibero == true).Select(x => x.Id).ToList()).ToArray();
                        var 退場選手除く = リベロ除く.Except(team.Players.Where(x => x.IsExplusion[_game.Set.Value]).Select(x => x.Id).ToList()).ToArray();
                        var 失格選手除く = 退場選手除く.Except(team.Players.Where(x => x.IsDisqualified).Select(x => x.Id).ToList()).ToArray();
                        var 例外的な選手交代をしたことある人を除く = 失格選手除く.Except(team.Players.Where(x => x.IsExceptionalSubstituted).Select(x => x.Id).ToList()).ToArray();


                        _dialogService.ShowDialog("SelectPlayerAndStaff", new DialogParameters()
                        {
                            {"isA", isA},
                            {"Players", 例外的な選手交代をしたことある人を除く.ToList()}
                        }, res =>
                        {
                            if (res.Result == ButtonResult.OK)
                            {
                                if (res.Parameters.TryGetValue("Mark", out string mark))
                                {
                                    _game.ExceptionalSubstitution(isA, int.Parse(mark), outPlayer.Id);
                                }
                            }
                        });
                    }
                    else
                    {
                        _dialogService.ShowDialog("NotificationDialog", new DialogParameters()
                        {
                            {"Title","通知" },
                            { "Message",$"例外的な選手交代を行うことが出来ません。\n3分間の回復のための時間を与えてください。"},
                            {"ButtonText","OK" }
                        }, res =>
                        {

                        });
                    }
                }

            });

            RefereeCommand.Subscribe(_ =>
            {
                _dialogService.ShowDialog("Referees", new DialogParameters
                {

                }, res =>
                {

                }, "AlertWindow");
            });
        }
        //public ReactiveProperty<DataTable> LeftTeamPlayer { get; set; } = new ReactiveProperty<DataTable>(new DataTable());
        //public ReactiveProperty<DataTable> RightTeamPlayer { get; set; } = new ReactiveProperty<DataTable>(new DataTable());
        //public ReactiveProperty<bool> UndoEnable { get; private set; } = Game.Instance.ObserveProperty(x => x.UndoEnable).ToReactiveProperty();



        public ReactiveCommand LeftSubstitutionCommand { get; set; } = new();
        public ReactiveCommand RightSubstitutionCommand { get; set; } = new();
        //コマンド
        public ReactiveCommand UndoCommand { get; set; } = new();
        public ReactiveCommand CardCommand { get; set; } = new();
        public ReactiveCommand ExceptionalSubstitutionCommand { get; set; } = new();
        public ReactiveCommand ScoresheetCommand { get; set; } = new();
        public ReactiveCommand RefereeCommand { get; set; } = new();

        public ReactiveProperty<bool> UndoEnable { get; set; }
        public ReactiveProperty<bool> IsEnablePoint { get; set; }
        public ReactiveProperty<bool> IsEnableTimeout { get; set; }
        public ReactiveProperty<bool> IsEnableSubstitution { get; set; }


        public ReactiveProperty<Team> ATeam { get; set; }
        public ReactiveProperty<Team> BTeam { get; set; }

        //デバッグ
        public ReactiveProperty<string> DebugMessage { get; private set; }

        public ReactiveProperty<int[]> LeftTeamStartingLineUp { get; set; } = new();
        public ReactiveProperty<int[]> RightTeamStartingLineUp { get; set; } = new();

        public ReactiveProperty<int?[]> LeftTeamSubstitutioned { get; set; } = new(new int?[6]);
        public ReactiveProperty<int?[]> RightTeamSubstitutioned { get; set; } = new(new int?[6]);

        public ReactiveProperty<bool[]> LeftTeamIsReturn { get; set; } = new(new bool[6]);
        public ReactiveProperty<bool[]> RightTeamIsReturn { get; set; } = new(new bool[6]);

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
