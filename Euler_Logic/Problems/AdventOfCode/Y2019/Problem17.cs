using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems.AdventOfCode.Y2019 {
    public class Problem17 : AdventOfCodeBase {
        private Dictionary<int, Dictionary<int, Node>> _grid;
        private int[] _functions;
        private int[][] _routes;
        private int[] _routeIndexes;
        private int _x;
        private int _y;
        private int _direction;
        private int _route;
        private int _function;
        private int _traverseCount;
        private int _traverseMax;
        private int[][] _directionChange;

        public enum enumNodeType {
            Empty,
            Scaffold,
            RobotGone,
            RobotUp,
            RobotDown,
            RobotLeft,
            RobotRight
        }

        public override string ProblemName {
            get { return "Advent of Code 2019: 17"; }
        }

        public override string GetAnswer() {
            return Answer2(Input()).ToString();
        }

        private int Answer1(List<string> input) {
            GetGrid(input);
            SetIntersection();
            return GetCalibrate();
        }

        private int Answer2(List<string> input) {
            GetGrid(input);
            SetTestInput(Test2Input());
            SetIntersection();
            SetAdjacent();
            Initialize();
            FindNextRoute();
            return 0;
        }

        private void Initialize() {
            _functions = new int[20];
            _routes = new int[3][];
            _routeIndexes = new int[3] { -1, -1, -1 };
            _routes[0] = new int[20];
            _routes[1] = new int[20];
            _routes[2] = new int[20];
            foreach (var x in _grid) {
                foreach (var y in x.Value) {
                    if (IsScaffold(y.Value)) _traverseMax++;
                }
            }
            _traverseCount = 1;
            _function = -1;
            _directionChange = new int[4][];
            _directionChange[0] = new int[4] { 0, 1, 0, -1 };
            _directionChange[1] = new int[4] { -1, 0, 1, 0 };
            _directionChange[2] = new int[4] { 0, -1, 0, 1 };
            _directionChange[3] = new int[4] { 1, 0, -1, 0 };
        }

        private bool FindNextRoute() {
            if (_traverseCount == _traverseMax) return true;
            if (_function < 19) {
                _function++;
                if (FindNextRouteExisting()) return true;
                if (FindNextRouteNew()) return true;
                _function--;
            }
            return false;
        }

        private bool FindNextRouteExisting() {
            for (int route = 0; route <= _route; route++) {
                _functions[_function] = route;
                if (PerformRoute(route, false)) return true;
            }
            return false;
        }

        private bool FindNextRouteNew() {
            if (_route < 3) {
                var node = _grid[_x][_y];
                SetNewRoute(node, 0, 1);
                SetNewRoute(node, 1, 0);
                SetNewRoute(node, 2, 1);
                SetNewRoute(node, 3, 0);
            }
            return false;
        }

        private bool SetNewRoute(Node node, int direction, int mod) {
            if (_direction % 2 == mod && node.Adjacent[direction] != null && !node.Adjacent[direction].IsTraversed) {
                _route++;
                _functions[_function] = _route;
                _routes[_route][0] = _directionChange[_direction][direction];
                var nodes = new List<Node>();
                int x = _x;
                int y = _y;
                int tdirection = _direction;
                int traverseCount = _traverseCount;
                int count = 0;
                do {
                    count++;
                    var next = node.Adjacent[_direction];
                    if (next.IsIntersection) {
                        
                    }
                } while (true);

                nodes.ForEach(node => node.IsTraversed = false);
                _x = x;
                _y = y;
                _direction = tdirection;
                _traverseCount = traverseCount;
                _route--;
            }
        }

        private bool PerformRoute(int route, bool isDebug) {
            int x = _x;
            int y = _y;
            int direction = _direction;
            int traverseCount = _traverseCount;
            var nodes = new List<Node>();
            bool isFirst = true;
            for (int index = 0; index <= _routeIndexes[route]; index += 2) {
                _direction += _routes[route][index];
                int maxMove = _routes[route][index + 1];
                for (int move = 1; move <= maxMove; move++) {
                    MoveDirection();
                    if (isFirst) {
                        if (_grid[_x][_y].IsTraversed) return false;
                        isFirst = false;
                    }
                    if (!IsScaffold(_x, _y)) return false;
                    var node = _grid[_x][_y];
                    if (!node.IsTraversed) {
                        node.IsTraversed = true;
                        nodes.Add(node);
                        _traverseCount++;
                    }
                }
            }
            if (FindNextRoute()) return true;
            nodes.ForEach(node => node.IsTraversed = false);
            _x = x;
            _y = y;
            _direction = direction;
            _traverseCount = traverseCount;
            return false;
        }

        private void MoveDirection() {
            switch (_direction) {
                case 0:
                    _y -= 1;
                    break;
                case 1:
                    _x += 1;
                    break;
                case 2:
                    _y += 1;
                    break;
                case 3:
                    _x -= 1;
                    break;
            }
        }

        private void SetIntersection() {
            foreach (var x in _grid) {
                foreach (var y in x.Value) {
                    if (IsScaffold(y.Value)) {
                        int count = 0;
                        if (IsScaffold(x.Key - 1, y.Key)) count++;
                        if (IsScaffold(x.Key + 1, y.Key)) count++;
                        if (IsScaffold(x.Key, y.Key - 1)) count++;
                        if (IsScaffold(x.Key, y.Key + 1)) count++;
                        if (count >= 4) y.Value.IsIntersection = true;
                    }
                }
            }
        }

        private int GetCalibrate() {
            int sum = 0;
            foreach (var x in _grid) {
                foreach (var y in x.Value) {
                    if (y.Value.IsIntersection) {
                        sum += x.Key * y.Key;
                    }
                }
            }
            return sum;
        }

        private bool IsScaffold(int x, int y) {
            if (_grid.ContainsKey(x) && _grid[x].ContainsKey(y)) {
                var node = _grid[x][y];
                return IsScaffold(node);
            }
            return false;
        }

        private bool IsScaffold(Node node) {
            return node.NodeType == enumNodeType.Scaffold || node.IsRobot;
        }

        private enumNodeType GetNodeType(int x, int y) {
            if (_grid.ContainsKey(x) && _grid[x].ContainsKey(y)) {
                return _grid[x][y].NodeType;
            }
            return enumNodeType.Empty;
        }

        private void GetGrid(List<string> input) {
            _grid = new Dictionary<int, Dictionary<int, Node>>();
            var computer = new IntComputer();
            int x = 0;
            int y = 0;
            computer.Run(input, null, () => {
                var output = computer.Output.Last();
                if (output == 10) {
                    x = 0;
                    y++;
                } else {
                    if (!_grid.ContainsKey(x)) {
                        _grid.Add(x, new Dictionary<int, Node>());
                    }
                    var node = new Node() { Digit = (char)output };
                    node.NodeType = GetNodeType(node.Digit);
                    _grid[x].Add(y, node);
                    node.IsRobot = (int)node.NodeType > (int)enumNodeType.RobotGone;
                    x++;
                }
            });
        }

        public void SetAdjacent() {
            int minX = _grid.Keys.Min();
            int maxX = _grid.Keys.Max();
            int minY = _grid.Select(x => x.Value.Keys.Min()).Min();
            int maxY = _grid.Select(x => x.Value.Keys.Max()).Max();
            for (int x = minX; x <= maxX; x++) {
                for (int y = minY; y <= maxY; y++) {
                    if (IsScaffold(x, y)) {
                        var node = _grid[x][y];
                        if (IsScaffold(x, y - 1)) node.Adjacent[0] = _grid[x][y - 1];
                        if (IsScaffold(x + 1, y)) node.Adjacent[1] = _grid[x + 1][y];
                        if (IsScaffold(x, y + 1)) node.Adjacent[2] = _grid[x][y + 1];
                        if (IsScaffold(x - 1, y)) node.Adjacent[3] = _grid[x - 1][y];
                    }
                }
            }
        }

        private enumNodeType GetNodeType(char digit) {
            switch (digit) {
                case '.': return enumNodeType.Empty;
                case '#': return enumNodeType.Scaffold;
                case '^': return enumNodeType.RobotUp;
                case 'v': return enumNodeType.RobotDown;
                case '<': return enumNodeType.RobotLeft;
                case '>': return enumNodeType.RobotRight;
                case 'X': return enumNodeType.RobotGone;
                default:
                    throw new Exception();
            }
        }

        private string Output() {
            var text = new StringBuilder();
            int minX = _grid.Keys.Min();
            int maxX = _grid.Keys.Max();
            int minY = _grid.Select(x => x.Value.Keys.Min()).Min();
            int maxY = _grid.Select(x => x.Value.Keys.Max()).Max();
            for (int y = minY; y <= maxY; y++) {
                for (int x = minX; x <= maxX; x++) {
                    var nodeType = GetNodeType(x, y);
                    switch (nodeType) {
                        case enumNodeType.Empty:
                            text.Append(".");
                            break;
                        case enumNodeType.RobotDown:
                            text.Append("v");
                            break;
                        case enumNodeType.RobotGone:
                            text.Append("X");
                            break;
                        case enumNodeType.RobotLeft:
                            text.Append("<");
                            break;
                        case enumNodeType.RobotRight:
                            text.Append(">");
                            break;
                        case enumNodeType.RobotUp:
                            text.Append("^");
                            break;
                        case enumNodeType.Scaffold:
                            text.Append("#");
                            break;
                    }
                }
                text.AppendLine("");
            }
            return text.ToString();
        }

        private void SetTestInput(List<string> input) {
            _grid = new Dictionary<int, Dictionary<int, Node>>();
            int y = 0;
            foreach (var line in input) {
                int x = 0;
                foreach (var digit in line) {
                    var node = new Node() { Digit = digit };
                    node.NodeType = GetNodeType(node.Digit);
                    if (!_grid.ContainsKey(x)) {
                        _grid.Add(x, new Dictionary<int, Node>());
                    }
                    _grid[x].Add(y, node);
                    x++;
                }
                y++;
            }
        }

        private List<string> Test1Input() {
            return new List<string>() {
                "..#..........",
                "..#..........",
                "#######...###",
                "#.#...#...#.#",
                "#############",
                "..#...#...#..",
                "..#####...^.."
            };
        }

        private List<string> Test2Input() {
            return new List<string>() {
                "#######...#####",
                "#.....#...#...#",
                "#.....#...#...#",
                "......#...#...#",
                "......#...###.#",
                "......#.....#.#",
                "^########...#.#",
                "......#.#...#.#",
                "......#########",
                "........#...#..",
                "....#########..",
                "....#...#......",
                "....#...#......",
                "....#...#......",
                "....#####......"
            };
        }

        private class Node {
            public Node() {
                Adjacent = new Node[4];
            }
            public enumNodeType NodeType { get; set; }
            public char Digit { get; set; }
            public bool IsRobot { get; set; }
            public Node[] Adjacent { get; set; }
            public bool IsTraversed { get; set; }
            public bool IsIntersection { get; set; }
        }
    }
}
