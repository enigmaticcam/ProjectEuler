using System;
using System.Collections.Generic;

namespace Euler_Logic.Helpers {
    public class Power {
        private Dictionary<long, Dictionary<long, long>> _powers = new Dictionary<long, Dictionary<long, long>>();
        private Dictionary<ulong, Dictionary<ulong, ulong>> _powersULong = new Dictionary<ulong, Dictionary<ulong, ulong>>();

        public long GetPower(long root, long exponent) {
            if (!_powers.ContainsKey(root)) {
                _powers.Add(root, new Dictionary<long, long>());
            }
            if (!_powers[root].ContainsKey(exponent)) {
                _powers[root].Add(exponent, (long)Math.Pow(root, exponent));
            }
            return _powers[root][exponent];
        }

        public ulong GetPower(ulong root, ulong exponent) {
            if (!_powersULong.ContainsKey(root)) {
                _powersULong.Add(root, new Dictionary<ulong, ulong>());
            }
            if (!_powersULong[root].ContainsKey(exponent)) {
                _powersULong[root].Add(exponent, (ulong)Math.Pow(root, exponent));
            }
            return _powersULong[root][exponent];
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