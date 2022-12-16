using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Services.Dialogs;
using Reactive.Bindings;
using VolleyballScoreSheet.Model;

namespace VolleyballScoreSheet.ViewModels
{
    public class SubstitutionViewModel : IDialogAware
    {
        private readonly IDialogService _dialogService;
        private readonly Game _game;
        public SubstitutionViewModel(Game game, IDialogService dialogService)
        {
            _game = game;
            _dialogService = dialogService;
            CancelCommand.Subscribe(_ => RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel)));
            SubstitutionCommand.Subscribe(_ => Substitution());
            OutSelectionChangedCommand.Subscribe(_ => OutMemberSelectionChanged());
        }
        public void Substitution()
        {
            var flag = true;


            if (OutMember.Value is not null && InMember.Value is not null)
            {
                var outMember = int.Parse(OutMember.Value.Split(' ')[0]);
                var inMember = int.Parse(InMember.Value.Split(' ')[0]);

                Model.Team team;
                Model.Team opponentTeam;
                char AorB;
                if (TeamName.Value == _game.ATeam.Value.Name.Value)
                {
                    team = _game.ATeam.Value;
                    opponentTeam = _game.BTeam.Value;
                    AorB = 'A';
                }
                else
                {
                    team = _game.BTeam.Value;
                    opponentTeam = _game.ATeam.Value;
                    AorB = 'B';
                }

                var 今入った選手 = team.Sets[^1].SubstitutionDetails
                .Where(x => x.Point == team.Sets[^1].Points.Value)
                .Where(x => x.OpponentPoint == opponentTeam.Sets[^1].Points.Value).Select(x => x.In).ToArray();

                if (今入った選手.Contains(outMember))
                {
                    SanctionEnum sanction;
                    if (team.ImproperRequests.Value == true)
                    {
                        if (team.DelayWarning is null)
                        {
                            sanction = SanctionEnum.DelayWarning;
                        }
                        else
                        {
                            sanction = SanctionEnum.DelayPenalty;
                        }
                    }
                    else
                    {
                        sanction = SanctionEnum.ImproperRequest;
                    }

                    //不当な要求
                    //怪我等の場合は認める
                    _dialogService.ShowDialog("SameInterruptionSubstitution", new DialogParameters
                    {
                        {"Title","注意" },
                        { "Message",$"同一中断中での選手交代です。\n怪我などやむを得ない場合は承認を、\nそれ以外の場合はセカンドレフェリーに確認し、拒否してください。"},
                        { "Sanction",SanctionToJapanese(sanction)},
                    }, res =>
                    {
                        if (res.Result == ButtonResult.OK)
                        {
                            if (AorB=='A')
                            {
                                _game.Substitution(true, inMember, outMember);
                            }
                            else
                            {
                                _game.Substitution(false, inMember, outMember);
                            }
                        }
                        else if (res.Result == ButtonResult.Abort)
                        {
                            if (sanction == SanctionEnum.DelayWarning)
                            {
                                _dialogService.ShowDialog("ConfirmDialog", new DialogParameters()
                                    {
                                        {"Text","ディレイワーニングを適用しますか？" },
                                        {"OK","適用" },
                                        {"Cancel","キャンセル" },
                                    }, res =>
                                    {
                                        if (res.Result == ButtonResult.OK)
                                        {
                                            var dw = new Model.DelayWarning()
                                            {
                                                Point = team.Sets[^1].Points.Value,
                                                OpponentPoint = opponentTeam.Sets[^1].Points.Value,
                                                Set = _game.Set.Value
                                            };

                                            team.DelayWarning = dw;

                                            _game.Sanctions.Value.Add(new(AorB, dw));
                                            _game.History.HistoryAdd("DelayWarning"+AorB);
                                        }
                                    });
                            }
                            else if (sanction == SanctionEnum.DelayPenalty)
                            {
                                //ディレイペナルティ
                                _dialogService.ShowDialog("ConfirmDialog", new DialogParameters()
                                    {
                                        {"Text","ディレイペナルティを適用しますか？" },
                                        {"OK","適用" },
                                        {"Cancel","キャンセル" },
                                    }, res =>
                                    {
                                        if (res.Result == ButtonResult.OK)
                                        {
                                            var dp = new Model.DelayPenalty()
                                            {
                                                Point = team.Sets[^1].Points.Value,
                                                OpponentPoint = opponentTeam.Sets[^1].Points.Value,
                                                Set = _game.Set.Value
                                            };

                                            team.DelayPenalties.Add(dp);

                                            _game.Sanctions.Value.Add(new(AorB, dp));
                                            if (AorB=='A')
                                            {
                                                _game.Point(false);
                                            }
                                            else
                                            {
                                                _game.Point(true);

                                            }
                                            _game.History.HistoryAdd("DelayPenalty"+AorB);
                                        }
                                    });
                            }
                            else if (sanction==SanctionEnum.ImproperRequest)
                            {
                                //不当な要求
                                _dialogService.ShowDialog("ConfirmDialog", new DialogParameters()
                                    {
                                        { "Text", "不当な要求を適用しますか？" },
                                        { "OK", "適用" },
                                        { "Cancel", "キャンセル" },
                                    }, res =>
                                    {
                                        if (res.Result == ButtonResult.OK)
                                        {
                                            team.ImproperRequests.Value = true;
                                            _game.History.HistoryAdd("ImproperRequests"+AorB);
                                        }
                                    });
                            }
                        }
                    }, "AlertWindow");
                }
                else
                {
                    if (TeamName.Value == _game.ATeam.Value.Name.Value)
                    {
                        _game.Substitution(true, inMember, outMember);
                    }
                    else
                    {
                        _game.Substitution(false, inMember, outMember);

                    }
                }

                if (TeamName.Value == _game.ATeam.Value.Name.Value)
                {
                    _game.ATeam.Value = team;
                    _game.BTeam.Value = opponentTeam;
                }
                else
                {
                    _game.BTeam.Value = team;
                    _game.ATeam.Value = opponentTeam;
                }

            }

            if (flag)
            {
                RequestClose?.Invoke(new DialogResult());
            }

        }
        public ReactiveProperty<string> OutMember { get; set; } = new();
        public ReactiveProperty<string> InMember { get; set; } = new();
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
            if (parameters.TryGetValue("Side", out string side))
            {
                Team team;
                if (side=="Left")
                {
                    team = _game.LeftTeam;
                    TeamName.Value  = _game.LeftTeam.Name.Value;
                }
                else
                {
                    team = _game.RightTeam;
                    TeamName.Value  = _game.RightTeam.Name.Value;
                }


                var 選手交代で出たことある人リスト = team.Sets[^1].SubstitutionDetails.Select(x => x.Out).ToList();
                var 選手交代で入ったことある人リスト = team.Sets[^1].SubstitutionDetails.Select(x => x.In).ToList();

                var 選手交代で下がれる人リスト = team.Sets[^1].Rotation.Value
                    .Except(選手交代で出たことある人リスト).OrderBy(x => x).ToArray();

                var コート外の選手 = team.Players.Select(x => x.Id)
                    .Except(team.Sets[^1].Rotation.Value).ToArray();

                var 選手交代で入れる人リスト = コート外の選手.Except(選手交代で入ったことある人リスト).ToArray();

                OnCourtMemberItem.Value = 選手交代で下がれる人リスト
                    .Select(x => x+" "+team.Players
                    .Where(y => y.Id == x)
                    .Select(x => x.Name).First())
                    .ToArray();

                OutCourtMemberItem.Value = 選手交代で入れる人リスト
                    .Select(x => x+" "+team.Players
                    .Where(y => y.Id == x)
                    .Select(x => x.Name)
                    .First())
                    .ToArray();
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
            Team team;
            if (TeamName.Value == _game.ATeam.Value.Name.Value)
            {
                team = _game.ATeam.Value;
            }
            else
            {
                team = _game.BTeam.Value;
            }

            var 選手交代で出たことある人リスト = team.Sets[^1].SubstitutionDetails.Select(x => x.Out).ToList();
            var 選手交代で入ったことある人リスト = team.Sets[^1].SubstitutionDetails.Select(x => x.In).ToList();

            var 選手交代で入れる人リスト = team.Players
                .Select(x => x.Id)
                .Except(team.Sets[^1].Rotation.Value)
                .Except(選手交代で入ったことある人リスト)
                .OrderBy(x => x)
                .ToArray();


            if (選手交代で入ったことある人リスト.Contains(int.Parse(OutMember.Value.Split(' ')[0])))
            {
                //再入場
                OutCourtMemberItem.Value = (string[])team.Sets[^1].SubstitutionDetails
                    .Where(x => x.In==int.Parse(OutMember.Value.Split(' ')[0]))
                    .Select(x => x.Out)
                    .Select(x => x+" "+team.Players
                    .Where(y => y.Id == x)
                    .Select(x=>x.Name)
                    .First())
                    .ToArray();
            }
            else
            {
                OutCourtMemberItem.Value = (string[])選手交代で入れる人リスト.Except(選手交代で出たことある人リスト).ToArray()
                    .Select(x => x+" "+team.Players
                    .Where(y => y.Id == x)
                    .Select(x => x.Name).First())
                    .ToArray();
            }
        }
        public ReactiveCommand OutSelectionChangedCommand { get; } = new();
        public ReactiveCommand CancelCommand { get; } = new();
        public ReactiveCommand SubstitutionCommand { get; } = new();
        public ReactiveProperty<string> TeamName { get; set; } = new();
        public ReactiveProperty<string[]> OnCourtMemberItem { get; set; } = new();
        public ReactiveProperty<string[]> OutCourtMemberItem { get; set; } = new();
        public string Title => "メンバーチェンジ";
        public event Action<IDialogResult>? RequestClose;
        public bool CanCloseDialog() => true;
        public void OnDialogClosed() { }
    }
}
