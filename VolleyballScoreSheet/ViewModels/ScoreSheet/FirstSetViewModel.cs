﻿using Prism.Commands;
using Prism.Mvvm;
using Reactive.Bindings;
using Reactive.Bindings.ObjectExtensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace VolleyballScoreSheet.ViewModels.ScoreSheet
{
    public class FirstSetViewModel : BindableBase
    {
        private readonly Game _game;
        public FirstSetViewModel(Game game)
        {
            _game = game;

            if (_game.CoinToss.ATeamLeftSide)
            {
                LeftTeamName = _game.ATeam.Value.Name.Value;
                RightTeamName = _game.BTeam.Value.Name.Value;
            }
            else
            {
                RightTeamName = _game.ATeam.Value.Name.Value;
                LeftTeamName = _game.BTeam.Value.Name.Value;
            }

            if (_game.CoinToss.ATeamServer)
            {
                if (_game.CoinToss.ATeamLeftSide)
                {
                    LeftTeamServe = true;
                    LeftTeamReception = false;
                    RightPoints.Add(null);
                }
                else
                {
                    LeftTeamServe = false;
                    LeftTeamReception = true;
                    LeftPoints.Add(null);
                }
            }
            else
            {
                if (_game.CoinToss.ATeamLeftSide)
                {
                    LeftTeamServe = false;
                    LeftTeamReception = true;
                    LeftPoints.Add(null);
                }
                else
                {
                    LeftTeamServe = true;
                    LeftTeamReception = false;
                    RightPoints.Add(null);
                }
            }

            LeftTeamStartingLineUp = _game.LeftTeam.StartingLineUp.Value;
            RightTeamStartingLineUp = _game.RightTeam.StartingLineUp.Value;

            LeftTeamSubstitutioned = _game.LeftTeam.Substitutioned.Value;
            RightTeamSubstitutioned = _game.RightTeam.Substitutioned.Value;

            LeftTeamIsReturn = _game.LeftTeam.isReturn.Value;
            RightTeamIsReturn = _game.RightTeam.isReturn.Value;

            var leftfrag = false;
            var apoint = 0;
            var bpoint = 0;
            for (int i = 0; i < _game.History.Histories.Value.Count; i++)
            {
                if (_game.History.Histories.Value[i].Command1 == "S1")
                {
                    leftfrag = true;

                    StartTime = _game.History.Histories.Value[i].DateTime.ToString("HH:mm");
                }
                if (leftfrag == true && _game.History.Histories.Value[i].Command1 == "WSA")
                {
                    EndTime = _game.History.Histories.Value[i].DateTime.ToString("HH:mm");

                    isEndSet = true;
                    //セット終了
                    if (LeftPoints[^1] != apoint)
                    {
                        LeftPoints.Add(apoint);
                    }
                    if (RightPoints[^1] != bpoint)
                    {
                        RightPoints.Add(bpoint);
                    }


                    //最後の位置に丸をつける
                    LeftFinalPoint[0] = ((int)Math.Ceiling(LeftPoints.Count / 6.0) - 1) % 4;
                    LeftFinalPoint[1] = ((int)Math.Ceiling(LeftPoints.Count % 6.0)-1)*2;

                    RightFinalPoint[1] = ((int)Math.Ceiling(RightPoints.Count % 6.0)-1)*2;
                    RightFinalPoint[0]= ((int)Math.Ceiling(RightPoints.Count / 6.0) - 1) % 4;

                    if (LeftPoints.Count > 24) { LeftFinalPoint[1] += 1; }
                    if (RightPoints.Count > 24) { RightFinalPoint[1] += 1; }

                    break;
                }
                if (leftfrag)
                {
                    if (_game.History.Histories.Value[i].Command1 == "PointA")
                    {
                        apoint++;

                        if (RightPoints.Count == 0)
                        {
                            RightPoints.Add(bpoint);
                        }
                        else if (RightPoints[^1] == null && bpoint == 0)
                        {

                        }
                        else if (RightPoints[^1] != bpoint)
                        {
                            RightPoints.Add(bpoint);
                        }
                    }
                    else if (_game.History.Histories.Value[i].Command1 == "PointB")
                    {
                        bpoint++;

                        if (LeftPoints.Count == 0)
                        {
                            LeftPoints.Add(apoint);
                        }
                        else if (LeftPoints[^1] == null && apoint == 0)
                        {

                        }
                        else if (LeftPoints[^1] != apoint)
                        {
                            LeftPoints.Add(apoint);
                        }
                    }
                    else if(_game.History.Histories.Value[i].Command1 == "SubstitutionA")
                    {
                        //0が出る 1が入る
                        var a = _game.History.Histories.Value[i].Command2.Split(',');


                        var index = Array.IndexOf(LeftTeamStartingLineUp, int.Parse(a[0]));
                        if(index != -1)
                        {
                            LeftSubstitutionPoint[index*2+1] = apoint +" : "+bpoint;
                        }
                        var index1 = Array.IndexOf(LeftTeamStartingLineUp, int.Parse(a[1]));
                        if(index1 != -1)
                        {
                            LeftSubstitutionPoint[index1*2] = apoint +" : "+bpoint;
                        }
                    }
                    else if (_game.History.Histories.Value[i].Command1 == "SubstitutionB")
                    {
                        var a = _game.History.Histories.Value[i].Command2.Split(',');


                        var index = Array.IndexOf(RightTeamStartingLineUp, int.Parse(a[0]));
                        if (index != -1)
                        {
                            RightSubstitutionPoint[index*2+1] = bpoint +" : "+apoint;
                        }
                        var index1 = Array.IndexOf(RightTeamStartingLineUp, int.Parse(a[1]));
                        if (index1 != -1)
                        {
                            RightSubstitutionPoint[index1*2] = bpoint +" : "+apoint;
                        }
                    }
                    else if (_game.History.Histories.Value[i].Command1 =="TimeOutA")
                    {
                        LeftTimeouts.Add(apoint +" : "+bpoint);
                    }
                    else if (_game.History.Histories.Value[i].Command1 =="TimeOutB")
                    {
                        RightTimeouts.Add(bpoint +" : "+apoint);
                    }
                }
            }
        }

        public string[] LeftSubstitutionPoint { get; set; } = new string[12];
        public string[] RightSubstitutionPoint { get; set; } = new string[12];

        public bool isEndSet { get; set; } = false;
        public int[] LeftFinalPoint { get; set; } = new int[2];
        public int[] RightFinalPoint { get; set; } = new int[2];

        //サーブのところ
        public List<int?> LeftPoints { get; set; } = new();
        public List<int?> RightPoints { get; set; } = new();

        //タイムアウト
        public List<string> LeftTimeouts { get; set; } = new();
        public List<string> RightTimeouts { get; set; } = new();

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

    }
}
