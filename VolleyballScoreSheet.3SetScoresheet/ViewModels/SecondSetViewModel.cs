using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace VolleyballScoreSheet._3SetScoresheet.ViewModels
{
    public class SecondSetViewModel : BindableBase
    {
        private readonly Game _game;
        public SecondSetViewModel(Game game)
        {
            _game = game;
            var secondSet = new Model.SecondSet(game);

            StartTime = secondSet.StartTime;
            EndTime = secondSet.EndTime;
            isEndSet = secondSet.isEndSet;

            LeftSubstitutionPoint = secondSet.LeftSubstitutionPoint;
            RightSubstitutionPoint = secondSet.RightSubstitutionPoint;

            LeftFinalPoint = secondSet.LeftFinalPoint;
            RightFinalPoint = secondSet.RightFinalPoint;

            for (int i = 0; i < Math.Min(45, secondSet.LeftPointSlash.Count); i++)
            {
                LeftPointSlash[i] = secondSet.LeftPointSlash[i];
            }
            for (int i = 0; i < Math.Min(45, secondSet.RightPointSlash.Count); i++)
            {
                RightPointSlash[i] = secondSet.RightPointSlash[i];
            }

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

            for (int i = 0; i < Math.Min(48, secondSet.LeftPointList.Count); i++)
            {
                LeftPoints[i] = secondSet.LeftPointList[i];
            }
            for (int i = 0; i < Math.Min(48, secondSet.RightPointList.Count); i++)
            {
                RightPoints[i] = secondSet.RightPointList[i];
            }

            for (int i = 0; i < Math.Min(48, secondSet.LeftServeCheckList.Count); i++)
            {
                LeftServeCheck[i] = secondSet.LeftServeCheckList[i];
            }
            for (int i = 0; i < Math.Min(48, secondSet.RightServeCheckList.Count); i++)
            {
                RightServeCheck[i] = secondSet.RightServeCheckList[i];
            }

            for (int i = 0; i < secondSet.LeftTimeouts.Count; i++)
            {
                LeftTimeouts[i] = secondSet.LeftTimeouts[i];
            }

            for (int i = 0; i < secondSet.RightTimeouts.Count; i++)
            {
                RightTimeouts[i] = secondSet.RightTimeouts[i];
            }
        }

        public string? StartTime { get; set; }
        public string? EndTime { get; set; }
        public bool isEndSet { get; set; }

        public string[] LeftSubstitutionPoint { get; set; }
        public string[] RightSubstitutionPoint { get; set; }

        public int[] LeftFinalPoint { get; set; }
        public int[] RightFinalPoint { get; set; }

        public bool[] LeftPointSlash { get; set; } = new bool[45];
        public bool[] RightPointSlash { get; set; } = new bool[45];

        public bool?[] LeftServeCheck { get; set; } = new bool?[48];
        public bool?[] RightServeCheck { get; set; } = new bool?[48];

        //サーブのところ
        public int?[] LeftPoints { get; set; } = new int?[48];
        public int?[] RightPoints { get; set; } = new int?[48];


        //タイムアウト
        public string?[] LeftTimeouts { get; set; } = new string[2];
        public string?[] RightTimeouts { get; set; } = new string[2];

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

        public Model.SecondSet.DeletePoint[] LeftDeletePoint { get; set; }
        public Model.SecondSet.DeletePoint[] RightDeletePoint { get; set; }
    }
}

