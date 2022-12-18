using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VolleyballScoreSheet.Model
{
    public class Team
    {
        public Team(string name, string color)
        {
            Name = new(name);
            Color = new(color);
        }
        public void CreateSet()
        {
            Sets.Add(new());
            Refresh();
        }
        public void DeleteSet()
        {
            if (Sets.Count>=2)
            {
                Sets.RemoveAt(Sets.Count-1);
            }
            Refresh();
        }
        public void Point(int i = 1)
        {
            Sets[^1].Points.Value+=i;
            Refresh();
        }
        public void TimeOut(int i = 1)
        {
            Sets[^1].TimeOuts.Value += i;
            Refresh();
        }

        public void Refresh()
        {
            Points.Value = Sets[^1].Points.Value;
            Timeouts.Value = Sets[^1].TimeOuts.Value;
            Substitutions.Value = Sets[^1].Substitutions.Value;
            Rotation.Value = Sets[^1].Rotation.Value;
            Substitutioned.Value = Sets[^1].Substitutioned.Value;
            isReturn.Value = Sets[^1].isReturn.Value;

            isReturn.ForceNotify();
            Substitutioned.ForceNotify();
            Rotation.ForceNotify();
        }
        public void Rotate()
        {
            (Sets[^1].Rotation.Value[0], Sets[^1].Rotation.Value[1], Sets[^1].Rotation.Value[2], Sets[^1].Rotation.Value[3], Sets[^1].Rotation.Value[4], Sets[^1].Rotation.Value[5])
                = (Sets[^1].Rotation.Value[1], Sets[^1].Rotation.Value[2], Sets[^1].Rotation.Value[3], Sets[^1].Rotation.Value[4], Sets[^1].Rotation.Value[5], Sets[^1].Rotation.Value[0]);
            Rotation.Value = Sets[^1].Rotation.Value;

            Rotation.ForceNotify();
        }
        public void RotateReverse()
        {
            (Sets[^1].Rotation.Value[0], Sets[^1].Rotation.Value[1], Sets[^1].Rotation.Value[2], Sets[^1].Rotation.Value[3], Sets[^1].Rotation.Value[4], Sets[^1].Rotation.Value[5])
                = (Sets[^1].Rotation.Value[5], Sets[^1].Rotation.Value[0], Sets[^1].Rotation.Value[1], Sets[^1].Rotation.Value[2], Sets[^1].Rotation.Value[3], Sets[^1].Rotation.Value[4]);
            Rotation.Value = Sets[^1].Rotation.Value;

            Rotation.ForceNotify();
        }

        public void MedamaRefresh()
        {
            Sets[^1].Substitutioned.Value = new int?[6];
            Sets[^1].isReturn.Value = new bool[6];

            foreach (var item in Sets[^1].SubstitutionDetails)
            {
                int k;
                if (Sets[^1].StartingLineUp.Value.Contains(item.In))
                {
                    k = item.In;
                }
                else
                {
                    k = item.Out;
                }

                var i = Array.IndexOf(Sets[^1].StartingLineUp.Value, k);

                if (Sets[^1].Substitutioned.Value[i] is not null)
                {
                    //return
                    Sets[^1].isReturn.Value[i] = true;
                }
                else
                {
                    Sets[^1].isReturn.Value[i] = false;
                    Sets[^1].Substitutioned.Value[i] = item.In;
                }
            }
            Refresh();
        }
        public void Substitution(int In, int Out, int Point, int OpponentPoint)
        {
            Sets[^1].SubstitutionDetails.Add(new()
            {
                In = In,
                Out = Out,
                Point = Point,
                OpponentPoint = OpponentPoint
            });

            Sets[^1].Substitutions.Value++;




            Sets[^1].Rotation.Value[Array.IndexOf(Sets[^1].Rotation.Value, Out)] = In;

            MedamaRefresh();
            Refresh();
        }
        public void ExceptionalSubstitution(int In,int Out, int Point, int OpponentPoint)
        {
            Sets[^1].SubstitutionDetails.Add(new()
            {
                ExceptionalSubstitution = true,
                In = In,
                Out = Out,
                Point = Point,
                OpponentPoint = OpponentPoint
            }); ;

            Sets[^1].Rotation.Value[Array.IndexOf(Sets[^1].Rotation.Value, Out)] = In;
            Refresh();
        }

        //現在の状況
        public ReactivePropertySlim<int> Points { get; } = new();
        public ReactivePropertySlim<int> Timeouts { get; } = new();
        public ReactivePropertySlim<int> Substitutions { get; } = new();
        public ReactivePropertySlim<int[]> Rotation { get; } = new();
        public ReactivePropertySlim<int[]> StartingLineUp { get; } = new();
        public ReactivePropertySlim<int?[]> Substitutioned { get; set; } = new(new int?[6]);
        public ReactivePropertySlim<bool[]> isReturn { get; set; } = new(new bool[6]);

        public ObservableCollection<Set> Sets { get; set; } = new();
        public ReactivePropertySlim<int> WinSets { get; set; } = new(0);
        public ReactivePropertySlim<string> Name { get; set; }
        public List<Player> Players { get; set; } = new List<Player>();
        public ReactivePropertySlim<string> Color { get; set; }
        public DelayWarning? DelayWarning { get; set; }
        public List<DelayPenalty> DelayPenalties { get; set; } = new();
        public ReactivePropertySlim<bool> ImproperRequests { get; set; } = new(false);
    }
    public class DelayWarning
    {
        public int Point;
        public int OpponentPoint;
        public int Set;
    }
    public class DelayPenalty
    {
        public int Point;
        public int OpponentPoint;
        public int Set;
    }
}
