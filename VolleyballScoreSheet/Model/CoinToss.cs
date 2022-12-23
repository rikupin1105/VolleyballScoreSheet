﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VolleyballScoreSheet.Model
{
    public class CoinToss
    {
        public bool CoinTossCompleted { get; set; } = false;
        public bool ATeamLeftSide { get; set; }
        public bool ATeamServer { get; set; }
    }
}
