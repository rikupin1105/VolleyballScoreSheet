using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace VolleyballScoreSheet.ViewModels.ScoreSheet
{
    public class MatchDataViewModel : BindableBase
    {
        private readonly Game _game;
        public MatchDataViewModel(Game game)
        {
            _game = game;

            MatchName = _game.MatchName;
            City = _game.City;
            Hall = _game.Hall;
            MatchNumber = _game.MatchNumber;
            Date = _game.Date.ToString("yyyy年MM月dd日");
            Team = _game.ATeam.Value.Name+" 対 "+_game.BTeam.Value.Name;

            if (_game.Sex == Model.Sex.Men)
                IsMen = true;
            else if (_game.Sex == Model.Sex.Women)
                IsWoMen = true;
        }

        public string MatchName { get; set; }
        public string? City { get; set; }
        public string? Hall { get; set; }
        public string? MatchNumber { get; set; }
        public string Date { get; set; }
        public string Team { get; set; }
        public bool IsMen { get; set; }
        public bool IsWoMen { get; set; }
    }
}
