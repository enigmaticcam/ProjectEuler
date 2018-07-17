using System;
using System.Collections.Generic;

namespace Euler_Logic.Problems {
    public class Problem323 : ProblemBase {
        private Random _random = new Random();
        private Dictionary<double, Dictionary<double, double>> _combinations = new Dictionary<double, Dictionary<double, double>>();
        private Dictionary<double, double> _factorials = new Dictionary<double, double>();
        private Dictionary<int, Dictionary<int, double>> _increments = new Dictionary<int, Dictionary<int, double>>();
        private Dictionary<int, double> _expectedValues = new Dictionary<int, double>();

        public override string ProblemName {
            get { return "323: Bitwise-OR operations on random integers"; }
        }

        public override string GetAnswer() {
            //return Test(16).ToString();
            int maxDigits = 33;
            CalcIncrements(maxDigits);
            CalcExpectedValues(maxDigits);
            return _expectedValues[maxDigits - 1].ToString();
        }

        private void CalcIncrements(int maxDigits) {
            double max = Math.Pow(2, maxDigits);
            for (int maxZeroes = 1; maxZeroes <= maxDigits; maxZeroes++) {
                _increments.Add(maxZeroes, new Dictionary<int, double>());
                for (int zeroCount = 0; zeroCount <= maxZeroes; zeroCount++) {
                    _increments[maxZeroes].Add(zeroCount, DistinctCombinationsOfSetInSet(maxZeroes, zeroCount) * Math.Pow(2, maxDigits - maxZeroes) / max);
                }
            }
        }

        public void CalcExpectedValues(int maxDigits) {
            _expectedValues.Add(0, 1);
            for (int maxZeroes = 1; maxZeroes < maxDigits; maxZeroes++) {
                double value = _increments[maxZeroes][0];
                for (int zeroCount = 1; zeroCount < maxZeroes; zeroCount++) {
                    value += ((1 + _expectedValues[zeroCount]) * _increments[maxZeroes][zeroCount]);
                }
                value += _increments[maxZeroes][maxZeroes];
                value = value / (1 - _increments[maxZeroes][maxZeroes]);
                _expectedValues.Add(maxZeroes, value);
            }
        }

        private double DistinctCombinationsOfSetInSet(double n, double r) {
            if (n == r || r == 0) {
                return 1;
            } else {
                return Factorial(n) / (Factorial(r) * Factorial(n - r));
            }
        }

        private double Factorial(double max) {
            double sum = max;
            for (double num = max - 1; num >= 1; num--) {
                sum *= num;
            }
            return sum;
        }

        private double Test(int maxDigits) {
            double sum = 0;
            double maxCount = 10000000;
            int maxValue = (int)Math.Pow(2, maxDigits) - 1;
            for (int index = 1; index <= maxCount; index++) {
                double count = 0;
                int last = 0;
                do {
                    count++;
                    int number = _random.Next(0, maxValue + 1);
                    last = number | last;
                    if (last == maxValue) {
                        sum += count;
                        break;
                    }
                } while (true);
            }
            return sum / maxCount;
        }
    }
}