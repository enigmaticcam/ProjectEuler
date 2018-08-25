namespace Euler_Logic.Helpers {
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

    public class LCMULong {
        private GCDULong _gcd = new GCDULong();

        public ulong LCM(ulong num1, ulong num2) {
            return (num1 / _gcd.GCD(num1, num2)) * num2;
        }
    }
}