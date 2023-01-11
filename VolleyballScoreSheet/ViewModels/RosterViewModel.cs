using Prism.Regions;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VolleyballScoreSheet;
using Reactive.Bindings.Extensions;
using Prism.Services.Dialogs;
using System.Reactive.Linq;
using System.ComponentModel.DataAnnotations;
using System.Collections.ObjectModel;
using System.Xml.Linq;
using CommonDialogLib;
using System.Security.Cryptography.Xml;

namespace VolleyballScoreSheet.ViewModels
{
    public class RosterViewModel
    {
        private readonly IRegionManager _regionManager;
        private readonly IDialogService _dialogService;
        private readonly ICommonDialogService _commonDialogService;
        private readonly Game _game;
        private readonly Roaster _roaster;
        public RosterViewModel(Game game, IRegionManager regionManager, ICommonDialogService commonDialogService, IDialogService dialogService)
        {
            _game = game;
            _dialogService = dialogService;
            _commonDialogService = commonDialogService;
            _regionManager = regionManager;
            _roaster = new Roaster(game);


            PlayerAddCommandA.Subscribe(_ => PlayerAdd(true));
            PlayerAddCommandB.Subscribe(_ => PlayerAdd(false));
            NextCommand.Subscribe(_ => Next());

            APlayers = _roaster.ATeamPlayers.ToReadOnlyReactiveCollection();
            BPlayers = _roaster.BTeamPlayers;

            _roaster.AlertCommand.Subscribe(message =>
            {
                _dialogService.ShowDialog(
                   "NotificationDialog",
                   new DialogParameters
                   {
                        { "Title", "警告" },
                        { "Message", message },
                        { "ButtonText", "OK" }
                   }, res =>
                   {
                   }, "AlertWindow");
            });


            OpenFileCommand.Subscribe(_ =>
            {
                bool isATeam = true;
                _dialogService.ShowDialog("SelectTeam", new DialogParameters() 
                {
                    {"Title","プレイヤー読み込み" },
                    {"Message","プレイヤーを読み込むチームを選択してください" }
                }, res =>
                {
                    if(res.Parameters.TryGetValue("isA", out bool isA))
                    {
                        isATeam = isA;
                    }
                });

                var setting = new OpenFileDialogSettings()
                {
                    Filter = "プレイヤーファイル(*.vpf)|*.vpf",
                    Title = "プレイヤーファイル読み込み"
                };

                if (_commonDialogService.ShowDialog(setting))
                {
                    _roaster.PlayerLoad(isATeam, setting.FileName);
                }
            });

            EditCommandA.Subscribe(x =>
            {
                Edit(true, (int)x!);
            });
            EditCommandB.Subscribe(x =>
            {

                Edit(false, (int)x!);
            });

            ATeamName = _game.ToReactivePropertyAsSynchronized(x => x.ATeam.Value.Name.Value);
            BTeamName = _game.ToReactivePropertyAsSynchronized(x => x.BTeam.Value.Name.Value);

            InputPlayerA = _roaster.ToReactivePropertyAsSynchronized(x => x.InputPlayerA.Value);
            InputPlayerB = _roaster.ToReactivePropertyAsSynchronized(x => x.InputPlayerB.Value);
        }
        private void Edit(bool isATeam, int x)
        {
            Roaster.Player player;
            if (isATeam) player = _roaster.ATeamPlayers.First(p => p.Id ==x);
            else player = _roaster.BTeamPlayers.First(p => p.Id==x);


            _dialogService.ShowDialog("EditPlayer", new DialogParameters()
            {
                {"Player",player }
            }, res =>
            {
                if (res.Result == ButtonResult.Abort)
                {
                    //削除
                    _roaster.PlayerDelete(isATeam, player);
                }
                else if (res.Result==ButtonResult.OK)
                {
                    if (res.Parameters.TryGetValue("Player", out Roaster.Player editedPlayer))
                    {
                        _roaster.PlayerEdit(isATeam, editedPlayer, (int)player.Id!);
                    }
                }
            });
        }
        public ReactiveProperty<string> ATeamName { get; }
        public ReactiveProperty<string> BTeamName { get; }
        public ReadOnlyReactiveCollection<Roaster.Player> APlayers { get; set; }
        public ObservableCollection<Roaster.Player> BPlayers { get; set; }
        public ReactiveCommand PlayerAddCommandA { get; } = new ReactiveCommand();
        public ReactiveCommand PlayerAddCommandB { get; } = new ReactiveCommand();

        public ReactiveProperty<Roaster.Player> InputPlayerA { get; set; }
        public ReactiveProperty<Roaster.Player> InputPlayerB { get; set; }

        public void PlayerAdd(bool isATeam)
        {
            _roaster.PlayerAdd(isATeam);
        }
        public void Next()
        {
            _roaster.Validate();
            if (!_roaster.IsError)
            {
                Navigate("Gaming");
            }
        }

        public ReactiveCommand NextCommand { get; set; } = new();
        public ReactiveCommand EditCommandA { get; set; } = new();
        public ReactiveCommand EditCommandB { get; set; } = new();
        public ReactiveCommand OpenFileCommand { get; } = new();

        private void Navigate(string navigatePath)
        {
            if (navigatePath != null)
                _regionManager.RequestNavigate("ContentRegion", navigatePath);
        }
    }
}
