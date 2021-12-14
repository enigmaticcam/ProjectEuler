using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Euler_Logic.Problems.AdventOfCode.Y2019 {
    public class Problem17 : AdventOfCodeBase {
        private Dictionary<int, Dictionary<int, Node>> _grid;

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
            // I cheated and found these in excel :-/
            var mainRoutine = "A,A,B,B,C,B,C,B,C,A\n";
            var routeA = "L,10,L,10,R,6\n";
            var routeB = "R,12,L,12,L,12\n";
            var routeC = "L,6,L,10,R,12,R,12\n";

            int digitIndex = -1;
            int textIndex = 0;
            var text = new string[5] { mainRoutine, routeA, routeB, routeC, "n\n" };

            var comp = new IntComputer();

            comp.Run(input, () => {
                digitIndex++;
                if (digitIndex == text[textIndex].Length) {
                    digitIndex = 0;
                    textIndex++;
                }
                return text[textIndex][digitIndex];
            }, () => { }, new List<IntComputer.LineOverride>() { new IntComputer.LineOverride() { Index = 0, Value = 2 } });
            return 0;
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
