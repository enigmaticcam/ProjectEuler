using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems.AdventOfCode.Y2019 {
    public class Problem15 : AdventOfCodeBase {
        public override string ProblemName {
            get { return "Advent of Code 2019: 15"; }
        }

        private Grid _grid;
        private int _currentX;
        private int _currentY;
        private int _robotX;
        private int _robotY;
        public override string GetAnswer() {
            _grid = new Grid();
            _grid.AddNode(0, 0, enumSpot.Empty);
            return Answer1(Input()).ToString();
        }

        private int Answer1(List<string> input) {
            var computer = new IntComputer();
            computer.Run(
                instructions: input,
                input: () => HandleInput(computer),
                outputCaller: () => HandleOutput(computer));
            return 0;
        }

        private int HandleInput(IntComputer computer) {
            if (!_grid[_robotX, _robotY].VisitCheck.IsFullyVisited) {
                return ExploreIncomplete();
            } else {
                return MoveTowardsClosestIncomplete();
            }
        }

        private int MoveTowardsClosestIncomplete() {
            var closestIncomplete = FindClosestIncomplete();
            var bestMove = BestMoveToIncomplete(closestIncomplete);
            _currentX = bestMove.X;
            _currentY = bestMove.Y;
            if (_currentX == _robotX + 1) {
                return 4;
            } else if (_currentX == _robotX - 1) {
                return 3;
            } else if (_currentY == _robotY + 1) {
                return 2;
            } else if (_currentY == _robotY - 1) {
                return 1;
            }
            throw new Exception();
        }

        private Node FindClosestIncomplete() {
            FindShortestDistance(_robotX, _robotY, GetUnvisited(), null);
            int bestDistance = int.MaxValue;
            Node bestNode = null;
            foreach (var node in _grid.Nodes) {
                if (!node.VisitCheck.IsFullyVisited) {
                    if (node.Distance < bestDistance) {
                        bestDistance = node.Distance;
                        bestNode = node;
                    }
                }
            }
            if (bestNode == null) {
                bool stop = true;
            }
            return bestNode;
        }

        private Node BestMoveToIncomplete(Node incomplete) {
            var complete = new List<Node>();
            if (_grid.CanMoveTo(_robotX + 1, _robotY)) {
                complete.Add(_grid[_robotX + 1, _robotY]);
            }
            if (_grid.CanMoveTo(_robotX - 1, _robotY)) {
                complete.Add(_grid[_robotX - 1, _robotY]);
            }
            if (_grid.CanMoveTo(_robotX, _robotY + 1)) {
                complete.Add(_grid[_robotX, _robotY + 1]);
            }
            if (_grid.CanMoveTo(_robotX, _robotY - 1)) {
                complete.Add(_grid[_robotX, _robotY - 1]);
            }
            FindShortestDistance(incomplete.X, incomplete.Y, GetUnvisited(), complete);
            Node bestNode = null;
            int bestDistance = int.MaxValue;
            foreach (var node in complete) {
                if (node.Distance < bestDistance) {
                    bestNode = node;
                    bestDistance = node.Distance;
                }
            }
            return bestNode;
        }

        private int ExploreIncomplete() {
            var node = _grid[_robotX, _robotY];
            if (!node.VisitCheck.VisitedFromRight) {
                _currentX = _robotX + 1;
                return 4;
            } else if (!node.VisitCheck.VisitedFromLeft) {
                _currentX = _robotX - 1;
                return 3;
            } else if (!node.VisitCheck.VisitedFromUp) {
                _currentY = _robotY - 1;
                return 1;
            } else if (!node.VisitCheck.VisitedFromDown) {
                _currentY = _robotY + 1;
                return 2;
            }
            throw new Exception();
        }

        private void HandleOutput(IntComputer computer) {
            var status = (enumSpot)computer.Output.Last();
            _grid.AddNode(_currentX, _currentY, status);
            if (_currentX == _robotX + 1) {
                _grid[_robotX, _robotY].VisitCheck.VisitedFromRight = true;
                _grid[_currentX, _currentY].VisitCheck.VisitedFromLeft = true;
            } else if (_currentX == _robotX - 1) {
                _grid[_robotX, _robotY].VisitCheck.VisitedFromLeft = true;
                _grid[_currentX, _currentY].VisitCheck.VisitedFromRight = true;
            } else if (_currentY == _robotY + 1) {
                _grid[_robotX, _robotY].VisitCheck.VisitedFromDown = true;
                _grid[_currentX, _currentY].VisitCheck.VisitedFromUp = true;
            } else {
                _grid[_robotX, _robotY].VisitCheck.VisitedFromUp = true;
                _grid[_currentX, _currentY].VisitCheck.VisitedFromDown = true;
            }
            if (status != enumSpot.Wall) {
                _robotX = _currentX;
                _robotY = _currentY;
            } else {
                _currentX = _robotX;
                _currentY = _robotY;
            }
        }

        private void FindShortestDistance(int x, int y, List<Node> unvisited, List<Node> complete) {
            bool finished = false;
            while (unvisited.Count > 0 || finished) {
                var shortest = GetShortest(unvisited);
                if (!shortest.VisitDistance.VisitedFromDown) {
                    if (_grid.CanMoveTo(shortest.X, shortest.Y + 1)) {
                        var next = _grid[shortest.X, shortest.Y + 1];
                        next.Distance = Math.Min(next.Distance, shortest.Distance + 1);
                    }
                    shortest.VisitDistance.VisitedFromDown = true;
                }
                if (!shortest.VisitDistance.VisitedFromUp) {
                    if (_grid.CanMoveTo(shortest.X, shortest.Y - 1)) {
                        var next = _grid[shortest.X, shortest.Y - 1];
                        next.Distance = Math.Min(next.Distance, shortest.Distance + 1);
                    }
                    shortest.VisitDistance.VisitedFromUp = true;
                }
                if (!shortest.VisitDistance.VisitedFromLeft) {
                    if (_grid.CanMoveTo(shortest.X - 1, shortest.Y)) {
                        var next = _grid[shortest.X - 1, shortest.Y];
                        next.Distance = Math.Min(next.Distance, shortest.Distance + 1);
                    }
                    shortest.VisitDistance.VisitedFromLeft = true;
                }
                if (!shortest.VisitDistance.VisitedFromRight) {
                    if (_grid.CanMoveTo(shortest.X + 1, shortest.Y)) {
                        var next = _grid[shortest.X + 1, shortest.Y];
                        next.Distance = Math.Min(next.Distance, shortest.Distance + 1);
                    }
                    shortest.VisitDistance.VisitedFromLeft = true;
                }
                if (complete != null) {
                    finished = true;
                    foreach (var node in complete) {
                        if (!node.VisitDistance.IsFullyVisited) {
                            finished = false;
                            break;
                        }
                    }
                }
            }
        }

        private List<Node> GetUnvisited() {
            var unvisited = new List<Node>();
            for (int x = _grid.MinX; x <= _grid.MaxX; x++) {
                for (int y = _grid.MinY; y <= _grid.MaxY; y++) {
                    if (_grid.CanMoveTo(x, y)) {
                        var next = _grid[x, y];
                        next.Distance = int.MaxValue;
                        next.VisitDistance.ResetVisits();
                        unvisited.Add(next);
                    }
                }
            }
            return unvisited;
        }

        private Node GetShortest(List<Node> unvisited) {
            Node shortest = null;
            foreach (var next in unvisited) {
                if (!next.VisitDistance.IsFullyVisited) {
                    if (shortest == null || next.Distance < shortest.Distance) {
                        shortest = next;
                    } 
                }
            }
            return shortest;
        }

        private string PrintOutput() {
            var text = new StringBuilder();
            for (int y = _grid.MinY; y <= _grid.MaxY; y++) {
                var line = new char[_grid.MaxX - _grid.MinX + 1];
                int index = 0;
                for (int x = _grid.MinX; x <= _grid.MaxX; x++) {
                    if (_grid.Exists(x, y)) {
                        var spot = _grid[x, y].Spot;
                        if (spot == enumSpot.Empty) {
                            line[index] = '.';
                        } else if (spot == enumSpot.Finish) {
                            line[index] = 'O';
                        } else {
                            line[index] = '|';
                        }
                    }  else {
                        line[index] = ' ';
                    }
                    index++;
                }
                text.AppendLine(new string(line));
            }
            return text.ToString();
        }

        private enum enumSpot {
            Wall,
            Empty,
            Finish
        }

        private class Grid {
            private Dictionary<int, Dictionary<int, Node>> _nodes = new Dictionary<int, Dictionary<int, Node>>();
            private List<Node> _list = new List<Node>();

            private int _maxX;
            public int MaxX {
                get { return _maxX; }
            }

            private int _maxY;
            public int MaxY {
                get { return _maxY; }
            }

            private int _minX;
            public int MinX {
                get { return _minX; }
            }

            private int _minY;
            public int MinY {
                get { return _minY; }
            }

            public IEnumerable<Node> Nodes {
                get { return _list; }
            }

            public bool CanMoveTo(int x, int y) {
                if (!_nodes.ContainsKey(x)) {
                    return false;
                } else if (!_nodes[x].ContainsKey(y)) {
                    return false;
                } else if (_nodes[x][y].Spot == enumSpot.Wall) {
                    return false;
                }
                return true;
            }

            public Node this[int x, int y] {
                get { return _nodes[x][y]; }
            }

            public void AddNode(int x, int y, enumSpot spot) {
                var newNode = new Node() {
                    Spot = spot,
                    X = x,
                    Y = y
                };
                if (!_nodes.ContainsKey(x)) {
                    _nodes.Add(x, new Dictionary<int, Node>());
                }
                _nodes[x].Add(y, newNode);
                _list.Add(newNode);
                if (x < _minX) {
                    _minX = x;
                } else if (x > _maxX) {
                    _maxX = x;
                }
                if (y < _minY) {
                    _minY = y;
                } else if (y > _maxY) {
                    _maxY = y;
                }
            }

            private void UpdateVisitCheck(int x, int y) {
                var node = _nodes[x][y];
                node.VisitCheck.ResetVisits();
                if (_nodes.ContainsKey(x + 1) && _nodes.ContainsKey(y)) {
                    node.VisitCheck.VisitedFromRight = true;
                }
                if (_nodes.ContainsKey(x - 1) && _nodes.ContainsKey(y)) {
                    node.VisitCheck.VisitedFromLeft = true;
                }
                if (_nodes.ContainsKey(x) && _nodes.ContainsKey(y - 1)) {
                    node.VisitCheck.VisitedFromUp = true;
                }
                if (_nodes.ContainsKey(x) && _nodes.ContainsKey(y + 1)) {
                    node.VisitCheck.VisitedFromDown = true;
                }
            }

            public bool Exists(int x, int y) {
                if (_nodes.ContainsKey(x) && _nodes[x].ContainsKey(y)) {
                    return true;
                }
                return false;
            }
        }

        private class Node {
            public Node() {
                VisitDistance = new Visit();
                VisitCheck = new Visit();
            }
            public int X { get; set; }
            public int Y { get; set; }
            public int Distance { get; set; }
            public enumSpot Spot { get; set; }
            public Visit VisitDistance { get; set; }
            public Visit VisitCheck { get; set; }
        }

        private class Visit {
            private int _visits = 0;
            public bool IsFullyVisited {
                get {
                    if (_visits == 15) {
                        return true;
                    } else {
                        return false;
                    }
                }
            }

            public void ResetVisits() {
                _visits = 0;
            }

            public bool VisitedFromUp {
                get {
                    if ((_visits & 1) == 1) {
                        return true;
                    } else {
                        return false;
                    }
                }
                set { _visits += 1; }
            }

            public bool VisitedFromLeft {
                get {
                    if ((_visits & 2) == 2) {
                        return true;
                    } else {
                        return false;
                    }
                }
                set { _visits += 2; }
            }

            public bool VisitedFromRight {
                get {
                    if ((_visits & 4) == 4) {
                        return true;
                    } else {
                        return false;
                    }
                }
                set { _visits += 4; }
            }

            public bool VisitedFromDown {
                get {
                    if ((_visits & 8) == 8) {
                        return true;
                    } else {
                        return false;
                    }
                }
                set { _visits += 8; }
            }
        }
    }
}
