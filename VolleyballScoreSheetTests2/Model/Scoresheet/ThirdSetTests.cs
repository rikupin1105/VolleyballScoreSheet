using Microsoft.VisualStudio.TestTools.UnitTesting;
using VolleyballScoreSheet.Model.Scoresheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using VolleyballScoreSheet.ViewModels;
using VolleyballScoreSheet.Shared;

namespace VolleyballScoreSheet.Model.Scoresheet.Tests
{
    [TestClass()]
    public class ThirdSetTests
    {
        [TestMethod()]
        public void InvertedTsTest()
        {
            var game = new Game();

            var a = new ThirdSet.InvertedT(10,0);
            var b = new ThirdSet.InvertedT(10,1);

            a.Y = 104;
            b.Y = 25;
        }
        [TestMethod()]
        public void ThirdSetTest()
        {
            var history = new History();

            history.HistoryAdd("S");
            history.HistoryAdd("S1");
            history.HistoryAdd("PointA");
            history.HistoryAdd("PointA");
            history.HistoryAdd("PointA");
            history.HistoryAdd("PointA");
            history.HistoryAdd("PointA");
            history.HistoryAdd("PointA");
            history.HistoryAdd("PointA");
            history.HistoryAdd("PointA");
            history.HistoryAdd("PointA");
            history.HistoryAdd("PointA");
            history.HistoryAdd("PointA");
            history.HistoryAdd("PointA");
            history.HistoryAdd("PointA");
            history.HistoryAdd("PointA");
            history.HistoryAdd("PointA");
            history.HistoryAdd("PointA");
            history.HistoryAdd("PointA");
            history.HistoryAdd("PointA");
            history.HistoryAdd("PointA");
            history.HistoryAdd("PointA");
            history.HistoryAdd("PointA");
            history.HistoryAdd("PointA");
            history.HistoryAdd("PointA");
            history.HistoryAdd("PointA");
            history.HistoryAdd("PointA");
            history.HistoryAdd("WSA");
            history.HistoryAdd("GS1");
            history.HistoryAdd("S2");
            history.HistoryAdd("PointB");
            history.HistoryAdd("PointB");
            history.HistoryAdd("PointB");
            history.HistoryAdd("PointB");
            history.HistoryAdd("PointB");
            history.HistoryAdd("PointB");
            history.HistoryAdd("PointB");
            history.HistoryAdd("PointB");
            history.HistoryAdd("PointB");
            history.HistoryAdd("PointB");
            history.HistoryAdd("PointB");
            history.HistoryAdd("PointB");
            history.HistoryAdd("PointB");
            history.HistoryAdd("PointB");
            history.HistoryAdd("PointB");
            history.HistoryAdd("PointB");
            history.HistoryAdd("PointB");
            history.HistoryAdd("PointB");
            history.HistoryAdd("PointB");
            history.HistoryAdd("PointB");
            history.HistoryAdd("PointB");
            history.HistoryAdd("PointB");
            history.HistoryAdd("PointB");
            history.HistoryAdd("PointB");
            history.HistoryAdd("PointB");
            history.HistoryAdd("WSB");
            history.HistoryAdd("GS2");
            history.HistoryAdd("S3");

            history.HistoryAdd("PointB");
            history.HistoryAdd("PointB");
            history.HistoryAdd("PointB");
            history.HistoryAdd("PointB");
            history.HistoryAdd("PointB");
            history.HistoryAdd("PointB");
            history.HistoryAdd("PointB");
            history.HistoryAdd("PointB");
            history.HistoryAdd("PointB");
            history.HistoryAdd("PointB");
            history.HistoryAdd("PointB");
            history.HistoryAdd("PointB");
            history.HistoryAdd("PointB");
            history.HistoryAdd("CCF");
            history.HistoryAdd("PointB");
            history.HistoryAdd("PointB");
            history.HistoryAdd("PointB");
            history.HistoryAdd("PointB");
            history.HistoryAdd("PointB");
            history.HistoryAdd("PointB");
            history.HistoryAdd("PointB");
            history.HistoryAdd("PointB");
            history.HistoryAdd("PointB");
            history.HistoryAdd("PointB");
            history.HistoryAdd("PointB");
            history.HistoryAdd("PointB");
            history.HistoryAdd("WSB");
            history.HistoryAdd("GS3");


            var sets = new ObservableCollection<Set>();
            sets.Add(new());
            sets.Add(new());
            sets.Add(new()
            {
                StartingLineUp = new(new int[] { 1, 2, 3, 4, 5, 6 })
            });


            var game = new Game()
            {
                Set = new(3),
                LeftTeam = new Team("A","black") 
                {
                    Sets = sets
                },
                RightTeam = new Team("B", "White")
                {
                    Sets = sets
                },
                FinalSetCoinToss = new CoinToss()
                {
                    ATeamLeftSide = false,
                    ATeamServer = false,
                    CoinTossCompleted= true,
                },
                History = history
            };


            var thirdset = new ThirdSet(game);
        }
    }
}