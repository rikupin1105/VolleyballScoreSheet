using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VolleyballScoreSheet
{
    public class Set
    {
        public int GameSet { get; set; }
        public bool ATeamRightSide { get; set; }
        public bool ATeamServer { get; set; }
        public int ATeamTimeOutCount { get; set; }
        public int BTeamTimeOutCount { get; set; }
        public int ATeamSubstitution { get; set; }
        public int BTeamSubstitution { get; set; }
        public int[] ATeamRotation { get; set; } = new int[6];
        public int[] BTeamRotation { get; set; } = new int[6];
    }
}
