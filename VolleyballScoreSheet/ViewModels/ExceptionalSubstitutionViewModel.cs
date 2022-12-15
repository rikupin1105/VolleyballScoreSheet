using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Services.Dialogs;
using Reactive.Bindings;

namespace VolleyballScoreSheet.ViewModels
{
    public class ExceptionalSubstitutionViewModel : IDialogAware
    {
        public ReactiveCommand ExceptionalSubstitutionCommand { get; } = new();
        public ReactiveCommand RejectSubstitutionCommand { get; } = new();
        public ReactiveCommand CloseCommand { get; } = new();
        public ExceptionalSubstitutionViewModel()
        {
            CloseCommand.Subscribe(_ => RequestClose.Invoke(new DialogResult(ButtonResult.Cancel)));
            ExceptionalSubstitutionCommand.Subscribe(_ => RequestClose.Invoke(new DialogResult(ButtonResult.Retry)));
            RejectSubstitutionCommand.Subscribe(_ => RequestClose.Invoke(new DialogResult(ButtonResult.No)));
        }
        public string Title => "";

        public event Action<IDialogResult>?RequestClose;

        public bool CanCloseDialog() => true;
        public void OnDialogClosed() { }
        public void OnDialogOpened(IDialogParameters parameters) { }
    }
}
