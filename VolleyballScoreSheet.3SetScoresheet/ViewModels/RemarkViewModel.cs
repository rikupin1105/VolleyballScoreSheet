using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace VolleyballScoreSheet._3SetScoresheet.ViewModels
{
    public class RemarkViewModel : BindableBase
    {
        private readonly Game _game;
        public RemarkViewModel(Game game)
        {
            _game = game;
            RemarkText = string.Join("\n", _game.Remarks);
        }
        public string RemarkText { get; set; }
    }
}
