using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static VolleyballScoreSheet.ViewModels.ScoreSheet.FirstSetViewModel;

namespace VolleyballScoreSheet.ViewModels.ScoreSheet.Tests
{
    [TestClass()]
    public class DeletePointTests
    {
        [TestMethod()]
        public void DeletePointTest()
        {
            var viewmodel = new FirstSetViewModel(new Game());

            var s = new DeletePoint();

            var dictionary = new Dictionary<int, DeletePoint[]>()
            {
                {
                    0, new DeletePoint[]
                    {
                        new DeletePoint()
                        {
                            StartRow = 0, Visible = true
                        },
                        new DeletePoint()
                        {
                            StartRow = 0, Visible = true
                        },
                        new DeletePoint()
                        {
                            StartRow = 0, Visible = true
                        },
                        new DeletePoint()
                        {
                            StartRow = 0, Visible = true
                        },
                        new DeletePoint()
                        {
                            StartRow = 0, Visible = true

                        },
                    }
                }
            };

            foreach (var item in dictionary)
            {
                var a = s.DeletePointCulc(item.Key);


                Assert.AreEqual(item.Value[0].StartRow, a[0].StartRow);
                Assert.AreEqual(item.Value[1].StartRow, a[1].StartRow);
                Assert.AreEqual(item.Value[2].StartRow, a[2].StartRow);
                Assert.AreEqual(item.Value[3].StartRow, a[3].StartRow);
                Assert.AreEqual(item.Value[4].StartRow, a[4].StartRow);
            }
        }
    }
}