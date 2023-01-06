using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace VolleyballScoreSheet.ViewModels.ScoreSheet
{
    public class ThirdSetViewModel : BindableBase
    {
        private readonly Game _game;
        public ThirdSetViewModel(Game game)
        {
            _game = game;
            var thirdSet = new Model.Scoresheet.ThirdSet(game);

            StartTime = thirdSet.StartTime;
            EndTime = thirdSet.EndTime;
            isEndSet = thirdSet.isEndSet;
            PointOfCourtChange = thirdSet.PointOfCourtChange;

            LeftSubstitutionPoint = thirdSet.LeftSubstitutionPoint;
            RightSubstitutionPoint= thirdSet.RightSubstitutionPoint;

            LeftFinalPoint= thirdSet.LeftFinalPoint;
            RightFinalPoint= thirdSet.RightFinalPoint;

            LeftPointSlash = thirdSet.LeftPointSlash;
            RightPointSlash = thirdSet.RightPointSlash;

            OldLeftPointSlash = thirdSet.OldLeftPointSlash;


            LeftTeamServe = thirdSet.LeftTeamServe;
            LeftTeamReception = thirdSet.LeftTeamReception;
            LeftTeamName = thirdSet.LeftTeamName;
            RightTeamName = thirdSet.RightTeamName;

            LeftTeamStartingLineUp = thirdSet.LeftTeamStartingLineUp;
            RightTeamStartingLineUp = thirdSet.RightTeamStartingLineUp;
            LeftTeamSubstitutioned = thirdSet.LeftTeamSubstitutioned;
            RightTeamSubstitutioned = thirdSet.RightTeamSubstitutioned;
            LeftTeamIsReturn = thirdSet.LeftTeamIsReturn;
            RightTeamIsReturn = thirdSet.RightTeamIsReturn;

            LeftDeletePoint = thirdSet.LeftDeletePoint;
            RightDeletePoint = thirdSet.RightDeletePoint;
            OldLeftDeletePoint = thirdSet.OldLeftDeletePoint;
            

            LeftPoints = thirdSet.LeftPoints;
            RightPoints = thirdSet.RightPoints;

            LeftServeCheck = thirdSet.LeftServeCheck;
            RightServeCheck = thirdSet.RightServeCheck;

            InvertedT = thirdSet.invertedT;

            for (int i = 0; i <  thirdSet.LeftTimeouts.Count; i++)
            {
                LeftTimeouts[i] =  thirdSet.LeftTimeouts[i];
            }

            for (int i = 0; i <  thirdSet.RightTimeouts.Count; i++)
            {
                RightTimeouts[i] =  thirdSet.RightTimeouts[i];
            }

            for (int i = 0; i < thirdSet.OldLeftTimeouts.Count; i++)
            {
                OldLeftTimeouts[i] = thirdSet.OldLeftTimeouts[i];
            }

            if (thirdSet.isLeftA != null)
            {
                if (thirdSet.isLeftA == true)
                {
                    ABLeft = "A";
                    ABRight = "B";
                }
                else
                {
                    ABLeft = "B";
                    ABRight = "A";
                }
            }
        }
        public string ABLeft { get; set; }
        public string ABRight { get; set; }
        public string? StartTime { get; set; }
        public string? EndTime { get; set; }
        public bool isEndSet { get; set; }
        public int? PointOfCourtChange { get; set; }

        public string[] LeftSubstitutionPoint { get; set; }
        public string[] RightSubstitutionPoint { get; set; }

        public int[] LeftFinalPoint { get; set; }
        public int[] RightFinalPoint { get; set; }

        public bool[] LeftPointSlash { get; set; }
        public bool[] RightPointSlash { get; set; }
        public bool[] OldLeftPointSlash { get; set; }

        public bool?[] LeftServeCheck { get; set; }
        public bool?[] RightServeCheck { get; set; }

        //サーブのところ
        public int?[] LeftPoints { get; set; }
        public int?[] RightPoints { get; set; }


        //タイムアウト
        public string?[] LeftTimeouts { get; set; } = new string[2];
        public string?[] RightTimeouts { get; set; } = new string[2];
        public string?[] OldLeftTimeouts { get; set; } = new string[2];

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

        public Model.Scoresheet.ThirdSet.InvertedT[] InvertedT { get; set; }
        public Model.Scoresheet.ThirdSet.DeletePoint[] LeftDeletePoint { get; set; }
        public Model.Scoresheet.ThirdSet.DeletePoint[] RightDeletePoint { get; set; }
        public Model.Scoresheet.ThirdSet.DeletePoint[] OldLeftDeletePoint { get; set; }
    }
}

