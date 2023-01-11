using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;

namespace VolleyballScoreSheet.ViewModels
{
    public class RefereesViewModel : BindableBase , IDialogAware
    {
        private readonly Game _game;
        public Referees Referees { get; set; }
        public ReactiveCommand SaveAndCloseCommand { get; set; } = new();
        public RefereesViewModel(Game game)
        {
            _game = game;
            Referees = _game.Referees;
            
            SaveAndCloseCommand.Subscribe(_ =>
            {
                _game.Referees = Referees;
                RequestClose.Invoke(new DialogResult());
            });
        }

        public string Title => "";

        public event Action<IDialogResult>? RequestClose;

        public bool CanCloseDialog() => true;

        public void OnDialogClosed() 
        {
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
        }
    }
}
