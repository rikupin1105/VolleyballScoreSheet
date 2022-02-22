﻿using System;
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
        public SubstitutionViewModel(Game game, IDialogService dialogService)
        {
            _game = game;
            _dialogService = dialogService;
            CancelCommand.Subscribe(_ => RequestClose.Invoke(new DialogResult(ButtonResult.Cancel)));
            SubstitutionCommand.Subscribe(_ => Substitution());
            SelectionChangedCommand.Subscribe(_ => SelectionChanged());
        }
        public void Substitution()
        {
            var flag = true;
            if (TeamName.Value == _game.ATeam)
            {
                //ATeam
            }
            else
            {
                //BTeam
            }

            if (flag &&OutMember != null && InMember !=null)
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
        public void Dialog(string message)
        {
            _dialogService.ShowDialog(
                    "NotificationDialog",
                    new DialogParameters
                    {
                        { "Title", "Alert" },
                        { "Message", message },
                        { "ButtonText", "OK" }
                    }, res =>
                    {

                    }, "AlertWindow");
        }



        public void OnDialogOpened(IDialogParameters parameters)
        {
            if (parameters.TryGetValue("Team", out string team))
            {
                TeamName.Value = team;
                if (team==_game.ATeam)
                {
                    //ATeam
                    var changeble = _game.Sets[^1].ATeamSubstitution.Select(x => x.OutMember);
                    OnCourtMemberItem.Value = _game.Sets[^1].ATeamRotation
                        .Where(x => !changeble.Contains(x)).ToArray();

                    //OutCourtMemberItem.Value = _game.ATeamPlayers.Select(x => x.Id).Where(x => !_game.Sets[^1].ATeamRotation.Contains(x)).ToArray();
                }
                else
                {
                    //BTeam
                    OnCourtMemberItem.Value = _game.Sets[^1].BTeamRotation;
                    //OutCourtMemberItem.Value = _game.BTeamPlayers.Select(x => x.Id).Where(x => !_game.Sets[^1].BTeamRotation.Contains(x)).ToArray();
                }
            }
        }
        public void SelectionChanged()
        {
            if (TeamName.Value == _game.ATeam)
            {
                //入ったことがある人
                var a = _game.Sets[^1].ATeamSubstitution
                    .Where(x => x.Set==_game.Sets[^1].GameSet)
                    .Select(x => x.InMember)
                    .ToList();

                if (a.Contains(int.Parse(OutMember)))
                {
                    var innable = _game.Sets[^1].ATeamSubstitution
                        .Where(x => x.Set==_game.Sets[^1].GameSet)
                        .Where(x => x.InMember==int.Parse(OutMember))
                        .Select(x => x.OutMember);

                    OutCourtMemberItem.Value = innable.ToArray();
                }
                else
                {
                    //一度でも出たことがある人
                    var outmember = _game.Sets[^1].ATeamSubstitution
                        .Where(x => x.Set==_game.Sets[^1].GameSet
                        ).Select(x => x.OutMember)
                        .ToList();

                    OutCourtMemberItem.Value = _game.ATeamPlayers.Select(x => x.Id).Where(x => !_game.Sets[^1].ATeamRotation.Contains(x))
                        .Where(x => !a.Contains(x))
                        .Where(x => !outmember.Contains(x))
                        .ToArray();
                }
            }
        }
        public ReactiveCommand SelectionChangedCommand { get; } = new();
        public ReactiveCommand CancelCommand { get; } = new();
        public ReactiveCommand SubstitutionCommand { get; } = new();
        public ReactiveProperty<string> TeamName { get; set; } = new();
        public ReactiveProperty<int[]> OnCourtMemberItem { get; set; } = new();
        public ReactiveProperty<int[]> OutCourtMemberItem { get; set; } = new();
        public string Title => "メンバーチェンジ";
        public event Action<IDialogResult> RequestClose;
        public bool CanCloseDialog() => true;
        public void OnDialogClosed() { }
    }
}
