using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VolleyballScoreSheet.Shared;

namespace VolleyballScoreSheet._3SetScoresheet.Model
{
    internal class MatchData
    {
        public MatchData(Game game)
        {
            MatchName = game.MatchInfo.MatchName;
            City = game.MatchInfo.City;
            Hall = game.MatchInfo.Hall;
            MatchNumber = game.MatchInfo.MatchNumber;
            Date = game.MatchInfo.Date;

            Team = game.ATeam.Value.Name + " 対 " + game.BTeam.Value.Name;
            if (game.MatchInfo.Sex == Sex.Men) IsMen = true;
            else if (game.MatchInfo.Sex == Sex.Women) IsWoMen = true;

            if (game.CoinToss.CoinTossCompleted)
            {
                if (game.CoinToss.ATeamLeftSide)
                {
                    LeftTeamAB = "A";
                    RightTeamAB = "B";
                }
                else
                {
                    LeftTeamAB = "B";
                    RightTeamAB = "A";
                }
            }
        }
        public string LeftTeamAB { get; set; }
        public string RightTeamAB { get; set; }
        public string MatchName { get; set; }
        public string? City { get; set; }
        public string? Hall { get; set; }
        public string? MatchNumber { get; set; }
        public DateTime Date { get; set; }
        public string Team { get; set; }
        public bool IsMen { get; set; }
        public bool IsWoMen { get; set; }
    }
}
