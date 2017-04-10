using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem33 : ProblemBase {
        public override string ProblemName {
            get { return "33: Digit cancelling fractions"; }
        }

        public override string GetAnswer() {
            return GetCount().ToString();
        }

        private decimal GetCount() {
            decimal num = 1;
            decimal den = 1;
            for (decimal a = 10; a <= 99; a++) {
                for (decimal b = a + 1; b <= 99; b++) {
                    if (a % 10 == 0 && b % 10 == 0) {
                        break;
                    }
                    string aAsString = a.ToString();
                    string bAsString = b.ToString();
                    for (int index = 0; index < 2; index++) {
                        string digit = aAsString.Substring(index, 1);
                        string aCancelled = aAsString.Replace(digit, "");
                        string bCancelled = bAsString.Replace(digit, "");

                        if (aCancelled.Length > 0 && bCancelled.Length > 0 && aCancelled != "0" && bCancelled != "0") {
                            decimal x = a / b;
                            decimal y = Convert.ToDecimal(aCancelled) / Convert.ToDecimal(bCancelled);
                            if (x == y) {
                                num *= a;
                                den *= b;
                            }
                        }
                    }
                }
            }
            return FindLowestDen(num, den);
        }

        private decimal FindLowestDen(decimal a, decimal b) {
            decimal den = 0;
            if (b % 2 == 0) {
                den = b / 2;
            } else {
                den = (b / 2) - Convert.ToDecimal(0.5);
            }
            while (true) {
                if (a % den == 0 && b % den == 0) {
                    return (b / den);
                }
                den -= 1;
            }
        }
    }
}
