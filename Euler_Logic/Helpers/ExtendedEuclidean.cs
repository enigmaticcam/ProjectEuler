using System;

namespace Euler_Logic.Helpers {
    public static class ExtendedEuclidean {
        public static Tuple<long, long> GetInverse(long a, long b) {
            long s = 0;
            long t = 1;
            long r = b;
            long oldS = 1;
            long oldT = 0;
            long oldR = a;

            while (r != 0) {
                var q = oldR / r;
                var nextR = oldR - q * r;
                oldR = r;
                r = nextR;

                var nextS = oldS - q * s;
                oldS = s;
                s = nextS;

                var nextT = oldT - q * t;
                oldT = t;
                t = nextT;
            }
            return new Tuple<long, long>(oldS, oldT);
        }
    }
}
