using Prism.Commands;
using Prism.Mvvm;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using VolleyballScoreSheet._3SetScoresheet.Model;
using VolleyballScoreSheet.Shared;
using VolleyballScoreSheet.ViewModels;

namespace VolleyballScoreSheet.ViewModels
{
    public class ScoreSheetWindowViewModel : BindableBase
    {
        public ScoreSheetWindowViewModel(Game game)
        {
            PrintCommand.Subscribe(_ => { Printer.Print(new _3SetScoresheet.ViewModels.ScoreSheetViewModel()); });
        }
        public ReactiveCommand PrintCommand { get; set; } = new();
    }
}
