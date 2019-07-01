using Euler_Logic.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Euler_Logic.Problems {
    public class Problem196 : ProblemBase {
        private PrimeSieve _primes;

        /*
            This turned out to be must easier than it looks. The numbers in row 7208785 are obviously very high. A primality test for each
            individual number is way too slow. However, row 7208785 only has 7208785 numbers. Thus, if I know all the prime numbers up to
            the square root of the maximum possible number in that row, I can use a prime seive to determine which numbers are prime. For 
            example, the first number greater than or equal to 100 that is divisible by 7 is 105. Thus, I know every 7th number starting
            with 105 is not a prime number. So I create an array of booleans and mark true all the numbers that are composites of some 
            prime number. If I'm trying to find S(x), then I do this for S(x - 2), S(x - 1), S(x), S(x + 1), and S(x + 2).

            Now it's just simply a matter of finding triplets. To make this easier, I append some blank values at the end of the smaller
            sets to I don't have to run a bunch of length checks. Loop through the entire row looking for prime numbers, then test if that
            number is in a prime triple, and if not test if any of its neighbors are.
         */

        public override string ProblemName {
            get { return "196: Prime triplets"; }
        }

        public override string GetAnswer() {
            _primes = new PrimeSieve(10000000);
            var x = S(5678027);
            var y = S(7208785);
            return (x + y).ToString();
        }

        private ulong S(ulong row) {
            ulong sum = 0;
            ulong num = (row * row / 2) - (row / 2) + 1;
            List<bool[]> sets = new List<bool[]>();
            sets.Add(GetPrimes(row - 2, 2));
            sets.Add(GetPrimes(row - 1, 1));
            sets.Add(GetPrimes(row, 0));
            sets.Add(GetPrimes(row + 1, 0));
            sets.Add(GetPrimes(row + 2, 0));
            for (int index = 0; index < sets[2].Count(); index++) {
                if (!sets[2][index]) {
                    bool isPrimeTriplet = false;
                    isPrimeTriplet = IsTriplet(index, sets[1], sets[2], sets[3]);
                    if (!isPrimeTriplet && index > 0 && !sets[2][index - 1]) {
                        isPrimeTriplet = IsTriplet(index - 1, sets[1], sets[2], sets[3]);
                    }
                    if (!isPrimeTriplet && !sets[2][index + 1]) {
                        isPrimeTriplet = IsTriplet(index + 1, sets[1], sets[2], sets[3]);
                    }
                    if (!isPrimeTriplet && !sets[1][index]) {
                        isPrimeTriplet = IsTriplet(index, sets[0], sets[1], sets[2]);
                    }
                    if (!isPrimeTriplet && index > 0 && !sets[1][index - 1]) {
                        isPrimeTriplet = IsTriplet(index - 1, sets[0], sets[1], sets[2]);
                    }
                    if (!isPrimeTriplet && !sets[1][index + 1]) {
                        isPrimeTriplet = IsTriplet(index + 1, sets[0], sets[1], sets[2]);
                    }
                    if (!isPrimeTriplet && !sets[3][index]) {
                        isPrimeTriplet = IsTriplet(index, sets[2], sets[3], sets[4]);
                    }
                    if (!isPrimeTriplet && index > 0 && !sets[3][index - 1]) {
                        isPrimeTriplet = IsTriplet(index - 1, sets[2], sets[3], sets[4]);
                    }
                    if (!isPrimeTriplet && !sets[3][index + 1]) {
                        isPrimeTriplet = IsTriplet(index + 1, sets[2], sets[3], sets[4]);
                    }
                    if (isPrimeTriplet) {
                        sum += num + (ulong)index;
                    }
                }
            }
            return sum;
        }

        private bool IsTriplet(int index, bool[] setPrior, bool[] setCurrent, bool[] setNext) {
            int count = 0;
            if (index > 0) {
                count += (!setPrior[index - 1] ? 1 : 0);
                count += (!setCurrent[index - 1] ? 1 : 0);
                count += (!setNext[index - 1] ? 1 : 0);
            }
            count += (!setPrior[index] ? 1 : 0);
            count += (!setNext[index] ? 1 : 0);
            count += (!setPrior[index + 1] ? 1 : 0);
            count += (!setCurrent[index + 1] ? 1 : 0);
            count += (!setNext[index + 1] ? 1 : 0);
            return count >= 2;
        }

        private bool[] GetPrimes(ulong row, int blanksToAdd) {
            ulong num = (row * row / 2) - (row / 2) + 1;
            ulong max = num + row - 1;
            ulong maxPrime = (ulong)Math.Sqrt(max);
            var isNotPrime = new bool[(int)row + blanksToAdd];
            foreach (var prime in _primes.Enumerate) {
                if (prime > maxPrime) {
                    break;
                }
                ulong start = num;
                ulong mod = start % prime;
                if (mod != 0) {
                    start += prime - mod;
                }
                for (ulong next = start - num; next < row; next += prime) {
                    isNotPrime[(int)next] = true;
                }
            }
            for (int index = (int)row - 1; index < isNotPrime.Count(); index++) {
                isNotPrime[index] = true;
            }
            return isNotPrime;
        }
    }
}
