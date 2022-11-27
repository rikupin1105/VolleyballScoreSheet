using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Regions;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;

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
        public CoinTossViewModel(Game game, IRegionManager regionManager)
        {
            _game = game;
            _regionManager = regionManager;

            LeftTeamToss.Value ="Server";
            RightTeamToss.Value ="Reception";
            SwitchCourtCommand.Subscribe(_ => SwitchCourt());
            SwitchServerCommand.Subscribe(_ => SwitchServer());
            NextCommand.Subscribe(_ => Next());

            LeftTeam = _game.ToReactivePropertyAsSynchronized(x => x.LeftTeam.Name.Value);
            RightTeam = _game.ToReactivePropertyAsSynchronized(x => x.RightTeam.Name.Value);
        }
        public void SwitchCourt()
        {
            (_game.ATeam, _game.BTeam) = (_game.BTeam, _game.ATeam);
            (LeftTeam, RightTeam) = (RightTeam, LeftTeam);
            (LeftTeamToss.Value, RightTeamToss.Value) = (RightTeamToss.Value, LeftTeamToss.Value);
        }
        public void SwitchServer()
        {
            (LeftTeamToss.Value, RightTeamToss.Value) = (RightTeamToss.Value, LeftTeamToss.Value);
        }
        public void Next()
        {
            if (_game.ATeam.Value.Name.Value == LeftTeam.Value)
            {
                _game.CoinToss.ATeamLeftSide = true;
                _game.CoinToss.BTeamLeftSide = false;
            }
            else
            {
                _game.CoinToss.ATeamLeftSide = false;
                _game.CoinToss.BTeamLeftSide = true;
            }

            if (_game.isATeamLeft.Value)
            {
                if (LeftTeamToss.Value == "Server")
                {
                    _game.CoinToss.ATeamServer = true;
                    _game.CoinToss.BTeamServer = false;

                    _game.NextServeTeam(true);
                }
                else
                {
                    _game.CoinToss.ATeamServer = false;
                    _game.CoinToss.BTeamServer = true;

                    _game.NextServeTeam(false);
                }
            }
            else
            {
                if (LeftTeamToss.Value == "Server")
                {
                    _game.CoinToss.ATeamServer = false;
                    _game.CoinToss.BTeamServer = true;

                    _game.NextServeTeam(false);
                }
                else
                {
                    _game.CoinToss.ATeamServer = true;
                    _game.CoinToss.BTeamServer = false;

                    _game.NextServeTeam(true);
                }
            }
            Navigate("BeforeMatch");
        }
        private void Navigate(string navigatePath)
        {
            if (navigatePath != null)
                _regionManager.RequestNavigate("ContentRegion", navigatePath);
        }
    }
}
