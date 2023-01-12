using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VolleyballScoreSheet.Shared;

namespace VolleyballScoreSheet._3SetScoresheet.Model
{
    public class FirstSet
    {
        public FirstSet(Game game)
        {
            if (!game.CoinToss.CoinTossCompleted) return;

            LeftTeamName = game.ATeam.Value.Name.Value;
            RightTeamName = game.BTeam.Value.Name.Value;

            if (game.CoinToss.ATeamServer)
            {
                LeftTeamServe = true;
                LeftTeamReception = false;
                LeftServeCheckList.Add(true);
                RightServeCheckList.Add(false);
                RightPointList.Add(null);
            }
            else
            {
                LeftTeamServe = false;
                LeftTeamReception = true;
                LeftServeCheckList.Add(false);
                RightServeCheckList.Add(true);
                LeftPointList.Add(null);
            }

            LeftTeamStartingLineUp = game.ATeam.Value.Sets[0].StartingLineUp.Value;
            RightTeamStartingLineUp = game.BTeam.Value.Sets[0].StartingLineUp.Value;

            LeftTeamSubstitutioned = game.ATeam.Value.Sets[0].Substitutioned.Value;
            RightTeamSubstitutioned = game.BTeam.Value.Sets[0].Substitutioned.Value;

            LeftTeamIsReturn = game.ATeam.Value.Sets[0].isReturn.Value;
            RightTeamIsReturn = game.BTeam.Value.Sets[0].isReturn.Value;

            var leftfrag = false;
            var apoint = 0;
            var bpoint = 0;
            for (int i = 0; i < game.History.Histories.Value.Count; i++)
            {
                if (game.History.Histories.Value[i].Command1 == "S1")
                {
                    leftfrag = true;

                    StartTime = game.History.Histories.Value[i].DateTime.ToString("HH:mm");
                }
                if (leftfrag == true && (game.History.Histories.Value[i].Command1 == "WSA" || game.History.Histories.Value[i].Command1 == "WSB"))
                {
                    EndTime = game.History.Histories.Value[i].DateTime.ToString("HH:mm");

                    if (game.History.Histories.Value[i].Command1 == "WSA")
                    {
                        for (int k = i - 2; 0 < i; k--)
                        {
                            if (game.History.Histories.Value[k].Command1 == "PointA")
                            {
                                break;
                            }
                            else if (game.History.Histories.Value[k].Command1 == "PointB")
                            {
                                LeftServeCheckList.RemoveAt(LeftServeCheckList.Count - 1);
                                break;
                            }
                        }
                    }
                    if (game.History.Histories.Value[i].Command1 == "WSB")
                    {
                        for (int k = i - 2; 0 < i; k--)
                        {
                            if (game.History.Histories.Value[k].Command1 == "PointA")
                            {
                                RightServeCheckList.RemoveAt(LeftServeCheckList.Count - 1);
                                break;
                            }
                            else if (game.History.Histories.Value[k].Command1 == "PointB")
                            {
                                break;
                            }
                        }
                    }

                    isEndSet = true;
                    //セット終了
                    if (LeftPointList.Count == 0)
                    {
                        LeftPointList.Add(apoint);
                    }
                    else if (LeftPointList[^1] != apoint)
                    {
                        LeftPointList.Add(apoint);
                    }

                    if (RightPointList.Count == 0)
                    {
                        RightPointList.Add(bpoint);
                    }
                    else if (RightPointList[^1] != bpoint)
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
                    if (game.History.Histories.Value[i].Command1 == "PointA")
                    {
                        apoint++;

                        if (RightPointList.Count == 0)
                        {
                            RightPointList.Add(bpoint);
                            LeftServeCheckList.Add(true);
                        }
                        else if (RightPointList[^1] == null && bpoint == 0)
                        {

                        }
                        else if (RightPointList[^1] != bpoint)
                        {
                            RightPointList.Add(bpoint);
                            LeftServeCheckList.Add(true);
                        }
                    }
                    else if (game.History.Histories.Value[i].Command1 == "PointB")
                    {
                        bpoint++;

                        if (LeftPointList.Count == 0)
                        {
                            LeftPointList.Add(apoint);
                            RightServeCheckList.Add(true);
                        }
                        else if (LeftPointList[^1] == null && apoint == 0)
                        {

                        }
                        else if (LeftPointList[^1] != apoint)
                        {
                            LeftPointList.Add(apoint);
                            RightServeCheckList.Add(true);
                        }

                    }
                    else if (game.History.Histories.Value[i].Command1 == "SubstitutionA")
                    {
                        var a = game.History.Histories.Value[i].Command2.Split(',');


                        var index = Array.IndexOf(LeftTeamStartingLineUp, int.Parse(a[0]));
                        if (index != -1)
                        {
                            LeftSubstitutionPoint[index * 2 + 1] = apoint + " : " + bpoint;
                        }
                        var index1 = Array.IndexOf(LeftTeamStartingLineUp, int.Parse(a[1]));
                        if (index1 != -1)
                        {
                            LeftSubstitutionPoint[index1 * 2] = apoint + " : " + bpoint;
                        }
                    }
                    else if (game.History.Histories.Value[i].Command1 == "SubstitutionB")
                    {
                        var a = game.History.Histories.Value[i].Command2.Split(',');


                        var index = Array.IndexOf(RightTeamStartingLineUp, int.Parse(a[0]));
                        if (index != -1)
                        {
                            RightSubstitutionPoint[index * 2 + 1] = bpoint + " : " + apoint;
                        }
                        var index1 = Array.IndexOf(RightTeamStartingLineUp, int.Parse(a[1]));
                        if (index1 != -1)
                        {
                            RightSubstitutionPoint[index1 * 2] = bpoint + " : " + apoint;
                        }
                    }
                    else if (game.History.Histories.Value[i].Command1 == "TimeOutA")
                    {
                        LeftTimeouts.Add(apoint + " : " + bpoint);
                    }
                    else if (game.History.Histories.Value[i].Command1 == "TimeOutB")
                    {
                        RightTimeouts.Add(bpoint + " : " + apoint);
                    }
                }
            }
            for (int i = 0; i < apoint; i++)
            {
                LeftPointSlash.Add(true);
            }
            for (int i = 0; i < bpoint; i++)
            {
                RightPointSlash.Add(true);
            }


            if (!game.CoinToss.ATeamLeftSide)
            {
                (LeftPointList, RightPointList) = (RightPointList, LeftPointList);
                (LeftTimeouts, RightTimeouts) = (RightTimeouts, LeftTimeouts);
                (LeftSubstitutionPoint, RightSubstitutionPoint) = (RightSubstitutionPoint, LeftSubstitutionPoint);
                (LeftPointSlash, RightPointSlash) = (RightPointSlash, LeftPointSlash);
                (LeftFinalPoint, RightFinalPoint) = (RightFinalPoint, LeftFinalPoint);
                (LeftServeCheckList, RightServeCheckList) = (RightServeCheckList, LeftServeCheckList);
                (LeftDeletePoint, RightDeletePoint) = (RightDeletePoint, LeftDeletePoint);

                (LeftTeamName, RightTeamName) = (RightTeamName, LeftTeamName);
                (LeftTeamStartingLineUp, RightTeamStartingLineUp) = (RightTeamStartingLineUp, LeftTeamStartingLineUp);
                (LeftTeamSubstitutioned, RightTeamSubstitutioned) = (RightTeamSubstitutioned, LeftTeamSubstitutioned);
                (LeftTeamIsReturn, RightTeamIsReturn) = (RightTeamIsReturn, LeftTeamIsReturn);

                LeftTeamServe = !LeftTeamServe;
                LeftTeamReception = !LeftTeamReception;
            }
        }
        public string? StartTime;
        public string? EndTime;
        public bool isEndSet;

