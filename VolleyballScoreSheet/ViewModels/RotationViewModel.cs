using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Regions;
using Prism.Services.Dialogs;
using Reactive.Bindings;
using WPFUI.Theme;
using System.Windows;
using System.Data;
using VolleyballScoreSheet;

namespace VolleyballScoreSheet.ViewModels
{
    public class RotationViewModel : IDialogAware, INavigationAware
    {
        public string Title => "";
        public event Action<IDialogResult> RequestClose;
        public bool CanCloseDialog() => true;
        public void OnDialogClosed() { }
        public void OnDialogOpened(IDialogParameters parameters) { }


        private readonly Game _game;
        private readonly IRegionManager _regionManager;
        public RotationViewModel(IRegionManager regionManager, Game game)
        {
            _game = game;
            _regionManager = regionManager;
            ATeam.Value = _game.ATeam;
            BTeam.Value = _game.BTeam;
            NextCommand.Subscribe(_ => Next());

            LeftTeamPlayer.Value.Clear();
            RightTeamPlayer.Value.Clear();
            LeftTeamPlayer.Value.Columns.Add("Number");
            LeftTeamPlayer.Value.Columns.Add("Name");
            RightTeamPlayer.Value.Columns.Add("Number");
            RightTeamPlayer.Value.Columns.Add("Name");

            foreach (var item in _game.ATeamPlayers)
            {
                var row1 = LeftTeamPlayer.Value.NewRow();
                row1[0] = item.Id;
                row1[1] = item.Name;
                LeftTeamPlayer.Value.Rows.Add(row1);
            }

            foreach (var item in _game.ATeamPlayers)
            {
                var row2 = RightTeamPlayer.Value.NewRow();
                row2[0] = item.Id;
                row2[1] = item.Name;
                RightTeamPlayer.Value.Rows.Add(row2);
            }
        }
        public ReactiveCommand NextCommand { get; } = new ReactiveCommand();
        public ReactiveProperty<string> ATeam { get; } = new ReactiveProperty<string>();
        public ReactiveProperty<string> BTeam { get; } = new ReactiveProperty<string>();
        public ReactiveProperty<int[]> ATeamRotation { get; set; } = new ReactiveProperty<int[]>(new int[6]);
        public ReactiveProperty<int[]> BTeamRotation { get; set; } = new ReactiveProperty<int[]>(new int[6]);
        public ReactiveProperty<DataTable> LeftTeamPlayer { get; set; } = new ReactiveProperty<DataTable>(new DataTable());
        public ReactiveProperty<DataTable> RightTeamPlayer { get; set; } = new ReactiveProperty<DataTable>(new DataTable());
        private void Next()
        {
            var param = new DialogParameters
            {
                { "ATeamRotation", ATeamRotation.Value },
                { "BTeamRotation", BTeamRotation.Value }
            };

            //_game.Sets.Add(new Set(_game));

            RequestClose.Invoke(new DialogResult(ButtonResult.OK, param));
        }
        private void Navigate(string navigatePath)
        {
            if (navigatePath != null)
                _regionManager.RequestNavigate("ContentRegion", navigatePath);
        }

        public void OnNavigatedTo(NavigationContext navigationContext) { }

        public bool IsNavigationTarget(NavigationContext navigationContext) => false;

        public void OnNavigatedFrom(NavigationContext navigationContext) { }
    }
}
