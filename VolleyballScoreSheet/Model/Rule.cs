using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VolleyballScoreSheet.Model
{
    public class Rule
    {
        public bool CourtChangeEnable { get; set; } = true;
        public int SetCount { get; set; } = 3;
        public int ToWinPoint { get; set; } = 25;
        public int FinalSetToWinPoint { get; set; } = 25;
        public int FinalSetCourtChangePoint { get; set; } = 13;
        public bool FinalSetCourtChanged { get; set; } = false;
    }
}
