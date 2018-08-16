using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem119 : ProblemBase {
        private List<ulong> _numbers = new List<ulong>();

        public override string ProblemName {
            get { return "119: Digit power sum"; }
        }
        
        /*
            So this amazingly worked first try. I'm sure there are better ways, but whatever.

            Pick a base number, say 8. Try 8^2=16. 1+6 is not equal to 8. How about 8^3=512? 5+1+2=8 so save it. Now try 8^4=4096. 4+9+0+6 is not equal to 8. In fact,
            we can stop adding after 9, because 4+9>8. Keep doing this until we reach the ulong limit, 2^63. Then try powers of 9. Do this for all base numbers 2
            to 100. Return the 30th.
        */

        public override string GetAnswer() {
            return Solve().ToString();
        }

        private ulong Solve() {
            for (ulong baseNum = 2; baseNum <= 100; baseNum++) {
                ulong number = baseNum * baseNum;
                ulong lastNumber = number;
                do {
                    if (number >= 10) {
                        ulong sum = 0;
                        ulong temp = number;
                        do {
                            ulong digit = temp % 10;
                            temp /= 10;
                            sum += digit;
                            if (sum > baseNum) {
                                break;
                            }
                        } while (temp > 0);
                        if (sum == baseNum) {
                            _numbers.Add(number);
                        }
                    }
                    lastNumber = number;
                    number *= baseNum;
                } while (number > lastNumber);
            }
            return _numbers.OrderBy(x => x).ElementAt(29);
        }
    }
}
