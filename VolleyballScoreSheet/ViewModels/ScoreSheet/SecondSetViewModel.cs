using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using VolleyballScoreSheet.Model.Scoresheet;
using VolleyballScoreSheet.Views.ScoreSheet;

namespace VolleyballScoreSheet.ViewModels.ScoreSheet
{
    public class SecondSetViewModel : BindableBase
    {
        private readonly Game _game;
        public SecondSetViewModel(Game game)
        {
            _game = game;
            var secondSet = new Model.Scoresheet.SecondSet(game);

            StartTime = secondSet.StartTime;
            EndTime = secondSet.EndTime;
            isEndSet = secondSet.isEndSet;

            LeftSubstitutionPoint = secondSet.LeftSubstitutionPoint;
            RightSubstitutionPoint= secondSet.RightSubstitutionPoint;

            LeftFinalPoint= secondSet.LeftFinalPoint;
            RightFinalPoint= secondSet.RightFinalPoint;

            LeftPointSlash = secondSet.LeftPointSlash;
            RightPointSlash = secondSet.RightPointSlash;

            LeftTimeouts = secondSet.LeftTimeouts;
            RightTimeouts = secondSet.RightTimeouts;

            LeftTeamServe = secondSet.LeftTeamServe;
            LeftTeamReception = secondSet.LeftTeamReception;
            LeftTeamName = secondSet.LeftTeamName;
            RightTeamName = secondSet.RightTeamName;

            LeftTeamStartingLineUp = secondSet.LeftTeamStartingLineUp;
            RightTeamStartingLineUp = secondSet.RightTeamStartingLineUp;
            LeftTeamSubstitutioned = secondSet.LeftTeamSubstitutioned;
            RightTeamSubstitutioned = secondSet.RightTeamSubstitutioned;
            LeftTeamIsReturn = secondSet.LeftTeamIsReturn;
            RightTeamIsReturn = secondSet.RightTeamIsReturn;

            LeftDeletePoint = secondSet.LeftDeletePoint;
            RightDeletePoint = secondSet.RightDeletePoint;

            LeftPoints = secondSet.LeftPoints;
            RightPoints = secondSet.RightPoints;

            LeftServeCheck = secondSet.LeftServeCheck;
            RightServeCheck = secondSet.RightServeCheck;
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

        public Model.Scoresheet.SecondSet.DeletePoint[] LeftDeletePoint { get; set; }
        public Model.Scoresheet.SecondSet.DeletePoint[] RightDeletePoint { get; set; }
    }
}

