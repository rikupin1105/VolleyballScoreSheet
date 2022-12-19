using Prism.Commands;
using Prism.Mvvm;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using VolleyballScoreSheet.Model;

namespace VolleyballScoreSheet.ViewModels.ScoreSheet
{
    public class SanctionsViewModel : BindableBase
    {
        private readonly Game _game;
        public SanctionsViewModel(Game game)
        {
            _game = game;


            foreach (var item in _game.Sanctions.Value)
            {
                Sanctions.Value.Add(new(item));
            }
            for (int i = 0; i < 10 - _game.Sanctions.Value.Count(); i++)
            {
                Sanctions.Value.Add(new());
            }
        }
        public ReactiveProperty<List<SanctionScoresheet>> Sanctions { get; set; } = new(new List<SanctionScoresheet>());
    }
    public class SanctionScoresheet
    {
        public SanctionScoresheet() { }
        public SanctionScoresheet(Sanction sanction)
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
