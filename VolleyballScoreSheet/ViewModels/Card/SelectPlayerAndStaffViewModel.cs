using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using VolleyballScoreSheet.Views.Card;

namespace VolleyballScoreSheet.ViewModels.Card
{
    public class SelectPlayerAndStaffViewModel : BindableBase, IDialogAware
    {
        private readonly Game _game;
        public SelectPlayerAndStaffViewModel(Game game)
        {
            _game=game;
            CancelCommand.Subscribe(_ => RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel)));
            SubmitCommand.Subscribe(_ => YellowCard());

        }
        private void YellowCard()
        {
            var mark = "";

            if(string.IsNullOrEmpty(SelectedItem.Value))
            {
                return;
            }

            if (SelectedItem.Value=="監督") mark = "C";
            else if (SelectedItem.Value=="コーチ") mark = "AC";
            else if (SelectedItem.Value=="マネージャー") mark = "M";
            else if (SelectedItem.Value=="部長") mark = "H";
            else mark = SelectedItem.Value;

            RequestClose?.Invoke(new DialogResult(ButtonResult.OK, new DialogParameters() { { "Mark", mark } }));
        }
        public ReactiveProperty<string> SelectedItem { get; set; } = new();
        public ReactiveCommand CancelCommand { get; set; } = new();
        public ReactiveCommand SubmitCommand { get; set; } = new();
        public List<string> PlayerAndStaff { get; set; } = new();

        public string Title => "";

        public event Action<IDialogResult>? RequestClose;

        public bool CanCloseDialog() => true;

        public void OnDialogClosed()
        {
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            if (parameters.TryGetValue("isLeft", out bool isLeft))
            {
                if (isLeft)
                {
                    PlayerAndStaff.AddRange(_game.LeftTeam.Players.Select(x=>x.Id.ToString()));
                }
                else
                {
                    PlayerAndStaff.AddRange(_game.RightTeam.Players.Select(x=>x.Id.ToString()));
                }

                PlayerAndStaff.Add("監督");
                PlayerAndStaff.Add("マネージャー");
                PlayerAndStaff.Add("コーチ");
                PlayerAndStaff.Add("部長");
            }
        }
    }
}
