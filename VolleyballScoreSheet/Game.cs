using Microsoft.Xaml.Behaviors.Media;
using Prism.Mvvm;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VolleyballScoreSheet.Model;
using Windows.Devices.Display.Core;

namespace VolleyballScoreSheet
{
    public class Game : BindableBase
    {
        public List<string> History { get; set; } = new List<string>() { "SG", "S1" };

        public ReactivePropertySlim<bool> DisplayRequestTimeOut { get; set; } = new(false);
        public ReactivePropertySlim<bool> DisplayRotation { get; set; } = new(true);
        public void RequestTimeOut()
        {
            DisplayRotation.Value = !DisplayRotation.Value;
            DisplayRequestTimeOut.Value = !DisplayRequestTimeOut.Value;
        }

        public void Undo()
        {
            if (History.Count == 1) return;

            var c = History[^1];
            if (c[0] == 'P')
            {
                if (c[1]=='A')
                {
                    PointRemove(true);
                }
                else //B
                {
                    PointRemove(false);
                }

                return;
            }

            else if (c=="TA")
            {
                ATeam.Value.Sets[^1].TimeOut.Value++;
            }
            else if (c=="TB")
            {
                BTeam.Value.Sets[^1].TimeOut.Value++;
            }

            History.RemoveAt(History.Count - 1);
        }
        public void EndSet()
        {
            if (Rule.CourtChangeEnable)
            {
                //コートチェンジ
            }

            ATeam.Value.CreateSet();
            BTeam.Value.CreateSet();

            ATeam.ForceNotify();
            BTeam.ForceNotify();
        }
        public void Point(bool isATeam)
        {
            if (isATeam)
            {
                //ポイント追加
                ATeam.Value.Sets[^1].Point.Value++;

                //ヒストリー追加
                History.Add("PA");

                //ファイナルセット終了
                if (ATeam.Value.Sets[^1].Point.Value == Rule.SetCount &&
                    ATeam.Value.Sets[^1].Point.Value >= Rule.FinalSetToWinPoint &&
                    ATeam.Value.Sets[^1].Point.Value - BTeam.Value.Sets[^1].Point.Value>=2)
                {
                    //操作ロック
                    //END GAME
                }


                //ノーマルセット終了
                if (ATeam.Value.Sets[^1].Point.Value >= Rule.ToWinPoint &&
                    ATeam.Value.Sets[^1].Point.Value - BTeam.Value.Sets[^1].Point.Value>=2)
                {
                    ATeam.Value.WinSets.Value++;

                    //ゲーム終了
                    if (ATeam.Value.WinSets.Value == Rule.SetCount / 2 + 1)
                    {
                        //操作ロック
                        //END GAME
                    }
                    //セット終了
                    else
                    {
                        //操作ロック
                        //END SET
                        EndButtonText.Value = "END SET";
                    }
                }

                //ローテーション
                if (!isLastPointA)
                {
                    ATeam.Value.Sets[^1].Rotate();
                    NextServeTeam(true);
                }
            }
            else
            {
                BTeam.Value.Sets[^1].Point.Value++;
                History.Add("PB");

                //ローテーション
                if (isLastPointA)
                {
                    BTeam.Value.Sets[^1].Rotate();
                    NextServeTeam(false);
                }
            }

            //Debug.Value = string.Join("\n", History);
        }
        public string Debug { get; set; } = "";
        public void PointRemove(bool isATeam)
        {
            History.RemoveAt(History.Count - 1);
            Debug = string.Join("\n", History);

            if (isATeam)
            {
                ATeam.Value.Sets[^1].Point.Value--;
                for (int i = History.Count-1; i >= 0; i--)
                {
                    if (History[i]=="PA")
                    {
                        //そのまま
                        return;
                    }
                    else if (History[i]=="PB")
                    {
                        //ローテーションとサーバーを戻す
                        ATeam.Value.Sets[^1].RotateReverse();
                        NextServeTeam(false);
                        return;
                    }
                    else if (History[i][0]=='S')
                    {
                        if (History[i][1] == Rule.SetCount)
                        {
                            if (FinalSetCoinToss.ATeamServer)
                            {
                                NextServeTeam(true);
                            }
                            else
                            {
                                NextServeTeam(false);
                            }
                            return;
                        }
                        else if (History[i][1]%2==0)
                        {
                            if (CoinToss.ATeamServer)
                            {
                                NextServeTeam(false);
                            }
                            else
                            {
                                NextServeTeam(true);
                            }
                            return;
                        }
                        else if (History[i][1]%2==1)
                        {
                            if (CoinToss.ATeamServer)
                            {
                                NextServeTeam(true);
                            }
                            else
                            {
                                NextServeTeam(false);
                            }
                        }
                        return;
                    }
                }
            }
            else
            {
                BTeam.Value.Sets[^1].Point.Value--;
                for (int i = History.Count-1; i >= 0; i--)
                {
                    if (History[i]=="PB")
                    {
                        //そのまま
                        return;
                    }
                    else if (History[i]=="PA")
                    {
                        //ローテーションとサーバーを戻す
                        BTeam.Value.Sets[^1].RotateReverse();
                        NextServeTeam(true);
                        return;
                    }
                    else if (History[i][0]=='S')
                    {
                        if (History[i][1] == Rule.SetCount)
                        {
                            if (FinalSetCoinToss.ATeamServer)
                            {
                                NextServeTeam(false);
                            }
                            else
                            {
                                BTeam.Value.Sets[^1].RotateReverse();
                                NextServeTeam(true);
                            }
                            return;
                        }
                        else if (History[i][1]%2==0)
                        {
                            if (CoinToss.ATeamServer)
                            {
                                NextServeTeam(false);
                            }
                            else
                            {
                                BTeam.Value.Sets[^1].RotateReverse();
                                NextServeTeam(true);
                            }
                            return;
                        }
                        else if (History[i][1]%2==1)
                        {
                            if (CoinToss.ATeamServer)
                            {
                                BTeam.Value.Sets[^1].RotateReverse();
                                NextServeTeam(true);
                            }
                            else
                            {
                                NextServeTeam(false);
                            }
                        }
                        return;
                    }
                }
            }
        }
        public void TimeOut(bool isATeam)
        {
            if (isATeam)
            {
                //タイムアウト可能判定
                //ヒストリー追加
                //計測(Feature)
                History.Add("TA");
            }
            else
            {
                History.Add("TB");
            }
        }
        public void Substitution(bool isAteam, int In, int Out)
        {
            //Feature
        }


