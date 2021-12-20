using System.Collections.Generic;
using System.Text;

namespace Euler_Logic.Problems.AdventOfCode.Y2021 {
    public class Problem20 : AdventOfCodeBase {
        private Dictionary<ulong, bool> _algorithm;
        private bool[,] _inputImage;
        private int _size;

        public override string ProblemName {
            get { return "Advent of Code 2021: 20"; }
        }

        public override string GetAnswer() {
            GetInputs(Input());
            return Answer2().ToString();
        }

        private int Answer1() {
            int last = 0;
            Enhance(6);
            last = Enhance(-2);
            return last;
        }

        private int Answer2() {
            int last = 0;
            for (int count = 1; count <= 25; count++) {
                Enhance(4);
                last = Enhance(-2);
            }
            return last;
        }

        private int Enhance(int offset) {
            int count = 0;
            var newImage = new bool[_size + offset * 2, _size + offset * 2];
            for (int x = 0 - offset; x < _size + offset; x++) {
                for (int y = 0 - offset; y < _size + offset; y++) {
                    var isLit = GetAlgorithmIndex(x, y);
                    newImage[x + offset, y + offset] = isLit;
                    if (isLit) count++;
                }
            }
            _inputImage = newImage;
            _size += offset * 2;
            return count;
        }

        private bool GetAlgorithmIndex(int startX, int startY) {
            ulong powerOf2 = 1;
            ulong index = 0;
            for (int y = startY + 1; y >= startY - 1; y--) {
                for (int x = startX + 1; x >= startX - 1; x--) {
                    if (x >= 0 && x < _size && y >= 0 && y < _size) {
                        index += (_inputImage[x, y] ? powerOf2 : 0);
                    }
                    powerOf2 *= 2;
                }
            }
            return _algorithm[index];
        }

        private void GetInputs(List<string> input) {
            _algorithm = new Dictionary<ulong, bool>();
            for (int index = 0; index < input[0].Length; index++) { 
                var digit = input[0][index];
                _algorithm.Add((ulong)index, digit == '.' ? false : true);
            }

            var size = input[2].Length;
            _inputImage = new bool[size, size];
            for (int y = 0; y < size; y++) {
                for (int x = 0; x < size; x++) {
                    _inputImage[x, y] = input[y + 2][x] == '#' ? true : false;
                }
            }
            _size = _inputImage.GetUpperBound(0) + 1;
        }

        private string Output() {
            var text = new StringBuilder();
            for (int y = 0; y < _size; y++) {
                for (int x = 0; x < _size; x++) {
                    text.Append(_inputImage[x, y] ? "#" : ".");
                }
                text.AppendLine();
            }
            return text.ToString();
        }
    }
}
