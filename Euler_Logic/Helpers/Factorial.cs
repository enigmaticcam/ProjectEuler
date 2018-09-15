using System.Collections.Generic;

namespace Euler_Logic.Helpers {
    public class FactorialWithHashULong {
        private Dictionary<ulong, ulong> _hash = new Dictionary<ulong, ulong>();

        public ulong Get(ulong num) {
            if (!_hash.ContainsKey(num)) {
                ulong result = 1;
                for (ulong product = 2; product <= num; product++) {
                    result *= product;
                }
                _hash.Add(num, result);
            }

            return _hash[num];
        }
    }
}