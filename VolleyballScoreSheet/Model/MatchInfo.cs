using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VolleyballScoreSheet.Model
{
    public class MatchInfo
    {
        public string MatchName { get; set; } = "2022男子バレーボール世界選手権壮行試合 日本代表紅白戦 in 沖縄";
        public string? City { get; set; } = "那覇市";
        public string? Hall { get; set; } = "沖縄アリーナ";
        public string? MatchNumber { get; set; } = "1";
        public DateTime Date { get; set; } = DateTime.Parse("2022/07/30");
        public Sex Sex { get; set; } = Sex.Men;
    }
}
