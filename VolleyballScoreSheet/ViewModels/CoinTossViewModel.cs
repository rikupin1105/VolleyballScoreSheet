using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Regions;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using VolleyballScoreSheet.Model;

namespace VolleyballScoreSheet.ViewModels
{
    public class CoinTossViewModel : BinableBase
    {
        public ReactiveCommand SwitchCourtCommand { get; } = new ReactiveCommand();
        public ReactiveCommand SwitchServerCommand { get; } = new ReactiveCommand();
        public ReactiveCommand NextCommand { get; } = new ReactiveCommand();

        public ReactiveProperty<bool> DisplayCoinToss { get; set; }


        public ReactiveProperty<string> LeftTeamToss { get; private set; } = new ReactiveProperty<string>();
        public ReactiveProperty<string> RightTeamToss { get; private set; } = new ReactiveProperty<string>();

        private readonly Game _game;
        public CoinTossViewModel(Game game)
        {
            _game = game;

            LeftTeamToss.Value ="Server";
            RightTeamToss.Value ="Reception";
            SwitchCourtCommand.Subscribe(_ => SwitchCourt());
            SwitchServerCommand.Subscribe(_ => SwitchServer());
            NextCommand.Subscribe(_ => Next());

            DisplayCoinToss = _game.ToReactivePropertyAsSynchronized(x => x.DisplayCoinToss.Value);
        }
        public void SwitchCourt()
        {
            _game.CourtChange();

            (LeftTeamToss.Value, RightTeamToss.Value) = (RightTeamToss.Value, LeftTeamToss.Value);
        }
        public void SwitchServer()
        {
            (LeftTeamToss.Value, RightTeamToss.Value) = (RightTeamToss.Value, LeftTeamToss.Value);
        }
        public void Next()
        {
            if (_game.isATeamLeft.Value)
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
            _game.DisplayMain("BeforeMatch");
        }
    }
}
