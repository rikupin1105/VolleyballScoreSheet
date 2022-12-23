using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using VolleyballScoreSheet.Model.Scoresheet;

namespace VolleyballScoreSheet.ViewModels.ScoreSheet
{
    public class MatchDataViewModel : BindableBase
    {
        private readonly Game _game;
        public MatchDataViewModel(Game game)
        {
            _game = game;
            var matchData = new MatchData(game);

            MatchName = matchData.MatchName;
            City = matchData.City;
            Hall = matchData.Hall;
            MatchNumber = matchData.MatchNumber;
            Date = matchData.Date.ToString("yyyy年MM月dd日");
            Team = matchData.Team;

            LeftTeamAB = matchData.LeftTeamAB;
            RightTeamAB = matchData.RightTeamAB;

            IsMen = matchData.IsMen;
            IsWoMen = matchData.IsWoMen;
        }
        public string LeftTeamAB { get; set; }
        public string RightTeamAB { get; set; }
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
