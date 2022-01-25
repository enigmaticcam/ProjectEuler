using Euler_Logic.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Euler_Logic.Problems.AdventOfCode.Y2018 {
    public class Problem15 : AdventOfCodeBase {
        private BinaryHeap_Min _heap;
        private Node[,] _grid;
        private List<Node> _nodes;
        private List<Unit> _units;
        private int[] _unitCounts;

        public override string ProblemName {
            get { return "Advent of Code 2018: 15"; }
        }

        public override string GetAnswer() {
            return Answer1(Input()).ToString();
        }

        public override string GetAnswer2() {
            return Answer2(Input()).ToString();
        }

        private int Answer1(List<string> input) {
            GetGrid(input);
            SetEdges();
            ResetUnits(3);
            SetUnitCounts();
            return Perform(false);
        }

        private int Answer2(List<string> input) {
            GetGrid(input);
            SetEdges();
            ResetUnits(3);
            SetUnitCounts();
            return FindPower();
        }

        private int FindPower() {
            int power = 4;
            do {
                ResetUnits(power);
                SetUnitCounts();
                var result = Perform(true);
                if (result != 0) return result;
                power++;
            } while (true);
        }

        private int Perform(bool stopAfterDeath) {
            int count = 0;
            int start = _unitCounts[0];
            do {
                count++;
                if (!PerformRound()) break;
                if (stopAfterDeath && _unitCounts[0] != start) return 0;
            } while (true);
            count--;
            var unitSum = _units.Where(x => x.HP > 0).Select(x => x.HP).Sum();
            return count * unitSum;
        }

        private bool PerformRound() {
            var units = _units.OrderBy(x => x.Point[1]).ThenBy(x => x.Point[0]).ToList();
            for (int index = 0; index < units.Count; index++) {
                var unit = units[index];
                if (unit.HP > 0) {
                    Move(unit);
                    Attack(unit);
                    if (_unitCounts[0] == 0 || _unitCounts[1] == 0) {
                        for (int next = index + 1; next < units.Count; next++) {
                            if (units[next].HP > 0) {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }

        private bool Move(Unit unit) {
            var targets = FindTargets(unit);
            if (targets.Where(x => x.Num == 1).Count() > 0) return false;
            var edges = targets.SelectMany(x => x.Edges)
                .Where(x => x.OccupiedUnit == null)
                .OrderBy(x => x.Num)
                .ThenBy(x => x.Point[1])
                .ThenBy(x => x.Point[0]);
            if (edges.Count() > 0) {
                var target = edges.First();
                var start = _grid[unit.Point[0], unit.Point[1]];
                Node bestEdge = null;
                int bestDistance = int.MaxValue;
                foreach (var edge in start.Edges) {
                    if (edge.OccupiedUnit == null) {
                        var distance = FindDistance(edge.Point, target.Point);
                        if (bestEdge == null) {
                            bestEdge = edge;
                            bestDistance = distance;
                        } else {
                            if (distance < bestDistance) {
                                bestEdge = edge;
                                bestDistance = distance;
                            } else if (distance == bestDistance && edge.Point[1] < bestEdge.Point[1]) {
                                bestEdge = edge;
                                bestDistance = distance;
                            } else if (distance == bestDistance && edge.Point[1] == bestEdge.Point[1] && edge.Point[0] < bestEdge.Point[0]) {

                                bestEdge = edge;
                                bestDistance = distance;
                            }
                        }
                    }
                }
                if (bestEdge != null && bestDistance != int.MaxValue) {
                    Move(unit, bestEdge);
                    return true;
                } else {
                    return false;
                }
            }
            return false;
        }

        private void Move(Unit unit, Node node) {
            _grid[unit.Point[0], unit.Point[1]].OccupiedUnit = null;
            unit.Point = node.Point;
            node.OccupiedUnit = unit;
        }

        private int FindDistance(int[] start, int[] end) {
            _heap.Reset();
            _nodes.ForEach(x => x.Num = int.MaxValue);
            _grid[start[0], start[1]].Num = 0;
            _heap.Adjust(_grid[start[0], start[1]]);
            for (int count = 1; count <= _nodes.Count; count++) {
                var current = (Node)_heap.Top;
                if (current.Num == int.MaxValue) return int.MaxValue;
                if (current.Point[0] == end[0] && current.Point[1] == end[1]) return current.Num;
                if (current.OccupiedUnit == null) {
                    foreach (var edge in current.Edges) {
                        if (edge.Num > current.Num + 1) {
                            edge.Num = current.Num + 1;
                            _heap.Adjust(edge);
                        }
                    }
                }
                _heap.Remove(current);
            }
            return int.MaxValue;
        }

        private List<Node> FindTargets(Unit unit) {
            var targets = new List<Node>();
            _heap.Reset();
            _nodes.ForEach(x => x.Num = int.MaxValue);
            var start = _grid[unit.Point[0], unit.Point[1]];
            start.Num = 0;
            _heap.Adjust(start);
            for (int count = 1; count <= _nodes.Count; count++) {
                var current = (Node)_heap.Top;
                if (current.Num == int.MaxValue) break;
                if (current.OccupiedUnit != null && current.OccupiedUnit.IsGoblin != unit.IsGoblin) targets.Add(current);
                if (current.OccupiedUnit == null || current == start) {
                    foreach (var edge in current.Edges) {
                        if (edge.Num > current.Num + 1) {
                            edge.Num = current.Num + 1;
                            _heap.Adjust(edge);
                        }
                    }
                }
                _heap.Remove(current);
            }
            return targets;
        }

        private bool Attack(Unit attacker) {
            var node = _grid[attacker.Point[0], attacker.Point[1]];
            var edges = node.Edges
                .Where(x => x.OccupiedUnit != null && x.OccupiedUnit.IsGoblin != attacker.IsGoblin)
                .OrderBy(x => x.OccupiedUnit.HP)
                .ThenBy(x => x.OccupiedUnit.Point[1])
                .ThenBy(x => x.OccupiedUnit.Point[0]);
            if (edges.Count() > 0) {
                PerformAttack(attacker, edges.First().OccupiedUnit);
                return true;
            }
            return false;
        }

        private void PerformAttack(Unit attacker, Unit defender) {
            defender.HP -= attacker.AttackPower;
            if (defender.HP <= 0) {
                _grid[defender.Point[0], defender.Point[1]].OccupiedUnit = null;
                _unitCounts[defender.IsGoblin ? 1 : 0]--;
            }
        }

        private void SetUnitCounts() {
            _unitCounts = new int[2];
            foreach (var unit in _units) {
                _unitCounts[unit.IsGoblin ? 1 : 0]++;
            }
        }

        private void ResetUnits(int elftAttackPower) {
            foreach (var unit in _units) {
                if (unit.IsGoblin) {
                    unit.AttackPower = 3;
                } else {
                    unit.AttackPower = elftAttackPower;
                }
                unit.HP = 200;
                if (_grid[unit.Point[0], unit.Point[1]].OccupiedUnit == unit) {
                    _grid[unit.Point[0], unit.Point[1]].OccupiedUnit = null;
                }
                unit.Start.OccupiedUnit = unit;
                unit.Point = unit.Start.Point;
            }
        }

        private void SetEdges() {
            foreach (var node in _nodes) {
                node.Edges = new List<Node>();
                if (_grid[node.Point[0] - 1, node.Point[1]] != null) node.Edges.Add(_grid[node.Point[0] - 1, node.Point[1]]);
                if (_grid[node.Point[0] + 1, node.Point[1]] != null) node.Edges.Add(_grid[node.Point[0] + 1, node.Point[1]]);
                if (_grid[node.Point[0], node.Point[1] - 1] != null) node.Edges.Add(_grid[node.Point[0], node.Point[1] - 1]);
                if (_grid[node.Point[0], node.Point[1] + 1] != null) node.Edges.Add(_grid[node.Point[0], node.Point[1] + 1]);
            }
        }

        private void GetGrid(List<string> input) {
            _heap = new BinaryHeap_Min();
            _grid = new Node[input[0].Length + 1, input.Count + 1];
            _nodes = new List<Node>();
            _units = new List<Unit>();
            for (int y = 0; y < input.Count; y++) {
                for (int x = 0; x < input[y].Length; x++) {
                    var digit = input[y][x];
                    if (digit != '#') {
                        var node = new Node() { Point = new int[2] { x + 1, y + 1 } };
                        _grid[x + 1, y + 1] = node;
                        _nodes.Add(node);
                        _heap.Add(node);
                        if (digit == 'E') {
                            node.OccupiedUnit = new Unit() { Point = node.Point };
                        } else if (digit == 'G') {
                            node.OccupiedUnit = new Unit() { Point = node.Point, IsGoblin = true };
                        }
                        if (node.OccupiedUnit != null) {
                            _units.Add(node.OccupiedUnit);
                            node.OccupiedUnit.Start = node;
                        }
                    }
                }
            }
        }

        private string Output() {
            var text = new StringBuilder();
            for (int y = 1; y <= _grid.GetUpperBound(1); y++) {
                for (int x = 1; x <= _grid.GetUpperBound(0); x++) {
                    if (_grid[x, y] == null) {
                        text.Append("#");
                    } else {
                        var node = _grid[x, y];
                        if (node.OccupiedUnit == null) {
                            text.Append(".");
                        } else if (node.OccupiedUnit.IsGoblin) {
                            text.Append("G");
                        } else {
                            text.Append("E");
                        }
                    }
                }
                text.AppendLine();
            }
            return text.ToString();
        }

        private class Node : BinaryHeap_Min.Node {
            public int[] Point { get; set; }
            public List<Node> Edges { get; set; }
            public Unit OccupiedUnit { get; set; }
        }

        private class Unit {
            public bool IsGoblin { get; set; }
            public int HP { get; set; }
            public int AttackPower { get; set; }
            public int[] Point { get; set; }
            public Node Start { get; set; }
        }
    }
}
