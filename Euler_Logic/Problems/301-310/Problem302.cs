using Euler_Logic.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Euler_Logic.Problems {
    public class Problem302 : ProblemBase {
        private PrimeSieve _primes;
        private List<ulong[]> _factors;
        private List<Tuple<ulong, ulong>> _primeFactors;
        private List<ulong> _found;

        public override string ProblemName {
            get { return "302: Strong Achilles Numbers"; }
        }

        public override string GetAnswer() {
            ulong max = (ulong)Math.Pow(10, 18);
            //ulong max = 100000000;
            Initialize(max);
            //return BruteForce(max).ToString();
            Begin(max);
            return _found.Count.ToString();
        }

        private void Initialize(ulong max) {
            //_primes = new PrimeSieve((ulong)Math.Sqrt(max) / 8);
            _primes = new PrimeSieve(18612708);
            _factors = new List<ulong[]>();
            _primeFactors = new List<Tuple<ulong, ulong>>();
            _found = new List<ulong>();
            foreach (var prime in _primes.Enumerate) {
                _factors.Add(new ulong[2] { prime, 0 });
            }
        }

        private void Begin(ulong max) {
            //Recursive1(1, max, 0, _primes.Enumerate.Count() - 1, 0);
            Recursive2(1, max, 0, 4, 0);
        }

        //private void Recursive1(ulong currentNum, ulong maxNum, int currentPrimeIndex, int maxPrimeIndex, ulong gcd) {
        //    if (currentPrimeIndex < maxPrimeIndex) Recursive1(currentNum, maxNum, currentPrimeIndex + 1, maxPrimeIndex, gcd);
        //    var prime = _primes.Enumerate.ElementAt(currentPrimeIndex);
        //    if (maxNum / currentNum >= prime * prime) {
        //        _factors[currentPrimeIndex][1] = 2;
        //        currentNum *= prime * prime;
        //        do {
        //            if (GCDULong.GCD(gcd, _factors[currentPrimeIndex][1]) == 1) {
        //                var result = IsGood(currentNum);
        //                if (result) _found.Add(currentNum);
        //            }
        //            if (currentPrimeIndex < maxPrimeIndex) Recursive1(currentNum, maxNum, currentPrimeIndex + 1, maxPrimeIndex, GCDULong.GCD(gcd, _factors[currentPrimeIndex][1]));
        //            if (maxNum / currentNum < prime) break;
        //            currentNum *= prime;
        //            _factors[currentPrimeIndex][1]++;
        //        } while (true);
        //    }
        //    _factors[currentPrimeIndex][1] = 0;
        //}

        private void Recursive2(ulong currentNum, ulong maxNum, ulong gcd, int remaining, int startIndex) {
            var tempNum = currentNum;
            for (int index = startIndex; index < _primes.Count; index++) {
                var prime = _primes.Enumerate.ElementAt(index);
                currentNum = tempNum;
                if (maxNum / currentNum >= prime * prime) {
                    _factors[index][1] = 2;
                    currentNum *= prime * prime;
                    do {
                        if (currentNum == 500) {
                            bool stop = true;
                        }
                        var subGcd = GCDULong.GCD(gcd, _factors[index][1]);
                        if (GCDULong.GCD(gcd, _factors[index][1]) == 1) {
                            var result = IsGood(currentNum);
                            if (result) _found.Add(currentNum);
                        }
                        if (remaining > 1) Recursive2(currentNum, maxNum, subGcd, remaining - 1, index + 1);
                        if (maxNum / currentNum < prime) break;
                        currentNum *= prime;
                        _factors[index][1]++;
                    } while (true);
                }
                _factors[index][1] = 0;
            }
        }

        private bool IsGood(ulong num) {
            var totient = Totient(num);
            FindPrimeFactors(totient);
            return IsArchilles();
        }

        private ulong Totient(ulong num) {
            ulong totient = num;
            foreach (var factor in _factors) {
                if (factor[1] > 0) totient = totient * (factor[0] - 1) / factor[0];
            }
            return totient;
        }

        private void FindPrimeFactors(ulong num) {
            _primeFactors.Clear();
            ulong max = (ulong)Math.Sqrt(num);
            foreach (var prime in _primes.Enumerate) {
                if (prime > max) break;
                if (num % prime == 0) {
                    ulong count = 0;
                    do {
                        num /= prime;
                        count++;
                    } while (num % prime == 0);
                    _primeFactors.Add(new Tuple<ulong, ulong>(prime, count));
                    if (num == 1) break;
                }
            }
        }

        private bool IsPowerful() {
            foreach (var factor in _primeFactors) {
                if (factor.Item2 == 1) return false;
            }
            return true;
        }

        private bool IsPerfect() {
            var gcd = _primeFactors[0].Item2;
            foreach (var factor in _primeFactors) {
                gcd = GCDULong.GCD(gcd, factor.Item2);
                if (gcd == 1) return false;
            }
            return true;
        }

        private bool IsArchilles() {
            return IsPowerful() && !IsPerfect();
        }

        private int BruteForce(ulong max) {
            _primeFactors = new List<Tuple<ulong, ulong>>();
            _primes = new PrimeSieve(max);
            var list = new List<ulong>();
            for (ulong num = 2; num <= max; num++) {
                if (IsS(num)) list.Add(num);
            }
            return list.Count;
        }

        private ulong _max;
        private bool IsS(ulong num) {
            FindPrimeFactors(num);
            if (IsArchilles()) {
                var totient = BruteForceTotient(num);
                FindPrimeFactors(totient);
                if (IsArchilles()) {
                    FindPrimeFactors(num);
                    var max = _primeFactors.Select(x => x.Item1).Max();
                    if (max > _max) _max = max;
                    return true;
                }
                //return IsArchilles();
            }
            return false;
        }

        private ulong BruteForceTotient(ulong num) {
            ulong totient = num;
            foreach (var factor in _primeFactors) {
                totient = totient * (factor.Item1 - 1) / factor.Item1;
            }
            return totient;
        }

        //private int BruteForce(ulong max) {
        //    _primeFactors = new List<Tuple<ulong, ulong>>();
        //    _primes = new PrimeSieve(max);
        //    var list = new List<ulong>();
        //    for (ulong num = 2; num <= max; num++) {
        //        if (IsS(num)) list.Add(num);
        //    }
        //    return list.Count;
        //}

        //private HashSet<ulong> _found = new HashSet<ulong>();
        //private bool IsS(ulong num) {
        //    FindPrimeFactors(num);
        //    if (IsArchilles()) {
        //        var totient = Totient(num);
        //        FindPrimeFactors(totient);
        //        if (IsArchilles()) {
        //            FindPrimeFactors(num);
        //            _primeFactors.ForEach(x => _found.Add(x.Item1));
        //            return true;
        //        }
        //    }
        //    return false;
        //}

        //private void FindPrimeFactors(ulong num) {
        //    _primeFactors.Clear();
        //    if (_primes.IsPrime(num)) {
        //        _primeFactors.Add(new Tuple<ulong, ulong>(num, 1));
        //    } else {
        //        ulong max = (ulong)Math.Sqrt(num);
        //        foreach (var prime in _primes.Enumerate) {
        //            if (prime > max) break;
        //            if (num % prime == 0) {
        //                ulong count = 0;
        //                do {
        //                    num /= prime;
        //                    count++;
        //                } while (num % prime == 0);
        //                _primeFactors.Add(new Tuple<ulong, ulong>(prime, count));
        //                if (_primes.IsPrime(num)) {
        //                    _primeFactors.Add(new Tuple<ulong, ulong>(num, 1));
        //                    num /= num;
        //                }
        //                if (num == 1) break;
        //            }
        //        }
        //    }
        //}

        //private bool IsPowerful() {
        //    foreach (var factor in _primeFactors) {
        //        if (factor.Item2 == 1) return false;
        //    }
        //    return true;
        //}

        //private bool IsPerfect() {
        //    var gcd = _primeFactors[0].Item2;
        //    foreach (var factor in _primeFactors) {
        //        gcd = GCDULong.GCD(gcd, factor.Item2);
        //        if (gcd == 1) return false;
        //    }
        //    return true;
        //}

        //private bool IsArchilles() {
        //    return IsPowerful() && !IsPerfect();
        //}

        //private ulong Totient(ulong num) {
        //    ulong totient = num;
        //    foreach (var factor in _primeFactors) {
        //        totient = totient * (factor.Item1 - 1) / factor.Item1;
        //    }
        //    return totient;
        //}
    }
}
