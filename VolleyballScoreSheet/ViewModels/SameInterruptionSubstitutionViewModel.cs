using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;

namespace VolleyballScoreSheet.ViewModels
{
	public class SameInterruptionSubstitutionViewModel : BindableBase , IDialogAware
	{
        public ReactiveCommand RejectCommand { get; set; } = new();
        public ReactiveCommand ApprovalCommand { get; set; } = new();

        public ReactiveProperty<string> Message { get; set; } = new("");
        public ReactiveProperty<string> ButtonText { get; set; } = new("OK");
        public ReactiveProperty<string> Title { get; set; } = new("");
        string IDialogAware.Title => Title.Value;

        public SameInterruptionSubstitutionViewModel()
        {
            RejectCommand.Subscribe(_ =>
            {
                RequestClose?.Invoke(new DialogResult(ButtonResult.Abort));
            });
            ApprovalCommand.Subscribe(_ =>
            {
                RequestClose?.Invoke(new DialogResult(ButtonResult.OK));
            });
        }
        public event Action<IDialogResult> RequestClose;


        public bool CanCloseDialog() => true;
        public void OnDialogClosed() { }
        public void OnDialogOpened(IDialogParameters parameters)
        {
            if (parameters.TryGetValue("Title", out string title))
            {
                Title.Value = title;
            }
            if (parameters.TryGetValue("Message", out string message))
            {
                Message.Value = message;
            }
        }
    }
}
