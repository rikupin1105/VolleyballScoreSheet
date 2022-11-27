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
        public ReactiveProperty<int[]> LeftSideRotation { get; set; } = new();
        public ReactiveProperty<int[]> RightSideRotation { get; set; } = new();
        public ReactiveCommand LineUpCommand { get; } = new ReactiveCommand();

        private void LineUp()
        {
            _dialogService.ShowDialog("Rotation", new DialogParameters(), (result) =>
             {
                 result.Parameters.TryGetValue("ATeamRotation", out int[] a);
                 result.Parameters.TryGetValue("BTeamRotation", out int[] b);

                 if (_game.CoinToss!.ATeamLeftSide)
                 {
                     LeftSideRotation.Value = a;
                     RightSideRotation.Value = b;
                 }
                 else
                 {
                     LeftSideRotation.Value = b;
                     RightSideRotation.Value = a;
                 }
             }, "DialogWindow");

            _game.ATeam.Value.Sets.Add(new());
            _game.BTeam.Value.Sets.Add(new());

            if (_game.isATeamLeft.Value)
            {
                _game.ATeam.Value.Sets[^1].Rotation.Value =LeftSideRotation.Value;
                _game.BTeam.Value.Sets[^1].Rotation.Value =RightSideRotation.Value;
            }
            else
            {
                _game.ATeam.Value.Sets[^1].Rotation.Value =RightSideRotation.Value;
                _game.BTeam.Value.Sets[^1].Rotation.Value =LeftSideRotation.Value;
            }

            Navigate("Gaming");
        }
        private readonly IDialogService _dialogService;
        private readonly IRegionManager _regionManager;
        private readonly Game _game;
        public BeforeMatchViewModel(Game game,IRegionManager regionManager, IDialogService dialogService)
        {
            _game = game;
            _regionManager = regionManager;
            _dialogService = dialogService;

            LeftTeamPlayer.Value.Clear();
            LeftTeamPlayer.Value.Columns.Add("Number");
            LeftTeamPlayer.Value.Columns.Add("Name");

            RightTeamPlayer.Value.Clear();
            RightTeamPlayer.Value.Columns.Add("Number");
            RightTeamPlayer.Value.Columns.Add("Name");

            LineUpCommand.Subscribe(_ => LineUp());
        }
        private void Navigate(string navigatePath)
        {
            if (navigatePath != null)
                _regionManager.RequestNavigate("ContentRegion", navigatePath);
        }


    }
}
