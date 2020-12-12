using System;
using System.Collections.Generic;
using System.Linq;

namespace Euler_Logic.Problems.AdventOfCode.Y2018 {
    public class Problem13 : AdventOfCodeBase {
        public override string ProblemName {
            get { return "Advent of Code 2018: 13"; }
        }

        private Grid _grid;
        public override string GetAnswer() {
            _grid = new Grid();
            MakeVelocity();
            _grid.Points = GetPoints().ToDictionary(x => x.XY, x => x);
            MakeGrid();
            GetCartVelocities();
            return Answer2();
        }

        
        private string Answer1() {
            return FindFirstCrash();
        }

        private string Answer2() {
            return FindRemaining();
        }

        private string FindFirstCrash() {
            int count = 0;
            var hash = _grid.Points.Keys.ToDictionary(x => x, x => false);
            _grid.Carts.ForEach(x => hash[x.Point] = true);
            var changeDirection = GetChangeDirection();
            do {
                count++;
                var carts = _grid.Carts.OrderBy(x => x.Point.Item2).ThenBy(x => x.Point.Item1);
                foreach (var cart in carts) {
                    hash[cart.CurrentConnection.Points[cart.PointIndex]] = false;
                    cart.PointIndex++;
                    if (hash[cart.CurrentConnection.Points[cart.PointIndex]]) {
                        return cart.CurrentConnection.Points[cart.PointIndex].Item1 + "," + cart.CurrentConnection.Points[cart.PointIndex].Item2;
                    }
                    hash[cart.CurrentConnection.Points[cart.PointIndex]] = true;
                    cart.Point = cart.CurrentConnection.Points[cart.PointIndex];
                    if (cart.PointIndex == cart.CurrentConnection.Points.Count - 1) {
                        cart.Direction = changeDirection[cart.CurrentConnection.DirectionOnEnd, cart.ChangeDirectionList[cart.ChangeDirectionIndex]];
                        cart.ChangeDirectionIndex = (cart.ChangeDirectionIndex + 1) % 3;
                        cart.PointIndex = 0;
                        cart.CurrentConnection = cart.CurrentConnection.Next.Connections[cart.Direction];
                    }
                }
            } while (true);
        }

        private string FindRemaining() {
            var hash = _grid.Points.Keys.ToDictionary(x => x, x => false);
            _grid.Carts.ForEach(x => hash[x.Point] = true);
            var changeDirection = GetChangeDirection();
            Cart[] cartsToRemove = new Cart[2];
            do {
                var carts = _grid.Carts.OrderBy(x => x.Point.Item2).ThenBy(x => x.Point.Item1);
                foreach (var cart in carts) {
                    hash[cart.CurrentConnection.Points[cart.PointIndex]] = false;
                    cart.PointIndex++;
                    if (hash[cart.CurrentConnection.Points[cart.PointIndex]]) {
                        foreach (var next in carts) {
                            if (next != cart && next.Point.Item1 == cart.CurrentConnection.Points[cart.PointIndex].Item1 && next.Point.Item2 == cart.CurrentConnection.Points[cart.PointIndex].Item2) {
                                cartsToRemove[0] = cart;
                                cartsToRemove[1] = next;
                                hash[next.Point] = false;
                                break;
                            }
                        }
                    } else if (cart != cartsToRemove[0] && cart != cartsToRemove[1]) {
                        hash[cart.CurrentConnection.Points[cart.PointIndex]] = true;
                        cart.Point = cart.CurrentConnection.Points[cart.PointIndex];
                        if (cart.PointIndex == cart.CurrentConnection.Points.Count - 1) {
                            cart.Direction = changeDirection[cart.CurrentConnection.DirectionOnEnd, cart.ChangeDirectionList[cart.ChangeDirectionIndex]];
                            cart.ChangeDirectionIndex = (cart.ChangeDirectionIndex + 1) % 3;
                            cart.PointIndex = 0;
                            cart.CurrentConnection = cart.CurrentConnection.Next.Connections[cart.Direction];
                        }
                    }
                }
                if (cartsToRemove[0] != null) {
                    _grid.Carts.Remove(cartsToRemove[0]);
                    _grid.Carts.Remove(cartsToRemove[1]);
                    cartsToRemove[0] = null;
                    if (_grid.Carts.Count == 1) {
                        return _grid.Carts[0].Point.Item1 + "," + _grid.Carts[0].Point.Item2;
                    }
                }
            } while (true);
        }

