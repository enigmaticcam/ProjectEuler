using System;
using System.Collections.Generic;

namespace Euler_Logic.Problems {
    public class Problem371 : ProblemBase {
        private Dictionary<decimal, Dictionary<bool, decimal>> _hash = new Dictionary<decimal, Dictionary<bool, decimal>>();

        /*
            This can be solved via dynamic programming. At any point after having seen 0 to n cars, we have 4 possibilities that can occur:

            a - If we've seen any cars before, we have a chance of simply outright winning on the next one by seeing an opposite number
            b - We get the half number (500) if we haven't seen it before
            c - We get a new number that we haven't seen before but is not the half
            d - We get a number we've already seen before that's not the half or we get 0

            The final probability would then be a1(a + 1) + b1(b + 1) + c1(c + 1) + d1(d + 1) where a1 + b1 + c1 + d1 = 1.

            So then, I use a recursive function that calculates these four variables, starting at the beginning having seen no cars.
            Recursive calls are made on b and c. 
            
            To calculate d, we use basic algebra. Once we know the variables a, b, and c (and their values a1, b1, c1, and d1), we can simplify 
            it down to: 
            
            x = a + b + c + x(d1 + x)
            x = z + d1x
            x - d1x = z
            x = z/1/(1-d1)
            
            I use a hash table to save the results and briefly speed up the algorithm.
         */

        public override string ProblemName {
            get { return "371: Licence plates"; }
        }

        public override string GetAnswer() {
            decimal test = Try(1000, 0, false);
            return Math.Round(test, 8).ToString();
        }

        private decimal Try(decimal maxPlateNumber, decimal seenTotal, bool seenHalfNum) {
            if (!_hash.ContainsKey(seenTotal)) {
                _hash.Add(seenTotal, new Dictionary<bool, decimal>());
            }
            if (!_hash[seenTotal].ContainsKey(seenHalfNum)) {

                // chances of winning outright
                decimal a = 0;
                if (seenTotal > 0) {
                    a = seenTotal / maxPlateNumber;
                }

                // chances of seeing the half if not already seen half
                decimal b = 0;
                if (!seenHalfNum) {
                    b = (1 + Try(maxPlateNumber, seenTotal + 1, true)) * (1 / maxPlateNumber);
                }

                // Chances of hitting something new that's not the half
                decimal c = 0;
                if (seenTotal < maxPlateNumber / 2) {
                    decimal remaining = 0;
                    if (seenHalfNum) {
                        remaining = maxPlateNumber - ((seenTotal - 1) * 2) - 2;
                    } else {
                        remaining = maxPlateNumber - (seenTotal * 2) - 2;
                    }
                    if (remaining > 0) {
                        //c = (1 + Try(maxPlateNumber, seenTotal + 1, seenHalfNum)) * (1 / remaining);
                        c = (1 + Try(maxPlateNumber, seenTotal + 1, seenHalfNum)) * (remaining / maxPlateNumber);
                    }
                }

                // Chances of staying in place
                decimal d = (seenTotal - (seenHalfNum ? 1 : 0) + 1) / maxPlateNumber;
                d = (a + b + c + d) / (1 - d);

                _hash[seenTotal].Add(seenHalfNum, d);
            }
            return _hash[seenTotal][seenHalfNum];
        }
    }
}