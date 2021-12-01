using System;
using System.Collections.Generic;

namespace Euler_Logic.Helpers {

    public class PrimeSieve {
        private bool[] _notPrimes;
        private List<ulong> _primes = new List<ulong>();

        public PrimeSieve(ulong max) {
            SievePrimes(max);
        }

        private void SievePrimes(ulong max) {
            _notPrimes = new bool[max / 2 + 1];
            _primes.Add(2);
            var maxComposite = (ulong)Math.Sqrt(max);
            for (ulong num = 3; num <= max; num += 2) {
                if (!_notPrimes[num / 2 - 1]) {
                    _primes.Add(num);
                    if (num <= maxComposite) {
                        for (ulong composite = num; composite * num <= max; composite += 2) {
                            _notPrimes[composite * num / 2 - 1] = true;
                        }
                    }
                }
            }
        }

        public bool IsPrime(ulong num) {
            if (num <= 1) {
                return false;
            } else if (num == 2) {
                return true;
            } else if (num % 2 == 0) {
                return false;
            } else {
                return !_notPrimes[num / 2 - 1];
            }
        }

        public int Count {
            get { return _primes.Count; }
        }

        public IEnumerable<ulong> Enumerate {
            get { return _primes; }
        }

        public ulong this[int index] {
            get { return _primes[index]; }
        }
    }

    public class PrimeSieveUInt {
        private bool[] _notPrimes;
        private List<uint> _primes = new List<uint>();

        public PrimeSieveUInt(uint max) {
            SievePrimes(max);
        }

        private void SievePrimes(uint max) {
            _notPrimes = new bool[max / 2 + 1];
            _primes.Add(2);
            var maxComposite = (uint)Math.Sqrt(max);
            for (uint num = 3; num <= max; num += 2) {
                if (!_notPrimes[num / 2 - 1]) {
                    _primes.Add(num);
                    if (num <= maxComposite) {
                        for (uint composite = num; composite * num <= max; composite += 2) {
                            _notPrimes[composite * num / 2 - 1] = true;
                        }
                    }
                }
            }
        }

        public bool IsPrime(uint num) {
            if (num <= 1) {
                return false;
            } else if (num == 2) {
                return true;
            } else if (num % 2 == 0) {
                return false;
            } else {
                return !_notPrimes[num / 2 - 1];
            }
        }

        public int Count {
            get { return _primes.Count; }
        }

        public IEnumerable<uint> Enumerate {
            get { return _primes; }
        }

        public uint this[int index] {
            get { return _primes[index]; }
        }
    }

    public class PrimeSieveSimple {
        private bool[] _notPrimes;

        public void SievePrimes(int max) {
            _notPrimes = new bool[max + 1];
            for (uint num = 2; num <= max; num++) {
                if (!_notPrimes[num]) {
                    for (uint composite = num; composite * num <= max; composite++) {
                        _notPrimes[composite * num] = true;
                    }
                }
            }
        }

        public bool IsPrime(int num) {
            if (num <= 1) {
                return false;
            } else {
                return !_notPrimes[num];
            }
        }
    }

    public class PrimeSieveWithPrimeListUInt {
        private bool[] _notPrimes;
        private List<uint> _primes = new List<uint>();

        public void SievePrimes(uint max) {
            _notPrimes = new bool[max + 1];
            for (uint num = 2; num <= max; num++) {
                if (!_notPrimes[num]) {
                    _primes.Add(num);
                    for (uint composite = num; composite * num <= max; composite++) {
                        _notPrimes[composite * num] = true;
                    }
                }
            }
        }

        public uint this[int index] {
            get { return _primes[index]; }
        }

        public bool IsPrime(uint num) {
            return !_notPrimes[num];
        }

        public List<uint> Enumerate {
            get { return _primes; }
        }

        public int Count {
            get { return _primes.Count; }
        }
    }

    public class PrimeSieveWithPrimeListULong {
        private bool[] _notPrimes;
        private List<ulong> _primes = new List<ulong>();

        public void SievePrimes(ulong max) {
            _notPrimes = new bool[max + 1];
            _notPrimes[0] = true;
            _notPrimes[1] = true;
            for (ulong num = 2; num <= max; num++) {
                if (!_notPrimes[num]) {
                    _primes.Add(num);
                    for (ulong composite = num; composite * num <= max; composite++) {
                        _notPrimes[composite * num] = true;
                    }
                }
            }
        }

        public ulong this[int index] {
            get { return _primes[index]; }
        }

        public bool IsPrime(ulong num) {
            return !_notPrimes[num];
        }

        public List<ulong> Enumerate {
            get { return _primes; }
        }

        public int Count {
            get { return _primes.Count; }
        }
    }

    public class PrimeSieveWithPrimeListDecimal {
        private bool[] _notPrimes;
        private List<decimal> _primes = new List<decimal>();

        public void SievePrimes(uint max) {
            _notPrimes = new bool[max + 1];
            _notPrimes[0] = true;
            _notPrimes[1] = true;
            for (uint num = 2; num <= max; num++) {
                if (!_notPrimes[num]) {
                    _primes.Add(num);
                    for (uint composite = num; composite * num <= max; composite++) {
                        _notPrimes[composite * num] = true;
                    }
                }
            }
        }

        public decimal this[int index] {
            get { return _primes[index]; }
        }

        public bool IsPrime(uint num) {
            return !_notPrimes[num];
        }

        public List<decimal> Enumerate {
            get { return _primes; }
        }

        public int Count {
            get { return _primes.Count; }
        }
    }
}