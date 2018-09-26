using System;

namespace Euler_Logic.Helpers {
    public static class PrimalityULong {
        public static bool IsPrime(ulong num) {
            if (num < 2) {
                return false;
            }
            if (num % 2 == 0 || num % 3 == 0) {
                return false;
            }
            ulong max = (ulong)Math.Sqrt(num);
            for (ulong sub = 5; sub <= max; sub++) {
                if (num % sub == 0) {
                    return false;
                }
            }
            return true;
        }
    }
}
