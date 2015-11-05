using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem75 : IProblem {
        private HashSet<double> _squares = new HashSet<double>();
        private List<double> _squaresList;
        private Dictionary<double, int> _numbers = new Dictionary<double, int>();
        private HashSet<double> _primes = new HashSet<double>();

        public string ProblemName {
            get { return "75: Singular Integer Right Triangles"; }
        }

        public string GetAnswer() {
            double max = 1500000;
            SievePrimes(max);
            GenerateSquares(max);
            NewTest(max);
            return GetFinalCount(max).ToString();
        }

        private void GenerateSquares(double max) {
            for (double num = 2; num <= max; num++) {
                _squares.Add(num * num);
            }
            _squaresList = _squares.ToList();
        }

        private void NewTest(double max) {
            _primes.Remove(2);
            _primes.Remove(3);
            foreach (double hypotenuse in _primes) {
                if (!CheckIfHypotenuseIsGood(hypotenuse, max)) {
                    break;
                }
            }
        }

        private bool CheckIfHypotenuseIsGood(double hypotenuse, double max) {
            double hypSquared = hypotenuse * hypotenuse;
            int squareIndex = 0;
            bool isFirst = true;
            do {
                if (_squares.Contains(hypSquared - _squaresList[squareIndex])) {
                    double a = Math.Sqrt(_squaresList[squareIndex]);
                    double b = Math.Sqrt(hypSquared - _squaresList[squareIndex]);
                    if (isFirst && a + b + hypotenuse > max) {
                        return false;
                    } else {
                        isFirst = false;
                    }
                    SeiveTrianglePerimeter(a, b, hypotenuse, max);
                    break;
                }
                squareIndex++;
            } while (_squaresList[squareIndex] < hypSquared / 2);
            return true;
        }

        private void SeiveTrianglePerimeter(double a, double b, double hypotenuse, double max) {
            double compositeFactor = 1;
            double length = 0;
            do {
                length = (a * compositeFactor) + (b * compositeFactor) + (hypotenuse * compositeFactor);
                if (_numbers.ContainsKey(length)) {
                    _numbers[length]++;
                } else {
                    _numbers.Add(length, 1);
                }
                compositeFactor++;
            } while (length < max);
        }

        private int GetFinalCount(double max) {
            int count = 0;
            List<double> keys = _numbers.Keys.ToList();
            keys.Sort();
            foreach (double length in keys) {
                if (length > max) {
                    break;
                }
                if (_numbers[length] == 1) {
                    count++;
                }
            }
            return count;
        }

        private void SievePrimes(double max) {
            HashSet<double> numbers = new HashSet<double>();
            for (double num = 2; num <= max; num++) {
                if (!numbers.Contains(num)) {
                    _primes.Add(num);
                    double composite = num;
                    do {
                        numbers.Add(composite);
                        composite += num;
                    } while (composite <= max);
                }
            }
        }
        
    }
}
