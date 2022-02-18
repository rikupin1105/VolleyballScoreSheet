using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Regions;
using Reactive.Bindings;
using Unity;
using VolleyballScoreSheet.Model;

namespace VolleyballScoreSheet.ViewModels
{
    public class MatchInfoViewModel
    {
        private readonly IRegionManager _regionManager;
        public ReactiveCommand NextCommand { get; set; } = new ReactiveCommand();
        public ReactiveProperty<string> MatchName { get; } = new ReactiveProperty<string>();
        public ReactiveProperty<string> ATeam { get; } = new ReactiveProperty<string>();
        public ReactiveProperty<string> BTeam { get; } = new ReactiveProperty<string>();
        public ReactiveProperty<string> City { get; } = new ReactiveProperty<string>();
        public ReactiveProperty<string> Hall { get; } = new ReactiveProperty<string>();
        public ReactiveProperty<int> MatchNumber { get; } = new ReactiveProperty<int>();
        public ReactiveProperty<DateTime> Date { get; } = new ReactiveProperty<DateTime>(DateTime.Today);
        public ReactiveProperty<Referee> FirstReferee { get; } = new ReactiveProperty<Referee>();
        public ReactiveProperty<Referee> SecondReferee { get; } = new ReactiveProperty<Referee>();
        public ReactiveProperty<Referee> Scorer { get; } = new ReactiveProperty<Referee>();
        public ReactiveProperty<Referee> AssistantScorer { get; } = new ReactiveProperty<Referee>();
        public ReactiveProperty<Referee> FirstLineJudge { get; } = new ReactiveProperty<Referee>();
        public ReactiveProperty<Referee> SecondLineJudge { get; } = new ReactiveProperty<Referee>();
        public ReactiveProperty<Referee> ThirdLineJudge { get; } = new ReactiveProperty<Referee>();
        public ReactiveProperty<Referee> FourthLineJudge { get; } = new ReactiveProperty<Referee>();


        private readonly Game _game;
        public MatchInfoViewModel(IRegionManager regionManager, Game game)
        {
            _game = game;
            _regionManager = regionManager;
            NextCommand.Subscribe(_ => Next());
        }
        public void Next()
        {
            var flag = true;
            if (ATeam.Value ==null || BTeam.Value==null) flag=false;
            if (flag)
            {
                _game.MatchName = MatchName.Value;
                _game.ATeam = ATeam.Value ?? throw new();
                _game.BTeam = BTeam.Value ?? throw new();
                _game.City = City.Value;
                _game.Hall = Hall.Value;
                _game.MatchName = MatchName.Value;
                _game.Date = Date.Value;
                _game.FirstReferee = FirstLineJudge.Value;
                _game.SecondReferee = SecondLineJudge.Value;
                _game.Scorer = Scorer.Value;
                _game.AssistantScorer = AssistantScorer.Value;
                _game.FirstLineJudge = FirstLineJudge.Value;
                _game.SecondLineJudge = SecondLineJudge.Value;
                _game.ThirdLineJudge = ThirdLineJudge.Value;
                _game.FourthLineJudge = FourthLineJudge.Value;
                Navigate("RosterA");
            }
        }
        private void Navigate(string navigatePath)
        {
            if (navigatePath != null)
                _regionManager.RequestNavigate("ContentRegion", navigatePath);
        }
    }
}