        private void GetCartVelocities() {
            foreach (var cart in _grid.Carts) {
                var point = _grid.Points[cart.Point];
                Tuple<int, int> nextPoint = null;
                switch (point.PointValue) {
                    case '<':
                        cart.Velocity = new Tuple<int, int>(-1, 0);
                        cart.Direction = (int)enumDirection.Left;
                        break;
                    case '>':
                        cart.Velocity = new Tuple<int, int>(1, 0);
                        cart.Direction = (int)enumDirection.Right;
                        break;
                    case '^':
                        cart.Velocity = new Tuple<int, int>(0, -1);
                        cart.Direction = (int)enumDirection.Up;
                        break;
                    case 'v':
                        cart.Velocity = new Tuple<int, int>(0, 1);
                        cart.Direction = (int)enumDirection.Down;
                        break;
                    default:
                        throw new NotImplementedException();
                }
                nextPoint = new Tuple<int, int>(point.XY.Item1 + cart.Velocity.Item1, point.XY.Item2 + cart.Velocity.Item2);
                foreach (var connection in _grid.Connections) {
                    if (connection.Points.Contains(point.XY)) {
                        var index1 = connection.Points.IndexOf(point.XY);
                        var index2 = connection.Points.IndexOf(nextPoint);
                        if (index2 > index1) {
                            cart.CurrentConnection = connection;
                            cart.PointIndex = index1;
                            break;
                        }
                    }
                }
            }
        }

        private void MakeVelocity() {
            var up = new Tuple<int, int>(0, -1);
            var down = new Tuple<int, int>(0, 1);
            var left = new Tuple<int, int>(-1, 0);
            var right = new Tuple<int, int>(1, 0);
            _grid.VelocityChange = new Dictionary<Tuple<int, int>, Dictionary<char, Tuple<int, int>>>();
            _grid.VelocityChange.Add(up, new Dictionary<char, Tuple<int, int>>());
            _grid.VelocityChange.Add(down, new Dictionary<char, Tuple<int, int>>());
            _grid.VelocityChange.Add(left, new Dictionary<char, Tuple<int, int>>());
            _grid.VelocityChange.Add(right, new Dictionary<char, Tuple<int, int>>());
            _grid.VelocityChange[up].Add('/', right);
            _grid.VelocityChange[up].Add('\\', left);
            _grid.VelocityChange[down].Add('/', left);
            _grid.VelocityChange[down].Add('\\', right);
            _grid.VelocityChange[left].Add('/', down);
            _grid.VelocityChange[left].Add('\\', up);
            _grid.VelocityChange[right].Add('/', up);
            _grid.VelocityChange[right].Add('\\', down);
            _grid.DirectionToVelocity.Add((int)enumDirection.Down, down);
            _grid.DirectionToVelocity.Add((int)enumDirection.Up, up);
            _grid.DirectionToVelocity.Add((int)enumDirection.Left, left);
            _grid.DirectionToVelocity.Add((int)enumDirection.Right, right);
            _grid.VelocityToDirection.Add(down, (int)enumDirection.Down);
            _grid.VelocityToDirection.Add(up, (int)enumDirection.Up);
            _grid.VelocityToDirection.Add(left, (int)enumDirection.Left);
            _grid.VelocityToDirection.Add(right, (int)enumDirection.Right);
        }

