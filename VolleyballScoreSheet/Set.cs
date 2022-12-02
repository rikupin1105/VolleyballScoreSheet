using Reactive.Bindings;
using System.Collections.Generic;
using VolleyballScoreSheet.Model;

namespace VolleyballScoreSheet
{
    public class Set : BinableBase
    {
        public ReactivePropertySlim<int> Points { get; set; } = new();
        public ReactivePropertySlim<int> TimeOuts { get; set; } = new();
        public ReactivePropertySlim<int> Substitutions { get; set; } = new();

        public List<SubstitutionDetail> SubstitutionDetails { get; set; } = new();
        public ReactivePropertySlim<int[]> StartingLineUp { get; set; } = new();
        public ReactivePropertySlim<int?[]> Substitutioned { get; set; } = new(new int?[6]);
        public ReactivePropertySlim<bool[]> isReturn { get; set; } = new(new bool[6]);

        public ReactivePropertySlim<int[]> Rotation { get; set; } = new();
    }
    public class SubstitutionDetail
    {
        public int In { get; set; }
        public int Out { get; set; }
        public int Point { get; set; }
        public int OpponentPoint { get; set; }
    }
}
