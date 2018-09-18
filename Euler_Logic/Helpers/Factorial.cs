using System.Collections.Generic;

namespace Euler_Logic.Helpers {
    public class FactorialWithHashULong {
        private Dictionary<ulong, ulong> _hash = new Dictionary<ulong, ulong>();
        private ulong _highest = 1;

        public FactorialWithHashULong() {
            _hash.Add(1, 1);
        }

        public ulong Get(ulong num) {
            if (!_hash.ContainsKey(num)) {
                ulong result = _hash[_highest];
                for (ulong product = _highest + 1; product <= num; product++) {
                    result *= product;
                    _hash.Add(product, result);
                }
            }

            return _hash[num];
        }
    }

    public class FactorialWithHashInt {
        private Dictionary<int, int> _hash = new Dictionary<int, int>();
        private int _highest = 1;

        public FactorialWithHashInt() {
            _hash.Add(1, 1);
        }

        public int Get(int num) {
            if (!_hash.ContainsKey(num)) {
                int result = _hash[_highest];
                for (int product = _highest + 1; product <= num; product++) {
                    result *= product;
                    _hash.Add(product, result);
                }
            }

            return _hash[num];
        }
    }

    public class FactorialWithHashUInt {
        private Dictionary<uint, uint> _hash = new Dictionary<uint, uint>();
        private uint _highest = 1;

        public FactorialWithHashUInt() {
            _hash.Add(1, 1);
        }

        public uint Get(uint num) {
            if (!_hash.ContainsKey(num)) {
                uint result = _hash[_highest];
                for (uint product = _highest + 1; product <= num; product++) {
                    result *= product;
                    _hash.Add(product, result);
                }
            }

            return _hash[num];
        }
    }
}