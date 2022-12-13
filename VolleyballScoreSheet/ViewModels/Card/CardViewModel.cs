using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;

namespace VolleyballScoreSheet.ViewModels.Card
{
	public class CardViewModel : BindableBase, IDialogAware
    {

        private readonly IDialogService _dialogService;
        private readonly Game _game;
        public CardViewModel(Game game, IDialogService dialogService)
        {
            _game = game;
            _dialogService = dialogService;

            CancelCommand.Subscribe(_ => RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel)));
            DelayWarningCommand.Subscribe(_ =>
            {
                CancelCommand.Execute();
                _dialogService.ShowDialog("DelayWarning", new DialogParameters()
                {

                }, res =>
                {

                }, "AlertWindow");
            });

            DelayPenaltyCommand.Subscribe(_ =>
            {
                CancelCommand.Execute();
                _dialogService.ShowDialog("DelayPenalty", new DialogParameters()
                {

                }, res =>
                {

                }, "AlertWindow");
            });
        }


        public ReactiveCommand CancelCommand { get; set; } = new();
        public ReactiveCommand DelayWarningCommand { get; set; } = new();
        public ReactiveCommand DelayPenaltyCommand { get; set; } = new();








        public string Title => "";

        public event Action<IDialogResult> RequestClose;

        public bool CanCloseDialog() => true;
        public void OnDialogClosed() { }

        public void OnDialogOpened(IDialogParameters parameters)
        {

        }
    }
}
