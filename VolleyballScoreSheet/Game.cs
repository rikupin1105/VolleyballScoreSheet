using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VolleyballScoreSheet.Model;

namespace VolleyballScoreSheet
{
    public class Game
    {
        public List<Set> Sets { get; set; } = new();
        public List<Player> ATeamPlayers { get; set; } = new List<Player>();
        public List<Player> BTeamPlayers { get; set; } = new List<Player>();
        public int ATeamSet { get; set; }
        public int BTeamSet { get; set; }
        public bool ATeamServer { get; set; }
        public bool BTeamServer { get; set; }
        public string? MatchName { get; set; }
        public string ATeam { get; set; } = "";
        public string BTeam { get; set; } = "";
        public string? City { get; set; }
        public string? Hall { get; set; }
        public int MatchNumber { get; set; }
        public DateTime Date { get; set; }
        public Referee? FirstReferee { get; set; }
        public Referee? SecondReferee { get; set; }
        public Referee? Scorer { get; set; }
        public Referee? AssistantScorer { get; set; }
        public Referee? FirstLineJudge { get; set; }
        public Referee? SecondLineJudge { get; set; }
        public Referee? ThirdLineJudge { get; set; }
        public Referee? FourthLineJudge { get; set; }
        public CoinToss? CoinToss { get; set; } = new();

        public bool CourtChangeEnable { get; set; } = true;
        public int SetCount { get; set; } = 5;
        public int ToWinPoint { get; set; } = 25;
        public int FinalSetToWinPoint { get; set; } = 25;
        public int FinalSetCoutChangePoint { get; set; } = 13;

        public Set GetCurrentSet()
        {
            return Sets[^1];
        }
        public Set CreateSet(Set set)
        {
            set.GameSet = Sets.Count + 1;
            Sets.Add(set);
            return set;
        }
    }
}
