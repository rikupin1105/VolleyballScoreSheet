using Microsoft.Xaml.Behaviors.Media;
using Prism.Mvvm;
using Prism.Regions;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VolleyballScoreSheet.Model;
using VolleyballScoreSheet.Views;
using Windows.Devices.Display.Core;

namespace VolleyballScoreSheet
{
    public class Game : BindableBase
    {
        public Game()
        {
            History.Subscribe(x =>
            {
                if (x.Count>1) UndoEnable.Value = true;
                else UndoEnable.Value = false;
                Debug.Value = string.Join(" , ", x);
            });

            ATeam.Value.CreateSet();
            BTeam.Value.CreateSet();
        }
        public ReactiveProperty<List<string>> History { get; } = new(new List<string>() { "SG" });
        public void HistoryAdd(string s)
        {
            History.Value.Add(s);
            History.ForceNotify();
        }
        public void HistoryRemove()
        {
            History.Value.RemoveAt(History.Value.Count - 1);

            History.ForceNotify();
        }
        public ReactivePropertySlim<int> Set { get; set; } = new(1);


        public ReactivePropertySlim<bool> IsEnablePoint { get; set; } = new(false);
        public ReactivePropertySlim<bool> IsEnableTimeout { get; set; } = new(false);

        public ReactivePropertySlim<bool> DisplayCoinToss { get; set; } = new(true);
        public ReactivePropertySlim<bool> DisplayBeforeMatch { get; set; } = new(false);
        public ReactivePropertySlim<bool> DisplayRotation { get; set; } = new(false);
        public ReactivePropertySlim<bool> DisplayRequestTimeOut { get; set; } = new(false);

        public ReactivePropertySlim<string> Debug { get; set; } = new();

        public void DisplayMain(string s)
        {
            DisplayRotation.Value = false;
            DisplayRequestTimeOut.Value = false;
            DisplayBeforeMatch.Value = false;
            DisplayCoinToss.Value = false;

            if (s=="Rotation")
            {
                IsEnableTimeout.Value=true;
                IsEnablePoint.Value=true;

                DisplayRotation.Value = true;
            }
            else if (s=="TimeOut")
            {
                IsEnableTimeout.Value = false;
                IsEnablePoint.Value = false;

                DisplayRequestTimeOut.Value = true;
            }
            else if (s=="BeforeMatch")
            {
                DisplayBeforeMatch.Value = true;
            }
            else if (s=="CoinToss")
            {
                IsEnableTimeout.Value=false;
                IsEnablePoint.Value=false;

                DisplayCoinToss.Value = true;
            }
        }

        public void RequestTimeOut()
        {
            DisplayMain("TimeOut");
        }
        public void TimeOut(bool isATeam)
        {
            if (isATeam)
            {
                //タイムアウト可能判定
                //ヒストリー追加
                //計測(Feature)
                if (ATeam.Value.Sets[^1].TimeOut.Value>=2)
                {
                    //タイムアウト不可
                }
                else
                {
                    ATeam.Value.TimeOut();
                    HistoryAdd("TA");
                }
            }
            else
            {
                if (BTeam.Value.Sets[^1].TimeOut.Value>=2)
                {
                    //タイムアウト不可
                }
                else
                {
                    BTeam.Value.TimeOut();
                    HistoryAdd("TB");
                }
            }

            //ローテーション表示に戻す
            DisplayMain("Rotation");
        }
        public void TimeOutSide(bool isLeftTeam)
        {
            if (isLeftTeam)
            {
                if (isATeamLeft.Value)
                {
                    TimeOut(true);
                }
                else
                {
                    TimeOut(false);
                }
            }
            else
            {
                if (isATeamLeft.Value)
                {
                    TimeOut(false);
                }
                else
                {
                    TimeOut(true);
                }
            }
        }
        public void CancelTimeOut()
        {
            DisplayMain("Rotation");
        }

