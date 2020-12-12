using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems.AdventOfCode.Y2018 {
    public class Problem14 : AdventOfCodeBase {
        public override string ProblemName {
            get { return "Advent of Code 2018: 14"; }
        }

        public override string GetAnswer() {
            return Answer2();
        }

        private string Answer1() {
            Initialize();
            return PerformRounds(760221);
        }

        private string Answer2() {
            Initialize();
            return LookFor(760221);
        }

        private List<byte> _scores;
        private int[] _elfs;
        private void Initialize() {
            _scores = new List<byte>();
            _elfs = new int[2];
            _elfs[0] = 0;
            _elfs[1] = 1;
            _scores.Add(3);
            _scores.Add(7);
        }

        private string PerformRounds(int count) {
            do {
                NextRound();
            } while (_scores.Count + 1 <= count + 10);
            string result = "";
            for (int index = count; index <= count + 9; index++) {
                result += _scores[index].ToString();
            }
            return result;
        }

        private string LookFor(int num) {
            var lookFor = GetDigits(num);
            do {
                var next = NextRound();
                if (next > 9) {
                    if (IsGood(lookFor, 1)) {
                        return (_scores.Count - lookFor.Length - 1).ToString();
                    }
                }
                if (IsGood(lookFor, 0)) {
                    return (_scores.Count - lookFor.Length).ToString();
                }
            } while (true);
        }

        private bool IsGood(int[] lookFor, int offset) {
            if (lookFor.Length > _scores.Count) {
                return false;
            }
            for (int index = 0; index < lookFor.Length; index++) {
                if (lookFor[lookFor.Length - index - 1] != _scores[_scores.Count - index - 1 - offset]) {
                    return false;
                }
            }
            return true;
        }

        private int[] GetDigits(int num) {
            var max = (int)Math.Log10(num) + 1;
            var digits = new int[max];
            for (int index = max - 1; index >= 0; index--) {
                digits[index] = num % 10;
                num /= 10;
            }
            return digits;
        }

        private int NextRound() {
            var nextScore = _scores[_elfs[0]] + _scores[_elfs[1]];
            AddScore(nextScore);
            _elfs[0] = (_elfs[0] + _scores[_elfs[0]] + 1) % _scores.Count;
            _elfs[1] = (_elfs[1] + _scores[_elfs[1]] + 1) % _scores.Count;
            return nextScore;
        }

        private void AddScore(int num) {
            if (num > 9) {
                _scores.Add(1);
                num -= 10;
            }
            _scores.Add((byte)num);
        }
    }
}
