using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interop;

namespace VolleyballScoreSheet._3SetScoresheet.Model
{
    public class ThirdSet
    {
        private readonly Game _game;
        public ThirdSet(Game game)
        {
            _game = game;

            if (!game.FinalSetCoinToss!.CoinTossCompleted) return;
            if (game.Set.Value != 3) return;


            if (game.FinalSetCoinToss.ATeamLeftSide)
            {
                if (game.FinalSetCoinToss.ATeamServer)
                {
                    LeftTeamName = game.ATeam.Value.Name.Value;
                    RightTeamName = game.BTeam.Value.Name.Value;

                    LeftTeamServe = true;
                    LeftTeamReception = false;

                    LeftServeCheckList.Add(true);
                    RightServeCheckList.Add(false);
                    RightPointList.Add(null);

                    LeftTeamStartingLineUp = game.ATeam.Value.Sets[2].StartingLineUp.Value;
                    LeftTeamSubstitutioned = game.ATeam.Value.Sets[2].Substitutioned.Value;
                    LeftTeamIsReturn = game.ATeam.Value.Sets[2].isReturn.Value;

                    RightTeamStartingLineUp = game.BTeam.Value.Sets[2].StartingLineUp.Value;
                    RightTeamSubstitutioned = game.BTeam.Value.Sets[2].Substitutioned.Value;
                    RightTeamIsReturn = game.BTeam.Value.Sets[2].isReturn.Value;
                }
                else
                {
                    LeftTeamName = game.ATeam.Value.Name.Value;
                    RightTeamName = game.BTeam.Value.Name.Value;

                    LeftTeamServe = false;
                    LeftTeamReception = true;

                    LeftServeCheckList.Add(false);
                    RightServeCheckList.Add(true);
                    LeftPointList.Add(null);

                    LeftTeamStartingLineUp = game.ATeam.Value.Sets[2].StartingLineUp.Value;
                    LeftTeamSubstitutioned = game.ATeam.Value.Sets[2].Substitutioned.Value;
                    LeftTeamIsReturn = game.ATeam.Value.Sets[2].isReturn.Value;

                    RightTeamStartingLineUp = game.BTeam.Value.Sets[2].StartingLineUp.Value;
                    RightTeamSubstitutioned = game.BTeam.Value.Sets[2].Substitutioned.Value;
                    RightTeamIsReturn = game.BTeam.Value.Sets[2].isReturn.Value;
                }
            }
            else
            {
                if (game.FinalSetCoinToss.ATeamServer)
                {
                    LeftTeamName = game.BTeam.Value.Name.Value;
                    RightTeamName = game.ATeam.Value.Name.Value;

                    LeftTeamServe = false;
                    LeftTeamReception = true;

                    LeftServeCheckList.Add(false);
                    RightServeCheckList.Add(true);
                    LeftPointList.Add(null);

                    LeftTeamStartingLineUp = game.BTeam.Value.Sets[2].StartingLineUp.Value;
                    LeftTeamSubstitutioned = game.BTeam.Value.Sets[2].Substitutioned.Value;
                    LeftTeamIsReturn = game.BTeam.Value.Sets[2].isReturn.Value;

                    RightTeamStartingLineUp = game.ATeam.Value.Sets[2].StartingLineUp.Value;
                    RightTeamSubstitutioned = game.ATeam.Value.Sets[2].Substitutioned.Value;
                    RightTeamIsReturn = game.ATeam.Value.Sets[2].isReturn.Value;
                }
                else
                {
                    LeftTeamName = game.BTeam.Value.Name.Value;
                    RightTeamName = game.ATeam.Value.Name.Value;

                    LeftTeamServe = true;
                    LeftTeamReception = false;

                    LeftServeCheckList.Add(true);
                    RightServeCheckList.Add(false);
                    RightPointList.Add(null);

                    LeftTeamStartingLineUp = game.BTeam.Value.Sets[2].StartingLineUp.Value;
                    LeftTeamSubstitutioned = game.BTeam.Value.Sets[2].Substitutioned.Value;
                    LeftTeamIsReturn = game.BTeam.Value.Sets[2].isReturn.Value;

                    RightTeamStartingLineUp = game.ATeam.Value.Sets[2].StartingLineUp.Value;
                    RightTeamSubstitutioned = game.ATeam.Value.Sets[2].Substitutioned.Value;
                    RightTeamIsReturn = game.ATeam.Value.Sets[2].isReturn.Value;
                }
            }



            var leftfrag = false;
            var courtChange = false;
            var rightPoint = 0;
            var leftPoint = 0;
            for (int i = 0; i < game.History.Histories.Value.Count; i++)
            {
                if (game.History.Histories.Value[i].Command1 == "S3")
                {
                    leftfrag = true;

                    StartTime = game.History.Histories.Value[i].DateTime.ToString("HH:mm");
                }
                if (leftfrag && (game.History.Histories.Value[i].Command1 == "WSA" || game.History.Histories.Value[i].Command1 == "WSB"))
                {
                    EndTime = game.History.Histories.Value[i].DateTime.ToString("HH:mm");

                    var command = ABtoLeftRight(game.History.Histories.Value[i].Command1);

                    if (command == "WSLeft")
                    {
                        for (int k = i - 2; 0 < i; k--)
                        {
                            if (ABtoLeftRight(game.History.Histories.Value[k].Command1) == "PointLeft")
                            {
                                break;
                            }
                            else if (ABtoLeftRight(game.History.Histories.Value[k].Command1) == "PointRight")
                            {
                                LeftServeCheckList.RemoveAt(LeftServeCheckList.Count - 1);
                                break;
                            }
                        }
                    }
                    if (command == "WSRight")
                    {
                        for (int k = i - 2; 0 < i; k--)
                        {
                            if (ABtoLeftRight(game.History.Histories.Value[k].Command1) == "PointLeft")
                            {
                                RightServeCheckList.RemoveAt(LeftServeCheckList.Count - 1);
                                break;
                            }
                            else if (ABtoLeftRight(game.History.Histories.Value[k].Command1) == "PointRight")
                            {
                                break;
                            }
                        }
                    }

                    isEndSet = true;

                    //セット終了
                    if (LeftPointList.Count == 0)
                    {
                        LeftPointList.Add(leftPoint);
                    }
                    else if (LeftPointList[^1] != leftPoint)
                    {
                        LeftPointList.Add(leftPoint);
                    }

                    if (RightPointList.Count == 0)
                    {
                        RightPointList.Add(rightPoint);
                    }
                    else if (RightPointList[^1] != rightPoint)
                    {
                        RightPointList.Add(rightPoint);
                    }


                    //閉じる
                    if (courtChange)
                    {
                        OldLeftDeletePoint = new DeletePoint().DeletePointCulc(leftPoint);
                    }
                    else
                    {
                        LeftDeletePoint = new DeletePoint().DeletePointCulc(leftPoint);
                    }
                    RightDeletePoint = new DeletePoint().DeletePointCulc(rightPoint);


                    //最後の位置に丸をつける
                    LeftFinalPoint = LastPosition(LeftPointList.Count);
                    RightFinalPoint = LastPosition(RightPointList.Count);

                    break;
                }
                if (leftfrag)
                {
                    var command = ABtoLeftRight(game.History.Histories.Value[i].Command1);

                    if (command == "PointRight")
                    {
                        rightPoint++;

                        if (LeftPointList.Count == 0)
                        {
                            LeftPointList.Add(leftPoint);
                            LeftServeCheckList.Add(true);
                        }
                        else if (LeftPointList[^1] == null && leftPoint == 0)
                        {

                        }
                        else if (LeftPointList[^1] != leftPoint)
                        {
                            LeftPointList.Add(leftPoint);
                            RightServeCheckList.Add(true);
                        }
                    }
                    else if (command == "PointLeft")
                    {
                        leftPoint++;

                        if (RightPointList.Count == 0)
                        {
                            RightPointList.Add(rightPoint);
                            RightServeCheckList.Add(true);
                        }
                        else if (RightPointList[^1] == null && rightPoint == 0)
                        {

                        }
                        else if (RightPointList[^1] != rightPoint)
                        {
                            RightPointList.Add(rightPoint);
                            LeftServeCheckList.Add(true);
                        }


                    }
                    else if (command == "SubstitutionRight")
                    {
                        var a = game.History.Histories.Value[i].Command2.Split(',');


                        var index = Array.IndexOf(LeftTeamStartingLineUp, int.Parse(a[0]));
                        if (index != -1)
                        {
                            RightSubstitutionPoint[index * 2 + 1] = rightPoint + " : " + leftPoint;
                        }
                        var index1 = Array.IndexOf(LeftTeamStartingLineUp, int.Parse(a[1]));
                        if (index1 != -1)
                        {
                            RightSubstitutionPoint[index1 * 2] = rightPoint + " : " + leftPoint;
                        }
                    }
                    else if (command == "SubstitutionLeft")
                    {
                        var a = game.History.Histories.Value[i].Command2.Split(',');


                        var index = Array.IndexOf(RightTeamStartingLineUp, int.Parse(a[0]));
                        if (index != -1)
                        {
                            LeftSubstitutionPoint[index * 2 + 1] = leftPoint + " : " + rightPoint;
                        }
                        var index1 = Array.IndexOf(RightTeamStartingLineUp, int.Parse(a[1]));
                        if (index1 != -1)
                        {
                            LeftSubstitutionPoint[index1 * 2] = leftPoint + " : " + rightPoint;
                        }
                    }
                    else if (command == "TimeOutLeft")
                    {
                        if (courtChange) OldLeftTimeouts.Add(rightPoint + " : " + leftPoint);
                        else LeftTimeouts.Add(rightPoint + " : " + leftPoint);
                    }
                    else if (command == "TimeOutRight")
                    {
                        RightTimeouts.Add(leftPoint + " : " + rightPoint);
                    }
                    else if (command == "CCF")
                    {
                        courtChange = true;

                        OldLeftTimeouts.AddRange(LeftTimeouts);
                        PointOfCourtChange = leftPoint;
                    }
                }
            }
            for (int i = 0; i < rightPoint; i++)
            {
                RightPointSlash[i] = true;
            }
            for (int i = 0; i < leftPoint; i++)
            {
                if (PointOfCourtChange is null)
                {
                    LeftPointSlash[i] = true;
                }
                else if (PointOfCourtChange < i + 1)
                {
                    OldLeftPointSlash[i] = true;
                }
                else
                {
                    LeftPointSlash[i] = true;
                }
            }

            invertedT[0] = new InvertedT(PointOfCourtChange, 0);
            invertedT[1] = new InvertedT(PointOfCourtChange, 1);

            //AB判定
            if (game.CoinToss.ATeamLeftSide)
            {
                if (game.FinalSetCoinToss.ATeamLeftSide)
                {
                    isLeftA = true;
                }
                else
                {
                    isLeftA = false;
                }
            }
            else
            {
                if (game.FinalSetCoinToss.ATeamLeftSide)
                {
                    isLeftA = false;
                }
                else
                {
                    isLeftA = true;
                }
            }
        }
        private string ABtoLeftRight(string command)
        {
            if (command.Contains("A"))
            {
                if (_game.FinalSetCoinToss!.ATeamLeftSide) command = command.Replace("A", "Left");
                else command = command.Replace("A", "Right");
            }
            else if (command.Contains("B"))
            {
                if (_game.FinalSetCoinToss!.ATeamLeftSide) command = command.Replace("B", "Right");
                else command = command.Replace("B", "Left");
            }

            return command;
        }

