using System;
using System.Collections.Generic;

namespace Euler_Logic.Helpers {
    public class PowerAll {
        private ulong _root;

        public PowerAll(ulong root) {
            _root = root;
            BuildPowers();
        }

        private Dictionary<int, ulong> _powers = new Dictionary<int, ulong>();
        public ulong GetPower(int exponent) {
            return _powers[exponent];
        }

        private Dictionary<ulong, int> _logs = new Dictionary<ulong, int>();
        public int GetLog(ulong log) {
            return _logs[log];
        }

        private void BuildPowers() {
            ulong num = 1;
            int max = (int)Math.Log(ulong.MaxValue, _root);
            for (int count = 0; count <= max; count++) {
                _powers.Add(count, num);
                _logs.Add(num, count);
                num *= _root;
            }
        }
    }
}
