using Reactive.Bindings;
using VolleyballScoreSheet.Model;

namespace VolleyballScoreSheet
{
    public class Set : BinableBase
    {
        public ReactivePropertySlim<int> Point { get; set; } = new();
        public ReactivePropertySlim<int> GameSet { get; set; } = new();
        public ReactivePropertySlim<int> TimeOut { get; set; } = new();
        public ReactivePropertySlim<int> Substitution { get; set; } = new();

        //public List<SubstitutionDetail> ATeamSubstitution { get; set; } = new();
        //public List<SubstitutionDetail> BTeamSubstitution { get; set; } = new();

        public ReactivePropertySlim<int[]> StartingLineUp { get; set; } = new();
        public ReactivePropertySlim<int[]> Rotation { get; set; } = new();
    }
    //public class SubstitutionDetail
    //{
    //    public int InMember { get; set; }
    //    public int OutMember { get; set; }
    //    public int Set { get; set; }
    //    public int Score { get; set; }
    //    public int OpponentScore { get; set; }
    //}
}
