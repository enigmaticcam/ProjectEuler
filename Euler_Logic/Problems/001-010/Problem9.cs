using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem9 : ProblemBase {
        public override string ProblemName {
            get { return "9: Special Pythagorean triplet"; }
        }

        public override string GetAnswer() {
            return FindTripletSum().ToString();
        }

        private double FindTripletSum() {
            double a = 1;
            double b = 2;
            double c = 0;
            do {
                c = Math.Sqrt((a * a) + (b * b));
                if (a + b + c == 1000) {
                    return a * b * c;
                } else if (a + b + c > 1000) {
                    a += 1;
                    b = a + 1;
                } else {
                    b += 1;
                }
            } while (true);
        }
    }
}
