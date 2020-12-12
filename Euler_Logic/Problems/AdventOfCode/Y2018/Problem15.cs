using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems.AdventOfCode.Y2018 {
    public class Problem15 : AdventOfCodeBase {
        private int _maxValue;
        private bool _noTargets;

        public override string ProblemName {
            get { return "Advent of Code 2018: 15"; }
        }

        public override string GetAnswer() {
            _maxValue = int.MaxValue - 100;
            return Answer2();
        }

        private GridNodeLists _gridNodes;
        public string Answer1() {
            _gridNodes = GetGridNodes(Input(), 3);
            return PerformFight().ToString();
        }

        public string Answer2() {
            return FindBestAttackPower().ToString();
        }

        private int FindBestAttackPower() {
            int attack = 100;
            int nextAttack = 50;
            int count = 0;
            int totalElfCount = GetGridNodes(Input(), 0).ElvesByList.Count;
            int result = 0;
            do {
                _gridNodes = GetGridNodes(Input(), attack);
                result = PerformFight();
                count = _gridNodes.ElvesByList.Where(x => x.HP > 0).Count();
                if (count < totalElfCount) {
                    attack += nextAttack;
                } else {
                    attack -= nextAttack;
                }
                nextAttack /= 2;
            } while (nextAttack > 1);
            if (count < totalElfCount) {
                nextAttack = 1;
            } else {
                nextAttack = -1;
            }
            int lastResult = result;
            do {
                attack += nextAttack;
                _gridNodes = GetGridNodes(Input(), attack);
                result = PerformFight();
                count = _gridNodes.ElvesByList.Where(x => x.HP > 0).Count();
                if (nextAttack == 1 && count == totalElfCount) {
                    return result;
                } else if (nextAttack == -1 && count < totalElfCount) {
                    return lastResult;
                }
                lastResult = result;
            } while (true);
        }

        private int PerformFight() {
            int rounds = -1;
            var finish = false;
            do {
                finish = PerformUnitAction();
                rounds++;
            } while (!finish);
            int count = 0;
            _gridNodes.ElvesByList.ForEach(x => count += Math.Max(0, x.HP));
            _gridNodes.GoblinsByList.ForEach(x => count += Math.Max(0, x.HP));
            return count * rounds;
        }

        private bool PerformUnitAction() {
            var units = _gridNodes.ElvesByList.Union(_gridNodes.GoblinsByList).OrderBy(x => x.Y).ThenBy(x => x.X);
            foreach (var unit in units) {
                if (unit.HP > 0) {
                    var canMove = FindMovingTargets(unit, (_gridNodes.ByPosition[unit.X, unit.Y].GridNodeType == enumGridNodeType.Elf ? _gridNodes.GoblinsByList : _gridNodes.ElvesByList));
                    if (_noTargets) {
                        return true;
                    }
                    if (canMove) {
                        MoveUnit(unit);
                    }
                    if (unit.TargetDistance <= 1) {
                        UnitAttack(unit);
                    }
                }
            }
            return false;
        }

        private bool UnitAttack(Unit unit) {
            _bestX = _maxValue;
            _bestY = _maxValue;
            _bestHp = _maxValue;
            _smallestHp = (unit.GridNodeType == enumGridNodeType.Goblin ? _gridNodes.ElvesByPosition : _gridNodes.GoblinsByPosition);
            if (_smallestHp[unit.X + 1, unit.Y] != null && _smallestHp[unit.X + 1, unit.Y].HP > 0) {
                IsBestAttack(unit.X + 1, unit.Y);
            }
            if (_smallestHp[unit.X - 1, unit.Y] != null && _smallestHp[unit.X - 1, unit.Y].HP > 0) {
                IsBestAttack(unit.X - 1, unit.Y);
            }
            if (_smallestHp[unit.X, unit.Y + 1] != null && _smallestHp[unit.X, unit.Y + 1].HP > 0) {
                IsBestAttack(unit.X, unit.Y + 1);
            }
            if (_smallestHp[unit.X, unit.Y - 1] != null && _smallestHp[unit.X, unit.Y - 1].HP > 0) {
                IsBestAttack(unit.X, unit.Y - 1);
            }
            _smallestHp[_bestX, _bestY].HP -= unit.AttackPower;
            if (_smallestHp[_bestX, _bestY].HP <= 0) {
                _gridNodes.ByPosition[_bestX, _bestY].GridNodeType = enumGridNodeType.Blank;
                return true;
            }
            return false;
        }
        
        private void MoveUnit(Unit unit) {
            _bestDistance = _maxValue;
            _bestX = _maxValue;
            _bestY = _maxValue;
            _shortest = FindShortestDistances(_gridNodes.ByPosition[unit.TargetX, unit.TargetY].Key);
            if (_gridNodes.ByPosition[unit.X + 1, unit.Y].GridNodeType == enumGridNodeType.Blank) {
                IsBestDistance(unit.X + 1, unit.Y);
            }
            if (_gridNodes.ByPosition[unit.X - 1, unit.Y].GridNodeType == enumGridNodeType.Blank) {
                IsBestDistance(unit.X - 1, unit.Y);
            }
            if (_gridNodes.ByPosition[unit.X, unit.Y + 1].GridNodeType == enumGridNodeType.Blank) {
                IsBestDistance(unit.X, unit.Y + 1);
            }
            if (_gridNodes.ByPosition[unit.X, unit.Y - 1].GridNodeType == enumGridNodeType.Blank) {
                IsBestDistance(unit.X, unit.Y - 1);
            }
            if (_gridNodes.ByPosition[unit.X, unit.Y].GridNodeType == enumGridNodeType.Elf) {
                _gridNodes.ElvesByPosition[_bestX, _bestY] = _gridNodes.ElvesByPosition[unit.X, unit.Y];
                _gridNodes.ElvesByPosition[unit.X, unit.Y] = null;
            } else {
                _gridNodes.GoblinsByPosition[_bestX, _bestY] = _gridNodes.GoblinsByPosition[unit.X, unit.Y];
                _gridNodes.GoblinsByPosition[unit.X, unit.Y] = null;
            }
            _gridNodes.ByPosition[_bestX, _bestY].GridNodeType = _gridNodes.ByPosition[unit.X, unit.Y].GridNodeType;
            _gridNodes.ByPosition[unit.X, unit.Y].GridNodeType = enumGridNodeType.Blank;
            unit.X = _bestX;
            unit.Y = _bestY;
        }
        
        private bool FindMovingTargets(Unit current, List<Unit> targets) {
            _noTargets = true;
            bool canMove = false;
            _bestX = _maxValue;
            _bestY = _maxValue;
            _bestDistance = _maxValue;
            _shortest = FindShortestDistances(_gridNodes.ByPosition[current.X, current.Y].Key);
            foreach (var target in targets) {
                if (target.HP > 0) {
                    _noTargets = false;
                    if (IsTargetInRange(current, target)) {
                        return false;
                    }
                    if (_gridNodes.ByPosition[target.X + 1, target.Y].GridNodeType == enumGridNodeType.Blank && _shortest[target.X + 1, target.Y] != _maxValue) {
                        canMove = true;
                        IsBestDistance(target.X + 1, target.Y);
                    }
                    if (_gridNodes.ByPosition[target.X - 1, target.Y].GridNodeType == enumGridNodeType.Blank && _shortest[target.X - 1, target.Y] != _maxValue) {
                        canMove = true;
                        IsBestDistance(target.X - 1, target.Y);
                    }
                    if (_gridNodes.ByPosition[target.X, target.Y + 1].GridNodeType == enumGridNodeType.Blank && _shortest[target.X, target.Y + 1] != _maxValue) {
                        canMove = true;
                        IsBestDistance(target.X, target.Y + 1);
                    }
                    if (_gridNodes.ByPosition[target.X, target.Y - 1].GridNodeType == enumGridNodeType.Blank && _shortest[target.X, target.Y - 1] != _maxValue) {
                        canMove = true;
                        IsBestDistance(target.X, target.Y - 1);
                    }
                }
            }
            current.TargetX = _bestX;
            current.TargetY = _bestY;
            current.TargetDistance = _bestDistance;
            return canMove;
        }

        private bool IsTargetInRange(Unit current, Unit target) {
            if (target.X == current.X) {
                var distance = Math.Abs(current.Y - target.Y);
                if (distance <= 1) {
                    current.TargetDistance = distance;
                    return true;
                }
            }
            if (target.Y == current.Y) {
                var distance = Math.Abs(current.X - target.X);
                if (distance <= 1) {
                    current.TargetDistance = distance;
                    return true;
                }
            }
            return false;
        }

        private int _bestX;
        private int _bestY;
        private int _bestDistance;
        private int[,] _shortest;
        private void IsBestDistance(int x, int y) {
            var distance = _shortest[x, y];
            if (distance < _bestDistance) {
                _bestDistance = distance;
                _bestX = x;
                _bestY = y;
            } else if (distance == _bestDistance) {
                if (y < _bestY) {
                    _bestX = x;
                    _bestY = y;
                } else if (y == _bestY && x < _bestX) {
                    _bestX = x;
                }
            }
        }

        private int _bestHp;
        private Unit[,] _smallestHp;
        private void IsBestAttack(int x, int y) {
            var hp = _smallestHp[x, y].HP;
            if (hp < _bestHp) {
                _bestHp = hp;
                _bestX = x;
                _bestY = y;
            } else if (hp == _bestHp) {
                if (y < _bestY) {
                    _bestX = x;
                    _bestY = y;
                } else if (y == _bestY && x < _bestX) {
                    _bestX = x;
                }
            }
        }

        private int[,] FindShortestDistances(Tuple<int, int> start) {
            var distanceNodes = GetDistanceNodes(start);
            var unvisited = new List<DistanceNode>();
            distanceNodes[start.Item1, start.Item2].Distance = 0;
            for (int x = 0; x <= distanceNodes.GetUpperBound(0); x++) {
                for (int y = 0; y <= distanceNodes.GetUpperBound(1); y++) {
                    if (distanceNodes[x, y] != null) {
                        unvisited.Add(distanceNodes[x, y]);
                    }
                }
            }
            FindShortestDistances(distanceNodes, unvisited);
            var shortest = new int[distanceNodes.GetUpperBound(0) + 1, distanceNodes.GetUpperBound(1) + 1];
            for (int x = 0; x <= distanceNodes.GetUpperBound(0); x++) {
                for (int y = 0; y <= distanceNodes.GetUpperBound(1); y++) {
                    if (distanceNodes[x, y] != null) {
                        shortest[x, y] = distanceNodes[x, y].Distance;
                    }
                }
            }
            return shortest;
        }

        private void FindShortestDistances(DistanceNode[,] distanceNodes, List<DistanceNode> unvisited) {
            while (unvisited.Count > 0) {
                var current = GetShortest(unvisited);
                if (!current.VisitedFromDown) {
                    if (distanceNodes[current.X, current.Y + 1] != null) {
                        if (_gridNodes.ByPosition[current.X, current.Y + 1].GridNodeType == enumGridNodeType.Blank) {
                            distanceNodes[current.X, current.Y + 1].Distance = Math.Min(distanceNodes[current.X, current.Y + 1].Distance, current.Distance + 1);
                        }
                        distanceNodes[current.X, current.Y + 1].VisitedFromUp = true;
                    }
                    current.VisitedFromDown = true;
                }
                if (!current.VisitedFromUp) {
                    if (distanceNodes[current.X, current.Y - 1] != null) {
                        if (_gridNodes.ByPosition[current.X, current.Y - 1].GridNodeType == enumGridNodeType.Blank) {
                            distanceNodes[current.X, current.Y - 1].Distance = Math.Min(distanceNodes[current.X, current.Y - 1].Distance, current.Distance + 1);
                        }
                        distanceNodes[current.X, current.Y - 1].VisitedFromDown = true;
                    }
                    current.VisitedFromUp = true;
                }
                if (!current.VisitedFromLeft) {
                    if (distanceNodes[current.X - 1, current.Y] != null) {
                        if (_gridNodes.ByPosition[current.X - 1, current.Y].GridNodeType == enumGridNodeType.Blank) {
                            distanceNodes[current.X - 1, current.Y].Distance = Math.Min(distanceNodes[current.X - 1, current.Y].Distance, current.Distance + 1);
                        }
                        distanceNodes[current.X - 1, current.Y].VisitedFromRight = true;
                    }
                    current.VisitedFromLeft = true;
                }
                if (!current.VisitedFromRight) {
                    if (distanceNodes[current.X + 1, current.Y] != null) {
                        if (_gridNodes.ByPosition[current.X + 1, current.Y].GridNodeType == enumGridNodeType.Blank) {
                            distanceNodes[current.X + 1, current.Y].Distance = Math.Min(distanceNodes[current.X + 1, current.Y].Distance, current.Distance + 1);
                        }
                        distanceNodes[current.X + 1, current.Y].VisitedFromLeft = true;
                    }
                    current.VisitedFromRight = true;
                }
                unvisited.Remove(current);
            }
        }

        private DistanceNode GetShortest(List<DistanceNode> unvisited) {
            DistanceNode shortest = unvisited[0];
            foreach (var next in unvisited) {
                if (next.Distance < shortest.Distance) {
                    shortest = next;
                }
            }
            return shortest;
        }

        private DistanceNode[,] GetDistanceNodes(Tuple<int, int> start) {
            var nodes = new DistanceNode[_gridNodes.ByPosition.GetUpperBound(0) + 1, _gridNodes.ByPosition.GetUpperBound(1) + 1];
            foreach (var gridNode in _gridNodes.ByList) {
                if (gridNode.GridNodeType != enumGridNodeType.Wall) {
                    var distanceNode = new DistanceNode() {
                        Distance = (gridNode.X == start.Item1 && gridNode.Y == start.Item2 ? 0 : _maxValue),
                        Key = gridNode.Key,
                        X = gridNode.X,
                        Y = gridNode.Y
                    };
                    if (gridNode.X == 0) {
                        distanceNode.VisitedFromLeft = true;
                    }
                    if (gridNode.X == _gridNodes.ByPosition.GetUpperBound(0)) {
                        distanceNode.VisitedFromRight = true;
                    }
                    if (gridNode.Y == 0) {
                        distanceNode.VisitedFromUp = true;
                    }
                    if (gridNode.Y == _gridNodes.ByPosition.GetUpperBound(1)) {
                        distanceNode.VisitedFromDown = true;
                    }
                    nodes[gridNode.X, gridNode.Y] = distanceNode;
                }
            }
            return nodes;
        }


        private GridNodeLists GetGridNodes(List<string> input, int elfAttackPower) {
            var grid = new GridNodeLists();
            grid.ByList = new List<GridNode>();
            grid.ByPosition = new GridNode[input[0].Length, input.Count];
            grid.ElvesByList = new List<Unit>();
            grid.GoblinsByList = new List<Unit>();
            grid.ElvesByPosition = new Unit[input[0].Length, input.Count];
            grid.GoblinsByPosition = new Unit[input[0].Length, input.Count];
            for (int y = 0; y < input.Count; y++) {
                var line = input[y];
                for (int x = 0; x < line.Length; x++) {
                    var node = new GridNode();
                    Unit unit = null;
                    switch (line[x]) {
                        case '#':
                            node.GridNodeType = enumGridNodeType.Wall;
                            break;
                        case 'E':
                            node.GridNodeType = enumGridNodeType.Elf;
                            unit = new Unit(x, y);
                            unit.GridNodeType = enumGridNodeType.Elf;
                            unit.AttackPower = elfAttackPower;
                            unit.HP = 200;
                            grid.ElvesByList.Add(unit);
                            grid.ElvesByPosition[x, y] = unit;
                            break;
                        case 'G':
                            node.GridNodeType = enumGridNodeType.Goblin;
                            unit = new Unit(x, y);
                            unit.GridNodeType = enumGridNodeType.Goblin;
                            unit.AttackPower = 3;
                            unit.HP = 200;
                            grid.GoblinsByList.Add(unit);
                            grid.GoblinsByPosition[x, y] = unit;
                            break;
                        case '.':
                            node.GridNodeType = enumGridNodeType.Blank;
                            break;
                    }
                    node.Key = new Tuple<int, int>(x, y);
                    node.X = x;
                    node.Y = y;
                    grid.ByPosition[x, y] = node;
                    grid.ByList.Add(node);
                }
            }
            return grid;
        }

        private string PrintGrid() {
            StringBuilder text = new StringBuilder();
            for (int y = 0; y <= _gridNodes.ByPosition.GetUpperBound(0); y++) {
                string line = "";
                for (int x = 0; x <= _gridNodes.ByPosition.GetUpperBound(1); x++) {
                    switch (_gridNodes.ByPosition[x, y].GridNodeType) {
                        case enumGridNodeType.Blank:
                            line += ".";
                            break;
                        case enumGridNodeType.Elf:
                            line += "E";
                            break;
                        case enumGridNodeType.Goblin:
                            line += "G";
                            break;
                        case enumGridNodeType.Wall:
                            line += "#";
                            break;
                    }
                }
                text.AppendLine(line);
            }
            return text.ToString();
        }

        private List<string> TestInput1() {
            return new List<string>() {
                "#######",
                "#E..G.#",
                "#...#.#",
                "#.G.#G#",
                "#######"
            };
        }

        private List<string> TestInput2() {
            return new List<string>() {
                "#########",
                "#G..G..G#",
                "#.......#",
                "#.......#",
                "#G..E..G#",
                "#.......#",
                "#.......#",
                "#G..G..G#",
                "#########"
            };
        }

        private List<string> TestInput3() {
            return new List<string>() {
                "#######",
                "#.G...#",
                "#...EG#",
                "#.#.#G#",
                "#..G#E#",
                "#.....#",
                "#######"
            };
        }

        private enum enumGridNodeType {
            Goblin,
            Elf,
            Wall,
            Blank
        }

        private class Unit {
            public Unit() { }
            public Unit(int x, int y) {
                X = x;
                Y = y;
            }
            public int X { get; set; }
            public int Y { get; set; }
            public int TargetX { get; set; }
            public int TargetY { get; set; }
            public int TargetDistance { get; set; }
            public int HP { get; set; }
            public enumGridNodeType GridNodeType { get; set; }
            public int AttackPower { get; set; }
        }

        private class GridNode {
            public int X { get; set; }
            public int Y { get; set; }
            public Tuple<int, int> Key { get; set; }
            public enumGridNodeType GridNodeType { get; set; }
        }

        private class DistanceNode {
            public int X { get; set; }
            public int Y { get; set; }
            public Tuple<int, int> Key { get; set; }
            public int Distance { get; set; }

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

        private class GridNodeLists {
            public GridNode[,] ByPosition { get; set; }
            public List<GridNode> ByList { get; set; }
            public List<Unit> ElvesByList { get; set; }
            public List<Unit> GoblinsByList { get; set; }
            public Unit[,] ElvesByPosition { get; set; }
            public Unit[,] GoblinsByPosition { get; set; }
        }
    }
}
