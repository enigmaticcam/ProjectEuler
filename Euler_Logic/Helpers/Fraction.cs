using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Helpers {
    public class Fraction {
        public Fraction() { }
        public Fraction(ulong x, ulong y) {
            X = x; 
            Y = y;
        }

        public ulong X { get; set; }
        public ulong Y { get; set; }
        
        public void Multiply(ulong x, ulong y) {
            X *= x;
            Y *= y;
            Reduce();
        }

        public void Reduce() {
            var gcd = GCD.GetGCD(X, Y);
            X /= gcd;
            Y /= gcd;
        }

        public void Subtract(ulong x, ulong y) {
            var lcm = LCM.GetLCM(Y, y);
            X = lcm / Y * X - lcm / y * x;
            Y = lcm;
            Reduce();
        }
    }
}