        public string[] LeftSubstitutionPoint = new string[12];
        public string[] RightSubstitutionPoint = new string[12];

        public int[] LeftFinalPoint = new int[2];
        public int[] RightFinalPoint = new int[2];

        public List<bool> LeftPointSlash = new();
        public List<bool> RightPointSlash = new();

        public List<bool> LeftServeCheckList = new();
        public List<bool> RightServeCheckList = new();

        public List<int?> LeftPointList = new();
        public List<int?> RightPointList = new();

        public List<string> LeftTimeouts = new();
        public List<string> RightTimeouts = new();

        public bool LeftTeamServe;
        public bool LeftTeamReception;
        public string LeftTeamName;
        public string RightTeamName;

        public int[] LeftTeamStartingLineUp;
        public int[] RightTeamStartingLineUp;

        public int?[] LeftTeamSubstitutioned;
        public int?[] RightTeamSubstitutioned;

        public bool[] LeftTeamIsReturn;
        public bool[] RightTeamIsReturn;

        public DeletePoint[] LeftDeletePoint = new DeletePoint[5] { new DeletePoint(), new DeletePoint(), new DeletePoint(), new DeletePoint(), new DeletePoint() };
        public DeletePoint[] RightDeletePoint = new DeletePoint[5] { new DeletePoint(), new DeletePoint(), new DeletePoint(), new DeletePoint(), new DeletePoint() };

        public int[] LastPosition(int x)
        {
            var array = new int[2];
            array[0] = ((int)Math.Ceiling(x / 6.0) - 1) % 4;
            array[1] = (x % 6 - 1) * 2;
            if (x % 6 == 0) array[1] = 10;
            if (x > 24) array[1] += 1;
            return array;
        }
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

                if (point < 36)
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
                if (point < 27)
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
                if (point < 18)
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
                if (point < 9)
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
