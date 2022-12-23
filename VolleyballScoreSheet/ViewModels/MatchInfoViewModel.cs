using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Regions;
using Prism.Services.Dialogs;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using Unity;
using VolleyballScoreSheet.Model;

namespace VolleyballScoreSheet.ViewModels
{
    public class MatchInfoViewModel
    {
        private readonly IRegionManager _regionManager;
        private readonly Game _game;
        public MatchInfoViewModel(Game game, IRegionManager regionManager, IDialogService dialogService)
        {
            MatchInfo = new MatchInfo();
            Referees = new Referees();
            Rule = new Rule();

            _game = game;
            _regionManager = regionManager;
            _dialogService = dialogService;
            NextCommand.Subscribe(_ => Next());

            ATeamName = _game.ToReactivePropertyAsSynchronized(x => x.ATeam.Value.Name.Value);
            BTeamName = _game.ToReactivePropertyAsSynchronized(x => x.BTeam.Value.Name.Value);

            SexSelectValue = (int)MatchInfo.Sex;
        }
        public ReactiveCommand NextCommand { get; set; } = new ReactiveCommand();
        public ReactiveProperty<string> ATeamName { get; set;}
        public ReactiveProperty<string> BTeamName { get; set;}

        public int SexSelectValue { get; set;}
        public MatchInfo MatchInfo { get; set; }
        public Referees Referees { get; set;}
        public Rule Rule { get; set; }

        private readonly IDialogService _dialogService;
        
        public void Dialog(string message)
        {
            _dialogService.ShowDialog(
                    "NotificationDialog",
                    new DialogParameters
                    {
                        { "Title", "注意" },
                        { "Message", message },
                        { "ButtonText", "OK" }
                    }, res =>
                    {

                    }, "AlertWindow");
        }
        public void Next()
        {
            if (string.IsNullOrEmpty(ATeamName.Value) || string.IsNullOrEmpty(BTeamName.Value))
            {
                Dialog("チーム名を入力してください。");
                return;
            }
            if (ATeamName.Value==BTeamName.Value)
            {
                Dialog("チーム名が同じです。");
                return;
            }

            _game.MatchInfo = MatchInfo;
            _game.Referees = Referees;
            _game.Rule = Rule;

            Navigate("Roster");
        }
        private void Navigate(string navigatePath)
        {
            if (navigatePath != null)
                _regionManager.RequestNavigate("ContentRegion", navigatePath);
        }
    }
}