        public string? StartTime;
        public string? EndTime;
        public bool isEndSet;
        public int? PointOfCourtChange;
        public bool? isLeftA;

        public string[] LeftSubstitutionPoint = new string[12];
        public string[] RightSubstitutionPoint = new string[12];

        public int[] LeftFinalPoint = new int[2];
        public int[] RightFinalPoint = new int[2];

        public bool[] LeftPointSlash = new bool[13];
        public bool[] RightPointSlash = new bool[32];
        public bool[] OldLeftPointSlash = new bool[32];

        private List<bool> LeftServeCheckList = new();
        private List<bool> RightServeCheckList = new();

        private List<int?> LeftPointList = new();
        private List<int?> RightPointList = new();

        public List<string> LeftTimeouts = new();
        public List<string> OldLeftTimeouts = new();
        public List<string> RightTimeouts = new();

        public int?[] LeftPoints
        {
            get
            {
                var array = new int?[48];

                for (int i = 0; i < LeftPointList.Count; i++)
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

        public bool?[] LeftServeCheck
        {
            get
            {
                var array = new bool?[48];

                for (int i = 0; i < LeftServeCheckList.Count; i++)
                {
                    array[i] = LeftServeCheckList[i];
                }

                return array;
            }
        }
        public bool?[] RightServeCheck
        {
            get
            {
                var array = new bool?[48];

                for (int i = 0; i < RightServeCheckList.Count; i++)
                {
                    array[i] = RightServeCheckList[i];
                }

                return array;
            }
        }

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

        public InvertedT[] invertedT = new InvertedT[2];

        public DeletePoint[] LeftDeletePoint = new DeletePoint[4] { new DeletePoint(), new DeletePoint(), new DeletePoint(), new DeletePoint() };
        public DeletePoint[] OldLeftDeletePoint = new DeletePoint[4] { new DeletePoint(), new DeletePoint(), new DeletePoint(), new DeletePoint() };
        public DeletePoint[] RightDeletePoint = new DeletePoint[4] { new DeletePoint(), new DeletePoint(), new DeletePoint(), new DeletePoint() };

        public int[] LastPosition(int x)
        {
            var array = new int[2];
            array[0] = ((int)Math.Ceiling(x / 6.0) - 1) % 3;
            array[1] = (x % 6 - 1) * 2;
            if (x % 6 == 0) array[1] = 10;
            if (x > 18) array[1] += 1;
            return array;
        }
        public class InvertedT
        {
            public int Y { get; set; }
            public bool Visible { get; set; } = false;
            public InvertedT(int? pointOfCourtChange, int row = 0)
            {
                if (pointOfCourtChange is null) return;

                if (pointOfCourtChange == 0)
                {

                }
                else if (row == 0)
                {
                    if (pointOfCourtChange >= 9)
                    {
                        Y = 104;
                        Visible = true;
                    }
                    else
                    {
                        Y = Calc((int)pointOfCourtChange);
                        Visible = true;
                    }
                }
                else if (row == 1)
                {
                    if (pointOfCourtChange < 9)
                    {

                    }
                    else
                    {
                        Y = Calc((int)pointOfCourtChange);
                        Visible = true;
                    }
                }
            }
            private int Calc(int pointOfCourtChange)
            {
                if (pointOfCourtChange == 13) return 65;
                if (pointOfCourtChange == 12) return 52;
                if (pointOfCourtChange == 11) return 38;
                if (pointOfCourtChange == 10) return 25;
                if (pointOfCourtChange == 9) return 12;
                if (pointOfCourtChange == 8) return 104;
                if (pointOfCourtChange == 7) return 91;
                if (pointOfCourtChange == 6) return 78;
                if (pointOfCourtChange == 5) return 65;
                if (pointOfCourtChange == 4) return 52;
                if (pointOfCourtChange == 3) return 38;
                if (pointOfCourtChange == 2) return 25;
                if (pointOfCourtChange == 1) return 12;

                return 0;
            }
        }
        public class DeletePoint
        {
            public DeletePoint() { }
            public DeletePoint[] DeletePointCulc(int point)
            {
                var array = new DeletePoint[4]
                {
                    new DeletePoint(),
                    new DeletePoint(),
                    new DeletePoint(),
                    new DeletePoint()
                };

                if (point < 32)
                {
                    array[3] = new DeletePoint()
                    {
                        Visible = true,
                        StartRow = point - 24
                    };
                }
                if (point < 24)
                {
                    array[3] = new DeletePoint()
                    {
                        Visible = true
                    };
                    array[2] = new DeletePoint()
                    {
                        Visible = true,
                        StartRow = point - 16
                    };
                }
                if (point < 16)
                {
                    array[2] = new DeletePoint()
                    {
                        Visible = true
                    };
                    array[1] = new DeletePoint()
                    {
                        Visible = true,
                        StartRow = point - 8
                    };
                }
                if (point < 8)
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
