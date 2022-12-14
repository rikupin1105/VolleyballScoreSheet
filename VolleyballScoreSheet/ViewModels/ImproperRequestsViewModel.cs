using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using VolleyballScoreSheet.Views;

namespace VolleyballScoreSheet.ViewModels
{
    public class ImproperRequestsViewModel : BindableBase, IDialogAware
    {
        private readonly IDialogService _dialogService;
        private readonly Game _game;
        public ImproperRequestsViewModel(Game game, IDialogService dialogService)
        {
            _game = game;
            _dialogService = dialogService;

            _game.LeftTeam.Name.Subscribe(x => LeftTeamName.Value=x);
            _game.RightTeam.Name.Subscribe(x => RightTeamName.Value=x);
            _game.LeftTeam.Color.Subscribe(x => LeftTeamColor.Value=x);
            _game.RightTeam.Color.Subscribe(x => RightTeamColor.Value=x);

            CancelCommand.Subscribe(_ => RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel)));
            LeftCommand.Subscribe(_ => ImproperRequests(true));
            RightCommand.Subscribe(_ => ImproperRequests(false));
        }

        public void ImproperRequests(bool isLeft)
        {
            if (isLeft)
            {
                if (_game.LeftTeam.ImproperRequests.Value == true)
                {
                    //二回目 ディレイ
                    _dialogService.ShowDialog("NotificationDialog", new DialogParameters()
                    {
                        {"Title","注意" },
                        { "Message",$"すでに不当な要求が適用されています。\nセカンドレフェリーに2回目以降の不当な要求であることを通知し、\nディレイワーニングまたは、ディレイペナルティを適用を勧めてください。"},
                        {"ButtonText","OK" }
                    }, res =>
                    {

                    });
                }
                else
                {
                    _dialogService.ShowDialog("ConfirmDialog", new DialogParameters()
                    {
                        { "Text", "不当な要求を適用しますか？" },
                        { "OK", "適用" },
                        { "Cancel", "キャンセル" },
                    }, res =>
                    {
                        if (res.Result == ButtonResult.OK)
                        {
                            _game.LeftTeam.ImproperRequests.Value = true;
                            if (_game.isATeamLeft.Value)
                            {
                                _game.HistoryAdd("ImproperRequestsA");
                            }
                            else
                            {
                                _game.HistoryAdd("ImproperRequestsB");
                            }
                        }
                        RequestClose?.Invoke(new DialogResult(res.Result));
                    });
                }
            }
            else
            {
                if (_game.RightTeam.ImproperRequests.Value == true)
                {
                    //二回目 ディレイ
                    _dialogService.ShowDialog("NotificationDialog", new DialogParameters()
                    {
                        {"Title","注意" },
                        { "Message",$"すでに不当な要求が適用されています。\nセカンドレフェリーに2回目以降の不当な要求であることを通知し、\nディレイワーニングまたは、ディレイペナルティを適用を勧めてください。"},
                        {"ButtonText","OK" }
                    }, res =>
                    {

                    });
                }
                else
                {
                    _dialogService.ShowDialog("ConfirmDialog", new DialogParameters()
                    {
                        { "Text", "不当な要求を適用しますか？" },
                        { "OK", "適用" },
                        { "Cancel", "キャンセル" },
                    }, res =>
                    {
                        if (res.Result == ButtonResult.OK)
                        {
                            _game.RightTeam.ImproperRequests.Value = true;
                            if (_game.isATeamLeft.Value)
                            {
                                _game.HistoryAdd("ImproperRequestsB");
                            }
                            else
                            {
                                _game.HistoryAdd("ImproperRequestsA");
                            }
                        }
                        RequestClose?.Invoke(new DialogResult(res.Result));
                    });
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
