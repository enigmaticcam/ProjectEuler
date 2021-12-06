using System.Numerics;

namespace Euler_Logic.Helpers {
    public class Fraction {
        public ulong X { get; set; }
        public ulong Y { get; set; }

        public Fraction() { }
        public Fraction(ulong x, ulong y) {
            X = x;
            Y = y;
        }

        public void Divide(ulong x, ulong y) {
            Multiply(y, x);
        }

        public void Add(Fraction fraction) {
            Add(fraction.X, fraction.Y);
        }

        public void Add(ulong x, ulong y) {
            var lcm = LCM.GetLCM(y, Y);
            var thisX = lcm / Y * X;
            var thatX = lcm / y * x;
            X = thisX + thatX;
            Y = lcm;
            Reduce();
        }

        public void Multiply(Fraction fraction) {
            Multiply(fraction.X, fraction.Y);
        }

        public void Multiply(ulong x, ulong y) {
            X *= x;
            Y *= y;
            Reduce();
        }

        public void Subtract(Fraction fraction) {
            Subtract(fraction.X, fraction.Y);
        }

        public void Subtract(ulong x, ulong y) {
            var lcm = LCM.GetLCM(y, Y);
            var thisX = lcm / Y * X;
            var thatX = lcm / y * x;
            X = thisX - thatX;
            Y = lcm;
            Reduce();
        }

        public void Reduce() {
            var gcd = GCD.GetGCD(X, Y);
            X /= gcd;
            Y /= gcd;
        }

        public int Compare(ulong x, ulong y) {
            var lcm = LCM.GetLCM(y, Y);
            var thisX = lcm / Y * X;
            var thatX = lcm / y * x;
            if (thisX < thatX) {
                return -1;
            } else if (thisX > thatX) {
                return 1;
            } else {
                return 0;
            }
        }

        public int Compare(Fraction fraction) {
            return Compare(fraction.X, fraction.Y);
        }
    }

    public class FractionBigInteger {
        public BigInteger X { get; set; }
        public BigInteger Y { get; set; }

        public FractionBigInteger() { }
        public FractionBigInteger(BigInteger x, BigInteger y) {
            X = x;
            Y = y;
        }

        public void Divide(BigInteger x, BigInteger y) {
            Multiply(y, x);
        }

        public void Add(FractionBigInteger fraction) {
            Add(fraction.X, fraction.Y);
        }

        public void Add(BigInteger x, BigInteger y) {
            var lcm = LCM.GetLCM(y, Y);
            var thisX = lcm / Y * X;
            var thatX = lcm / y * x;
            X = thisX + thatX;
            Y = lcm;
            Reduce();
        }

        public void Multiply(FractionBigInteger fraction) {
            Multiply(fraction.X, fraction.Y);
        }

        public void Multiply(BigInteger x, BigInteger y) {
            X *= x;
            Y *= y;
            Reduce();
        }

        public void Subtract(FractionBigInteger fraction) {
            Subtract(fraction.X, fraction.Y);
        }

        public void Subtract(BigInteger x, BigInteger y) {
            var lcm = LCM.GetLCM(y, Y);
            var thisX = lcm / Y * X;
            var thatX = lcm / y * x;
            X = thisX - thatX;
            Y = lcm;
            Reduce();
        }

        public void Reduce() {
            var gcd = GCD.GetGCD(X, Y);
            X /= gcd;
            Y /= gcd;
        }

        public int Compare(BigInteger x, BigInteger y) {
            var lcm = LCM.GetLCM(y, Y);
            var thisX = lcm / Y * X;
            var thatX = lcm / y * x;
            if (thisX < thatX) {
                return -1;
            } else if (thisX > thatX) {
                return 1;
            } else {
                return 0;
            }
        }

        public int Compare(FractionBigInteger fraction) {
            return Compare(fraction.X, fraction.Y);
        }
    }
}
