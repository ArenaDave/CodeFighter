using CodeFighter.Logic.Scenarios;
using CodeFighter.Logic.Ships;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFighter.Logic.Utility
{
    enum GridFacing
    {
        NorthEast = 0,
        East = 1,
        SouthEast = 2,
        South = 3,
        SouthWest = 4,
        West = 5,
        NorthWest = 6,
        North = 7

    }

    public class Pathing
    {
        #region Private Variables
        List<Ship> allShips = new List<Ship>();
        List<Feature> allFeatures = new List<Feature>();
        int boardWidth = 25;
        int boardHeight = 25;
        List<Node> openList = new List<Node>();
        List<Node> closedList = new List<Node>();
        #endregion

        #region Constructor
        public Pathing(List<Ship> ships, List<Feature> features)
        {
            this.allShips = ships;
            this.allFeatures = features;
        }
        #endregion
        
        public Point GetTargetPointInRadius(Point origin, Point target, double radius)
        {
            if (origin.DistanceTo(target) <= radius)
                return origin;

            return getTargetPointOnRadius(origin, target, radius);
        }

        public Point GetTargetPointOnRadius(Point origin, Point target, double radius)
        {
            return getTargetPointOnRadius(origin, target, radius);
        }

        public Point GetNextPoint(Point sourceLoc, Point targetLoc)
        {
            return getNextPoint(sourceLoc, targetLoc);
        }

        private Point getTargetPointOnRadius(Point origin, Point target, double radius)
        {
            // figure out dimension of target matrix: radius plus target square
            int dimension = Convert.ToInt32(radius * 2) + 1;
            // figure out zero point of target matrix: target minus radius
            Point zero = target;
            zero.Offset(Convert.ToInt32(-radius), Convert.ToInt32(-radius));
            // create and populate target matrix
            Point[,] matrix = new Point[dimension, dimension];
            for (int x = 0; x < dimension; x++)
            {
                for (int y = 0; y < dimension; y++)
                {
                    matrix[x, y] = new Point(zero.X + x, zero.Y + y);
                }
            }
            // get all points in radius
            List<Point> inRadius = new List<Point>();
            foreach (Point p in matrix)
                if (Math.Round(target.DistanceTo(p), 0) == radius 
                    && (p.X > -1 && p.X < boardWidth 
                        && p.Y > -1 && p.Y < boardHeight) 
                    && !p.Equals(origin))
                    inRadius.Add(p);

            double shortestDistance = double.MaxValue;
            Point result = new Point(target.X, target.Y);
            foreach (Point p in inRadius)
            {
                double current = origin.DistanceTo(p);
                bool isBlocked = (allShips.Any(x => x.Position.Equals(p)) || allFeatures.Any(x => x.Position.Equals(p)));
                if (current < shortestDistance && !isBlocked)
                {
                    shortestDistance = current;
                    result = p;
                }
            }

            return result;
        }
        
        private Point getNextPoint(Point sourceLoc, Point targetLoc)
        {
            // clear the lists
            openList.Clear();
            closedList.Clear();
            // find direction of targetLoc
            GridFacing facing = findFacing(sourceLoc, targetLoc);
            // get starting adjacent locations
            foreach (Node node in getNeighbors(new Node(sourceLoc.X, sourceLoc.Y), (int)facing, targetLoc))
            {
                openList.Add(node);
            }

            Node previous = new Node();
            while (openList.Count > 0)
            {
                // current lowest G+H
                int indexS = openList.Min(f => f.F);
                // get next node with lowest G+H that is not the previous node
                Node node = openList.First(f => f.F == indexS && f != previous);
                // check for goal
                if (node.Loc == targetLoc)
                {
                    return recurseNextNode(node).Loc;
                }
                // remove from openset
                openList.Remove(node);
                // add to closedset
                closedList.Add(node);
                // get neighbors of current location
                List<Node> sortedNeighbors = getNeighbors(node, (int)facing, targetLoc);
                foreach (Node neighbor in sortedNeighbors)
                {
                    // not in closed set
                    if (closedList.Contains(neighbor))
                        continue;
                    // steps from source
                    int tempG = node.G + 1;
                    // not already in openset or temp g less than existing g;
                    if (!openList.Any(f => f.Loc == neighbor.Loc && f.G <= tempG))//|| tempG < neighbor.G)
                    {
                        neighbor.Parent = node;
                        neighbor.G = tempG;
                        neighbor.H = Convert.ToInt32(neighbor.Loc.DistanceTo(targetLoc));
                        if (!openList.Contains(neighbor) && !closedList.Contains(neighbor))
                            openList.Add(neighbor);
                    }
                }
                previous = node;
            }

            return new Point(0, 0);
        }
        
        private Node recurseNextNode(Node node)
        {
            Node result = node;
            if (node.Parent != null)
            {
                result = recurseNextNode(node.Parent);
            }
            return result;
        }

        private GridFacing findFacing(Point a, Point b)
        {
            if (a.X > b.X)
            {
                if (a.Y > b.Y)
                    return GridFacing.NorthWest;
                else if (a.Y < b.Y)
                    return GridFacing.SouthWest;
                else
                    return GridFacing.West;
            }
            else if (a.X < b.X)
            {
                if (a.Y > b.Y)
                    return GridFacing.NorthEast;
                else if (a.Y < b.Y)
                    return GridFacing.SouthEast;
                else
                    return GridFacing.East;
            }
            else
            {
                if (a.Y > b.Y)
                    return GridFacing.North;
                else
                    return GridFacing.South;
            }
        }

        private List<Node> getNeighbors(Node node, int offset, Point targetLoc)
        {
            List<Node> results = new List<Node>();
            for (int i = 0; i < 8; i++)
            {
                int val = i + offset;
                if (val > 7)
                    val = val - 8;
                Node newNode = findNeighborInDirection(node, (GridFacing)Enum.Parse(typeof(GridFacing), (val).ToString()));
                if (newNode.Loc.X >= 0 && newNode.Loc.X < boardWidth && newNode.Loc.Y >= 0 && newNode.Loc.Y < boardHeight)
                {
                    bool isBlocked = (allShips.Any(x => x.Position.Equals(newNode.Loc)) 
                        || allFeatures.Any(x => x.Position.Equals(newNode.Loc)));
                    if (!isBlocked)
                    {
                        newNode.G = node.G + 1;
                        newNode.H = Convert.ToInt32(newNode.Loc.DistanceTo(targetLoc));
                        newNode.Depth = node.Depth + 1;
                        if (!closedList.Contains(newNode))
                            results.Add(newNode);
                    }
                }
            }

            return results;
        }

        private Node findNeighborInDirection(Node source, GridFacing direction)
        {
            Node result = new Node();
            switch (direction)
            {
                case GridFacing.NorthEast:
                    result = new Node(source.Loc.X + 1, source.Loc.Y - 1);
                    break;
                case GridFacing.East:
                    result = new Node(source.Loc.X + 1, source.Loc.Y);
                    break;
                case GridFacing.SouthEast:
                    result = new Node(source.Loc.X + 1, source.Loc.Y + 1);
                    break;
                case GridFacing.North:
                    result = new Node(source.Loc.X, source.Loc.Y - 1);
                    break;
                case GridFacing.South:
                    result = new Node(source.Loc.X, source.Loc.Y + 1);
                    break;
                case GridFacing.NorthWest:
                    result = new Node(source.Loc.X - 1, source.Loc.Y - 1);
                    break;
                case GridFacing.West:
                    result = new Node(source.Loc.X - 1, source.Loc.Y);
                    break;
                case GridFacing.SouthWest:
                    result = new Node(source.Loc.X - 1, source.Loc.Y + 1);
                    break;
            }
            return result;
        }
    }
}
