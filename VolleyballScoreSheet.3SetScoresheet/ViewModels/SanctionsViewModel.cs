using Prism.Commands;
using Prism.Mvvm;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using VolleyballScoreSheet._3SetScoresheet.Model;
using static VolleyballScoreSheet._3SetScoresheet.Model.Sanction;

namespace VolleyballScoreSheet._3SetScoresheet.ViewModels
{
    public class SanctionsViewModel : BindableBase
    {
        public SanctionsViewModel(Game game)
        {
            var sanction = new Model.Sanction(game);
            Sanctions = sanction.Sanctions;
            ImproperRequestedA = sanction.ImproperRequestedA;
            ImproperRequestedB = sanction.ImproperRequestedB;
        }
        public bool ImproperRequestedA { get; set; }
        public bool ImproperRequestedB { get; set; }
        public List<SanctionScoresheet> Sanctions { get; set; }
    }
}
