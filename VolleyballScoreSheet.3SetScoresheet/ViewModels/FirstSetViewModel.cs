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
using VolleyballScoreSheet._3SetScoresheet.Model;

namespace VolleyballScoreSheet._3SetScoresheet.ViewModels
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
            RightSubstitutionPoint = firstSet.RightSubstitutionPoint;

            LeftFinalPoint = firstSet.LeftFinalPoint;
            RightFinalPoint = firstSet.RightFinalPoint;

            for (int i = 0; i < Math.Min(45, firstSet.LeftPointSlash.Count); i++)
            {
                LeftPointSlash[i] = firstSet.LeftPointSlash[i];
            }
            for (int i = 0; i < Math.Min(45, firstSet.RightPointSlash.Count); i++)
            {
                RightPointSlash[i] = firstSet.RightPointSlash[i];
            }

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

            for (int i = 0; i < Math.Min(48, firstSet.LeftPointList.Count); i++)
            {
                LeftPoints[i] = firstSet.LeftPointList[i];
            }
            for (int i = 0; i < Math.Min(48, firstSet.RightPointList.Count); i++)
            {
                RightPoints[i] = firstSet.RightPointList[i];
            }

            for (int i = 0; i < Math.Min(48, firstSet.LeftServeCheckList.Count); i++)
            {
                LeftServeCheck[i] = firstSet.LeftServeCheckList[i];
            }
            for (int i = 0; i < Math.Min(48, firstSet.RightServeCheckList.Count); i++)
            {
                RightServeCheck[i] = firstSet.RightServeCheckList[i];
            }

            for (int i = 0; i < firstSet.LeftTimeouts.Count; i++)
            {
                LeftTimeouts[i] = firstSet.LeftTimeouts[i];
            }

            for (int i = 0; i < firstSet.RightTimeouts.Count; i++)
            {
                RightTimeouts[i] = firstSet.RightTimeouts[i];
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

        public FirstSet.DeletePoint[] LeftDeletePoint { get; set; }
        public FirstSet.DeletePoint[] RightDeletePoint { get; set; }
    }
}
