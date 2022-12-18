using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using VolleyballScoreSheet.Model;
using VolleyballScoreSheet.Views.Card;

namespace VolleyballScoreSheet.ViewModels.Card
{
    public enum SelectEnum
    {
        Player,
        OnCourtPlayer,
        Staff,
        Libero,
        All
    }
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

            if (string.IsNullOrEmpty(SelectedItem.Value))
            {
                return;
            }

            else mark = SelectedItem.Value.Split(' ')[0];

            RequestClose?.Invoke(new DialogResult(ButtonResult.OK, new DialogParameters() { { "Mark", mark } }));
        }
        public ReactiveProperty<string> SelectedItem { get; set; } = new();
        public ReactiveCommand CancelCommand { get; set; } = new();
        public ReactiveCommand SubmitCommand { get; set; } = new();
        public List<string> PlayerAndStaff { get; set; } = new();
        public ReactiveProperty<string> Message { get; set; } = new();

        public string Title => "";

        public event Action<IDialogResult>? RequestClose;

        public bool CanCloseDialog() => true;

        public void OnDialogClosed()
        {
        }
        public void AAA(SelectEnum selectEnum,Team team)
        {
            switch (selectEnum)
            {
                case SelectEnum.Player:
                    PlayerAndStaff.AddRange(team.Players.Select(x => x.Id.ToString()+" "+x.Name));
                    break;
                case SelectEnum.Staff:
                    PlayerAndStaff.Add("C 監督");
                    PlayerAndStaff.Add("M マネージャー");
                    PlayerAndStaff.Add("AC コーチ");
                    PlayerAndStaff.Add("H 部長");
                    break;
                case SelectEnum.Libero:
                    break;
                case SelectEnum.All:
                    PlayerAndStaff.AddRange(team.Players.Select(x => x.Id.ToString()+" "+x.Name));
                    PlayerAndStaff.Add("C 監督");
                    PlayerAndStaff.Add("M マネージャー");
                    PlayerAndStaff.Add("AC コーチ");
                    PlayerAndStaff.Add("H 部長");
                    break;
                case SelectEnum.OnCourtPlayer:
                    PlayerAndStaff.AddRange(team.Sets[^1].Rotation.Value.OrderBy(x=>x).Select(x => x +" "+team.Players.Where(_=>_.Id==x).Select(x=>x.Name).First()));
                    break;
            }
        }
        public void OnDialogOpened(IDialogParameters parameters)
        {
            if (parameters.TryGetValue("Message", out string message))
            {
                Message.Value = message;
            }
            if (parameters.TryGetValue("SelectEnum", out SelectEnum selectEnum))
            {

            }
            if (parameters.TryGetValue("isLeft", out bool isLeft))
            {
                if (isLeft)
                {
                    PlayerAndStaff.AddRange(_game.LeftTeam.Players.Select(x => x.Id.ToString()+" "+x.Name));
                }
                else
                {
                    PlayerAndStaff.AddRange(_game.RightTeam.Players.Select(x => x.Id.ToString()+" "+x.Name));
                }


                PlayerAndStaff.Add("C 監督");
                PlayerAndStaff.Add("M マネージャー");
                PlayerAndStaff.Add("AC コーチ");
                PlayerAndStaff.Add("H 部長");
            }
            else if (parameters.TryGetValue("isA", out bool isA))
            {
                if (parameters.TryGetValue("Players", out List<int> players))
                {
                    if (isA)
                    {
                        PlayerAndStaff.AddRange(players.Select(x => x+" "+_game.ATeam.Value.Players.Where(y=>x==y.Id).Select(x=>x.Name).First()));
                    }
                    else
                    {
                        PlayerAndStaff.AddRange(players.Select(x => x+" "+_game.BTeam.Value.Players.Where(y=>x==y.Id).Select(x=>x.Name).First()));
                    }
                }
                else
                {
                    if (isA)
                    {
                        AAA(selectEnum, _game.ATeam.Value);
                    }
                    else
                    {
                        AAA(selectEnum, _game.BTeam.Value);
                    }
                }
            }
        }
    }
}
