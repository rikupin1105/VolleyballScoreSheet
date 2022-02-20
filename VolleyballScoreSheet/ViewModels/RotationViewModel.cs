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

        private readonly IDialogService _dialogService;
        private readonly Game _game;
        private readonly IRegionManager _regionManager;
        public RotationViewModel(IRegionManager regionManager, Game game, IDialogService dialogService)
        {
            _game = game;
            _regionManager = regionManager;
            _dialogService = dialogService;
            ATeam.Value = _game.ATeam;
            BTeam.Value = _game.BTeam;
            NextCommand.Subscribe(_ => Next());

            ATeamPlayer.Value.Clear();
            BTeamPlayer.Value.Clear();
            ATeamPlayer.Value.Columns.Add("Number");
            ATeamPlayer.Value.Columns.Add("Name");
            BTeamPlayer.Value.Columns.Add("Number");
            BTeamPlayer.Value.Columns.Add("Name");

            foreach (var item in _game.ATeamPlayers)
            {
                var row1 = ATeamPlayer.Value.NewRow();
                row1[0] = item.Id;
                row1[1] = item.Name;
                ATeamPlayer.Value.Rows.Add(row1);
            }

            foreach (var item in _game.BTeamPlayers)
            {
                var row2 = BTeamPlayer.Value.NewRow();
                row2[0] = item.Id;
                row2[1] = item.Name;
                BTeamPlayer.Value.Rows.Add(row2);
            }
        }
        public ReactiveCommand NextCommand { get; } = new ReactiveCommand();
        public ReactiveProperty<string> ATeam { get; } = new ReactiveProperty<string>();
        public ReactiveProperty<string> BTeam { get; } = new ReactiveProperty<string>();
        public ReactiveProperty<int[]> ATeamRotation { get; set; } = new ReactiveProperty<int[]>(new int[6]);
        public ReactiveProperty<int[]> BTeamRotation { get; set; } = new ReactiveProperty<int[]>(new int[6]);
        public ReactiveProperty<DataTable> ATeamPlayer { get; set; } = new ReactiveProperty<DataTable>(new DataTable());
        public ReactiveProperty<DataTable> BTeamPlayer { get; set; } = new ReactiveProperty<DataTable>(new DataTable());
        private void Next()
        {
            var duplicateFlag = false;
            var duplicateErrorSentence = string.Empty;

            var unregisteredFlag = false;
            var unregisteredErrorSentence = string.Empty;

            //重複チェック
            for (int i = 0; i < ATeamRotation.Value.Length; i++)
            {
                for (int j = i + 1; j < ATeamRotation.Value.Length; j++)
                {
                    if (ATeamRotation.Value[i] == ATeamRotation.Value[j])
                    {
                        duplicateFlag = true;
                        duplicateErrorSentence += $"{_game.ATeam} の {ATeamRotation.Value[i]} が重複しています。\n";
                        break;
                    }
                }

            }

            //重複チェック
            for (int i = 0; i<BTeamRotation.Value.Length; i++)
            {
                for (int j = i + 1; j<BTeamRotation.Value.Length; j++)
                {
                    if (BTeamRotation.Value[i] == BTeamRotation.Value[j])
                    {
                        duplicateFlag = true;
                        duplicateErrorSentence += $"{_game.BTeam} の {BTeamRotation.Value[i]} が重複しています。\n";
                        break;
                    }
                }
            }

            //未登録チェック
            for (int i = 0; i < ATeamRotation.Value.Length; i++)
            {
                var register = false;
                foreach (var item in _game.ATeamPlayers)
                {
                    if (ATeamRotation.Value[i] == item.Id)
                    {
                        register = true;
                        break;
                    }
                }
                if (!register)
                {
                    unregisteredFlag = true;
                    unregisteredErrorSentence += $"{_game.ATeam} の {ATeamRotation.Value[i]} は登録されていません。\n";
                }
            }

            //未登録チェック
            for (int i = 0; i < BTeamRotation.Value.Length; i++)
            {
                var register = false;
                foreach (var item in _game.BTeamPlayers)
                {
                    if (BTeamRotation.Value[i] == item.Id)
                    {
                        register = true;
                        break;
                    }

                }
                if (!register)
                {
                    unregisteredFlag = true;
                    unregisteredErrorSentence += $"{_game.BTeam} の {BTeamRotation.Value[i]} は登録されていません。\n";
                }
            }

            if (duplicateFlag)
            {
                _dialogService.ShowDialog(
                     "NotificationDialog",
                     new DialogParameters
                     {
                         { "Title", "Alert" },
                         { "Message", duplicateErrorSentence},
                         { "ButtonText", "OK" }
                     }, res =>
                     {

                     }, "AlertWindow");
                return;
            }

            if (unregisteredFlag)
            {
                _dialogService.ShowDialog(
                     "NotificationDialog",
                     new DialogParameters
                     {
                         { "Title", "Alert" },
                         { "Message", unregisteredErrorSentence},
                         { "ButtonText", "OK" }
                     }, res =>
                     {

                     }, "AlertWindow");
                return;
            }

            var param = new DialogParameters
            {
                { "ATeamRotation", ATeamRotation.Value },
                { "BTeamRotation", BTeamRotation.Value }
            };

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
