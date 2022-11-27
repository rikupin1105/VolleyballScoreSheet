using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VolleyballScoreSheet.Model;
using VolleyballScoreSheet.ViewModels;

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

        public ReactivePropertySlim<int[]> Rotation { get; set; } = new();
        public void Rotate()
        {
            (Rotation.Value[0], Rotation.Value[1], Rotation.Value[2], Rotation.Value[3], Rotation.Value[4], Rotation.Value[5])
                = (Rotation.Value[1], Rotation.Value[2], Rotation.Value[3], Rotation.Value[4], Rotation.Value[5], Rotation.Value[0]);
            Rotation.ForceNotify();
        }
        public void RotateReverse()
        {
            (Rotation.Value[0], Rotation.Value[1], Rotation.Value[2], Rotation.Value[3], Rotation.Value[4], Rotation.Value[5])
                = (Rotation.Value[5], Rotation.Value[0], Rotation.Value[1], Rotation.Value[2], Rotation.Value[3], Rotation.Value[4]);
            Rotation.ForceNotify();
        }
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
