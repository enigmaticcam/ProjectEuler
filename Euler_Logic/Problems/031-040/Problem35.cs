using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem35 : IProblem {
        private HashSet<string> _primes = new HashSet<string>();

        public string ProblemName {
            get { return "35: Circular primes"; }
        }

        public string GetAnswer() {
            return CircularPrimeCount().ToString();
        }

        private int CircularPrimeCount() {
            int count = 1;
            _primes.Add("2");
            for (int num = 3; num < 1000000; num += 2) {
                string numAsString = num.ToString();
                if (!_primes.Contains(numAsString)) {
                    List<string> rotations = GenerateRotations(numAsString);
                    bool isGood = true;
                    foreach (string rotation in rotations) {
                        if (!IsPrime(Convert.ToDecimal(rotation))) {
                            isGood = false;
                        }
                        _primes.Add(rotation);
                    }
                    if (isGood) {
                        count += rotations.Count;
                    }
                }
                
            }
            return count;
        }

        private List<string> GenerateRotations(string number) {
            List<string> rotations = new List<string>();
            rotations.Add(number);
            if (number.Replace(number.Substring(0, 1), "") != "") {
                string lastRotation = number;
                for (int index = 0; index < number.Length - 1; index++) {
                    lastRotation = lastRotation.Substring(lastRotation.Length - 1, 1) + lastRotation.Substring(0, lastRotation.Length - 1);
                    rotations.Add(lastRotation);
                }
            }
            return rotations;
        }

        private bool IsPrime(decimal num) {
            if (num == 2) {
                return true;
            } else if (num % 2 == 0) {
                return false;
            } else {
                for (ulong factor = 3; factor <= Math.Sqrt((double)num); factor += 2) {
                    if (num % factor == 0) {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