        public void Undo()
        {
            //PA Aチームの得点
            //PB Bチームの得点
            //TA Aチームのタイムアウト
            //TB Bチームのタイムアウト

            if (History.Value.Count == 1)
            {
                return;
            }

            var c = History.Value[^1];
            if (c[0] == 'P')
            {
                HistoryRemove();
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
            else if (c[0]=='S')
            {
                var set = int.Parse(c[1].ToString());
                HistoryRemove();

                DisplayMain("BeforeMatch");
                IsEnableTimeout.Value= false;
                IsEnablePoint.Value=false;

                return;
            }
            else if (c[0]=='G' && c[1]=='S')
            {
                HistoryRemove();
                ATeam.Value.DeleteSet();
                BTeam.Value.DeleteSet();

                DisplayMain("Rotation");

                IsEnablePoint.Value=false;
                IsEnableTimeout.Value=false;

                Set.Value--;

                if (History.Value[^1]=="PA")
                {
                    ATeam.Value.WinSets.Value--;
                    NextServeTeam(true);
                }
                else if (History.Value[^1]=="PB")
                {
                    BTeam.Value.WinSets.Value--;
                    NextServeTeam(false);
                }

                if (Rule.CourtChangeEnable)
                {
                    CourtChange();
                }

                return;
            }

            else if (c=="TA")
            {
                ATeam.Value.TimeOut(-1);
            }
            else if (c=="TB")
            {
                BTeam.Value.TimeOut(-1);
            }

            HistoryRemove();
        }
        public void EndSet()
        {
            //ATeam.Value.Sets[^1].Point.ForceNotify();

            HistoryAdd("GS"+Set.Value);
            Set.Value++;

            ATeam.Value.CreateSet();
            BTeam.Value.CreateSet();

            if(Set.Value==Rule.SetCount)
            {
                //コートチェンジせずにコイントスへ
                DisplayMain("CoinToss");
            }
            else if (Rule.CourtChangeEnable)
            {
                //コートチェンジ
                CourtChange();
            }
            
            DisplayMain("BeforeMatch");
        }

        public void Point(bool isATeam)
        {
            if (isATeam)
            {
                //ポイント追加
                ATeam.Value.Point();

                //ヒストリー追加
                HistoryAdd("PA");

                //ファイナルセット終了
                if (ATeam.Value.Sets[^1].Point.Value == Rule.SetCount &&
                    ATeam.Value.Sets[^1].Point.Value >= Rule.FinalSetToWinPoint &&
                    ATeam.Value.Sets[^1].Point.Value - BTeam.Value.Sets[^1].Point.Value>=2)
                {
                    //操作ロック
                    //END GAME

                    IsEnablePoint.Value = false;
                    IsEnableTimeout.Value = false;

                    EndButtonText.Value = "END GAME";
                }
                //ノーマルセット終了
                else if (ATeam.Value.Sets[^1].Point.Value >= Rule.ToWinPoint &&
                    ATeam.Value.Sets[^1].Point.Value - BTeam.Value.Sets[^1].Point.Value>=2)
                {
                    ATeam.Value.WinSets.Value++;

                    //ゲーム終了
                    if (ATeam.Value.WinSets.Value == Rule.SetCount / 2 + 1)
                    {
                        //操作ロック
                        //END GAME

                        IsEnablePoint.Value = false;
                        IsEnableTimeout.Value = false;

                        EndButtonText.Value = "END GAME";
                    }
                    //セット終了
                    else
                    {
                        //操作ロック
                        //END SET
                        IsEnablePoint.Value = false;
                        IsEnableTimeout.Value = false;

                        EndButtonText.Value = "END SET";
                    }
                }
                else
                {
                    IsEnablePoint.Value = true;
                    IsEnableTimeout.Value = true;
                }

                //ローテーション
                if (!isLastPointA)
                {
                    ATeam.Value.Rotate();
                    NextServeTeam(true);
                }
            }
            else
            {
                //ポイント追加
                BTeam.Value.Point();

                //ヒストリー追加
                HistoryAdd("PB");

                //ファイナルセット終了
                if (BTeam.Value.Sets[^1].Point.Value == Rule.SetCount &&
                    BTeam.Value.Sets[^1].Point.Value >= Rule.FinalSetToWinPoint &&
                    BTeam.Value.Sets[^1].Point.Value - ATeam.Value.Sets[^1].Point.Value>=2)
                {
                    //操作ロック
                    //END GAME

                    IsEnablePoint.Value = false;
                    IsEnableTimeout.Value = false;

                    EndButtonText.Value = "END GAME";
                }
                //ノーマルセット終了
                else if (BTeam.Value.Sets[^1].Point.Value >= Rule.ToWinPoint &&
                    BTeam.Value.Sets[^1].Point.Value - ATeam.Value.Sets[^1].Point.Value>=2)
                {
                    BTeam.Value.WinSets.Value++;

                    //ゲーム終了
                    if (BTeam.Value.WinSets.Value == Rule.SetCount / 2 + 1)
                    {
                        //操作ロック
                        //END GAME

                        IsEnablePoint.Value = false;
                        IsEnableTimeout.Value = false;

                        EndButtonText.Value = "END GAME";
                    }
                    //セット終了
                    else
                    {
                        //操作ロック
                        //END SET
                        IsEnablePoint.Value = false;
                        IsEnableTimeout.Value = false;

                        EndButtonText.Value = "END SET";
                    }
                }
                else
                {
                    IsEnablePoint.Value = true;
                    IsEnableTimeout.Value = true;
                }

                //ローテーション
                if (isLastPointA)
                {
                    BTeam.Value.Rotate();
                    NextServeTeam(false);
                }
            }

            //Debug.Value = string.Join("\n", History);
        }
        public void CourtChange()
        {
            isATeamLeft.Value = !isATeamLeft.Value;
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
        public void PointRemove(bool isATeam)
        {
            if (isATeam)
            {
                ATeam.Value.Point(-1);

                IsEnablePoint.Value = true;
                IsEnableTimeout.Value = true;
                for (int i = History.Value.Count-1; i >= 0; i--)
                {
                    if (History.Value[i]=="PA")
                    {
                        //そのまま
                        return;
                    }
                    else if (History.Value[i]=="PB")
                    {
                        //ローテーションとサーバーを戻す
                        ATeam.Value.RotateReverse();
                        NextServeTeam(false);
                        return;
                    }
                    else if (History.Value[i][0]=='S')
                    {
                        if (History.Value[i][1] == Rule.SetCount)
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
                        else if (History.Value[i][1] =='1')
                        {
                            UndoEnable.Value = true;
                            if (CoinToss.ATeamServer)
                            {
                                NextServeTeam(true);
                            }
                            else
                            {
                                NextServeTeam(false);
                            }
                        }
                        else if (History.Value[i][1]%2==0)
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
                        else if (History.Value[i][1]%2==1)
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
                BTeam.Value.Point(-1);
                IsEnablePoint.Value = true;
                IsEnableTimeout.Value = true;

                for (int i = History.Value.Count-1; i >= 0; i--)
                {
                    if (History.Value[i]=="PB")
                    {
                        //そのまま
                        return;
                    }
                    else if (History.Value[i]=="PA")
                    {
                        //ローテーションとサーバーを戻す
                        BTeam.Value.RotateReverse();
                        NextServeTeam(true);
                        return;
                    }
                    else if (History.Value[i][0]=='S')
                    {
                        if (History.Value[i][1] == Rule.SetCount)
                        {
                            if (FinalSetCoinToss.ATeamServer)
                            {
                                NextServeTeam(false);
                            }
                            else
                            {
                                BTeam.Value.RotateReverse();
                                NextServeTeam(true);
                            }
                            return;
                        }
                        else if (History.Value[i][1]%2==0)
                        {
                            if (CoinToss.ATeamServer)
                            {
                                NextServeTeam(false);
                            }
                            else
                            {
                                BTeam.Value.RotateReverse();
                                NextServeTeam(true);
                            }
                            return;
                        }
                        else if (History.Value[i][1]%2==1)
                        {
                            if (CoinToss.ATeamServer)
                            {
                                BTeam.Value.RotateReverse();
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

        public void Substitution(bool isAteam, int In, int Out)
        {
            //Feature
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

        public ReactivePropertySlim<bool> UndoEnable { get; set; } = new(false);
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
        public Model.CoinToss? CoinToss { get; set; } = new();
        public Model.CoinToss? FinalSetCoinToss { get; set; } = new();
        public Rule Rule { get; set; } = new();
    }
}
