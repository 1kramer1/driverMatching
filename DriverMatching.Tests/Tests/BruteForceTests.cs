using System.Linq;
using DriverMatching.Algorithms.Algorithms;
using DriverMatching.Core.Models;
using NUnit.Framework;

namespace DriverMatching.Tests
{
    public class BruteForceTests
    {
        [Test]
        public void SmallSet_ReturnsNearest()
        {
            var drivers = new[] {
                new Driver("a", new Point(0,0)),
                new Driver("b", new Point(10,0)),
                new Driver("c", new Point(0,10)),
                new Driver("d", new Point(5,1)),
                new Driver("e", new Point(2,2)),
            };
            var matcher = new BruteForceMatcher();
            var res = matcher.FindNearest(new Point(1,1), drivers, 3).ToList();
            Assert.AreEqual(3, res.Count);
            CollectionAssert.Contains(res.Select(d => d.Id).ToList(), "e");
            CollectionAssert.Contains(res.Select(d => d.Id).ToList(), "a");
        }
    }
}
