using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VolleyballScoreSheet.Model
{
    public class Player
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public bool IsLebero { get; set; }
        public bool IsCaptain { get; set; }
    }
}
