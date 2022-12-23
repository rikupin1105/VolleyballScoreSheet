using Prism.Commands;
using Prism.Mvvm;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using VolleyballScoreSheet.Model;
using VolleyballScoreSheet.Model.Scoresheet;
using static VolleyballScoreSheet.Model.Scoresheet.Sanction;

namespace VolleyballScoreSheet.ViewModels.ScoreSheet
{
    public class SanctionsViewModel : BindableBase
    {
        public SanctionsViewModel(Game game)
        {
            var sanction = new Model.Scoresheet.Sanction(game);
            Sanctions = sanction.Sanctions;
            ImproperRequestedA = sanction.ImproperRequestedA;
            ImproperRequestedB = sanction.ImproperRequestedB;
        }
        public bool ImproperRequestedA { get; set; }
        public bool ImproperRequestedB { get; set; }
        public List<SanctionScoresheet> Sanctions { get; set; }
    }
}
