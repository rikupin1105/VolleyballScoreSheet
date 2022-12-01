using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reactive.Bindings;
using Prism.Services.Dialogs;
using VolleyballScoreSheet.Model;
using System.Data;
using VolleyballScoreSheet;
using Reactive.Bindings.Extensions;

namespace VolleyballScoreSheet.ViewModels
{
    public class BeforeMatchViewModel : INavigationAware
    {
        public void OnNavigatedTo(NavigationContext navigationContext) { }
        public void OnNavigatedFrom(NavigationContext navigationContext) { }
        public bool IsNavigationTarget(NavigationContext navigationContext) => false;

        public ReactiveProperty<DataTable> LeftTeamPlayer { get; set; } = new ReactiveProperty<DataTable>(new DataTable());
        public ReactiveProperty<DataTable> RightTeamPlayer { get; set; } = new ReactiveProperty<DataTable>(new DataTable());

        public ReactiveProperty<bool> DisplayBeforeMatch { get; set; }

        public ReactiveProperty<int[]> LeftSideRotation { get; set; } = new();
        public ReactiveProperty<int[]> RightSideRotation { get; set; } = new();
        public ReactiveCommand LineUpCommand { get; } = new ReactiveCommand();

        private void LineUp()
        {
            _dialogService.ShowDialog("Rotation", new DialogParameters(), (result) =>
             {
                 result.Parameters.TryGetValue("LeftTeamRotation", out int[] a);
                 result.Parameters.TryGetValue("RightTeamRotation", out int[] b);

                  if (a is null || b is null)
                 {
                     return;
                 }

                 _game.LeftTeam.Sets[^1].Rotation.Value = a;
                 _game.RightTeam.Sets[^1].Rotation.Value = b;

                 _game.ATeam.Value.Refresh();
                 _game.BTeam.Value.Refresh();

                 _game.HistoryAdd("S"+_game.Set.Value);
                 _game.DisplayMain("Rotation");

             }, "DialogWindow");
        }
        private readonly IDialogService _dialogService;
        private readonly Game _game;
        public BeforeMatchViewModel(Game game, IDialogService dialogService)
        {
            _game = game;
            _dialogService = dialogService;

            LeftTeamPlayer.Value.Clear();
            LeftTeamPlayer.Value.Columns.Add("Number");
            LeftTeamPlayer.Value.Columns.Add("Name");

            RightTeamPlayer.Value.Clear();
            RightTeamPlayer.Value.Columns.Add("Number");
            RightTeamPlayer.Value.Columns.Add("Name");

            DisplayBeforeMatch = _game.ToReactivePropertyAsSynchronized(x => x.DisplayBeforeMatch.Value);

            LineUpCommand.Subscribe(_ => LineUp());
        }
    }
}
