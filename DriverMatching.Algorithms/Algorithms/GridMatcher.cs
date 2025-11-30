using System.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using DriverMatching.Core.Models;

namespace DriverMatching.Algorithms.Algorithms
{
    public class GridMatcher : IMatchingAlgorithm
    {
        private readonly int _bucketSize;

        public GridMatcher(int bucketSize = 10)
        {
            if (bucketSize <= 0) throw new ArgumentOutOfRangeException(nameof(bucketSize));
            _bucketSize = bucketSize;
        }

        public IList<Driver> FindNearest(Point order, IEnumerable<Driver> drivers, int k)
        {
            if (k <= 0) return new List<Driver>();
            if (drivers == null) return new List<Driver>();

            // Build grid buckets
            var map = new Dictionary<long, List<(Driver driver, int idx)>>();
            int idx = 0;
            foreach (var d in drivers)
            {
                int bx = d.Location.X / _bucketSize;
                int by = d.Location.Y / _bucketSize;
                long key = ((long)bx << 32) | (uint)by;
                if (!map.TryGetValue(key, out var list)) { list = new List<(Driver, int)>(); map[key] = list; }
                list.Add((d, idx));
                idx++;
            }

            int obx = order.X / _bucketSize;
            int oby = order.Y / _bucketSize;

            var candidates = new List<(Driver driver, int idx)>();
            // Exact strategy: compute minimal possible distance from order to each bucket, sort buckets by that
            var bucketList = new List<(long key, long minDist2)>();
            foreach (var kv in map)
            {
                long key = kv.Key;
                int bx = (int)(key >> 32);
                int by = (int)(key & 0xFFFFFFFF);
                int minX = bx * _bucketSize;
                int maxX = (bx + 1) * _bucketSize - 1;
                int minY = by * _bucketSize;
                int maxY = (by + 1) * _bucketSize - 1;
                int cx = order.X < minX ? minX : (order.X > maxX ? maxX : order.X);
                int cy = order.Y < minY ? minY : (order.Y > maxY ? maxY : order.Y);
                long dx0 = order.X - cx;
                long dy0 = order.Y - cy;
                long minDist2 = dx0 * dx0 + dy0 * dy0;
                bucketList.Add((key, minDist2));
            }

            bucketList.Sort((a, b) => a.minDist2.CompareTo(b.minDist2));

            // iterate buckets in increasing minDist order, add drivers; stop when next bucket's minDist > current kthDist
            candidates.Clear();
            for (int i = 0; i < bucketList.Count; i++)
            {
                var key = bucketList[i].key;
                candidates.AddRange(map[key]);

                if (candidates.Count < k) continue;

                var kthDist = candidates.Select(t => t.driver.Location.DistanceSquared(order)).OrderBy(x => x).Take(k).Last();
                long nextMin = (i + 1) < bucketList.Count ? bucketList[i + 1].minDist2 : long.MaxValue;
                if (nextMin > kthDist)
                {
                    break;
                }
            }
            if (candidates.Count == 0) return new List<Driver>();

            // select top-k by distance squared, tie-break by original input order (idx)
            var top = candidates
                .Select(t => (dist2: t.driver.Location.DistanceSquared(order), idx: t.idx, driver: t.driver))
                .OrderBy(t => t.dist2)
                .ThenBy(t => t.idx)
                .Take(k)
                .Select(t => t.driver)
                .ToList();

            return top;
        }
    }
}
