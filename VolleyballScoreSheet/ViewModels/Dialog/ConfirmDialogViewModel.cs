using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;

namespace VolleyballScoreSheet.ViewModels.Dialog
{
    public class ConfirmDialogViewModel : BindableBase, IDialogAware
    {
        public ConfirmDialogViewModel()
        {
            CancelCommand.Subscribe(_ => RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel)));
            OKCommand.Subscribe(_ => RequestClose?.Invoke(new DialogResult(ButtonResult.OK)));
        }
        public ReactiveProperty<string> Text { get; set; } = new();
        public ReactiveProperty<string> Cancel { get; set; } = new();
        public ReactiveProperty<string> OK { get; set; } = new();
        public ReactiveCommand CancelCommand { get; set; } = new();
        public ReactiveCommand OKCommand { get; set; } = new();

        public string Title => "";

        public event Action<IDialogResult> RequestClose;

        public bool CanCloseDialog() => true;

        public void OnDialogClosed()
        {
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            if (parameters.TryGetValue("Text", out string text))
            {
                Text.Value = text;
            }
            if (parameters.TryGetValue("Cancel", out string cencel))
            {
                Cancel.Value = cencel;
            }
            if (parameters.TryGetValue("OK", out string ok))
            {
                OK.Value = ok;
            }
        }
    }
}
