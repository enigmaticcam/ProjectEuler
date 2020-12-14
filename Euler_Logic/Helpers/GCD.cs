using System.Numerics;

namespace Euler_Logic.Helpers {
    public static class GCD {
        public static ulong GetGCD(ulong num1, ulong num2) {
            while (num1 != 0 && num2 != 0) {
                if (num1 > num2) {
                    num1 %= num2;
                } else {
                    num2 %= num1;
                }
            }
            return num1 == 0 ? num2 : num1;
        }

        public static int GetGCD(int num1, int num2) {
            while (num1 != 0 && num2 != 0) {
                if (num1 > num2) {
                    num1 %= num2;
                } else {
                    num2 %= num1;
                }
            }
            return num1 == 0 ? num2 : num1;
        }

        public static long GetGCD(long num1, long num2) {
            while (num1 != 0 && num2 != 0) {
                if (num1 > num2) {
                    num1 %= num2;
                } else {
                    num2 %= num1;
                }
            }
            return num1 == 0 ? num2 : num1;
        }

        public static BigInteger GetGCD(BigInteger num1, BigInteger num2) {
            while (num1 != 0 && num2 != 0) {
                if (num1 > num2) {
                    num1 %= num2;
                } else {
                    num2 %= num1;
                }
            }
            return num1 == 0 ? num2 : num1;
        }
    }

    public static class LCM {
        public static ulong GetLCM(ulong num1, ulong num2) {
            return (num1 / GCD.GetGCD(num1, num2)) * num2;
        }

        public static int GetLCM(int num1, int num2) {
            return (num1 / GCD.GetGCD(num1, num2)) * num2;
        }

        public static long GetLCM(long num1, long num2) {
            return (num1 / GCD.GetGCD(num1, num2)) * num2;
        }

        public static BigInteger GetLCM(BigInteger num1, BigInteger num2) {
            return (num1 / GCD.GetGCD(num1, num2)) * num2;
        }
    }

    public class GCDUInt {
        public uint GCD(uint num1, uint num2) {
            while (num1 != 0 && num2 != 0) {
                if (num1 > num2) {
                    num1 %= num2;
                } else {
                    num2 %= num1;
                }
            }
            return num1 == 0 ? num2 : num1;
        }
    }

    public class GCDInt {
        public int GCD(int num1, int num2) {
            while (num1 != 0 && num2 != 0) {
                if (num1 > num2) {
                    num1 %= num2;
                } else {
                    num2 %= num1;
                }
            }
            return num1 == 0 ? num2 : num1;
        }
    }

    public class GCDULong {
        public ulong GCD(ulong num1, ulong num2) {
            while (num1 != 0 && num2 != 0) {
                if (num1 > num2) {
                    num1 %= num2;
                } else {
                    num2 %= num1;
                }
            }
            return num1 == 0 ? num2 : num1;
        }
    }

    public class GCDDec {
        public decimal GCD(decimal num1, decimal num2) {
            while (num1 != 0 && num2 != 0) {
                if (num1 > num2) {
                    num1 %= num2;
                } else {
                    num2 %= num1;
                }
            }
            return num1 == 0 ? num2 : num1;
        }
    }

    public class LCMULong {
        private GCDULong _gcd = new GCDULong();

        public ulong LCM(ulong num1, ulong num2) {
            return (num1 / _gcd.GCD(num1, num2)) * num2;
        }
    }
}
