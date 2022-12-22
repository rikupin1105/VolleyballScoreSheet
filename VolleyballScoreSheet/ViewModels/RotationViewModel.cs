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

            LeftPlayer.AddRange(_game.LeftTeam.Players
                .Where(x => x.IsDisqualified != true)
                .Where(x => x.IsLibero != true)
                .Where(x => x.IsExceptionalSubstituted != true)
                .OrderBy(x => x.Id));

            RightPlayer.AddRange(_game.RightTeam.Players
                .Where(x => x.IsDisqualified != true)
                .Where(x => x.IsLibero != true)
                .Where(x => x.IsExceptionalSubstituted != true)
                .OrderBy(x => x.Id));

            LeftSideTeamName =  _game.LeftTeam.Name.Value;
            RightSideTeamName =  _game.RightTeam.Name.Value;
        }
        public ReactiveCommand NextCommand { get; } = new ReactiveCommand();

        public int?[] LeftTeamRotatiton { get; } = new int?[6];
        public int?[] RightTeamRotatiton { get; } = new int?[6];
        public List<Player> LeftPlayer { get; set; } = new();
        public List<Player> RightPlayer { get; set; } = new();

        public string LeftSideTeamName { get; set; }
        public string RightSideTeamName { get; set; }

        private void Next()
        {
            var errorMessageFlag = false;
            var errorMessage = string.Empty;

            //未入力
            if (LeftTeamRotatiton.Any(x => x == null))
            {
                return;
            }
            if (RightTeamRotatiton.Any(x => x == null))
            {
                return;
            }

            //リベロチェック
            if (LeftTeamRotatiton.Where(x => _game.LeftTeam.Players.Where(x => x.IsLibero).Select(x => x.Id).ToList().Contains((int)x)).Any())
            {
                errorMessageFlag = true;
                errorMessage += $"{_game.LeftTeam.Name.Value}にリベロが含まれています。\n";
            }
            if (RightTeamRotatiton.Where(x => _game.RightTeam.Players.Where(x => x.IsLibero).Select(x => x.Id).ToList().Contains((int)x)).Any())
            {
                errorMessageFlag = true;
                errorMessage += $"{_game.RightTeam.Name.Value}にリベロが含まれています。\n";
            }

            //例外的な選手交代をした選手
            if(LeftTeamRotatiton.Where(x => _game.LeftTeam.Players.Where(x => x.IsExceptionalSubstituted).Select(x => x.Id).ToList().Contains((int)x)).Any())
            {
                errorMessageFlag = true;
                errorMessage += $"{_game.LeftTeam.Name.Value}に例外的な選手交代をした選手が含まれています。\n";
            }
            if (RightTeamRotatiton.Where(x => _game.RightTeam.Players.Where(x => x.IsExceptionalSubstituted).Select(x => x.Id).ToList().Contains((int)x)).Any())
            {
                errorMessageFlag = true;
                errorMessage += $"{_game.RightTeam.Name.Value}に例外的な選手交代をした選手が含まれています。\n";
            }

            //失格になった選手
            if (LeftTeamRotatiton.Where(x => _game.LeftTeam.Players.Where(x => x.IsDisqualified).Select(x => x.Id).ToList().Contains((int)x)).Any())
            {
                errorMessageFlag = true;
                errorMessage += $"{_game.LeftTeam.Name.Value}に失格になった選手が含まれています。\n";
            }
            if (RightTeamRotatiton.Where(x => _game.RightTeam.Players.Where(x => x.IsDisqualified).Select(x => x.Id).ToList().Contains((int)x)).Any())
            {
                errorMessageFlag = true;
                errorMessage += $"{_game.RightTeam.Name.Value}に失格になった選手が含まれています。\n";
            }

            //重複チェック
            if (LeftTeamRotatiton.DistinctBy(x => x).Count() != 6)
            {
                errorMessageFlag = true;
                errorMessage += $"{_game.LeftTeam.Name.Value}の選手が重複しています。\n";
            }
            if (RightTeamRotatiton.DistinctBy(x => x).Count() != 6)
            {
                errorMessageFlag = true;
                errorMessage += $"{_game.RightTeam.Name.Value}の選手が重複しています。\n";
            }

            foreach (var item in LeftTeamRotatiton)
            {
                if (!_game.LeftTeam.Players.Any(x => x.Id == item))
                {
                    errorMessageFlag = true;
                    errorMessage += $"{_game.LeftTeam.Name.Value}の{item}番は登録されていません。\n";
                }
            }

            foreach (var item in RightTeamRotatiton)
            {
                if (!_game.RightTeam.Players.Any(x => x.Id == item))
                {
                    errorMessageFlag = true;
                    errorMessage += $"{_game.RightTeam.Name.Value}の{item}番は登録されていません。\n";
                }
            }


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

            _game.LeftTeam.Sets[^1].StartingLineUp.Value = LeftTeamRotatiton.Select(x => x!.Value).ToArray();
            _game.LeftTeam.StartingLineUp.Value = _game.LeftTeam.Sets[^1].StartingLineUp.Value;
            _game.RightTeam.Sets[^1].StartingLineUp.Value = RightTeamRotatiton.Select(x => x!.Value).ToArray();
            _game.RightTeam.StartingLineUp.Value = _game.RightTeam.Sets[^1].StartingLineUp.Value;

            var param = new DialogParameters
            {
                { "LeftTeamRotation", LeftTeamRotatiton.Select(x=>x!.Value).ToArray() },
                { "RightTeamRotation", RightTeamRotatiton.Select(x=>x!.Value).ToArray() }
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
