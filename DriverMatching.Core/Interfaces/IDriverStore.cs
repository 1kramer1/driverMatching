using System.Collections.Generic;
using DriverMatching.Core.Models;

namespace DriverMatching.Core.Interfaces
{
    public interface IDriverStore
    {
        void AddOrUpdate(Driver driver);
        bool Remove(string id);
        IEnumerable<Driver> GetAll();
    }
}
