using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem47 : ProblemBase {
        private Dictionary<double, bool> _isPrime = new Dictionary<double, bool>();
        private HashSet<double> _primes = new HashSet<double>();
        private Dictionary<double, int> _primeFactorCount = new Dictionary<double, int>();
        private Dictionary<double, Dictionary<int, Dictionary<int, bool>>> _hash = new Dictionary<double, Dictionary<int, Dictionary<int, bool>>>();

        public override string ProblemName {
            get { return "47: Distinct primes factors"; }
        }

        public override string GetAnswer() {
            return FindPrimes(4).ToString();
        }

        private double FindPrimes(int count) {
            double num = 3;
            double first = 0;
            bool isGood = false;
            int goodCount = 0;

            _primes.Add(2);
            do {
                if (IsPrime(num)) {
                    _primes.Add(num);
                }
                if (FindPrimeSums(num, 0, count)) {
                    if (isGood) {
                        goodCount++;
                    } else {
                        isGood = true;
                        goodCount = 1;
                        first = num;
                    }
                    if (goodCount == count) {
                        return first;
                    }
                } else {
                    isGood = false;
                }
                num++;
                if (num == 200000) {
                    throw new Exception();
                }
            } while (true);
        }

        private bool FindPrimeSums(double num, int primeIndex, int count) {
            if (count == 1) {
                for (int a = primeIndex; a < _primes.Count; a++) {
                    if (_primes.ElementAt(a) == num) {
                        return true;
                    } else if (_primes.ElementAt(a) > num) {
                        return false;
                    }
                }
                return false;

            } else if (_primes.Count - primeIndex - 1 < count) {
                return false;

            } else if (_hash.ContainsKey(num) && _hash[num].ContainsKey(primeIndex) && _hash[num][primeIndex].ContainsKey(count)) {
                return _hash[num][primeIndex][count];

            } else {                
                for (int index = primeIndex; index < _primes.Count; index++) {
                    double prime = _primes.ElementAt(index);
                    if (prime > Math.Sqrt(num)) {
                        AddToHash(num, primeIndex, count, false);
                        return false;
                    }
                    if (num % prime == 0) {
                        if (FindPrimeSums(num / prime, index + 1, count - 1)) {
                            AddToHash(num, primeIndex, count, true);
                            return true;
                        }
                    }
                    double power = Math.Pow(prime, prime);
                    if (power <= Math.Sqrt(num) && num % power == 0) {
                        if (FindPrimeSums(num / power, index + 1, count - 1)) {
                            AddToHash(num, primeIndex, count, true);
                            return true;
                        }
                    }

                    double timesItself = prime * prime;
                    if (timesItself <= Math.Sqrt(num) && num % timesItself == 0) {
                        if (FindPrimeSums(num / timesItself, index + 1, count - 1)) {
                            AddToHash(num, primeIndex, count, true);
                            return true;
                        }
                    }
                }
                AddToHash(num, primeIndex, count, false);
                return false;
            }
        }

        private void AddToHash(double num, int primeIndex, int count, bool value) {
            if (!_hash.ContainsKey(num)) {
                _hash.Add(num, new Dictionary<int, Dictionary<int, bool>>());
            }
            if (!_hash[num].ContainsKey(primeIndex)) {
                _hash[num].Add(primeIndex, new Dictionary<int, bool>());
            }
            _hash[num][primeIndex].Add(count, value);
        }

        private bool IsPrime(double num) {
            if (_isPrime.ContainsKey(num)) {
                return _isPrime[num];
            } else if (num == 2) {
                _isPrime.Add(num, true);
                return true;
            } else if (num % 2 == 0) {
                _isPrime.Add(num, false);
                return false;
            } else {
                for (ulong factor = 3; factor <= Math.Sqrt((double)num); factor += 2) {
                    if (num % factor == 0) {
                        _isPrime.Add(num, false);
                        return false;
                    }
                }
            }
            _isPrime.Add(num, true);
            return true;
        }
    }
}
