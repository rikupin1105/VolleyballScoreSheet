using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace VolleyballScoreSheet.ViewModels.ScoreSheet
{
    public class FirstSetViewModel : BindableBase
    {
        private readonly Game _game;
        public FirstSetViewModel(Game game)
        {
            _game = game;

            if (_game.CoinToss.ATeamLeftSide)
            {
                LeftTeamName = _game.ATeam.Value.Name.Value;
                RightTeamName = _game.BTeam.Value.Name.Value;
            }
            else
            {
                RightTeamName = _game.ATeam.Value.Name.Value;
                LeftTeamName = _game.BTeam.Value.Name.Value;
            }

            if(_game.isLeftServe.Value)
            {
                LeftTeamServe = true;
                LeftTeamReception = false;
            }
            else
            {
                LeftTeamServe = false;
                LeftTeamReception = true;
            }
        }
        public bool LeftTeamServe { get; set; }
        public bool LeftTeamReception { get; set; }
        public string LeftTeamName { get; set; }
        public string RightTeamName { get; set; }
    }
}
