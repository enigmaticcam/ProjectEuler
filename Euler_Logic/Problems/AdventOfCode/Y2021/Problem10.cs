using System.Collections.Generic;
using System.Linq;

namespace Euler_Logic.Problems.AdventOfCode.Y2021 {
    public class Problem10 : AdventOfCodeBase {
        private Dictionary<char, int> _badPoints;
        private Dictionary<char, ulong> _goodPoints;

        public override string ProblemName {
            get { return "Advent of Code 2021: 10"; }
        }

        public override string GetAnswer() {
            SetPoints();
            return Answer2(Input()).ToString();
        }

        private int Answer1(List<string> input) {
            int points = 0;
            foreach (var line in input) {
                Chunk chunk = null;
                foreach (var digit in line) {
                    if (digit == '(') {
                        var nextChunk = new Chunk() { Open = digit, Close = ')' };
                        nextChunk.Parent = chunk;
                        chunk = nextChunk;
                    } else if (digit == '[') {
                        var nextChunk = new Chunk() { Open = digit, Close = ']' };
                        nextChunk.Parent = chunk;
                        chunk = nextChunk;
                    } else if (digit == '{') {
                        var nextChunk = new Chunk() { Open = digit, Close = '}' };
                        nextChunk.Parent = chunk;
                        chunk = nextChunk;
                    } else if (digit == '<') {
                        var nextChunk = new Chunk() { Open = digit, Close = '>' };
                        nextChunk.Parent = chunk;
                        chunk = nextChunk;
                    } else if (chunk.Close != digit) {
                        points += _badPoints[digit];
                        break;
                    } else {
                        chunk = chunk.Parent;
                    }
                }
            }
            return points;
        }

        private List<ulong> _goodScores;
        private ulong Answer2(List<string> input) {
            _goodScores = new List<ulong>();
            foreach (var line in input) {
                Chunk chunk = null;
                bool isGood = true;
                foreach (var digit in line) {
                    if (digit == '(') {
                        var nextChunk = new Chunk() { Open = digit, Close = ')' };
                        nextChunk.Parent = chunk;
                        chunk = nextChunk;
                    } else if (digit == '[') {
                        var nextChunk = new Chunk() { Open = digit, Close = ']' };
                        nextChunk.Parent = chunk;
                        chunk = nextChunk;
                    } else if (digit == '{') {
                        var nextChunk = new Chunk() { Open = digit, Close = '}' };
                        nextChunk.Parent = chunk;
                        chunk = nextChunk;
                    } else if (digit == '<') {
                        var nextChunk = new Chunk() { Open = digit, Close = '>' };
                        nextChunk.Parent = chunk;
                        chunk = nextChunk;
                    } else if (chunk.Close != digit) {
                        isGood = false;
                        break;
                    } else {
                        chunk = chunk.Parent;
                    }
                }
                if (isGood) {
                    _goodScores.Add(GetGoodScore(chunk));
                }
            }
            return GetFinalGoodScore();
        }

        private ulong GetFinalGoodScore() {
            var scores = _goodScores.OrderBy(x => x).ToList();
            return scores[scores.Count / 2];
        }

        private ulong GetGoodScore(Chunk chunk) {
            ulong points = 0;
            while (chunk != null) {
                points *= 5;
                points += _goodPoints[chunk.Close];
                chunk = chunk.Parent;
            }
            return points;
        }

        private void SetPoints() {
            _badPoints = new Dictionary<char, int>();
            _badPoints.Add(')', 3);
            _badPoints.Add(']', 57);
            _badPoints.Add('}', 1197);
            _badPoints.Add('>', 25137);

            _goodPoints = new Dictionary<char, ulong>();
            _goodPoints.Add(')', 1);
            _goodPoints.Add(']', 2);
            _goodPoints.Add('}', 3);
            _goodPoints.Add('>', 4);
        }

        private class Chunk {
            public char Open { get; set; }
            public char Close { get; set; }
            public Chunk Parent { get; set; }
        }
    }
}
