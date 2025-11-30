using BenchmarkDotNet.Running;
using System;
using System.Collections.Generic;
using System.Linq;
using DriverMatching.Algorithms.Algorithms;
using DriverMatching.Core.Models;

namespace DriverMatching.Benchmark
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length == 1 && args[0] == "debug-grid")
            {
                DebugGridMismatch();
                return;
            }

            BenchmarkRunner.Run<Benchmarks.MatchingBenchmarks>();
        }

        private static void DebugGridMismatch()
        {
            var rng = new Random(99);
            var drivers = Enumerable.Range(0, 2000).Select(i => new Driver(i.ToString(), new Point(rng.Next(0,1000), rng.Next(0,1000)))).ToList();
            var orders = Enumerable.Range(0, 30).Select(_ => new Point(rng.Next(0,1000), rng.Next(0,1000))).ToList();

            var bf = new BruteForceMatcher();
            var grid = new GridMatcher(bucketSize: 20);

            foreach (var o in orders)
            {
                var expected = bf.FindNearest(o, drivers, 5).Select(d => (d.Id, d.Location, dist: d.Location.DistanceSquared(o))).ToList();
                var actual = grid.FindNearest(o, drivers, 5).Select(d => (d.Id, d.Location, dist: d.Location.DistanceSquared(o))).ToList();
                var expectedIds = expected.Select(x => x.Id).OrderBy(x => x).ToArray();
                var actualIds = actual.Select(x => x.Id).OrderBy(x => x).ToArray();
                if (!expectedIds.SequenceEqual(actualIds))
                {
                    Console.WriteLine($"Order: {o}");
                    Console.WriteLine("Expected:");
                    foreach (var e in expected) Console.WriteLine($"  {e.Id} at {e.Location} dist2={e.dist}");
                    Console.WriteLine("Actual:");
                    foreach (var a in actual) Console.WriteLine($"  {a.Id} at {a.Location} dist2={a.dist}");
                    return;
                }
            }
            Console.WriteLine("No mismatches found");
        }
    }
}
