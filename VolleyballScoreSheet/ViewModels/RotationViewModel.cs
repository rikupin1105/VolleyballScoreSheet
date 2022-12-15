using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Regions;
using Prism.Services.Dialogs;
using Reactive.Bindings;
using System.Windows;
using System.Data;
using VolleyballScoreSheet;
using Reactive.Bindings.Extensions;

namespace VolleyballScoreSheet.ViewModels
{
    public class RotationViewModel : IDialogAware, INavigationAware
    {
        public string Title => "";
        public event Action<IDialogResult>?RequestClose;
        public bool CanCloseDialog() => true;
        public void OnDialogClosed()
        {

        }
        public void OnDialogOpened(IDialogParameters parameters) { }

        private readonly IDialogService _dialogService;
        private readonly IRegionManager _regionManager;
        private readonly Game _game;
        public RotationViewModel(Game game, IRegionManager regionManager, IDialogService dialogService)
        {
            _game = game;
            _regionManager = regionManager;
            _dialogService = dialogService;

            NextCommand.Subscribe(_ => Next());

            LeftPlayer.Value.Clear();
            RightPlayer.Value.Clear();
            LeftPlayer.Value.Columns.Add("Number");
            LeftPlayer.Value.Columns.Add("Name");
            RightPlayer.Value.Columns.Add("Number");
            RightPlayer.Value.Columns.Add("Name");

            foreach (var item in _game.ATeam.Value.Players)
            {
                if (_game.isATeamLeft.Value)
                {
                    var row = LeftPlayer.Value.NewRow();
                    row[0] = item.Id;
                    row[1] = item.Name;
                    LeftPlayer.Value.Rows.Add(row);
                }
                else
                {
                    var row = RightPlayer.Value.NewRow();
                    row[0] = item.Id;
                    row[1] = item.Name;
                    RightPlayer.Value.Rows.Add(row);
                }
            }

            foreach (var item in _game.BTeam.Value.Players)
            {
                if (_game.isATeamLeft.Value)
                {
                    var row = RightPlayer.Value.NewRow();
                    row[0] = item.Id;
                    row[1] = item.Name;
                    RightPlayer.Value.Rows.Add(row);
                }
                else
                {
                    var row = LeftPlayer.Value.NewRow();
                    row[0] = item.Id;
                    row[1] = item.Name;
                    LeftPlayer.Value.Rows.Add(row);
                }
            }


            _game.ATeam.Value.Name.Subscribe(x =>
            {
                if (_game.isATeamLeft.Value)
                {
                    LeftSideTeamName.Value=x;
                }
                else
                {
                    RightSideTeamName.Value=x;
                }
            });
            _game.BTeam.Value.Name.Subscribe(x =>
            {
                if (_game.isATeamLeft.Value)
                {
                    RightSideTeamName.Value=x;
                }
                else
                {
                    LeftSideTeamName.Value=x;
                }
            });
        }
        public ReactiveCommand NextCommand { get; } = new ReactiveCommand();
        public ReactiveProperty<string> ATeamName { get; }
        public ReactiveProperty<string> BTeamName { get; }

        public ReactiveProperty<int?[]> LeftTeamRotatiton { get; } = new ReactiveProperty<int?[]>(new int?[6]);
        public ReactiveProperty<int?[]> RightTeamRotatiton { get; } = new ReactiveProperty<int?[]>(new int?[6]);
        public ReactiveProperty<DataTable> LeftPlayer { get; set; } = new ReactiveProperty<DataTable>(new DataTable());
        public ReactiveProperty<DataTable> RightPlayer { get; set; } = new ReactiveProperty<DataTable>(new DataTable());
        public ReactiveProperty<string> LeftSideTeamName { get; set; } = new();
        public ReactiveProperty<string> RightSideTeamName { get; set; } = new();



        private void Next()
        {
            var duplicateFlag = false;
            var duplicateErrorSentence = string.Empty;

            var unregisteredFlag = false;
            var unregisteredErrorSentence = string.Empty;

            //重複チェック
            for (int i = 0; i < LeftTeamRotatiton.Value.Length; i++)
            {
                for (int j = i + 1; j < LeftTeamRotatiton.Value.Length; j++)
                {
                    if (LeftTeamRotatiton.Value[i] == LeftTeamRotatiton.Value[j])
                    {
                        duplicateFlag = true;
                        duplicateErrorSentence += $"{LeftSideTeamName.Value} の {LeftTeamRotatiton.Value[i]} が重複しています。\n";
                        break;
                    }
                }

            }

            //重複チェック
            for (int i = 0; i<RightTeamRotatiton.Value.Length; i++)
            {
                for (int j = i + 1; j<RightTeamRotatiton.Value.Length; j++)
                {
                    if (RightTeamRotatiton.Value[i] == RightTeamRotatiton.Value[j])
                    {
                        duplicateFlag = true;
                        duplicateErrorSentence += $"{RightSideTeamName.Value} の {RightTeamRotatiton.Value[i]} が重複しています。\n";
                        break;
                    }
                }
            }

            //未登録チェック
            for (int i = 0; i < LeftTeamRotatiton.Value.Length; i++)
            {
                var register = false;
                foreach (var item in _game.LeftTeam.Players)
                {
                    if (LeftTeamRotatiton.Value[i] == item.Id)
                    {
                        register = true;
                        break;
                    }
                }
                if (!register)
                {
                    unregisteredFlag = true;
                    unregisteredErrorSentence += $"{LeftSideTeamName.Value} の {LeftTeamRotatiton.Value[i]} は登録されていません。\n";
                }
            }

            //未登録チェック
            for (int i = 0; i < RightTeamRotatiton.Value.Length; i++)
            {
                var register = false;
                foreach (var item in _game.RightTeam.Players)
                {
                    if (RightTeamRotatiton.Value[i] == item.Id)
                    {
                        register = true;
                        break;
                    }

                }
                if (!register)
                {
                    unregisteredFlag = true;
                    unregisteredErrorSentence += $"{RightSideTeamName.Value} の {RightTeamRotatiton.Value[i]} は登録されていません。\n";
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

            _game.LeftTeam.Sets[^1].StartingLineUp.Value = LeftTeamRotatiton.Value.Select(x => x!.Value).ToArray();
            _game.LeftTeam.StartingLineUp.Value = _game.LeftTeam.Sets[^1].StartingLineUp.Value;
            _game.RightTeam.Sets[^1].StartingLineUp.Value = RightTeamRotatiton.Value.Select(x => x!.Value).ToArray();
            _game.RightTeam.StartingLineUp.Value = _game.RightTeam.Sets[^1].StartingLineUp.Value;

            var param = new DialogParameters
            {
                { "LeftTeamRotation", LeftTeamRotatiton.Value.Select(x=>x!.Value).ToArray() },
                { "RightTeamRotation", RightTeamRotatiton.Value.Select(x=>x!.Value).ToArray() }
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