        private void MakeGrid() {
            var hash = new Dictionary<Tuple<int, int>, Intersection>();
            bool foundIntersections = false;
            foreach (var point in _grid.Points.Values) {
                if (!foundIntersections && point.PointType == enumPointType.Intersection) {
                    FindIntersections(point.XY, hash);
                    foundIntersections = true;
                }
                if (point.PointType == enumPointType.Cart) {
                    _grid.Carts.Add(new Cart() {
                        Point = point.XY,
                        ChangeDirectionList = new int[3] { 0, 1, 2 },
                        ChangeDirectionIndex = 0
                    });
                    
                }
            }
        }

        private int[,] GetChangeDirection() {
            var changeDirection = new int[4, 3];
            changeDirection[(int)enumDirection.Down, 0] = (int)enumDirection.Right;
            changeDirection[(int)enumDirection.Down, 1] = (int)enumDirection.Down;
            changeDirection[(int)enumDirection.Down, 2] = (int)enumDirection.Left;
            changeDirection[(int)enumDirection.Left, 0] = (int)enumDirection.Down;
            changeDirection[(int)enumDirection.Left, 1] = (int)enumDirection.Left;
            changeDirection[(int)enumDirection.Left, 2] = (int)enumDirection.Up;
            changeDirection[(int)enumDirection.Right, 0] = (int)enumDirection.Up;
            changeDirection[(int)enumDirection.Right, 1] = (int)enumDirection.Right;
            changeDirection[(int)enumDirection.Right, 2] = (int)enumDirection.Down;
            changeDirection[(int)enumDirection.Up, 0] = (int)enumDirection.Left;
            changeDirection[(int)enumDirection.Up, 1] = (int)enumDirection.Up;
            changeDirection[(int)enumDirection.Up, 2] = (int)enumDirection.Right;
            return changeDirection;
        }

        private Intersection FindIntersections(Tuple<int, int> current, Dictionary<Tuple<int, int>, Intersection> hash) {
            if (!hash.ContainsKey(current)) {
                var intersection = new Intersection();
                hash.Add(current, intersection);
                intersection.XY = current;
                intersection.Connections = new Connection[4];
                intersection.Connections[0] = FindConnection(current, new Tuple<int, int>(-1, 0), hash);
                intersection.Connections[1] = FindConnection(current, new Tuple<int, int>(0, -1), hash);
                intersection.Connections[2] = FindConnection(current, new Tuple<int, int>(1, 0), hash);
                intersection.Connections[3] = FindConnection(current, new Tuple<int, int>(0, 1), hash);
                _grid.Intersections.Add(current, intersection);
            }
            return hash[current];
        }

        private Connection FindConnection(Tuple<int, int> current, Tuple<int, int> velocity, Dictionary<Tuple<int, int>, Intersection> hash) {
            var connection = new Connection();
            _grid.Connections.Add(connection);
            connection.XYFrom = current;
            connection.Points.Add(current);
            connection.DirectionOnStart = _grid.VelocityToDirection[velocity];
            do {
                connection.Length++;
                current = new Tuple<int, int>(current.Item1 + velocity.Item1, current.Item2 + velocity.Item2);
                connection.Points.Add(current);
                var point = _grid.Points[current];
                switch (point.PointType) {
                    case enumPointType.Intersection:
                        connection.Next = FindIntersections(current, hash);
                        connection.XYTo = current;
                        connection.DirectionOnEnd = _grid.VelocityToDirection[velocity];
                        return connection;
                    case enumPointType.Turn:
                        velocity = _grid.VelocityChange[velocity][_grid.Points[current].PointValue];
                        break;
                }
            } while (true);
        }

        private Point FindFirstIntersection(List<Point> points) {
            foreach (var point in points) {
                if (point.PointType == enumPointType.Intersection) {
                    return point;
                }
            }
            throw new NotImplementedException();
        }

        private Tuple<int, int>[,] GetGridKeys() {
            var xMax = _grid.Points.Values.Select(x => x.XY.Item1).Max();
            var yMax = _grid.Points.Values.Select(x => x.XY.Item2).Max();
            var keys = new Tuple<int, int>[xMax + 1, yMax + 1];
            foreach (var point in _grid.Points.Values) {
                keys[point.XY.Item1, point.XY.Item2] = point.XY;
            }
            return keys;
        }

