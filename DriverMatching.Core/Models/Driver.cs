namespace DriverMatching.Core.Models
{
    public sealed class Driver
    {
        public string Id { get; }
        public Point Location { get; set; }
        public Driver(string id, Point location)
        {
            Id = id ?? throw new System.ArgumentNullException(nameof(id));
            Location = location;
        }
    }
}
