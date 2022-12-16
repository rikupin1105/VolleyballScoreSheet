using System;

namespace VolleyballScoreSheet.Model
{
    public enum SanctionEnum
    {
        ImproperRequest,
        DelayWarning,
        DelayPenalty,
        Warning,
        Penalty,
        Explusion,
        Disqualification
    }
    public class Sanction
    {
        public Sanction(char team, DelayWarning delayWarning)
        {
            _ = delayWarning ?? throw new NullReferenceException(nameof(delayWarning));

            Warning = "D";
            Team = team;
            Set = delayWarning.Set;
            Point = delayWarning.Point;
            OpponentPoint = delayWarning.OpponentPoint;
        }
        public Sanction(char team, DelayPenalty delayPenaltie)
        {
            _ = delayPenaltie ?? throw new NullReferenceException(nameof(delayPenaltie));

            Penalty = "D";
            Team = team;
            Set = delayPenaltie.Set;
            Point = delayPenaltie.Point;
            OpponentPoint = delayPenaltie.OpponentPoint;
        }
        public Sanction() { }
        public string? Warning { get; set; }
        public string? Penalty { get; set; }
        public string? Explusion { get; set; }
        public string? Disqualification { get; set; }
        public char Team { get; set; }
        public int Set { get; set; }
        public string? Score { get => Point + " : " + OpponentPoint; }

        internal int Point { get; set; }
        internal int OpponentPoint { get; set; }
    }
}
