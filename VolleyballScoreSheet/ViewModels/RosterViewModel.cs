using Prism.Regions;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VolleyballScoreSheet.Model;
using VolleyballScoreSheet;
using Reactive.Bindings.Extensions;
using Prism.Services.Dialogs;
using System.Reactive.Linq;
using System.ComponentModel.DataAnnotations;
using System.Collections.ObjectModel;
using System.Xml.Linq;

namespace VolleyballScoreSheet.ViewModels
{
    public class RosterViewModel
    {
        private readonly IRegionManager _regionManager;
        private readonly IDialogService _dialogService;
        private readonly Game _game;
        public RosterViewModel(Game game, IRegionManager regionManager, IDialogService dialogService)
        {
            _game = game;
            _dialogService = dialogService;
            _regionManager = regionManager;

            PlayerAddCommandA.Subscribe(_ => PlayerAdd(true));
            PlayerAddCommandB.Subscribe(_ => PlayerAdd(false));
            NextCommand.Subscribe(_ => Next());

            APlayers.AddRange(new List<Player>
            {
                new(1, "西田有志"),
                new(2, "小野寺太志"),
                new(5, "大塚達宣"),
                new(7, "高梨健太"),
                new(8, "関田誠大",isCaptain:true),
                new(9, "大宅真樹"),
                new(12, "高橋藍"),
                new(13, "小川智大", true),
                new(16, "宮浦健人"),
                new(20, "山本智大", true),
                new(23, "佐藤駿一郎"),
                new(26, "村山豪"),
                new(43, "デ・アルマスアライン")
            });

            BPlayers.AddRange(new List<Player>
            {
                new(3, "深津旭弘",isCaptain:true),
                new(4, "大竹壱青"),
                new(11, "富田将馬"),
                new(18, "仲本賢優"),
                new(21, "永露元稀"),
                new(22, "樋口裕希"),
                new(24, "高橋和幸",true),
                new(29, "藤中颯志",true),
                new(30, "エバダデンラリー"),
                new(37, "藤中謙也"),
                new(38, "小野遥輝"),
                new(39, "小澤宙輝"),
                new(40, "難波尭弘"),
                new(41, "山田大悟")
            });

            EditCommandA.Subscribe(x =>
            {
                Edit(true, (int)x);
            });
            EditCommandB.Subscribe(x =>
            {

                Edit(false, (int)x);
            });

            ATeamName = _game.ToReactivePropertyAsSynchronized(x => x.ATeam.Value.Name.Value);
            BTeamName = _game.ToReactivePropertyAsSynchronized(x => x.BTeam.Value.Name.Value);
        }
        private void Edit(bool isA, int x)
        {
            ReactiveCollection<Player> dataTable;
            if (isA)
                dataTable = APlayers;
            else
                dataTable = BPlayers;

            _dialogService.ShowDialog("EditPlayer", new DialogParameters()
            {
                {"Player",dataTable.First(y=>y.Id==(int)x!) }
            }, res =>
            {
                if (res.Result == ButtonResult.Abort)
                {
                    //削除
                    dataTable.Remove(dataTable.First(y => y.Id==(int)x));
                }
                else if (res.Result==ButtonResult.OK)
                {
                    //修正
                    if (res.Parameters.TryGetValue("Player", out Player player))
                    {
                        //番号が一緒
                        if (player.Id == (int)x)
                        {
                            dataTable[dataTable.IndexOf(dataTable.First(y => y.Id==player.Id))] = player;
                        }
                        else
                        {
                            //番号が変更
                            if (dataTable.Select(x => x.Id).Contains(player.Id))
                            {
                                //番号重複
                                _dialogService.ShowDialog("NotificationDialog", new DialogParameters()
                                {
                                    {"Title","注意" },
                                    { "Message",$"すでに{player.Id}番は登録されています。"},
                                    {"ButtonText","OK" }
                                }, res =>
                                {

                                });
                            }
                            else
                            {
                                dataTable.Remove(dataTable.First(y => y.Id==(int)x));
                                dataTable.Add(player);
                            }
                        }
                    }
                }
            });

            if (isA)
                APlayers = dataTable;
            else
                BPlayers = dataTable;
        }
        public ReactiveProperty<string> ATeamName { get; }
        public ReactiveProperty<string> BTeamName { get; }
        public ReactiveCollection<Player> APlayers { get; set; } = new ReactiveCollection<Player>();
        public ReactiveCollection<Player> BPlayers { get; set; } = new ReactiveCollection<Player>();
        public ReactiveCommand PlayerAddCommandA { get; } = new ReactiveCommand();
        public ReactiveCommand PlayerAddCommandB { get; } = new ReactiveCommand();
        public ReactiveProperty<int?> IdA { get; set; } = new ReactiveProperty<int?>();
        public ReactiveProperty<int?> IdB { get; set; } = new ReactiveProperty<int?>();
        public ReactiveProperty<bool> IsLiberoA { get; set; } = new ReactiveProperty<bool>();
        public ReactiveProperty<bool> IsLiberoB { get; set; } = new ReactiveProperty<bool>();
        public ReactiveProperty<bool> IsCaptainA { get; set; } = new ReactiveProperty<bool>();
        public ReactiveProperty<bool> IsCaptainB { get; set; } = new ReactiveProperty<bool>();

