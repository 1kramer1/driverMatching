using System.Collections.Generic;
using DriverMatching.Core.Models;

namespace DriverMatching.Algorithms.Algorithms
{
    public interface IMatchingAlgorithm
    {
        IList<Driver> FindNearest(Point order, IEnumerable<Driver> drivers, int k);
    }
}
