using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;

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

            _game.LeftTeam.Name.Subscribe(x => LeftTeamName.Value=x);
            _game.RightTeam.Name.Subscribe(x => RightTeamName.Value=x);
            _game.LeftTeam.Color.Subscribe(x => LeftTeamColor.Value=x);
            _game.RightTeam.Color.Subscribe(x => RightTeamColor.Value=x);

            CancelCommand.Subscribe(_ => RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel)));
            LeftCommand.Subscribe(_ => DelayWarning(true));
            RightCommand.Subscribe(_ => DelayWarning(false));
        }
        private void DelayWarning(bool isLeft)
        {
            if (isLeft)
            {
                if (_game.LeftTeam.DelayWarning is not null)
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
                    _game.LeftTeam.DelayWarning = new Model.DelayWarning()
                    {
                        Point = _game.LeftTeam.Sets[^1].Points.Value,
                        OpponentPoint = _game.RightTeam.Sets[^1].Points.Value,
                        Set = _game.Set.Value
                    };

                    if (_game.isATeamLeft.Value)
                    {
                        _game.HistoryAdd("DWA");
                    }
                    else
                    {
                        _game.HistoryAdd("DWB");
                    }

                    RequestClose?.Invoke(new DialogResult());
                }
            }
            else
            {
                if (_game.RightTeam.DelayWarning is not null)
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
                    _game.RightTeam.DelayWarning = new Model.DelayWarning()
                    {
                        Point = _game.RightTeam.Sets[^1].Points.Value,
                        OpponentPoint = _game.RightTeam.Sets[^1].Points.Value,
                        Set = _game.Set.Value
                    };

                    if (_game.isATeamLeft.Value)
                    {
                        _game.HistoryAdd("DWB");
                    }
                    else
                    {
                        _game.HistoryAdd("DWA");
                    }
                    RequestClose?.Invoke(new DialogResult());
                }
            }
        }
        public ReactiveProperty<string> LeftTeamName { get; set; } = new();
        public ReactiveProperty<string> RightTeamName { get; set; } = new();
        public ReactiveProperty<string> LeftTeamColor { get; set; } = new();
        public ReactiveProperty<string> RightTeamColor { get; set; } = new();




        public ReactiveCommand CancelCommand { get; set; } = new();
        public ReactiveCommand LeftCommand { get; set; } = new();
        public ReactiveCommand RightCommand { get; set; } = new();

        public string Title => "";

        public event Action<IDialogResult> RequestClose;

        public bool CanCloseDialog() => true;

        public void OnDialogClosed()
        {
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
        }
    }
}
