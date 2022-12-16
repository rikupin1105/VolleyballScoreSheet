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
    public class DelayWarningViewModel : BindableBase, IDialogAware
    {
        private readonly Game _game;
        private readonly DialogService _dialogService;
        public DelayWarningViewModel(Game game, DialogService dialogService)
        {
            _game = game;
            _dialogService = dialogService;

            LeftTeamName =  _game.LeftTeam.Name.Value;
            RightTeamName = _game.RightTeam.Name.Value;
            LeftTeamColor = _game.LeftTeam.Color.Value;
            RightTeamColor = _game.RightTeam.Color.Value;

            CancelCommand.Subscribe(_ => RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel)));
            LeftCommand.Subscribe(_ => DelayWarning(true));
            RightCommand.Subscribe(_ => DelayWarning(false));
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

                    });
            }
            else
            {
                _dialogService.ShowDialog("ConfirmDialog", new DialogParameters()
                {
                    {"Text","ディレイワーニングを適用しますか？" },
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
