using System.Linq;
using DriverMatching.Algorithms.Algorithms;
using DriverMatching.Core.Models;
using NUnit.Framework;

namespace DriverMatching.Tests
{
    public class KDTreeTests
    {
        [Test]
        public void KDTree_EqualsBruteForce_OnRandomSet()
        {
            var rng = new System.Random(42);
            var drivers = Enumerable.Range(0, 1000).Select(i => new Driver(i.ToString(), new Point(rng.Next(0,1000), rng.Next(0,1000)))).ToList();
            var orders = Enumerable.Range(0, 50).Select(_ => new Point(rng.Next(0,1000), rng.Next(0,1000))).ToList();

            var bf = new BruteForceMatcher();
            var kd = new KDTreeMatcher();

            foreach (var o in orders)
            {
                var expected = bf.FindNearest(o, drivers, 5).Select(d => d.Id).ToList();
                var actual = kd.FindNearest(o, drivers, 5).Select(d => d.Id).ToList();
                CollectionAssert.AreEquivalent(expected, actual);
            }
        }
    }
}
