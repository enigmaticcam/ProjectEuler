using System;
using System.Collections.Generic;

namespace Euler_Logic.Problems {
    public class Problem60 : ProblemBase {
        private Dictionary<int, HashSet<int>> _primeNetwork = new Dictionary<int, HashSet<int>>();
        private Dictionary<int, bool> _primes = new Dictionary<int, bool>();
        
        public override string ProblemName {
            get { return "60: Prime pair sets"; }
        }

        public override string GetAnswer() {
            return LowestPairSum(5).ToString();
        }

        private int LowestPairSum(int max) {
            int num = 3;
            do {
                if (IsPrime(num)) {
                    _primeNetwork.Add(num, new HashSet<int>());
                    int sum = FindPairSums(num, max);
                    if (sum > 0) {
                        return sum;
                    }
                }
                num += 2;
            } while (true);
        }

        private int FindPairSums(int num, int max) {
            HashSet<int> alreadyTriedPrimes = new HashSet<int>();
            HashSet<int> goodPrimes = new HashSet<int>();
            goodPrimes.Add(num);
            foreach (int prime in _primeNetwork.Keys) {
                if (CanPair(num, prime)) {
                    _primeNetwork[num].Add(prime);
                    _primeNetwork[prime].Add(num);
                    alreadyTriedPrimes.Clear();
                    alreadyTriedPrimes.Add(prime);
                    goodPrimes.Add(prime);
                    int sum = PairSum(num, 2, _primeNetwork[prime], prime + num, max, alreadyTriedPrimes, goodPrimes);
                    if (sum > 0) {
                        return sum;
                    } else {
                        goodPrimes.Remove(prime);
                    }
                }
            }
            return 0;
        }

        private int PairSum(int num, int count, HashSet<int> primes, int sum, int max, HashSet<int> alreadyTriedPrimes, HashSet<int> goodPrimes) {
            foreach (int prime in primes) {
                if (prime != num && !alreadyTriedPrimes.Contains(prime)) {
                    if (CanPair(num, prime)) {
                        _primeNetwork[num].Add(prime);
                        bool canTry = true;
                        foreach (int goodPrime in goodPrimes) {
                            if (!CanPair(goodPrime, prime)) {
                                canTry = false;
                                break;
                            }
                        }
                        if (canTry) {
                            if (count + 1 == max) {
                                return sum += prime;
                            } else {
                                alreadyTriedPrimes.Add(prime);
                                goodPrimes.Add(prime);
                                int nextPrime = PairSum(num, count + 1, _primeNetwork[prime], sum += prime, max, alreadyTriedPrimes, goodPrimes);
                                if (nextPrime > 0) {
                                    return nextPrime;
                                } else {
                                    goodPrimes.Remove(prime);
                                }
                            }
                        }
                    }
                }
            }
            return 0;
        }

        private bool CanPair(int num, int prime) {
            if (prime != num) {
                var append = Append(num, prime);
                 if (IsPrime(append)) {
                    append = Append(prime, num);
                    if (IsPrime(append)) return true;
                }
            }
            return false;
        }

        private int Append(int num1, int num2) {
            var log = (int)Math.Log10(num2);
            var ten = (int)Math.Pow(10, log);
            if (ten != num2) ten *= 10;
            return (ten * num1) + num2;
        }

        private bool IsPrime(int num) {
            if (_primes.ContainsKey(num)) {
                return _primes[num];
            } else if (num == 2) {
                _primes.Add(num, true);
                return true;
            } else if (num % 2 == 0) {
                _primes.Add(num, false);
                return false;
            } else {
                int max = (int)Math.Sqrt(num);
                for (int factor = 3; factor <= max; factor += 2) {
                    if (num % factor == 0) {
                        _primes.Add(num, false);
                        return false;
                    }
                }
            }
            _primes.Add(num, true);
            return true;
        }
    }
}
