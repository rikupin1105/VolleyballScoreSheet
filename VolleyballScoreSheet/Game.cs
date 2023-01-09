using Microsoft.Xaml.Behaviors.Media;
using Prism.Mvvm;
using Prism.Regions;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using VolleyballScoreSheet.Model;
using VolleyballScoreSheet.Views;

namespace VolleyballScoreSheet
{
    public class Game : BindableBase
    {
        public Game()
        {
            History.HistoryAdd("S");
            History.Histories.Subscribe(x =>
            {
                if (x.Count>1) UndoEnable.Value = true;
                else UndoEnable.Value = false;
                Debug.Value = string.Join("\n", x.Select(x => x.DateTime.ToString("HH:mm:ss:FF")+" "+ x.Command1 + x.Command2));
            });

            ATeam.Value.CreateSet();
            BTeam.Value.CreateSet();
        }

        public History History { get; set; } = new();

        public ReactivePropertySlim<int> Set { get; set; } = new(1);

        public void LockControl()
        {
            IsEnablePoint.Value = false;
            IsEnableTimeout.Value = false;
            IsEnableSubstitution.Value = false;
        }
        public void UnlockControl()
        {
            IsEnablePoint.Value = true;
            IsEnableTimeout.Value = true;
            IsEnableSubstitution.Value = true;
        }
        public ReactivePropertySlim<bool> IsEnablePoint { get; set; } = new(false);
        public ReactivePropertySlim<bool> IsEnableTimeout { get; set; } = new(false);
        public ReactivePropertySlim<bool> IsEnableSubstitution { get; set; } = new(false);

        public ReactivePropertySlim<bool> DisplayCoinToss { get; set; } = new(true);
        public ReactivePropertySlim<bool> DisplayBeforeMatch { get; set; } = new(false);
        public ReactivePropertySlim<bool> DisplayRotation { get; set; } = new(false);
        public ReactivePropertySlim<bool> DisplayRequestTimeOut { get; set; } = new(false);
        public ReactivePropertySlim<List<Sanction>> Sanctions { get; set; } = new(new());

        public ReactivePropertySlim<string> Debug { get; set; } = new();
        public List<string> Remarks { get; set; } = new();

        public void DisplayMain(string s)
        {
            DisplayRotation.Value = false;
            DisplayRequestTimeOut.Value = false;
            DisplayBeforeMatch.Value = false;
            DisplayCoinToss.Value = false;

            if (s=="Rotation")
            {
                UnlockControl();

                DisplayRotation.Value = true;
            }
            else if (s=="TimeOut")
            {
                LockControl();

                DisplayRequestTimeOut.Value = true;
            }
            else if (s=="BeforeMatch")
            {
                DisplayBeforeMatch.Value = true;
                LockControl();
            }
            else if (s=="CoinToss")
            {
                LockControl();

                DisplayCoinToss.Value = true;
            }
        }

        public void RequestTimeOut()
        {
            DisplayMain("TimeOut");
        }

