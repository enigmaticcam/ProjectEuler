using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem493 : IProblem {
        private int _colors;
        private int _colorCount;
        private int _pullCount;
        private double _sum;
        private double _count;
        private Dictionary<double, Dictionary<double, double>> _combinations = new Dictionary<double, Dictionary<double, double>>();
        private Dictionary<double, double> _factorials = new Dictionary<double, double>();
        private List<int> _colorCounts = new List<int>();

        public string ProblemName {
            get { return "493: Under The Rainbow"; }
        }

        public string GetAnswer() {
            SetProblemParameters();
            BuildColorCounts();
            Solve(_colors * _colorCount, _colors, _pullCount);
            return (_sum / _count).ToString();
        }

        private void SetTestParameters() {
            _colors = 3;
            _colorCount = 3;
            _pullCount = 2;
        }

        private void SetProblemParameters() {
            _colors = 7;
            _colorCount = 10;
            _pullCount = 20;
        }

        private void BuildColorCounts() {
            for (int count = 0; count < _colors; count++) {
                _colorCounts.Add(0);
            }
        }

        private double Factorial(double max) {
            if (!_factorials.ContainsKey(max)) {
                double sum = max;
                for (double num = max - 1; num >= 1; num--) {
                    sum *= num;
                }
                _factorials.Add(max, sum);
            }
            return _factorials[max];
        }

        private double DistinctCombinationsOfSetInSet(double n, double r) {
            if (!_combinations.ContainsKey(n)) {
                _combinations.Add(n, new Dictionary<double, double>());
            }
            if (!_combinations[n].ContainsKey(r)) {
                if (n == r) {
                    _combinations[n].Add(r, 1);
                } else {
                    _combinations[n].Add(r, Factorial(n) / (Factorial(r) * Factorial(n - r)));
                }
            }
            return _combinations[n][r];
        }

        private void Solve(int totalRemainingBalls, int colorsLeft, int ballsLeftToPull) {
            int max = _colorCount;
            if (colorsLeft == 1) {
                max = ballsLeftToPull;
            }
            for (int ball = max; ball >= 0; ball--) {
                if (ballsLeftToPull - ball > (colorsLeft - 1) * _colorCount) {
                    break;
                }
                _colorCounts[colorsLeft - 1] += ball;
                ballsLeftToPull -= ball;
                colorsLeft -= 1;
                if (ballsLeftToPull == 0 || colorsLeft == 0) {
                    double sub = 1;
                    double colorCount = 0;
                    for (int color = 0; color < _colorCounts.Count; color++) {
                        if (_colorCounts[color] > 0) {
                            sub *= DistinctCombinationsOfSetInSet(_colorCount, _colorCounts[color]);
                            colorCount += 1;
                        }
                    }
                    sub = Factorial(_pullCount) * sub;
                    _sum += colorCount * sub;
                    _count += sub;
                } else {
                    Solve(totalRemainingBalls - ball, colorsLeft, ballsLeftToPull);
                }
                colorsLeft += 1;
                _colorCounts[colorsLeft - 1] -= ball;
                ballsLeftToPull += ball;
            }
        }
    }
}
