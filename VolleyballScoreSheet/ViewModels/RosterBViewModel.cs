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

namespace VolleyballScoreSheet.ViewModels
{
    public class RosterBViewModel
    {
        public ReactiveProperty<string> BTeamName { get; } 
        public ReactiveProperty<DataTable> PlayerDataTable { get; } = new ReactiveProperty<DataTable>(new DataTable());
        public ReactiveCommand PlayerAddCommand { get; } = new ReactiveCommand();
        public ReactiveProperty<int?> Id { get; private set; } = new ReactiveProperty<int?>();
        public ReactiveProperty<string> PlayerName { get; private set; } = new ReactiveProperty<string>();
        public void PlayerAdd()
        {
            if (Id.Value != null && Id.Value >= 0 && PlayerOrganize() < 14)
            {
                var row = PlayerDataTable.Value.NewRow();

                row[0] = Id.Value;
                row[1] = PlayerName.Value;
                PlayerDataTable.Value.Rows.Add(row);

                Id.Value = null;
                PlayerName.Value = string.Empty;
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
            if (PlayerOrganize() >= 1)
            {
                //画面遷移
                for (int i = 0; i < PlayerDataTable.Value.Rows.Count; i++)
                {
                    var row = PlayerDataTable.Value.Rows[i];

                   _game.BTeam.Value.Players.Add(new Player()
                    {
                        Id = int.Parse(row.ItemArray[0].ToString()),
                        Name = row.ItemArray[1].ToString()
                    });
                }
                Navigate("Gaming");
            }
        }

        private readonly IRegionManager _regionManager;
        private readonly Game _game;
        public ReactiveCommand NextCommand { get; set; } = new ReactiveCommand();
        public RosterBViewModel(Game game, IRegionManager regionManager)
        {
            _game = game;
            _regionManager = regionManager;

            PlayerDataTable.Value.Clear();
            PlayerDataTable.Value.Columns.Add("Number");
            PlayerDataTable.Value.Columns.Add("Name");

            PlayerAddCommand.Subscribe(_ => PlayerAdd());
            NextCommand.Subscribe(_ => Next());

            BTeamName =_game.ToReactivePropertyAsSynchronized(x => x.BTeam.Value.Name.Value);

            var dictionary = new Dictionary<string, string>();
            dictionary.Add("3", "深津旭弘");
            dictionary.Add("4", "大竹壱青");
            dictionary.Add("11", "富田将馬");
            dictionary.Add("18", "仲本賢優");
            dictionary.Add("21", "永露元稀");
            dictionary.Add("22", "樋口裕希");
            dictionary.Add("24", "高橋和幸");
            dictionary.Add("29", "藤中颯志");
            dictionary.Add("30", "エバダデンラリー");
            dictionary.Add("37", "藤中謙也");
            dictionary.Add("38", "小野遥輝");
            dictionary.Add("39", "小澤宙輝");
            dictionary.Add("40", "難波尭弘");
            dictionary.Add("41", "山田大悟");

            foreach (var item in dictionary)
            {
                var r = PlayerDataTable.Value.NewRow();
                r[0] = item.Key;
                r[1] =item.Value;
                PlayerDataTable.Value.Rows.Add(r);
            }
        }
        private void Navigate(string navigatePath)
        {
            if (navigatePath != null)
                _regionManager.RequestNavigate("ContentRegion", navigatePath);
        }
    }
}
