using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VolleyballScoreSheet.Model
{
    public class Referees
    {
        public Referee? FirstReferee { get; set; } = new();
        public Referee? SecondReferee { get; set; } = new();
        public Referee? Scorer { get; set; } = new();
        public Referee? AssistantScorer { get; set; } = new();
        public Referee? FirstLineJudge { get; set; } = new();
        public Referee? SecondLineJudge { get; set; } = new();
        public Referee? ThirdLineJudge { get; set; } = new();
        public Referee? FourthLineJudge { get; set; } = new();
    }
    public class Referee
    {
        public string? Name { get; set; }
        public string? City { get; set; }
    }
}