        private List<Point> GetPoints() {
            var points = new List<Point>();
            int y = 0;
            foreach (var line in Input()) {
                for (int x = 0; x < line.Length; x++) {
                    switch (line[x]) {
                        case '-':
                        case '|':
                            points.Add(new Point() {
                                PointType = enumPointType.Line,
                                PointValue = line[x],
                                XY = new Tuple<int, int>(x, y)
                            });
                            break;
                        case '/':
                        case '\\':
                            points.Add(new Point() {
                                PointType = enumPointType.Turn,
                                PointValue = line[x],
                                XY = new Tuple<int, int>(x, y)
                            });
                            break;
                        case '+':
                            points.Add(new Point() {
                                PointType = enumPointType.Intersection,
                                PointValue = line[x],
                                XY = new Tuple<int, int>(x, y)
                            });
                            break;
                        case '>':
                        case '<':
                        case '^':
                        case 'v':
                            points.Add(new Point() {
                                PointType = enumPointType.Cart,
                                PointValue = line[x],
                                XY = new Tuple<int, int>(x, y)
                            });
                            break;
                        case ' ':
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                }
                y++;
            }
            return points;
        }

        private List<string> TestInput() {
            return new List<string>() {
                @"/->-\        ",
                @"|   |  /----\",
                @"| /-+--+-\  |",
                @"| | |  | v  |",
                @"\-+-/  \-+--/",
                @"  \------/   "
            };
        }

        private enum enumPointType {
            Line,
            Turn,
            Intersection,
            Cart
        }

        private enum enumDirection {
            Left,
            Up,
            Right,
            Down
        }

        private class Point {
            public enumPointType PointType { get; set; }
            public Tuple<int, int> XY { get; set; }
            public char PointValue { get; set; }
        }

        private class Intersection {
            public Tuple<int, int> XY { get; set; }
            public Connection[] Connections { get; set; }
        }

        private class Connection {
            public Connection() {
                Points = new List<Tuple<int, int>>();
            }
            public Tuple<int, int> XYFrom { get; set; }
            public Tuple<int, int> XYTo { get; set; }
            public Intersection Next { get; set; }
            public int Length { get; set; }
            public List<Tuple<int, int>> Points { get; set; }
            public int DirectionOnStart { get; set; }
            public int DirectionOnEnd { get; set; }
        }

        private class Cart {
            public Tuple<int, int> Point { get; set; }
            public int PointIndex { get; set; }
            public int Direction { get; set; }
            public Tuple<int, int> Velocity { get; set; }
            public int ChangeDirectionIndex { get; set; }
            public int[] ChangeDirectionList { get; set; }
            public Connection CurrentConnection { get; set; }
        }

        private class Grid {
            public Grid() {
                Intersections = new Dictionary<Tuple<int, int>, Intersection>();
                Connections = new List<Connection>();
                Carts = new List<Cart>();
                Points = new Dictionary<Tuple<int, int>, Point>();
                VelocityChange = new Dictionary<Tuple<int, int>, Dictionary<char, Tuple<int, int>>>();
                VelocityToDirection = new Dictionary<Tuple<int, int>, int>();
                DirectionToVelocity = new Dictionary<int, Tuple<int, int>>();
            }
            public Dictionary<Tuple<int, int>, Intersection> Intersections { get; set; }
            public List<Connection> Connections { get; set; }
            public List<Cart> Carts { get; set; }
            public Dictionary<Tuple<int, int>, Point> Points { get; set; }
            public Dictionary<Tuple<int, int>, Dictionary<char, Tuple<int, int>>> VelocityChange;
            public Dictionary<Tuple<int, int>, int> VelocityToDirection { get; set; }
            public Dictionary<int, Tuple<int, int>> DirectionToVelocity { get; set; }
        }
    }
}
