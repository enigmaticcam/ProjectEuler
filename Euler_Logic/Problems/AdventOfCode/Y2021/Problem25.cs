using System.Collections.Generic;
using System.Text;

namespace Euler_Logic.Problems.AdventOfCode.Y2021 {
    public class Problem25 : AdventOfCodeBase {
        private Creature[,] _grid;
        private List<Creature> _creaturesEast;
        private List<Creature> _creaturesSouth;

        public override string ProblemName {
            get { return "Advent of Code 2021: 25"; }
        }

        public override string GetAnswer() {
            GetGrid(Input());
            return Answer1().ToString();
        }

        private int Answer1() {
            int steps = 0;
            do {
                steps++;
            } while (Simulate(steps));
            return steps;
        }

        private bool Simulate(int step) {
            var nextGrid = new Creature[_grid.GetUpperBound(0) + 1, _grid.GetUpperBound(1) + 1];
            bool didMove = false;
            foreach (var creature in _creaturesEast) {
                int next = (creature.X + 1) % (_grid.GetUpperBound(0) + 1);
                if (_grid[next, creature.Y] == null) {
                    _grid[next, creature.Y] = creature;
                    creature.X = next;
                    didMove = true;
                    creature.LastStep = step;
                }
                nextGrid[creature.X, creature.Y] = creature;
            }
            foreach (var creature in _creaturesSouth) {
                int next = (creature.Y + 1) % (_grid.GetUpperBound(1) + 1);
                bool canMove = false;
                if (_grid[creature.X, next] == null && nextGrid[creature.X, next] == null) {
                    canMove = true;
                } else if (nextGrid[creature.X, next] == null) {
                    canMove = _grid[creature.X, next].IsEast && _grid[creature.X, next].LastStep == step;
                }
                if (canMove) {
                    _grid[creature.X, next] = creature;
                    creature.Y = next;
                    didMove = true;
                    creature.LastStep = step;
                }
                nextGrid[creature.X, creature.Y] = creature;
            }
            _grid = nextGrid;
            return didMove;
        }

        private void GetGrid(List<string> input) {
            _grid = new Creature[input[0].Length, input.Count];
            _creaturesEast = new List<Creature>();
            _creaturesSouth = new List<Creature>();
            for (int y = 0; y < input.Count; y++) {
                for (int x = 0; x < input[y].Length; x++) {
                    var digit = input[y][x];
                    if (digit != '.') {
                        var creature = new Creature() {
                            X = x,
                            Y = y
                        };
                        _grid[x, y] = creature;
                        if (digit == 'v') {
                            _creaturesSouth.Add(creature);
                        } else {
                            _creaturesEast.Add(creature);
                            creature.IsEast = true;
                        }
                    }
                }
            }
        }

        private string Output() {
            var text = new StringBuilder();
            for (int y = 0; y <= _grid.GetUpperBound(1); y++) {
                for (int x = 0; x <= _grid.GetUpperBound(0); x++) {
                    if (_grid[x, y] != null) {
                        if (_grid[x, y].IsEast) {
                            text.Append(">");
                        } else {
                            text.Append("v");
                        }
                    } else {
                        text.Append(".");
                    }
                }
                text.AppendLine();
            }
            return text.ToString();
        }

        private class Creature {
            public int X { get; set; }
            public int Y { get; set; }
            public bool IsEast { get; set; }
            public int LastStep { get; set; }
        }
    }
}
