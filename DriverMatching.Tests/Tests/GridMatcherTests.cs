using System.Linq;
using DriverMatching.Algorithms.Algorithms;
using DriverMatching.Core.Models;
using NUnit.Framework;

namespace DriverMatching.Tests
{
    public class GridMatcherTests
    {
        [Test]
        public void Grid_EqualsBruteForce_OnRandomSet()
        {
            var rng = new System.Random(99);
            var drivers = Enumerable.Range(0, 2000).Select(i => new Driver(i.ToString(), new Point(rng.Next(0,1000), rng.Next(0,1000)))).ToList();
            var orders = Enumerable.Range(0, 30).Select(_ => new Point(rng.Next(0,1000), rng.Next(0,1000))).ToList();

            var bf = new BruteForceMatcher();
            var grid = new GridMatcher(bucketSize: 20);

            foreach (var o in orders)
            {
                var expected = bf.FindNearest(o, drivers, 5).Select(d => d.Id).ToList();
                var actual = grid.FindNearest(o, drivers, 5).Select(d => d.Id).ToList();
                CollectionAssert.AreEquivalent(expected, actual);
            }
        }
    }
}
