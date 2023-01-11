using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;

namespace VolleyballScoreSheet.ViewModels
{
    public class EditPlayerViewModel : BindableBase, IDialogAware
    {
        private readonly DialogService _dialogService;
        public EditPlayerViewModel(DialogService dialogService)
        {
            _dialogService = dialogService;
            DeleteCommand.Subscribe(_ =>
            {
                _dialogService.ShowDialog("ConfirmDialog", new DialogParameters()
                {
                    {"Text",$"選手を削除してもいいですか？" },
                    {"Cancel","キャンセル" },
                    {"OK" ,"削除"}
                }, res =>
                {
                    if (res.Result == ButtonResult.OK)
                    {
                        RequestClose?.Invoke(new DialogResult(ButtonResult.Abort));
                    }
                });

            });
            SubmitCommand.Subscribe(_ =>
            {
                RequestClose?.Invoke(new DialogResult(ButtonResult.OK, new DialogParameters()
                {
                    { "Player", new Roaster.Player(Id.Value,Name.Value,isLibero.Value,isCaptain.Value) }
                }));
            });
        }
        public ReactiveCommand DeleteCommand { get; set; } = new();
        public ReactiveCommand SubmitCommand { get; set; } = new();
        public ReactiveProperty<int> Id { get; set; } = new();
        public ReactiveProperty<string?> Name { get; set; } = new();
        public ReactiveProperty<bool> isLibero { get; set; } = new();
        public ReactiveProperty<bool> isCaptain { get; set; } = new();

        public string Title => "";

        public event Action<IDialogResult>? RequestClose;

        public bool CanCloseDialog() => true;

        public void OnDialogClosed()
        {

        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            if (parameters.TryGetValue("Player", out Roaster.Player player))
            {
                Id.Value = (int)player.Id!;
                Name.Value = player.Name;
                isLibero.Value = player.IsLibero;
                isCaptain.Value = player.IsCaptain;
            }
        }
    }
}