        public ReactiveProperty<string> PlayerNameA { get; set; } = new ReactiveProperty<string>();
        public ReactiveProperty<string> PlayerNameB { get; set; } = new ReactiveProperty<string>();
        public void PlayerAdd(bool isA)
        {
            ReactiveCollection<Player> dataTable;
            int? Id;
            string Name;
            bool isLibero;
            bool isCaptain;

            if (isA)
            {
                Id = IdA.Value;
                Name = PlayerNameA.Value;
                isLibero = IsLiberoA.Value;
                isCaptain = IsCaptainA.Value;
                dataTable = APlayers;
            }
            else
            {
                Id = IdB.Value;
                Name = PlayerNameB.Value;
                isLibero = IsLiberoB.Value;
                isCaptain = IsCaptainB.Value;
                dataTable = BPlayers;
            }

            if (dataTable.Count>=14)
            {
                _dialogService.ShowDialog(
                   "NotificationDialog",
                   new DialogParameters
                   {
                        { "Title", "Alert" },
                        { "Message", "14人以上は登録できません。" },
                        { "ButtonText", "OK" }
                   }, res =>
                   {
                   }, "AlertWindow");
                return;
            }
            var distincted = dataTable.DistinctBy(x => x.Id).Count();
            if (distincted != dataTable.Count)
            {
                _dialogService.ShowDialog(
                   "NotificationDialog",
                   new DialogParameters
                   {
                        { "Title", "Alert" },
                        { "Message", $"{string.Join(' ', dataTable.Except(dataTable.DistinctBy(x => x.Id)))}番号が重複しています。" },
                        { "ButtonText", "OK" }
                   }, res =>
                   {
                   }, "AlertWindow");
            }

            if (Id != null)
            {
                //人数オーバチェック
                if (dataTable.Count > 14)
                {
                    _dialogService.ShowDialog("NotificationDialog", new DialogParameters
                   {
                        { "Title", "Alert" },
                        { "Message", $"14人以上は登録できません" },
                        { "ButtonText", "OK" }
                   }, res =>
                   {
                   }, "AlertWindow");
                    return;
                }

                //重複チェック
                var player = new Player((int)Id, Name,isLibero,isCaptain);

                if (dataTable.Select(x => x.Id).ToList().Contains(player.Id))
                {
                    _dialogService.ShowDialog("NotificationDialog", new DialogParameters
                   {
                        { "Title", "Alert" },
                        { "Message", $"番号が重複しています。" },
                        { "ButtonText", "OK" }
                   }, res =>
                   {
                   }, "AlertWindow");
                    return;
                };

                dataTable.Add(player);
                Id = null;
                Name = string.Empty;
                isLibero = false;
                isCaptain = false;
            }
            else
            {
            }

            if (isA)
            {
                IsLiberoA.Value = isLibero;
                IsCaptainA.Value = isCaptain;
                IdA.Value = Id;
                PlayerNameA.Value=Name;
            }
            else
            {
                IsLiberoB.Value = isLibero;
                IsCaptainB.Value = isCaptain;
                IdB.Value = Id;
                PlayerNameB.Value = Name;
            }
        }
        public int PlayerOrganize(bool isA)
        {
            ReactiveCollection<Player> dataTable;
            int? Id;
            string Name;
            if (isA)
            {
                Id = IdA.Value;
                Name = PlayerNameB.Value;
                dataTable = APlayers;
            }
            else
            {
                Id = IdB.Value;
                Name = PlayerNameB.Value;
                dataTable = BPlayers;
            }

            var deleteList = new List<int>();
            for (int i = 0; i < dataTable.Count; i++)
            {
                int id = dataTable[i].Id;

                deleteList.Add(i);

                if (id == Id)
                {
                    Id = null;
                    Name = string.Empty;
                }
            }

            foreach (var item in deleteList)
            {
                //dataTable.Rows.RemoveAt(item);
            }

            if (isA)
            {
                IdA.Value = Id;
                PlayerNameA.Value=Name;
            }
            else
            {
                IdB.Value = Id;
                PlayerNameB.Value = Name;
            }
            return dataTable.Count;
        }
        public void Next()
        {
            if (PlayerOrganize(true) >= 6 && PlayerOrganize(false)>=6)
            {
                //画面遷移

                _game.ATeam.Value.Players.AddRange(APlayers);
                _game.BTeam.Value.Players.AddRange(BPlayers);

                Navigate("Gaming");
            }
            else
            {
                _dialogService.ShowDialog(
                   "NotificationDialog",
                   new DialogParameters
                   {
                        { "Title", "Alert" },
                        { "Message", "6人以上登録してください。" },
                        { "ButtonText", "OK" }
                   }, res =>
                   {

                   }, "AlertWindow");
            }
        }

        public ReactiveCommand NextCommand { get; set; } = new();
        public ReactiveCommand EditCommandA { get; set; } = new();
        public ReactiveCommand EditCommandB { get; set; } = new();



        private void Navigate(string navigatePath)
        {
            if (navigatePath != null)
                _regionManager.RequestNavigate("ContentRegion", navigatePath);
        }
    }
}
