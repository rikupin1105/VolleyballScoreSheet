using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using Unity.Injection;
using VolleyballScoreSheet.Model.Scoresheet;
using VolleyballScoreSheet.Views;
using Wpf.Ui.Interop.WinDef;
using static VolleyballScoreSheet.Model.Scoresheet.MatchResult;

namespace VolleyballScoreSheet.ViewModels.ScoreSheet
{
    public class MatchResultViewModel : BindableBase
    {
        public MatchResultViewModel(Game game)
        {
            var matchResult = new MatchResult(game);

            ATeamName = matchResult.ATeamName;
            BTeamName = matchResult.BTeamName;

            ASet = matchResult.ASet;
            BSet = matchResult.BSet;

            SetDuration = matchResult.SetDuration;
            TotalTime = matchResult.TotalTime;
        }

        public string ATeamName { get; set; }
        public string BTeamName { get; set; }
        public MatchResultSet[] ASet { get; set; } = new MatchResultSet[3];
        public MatchResultSet[] BSet { get; set; } = new MatchResultSet[3];
        public MatchResultSet ATotal { get; set; } = new();
        public MatchResultSet BTotal { get; set; } = new();
        public TimeSpan?[] SetDuration { get; set; }
        public int TotalTime { get; set; }
    }
}
