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
using Reactive.Bindings.Extensions;
using System.Reactive.Linq;
using VolleyballScoreSheet.Model;
using System.Collections.ObjectModel;

namespace VolleyballScoreSheet.ViewModels
{
    public class RotationViewModel : IDialogAware, INavigationAware
    {
        public string Title => "";
        public event Action<IDialogResult>? RequestClose;
        public bool CanCloseDialog() => true;
        public void OnDialogClosed() { }

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

            LeftPlayer.AddRange(_game.LeftTeam.Players.OrderBy(x => x.IsLibero));
            RightPlayer.AddRange(_game.RightTeam.Players.OrderBy(x => x.IsLibero));

            _game.LeftTeam.Name.Subscribe(x => { LeftSideTeamName.Value = x; });
            _game.RightTeam.Name.Subscribe(x => { RightSideTeamName.Value = x; });
        }
        public ReactiveCommand NextCommand { get; } = new ReactiveCommand();
        public ReactiveProperty<string> ATeamName { get; }
        public ReactiveProperty<string> BTeamName { get; }

        public ReactiveProperty<int?[]> LeftTeamRotatiton { get; } = new ReactiveProperty<int?[]>(new int?[6]);
        public ReactiveProperty<int?[]> RightTeamRotatiton { get; } = new ReactiveProperty<int?[]>(new int?[6]);
        public ReactiveCollection<Player> LeftPlayer { get; set; } = new ReactiveCollection<Player>();
        public ReactiveCollection<Player> RightPlayer { get; set; } = new ReactiveCollection<Player>();
        public ReactiveProperty<string> LeftSideTeamName { get; set; } = new();
        public ReactiveProperty<string> RightSideTeamName { get; set; } = new();



        private void Next()
        {
            var errorMessageFlag = false;
            var errorMessage = string.Empty;

            //未入力
            if (LeftTeamRotatiton.Value.Any(x => x == null))
            {
                return;
            }
            if (RightTeamRotatiton.Value.Any(x => x == null))
            {
                return;
            }

            //リベロチェック
            if (LeftTeamRotatiton.Value.Where(x => _game.LeftTeam.Players.Where(x => x.IsLibero).Select(x => x.Id).ToList().Contains((int)x)).Count() != 0)
            {
                errorMessageFlag = true;
                errorMessage += $"{_game.LeftTeam.Name.Value}にリベロが含まれています。\n";
            }
            if (RightTeamRotatiton.Value.Where(x => _game.RightTeam.Players.Where(x => x.IsLibero).Select(x => x.Id).ToList().Contains((int)x)).Count() != 0)
            {
                errorMessageFlag = true;
                errorMessage += $"{_game.RightTeam.Name.Value}にリベロが含まれています。\n";
            }

            //重複チェック
            if (LeftTeamRotatiton.Value.DistinctBy(x => x).Count() != 6)
            {
                errorMessageFlag = true;
                errorMessage += $"{_game.LeftTeam.Name.Value}の選手が重複しています。\n";
            }
            if (RightTeamRotatiton.Value.DistinctBy(x => x).Count() != 6)
            {
                errorMessageFlag = true;
                errorMessage += $"{_game.RightTeam.Name.Value}の選手が重複しています。\n";
            }

            foreach (var item in LeftTeamRotatiton.Value)
            {
                if (!_game.LeftTeam.Players.Any(x => x.Id == item))
                {
                    errorMessageFlag = true;
                    errorMessage += $"{_game.LeftTeam.Name.Value}の{item}番は登録されていません。\n";
                }
            }

            foreach (var item in RightTeamRotatiton.Value)
            {
                if (!_game.RightTeam.Players.Any(x => x.Id == item))
                {
                    errorMessageFlag = true;
                    errorMessage += $"{_game.RightTeam.Name.Value}の{item}番は登録されていません。\n";
                }
            }

            ////未登録チェック
            //for (int i = 0; i < LeftTeamRotatiton.Value.Length; i++)
            //{
            //    var register = false;
            //    foreach (var item in _game.LeftTeam.Players.Where(x => x.IsLibero==false))
            //    {
            //        if (LeftTeamRotatiton.Value[i] == item.Id)
            //        {
            //            register = true;
            //            break;
            //        }
            //    }
            //    if (!register)
            //    {
            //        unregisteredFlag = true;
            //        unregisteredErrorSentence += $"{LeftSideTeamName.Value} の {LeftTeamRotatiton.Value[i]} は登録されていません。\n";
            //    }
            //}

            ////未登録チェック
            //for (int i = 0; i < RightTeamRotatiton.Value.Length; i++)
            //{
            //    var register = false;
            //    foreach (var item in _game.RightTeam.Players.Where(x=>x.IsLibero==false))
            //    {
            //        if (RightTeamRotatiton.Value[i] == item.Id)
            //        {
            //            register = true;
            //            break;
            //        }

            //    }
            //    if (!register)
            //    {
            //        unregisteredFlag = true;
            //        unregisteredErrorSentence += $"{RightSideTeamName.Value} の {RightTeamRotatiton.Value[i]} は登録されていません。\n";
            //    }
            //}



            if (errorMessageFlag)
            {
                _dialogService.ShowDialog(
                     "NotificationDialog",
                     new DialogParameters
                     {
                         { "Title", "警告" },
                         { "Message", errorMessage},
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

            RequestClose?.Invoke(new DialogResult(ButtonResult.OK, param));
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
