using System.Collections.Generic;
using System.Linq;
using DriverMatching.Core.Models;

namespace DriverMatching.Algorithms.Algorithms
{
    public class BruteForceMatcher : IMatchingAlgorithm
    {
        public IList<Driver> FindNearest(Point order, IEnumerable<Driver> drivers, int k)
        {
            if (k <= 0) return new List<Driver>();
            var best = new List<(long dist2, Driver driver)>();
            foreach (var d in drivers)
            {
                long dist2 = d.Location.DistanceSquared(order);
                if (best.Count < k)
                {
                    best.Add((dist2, d));
                }
                else
                {
                    // find current max
                    int idxMax = 0;
                    long max = best[0].dist2;
                    for (int i = 1; i < best.Count; i++)
                    {
                        if (best[i].dist2 > max) { max = best[i].dist2; idxMax = i; }
                    }
                    if (dist2 < max)
                    {
                        best[idxMax] = (dist2, d);
                    }
                }
            }
            return best.OrderBy(t => t.dist2).Select(t => t.driver).ToList();
        }
    }
}
