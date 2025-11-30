using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using DriverMatching.Core.Interfaces;
using DriverMatching.Core.Models;

namespace DriverMatching.Core.Stores
{
    public class DriverStore : IDriverStore
    {
        private readonly ConcurrentDictionary<string, Driver> _byId = new();
        public void AddOrUpdate(Driver driver)
        {
            _byId.AddOrUpdate(driver.Id, driver, (k, old) => driver);
        }
        public bool Remove(string id) => _byId.TryRemove(id, out _);
        public IEnumerable<Driver> GetAll() => _byId.Values.ToArray();
    }
}
