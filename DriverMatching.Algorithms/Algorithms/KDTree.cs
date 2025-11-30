using System;
using System.Collections.Generic;
using System.Linq;
using DriverMatching.Core.Models;

namespace DriverMatching.Algorithms.Algorithms
{
    internal sealed class KDTree
    {
        private sealed class Node
        {
            public Driver Driver { get; }
            public Point Point { get; }
            public int Axis { get; }
            public Node? Left { get; set; }
            public Node? Right { get; set; }
            public Node(Driver driver, Point point, int axis)
            {
                Driver = driver;
                Point = point;
                Axis = axis;
            }
        }

        private Node? _root;

        public KDTree(IEnumerable<Driver> drivers)
        {
            if (drivers == null) throw new ArgumentNullException(nameof(drivers));
            var arr = drivers as Driver[] ?? drivers.ToArray();
            _root = BuildRec(arr, 0);
        }

        private Node? BuildRec(Driver[] items, int depth)
        {
            if (items.Length == 0) return null;
            int axis = depth % 2;
            Array.Sort(items, (a, b) => axis == 0 ? a.Location.X.CompareTo(b.Location.X) : a.Location.Y.CompareTo(b.Location.Y));
            int mid = items.Length / 2;
            var node = new Node(items[mid], items[mid].Location, axis);
            if (mid > 0)
            {
                node.Left = BuildRec(items.Take(mid).ToArray(), depth + 1);
            }
            if (mid + 1 < items.Length)
            {
                node.Right = BuildRec(items.Skip(mid + 1).ToArray(), depth + 1);
            }
            return node;
        }

        public IList<Driver> KNearest(Point target, int k)
        {
            if (k <= 0) return Array.Empty<Driver>();
            var pq = new PriorityQueue<(long dist2, Driver driver), long>(); // priority = -dist2 to emulate max-heap

            void Visit(Node? node)
            {
                if (node == null) return;
                long dist2 = node.Point.DistanceSquared(target);
                long priority = -dist2;
                if (pq.Count < k) pq.Enqueue((dist2, node.Driver), priority);
                else
                {
                    var far = pq.Peek();
                    if (dist2 < far.dist2)
                    {
                        pq.Dequeue();
                        pq.Enqueue((dist2, node.Driver), priority);
                    }
                }

                int axis = node.Axis;
                int coordNode = axis == 0 ? node.Point.X : node.Point.Y;
                int coordTarget = axis == 0 ? target.X : target.Y;

                Node? first = coordTarget <= coordNode ? node.Left : node.Right;
                Node? second = ReferenceEquals(first, node.Left) ? node.Right : node.Left;

                Visit(first);

                long currentMax = pq.Count < k ? long.MaxValue : pq.Peek().dist2;
                long diff = coordTarget - coordNode;
                long diff2 = diff * diff;
                if (pq.Count < k || diff2 <= currentMax)
                {
                    Visit(second);
                }
            }

            Visit(_root);

            var result = new List<Driver>(pq.Count);
            while (pq.Count > 0) result.Add(pq.Dequeue().driver);
            result.Reverse();
            return result;
        }
    }
}
