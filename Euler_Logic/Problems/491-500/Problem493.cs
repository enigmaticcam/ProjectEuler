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
        private double _factorial;
        private Dictionary<double, Dictionary<double, double>> _combinations = new Dictionary<double, Dictionary<double, double>>();
        private Dictionary<double, double> _factorials = new Dictionary<double, double>();

        public string ProblemName {
            get { return "493: Under The Rainbow"; }
        }

        public string GetAnswer() {
            SetTestParameters();
            Solve(2, 4);
            return _count.ToString();
            return "done";
        }

        private void SetTestParameters() {
            _colors = 2;
            _colorCount = 3;
            _pullCount = 2;
            _factorial = Factorial(_pullCount);
        }

        private void SetProblemParameters() {
            _colors = 7;
            _colorCount = 10;
            _pullCount = 20;
            _factorial = Factorial(_pullCount);
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
                _combinations[n].Add(r, Factorial(n) / (Factorial(r) * Factorial(n - r)));
            }
            return _combinations[n][r];
        }

        private double _count;
        private void Solve() {
            
        }


        //private Dictionary<string, HashItem> _hash = new Dictionary<string, HashItem>();
        //private List<int> _balls = new List<int>();
        //private List<bool> _selectedBalls = new List<bool>();
        //private Dictionary<int, int> _colorCounts = new Dictionary<int, int>();
        //private int _colors;
        //private int _colorCount;
        //private int _pullCount;

        //public string ProblemName {
        //    get { return "493: Under The Rainbow"; }
        //}

        //public string GetAnswer() {
        //    SetTestParameters();
        //    BuildBalls();
        //    //return Solve(0, _pullCount).GetAverage().ToString();
        //    return Solve(0, _pullCount).Count.ToString();
        //}

        //private void SetTestParameters() {
        //    _colors = 2;
        //    _colorCount = 3;
        //    _pullCount =4;
        //}

        //private void SetProblemParameters() {
        //    _colors = 7;
        //    _colorCount = 10;
        //    _pullCount = 20;
        //}

        //public void BuildBalls() {
        //    for (int colors = 1; colors <= _colors; colors++) {
        //        for (int colorCount = 1; colorCount <= _colorCount; colorCount++) {
        //            _balls.Add((int)Math.Pow(2, colors - 1));
        //            _selectedBalls.Add(false);
        //        }
        //        _colorCounts.Add((int)Math.Pow(2, colors - 1), 0);
        //    }
        //}

        //public HashItem Solve(int colorBits, int ballsRemaining) {
        //    HashItem score = new HashItem();
        //    for (int ball = 0; ball < _selectedBalls.Count; ball++) {
        //        if (!_selectedBalls[ball]) {
        //            bool remove = false;
        //            if ((colorBits & _balls[ball]) == 0) {
        //                remove = true;
        //            }
        //            colorBits = (colorBits | _balls[ball]);
        //            _colorCounts[_balls[ball]]++;
        //            string hashKey = GetColorCountKey();
        //            if (ballsRemaining == 1) {
        //                score.Count++;
        //                score.Sum += (double)CountBitsInInteger(colorBits);
        //            } else if (_hash.ContainsKey(hashKey)) {
        //                HashItem next = _hash[hashKey];
        //                score.Merge(next);
        //            } else {
        //                _selectedBalls[ball] = true;
        //                HashItem next = Solve(colorBits, ballsRemaining - 1);
        //                score.Merge(next);
        //                _selectedBalls[ball] = false;
        //                _hash.Add(hashKey, new HashItem(next));
        //            }
        //            if (remove) {
        //                colorBits = (colorBits ^ _balls[ball]);
        //            }
        //            _colorCounts[_balls[ball]]--;
        //        }
        //    }
        //    return score;
        //}

        //private int CountBitsInInteger(int i) {
        //    i = i - ((i >> 1) & 0x55555555);
        //    i = (i & 0x33333333) + ((i >> 2) & 0x33333333);
        //    return (((i + (i >> 4)) & 0x0F0F0F0F) * 0x01010101) >> 24;
        //}

        //private string GetColorCountKey() {
        //    StringBuilder key = new StringBuilder();
        //    foreach (int color in _colorCounts.Keys) {
        //        key.Append(_colorCounts[color] + ";");
        //    }
        //    return key.ToString();
        //}

        //public class HashItem {
        //    public double Count { get; set; }
        //    public double Sum { get; set; }

        //    public void Merge(HashItem item) {
        //        this.Count += item.Count;
        //        this.Sum += item.Sum;
        //    }

        //    public double GetAverage() {
        //        return this.Sum / this.Count;
        //    }

        //    public HashItem() {

        //    }

        //    public HashItem(HashItem copyFrom) {
        //        this.Count = copyFrom.Count;
        //        this.Sum = copyFrom.Sum;
        //    }
        //}
    }
}
