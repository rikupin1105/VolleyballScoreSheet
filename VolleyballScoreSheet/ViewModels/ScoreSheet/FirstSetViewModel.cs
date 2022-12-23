using Prism.Commands;
using Prism.Mvvm;
using Reactive.Bindings;
using Reactive.Bindings.ObjectExtensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Windows.Media;
using VolleyballScoreSheet.Model.Scoresheet;

namespace VolleyballScoreSheet.ViewModels.ScoreSheet
{
    public class FirstSetViewModel : BindableBase
    {
        private readonly Game _game;
        public FirstSetViewModel(Game game)
        {
            _game = game;

            var firstSet = new FirstSet(game);

            StartTime = firstSet.StartTime;
            EndTime = firstSet.EndTime;
            isEndSet = firstSet.isEndSet;

            LeftSubstitutionPoint = firstSet.LeftSubstitutionPoint;
            RightSubstitutionPoint= firstSet.RightSubstitutionPoint;

            LeftFinalPoint= firstSet.LeftFinalPoint;
            RightFinalPoint= firstSet.RightFinalPoint;

            LeftPointSlash = firstSet.LeftPointSlash;
            RightPointSlash = firstSet.RightPointSlash;

            LeftTimeouts = firstSet.LeftTimeouts;
            RightTimeouts = firstSet.RightTimeouts;

            LeftTeamServe = firstSet.LeftTeamServe;
            LeftTeamReception = firstSet.LeftTeamReception;
            LeftTeamName = firstSet.LeftTeamName;
            RightTeamName = firstSet.RightTeamName;

            LeftTeamStartingLineUp = firstSet.LeftTeamStartingLineUp;
            RightTeamStartingLineUp = firstSet.RightTeamStartingLineUp;
            LeftTeamSubstitutioned = firstSet.LeftTeamSubstitutioned;
            RightTeamSubstitutioned = firstSet.RightTeamSubstitutioned;
            LeftTeamIsReturn = firstSet.LeftTeamIsReturn;
            RightTeamIsReturn = firstSet.RightTeamIsReturn;

            LeftDeletePoint = firstSet.LeftDeletePoint;
            RightDeletePoint = firstSet.RightDeletePoint;

            LeftPoints = firstSet.LeftPoints;
            RightPoints = firstSet.RightPoints;

            LeftServeCheck = firstSet.LeftServeCheck;
            RightServeCheck = firstSet.RightServeCheck;
        }

        

        public string? StartTime { get; set; }
        public string? EndTime { get; set; }
        public bool isEndSet { get; set; }

        public string[] LeftSubstitutionPoint { get; set; }
        public string[] RightSubstitutionPoint { get; set; }

        public int[] LeftFinalPoint { get; set; }
        public int[] RightFinalPoint { get; set; }

        public bool[] LeftPointSlash { get; set; }
        public bool[] RightPointSlash { get; set; }

        public bool?[] LeftServeCheck { get; set; }
        public bool?[] RightServeCheck { get; set; }

        //サーブのところ
        public int?[] LeftPoints { get; set; }
        public int?[] RightPoints { get; set; }


        //タイムアウト
        public List<string> LeftTimeouts { get; set; }
        public List<string> RightTimeouts { get; set; }

        public bool LeftTeamServe { get; set; }
        public bool LeftTeamReception { get; set; }
        public string LeftTeamName { get; set; }
        public string RightTeamName { get; set; }

        public int[] LeftTeamStartingLineUp { get; set; } = new int[6];
        public int[] RightTeamStartingLineUp { get; set; } = new int[6];

        public int?[] LeftTeamSubstitutioned { get; set; } = new int?[6];
        public int?[] RightTeamSubstitutioned { get; set; } = new int?[6];

        public bool[] LeftTeamIsReturn { get; set; } = new bool[6];
        public bool[] RightTeamIsReturn { get; set; } = new bool[6];

        public FirstSet.DeletePoint[] LeftDeletePoint { get; set; }
        public FirstSet.DeletePoint[] RightDeletePoint { get; set; }

        
    }
}
