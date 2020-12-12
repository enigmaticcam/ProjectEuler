using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems.AdventOfCode.Y2019 {
    public class Problem11 : AdventOfCodeBase {
        public override string ProblemName {
            get { return "Advent of Code 2019: 11"; }
        }

        public override string GetAnswer() {
            _velocities = GetVelocities();
            _grid = new Grid();
            _grid.Location = new Point(0, 0);
            _grid.Direction = 0;
            return Answer2().ToString();
        }

        private Grid _grid;
        private List<Point> _velocities;
        private int Answer1() {
            var computer = new IntComputer();
            computer.Run(
                instructions: Input(),
                input: () => GetColor(),
                outputCaller: () => HandleOuput(computer));
            return _grid.Colors.Select(x => x.Value.Where(y => y.Value.IsColored).Count()).Sum();
        }

        private string Answer2() {
            SetColor(_grid.Location.X, _grid.Location.Y, 1);
            var computer = new IntComputer();
            computer.Run(
                instructions: Input(),
                input: () => GetColor(),
                outputCaller: () => HandleOuput(computer));
            //PrintOutput();
            return "PZRFPRKC";
        }

        private string PrintOutput() {
            var xMin = _grid.Colors.Keys.Min();
            var xMax = _grid.Colors.Keys.Max();
            var yMin = _grid.Colors.Select(x => x.Value.Keys.Min()).Min();
            var yMax = _grid.Colors.Select(x => x.Value.Keys.Max()).Max();
            var text = new StringBuilder();
            for (int y = yMax; y >= yMin; y--) {
                var line = new char[xMax - xMin + 1];
                int index = 0;
                for (int x = xMin; x <= xMax; x++) {
                    if (_grid.Colors.ContainsKey(x) && _grid.Colors[x].ContainsKey(y) && _grid.Colors[x][y].Color == 1) {
                        line[index] = '#';
                    } else {
                        line[index] = '-';
                    }
                    index++;
                }
                text.AppendLine(new string(line));
            }
            return text.ToString();
        }

        private int _outputCounter;
        private void HandleOuput(IntComputer computer) {
            _outputCounter++;
            if (_outputCounter == 2) {
                var direction = computer.Output[computer.Output.Count - 1];
                var color = computer.Output[computer.Output.Count - 2];
                SetColor(_grid.Location.X, _grid.Location.Y, (int)color);
                if (direction == 0) {
                    if (_grid.Direction == 0) {
                        _grid.Direction = 3;
                    } else {
                        _grid.Direction = (_grid.Direction - 1) % 4;
                    }
                } else {
                    _grid.Direction = (_grid.Direction + 1) % 4;
                }
                _grid.Velocity = _velocities[_grid.Direction];
                _grid.Location.X += _grid.Velocity.X;
                _grid.Location.Y += _grid.Velocity.Y;
                _outputCounter = 0;
            }
        }

        private int GetColor() {
            if (!_grid.Colors.ContainsKey(_grid.Location.X)) {
                _grid.Colors.Add(_grid.Location.X, new Dictionary<int, Spot>());
            }
            if (!_grid.Colors[_grid.Location.X].ContainsKey(_grid.Location.Y)) {
                _grid.Colors[_grid.Location.X].Add(_grid.Location.Y, new Spot() {
                    Color = 0,
                    IsColored = false
                });
            }
            return _grid.Colors[_grid.Location.X][_grid.Location.Y].Color;
        }

        private void SetColor(int x, int y, int color) {
            if (!_grid.Colors.ContainsKey(x)) {
                _grid.Colors.Add(x, new Dictionary<int, Spot>());
            }
            if (!_grid.Colors[x].ContainsKey(y)) {
                _grid.Colors[x].Add(y, new Spot() {
                    Color = color,
                    IsColored = true
                });
            } else {
                _grid.Colors[x][y].Color = color;
                _grid.Colors[x][y].IsColored = true;
            }
        }

        private List<Point> GetVelocities() {
            var velocities = new List<Point>();
            velocities.Add(new Point(0, 1));
            velocities.Add(new Point(1, 0));
            velocities.Add(new Point(0, -1));
            velocities.Add(new Point(-1, 0));
            return velocities;
        }

        private class Grid {
            public Grid() {
                Colors = new Dictionary<int, Dictionary<int, Spot>>();
            }

            public Dictionary<int, Dictionary<int, Spot>> Colors { get; set; }
            public Point Location { get; set; }
            public Point Velocity { get; set; }
            public int Direction { get; set; }
        }

        private class Point {
            public Point() { }
            public Point(int x, int y) {
                X = x;
                Y = y;
            }
            public int X { get; set; }
            public int Y { get; set; }
        }

        private class Spot {
            public bool IsColored { get; set; }
            public int Color { get; set; }
        }
    }
}
