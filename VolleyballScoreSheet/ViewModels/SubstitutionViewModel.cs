using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
using Prism.Services.Dialogs;
using Reactive.Bindings;
using Unity.Policy;
using VolleyballScoreSheet.Views;

namespace VolleyballScoreSheet.ViewModels
{
    public class SubstitutionViewModel : IDialogAware
    {
        private readonly IDialogService _dialogService;
        private readonly Game _game;
        private Shared.Substitution _substitution;
        public SubstitutionViewModel(Game game, IDialogService dialogService)
        {
            _game = game;
            _dialogService = dialogService;
            _substitution = new Shared.Substitution(game);

            CancelCommand.Subscribe(_ => RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel)));
            SubstitutionCommand.Subscribe(_ => Substitution());
            OutSelectionChangedCommand.Subscribe(_ => OutMemberSelectionChanged());

            _substitution.SubstitutionCountNotifyCommand.Subscribe(x =>
            {
                _dialogService.ShowDialog("NotificationDialog", new DialogParameters
                {
                    {"Title","通知"},
                    {"Message",$"{x}回目の選手交代です。\nセカンドレフェリーに伝えてください。"},
                    {"ButtonText","OK"}
                }, res => { });
            });

            _substitution.SubstitutionOverSixTimeCommand.Subscribe(_ =>
            {
                _dialogService.ShowDialog("NotificationDialog", new DialogParameters
                {
                    {"Title","警告"},
                    {"Message",$"選手交代回数が6回を超えています。\nセカンドレフェリーに確認してください。"},
                    {"ButtonText","OK"}
                }, res => { });
            });
        }
        public void Substitution()
        {
            if (OutMember.Value is null) return;
            if (InMember.Value is null) return;

            _substitution.DoSubstitution(IsLeft, OutMember.Value, InMember.Value);
            RequestClose?.Invoke(new DialogResult());

            return;
            //var flag = true;


            //if (OutMember.Value is not null && InMember.Value is not null)
            //{
            //    var outMember = OutMember.Value.Id;
            //    var inMember = InMember.Value.Id;

            //    Model.Team team;
            //    Model.Team opponentTeam;
            //    char AorB;
            //    if (TeamName.Value == _game.ATeam.Value.Name.Value)
            //    {
            //        team = _game.ATeam.Value;
            //        opponentTeam = _game.BTeam.Value;
            //        AorB = 'A';
            //    }
            //    else
            //    {
            //        team = _game.BTeam.Value;
            //        opponentTeam = _game.ATeam.Value;
            //        AorB = 'B';
            //    }

            //    var 今入った選手 = team.Sets[^1].SubstitutionDetails
            //    .Where(x => x.Point == team.Sets[^1].Points.Value)
            //    .Where(x => x.OpponentPoint == opponentTeam.Sets[^1].Points.Value).Select(x => x.In).ToArray();

            //    if (今入った選手.Contains(outMember))
            //    {
            //        SanctionEnum sanction;
            //        if (team.ImproperRequests.Value == true)
            //        {
            //            if (team.DelayWarning is null)
            //            {
            //                sanction = SanctionEnum.DelayWarning;
            //            }
            //            else
            //            {
            //                sanction = SanctionEnum.DelayPenalty;
            //            }
            //        }
            //        else
            //        {
            //            sanction = SanctionEnum.ImproperRequest;
            //        }

            //        //不当な要求
            //        //怪我等の場合は認める
            //        _dialogService.ShowDialog("SameInterruptionSubstitution", new DialogParameters
            //        {
            //            {"Title","注意" },
            //            { "Message",$"同一中断中での選手交代です。\n怪我などやむを得ない場合は承認を、\nそれ以外の場合はセカンドレフェリーに確認し、拒否してください。"},
            //            { "Sanction",SanctionToJapanese(sanction)},
            //        }, res =>
            //        {
            //            if (res.Result == ButtonResult.OK)
            //            {
            //                if (AorB=='A')
            //                {
            //                    _game.Substitution(true, inMember, outMember);
            //                }
            //                else
            //                {
            //                    _game.Substitution(false, inMember, outMember);
            //                }
            //            }
            //            else if (res.Result == ButtonResult.Abort)
            //            {
            //                if (sanction == SanctionEnum.DelayWarning)
            //                {
            //                    _dialogService.ShowDialog("ConfirmDialog", new DialogParameters()
            //                        {
            //                            {"Text","ディレイワーニングを適用しますか？" },
            //                            {"OK","適用" },
            //                            {"Cancel","キャンセル" },
            //                        }, res =>
            //                        {
            //                            if (res.Result == ButtonResult.OK)
            //                            {
            //                                var dw = new Model.DelayWarning()
            //                                {
            //                                    Point = team.Sets[^1].Points.Value,
            //                                    OpponentPoint = opponentTeam.Sets[^1].Points.Value,
            //                                    Set = _game.Set.Value
            //                                };

            //                                team.DelayWarning = dw;

            //                                _game.Sanctions.Value.Add(new(AorB, dw));
            //                                _game.History.HistoryAdd("DelayWarning"+AorB);
            //                            }
            //                        });
            //                }
            //                else if (sanction == SanctionEnum.DelayPenalty)
            //                {
            //                    //ディレイペナルティ
            //                    _dialogService.ShowDialog("ConfirmDialog", new DialogParameters()
            //                        {
            //                            {"Text","ディレイペナルティを適用しますか？" },
            //                            {"OK","適用" },
            //                            {"Cancel","キャンセル" },
            //                        }, res =>
            //                        {
            //                            if (res.Result == ButtonResult.OK)
            //                            {
            //                                var dp = new Model.DelayPenalty()
            //                                {
            //                                    Point = team.Sets[^1].Points.Value,
            //                                    OpponentPoint = opponentTeam.Sets[^1].Points.Value,
            //                                    Set = _game.Set.Value
            //                                };

            //                                team.DelayPenalties.Add(dp);

            //                                _game.Sanctions.Value.Add(new(AorB, dp));
            //                                if (AorB=='A')
            //                                {
            //                                    _game.Point(false);
            //                                }
            //                                else
            //                                {
            //                                    _game.Point(true);

            //                                }
            //                                _game.History.HistoryAdd("DelayPenalty"+AorB);
            //                            }
            //                        });
            //                }
            //                else if (sanction==SanctionEnum.ImproperRequest)
            //                {
            //                    //不当な要求
            //                    _dialogService.ShowDialog("ConfirmDialog", new DialogParameters()
            //                        {
            //                            { "Text", "不当な要求を適用しますか？" },
            //                            { "OK", "適用" },
            //                            { "Cancel", "キャンセル" },
            //                        }, res =>
            //                        {
            //                            if (res.Result == ButtonResult.OK)
            //                            {
            //                                team.ImproperRequests.Value = true;
            //                                _game.History.HistoryAdd("ImproperRequests"+AorB);
            //                            }
            //                        });
            //                }
            //            }
            //        }, "AlertWindow");
            //    }
            //    else
            //    {
            //        if (TeamName.Value == _game.ATeam.Value.Name.Value)
            //        {
            //            _game.Substitution(true, inMember, outMember);
            //        }
            //        else
            //        {
            //            _game.Substitution(false, inMember, outMember);

            //        }
            //    }

            //    if (TeamName.Value == _game.ATeam.Value.Name.Value)
            //    {
            //        _game.ATeam.Value = team;
            //        _game.BTeam.Value = opponentTeam;
            //    }
            //    else
            //    {
            //        _game.BTeam.Value = team;
            //        _game.ATeam.Value = opponentTeam;
            //    }

            //}

            //if (flag)
            //{

            //}

        }
        public ReactiveProperty<Player> OutMember { get; set; } = new();
        public ReactiveProperty<Player> InMember { get; set; } = new();
        public ReactiveProperty<string> TeamName { get; set; } = new();
        private bool IsLeft { get; set; }
        public void Dialog(string message)
        {
            _dialogService.ShowDialog(
                    "NotificationDialog",
                    new DialogParameters
                    {
                        { "Title", "Alert" },
                        { "Message", message },
                        { "ButtonText", "OK" }
                    }, res =>
                    {

                    }, "AlertWindow");
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            if (parameters.TryGetValue("Side", out bool isLeft))
            {
                if (isLeft)
                {
                    TeamName.Value = _game.LeftTeam.Name.Value;
                }
                else
                {
                    TeamName.Value = _game.RightTeam.Name.Value;
                }
                IsLeft = isLeft;
            }
            if (parameters.TryGetValue("isA", out bool isA))
            {
                if (isA)
                {
                    TeamName.Value = _game.ATeam.Value.Name.Value;
                    isLeft = _game.isATeamLeft.Value;
                }
                else
                {
                    TeamName.Value = _game.BTeam.Value.Name.Value;
                    isLeft = !_game.isATeamLeft.Value;
                }
            }


            if (parameters.TryGetValue("OutPlayer", out int outPlayer))
            {

                _substitution.SubstitutionOpenDialog(isLeft, outPlayer);

                OnCourtMemberItem.Value = _substitution.OncourtMember;
                OutMember.Value = _substitution.OutMember;

                OutMemberSelectionChanged();
            }
            else
            {
                _substitution.SubstitutionOpenDialog(isLeft);

                OnCourtMemberItem.Value = _substitution.OncourtMember;
                OutCourtMemberItem.Value = _substitution.OffCourtMember;
            }

        }

        public string SanctionToJapanese(SanctionEnum sanction)
        {
            switch (sanction)
            {
                case SanctionEnum.ImproperRequest:
                    return "不当な要求";
                case SanctionEnum.DelayWarning:
                    return "ディレイワーニング";
                case SanctionEnum.DelayPenalty:
                    return "ディレイペナルティ";
                default:
                    return "";
            }
        }
        public void OutMemberSelectionChanged()
        {
            _substitution.OutMemberSelectionChanged(IsLeft, OutMember.Value.Id);


            InMember.Value = _substitution.InMember;
            OnCourtMemberItem.Value = _substitution.OncourtMember;
            OutCourtMemberItem.Value = _substitution.OffCourtMember;
        }
        public ReactiveCommand OutSelectionChangedCommand { get; } = new();
        public ReactiveCommand CancelCommand { get; } = new();
        public ReactiveCommand SubstitutionCommand { get; } = new();
        public ReactiveProperty<List<Player>> OnCourtMemberItem { get; set; } = new();
        public ReactiveProperty<List<Player>> OutCourtMemberItem { get; set; } = new();
        public string Title => "メンバーチェンジ";
        public event Action<IDialogResult>? RequestClose;
        public bool CanCloseDialog() => true;
        public void OnDialogClosed() { }
    }
}
