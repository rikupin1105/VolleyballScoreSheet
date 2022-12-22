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

namespace VolleyballScoreSheet.ViewModels.ScoreSheet
{
    public class FirstSetViewModel : BindableBase
    {
        private readonly Game _game;
        public FirstSetViewModel(Game game)
        {
            _game = game;
            Refresh();
        }
        public void Refresh()
        {
            if (_game.CoinToss.ATeamLeftSide)
            {
                LeftTeamName = _game.ATeam.Value.Name.Value;
                RightTeamName = _game.BTeam.Value.Name.Value;
            }
            else
            {
                LeftTeamName = _game.BTeam.Value.Name.Value;
                RightTeamName = _game.ATeam.Value.Name.Value;
            }

            if (_game.CoinToss.ATeamServer)
            {
                if (_game.CoinToss.ATeamLeftSide)
                {
                    LeftTeamServe = true;
                    LeftTeamReception = false;
                    RightPointList.Add(null);
                }
                else
                {
                    LeftTeamServe = false;
                    LeftTeamReception = true;
                    LeftPointList.Add(null);
                }
            }
            else
            {
                if (_game.CoinToss.ATeamLeftSide)
                {
                    LeftTeamServe = false;
                    LeftTeamReception = true;
                    LeftPointList.Add(null);
                }
                else
                {
                    LeftTeamServe = true;
                    LeftTeamReception = false;
                    RightPointList.Add(null);
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
                if (leftfrag == true && _game.History.Histories.Value[i].Command1 == "WSA" || _game.History.Histories.Value[i].Command1 == "WSB")
                {
                    EndTime = _game.History.Histories.Value[i].DateTime.ToString("HH:mm");

                    isEndSet = true;
                    //セット終了
                    if (LeftPointList[^1] != apoint)
                    {
                        LeftPointList.Add(apoint);
                    }
                    if (RightPointList[^1] != bpoint)
                    {
                        RightPointList.Add(bpoint);
                    }


                    //閉じる
                    LeftDeletePoint = new DeletePoint().DeletePointCulc(apoint);
                    RightDeletePoint = new DeletePoint().DeletePointCulc(bpoint);

                    //最後の位置に丸をつける
                    LeftFinalPoint = LastPosition(LeftPointList.Count);
                    RightFinalPoint = LastPosition(RightPointList.Count);

                    break;
                }
                if (leftfrag)
                {
                    if (_game.History.Histories.Value[i].Command1 == "PointA")
                    {
                        if (_game.CoinToss.ATeamLeftSide)
                        {
                            apoint++;

                            if (RightPointList.Count == 0)
                            {
                                RightPointList.Add(bpoint);
                            }
                            else if (RightPointList[^1] == null && bpoint == 0)
                            {

                            }
                            else if (RightPointList[^1] != bpoint)
                            {
                                RightPointList.Add(bpoint);
                            }
                        }
                        else
                        {
                            bpoint++;

                            if (LeftPointList.Count == 0)
                            {
                                LeftPointList.Add(apoint);
                            }
                            else if (LeftPointList[^1] == null && apoint == 0)
                            {

                            }
                            else if (LeftPointList[^1] != apoint)
                            {
                                LeftPointList.Add(apoint);
                            }
                        }
                    }
                    else if (_game.History.Histories.Value[i].Command1 == "PointB")
                    {
                        if (_game.CoinToss.ATeamLeftSide)
                        {
                            bpoint++;

                            if (LeftPointList.Count == 0)
                            {
                                LeftPointList.Add(apoint);
                            }
                            else if (LeftPointList[^1] == null && apoint == 0)
                            {

                            }
                            else if (LeftPointList[^1] != apoint)
                            {
                                LeftPointList.Add(apoint);
                            }

                        }
                        else
                        {
                            apoint++;

                            if (RightPointList.Count == 0)
                            {
                                RightPointList.Add(bpoint);
                            }
                            else if (RightPointList[^1] == null && bpoint == 0)
                            {

                            }
                            else if (RightPointList[^1] != bpoint)
                            {
                                RightPointList.Add(bpoint);
                            }
                        }
                    }
                    else if (_game.History.Histories.Value[i].Command1 == "SubstitutionA")
                    {
                        if (_game.CoinToss.ATeamLeftSide)
                        {
                            //0が出る 1が入る
                            var a = _game.History.Histories.Value[i].Command2.Split(',');


                            var index = Array.IndexOf(LeftTeamStartingLineUp, int.Parse(a[0]));
                            if (index != -1)
                            {
                                LeftSubstitutionPoint[index*2+1] = apoint +" : "+bpoint;
                            }
                            var index1 = Array.IndexOf(LeftTeamStartingLineUp, int.Parse(a[1]));
                            if (index1 != -1)
                            {
                                LeftSubstitutionPoint[index1*2] = apoint +" : "+bpoint;
                            }
                        }
                        else
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
                    }
                    else if (_game.History.Histories.Value[i].Command1 == "SubstitutionB")
                    {
                        if (_game.CoinToss.ATeamLeftSide)
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
                        else
                        {
                            //0が出る 1が入る
                            var a = _game.History.Histories.Value[i].Command2.Split(',');


                            var index = Array.IndexOf(LeftTeamStartingLineUp, int.Parse(a[0]));
                            if (index != -1)
                            {
                                LeftSubstitutionPoint[index*2+1] = apoint +" : "+bpoint;
                            }
                            var index1 = Array.IndexOf(LeftTeamStartingLineUp, int.Parse(a[1]));
                            if (index1 != -1)
                            {
                                LeftSubstitutionPoint[index1*2] = apoint +" : "+bpoint;
                            }
                        }
                    }
                    else if (_game.History.Histories.Value[i].Command1 =="TimeOutA")
                    {
                        if (_game.CoinToss.ATeamLeftSide)
                        {
                            LeftTimeouts.Add(apoint +" : "+bpoint);
                        }
                        else
                        {
                            RightTimeouts.Add(bpoint +" : "+apoint);
                        }
                    }
                    else if (_game.History.Histories.Value[i].Command1 =="TimeOutB")
                    {
                        if (_game.CoinToss.ATeamLeftSide)
                        {
                            RightTimeouts.Add(bpoint +" : "+apoint);
                        }
                        else
                        {
                            LeftTimeouts.Add(apoint +" : "+bpoint);
                        }
                    }
                }
            }
            for (int i = 0; i < apoint; i++)
            {
                LeftPointSlash[i] = true;
            }
            for (int i = 0; i < bpoint; i++)
            {
                RightPointSlash[i] = true;
            }
        }

        public int[] LastPosition(int x)
        {
            var array = new int[2];
            array[0] = ((int)Math.Ceiling(x / 6.0) - 1) % 4;
            array[1] = (x % 6 - 1) * 2;
            if (x%6==0) array[1] = 10;
            if (x>24) array[1]+=1;
            return array;
        }

        public string StartTime { get; set; }
        public string EndTime { get; set; }

        public string[] LeftSubstitutionPoint { get; set; } = new string[12];
        public string[] RightSubstitutionPoint { get; set; } = new string[12];

        public bool isEndSet { get; set; } = false;
        public int[] LeftFinalPoint { get; set; } = new int[2];
        public int[] RightFinalPoint { get; set; } = new int[2];

        public bool[] LeftPointSlash { get; set; } = new bool[45];
        public bool[] RightPointSlash { get; set; } = new bool[45];

        //サーブのところ
        private List<int?> LeftPointList { get; set; } = new();
        private List<int?> RightPointList { get; set; } = new();

        public int?[] LeftPoints
        {
            get
            {
                var array = new int?[48];

                for (int i = 0; i<LeftPointList.Count; i++)
                {
                    array[i] = LeftPointList[i];
                }

                return array;
            }
        }
        public int?[] RightPoints
        {
            get
            {
                var array = new int?[48];

                for (int i = 0; i < RightPointList.Count; i++)
                {
                    array[i] = RightPointList[i];
                }

                return array;
            }
        }


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

        public DeletePoint[] LeftDeletePoint { get; set; } = new DeletePoint[5] { new DeletePoint(), new DeletePoint(), new DeletePoint(), new DeletePoint(), new DeletePoint() };
        public DeletePoint[] RightDeletePoint { get; set; } = new DeletePoint[5] { new DeletePoint(), new DeletePoint(), new DeletePoint(), new DeletePoint(), new DeletePoint() };

        public class DeletePoint
        {
            public DeletePoint() { }
            public DeletePoint[] DeletePointCulc(int point) 
            {
                var array = new DeletePoint[5]
                {
                    new DeletePoint(),
                    new DeletePoint(),
                    new DeletePoint(),
                    new DeletePoint(),
                    new DeletePoint()
                };

                if(point <= 36)
                {
                    array[4] = new DeletePoint()
                    {
                        Visible = true
                    };
                    array[3] = new DeletePoint()
                    {
                        Visible = true,
                        StartRow = point - 27
                    };
                }
                if (point <= 27)
                {
                    array[3] = new DeletePoint()
                    {
                        Visible = true
                    };
                    array[2] = new DeletePoint()
                    {
                        Visible = true,
                        StartRow = point - 18
                    };
                }
                if (point <= 18)
                {
                    array[2] = new DeletePoint()
                    {
                        Visible = true
                    };
                    array[1] = new DeletePoint()
                    {
                        Visible = true,
                        StartRow = point - 9
                    };
                }
                if (point <= 9)
                {
                    array[1] = new DeletePoint()
                    {
                        Visible = true
                    };

                    array[0] = new DeletePoint()
                    {
                        Visible = true,
                        StartRow = point
                    };
                }

                return array;
            }
            public bool Visible { get; set; } = false;
            public int StartRow { get; set; } = 0;
            public int EndRow { get; set; } = 9;

            public int Y
            { 
                get
                {
                    if (StartRow == 0) return 1;
                    if (StartRow == 1) return 14;
                    if (StartRow == 2) return 28;
                    if (StartRow == 3) return 41;
                    if (StartRow == 4) return 54;
                    if (StartRow == 5) return 67;
                    if (StartRow == 6) return 80;
                    if (StartRow == 7) return 94;
                    if (StartRow == 8) return 107;
                    return 0;
                } 
            }
        }
    }
}
