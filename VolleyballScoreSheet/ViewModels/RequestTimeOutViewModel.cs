using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using Wpf.Ui.Controls.Interfaces;

namespace VolleyballScoreSheet.ViewModels
{
    public class RequestTimeOutViewModel : BindableBase
    {
        private readonly Game _game;
        private readonly IDialogService _dialogService;
        public RequestTimeOutViewModel(Game game, IDialogService dialogService)
        {
            _game = game;
            _dialogService = dialogService;

            DisplayRequestTimeOut = _game.ToReactivePropertyAsSynchronized(x => x.DisplayRequestTimeOut.Value);

            LeftCommand.Subscribe(_ => _game.TimeOutSide(true));
            RightCommand.Subscribe(_ => _game.TimeOutSide(false));
            CancelCommand.Subscribe(_ => _game.CancelTimeOut());

            _game.TimeoutRejectionCommand.Subscribe(x =>
            {
                _dialogService.ShowDialog("NotificationDialog", new DialogParameters
                {
                    {"Title","注意" },
                    { "Message",$"{x}のタイムアウト回数が規定を超えています。\nセカンドレフェリーに伝えてください。"},
                    {"ButtonText","OK" }
                }, res =>
                {

                }, "AlertWindow");
            });
            _game.SecondTimeoutCommand.Subscribe(x =>
            {
                _dialogService.ShowDialog("NotificationDialog", new DialogParameters
                {
                    {"Title","通知" },
                    { "Message",$"{x}のタイムアウトは2回目です。\nセカンドレフェリーに伝えてください。"},
                    {"ButtonText","OK" }
                }, res =>
                {

                }, "AlertWindow");
            });

            _game.ATeam.Value.Name.Subscribe(x =>
            {
                if (_game.isATeamLeft.Value)
                {
                    LeftTeamName.Value=x;
                }
                else
                {
                    RightTeamName.Value=x;
                }
            });
            _game.BTeam.Value.Name.Subscribe(x =>
            {
                if (_game.isATeamLeft.Value)
                {
                    RightTeamName.Value=x;
                }
                else
                {
                    LeftTeamName.Value=x;
                }
            });

            _game.ATeam.Value.Color.Subscribe(x =>
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
            _game.BTeam.Value.Color.Subscribe(x =>
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
        }


        //表示
        public ReactiveProperty<bool> DisplayRequestTimeOut { get; set; }

        public ReactiveCommand LeftCommand { get; } = new();
        public ReactiveCommand RightCommand { get; } = new();
        public ReactiveCommand CancelCommand { get; } = new();
        public ReactiveProperty<string> LeftTeamName { get; set; } = new();
        public ReactiveProperty<string> RightTeamName { get; set; } = new();
        public ReactiveProperty<string> LeftTeamColor { get; set; } = new();
        public ReactiveProperty<string> RightTeamColor { get; set; } = new();
    }
}
