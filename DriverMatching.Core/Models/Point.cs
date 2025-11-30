namespace DriverMatching.Core.Models
{
    public readonly struct Point
    {
        public int X { get; }
        public int Y { get; }
        public Point(int x, int y) { X = x; Y = y; }
        public long DistanceSquared(in Point other)
        {
            long dx = X - other.X;
            long dy = Y - other.Y;
            return dx * dx + dy * dy;
        }
        public override string ToString() => $"({X},{Y})";
    }
}
