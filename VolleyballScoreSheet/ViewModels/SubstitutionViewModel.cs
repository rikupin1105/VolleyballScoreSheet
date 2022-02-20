using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Services.Dialogs;
using Reactive.Bindings;

namespace VolleyballScoreSheet.ViewModels
{
    public class SubstitutionViewModel : IDialogAware
    {
        private readonly IDialogService _dialogService;
        private readonly Game _game;
        public SubstitutionViewModel(Game game)
        {
            _game = game;
            CancelCommand.Subscribe(_ => RequestClose.Invoke(new DialogResult(ButtonResult.Cancel)));
            SubstitutionCommand.Subscribe(_ => Substitution());
        }
        public void Substitution()
        {
            if (OutMember != null && InMember !=null)
            {
                RequestClose.Invoke(new DialogResult(ButtonResult.OK, new DialogParameters
                {
                    { "Out", int.Parse(OutMember) },
                    { "In", int.Parse(InMember) }
                }));
            }
        }
        public string OutMember { get; set; }
        public string InMember { get; set; }



        public ReactiveCommand CancelCommand { get; } = new();
        public ReactiveCommand SubstitutionCommand { get; } = new();
        public ReactiveProperty<string> TeamName { get; set; } = new();

        public ReactiveProperty<int[]> OnCourtMemberItem { get; set; } = new();
        public ReactiveProperty<int[]> OutCourtMemberItem { get; set; } = new();

        public string Title => "メンバーチェンジ";
        public event Action<IDialogResult> RequestClose;
        public bool CanCloseDialog() => true;
        public void OnDialogClosed() { }



        public void OnDialogOpened(IDialogParameters parameters)
        {
            if (parameters.TryGetValue("Team", out string team))
            {
                TeamName.Value = team;
                if (team==_game.ATeam)
                {
                    //ATeam
                    OnCourtMemberItem.Value = _game.Sets[^1].ATeamRotation;
                    OutCourtMemberItem.Value = _game.ATeamPlayers.Select(x => x.Id).Where(x => !_game.Sets[^1].ATeamRotation.Contains(x)).ToArray();
                }
                else
                {
                    //BTeam
                    OnCourtMemberItem.Value = _game.Sets[^1].BTeamRotation;
                    OutCourtMemberItem.Value = _game.BTeamPlayers.Select(x => x.Id).Where(x => !_game.Sets[^1].BTeamRotation.Contains(x)).ToArray();
                }
            }
        }
    }
}
