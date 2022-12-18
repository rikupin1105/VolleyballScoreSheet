using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VolleyballScoreSheet.Model
{
    public class Player : BinableBase
    {
        public Player(int id, string? name, bool isLibero = false, bool isCaptain = false)
        {
            Id=id;
            Name=name;
            IsLibero=isLibero;
            IsCaptain=isCaptain;
        }
        public int Id { get; set;}
        public string? Name { get; set; }
        public bool IsLibero { get; set; }
        public bool IsCaptain { get; set; }
        public bool IsDisqualified { get; set; } = false;
        public bool IsExceptionalSubstituted { get; set; } = false;
        public bool[] IsExplusion { get; set; } = new bool[20];
    }
}
