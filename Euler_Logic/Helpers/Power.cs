using System;
using System.Collections.Generic;

namespace Euler_Logic.Helpers {
    public class PowerULong {
        private Dictionary<int, Dictionary<int, ulong>> _hash = new Dictionary<int, Dictionary<int, ulong>>();

        public ulong Power(int root, int power) {
            if (!_hash.ContainsKey(root)) {
                _hash.Add(root, new Dictionary<int, ulong>());
            }
            if (!_hash[root].ContainsKey(power)) {
                _hash[root].Add(power, (ulong)Math.Pow(root, power));
            }
            return _hash[root][power];
        }
    }
}
