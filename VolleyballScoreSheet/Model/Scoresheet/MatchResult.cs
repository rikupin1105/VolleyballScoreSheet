using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VolleyballScoreSheet.Model.Scoresheet
{
    public class MatchResult
    {
        private readonly Game _game;
        public MatchResult(Game game)
        {
            _game = game;

            if (!game.CoinToss.CoinTossCompleted) return;

            ATeamName = _game.ATeam.Value.Name.Value;
            BTeamName = _game.BTeam.Value.Name.Value;

            for (int i = 0; i < _game.Set.Value; i++)
            {
                ASet[i] = new()
                {
                    Timeouts = _game.ATeam.Value.Sets[i].TimeOuts.Value,
                    Substitutions = _game.ATeam.Value.Sets[i].Substitutions.Value + _game.ATeam.Value.Sets[i].SubstitutionDetails.Where(x => x.ExceptionalSubstitution == true).Count(),

                    Points = _game.ATeam.Value.Sets[i].Points.Value,

                };
                BSet[i] = new()
                {
                    Timeouts = _game.BTeam.Value.Sets[i].TimeOuts.Value,
                    Substitutions = _game.BTeam.Value.Sets[i].Substitutions.Value + _game.BTeam.Value.Sets[i].SubstitutionDetails.Where(x => x.ExceptionalSubstitution == true).Count(),

                    Points = _game.BTeam.Value.Sets[i].Points.Value,
                };
            }
            ASet[3] = new();
            BSet[3] = new();

            ASet[3].Points = _game.ATeam.Value.Sets.Sum(x => x.Points.Value);
            ASet[3].Substitutions = _game.ATeam.Value.Sets.Sum(x => x.Substitutions.Value);
            ASet[3].Timeouts = _game.ATeam.Value.Sets.Sum(x => x.TimeOuts.Value);

            BSet[3].Points = _game.BTeam.Value.Sets.Sum(x => x.Points.Value);
            BSet[3].Substitutions = _game.BTeam.Value.Sets.Sum(x => x.Substitutions.Value);
            BSet[3].Timeouts = _game.BTeam.Value.Sets.Sum(x => x.TimeOuts.Value);

            SetDuration[0] = Duration(_game.History.Histories.Value.Where(x => x.Command1 == "WSA" || x.Command1 == "WSB").Select(x => x.DateTime).FirstOrDefault()
                , _game.History.Histories.Value.FirstOrDefault(x => x.Command1 == "S1"));

            SetDuration[1] = Duration(_game.History.Histories.Value.Where(x => x.Command1 == "WSA" || x.Command1 == "WSB").Select(x => x.DateTime).Skip(1).FirstOrDefault()
                , _game.History.Histories.Value.FirstOrDefault(x => x.Command1 == "S2"));

            SetDuration[2] = Duration(_game.History.Histories.Value.Where(x => x.Command1 == "WSA" || x.Command1 == "WSB").Select(x => x.DateTime).Skip(2).FirstOrDefault()
                , _game.History.Histories.Value.FirstOrDefault(x => x.Command1 == "S3"));

            for (int i = 0; i < 3; i++)
            {
                if (SetDuration[i] is not null)
                {
                    TotalTime += (int)SetDuration[i].Value.TotalMinutes;
                }
            }
        }
        public string ATeamName { get; set; }
        public string BTeamName { get; set; }
        public MatchResultSet[] ASet { get; set; } = new MatchResultSet[4];
        public MatchResultSet[] BSet { get; set; } = new MatchResultSet[4];
        public MatchResultSet ATotal { get; set; } = new();
        public MatchResultSet BTotal { get; set; } = new();
        public int TotalTime { get; set; } = 0;
        public TimeSpan?[] SetDuration { get; set; } = new TimeSpan?[3];

        private static TimeSpan? Duration(DateTime dt1, History? history)
        {
            if (history is null) return null;
            if (dt1 == new DateTime()) return null;
            var dt2 = history.DateTime;

            var d1 = new DateTime(dt1.Year, dt1.Month, dt1.Day, dt1.Hour, dt1.Minute, 0);
            var d2 = new DateTime(dt2.Year, dt2.Month, dt2.Day, dt2.Hour, dt2.Minute, 0);

            return d1 - d2;
        }
        public class MatchResultSet
        {
            public int? Timeouts { get; set; }
            public int? Substitutions { get; set; }
            public int? Win { get; set; }
            public int? Points { get; set; }
        }
    }
}
