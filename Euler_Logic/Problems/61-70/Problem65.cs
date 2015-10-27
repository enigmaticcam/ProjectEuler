using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem65 : IProblem {
        private List<IPattern> _patterns = new List<IPattern>();

        public string ProblemName {
            get { return "65: Convergents of e"; }
        }

        public string GetAnswer() {
            LoadPatterns();
            return FindDigitSum(100).ToString();
        }

        private int FindDigitSum(int max) {
            List<BigInteger> fractions = new List<BigInteger>();
            int count = 1;
            do {
                foreach (IPattern pattern in _patterns) {
                    fractions.Add(pattern.Number);
                    BigInteger denominator = fractions[fractions.Count - 1];
                    BigInteger numerator = 1;
                    BigInteger temp = 0;
                    for (int index = fractions.Count - 2; index >= 0; index--) {
                        numerator += fractions[index] * denominator;
                        temp = numerator;
                        numerator = denominator;
                        denominator = temp;
                    }
                    numerator += denominator * 2;
                    count++;
                    if (count == max) {
                        return DigitCount(numerator);
                    }
                    pattern.NextNumber();
                }
            } while (true);
        }

        private int DigitCount(BigInteger num) {
            string text = num.ToString();
            int sum = 0;
            for (int index = 0; index < text.Length; index++) {
                sum += Convert.ToInt32(text.Substring(index, 1));
            }
            return sum;
        }

        private void LoadPatterns() {
            _patterns.Add(new Pattern1());
            _patterns.Add(new PatternIncrement());
            _patterns.Add(new Pattern1());
        }

        private interface IPattern {
            BigInteger Number { get; set; }
            void NextNumber();
        }

        private class Pattern1 : IPattern {
            public BigInteger Number { get; set; }
            public void NextNumber() {

            }

            public Pattern1() {
                this.Number = 1;
            }
        }

        private class PatternIncrement : IPattern {
            public BigInteger Number { get; set; }
            public void NextNumber() {
                this.Number += 2;
            }

            public PatternIncrement() {
                this.Number = 2;
            }
        }
    }
}
