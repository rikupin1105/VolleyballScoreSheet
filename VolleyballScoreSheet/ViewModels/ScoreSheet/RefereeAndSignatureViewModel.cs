using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using VolleyballScoreSheet.Model;

namespace VolleyballScoreSheet.ViewModels.ScoreSheet
{
    public class RefereeAndSignatureViewModel : BindableBase
    {
        private readonly Game _game;
        public RefereeAndSignatureViewModel(Game game)
        {
            _game=game;
            Referees = _game.Referees;
        }
        public Referees Referees { get; set; } = new();

    }
}
