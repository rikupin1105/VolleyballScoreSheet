using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using VolleyballScoreSheet.Model;
using VolleyballScoreSheet.Views.Card;

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

            _game.ATeam.Value.ImproperRequests.Subscribe(x => { ImproperRequestA.Value = x; });
            _game.BTeam.Value.ImproperRequests.Subscribe(x => { ImproperRequestB.Value = x; });
            _game.Sanctions.Subscribe(x => Sanctions.Value=x);

            CancelCommand.Subscribe(_ => RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel)));
            DelayWarningCommand.Subscribe(_ =>
            {
                _dialogService.ShowDialog("DelayWarning", new DialogParameters()
                {

                }, res =>
                {
                    if(res.Result == ButtonResult.OK)
                    {
                        RequestClose?.Invoke(new DialogResult(res.Result));
                    }
                }, "AlertWindow");
            });

            DelayPenaltyCommand.Subscribe(_ =>
            {
                _dialogService.ShowDialog("DelayPenalty", new DialogParameters()
                {

                }, res =>
                {
                    if (res.Result == ButtonResult.OK)
                    {
                        RequestClose?.Invoke(new DialogResult(res.Result));
                    }
                }, "AlertWindow");
            });

            ImproperRequestCommmand.Subscribe(_ =>
            {
                _dialogService.ShowDialog("ImproperRequests", new DialogParameters()
                {

                }, res =>
                {
                    if (res.Result == ButtonResult.OK)
                    {
                        RequestClose?.Invoke(new DialogResult(res.Result));
                    }
                }, "AlertWindow");
            });
        }
        public ReactiveProperty<bool> ImproperRequestA { get; set; } = new(false);
        public ReactiveProperty<bool> ImproperRequestB { get; set; } = new(false);
        public ReactiveProperty<List<Sanction>> Sanctions { get; set; } = new();
        public ReactiveCommand CancelCommand { get; set; } = new();
        public ReactiveCommand DelayWarningCommand { get; set; } = new();
        public ReactiveCommand DelayPenaltyCommand { get; set; } = new();
        public ReactiveCommand ImproperRequestCommmand { get; set; } = new();


        public string Title => "";

        public event Action<IDialogResult>?RequestClose;

        public bool CanCloseDialog() => true;
        public void OnDialogClosed() { }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            ImproperRequestA = new(_game.ATeam.Value.ImproperRequests);
            ImproperRequestB = new(_game.BTeam.Value.ImproperRequests);
        }
    }
}
