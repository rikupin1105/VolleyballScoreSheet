using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VolleyballScoreSheet.Views.ScoreSheet;

namespace VolleyballScoreSheet.Model.Scoresheet
{
    public class Sanction
    {
        public Sanction(Game game) 
        {
            foreach (var item in game.Sanctions.Value)
            {
                Sanctions.Add(new(item));
            }
            for (int i = 0; i < 10 - game.Sanctions.Value.Count(); i++)
            {
                Sanctions.Add(new());
            }

            if (game.CoinToss.ATeamLeftSide)
            {
                ImproperRequestedA = game.ATeam.Value.ImproperRequests.Value;
                ImproperRequestedB = game.BTeam.Value.ImproperRequests.Value;
            }
            else
            {
                ImproperRequestedA = game.BTeam.Value.ImproperRequests.Value;
                ImproperRequestedB = game.ATeam.Value.ImproperRequests.Value;
            }
        }
        public bool ImproperRequestedA { get; set; }
        public bool ImproperRequestedB { get; set; }
        public List<SanctionScoresheet> Sanctions { get; set; } = new();

        public class SanctionScoresheet
        {
            public SanctionScoresheet() { }
            public SanctionScoresheet(Model.Sanction sanction)
            {
                Warning = sanction.Warning;
                Penalty = sanction.Penalty;
                Explusion = sanction.Explusion;
                Disqualification = sanction.Disqualification;
                Team = sanction.Team;
                Set = sanction.Set;
                Point = sanction.Point;
                OpponentPoint = sanction.OpponentPoint;
            }
            public string? Warning { get; set; }
            public string? Penalty { get; set; }
            public string? Explusion { get; set; }
            public string? Disqualification { get; set; }
            public char? Team { get; set; }
            public int? Set { get; set; }
            public string? Score { get => Point + " : " + OpponentPoint; }

            internal int? Point { get; set; }
            internal int? OpponentPoint { get; set; }
        }
    }

}
