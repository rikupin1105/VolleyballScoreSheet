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
        public List<SubstitutionDetail> ATeamSubstitution { get; set; } = new();
        public List<SubstitutionDetail> BTeamSubstitution { get; set; } = new();
        public int[] ATeamRotation { get; set; } = new int[6];
        public int[] BTeamRotation { get; set; } = new int[6];
    }
    public class SubstitutionDetail
    {
        public int InMember { get; set; }
        public int OutMember { get; set; }
        public int Set { get; set; }
        public int Score { get; set; }
        public int OpponentScore { get; set; }
    }
}
