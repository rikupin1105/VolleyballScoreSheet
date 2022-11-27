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
using System.Windows.Forms;

namespace VolleyballScoreSheet.ViewModels
{
    public class RosterAViewModel
    {
        private readonly IRegionManager _regionManager;
        private readonly IDialogService _dialogService;
        private readonly Game _game;
        public RosterAViewModel(Game game, IRegionManager regionManager, IDialogService dialogService)
        {
            _game = game;
            _dialogService = dialogService;
            _regionManager = regionManager;


            PlayerDataTable.Value.Clear();
            PlayerDataTable.Value.Columns.Add("Number");
            PlayerDataTable.Value.Columns.Add("Name");

            PlayerAddCommand.Subscribe(_ => PlayerAdd());
            NextCommand.Subscribe(_ => Next());

            for (int i = 1; i <= 12; i++)
            {
                var r = PlayerDataTable.Value.NewRow();
                r[0] = i;
                r[1] = "Player_"+i;
                PlayerDataTable.Value.Rows.Add(r);
            }

            ATeamName = _game.ToReactivePropertyAsSynchronized(x => x.ATeam.Value.Name.Value);
        }
        public ReactiveProperty<string> ATeamName { get; }
        public ReactiveProperty<DataTable> PlayerDataTable { get; } = new ReactiveProperty<DataTable>(new DataTable());
        public ReactiveCommand PlayerAddCommand { get; } = new ReactiveCommand();
        public ReactiveProperty<int?> Id { get; private set; } = new ReactiveProperty<int?>();
        public ReactiveProperty<string> PlayerName { get; private set; } = new ReactiveProperty<string>();
        public void PlayerAdd()
        {
            if (PlayerOrganize()>14)
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

            }
            if (Id.Value != null && Id.Value >= 0 && PlayerOrganize() < 14)
            {
                var row = PlayerDataTable.Value.NewRow();

                //重複チェック


                row[0] = Id.Value;
                row[1] = PlayerName.Value;
                PlayerDataTable.Value.Rows.Add(row);

                Id.Value = null;
                PlayerName.Value = string.Empty;
            }
            else
            {
            }
        }
        public int PlayerOrganize()
        {
            var deleteList = new List<int>();
            for (int i = 0; i < PlayerDataTable.Value.Rows.Count; i++)
            {
                int id;
                var success = int.TryParse(PlayerDataTable.Value.Rows[i][0].ToString(), out id);

                if (!success)
                {
                    deleteList.Add(i);
                }
                if (id == Id.Value)
                {
                    Id.Value = null;
                    PlayerName.Value = string.Empty;
                }
            }
            foreach (var item in deleteList)
            {
                PlayerDataTable.Value.Rows.RemoveAt(item);
            }
            return PlayerDataTable.Value.Rows.Count;
        }
        public void Next()
        {
            

            if (PlayerOrganize() >= 6)
            {
                //画面遷移
                for (int i = 0; i < PlayerDataTable.Value.Rows.Count; i++)
                {
                    var row = PlayerDataTable.Value.Rows[i];

                    _game.ATeam.Value.Players.Add(new Player()
                    {
                        Id = int.Parse(row.ItemArray[0].ToString())
                    });
                }
                Navigate("RosterB");
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


        public ReactiveCommand NextCommand { get; set; } = new ReactiveCommand();




        private void Navigate(string navigatePath)
        {
            if (navigatePath != null)
                _regionManager.RequestNavigate("ContentRegion", navigatePath);
        }
    }
}
