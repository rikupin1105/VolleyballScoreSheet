using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using VolleyballScoreSheet.Model;

namespace VolleyballScoreSheet.ViewModels.Card
{
    public class SelectTeamViewModel : BindableBase, IDialogAware
    {
        private readonly Game _game;
        private readonly DialogService _dialogService;
        public SelectTeamViewModel(Game game, DialogService dialogService)
        {
            _game = game;
            _dialogService=dialogService;

            LeftTeamName = _game.LeftTeam.Name.Value;
            RightTeamName = _game.RightTeam.Name.Value;
            LeftTeamColor = _game.LeftTeam.Color.Value;
            RightTeamColor = _game.RightTeam.Color.Value;

            CancelCommand.Subscribe(_ => RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel)));
        }
        private void RedCard(bool isLeft)
        {
            Team team;
            Team oponentTeam;
            char AorB;
            if (isLeft == _game.isATeamLeft.Value)
            {
                team = _game.LeftTeam;
                oponentTeam = _game.RightTeam;
                AorB = 'A';
            }
            else
            {
                team = _game.RightTeam;
                oponentTeam = _game.LeftTeam;
                AorB = 'B';
            }

            //人選択へ
            _dialogService.ShowDialog("SelectPlayerAndStaff", new DialogParameters()
            {
                { "isLeft" , isLeft },
                { "Message",$"{team.Name}にレッドカードを適用します。\n相手に1点と、サーブ権が与えられます。"}
            }, res =>
            {
                if (res.Parameters.TryGetValue("Mark", out string mark))
                {
                    _game.Sanctions.Value.Add(new Model.Sanction { Set = _game.Set.Value, Penalty= mark, Point=team.Sets[^1].Points.Value, OpponentPoint = oponentTeam.Sets[^1].Points.Value, Team=AorB });
                    _game.PointAdd(!isLeft);
                    _game.History.HistoryAdd($"RedCard{AorB}", mark);

                }
                RequestClose?.Invoke(new DialogResult(res.Result));
            });
        }
        private void DelayPenalty(bool isLeft)
        {
            Team team;
            Team opponentTeam;
            if (isLeft)
            {
                team = _game.LeftTeam;
                opponentTeam = _game.RightTeam;
            }
            else
            {
                team = _game.RightTeam;
                opponentTeam = _game.LeftTeam;
            }


            if (team.DelayWarning is null)
            {
                //1回目の場合はディレイワーニング
                _dialogService.ShowDialog("NotificationDialog", new DialogParameters()
                    {
                        {"Title","注意" },
                        { "Message",$"1回目の遅延行為はワーニングが適用されます。\nセカンドレフェリーに遅延行為が1回目であることを通知し、\nディレイワーニング(黄)の適用を勧めてください。"},
                        {"ButtonText","OK" }
                    }, res =>
                    {
                        RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel));
                    });
            }
            else
            {
                if (team.Sets[^1].Rotation.Value is null)
                {
                    _dialogService.ShowDialog("NotificationDialog", new DialogParameters()
                        {
                            {"Title","注意" },
                            { "Message",$"セット開始前、セット間に適用された遅延行為は\n各チームのラインアップシート提出後に適用してください。"},
                            {"ButtonText","OK" }
                        }, res =>
                        {
                            RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel));
                        });
                }
                else
                {

                    _dialogService.ShowDialog("ConfirmDialog", new DialogParameters()
                    {
                        {"Text",$"{team.Name}にディレイペナルティを適用しますか？" },
                        {"OK","適用" },
                        {"Cancel","キャンセル" },
                    }, res =>
                    {
                        if (res.Result == ButtonResult.OK)
                        {
                            var dp = new Model.DelayPenalty()
                            {
                                Point = _game.LeftTeam.Sets[^1].Points.Value,
                                OpponentPoint = _game.RightTeam.Sets[^1].Points.Value,
                                Set = _game.Set.Value
                            };

                            team.DelayPenalties.Add(dp);

                            if (_game.isATeamLeft.Value)
                            {
                                _game.Sanctions.Value.Add(new('A', dp));
                                _game.PointAdd(false);
                                _game.History.HistoryAdd("DelayPenaltyA");
                            }
                            else
                            {
                                _game.Sanctions.Value.Add(new('B', dp));
                                _game.PointAdd(true);
                                _game.History.HistoryAdd("DelayPenaltyB");
                            }
                        }
                        RequestClose?.Invoke(new DialogResult(res.Result));
                    });
                }

            }

            if (isLeft)
            {
                _game.LeftTeam = team;
            }
            else
            {
                _game.RightTeam = team;
            }
        }
        private void DelayWarning(bool isLeft)
        {
            Team team;
            Team opponentTeam;
            if (isLeft)
            {
                team = _game.LeftTeam;
                opponentTeam = _game.RightTeam;
            }
            else
            {
                team = _game.RightTeam;
                opponentTeam = _game.LeftTeam;
            }

            if (team.DelayWarning is not null)
            {
                //二回目の場合はディレイペナルティ
                _dialogService.ShowDialog("NotificationDialog", new DialogParameters()
                    {
                        {"Title","注意" },
                        { "Message",$"2回目以降の遅延行為はペナルティが適用されます。\nセカンドレフェリーに遅延行為が2回目以降であることを通知し、\nディレイペナルティ(赤)の適用を勧めてください。"},
                        {"ButtonText","OK" }
                    }, res =>
                    {
                        RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel));
                    });
            }
            else
            {
                _dialogService.ShowDialog("ConfirmDialog", new DialogParameters()
                {
                    {"Text",$"{team.Name}にディレイワーニングを適用しますか？" },
                    {"Cancel","キャンセル" },
                    {"OK" ,"適用"}
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

                        if (_game.isATeamLeft.Value)
                        {
                            _game.Sanctions.Value.Add(new('A', dw));
                            _game.History.HistoryAdd("DelayWarningA");
                        }
                        else
                        {
                            _game.Sanctions.Value.Add(new('B', dw));
                            _game.History.HistoryAdd("DelayWarningB");
                        }
                    }
                    RequestClose?.Invoke(new DialogResult(res.Result));
                });
            }
            if (isLeft)
            {
                _game.LeftTeam = team;
            }
            else
            {
                _game.RightTeam = team;
            }

        }
        private void YellowCard(bool isLeft)
        {
            Team team;
            Team oponentTeam;
            char AorB;
            if (isLeft == _game.isATeamLeft.Value)
            {
                team = _game.LeftTeam;
                oponentTeam = _game.RightTeam;
                AorB = 'A';
            }
            else
            {
                team = _game.RightTeam;
                oponentTeam = _game.LeftTeam;
                AorB = 'B';
            }

            if (_game.Sanctions.Value
                .Where(x => x.Team==AorB)
                .Where(x => x.Warning is not "D")
                .Where(x => x.Warning is not null)
                .Any())
            {
                //二回目のイエロー
                _dialogService.ShowDialog("NotificationDialog", new DialogParameters()
                {
                     {"Title","注意" },
                     { "Message",$"{team.Name}には、すでにイエローカードが適用されています。\nセカンドレフェリーにイエローカードが2回目以降であることを通知し、\nペナルティ(赤)の適用を勧めてください。"},
                     {"ButtonText","OK" }
                }, res => { RequestClose?.Invoke(new DialogResult(ButtonResult.OK)); });
            }
            else
            {
                //イエロー適用 人選択へ
                _dialogService.ShowDialog("SelectPlayerAndStaff", new DialogParameters()
                {
                    { "isLeft" , isLeft },
                    { "Message",$"{team.Name}にイエローカードを適用します。"}
                }, res =>
                {
                    if (res.Parameters.TryGetValue("Mark", out string mark))
                    {
                        _game.Sanctions.Value.Add(new Model.Sanction { Set = _game.Set.Value, Warning= mark, Point=team.Sets[^1].Points.Value, OpponentPoint = oponentTeam.Sets[^1].Points.Value, Team=AorB });
                        _game.History.HistoryAdd($"YellowCard{AorB}", mark);

                    }
                    RequestClose?.Invoke(new DialogResult(res.Result));
                });
            }

        }
        public void ImproperRequests(bool isLeft)
        {
            Team team;
            if (isLeft)
            {
                team = _game.LeftTeam;
            }
            else
            {
                team = _game.RightTeam;
            }

            if (team.ImproperRequests.Value == true)
            {
                //二回目 ディレイ
                _dialogService.ShowDialog("NotificationDialog", new DialogParameters()
                {
                    {"Title","注意" },
                    { "Message",$"{team.Name}には、すでに不当な要求が適用されています。\nセカンドレフェリーに2回目以降の不当な要求であることを通知し、\nディレイワーニングまたは、ディレイペナルティを適用を勧めてください。"},
                    {"ButtonText","OK" }
                }, res =>
                {

                });
            }
            else
            {
                _dialogService.ShowDialog("ConfirmDialog", new DialogParameters()
                {
                    { "Text", $"{team.Name}に不当な要求を適用しますか？" },
                    { "OK", "適用" },
                    { "Cancel", "キャンセル" },
                }, res =>
                {
                    if (res.Result == ButtonResult.OK)
                    {
                        team.ImproperRequests.Value = true;
                        if (_game.isATeamLeft.Value)
                        {
                            _game.History.HistoryAdd("ImproperRequestsA");
                        }
                        else
                        {
                            _game.History.HistoryAdd("ImproperRequestsB");
                        }
                    }
                    RequestClose?.Invoke(new DialogResult(res.Result));
                });
            }

            if (isLeft)
            {
                team = _game.RightTeam;
            }
            else
            {
                team = _game.LeftTeam;
            }
        }

        public ReactiveCommand CancelCommand { get; set; } = new();
        public ReactiveCommand LeftCommand { get; set; } = new();
        public ReactiveCommand RightCommand { get; set; } = new();

        public string LeftTeamName { get; set; }
        public string RightTeamName { get; set; }
        public string LeftTeamColor { get; set; }
        public string RightTeamColor { get; set; }

        public ReactiveProperty<string> Title { get; set; } = new();
        public ReactiveProperty<string> Message { get; set; } = new();

        string IDialogAware.Title => "";

        public event Action<IDialogResult>? RequestClose;

        public bool CanCloseDialog() => true;

        public void OnDialogClosed()
        {
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            if (parameters.TryGetValue("Sanctions", out SanctionEnum sanction))
            {
                switch (sanction)
                {
                    case SanctionEnum.ImproperRequest:
                        LeftCommand.Subscribe(_ => ImproperRequests(true));
                        RightCommand.Subscribe(_ => ImproperRequests(false));
                        Message.Value = "これ自体は罰則ではないが、さらに不当な要求をした場合は遅延行為となる。";
                        Title.Value = "不当な要求";
                        break;
                    case SanctionEnum.DelayWarning:
                        LeftCommand.Subscribe(_ => DelayWarning(true));
                        RightCommand.Subscribe(_ => DelayWarning(false));
                        Message.Value = "これ自体は罰則ではないが、次からは罰則になる。";
                        Title.Value = "ディレイワーニング";
                        break;
                    case SanctionEnum.DelayPenalty:
                        LeftCommand.Subscribe(_ => DelayPenalty(true));
                        RightCommand.Subscribe(_ => DelayPenalty(false));
                        Message.Value = "相手に1点と、サーブ権が与えられます。";
                        Title.Value = "ディレイペナルティ";
                        break;
                    case SanctionEnum.Warning:
                        LeftCommand.Subscribe(_ => YellowCard(true));
                        RightCommand.Subscribe(_ => YellowCard(false));
                        Message.Value = "これ自体は罰則ではないが、次からは罰則になる。";
                        Title.Value = "イエローカード";
                        break;
                    case SanctionEnum.Penalty:
                        LeftCommand.Subscribe(_ => RedCard(true));
                        RightCommand.Subscribe(_ => RedCard(false));
                        Message.Value = "相手に1点と、サーブ権が与えられます。";
                        Title.Value = "レッドカード";
                        break;
                    case SanctionEnum.Explusion:
                        break;
                    case SanctionEnum.Disqualification:
                        break;
                }
            }

        }
    }
}
