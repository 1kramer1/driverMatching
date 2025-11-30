using System.Collections.Generic;
using DriverMatching.Core.Models;

namespace DriverMatching.Algorithms.Algorithms
{
    public class KDTreeMatcher : IMatchingAlgorithm
    {
        private object? _driversRef;
        private KDTree? _tree;

        public IList<Driver> FindNearest(Point order, IEnumerable<Driver> drivers, int k)
        {
            if (drivers == null) return new List<Driver>();

            // cache tree per drivers instance to avoid rebuilding on repeated queries
            if (!ReferenceEquals(_driversRef, drivers) || _tree == null)
            {
                _driversRef = drivers;
                _tree = new KDTree(drivers);
            }

            return _tree.KNearest(order, k);
        }
    }
}
