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
            _game = game;
            _regionManager = regionManager;
            _dialogService = dialogService;
            NextCommand.Subscribe(_ => Next());

            MatchName = _game.ToReactivePropertyAsSynchronized(x => x.MatchName)!;
            ATeamName = _game.ToReactivePropertyAsSynchronized(x => x.ATeam.Value.Name.Value);
            BTeamName = _game.ToReactivePropertyAsSynchronized(x => x.BTeam.Value.Name.Value);
            City = _game.ToReactivePropertyAsSynchronized(x => x.City)!;
            Hall = _game.ToReactivePropertyAsSynchronized(x => x.Hall)!;
            MatchNumber = _game.ToReactivePropertyAsSynchronized(x => x.MatchNumber)!;
            Date = _game.ToReactivePropertyAsSynchronized(x => x.Date)!;
            FirstReferee = _game.ToReactivePropertyAsSynchronized(x => x.Referees.FirstReferee)!;
            SecondReferee = _game.ToReactivePropertyAsSynchronized(x => x.Referees.SecondReferee)!;
            Scorer = _game.ToReactivePropertyAsSynchronized(x => x.Referees.Scorer)!;
            AssistantScorer = _game.ToReactivePropertyAsSynchronized(x => x.Referees.AssistantScorer)!;
            FirstLineJudge = _game.ToReactivePropertyAsSynchronized(x => x.Referees.FirstLineJudge)!;
            SecondLineJudge = _game.ToReactivePropertyAsSynchronized(x => x.Referees.SecondLineJudge)!;
            ThirdLineJudge = _game.ToReactivePropertyAsSynchronized(x => x.Referees.ThirdLineJudge)!;
            FourthLineJudge = _game.ToReactivePropertyAsSynchronized(x => x.Referees.FourthLineJudge)!;

            SetCount = _game.ToReactivePropertyAsSynchronized(x => x.Rule.SetCount);
            ToWinPoint = _game.ToReactivePropertyAsSynchronized(x => x.Rule.ToWinPoint);
            FinalSetToWinPoint = _game.ToReactivePropertyAsSynchronized(x => x.Rule.FinalSetToWinPoint);
            FinalSetCourtChangePoint = _game.ToReactivePropertyAsSynchronized(x => x.Rule.FinalSetCourtChangePoint);
        }
        public ReactiveCommand NextCommand { get; set; } = new ReactiveCommand();
        public ReactiveProperty<string> MatchName { get; }
        public ReactiveProperty<string> ATeamName { get; }
        public ReactiveProperty<string> BTeamName { get; }
        public ReactiveProperty<string> City { get; } 
        public ReactiveProperty<string> Hall { get; }
        public ReactiveProperty<string> MatchNumber { get; }
        public ReactiveProperty<DateTime> Date { get; }

        public ReactiveProperty<Referee> FirstReferee { get; }
        public ReactiveProperty<Referee> SecondReferee { get; }
        public ReactiveProperty<Referee> Scorer { get; }
        public ReactiveProperty<Referee> AssistantScorer { get; }
        public ReactiveProperty<Referee> FirstLineJudge { get; } 
        public ReactiveProperty<Referee> SecondLineJudge { get; }
        public ReactiveProperty<Referee> ThirdLineJudge { get; } 
        public ReactiveProperty<Referee> FourthLineJudge { get; }

        public ReactiveProperty<int> SetCount { get; }
        public ReactiveProperty<int> ToWinPoint { get; }
        public ReactiveProperty<int> FinalSetToWinPoint { get; }
        public ReactiveProperty<int> FinalSetCourtChangePoint { get; }

        private readonly IDialogService _dialogService;
        
        public void Dialog(string message)
        {
            _dialogService.ShowDialog(
                    "NotificationDialog",
                    new DialogParameters
                    {
                        { "Title", "Alert" },
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



            MatchName.Subscribe(x => { _game.MatchName = x; });
            ATeamName.Subscribe(x => { _game.ATeam.Value.Name.Value = x; });
            BTeamName.Subscribe(x => { _game.BTeam.Value.Name.Value = x; });


            Navigate("Roster");
        }
        private void Navigate(string navigatePath)
        {
            if (navigatePath != null)
                _regionManager.RequestNavigate("ContentRegion", navigatePath);
        }
    }
}