        public ReactiveCommand TimeoutRejectionCommand { get; } = new();
        public ReactiveCommand SecondTimeoutCommand { get; } = new();
        public ReactiveCommand FinalSetCourtChangeNotifyCommand { get; } = new();
        private void TimeOut(bool isATeam)
        {
            if (isATeam)
            {
                //タイムアウト可能判定
                //ヒストリー追加
                //計測(Feature)
                if (ATeam.Value.Sets[^1].TimeOuts.Value>=2)
                {
                    //タイムアウト不可
                    TimeoutRejectionCommand.Execute(ATeam.Value.Name.Value);
                    return;
                }
                else
                {
                    ATeam.Value.TimeOut();
                    History.HistoryAdd("TimeOutA");
                    if (ATeam.Value.Timeouts.Value == 2)
                    {
                        SecondTimeoutCommand.Execute(ATeam.Value.Name.Value);
                    }
                }
            }
            else
            {
                if (BTeam.Value.Sets[^1].TimeOuts.Value>=2)
                {
                    //タイムアウト不可
                    TimeoutRejectionCommand.Execute(BTeam.Value.Name.Value);
                    return;
                }
                else
                {
                    BTeam.Value.TimeOut();
                    History.HistoryAdd("TimeOutB");

                    if (BTeam.Value.Timeouts.Value == 2)
                    {
                        SecondTimeoutCommand.Execute(BTeam.Value.Name.Value);
                    }
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
            //PointA Aチームの得点
            //PointB Bチームの得点
            //TimeOutA Aチームのタイムアウト
            //TimeOutB Bチームのタイムアウト

            if (History.Histories.Value.Count == 1)
            {
                //return;
            }

            var c = History.Histories.Value[^1].Command1;
            var c2 = History.Histories.Value[^1].Command2;


            if (c=="PointA")
            {
                History.HistoryRemove();
                PointRemove(true);
                return;
            }
            else if (c=="PointB")
            {
                History.HistoryRemove();
                PointRemove(false);
                return;
            }
            else if (c=="SubstitutionA")
            {
                History.HistoryRemove();
                var In = int.Parse(c2.Split(',')[0]);
                var Out = int.Parse(c2.Split(',')[1]);

                ATeam.Value.Sets[^1].Substitutions.Value--;
                ATeam.Value.Sets[^1].Rotation.Value[Array.IndexOf(ATeam.Value.Sets[^1].Rotation.Value, In)] = Out;
                ATeam.Value.Refresh();

                var s = ATeam.Value.Sets[^1].SubstitutionDetails
                    .Where(x => x.In==In)
                    .Where(x => x.Out ==Out).First();

                ATeam.Value.Sets[^1].SubstitutionDetails.Remove(s);
                ATeam.Value.MedamaRefresh();
                return;
            }
            else if (c=="SubstitutionB")
            {
                History.HistoryRemove();
                var In = int.Parse(c2.Split(',')[0]);
                var Out = int.Parse(c2.Split(',')[1]);

                BTeam.Value.Sets[^1].Substitutions.Value--;
                BTeam.Value.Sets[^1].Rotation.Value[Array.IndexOf(BTeam.Value.Sets[^1].Rotation.Value, In)] = Out;
                BTeam.Value.Refresh();

                var s = BTeam.Value.Sets[^1].SubstitutionDetails
                    .Where(x => x.In==In)
                    .Where(x => x.Out ==Out).First();

                BTeam.Value.Sets[^1].SubstitutionDetails.Remove(s);
                BTeam.Value.MedamaRefresh();
                return;
            }
            else if (c=="TimeOutA")
            {
                History.HistoryRemove();
                ATeam.Value.TimeOut(-1);
                return;
            }
            else if (c=="TimeOutB")
            {
                History.HistoryRemove();
                BTeam.Value.TimeOut(-1);
                return;
            }
            else if (c=="CCF")
            {
                History.HistoryRemove();
                Rule.FinalSetCourtChanged = false;
                CourtChange();
                Undo();
                return;
            }
            else if (c=="DelayWarningA")
            {
                History.HistoryRemove();
                Sanctions.Value.RemoveAt(Sanctions.Value.Count-1);
                ATeam.Value.DelayWarning = null;
                return;
            }
            else if (c=="DelayWarningB")
            {
                History.HistoryRemove();
                Sanctions.Value.RemoveAt(Sanctions.Value.Count-1);
                BTeam.Value.DelayWarning = null;
                return;
            }
            else if (c=="DelayPenaltyA")
            {
                History.HistoryRemove();
                Sanctions.Value.RemoveAt(Sanctions.Value.Count-1);
                ATeam.Value.DelayPenalties.RemoveAt(ATeam.Value.DelayPenalties.Count-1);
                PointRemove(true);
                return;
            }
            else if (c=="DelayPenaltyB")
            {
                History.HistoryRemove();
                Sanctions.Value.RemoveAt(Sanctions.Value.Count-1);
                BTeam.Value.DelayPenalties.RemoveAt(BTeam.Value.DelayPenalties.Count-1);
                PointRemove(false);
                return;
            }
            else if (c=="ImproperRequestsA")
            {
                History.HistoryRemove();
                ATeam.Value.ImproperRequests.Value = false;
            }
            else if (c=="ImproperRequestsB")
            {
                History.HistoryRemove();
                BTeam.Value.ImproperRequests.Value = false;
            }
            else if (c=="YellowCardA")
            {
                History.HistoryRemove();
                var s = Sanctions.Value.Where(x => x.Team=='A')
                    .Where(x => x.Warning == c2).First();
                Sanctions.Value.Remove(s);
                return;
            }
            else if (c=="YellowCardB")
            {
                History.HistoryRemove();
                var s = Sanctions.Value.Where(x => x.Team=='B')
                    .Where(x => x.Warning == c2).First();
                Sanctions.Value.Remove(s);
                return;
            }
            else if (c=="RedCardA")
            {
                History.HistoryRemove();
                var s = Sanctions.Value.Where(x => x.Team=='A')
                    .Where(x => x.Penalty == c2).Last();
                Sanctions.Value.Remove(s);
                PointRemove(false);
                return;
            }
            else if (c=="RedCardB")
            {
                History.HistoryRemove();
                var s = Sanctions.Value.Where(x => x.Team=='B')
                    .Where(x => x.Penalty == c2).Last();
                Sanctions.Value.Remove(s);
                PointRemove(true);
                return;
            }
            else if (c=="ExceptionalSubstitutionA")
            {
                History.HistoryRemove();
                var In = int.Parse(c2.Split(',')[0]);
                var Out = int.Parse(c2.Split(',')[1]);

                ATeam.Value.Sets[^1].Rotation.Value[Array.IndexOf(ATeam.Value.Sets[^1].Rotation.Value, In)] = Out;
                ATeam.Value.Players.First(x=>x.Id == Out).IsExceptionalSubstituted = false;
                ATeam.Value.Sets[^1].SubstitutionDetails.RemoveAt(ATeam.Value.Sets[^1].SubstitutionDetails.Count()-1);
                Remarks.RemoveAt(Remarks.Count-1);
                ATeam.Value.Refresh();
                return;
            }
            else if (c=="ExceptionalSubstitutionB")
            {
                History.HistoryRemove();
                var In = int.Parse(c2.Split(',')[0]);
                var Out = int.Parse(c2.Split(',')[1]);

                BTeam.Value.Sets[^1].Rotation.Value[Array.IndexOf(BTeam.Value.Sets[^1].Rotation.Value, In)] = Out;
                BTeam.Value.Players.First(x => x.Id == Out).IsExceptionalSubstituted = false;
                BTeam.Value.Sets[^1].SubstitutionDetails.RemoveAt(BTeam.Value.Sets[^1].SubstitutionDetails.Count()-1);
                Remarks.RemoveAt(Remarks.Count-1);
                BTeam.Value.Refresh();
                return;
            }

            else if (c[0]=='W'&&c[1]=='S')
            {
                History.HistoryRemove();
                if (c[2]=='A')
                {
                    ATeam.Value.WinSets.Value--;
                }
                else //B
                {
                    BTeam.Value.WinSets.Value--;
                }
                Undo();
                return;
            }
            else if (c=="S1")
            {
                History.HistoryRemove();
                DisplayMain("CoinToss");
                LockControl();
                return;
            }
            else if (c==$"S{Rule.SetCount}")
            {
                History.HistoryRemove();
                DisplayMain("CoinToss");
                LockControl();
                return;
            }
            else if (c[0]=='S')
            {
                var set = int.Parse(c[1].ToString());
                History.HistoryRemove();

                DisplayMain("BeforeMatch");
                LockControl();

                return;
            }
            else if (c[0]=='G' && c[1]=='S')
            {
                History.HistoryRemove();
                ATeam.Value.DeleteSet();
                BTeam.Value.DeleteSet();

                DisplayMain("Rotation");

                Set.Value--;

                if(Set.Value == Rule.SetCount - 1)
                {
                    if (Rule.CourtChangeEnable)
                    {
                        isATeamLeft.Value = !CoinToss.ATeamLeftSide;
                    }
                }
                else if (Rule.CourtChangeEnable)
                {
                    CourtChange();
                }

                if (History.Histories.Value[^1].Command1=="WSA")
                {
                    NextServeTeam(true);
                }
                else if (History.Histories.Value[^1].Command1=="WSB")
                {
                    NextServeTeam(false);
                }


                ATeam.Value.MedamaRefresh();
                BTeam.Value.MedamaRefresh();
                LockControl();
                return;
            }

        }
        public void EndSet()
        {
            //ATeam.Value.Sets[^1].Point.ForceNotify();

            History.HistoryAdd("GS"+Set.Value);
            Set.Value++;

            ATeam.Value.CreateSet();
            BTeam.Value.CreateSet();

            if (Set.Value==Rule.SetCount)
            {
                //コートチェンジせずにコイントスへ
                DisplayMain("CoinToss");
                return;
            }
            else if (Rule.CourtChangeEnable)
            {
                //コートチェンジ
                CourtChange();
            }


            if (Set.Value%2==0)
            {
                //偶数セット コイントスと逆
                if (CoinToss.ATeamServer)
                {
                    NextServeTeam(false);
                }
                else
                {
                    NextServeTeam(true);
                }
            }
            else
            {
                //コイントス通り
                if (CoinToss.ATeamServer)
                {
                    NextServeTeam(true);
                }
                else
                {
                    NextServeTeam(false);
                }
            }

            DisplayMain("BeforeMatch");
        }

        public void Point(bool isATeam, bool history = true)
        {
            if (isATeam)
            {
                //ポイント追加
                ATeam.Value.Point();
                //ローテーション
                if (!isLastPointA)
                {
                    ATeam.Value.Rotate();
                    NextServeTeam(true);
                }

                //ヒストリー追加
                if (history) History.HistoryAdd("PointA");

                if (Set.Value==Rule.SetCount)
                {
                    //ファイナルセット
                    if (Rule.CourtChangeEnable &&
                        Rule.FinalSetCourtChangePoint == ATeam.Value.Sets[^1].Points.Value &&
                        Rule.FinalSetCourtChanged == false)
                    {
                        //コートチェンジ到達
                        FinalSetCourtChangeNotifyCommand.Execute();

                        CourtChange();
                        NextServeTeam(true);
                        if (history)  History.HistoryAdd("CCF");
                        Rule.FinalSetCourtChanged = true;
                    }

                    //ファイナルセット終了
                    if (ATeam.Value.Sets[^1].Points.Value >= Rule.FinalSetToWinPoint &&
                        ATeam.Value.Sets[^1].Points.Value - BTeam.Value.Sets[^1].Points.Value>=2)
                    {
                        //操作ロック
                        //END GAME
                        if (history) History.HistoryAdd("WSA");
                        ATeam.Value.WinSets.Value++;

                        LockControl();

                        EndButtonText.Value = "END GAME";
                    }
                }
                //ノーマルセット終了
                else if (ATeam.Value.Sets[^1].Points.Value >= Rule.ToWinPoint &&
                    ATeam.Value.Sets[^1].Points.Value - BTeam.Value.Sets[^1].Points.Value>=2)
                {

                    if (history) History.HistoryAdd("WSA");
                    ATeam.Value.WinSets.Value++;
                    LockControl();

                    //ゲーム終了
                    if (ATeam.Value.WinSets.Value == Rule.SetCount / 2 + 1)
                    {
                        //END GAME
                        EndButtonText.Value = "END GAME";
                    }
                    //セット終了
                    else
                    {
                        //END SET
                        EndButtonText.Value = "END SET";
                    }
                }
                else
                {
                    UnlockControl();
                }

                
            }
            else
            {
                //ポイント追加
                BTeam.Value.Point();
                //ローテーション
                if (isLastPointA)
                {
                    BTeam.Value.Rotate();
                    NextServeTeam(false);
                }

                //ヒストリー追加
                if (history)History.HistoryAdd("PointB");

                if (Set.Value == Rule.SetCount)
                {
                    //ファイナルセット
                    if (Rule.CourtChangeEnable &&
                        Rule.FinalSetCourtChangePoint == BTeam.Value.Sets[^1].Points.Value &&
                        Rule.FinalSetCourtChanged == false)
                    {
                        //コートチェンジ到達
                        FinalSetCourtChangeNotifyCommand.Execute();

                        CourtChange();
                        NextServeTeam(false);
                        if (history)History.HistoryAdd("CCF");
                        Rule.FinalSetCourtChanged = true;
                    }


                    //ファイナルセット終了
                    if (BTeam.Value.Sets[^1].Points.Value >= Rule.FinalSetToWinPoint &&
                        BTeam.Value.Sets[^1].Points.Value - ATeam.Value.Sets[^1].Points.Value>=2)
                    {
                        //操作ロック
                        //END GAME

                        LockControl();

                        if(history)History.HistoryAdd("WSB");
                        BTeam.Value.WinSets.Value++;

                        EndButtonText.Value = "END GAME";
                    }
                }
                //ノーマルセット終了
                else if (BTeam.Value.Sets[^1].Points.Value >= Rule.ToWinPoint &&
                    BTeam.Value.Sets[^1].Points.Value - ATeam.Value.Sets[^1].Points.Value>=2)
                {
                    if(history)History.HistoryAdd("WSB");
                    BTeam.Value.WinSets.Value++;
                    LockControl();
                    //ゲーム終了
                    if (BTeam.Value.WinSets.Value == Rule.SetCount / 2 + 1)
                    {
                        //END GAME
                        EndButtonText.Value = "END GAME";
                    }
                    //セット終了
                    else
                    {
                        //END SET
                        EndButtonText.Value = "END SET";
                    }
                }
                else
                {
                    UnlockControl();
                }

                
            }
        }
        public void CourtChange()
        {
            isATeamLeft.Value = !isATeamLeft.Value;
            isLeftServe.Value = !isLeftServe.Value;
        }
        public void PointAdd(bool isLeftSide,bool history = true)
        {
            if (isLeftSide == isATeamLeft.Value)
            {
                Point(true, history);
            }
            else
            {
                Point(false, history);
            }
        }
        public void PointRemove(bool isATeam)
        {
            if (isATeam)
            {
                ATeam.Value.Point(-1);

                UnlockControl();
                for (int i = History.Histories.Value.Count-1; i >= 0; i--)
                {
                    if (History.Histories.Value[i].Command1=="PointA" || History.Histories.Value[i].Command1=="RedCardB" || History.Histories.Value[i].Command1 == "DelayPenaltyB")
                    {
                        //そのまま
                        NextServeTeam(true);
                        return;
                    }
                    else if (History.Histories.Value[i].Command1=="PointB" || History.Histories.Value[i].Command1=="RedCardA" || History.Histories.Value[i].Command1 == "DelayPenaltyA")
                    {
                        //ローテーションとサーバーを戻す
                        ATeam.Value.RotateReverse();
                        NextServeTeam(false);
                        return;
                    }
                    else if (History.Histories.Value[i].Command1[0]=='S')
                    {
                        if (History.Histories.Value[i].Command1[1] == Rule.SetCount.ToString()[0])
                        {
                            if (FinalSetCoinToss.ATeamServer)
                            {
                                NextServeTeam(true);
                            }
                            else
                            {
                                NextServeTeam(false);
                                ATeam.Value.RotateReverse();
                            }
                            return;
                        }
                        else if (History.Histories.Value[i].Command1[1] =='1')
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
                        else if (History.Histories.Value[i].Command1[1]%2==0)
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
                        else if (History.Histories.Value[i].Command1[1]%2==1)
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
                UnlockControl();

                for (int i = History.Histories.Value.Count-1; i >= 0; i--)
                {
                    if (History.Histories.Value[i].Command1=="PointB")
                    {
                        //そのまま
                        NextServeTeam(false);
                        return;
                    }
                    else if (History.Histories.Value[i].Command1=="PointA")
                    {
                        //ローテーションとサーバーを戻す
                        BTeam.Value.RotateReverse();
                        NextServeTeam(true);
                        return;
                    }
                    else if (History.Histories.Value[i].Command1[0]=='S')
                    {
                        if (History.Histories.Value[i].Command1[1] == Rule.SetCount.ToString()[0])
                        {
                            if (FinalSetCoinToss!.ATeamServer)
                            {
                                BTeam.Value.RotateReverse();
                                NextServeTeam(true);
                            }
                            else
                            {
                                NextServeTeam(false);
                            }
                            return;
                        }
                        else if (History.Histories.Value[i].Command1[1]%2==0)
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
                        else if (History.Histories.Value[i].Command1[1]%2==1)
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


        public ReactiveCommand SubstitutionCountNotifyCommand { get; } = new();

        public void ExceptionalSubstitution(bool isAteam, int In, int Out)
        {
            if (isAteam)
            {
                ATeam.Value.Players[ATeam.Value.Players.IndexOf(ATeam.Value.Players.First(x => x.Id==Out))].IsExceptionalSubstituted = true;
                History.HistoryAdd($"ExceptionalSubstitutionA", $"{In},{Out}");
                ATeam.Value.ExceptionalSubstitution(In, Out, ATeam.Value.Sets[^1].Points.Value, BTeam.Value.Sets[^1].Points.Value);

                if (CoinToss.ATeamLeftSide)
                {
                    Remarks.Add($"例外的な選手交代 / A / {Set.Value} ({ATeam.Value.Sets[^1].Points}:{BTeam.Value.Sets[^1].Points}) No.{Out}→No.{In}");
                }
                else
                {
                    Remarks.Add($"例外的な選手交代 / B / {Set.Value} ({ATeam.Value.Sets[^1].Points}:{BTeam.Value.Sets[^1].Points}) No.{Out}→No.{In}");
                }
            }
            else
            {
                BTeam.Value.Players[BTeam.Value.Players.IndexOf(BTeam.Value.Players.First(x => x.Id==Out))].IsExceptionalSubstituted = true;
                History.HistoryAdd($"ExceptionalSubstitutionB", $"{In},{Out}");
                BTeam.Value.ExceptionalSubstitution(In, Out, BTeam.Value.Sets[^1].Points.Value, ATeam.Value.Sets[^1].Points.Value);

                if (CoinToss.ATeamLeftSide)
                {
                    Remarks.Add($"例外的な選手交代 / A / {Set.Value} ({ATeam.Value.Sets[^1].Points}:{BTeam.Value.Sets[^1].Points}) No.{Out}→No.{In}");
                }
                else
                {
                    Remarks.Add($"例外的な選手交代 / B / {Set.Value} ({ATeam.Value.Sets[^1].Points}:{BTeam.Value.Sets[^1].Points}) No.{Out}→No.{In}");
                }
            }
        }

        public void NextServeTeam(bool isATeam)
        {
            if (isATeam)
            {
                isLastPointA = true;
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
                isLastPointA = false;
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
            set
            {
                if (isATeamLeft.Value)
                {
                    ATeam.Value = value;
                }
                else
                {
                    BTeam.Value = value;
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
            set
            {
                if (isATeamLeft.Value)
                {
                    BTeam.Value = value;
                }
                else
                {
                    ATeam.Value = value;
                }
            }
        }
        public bool isLastPointA { get; set; }

        public ReactiveProperty<Team> ATeam { get; set; } = new(new Team("ATeam", "#cd1141"));
        public ReactiveProperty<Team> BTeam { get; set; } = new(new Team("BTeam", "#0146ae"));


        public Model.MatchInfo MatchInfo { get; set; }


        public Model.Referees Referees { get; set; } = new();
        public Model.CoinToss CoinToss { get; set; } = new();
        public Model.CoinToss? FinalSetCoinToss { get; set; } = new();
        public Rule Rule { get; set; } = new();
    }
}
