using Prism.Mvvm;
using Prism.Services.Dialogs;
using Reactive.Bindings;
using System;
using VolleyballScoreSheet.Model;

namespace VolleyballScoreSheet.ViewModels.Card
{
    public class DelayPenaltyViewModel : BindableBase, IDialogAware
    {
        private readonly Game _game;
        private readonly DialogService _dialogService;

        public DelayPenaltyViewModel(Game game, DialogService dialogService)
        {
            _game = game;
            _dialogService = dialogService;

            LeftTeamName = _game.LeftTeam.Name.Value;
            RightTeamName = _game.RightTeam.Name.Value;
            LeftTeamColor = _game.LeftTeam.Color.Value;
            RightTeamColor = _game.RightTeam.Color.Value;

            CancelCommand.Subscribe(_ => RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel)));
            LeftCommand.Subscribe(_ => DelayPenalty(true));
            RightCommand.Subscribe(_ => DelayPenalty(false));

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

                        });
                }
                else
                {

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


        public string LeftTeamName { get; set; }
        public string RightTeamName { get; set; }
        public string LeftTeamColor { get; set; }
        public string RightTeamColor { get; set; }

        public ReactiveCommand CancelCommand { get; set; } = new();
        public ReactiveCommand LeftCommand { get; set; } = new();
        public ReactiveCommand RightCommand { get; set; } = new();


        public string Title => "";

        public event Action<IDialogResult>? RequestClose;

        public bool CanCloseDialog() => true;

        public void OnDialogClosed()
        {
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
        }
    }
}

