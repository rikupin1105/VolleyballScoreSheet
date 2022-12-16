using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using Reactive.Bindings;

namespace VolleyballScoreSheet.ViewModels.Dialog
{
    public class NotificationDialogViewModel : IDialogAware
    {
        /// <summary>
        /// Close command.
        /// </summary>
        public ReactiveCommand CloseDialogCommand { get; set; } = new();

        public ReactiveProperty<string> Message { get; set; } = new("");
        public ReactiveProperty<string> ButtonText { get; set; } = new("OK");
        public ReactiveProperty<string> Title { get; set; } = new("");

        string IDialogAware.Title => Title.Value;


        public NotificationDialogViewModel()
        {
            CloseDialogCommand.Subscribe(_ => RequestClose?.Invoke(new DialogResult(ButtonResult.OK)));
        }
        public event Action<IDialogResult>?RequestClose;


        public bool CanCloseDialog() => true;
        public void OnDialogClosed() { }
        public void OnDialogOpened(IDialogParameters parameters)
        {
            // Configure parameters
            if (parameters.TryGetValue("Title", out string title))
            {
                Title.Value = title;
            }
            if (parameters.TryGetValue("Message", out string message))
            {
                Message.Value = message;
            }
            if (parameters.TryGetValue("ButtonText", out string buttonText))
            {
                ButtonText.Value = buttonText;
            }
        }
    }
}
