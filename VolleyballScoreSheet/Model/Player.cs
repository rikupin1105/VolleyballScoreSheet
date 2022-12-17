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
        private int _id;
        [Required]
        [Range(0, 99)]
        public int Id { get => _id; set => SetProperty(ref _id, value); }
        public string? Name { get; set; }
        public bool IsLibero { get; set; }
        public bool IsCaptain { get; set; }
    }
}
