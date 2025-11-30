using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using DriverMatching.Algorithms.Algorithms;
using DriverMatching.Core.Models;

namespace DriverMatching.Benchmark.Benchmarks
{
    [MemoryDiagnoser]
    public class MatchingBenchmarks
    {
        private List<Driver> _drivers = null!;
        private BruteForceMatcher _brute = null!;
        private KDTreeMatcher _kdtree = null!;
        private GridMatcher _grid = null!;
        private Point[] _orders = null!;

        [Params(1000, 10000)]
        public int DriverCount { get; set; }

        [GlobalSetup]
        public void Setup()
        {
            var rng = new Random(12345);
            _drivers = new List<Driver>(DriverCount);
            for (int i = 0; i < DriverCount; i++)
            {
                var p = new Point(rng.Next(0, 1000), rng.Next(0, 1000));
                _drivers.Add(new Driver(i.ToString(), p));
            }
            _brute = new BruteForceMatcher();
            _kdtree = new KDTreeMatcher();
            _grid = new GridMatcher();

            _orders = Enumerable.Range(0, 200).Select(_ => new Point(rng.Next(0, 1000), rng.Next(0, 1000))).ToArray();
        }

        [Benchmark]
        public void BruteForce_FindNearest()
        {
            foreach (var o in _orders) _ = _brute.FindNearest(o, _drivers, 5);
        }

        [Benchmark]
        public void KDTree_FindNearest()
        {
            foreach (var o in _orders) _ = _kdtree.FindNearest(o, _drivers, 5);
        }

        [Benchmark]
        public void Grid_FindNearest()
        {
            foreach (var o in _orders) _ = _grid.FindNearest(o, _drivers, 5);
        }
    }
}
