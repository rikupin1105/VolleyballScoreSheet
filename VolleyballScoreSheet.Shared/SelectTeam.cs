using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Policy;

namespace VolleyballScoreSheet.Shared
{
    public class SelectTeam
    {
        private readonly Game _game;

        public SelectTeam(Game game)
        {
            _game=game;

        }
        public void IsLeft(bool isLeft)
        {
            _isLeft=isLeft;

            if (isLeft)
            {
                _team = _game.LeftTeam;
                _opponentTeam = _game.RightTeam;
                if (_game.isATeamLeft.Value)
                {
                    _AorB = 'A';

                }
                else
                {
                    _AorB = 'B';
                }
            }
            else
            {
                _team = _game.RightTeam;
                _opponentTeam = _game.LeftTeam;
                if (_game.isATeamLeft.Value)
                {
                    _AorB = 'B';
                }
                else
                {
                    _AorB = 'A';
                }
            }
        }

        private bool _isLeft;
        private Team _team;
        private Team _opponentTeam;
        private char _AorB;
        public ReactiveCommand AlertCommand { get; } = new();

        public void YelloCard(string mark)
        {
            _game.Sanctions.Value.Add(new Sanction
            {
                Set = _game.Set.Value,
                Warning = mark,
                Point = _team.Sets[^1].Points.Value,
                OpponentPoint = _opponentTeam.Sets[^1].Points.Value,
                Team = _AorB
            });

            _game.History.HistoryAdd($"YellowCard{_AorB}", mark);
        }
        public void RedCard(string mark)
        {
            _game.Sanctions.Value.Add(new Sanction
            {
                Set = _game.Set.Value,
                Penalty= mark,
                Point = _team.Sets[^1].Points.Value,
                OpponentPoint = _opponentTeam.Sets[^1].Points.Value,
                Team = _AorB
            });

            _game.History.HistoryAdd($"RedCard{_AorB}", mark);
            _game.PointAdd(!_isLeft, false);
        }
        public void DelayPenalty()
        {
            var dp = new DelayPenalty()
            {
                Point = _team.Sets[^1].Points.Value,
                OpponentPoint = _opponentTeam.Sets[^1].Points.Value,
                Set = _game.Set.Value
            };

            _game.Sanctions.Value.Add(new(_AorB, dp));
            _game.PointAdd(!_isLeft, false);
            _game.History.HistoryAdd($"DelayPenalty{_AorB}");
        }
        public void DelayWarning()
        {
            var dw = new DelayWarning()
            {
                Point = _team.Sets[^1].Points.Value,
                OpponentPoint = _opponentTeam.Sets[^1].Points.Value,
                Set = _game.Set.Value
            };
            _team.DelayWarning = dw;

            _game.Sanctions.Value.Add(new(_AorB, dw));
            _game.History.HistoryAdd($"DelayWarning{_AorB}");
        }
    }
}
