using Prism.Commands;
using Prism.Mvvm;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using VolleyballScoreSheet.Model;

namespace VolleyballScoreSheet.ViewModels.ScoreSheet
{
    public class ScoreSheetWindowViewModel : BindableBase
    {
        public ScoreSheetWindowViewModel(Game game)
        {
            PrintCommand.Subscribe(_ => { Printer.Print(new ScoreSheetViewModel()); });
        }
        public ReactiveCommand PrintCommand { get; set; } = new();
    }
}
