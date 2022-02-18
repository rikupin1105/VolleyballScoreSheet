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

namespace VolleyballScoreSheet.ViewModels
{
    public class BeforeMatchViewModel : INavigationAware
    {
        public void OnNavigatedTo(NavigationContext navigationContext) { }
        public void OnNavigatedFrom(NavigationContext navigationContext) { }
        public bool IsNavigationTarget(NavigationContext navigationContext) => false;

        public ReactiveProperty<DataTable> LeftTeamPlayer { get; set; } = new ReactiveProperty<DataTable>(new DataTable());
        public ReactiveProperty<DataTable> RightTeamPlayer { get; set; } = new ReactiveProperty<DataTable>(new DataTable());

        public ReactiveProperty<string> Set { get; set; } = new();
        public ReactiveProperty<string> LeftSideTeamName { get; set; } = new ReactiveProperty<string>();
        public ReactiveProperty<string> RightSideTeamName { get; set; } = new ReactiveProperty<string>();
        public ReactiveProperty<int> LeftSidePoints { get; set; } = new ReactiveProperty<int>();
        public ReactiveProperty<int> RightSidePoints { get; set; } = new ReactiveProperty<int>();
        public ReactiveProperty<int> LeftSideSets { get; set; } = new ReactiveProperty<int>();
        public ReactiveProperty<int> RightSideSets { get; set; } = new ReactiveProperty<int>();
        public ReactiveCommand LineUpCommand { get; } = new ReactiveCommand();
        public ReactiveProperty<int[]> LeftSideLotation { get; set; } = new();
        public ReactiveProperty<int[]> RightSideLotation { get; set; } = new();
        private void LineUp()
        {
            _dialogService.ShowDialog("Rotation", new DialogParameters(), (result) =>
             {
                 result.Parameters.TryGetValue("ATeamRotation", out int[] a);
                 result.Parameters.TryGetValue("BTeamRotation", out int[] b);

                 if (_game.CoinToss.ATeamLeftSide)
                 {
                     LeftSideLotation.Value = a;
                     RightSideLotation.Value = b;
                 }
                 else
                 {
                     LeftSideLotation.Value = b;
                     RightSideLotation.Value = a;
                 }
             }, "DialogWindow");



            var set = new Set();

            if (_game.CoinToss.ATeamLeftSide)
            {
                set.ATeamRotation = LeftSideLotation.Value;
                set.BTeamRotation = RightSideLotation.Value;

                set.ATeamRightSide = false;
                LeftSideTeamName.Value=_game.ATeam;
                RightSideTeamName.Value=_game.BTeam;
            }
            else
            {
                set.ATeamRotation = RightSideLotation.Value;
                set.BTeamRotation = LeftSideLotation.Value;

                set.ATeamRightSide = true;
                LeftSideTeamName.Value=_game.BTeam;
                RightSideTeamName.Value=_game.ATeam;
            }
            if (_game.CoinToss.ATeamServer)
            {
                set.ATeamServer = true;
            }
            else
            {
                set.ATeamServer = false;
            }

            _ = _game.CreateSet(set);

            Navigate("Gaming");
        }
        private readonly IDialogService _dialogService;
        private readonly IRegionManager _regionManager;
        private readonly Game _game;
        public BeforeMatchViewModel(IRegionManager regionManager, Game game, IDialogService dialogService)
        {
            _game = game;
            _regionManager = regionManager;
            _dialogService = dialogService;

            LeftTeamPlayer.Value.Clear();
            LeftTeamPlayer.Value.Columns.Add("Number");
            LeftTeamPlayer.Value.Columns.Add("Name");

            foreach (var item in _game.ATeamPlayers)
            {
                var row1 = LeftTeamPlayer.Value.NewRow();
                row1[0] = item.Id;
                row1[1] = item.Name;
                LeftTeamPlayer.Value.Rows.Add(row1);
            }

            RightTeamPlayer.Value.Clear();
            RightTeamPlayer.Value.Columns.Add("Number");
            RightTeamPlayer.Value.Columns.Add("Name");
            foreach (var item in _game.BTeamPlayers)
            {
                var row2 = RightTeamPlayer.Value.NewRow();
                row2[0] = item.Id;
                row2[1] = item.Name;
                RightTeamPlayer.Value.Rows.Add(row2);
            }


            LeftSideTeamName.Value = _game.ATeam;
            RightSideTeamName.Value = _game.BTeam;

            Set.Value = $"SET {_game.Sets.Count + 1}";

            LeftSideSets.Value = _game.ATeamSet;
            RightSideSets.Value = _game.BTeamSet;

            LineUpCommand.Subscribe(_ => LineUp());
        }
        private void Navigate(string navigatePath)
        {
            if (navigatePath != null)
                _regionManager.RequestNavigate("ContentRegion", navigatePath);
        }


    }
}
