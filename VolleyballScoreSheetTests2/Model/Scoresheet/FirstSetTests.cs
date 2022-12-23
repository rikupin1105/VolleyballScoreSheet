using Microsoft.VisualStudio.TestTools.UnitTesting;
using VolleyballScoreSheet.Model.Scoresheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VolleyballScoreSheet.ViewModels.ScoreSheet;
using VolleyballScoreSheet.Views.ScoreSheet;

namespace VolleyballScoreSheet.Model.Scoresheet.Tests
{
    [TestClass()]
    public class FirstSetTests
    {
        [TestMethod()]
        public void LastPositionTest()
        {
            var dictionary = new Dictionary<int, int?[]>()
            {
                {1, new int?[]{0,0}},
                {2, new int?[]{0,2}},
                {3, new int?[]{0,4}},
                {4, new int?[]{0,6}},
                {5, new int?[]{0,8}},
                {6, new int?[]{0,10}},
                {7, new int?[]{1,0}},
                {8, new int?[]{1,2}},
                {9, new int?[]{1,4}},
                {10, new int?[]{1,6}},
                {11, new int?[]{1,8}},
                {12, new int?[]{1,10}},
                {13, new int?[]{2,0}},
                {14, new int?[]{2,2}},
                {15, new int?[]{2,4}},
                {16, new int?[]{2,6}},
                {17, new int?[]{2,8}},
                {18, new int?[]{2,10}},
                {19, new int?[]{3,0}},
                {20, new int?[]{3,2}},
                {21, new int?[]{3,4}},
                {22, new int?[]{3,6}},
                {23, new int?[]{3,8}},
                {24, new int?[]{3,10}},
                {25, new int?[]{0,1}},
                {26, new int?[]{0,3}},
                {27, new int?[]{0,5}},
                {28, new int?[]{0,7}},
                {29, new int?[]{0,9}},
                {30, new int?[]{0,11}},
                {31, new int?[]{1,1}},
                {32, new int?[]{1,3}},
                {33, new int?[]{1,5}},
                {34, new int?[]{1,7}},
                {35, new int?[]{1,9}},
                {36, new int?[]{1,11}},
                {37, new int?[]{2,1}},
                {38, new int?[]{2,3}},
                {39, new int?[]{2,5}},
                {40, new int?[]{2,7}},
                {41, new int?[]{2,9}},
                {42, new int?[]{2,11}},
                {43, new int?[]{3,1}},
                {44, new int?[]{3,3}},
                {45, new int?[]{3,5}},
                {46, new int?[]{3,7}},
                {47, new int?[]{3,9}},
                {48, new int?[]{3,11}}
            };

            var aa = new FirstSet(new Game());
            foreach (var item in dictionary)
            {
                var a = aa.LastPosition(item.Key);
                Assert.AreEqual(a[0], item.Value[0]!.Value);
                Assert.AreEqual(item.Value[1]!.Value, a[1]);
            }
        }
    }
}