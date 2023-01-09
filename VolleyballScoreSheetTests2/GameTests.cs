using Microsoft.VisualStudio.TestTools.UnitTesting;
using VolleyballScoreSheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using VolleyballScoreSheet.Views;

namespace VolleyballScoreSheet.Tests
{
    [TestClass()]
    public class GameTests
    {
        [TestMethod()]
        public void PointTest()
        {
            var game = new Game();
            var substitution = new Model.Substitution(game);

            game.ATeam.Value = new Model.Team("ATeam", "black");
            game.BTeam.Value = new Model.Team("BTeam", "black");

            //game.ATeam.Value.Players 
            game.ATeam.Value.Players = new List<Model.Player>
            {
                new Model.Player() { Id = 1 },
                new Model.Player() { Id = 2 },
                new Model.Player() { Id = 3 },
                new Model.Player() { Id = 4 },
                new Model.Player() { Id = 5 },
                new Model.Player() { Id = 6 },
                new Model.Player() { Id = 7 },
                new Model.Player() { Id = 8 },
                new Model.Player() { Id = 9 },
                new Model.Player() { Id = 10 },
                new Model.Player() { Id = 11 },
                new Model.Player() { Id = 12 }
            };

            game.BTeam.Value.Players = new List<Model.Player>
            {
                new Model.Player() { Id = 1 },
                new Model.Player() { Id = 2 },
                new Model.Player() { Id = 3 },
                new Model.Player() { Id = 4 },
                new Model.Player() { Id = 5 },
                new Model.Player() { Id = 6 },
                new Model.Player() { Id = 7 },
                new Model.Player() { Id = 8 },
                new Model.Player() { Id = 9 },
                new Model.Player() { Id = 10 },
                new Model.Player() { Id = 11 },
                new Model.Player() { Id = 12 }
            };


            game.CoinToss.ATeamServer = true;
            game.CoinToss.ATeamLeftSide = true;
            game.CoinToss.CoinTossCompleted= true;
            game.isLastPointA = true;

            game.ATeam.Value.CreateSet();
            game.BTeam.Value.CreateSet();

            game.ATeam.Value.Sets[^1].StartingLineUp.Value = new int[] { 1, 2, 3, 4, 5, 6 };
            game.ATeam.Value.StartingLineUp.Value =  new int[] { 1, 2, 3, 4, 5, 6 };
            game.ATeam.Value.Rotation.Value = new int[] { 1, 2, 3, 4, 5, 6 };
            game.ATeam.Value.Sets[^1].Rotation.Value = new int[] { 1, 2, 3, 4, 5, 6 };

            game.BTeam.Value.Sets[^1].StartingLineUp.Value =  new int[] { 11, 22, 33, 44, 55, 66 };
            game.BTeam.Value.StartingLineUp.Value = new int[] { 11, 22, 33, 44, 55, 66 };
            game.BTeam.Value.Rotation.Value = new int[] { 11, 22, 33, 44, 55, 66 };
            game.BTeam.Value.Sets[^1].Rotation.Value = new int[] { 11, 22, 33, 44, 55, 66 };

            game.History.HistoryAdd("S"+game.Set.Value);

            game.Point(true);
            CollectionAssert.AreEqual(new int[] { 1, 2, 3, 4, 5, 6 }, game.ATeam.Value.Sets[^1].Rotation.Value);
            CollectionAssert.AreEqual(new int[] { 11, 22, 33, 44, 55, 66 }, game.BTeam.Value.Sets[^1].Rotation.Value);
            Assert.AreEqual(1, game.ATeam.Value.Points.Value);
            Assert.AreEqual(0, game.BTeam.Value.Points.Value);

            game.Point(false);
            CollectionAssert.AreEqual(new int[] { 1, 2, 3, 4, 5, 6 }, game.ATeam.Value.Sets[^1].Rotation.Value);
            CollectionAssert.AreEqual(new int[] { 22, 33, 44, 55, 66, 11 }, game.BTeam.Value.Sets[^1].Rotation.Value);
            Assert.AreEqual(1, game.ATeam.Value.Points.Value);
            Assert.AreEqual(1, game.BTeam.Value.Points.Value);

            game.Point(false);
            CollectionAssert.AreEqual(new int[] { 1, 2, 3, 4, 5, 6 }, game.ATeam.Value.Sets[^1].Rotation.Value);
            CollectionAssert.AreEqual(new int[] { 22, 33, 44, 55, 66, 11 }, game.BTeam.Value.Sets[^1].Rotation.Value);
            Assert.AreEqual(1, game.ATeam.Value.Points.Value);
            Assert.AreEqual(2, game.BTeam.Value.Points.Value);

            game.Point(true);
            CollectionAssert.AreEqual(new int[] { 2, 3, 4, 5, 6, 1 }, game.ATeam.Value.Sets[^1].Rotation.Value);
            CollectionAssert.AreEqual(new int[] { 22, 33, 44, 55, 66, 11 }, game.BTeam.Value.Sets[^1].Rotation.Value);
            Assert.AreEqual(2, game.ATeam.Value.Points.Value);
            Assert.AreEqual(2, game.BTeam.Value.Points.Value);

            game.Undo();
            CollectionAssert.AreEqual(new int[] { 1, 2, 3, 4, 5, 6 }, game.ATeam.Value.Sets[^1].Rotation.Value);
            CollectionAssert.AreEqual(new int[] { 22, 33, 44, 55, 66, 11 }, game.BTeam.Value.Sets[^1].Rotation.Value);
            Assert.AreEqual(1, game.ATeam.Value.Points.Value);
            Assert.AreEqual(2, game.BTeam.Value.Points.Value);

            game.Point(true);
            CollectionAssert.AreEqual(new int[] { 2, 3, 4, 5, 6, 1 }, game.ATeam.Value.Sets[^1].Rotation.Value);
            CollectionAssert.AreEqual(new int[] { 22, 33, 44, 55, 66, 11 }, game.BTeam.Value.Sets[^1].Rotation.Value);
            Assert.AreEqual(2, game.ATeam.Value.Points.Value);
            Assert.AreEqual(2, game.BTeam.Value.Points.Value);

            game.Point(true);
            CollectionAssert.AreEqual(new int[] { 2, 3, 4, 5, 6, 1 }, game.ATeam.Value.Sets[^1].Rotation.Value);
            CollectionAssert.AreEqual(new int[] { 22, 33, 44, 55, 66, 11 }, game.BTeam.Value.Sets[^1].Rotation.Value);
            Assert.AreEqual(3, game.ATeam.Value.Points.Value);
            Assert.AreEqual(2, game.BTeam.Value.Points.Value);

            game.Point(true);
            game.Point(true);
            game.Point(true);
            game.Point(true);
            game.Point(true);
            game.Point(true);
            game.Point(true);
            game.Point(true);
            game.Point(true);
            game.Point(true);
            game.Point(true);
            game.Point(true);
            game.Point(true);
            game.Point(true);
            game.Point(true);
            game.Point(true);
            game.Point(true);
            game.Point(true);
            game.Point(true);
            game.Point(true);
            game.Point(true);
            game.Point(true);

            CollectionAssert.AreEqual(new int[] { 2, 3, 4, 5, 6, 1 }, game.ATeam.Value.Sets[^1].Rotation.Value);
            CollectionAssert.AreEqual(new int[] { 22, 33, 44, 55, 66, 11 }, game.BTeam.Value.Sets[^1].Rotation.Value);
            Assert.AreEqual(25, game.ATeam.Value.Points.Value);
            Assert.AreEqual(2, game.BTeam.Value.Points.Value);
            Assert.AreEqual("WSA", game.History.Histories.Value[^1].Command1);


            //2セット目
            game.ATeam.Value.CreateSet();
            game.BTeam.Value.CreateSet();

            game.ATeam.Value.Sets[^1].StartingLineUp.Value = new int[] { 1, 2, 3, 4, 5, 6 };
            game.ATeam.Value.StartingLineUp.Value =  new int[] { 1, 2, 3, 4, 5, 6 };
            game.ATeam.Value.Rotation.Value = new int[] { 1, 2, 3, 4, 5, 6 };
            game.ATeam.Value.Sets[^1].Rotation.Value = new int[] { 1, 2, 3, 4, 5, 6 };

            game.BTeam.Value.Sets[^1].StartingLineUp.Value =  new int[] { 11, 22, 33, 44, 55, 66 };
            game.BTeam.Value.StartingLineUp.Value = new int[] { 11, 22, 33, 44, 55, 66 };
            game.BTeam.Value.Rotation.Value = new int[] { 11, 22, 33, 44, 55, 66 };
            game.BTeam.Value.Sets[^1].Rotation.Value = new int[] { 11, 22, 33, 44, 55, 66 };

            game.History.HistoryAdd("S"+game.Set.Value);

            game.Point(true);
            CollectionAssert.AreEqual(new int[] { 1, 2, 3, 4, 5, 6 }, game.ATeam.Value.Sets[^1].Rotation.Value);
            CollectionAssert.AreEqual(new int[] { 11, 22, 33, 44, 55, 66 }, game.BTeam.Value.Sets[^1].Rotation.Value);
            Assert.AreEqual(1, game.ATeam.Value.Points.Value);
            Assert.AreEqual(0, game.BTeam.Value.Points.Value);

            game.Point(false);
            CollectionAssert.AreEqual(new int[] { 1, 2, 3, 4, 5, 6 }, game.ATeam.Value.Sets[^1].Rotation.Value);
            CollectionAssert.AreEqual(new int[] { 22, 33, 44, 55, 66, 11 }, game.BTeam.Value.Sets[^1].Rotation.Value);
            Assert.AreEqual(1, game.ATeam.Value.Points.Value);
            Assert.AreEqual(1, game.BTeam.Value.Points.Value);

            game.Point(false);
            CollectionAssert.AreEqual(new int[] { 1, 2, 3, 4, 5, 6 }, game.ATeam.Value.Sets[^1].Rotation.Value);
            CollectionAssert.AreEqual(new int[] { 22, 33, 44, 55, 66, 11 }, game.BTeam.Value.Sets[^1].Rotation.Value);
            Assert.AreEqual(1, game.ATeam.Value.Points.Value);
            Assert.AreEqual(2, game.BTeam.Value.Points.Value);

            game.Point(true);
            CollectionAssert.AreEqual(new int[] { 2, 3, 4, 5, 6, 1 }, game.ATeam.Value.Sets[^1].Rotation.Value);
            CollectionAssert.AreEqual(new int[] { 22, 33, 44, 55, 66, 11 }, game.BTeam.Value.Sets[^1].Rotation.Value);
            Assert.AreEqual(2, game.ATeam.Value.Points.Value);
            Assert.AreEqual(2, game.BTeam.Value.Points.Value);

            game.Undo();
            CollectionAssert.AreEqual(new int[] { 1, 2, 3, 4, 5, 6 }, game.ATeam.Value.Sets[^1].Rotation.Value);
            CollectionAssert.AreEqual(new int[] { 22, 33, 44, 55, 66, 11 }, game.BTeam.Value.Sets[^1].Rotation.Value);
            Assert.AreEqual(1, game.ATeam.Value.Points.Value);
            Assert.AreEqual(2, game.BTeam.Value.Points.Value);

            game.Point(true);
            CollectionAssert.AreEqual(new int[] { 2, 3, 4, 5, 6, 1 }, game.ATeam.Value.Sets[^1].Rotation.Value);
            CollectionAssert.AreEqual(new int[] { 22, 33, 44, 55, 66, 11 }, game.BTeam.Value.Sets[^1].Rotation.Value);
            Assert.AreEqual(2, game.ATeam.Value.Points.Value);
            Assert.AreEqual(2, game.BTeam.Value.Points.Value);

            game.Point(true);
            CollectionAssert.AreEqual(new int[] { 2, 3, 4, 5, 6, 1 }, game.ATeam.Value.Sets[^1].Rotation.Value);
            CollectionAssert.AreEqual(new int[] { 22, 33, 44, 55, 66, 11 }, game.BTeam.Value.Sets[^1].Rotation.Value);
            Assert.AreEqual(3, game.ATeam.Value.Points.Value);
            Assert.AreEqual(2, game.BTeam.Value.Points.Value);

            game.Point(false);

            substitution.DoSubstitution(true, game.ATeam.Value.Players.First(x => x.Id==1), game.ATeam.Value.Players.First(x => x.Id==7));
            CollectionAssert.AreEqual(new int[] { 2, 3, 4, 5, 6, 7 }, game.ATeam.Value.Sets[^1].Rotation.Value);
            game.Undo();


            game.Point(false);
            game.Point(false);
            game.Point(false);
            game.Point(false);
            game.Point(false);
            game.Point(false);
            game.Point(false);
            game.Point(false);
            game.Point(false);
            game.Point(false);
            game.Point(false);
            game.Point(false);
            game.Point(false);
            game.Point(false);
            game.Point(false);
            game.Point(false);
            game.Point(false);
            game.Point(false);
            game.Point(false);
            game.Point(false);
            game.Point(false);
            game.Point(false);

            CollectionAssert.AreEqual(new int[] { 2, 3, 4, 5, 6, 1 }, game.ATeam.Value.Sets[^1].Rotation.Value);
            CollectionAssert.AreEqual(new int[] { 33, 44, 55, 66, 11, 22 }, game.BTeam.Value.Sets[^1].Rotation.Value);
            Assert.AreEqual(3, game.ATeam.Value.Points.Value);
            Assert.AreEqual(25, game.BTeam.Value.Points.Value);

            //3セット目
            game.FinalSetCoinToss.ATeamServer = true;
            game.FinalSetCoinToss.ATeamLeftSide = true;
            game.FinalSetCoinToss.CoinTossCompleted= true;
            game.isLastPointA = true;

            game.ATeam.Value.CreateSet();
            game.BTeam.Value.CreateSet();

            game.ATeam.Value.Sets[^1].StartingLineUp.Value = new int[] { 1, 2, 3, 4, 5, 6 };
            game.ATeam.Value.StartingLineUp.Value =  new int[] { 1, 2, 3, 4, 5, 6 };
            game.ATeam.Value.Rotation.Value = new int[] { 1, 2, 3, 4, 5, 6 };
            game.ATeam.Value.Sets[^1].Rotation.Value = new int[] { 1, 2, 3, 4, 5, 6 };

            game.BTeam.Value.Sets[^1].StartingLineUp.Value =  new int[] { 11, 22, 33, 44, 55, 66 };
            game.BTeam.Value.StartingLineUp.Value = new int[] { 11, 22, 33, 44, 55, 66 };
            game.BTeam.Value.Rotation.Value = new int[] { 11, 22, 33, 44, 55, 66 };
            game.BTeam.Value.Sets[^1].Rotation.Value = new int[] { 11, 22, 33, 44, 55, 66 };

            game.History.HistoryAdd("S"+game.Set.Value);


            game.Point(true);
            CollectionAssert.AreEqual(new int[] { 1, 2, 3, 4, 5, 6 }, game.ATeam.Value.Sets[^1].Rotation.Value);
            CollectionAssert.AreEqual(new int[] { 11, 22, 33, 44, 55, 66 }, game.BTeam.Value.Sets[^1].Rotation.Value);
            Assert.AreEqual(1, game.ATeam.Value.Points.Value);
            Assert.AreEqual(0, game.BTeam.Value.Points.Value);

            game.Point(true);
            game.Point(true);
            game.Point(true);
            game.Point(true);
            game.Point(true);
            game.Point(true);
            game.Point(true);
            game.Point(true);
            game.Point(true);
            game.Point(true);
            game.Point(true);
            Assert.AreEqual(12, game.ATeam.Value.Points.Value);
            game.Point(false);
            Assert.AreEqual(1, game.BTeam.Value.Points.Value);
            CollectionAssert.AreEqual(new int[] { 22, 33, 44, 55, 66, 11 }, game.BTeam.Value.Sets[^1].Rotation.Value);

            game.Point(true);
            Assert.AreEqual(13, game.ATeam.Value.Points.Value);
            CollectionAssert.AreEqual(new int[] { 2, 3, 4, 5, 6, 1 }, game.ATeam.Value.Sets[^1].Rotation.Value);


        }
    }
}