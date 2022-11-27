using Reactive.Bindings;
using System;
using System.Collections.Generic;
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
        }
        public ReactiveCollection<Set> Sets { get; set; } = new();
        public ReactivePropertySlim<int> WinSets { get; set; } = new(0);
        public ReactivePropertySlim<string> Name { get; set; }
        public List<Player> Players { get; set; } = new List<Player>();
        public ReactivePropertySlim<string> Color { get; set; }
    }
}
