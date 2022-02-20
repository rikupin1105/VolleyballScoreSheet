using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Regions;
using Reactive.Bindings;

namespace VolleyballScoreSheet.ViewModels
{
    public class CoinTossViewModel
    {
        public ReactiveCommand SwitchCourtCommand { get; } = new ReactiveCommand();
        public ReactiveCommand SwitchServerCommand { get; } = new ReactiveCommand();
        public ReactiveCommand NextCommand { get; } = new ReactiveCommand();

        public ReactiveProperty<string> LeftTeam { get; private set; }
        public ReactiveProperty<string> RightTeam { get; private set; }
        public ReactiveProperty<string> LeftTeamToss { get; private set; } = new ReactiveProperty<string>();
        public ReactiveProperty<string> RightTeamToss { get; private set; } = new ReactiveProperty<string>();

        private readonly IRegionManager _regionManager;
        private readonly Game _game;
        public CoinTossViewModel(IRegionManager regionManager, Game game)
        {
            _regionManager = regionManager;
            _game = game;
            LeftTeam = new ReactiveProperty<string>(_game.ATeam);
            RightTeam = new ReactiveProperty<string>(_game.BTeam);
            LeftTeamToss.Value ="Server";
            RightTeamToss.Value ="Reception";
            SwitchCourtCommand.Subscribe(_ => SwitchCourt());
            SwitchServerCommand.Subscribe(_ => SwitchServer());
            NextCommand.Subscribe(_ => Next());
        }
        public void SwitchCourt()
        {
            (LeftTeam.Value, RightTeam.Value)=(RightTeam.Value, LeftTeam.Value);
            (LeftTeamToss.Value, RightTeamToss.Value)=(RightTeamToss.Value, LeftTeamToss.Value);
        }
        public void SwitchServer()
        {
            (LeftTeamToss.Value, RightTeamToss.Value) = (RightTeamToss.Value, LeftTeamToss.Value);
        }
        public void Next()
        {
            var set = new Set();
            if (LeftTeam.Value == _game.ATeam)
            {
                set.ATeamRightSide = false;
                if(LeftTeamToss.Value == "Server")
                {
                    set.ATeamServer = true;
                }
                else
                {
                    set.ATeamServer= false;
                }
            }
            else
            {
                set.ATeamRightSide = true;
                if (LeftTeamToss.Value == "Server")
                {
                    set.ATeamServer= false;
                }
                else
                {
                    set.ATeamServer = true;
                }
            }
            _game.CreateSet(set);
            Navigate("BeforeMatch");
        }
        private void Navigate(string navigatePath)
        {
            if (navigatePath != null)
                _regionManager.RequestNavigate("ContentRegion", navigatePath);
        }
    }
}
