using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems.AdventOfCode.Y2017 {
    public class Problem15 : AdventOfCodeBase {
        private ulong[] _generators;

        public override string ProblemName {
            get { return "Advent of Code 2017: 15"; }
        }

        public override string GetAnswer() {
            return Answer2(Input(), 5000000).ToString();
        }

        private int Answer1(List<string> input, int maxCount) {
            GetGenerators(input);
            return CountDiffA(maxCount);
        }

        private int Answer2(List<string> input, int maxCount) {
            GetGenerators(input);
            return CountDiffB(maxCount);
        }

        private int CountDiffB(int maxCount) {
            ulong factorA = 16807;
            ulong factorB = 48271;
            int sum = 0;
            int count = 0;
            var prior = new List<ulong>[2];
            prior[0] = new List<ulong>();
            prior[1] = new List<ulong>();
            int pairIndex = 0;
            do {
                _generators[0] = (_generators[0] * factorA) % 2147483647;
                _generators[1] = (_generators[1] * factorB) % 2147483647;
                if (_generators[0] % 4 == 0) {
                    prior[0].Add(_generators[0]);
                }
                if (_generators[1] % 8 == 0) {
                    prior[1].Add(_generators[1]);
                }
                while (count < maxCount && pairIndex < prior[0].Count && pairIndex < prior[1].Count) {
                    if ((prior[0][pairIndex] & 65535) == (prior[1][pairIndex] & 65535)) {
                        sum++;
                    }
                    count++;
                    pairIndex++;
                }
            } while (count < maxCount);
            return sum;
        }

        private int CountDiffA(int maxCount) {
            ulong factorA = 16807;
            ulong factorB = 48271;
            int sum = 0;
            for (int count = 1; count <= maxCount; count++) {
                _generators[0] = (_generators[0] * factorA) % 2147483647;
                _generators[1] = (_generators[1] * factorB) % 2147483647;
                if ((_generators[0] & 65535) == (_generators[1] & 65535)) {
                    sum++;
                }
            }
            return sum;
        }

        private void GetGenerators(List<string> input) {
            _generators = new ulong[2];
            _generators[0] = Convert.ToUInt64(input[0].Split(' ')[4]);
            _generators[1] = Convert.ToUInt64(input[1].Split(' ')[4]);
        }

        private List<string> TestInput() {
            return new List<string>() {
                "Generator A starts with 65",
                "Generator B starts with 8921"
            };
        }
    }
}
