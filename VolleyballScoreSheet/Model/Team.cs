using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
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
            Sets[^1].Point.Value+=i;
            Refresh();
        }
        public void TimeOut(int i = 1)
        {
            Sets[^1].TimeOut.Value += i;
            Refresh();
        }
        
        public void Refresh()
        {
            Points.Value = Sets[^1].Point.Value;
            Timeouts.Value = Sets[^1].TimeOut.Value;
            Substitutions.Value = Sets[^1].Substitution.Value;
            Rotation.Value = Sets[^1].Rotation.Value;
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

        //現在の状況
        public ReactivePropertySlim<int> Points { get; } = new();
        public ReactivePropertySlim<int> Timeouts { get; } = new();
        public ReactivePropertySlim<int> Substitutions { get; } = new();
        public ReactivePropertySlim<int[]> Rotation { get; } = new();
        public ReactivePropertySlim<int[]> StartingLineUp { get; } = new();

        public ObservableCollection<Set> Sets { get; set; } = new();
        public ReactivePropertySlim<int> WinSets { get; set; } = new(0);
        public ReactivePropertySlim<string> Name { get; set; }
        public List<Player> Players { get; set; } = new List<Player>();
        public ReactivePropertySlim<string> Color { get; set; }
    }
}
