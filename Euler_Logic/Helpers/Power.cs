using System;
using System.Collections.Generic;

namespace Euler_Logic.Helpers {
    public class Power {
        private Dictionary<long, Dictionary<long, long>> _powers = new Dictionary<long, Dictionary<long, long>>();

        public long GetPower(long root, long exponent) {
            if (!_powers.ContainsKey(root)) {
                _powers.Add(root, new Dictionary<long, long>());
            }
            if (!_powers[root].ContainsKey(exponent)) {
                _powers[root].Add(exponent, (long)Math.Pow(root, exponent));
            }
            return _powers[root][exponent];
        }

        // Calculate x^y % z
        public static ulong Exp(ulong num, ulong exponent, ulong mod) {
            if (exponent == 0) {
                return 1;
            } else if (exponent == 1) {
                return num % mod;
            } else if (exponent % 2 == 0) {
                return Exp((num * num) % mod, exponent / 2, mod);
            } else {
                return (num * Exp((num * num) % mod, (exponent - 1) / 2, mod)) % mod;
            }
        }
    }
}