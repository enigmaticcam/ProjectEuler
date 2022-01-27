using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Euler_Logic.Problems.AdventOfCode.Y2019 {
    public class Problem13 : AdventOfCodeBase {
        public override string ProblemName {
            get { return "Advent of Code 2019: 13"; }
        }

        public override string GetAnswer() {
            _tiles = new Dictionary<long, Dictionary<long, enumTile>>();
            return Answer1(Input()).ToString();
        }

        public override string GetAnswer2() {
            _tiles = new Dictionary<long, Dictionary<long, enumTile>>();
            return Answer2(Input()).ToString();
        }

        private int Answer1(List<string> input) {
            var computer = new IntComputer();
            computer.Run(input, () => 0, () => HandleOutput(computer));
            return _tiles.Select(x => x.Value.Where(y => y.Value == enumTile.Block).Count()).Sum();
        }

        private long Answer2(List<string> input) {
            var over = new IntComputer.LineOverride() { Index = 0, Value = 2 };
            var computer = new IntComputer();
            computer.Run(input, () => HandleInput(), () => HandleOutput(computer), new List<IntComputer.LineOverride>() { over });
            return _score;
        }

        private string PrintOutput() {
            var minX = _tiles.Keys.Min();
            var maxX = _tiles.Keys.Max();
            var minY = _tiles.Select(x => x.Value.Keys.Min()).Min();
            var maxY = _tiles.Select(x => x.Value.Keys.Max()).Max();
            var text = new StringBuilder();
            for (var y = minY; y <= maxY; y++) {
                var line = new char[maxX - minX + 1];
                int index = 0;
                for (var x = minX; x <= maxX; x++) {
                    switch (_tiles[x][y]) {
                        case enumTile.Ball:
                            line[index] = 'B';
                            break;
                        case enumTile.Block:
                            line[index] = '#';
                            break;
                        case enumTile.Empty:
                            line[index] = '.';
                            break;
                        case enumTile.PaddleH:
                            line[index] = '-';
                            break;
                        case enumTile.Wall:
                            line[index] = '|';
                            break;
                    }
                    index++;
                }
                text.AppendLine(new string(line));
            }
            return text.ToString();
        }

        private int _count = 0;
        private long _score = 0;
        private long _ballX = 0;
        private long _paddleX = 0;
        private void HandleOutput(IntComputer computer) {
            _count++;
            if (_count == 3) {
                var x = computer.Output[computer.Output.Count - 3];
                var y = computer.Output[computer.Output.Count - 2];
                if (x == -1 && y == 0) {
                    _score = computer.Output[computer.Output.Count - 1];
                } else {
                    DrawTile(x, y, (enumTile)computer.Output[computer.Output.Count - 1]);
                }
                _count = 0;
            }
        }

        private void DrawTile(long x, long y, enumTile tile) {
            if (!_tiles.ContainsKey(x)) {
                _tiles.Add(x, new Dictionary<long, enumTile>());
            }
            if (!_tiles[x].ContainsKey(y)) {
                _tiles[x].Add(y, tile);
            } else {
                _tiles[x][y] = tile;
            }
            if (tile == enumTile.Ball) {
                _ballX = x;
            } else if (tile == enumTile.PaddleH) {
                _paddleX = x;
            }
        }

        private int HandleInput() {
            if (_ballX > _paddleX) {
                return 1;
            } else if (_ballX < _paddleX) {
                return -1;
            } else {
                return 0;
            }
        }

        private Dictionary<long, Dictionary<long, enumTile>> _tiles;

        private enum enumTile {
            Empty,
            Wall,
            Block,
            PaddleH,
            Ball
        }

        
    }
}