        public void PointAdd(bool isLeftSide)
        {
            if (isLeftSide)
            {
                if (isATeamLeft.Value)
                {
                    Point(true);
                }
                else
                {
                    Point(false);
                }
            }
            else
            {
                if (isATeamLeft.Value)
                {
                    Point(false);
                }
                else
                {
                    Point(true);
                }
            }
        }
        public void NextServeTeam(bool isATeam)
        {
            if (isATeam)
            {
                isLastPointA= true;
                if (isATeamLeft.Value)
                {
                    isLeftServe.Value = true;
                }
                else
                {
                    isLeftServe.Value = false;
                }
            }
            else
            {
                isLastPointA= false;
                if (isATeamLeft.Value)
                {
                    isLeftServe.Value = false;
                }
                else
                {
                    isLeftServe.Value = true;
                }
            }

        }

        public bool UndoEnable { get; set; } = false;
        public ReactivePropertySlim<bool> isATeamLeft { get; set; } = new(true);
        public ReactivePropertySlim<bool> isLeftServe { get; set; } = new(true);
        public ReactivePropertySlim<string> EndButtonText { get; set; } = new("END GAME");
        public Team LeftTeam
        {
            get
            {
                if (isATeamLeft.Value)
                {
                    return ATeam.Value;
                }
                else
                {
                    return BTeam.Value;
                }
            }
        }
        public Team RightTeam
        {
            get
            {
                if (isATeamLeft.Value)
                {
                    return BTeam.Value;
                }
                else
                {
                    return ATeam.Value;
                }
            }
        }
        public bool isLastPointA { get; set; } = false;

        public ReactiveProperty<Team> ATeam { get; set; } = new(new Team("ATeam", "#cd1141"));
        public ReactiveProperty<Team> BTeam { get; set; } = new(new Team("BTeam", "#0146ae"));


        public string? MatchName { get; set; } = "";
        public string? City { get; set; } = "";
        public string? Hall { get; set; } = "";
        public string? MatchNumber { get; set; } = "";
        public DateTime Date { get; set; } = DateTime.Today;



        public Referees Referees { get; set; } = new();
        public CoinToss? CoinToss { get; set; } = new();
        public CoinToss? FinalSetCoinToss { get; set; } = new();
        public Rule Rule { get; set; } = new();
    }
}
