using System;
using System.Collections.Generic;

namespace Euler_Logic.Problems {
    public class Problem500 : ProblemBase {
        private bool[] _notPrimes;
        private List<uint> _primes = new List<uint>();
        private double[] _powers;
        private Dictionary<double, int> _hash = new Dictionary<double, int>();

        public override string ProblemName {
            get { return "500: Problem 500!!!"; }
        }

        /*
            120 has 16 divisors. Obviously you can test this by looping through all numbers 1-120 and counting how many 120 can divide evenly by. Another way however
            is to consider the prime factors of 120: 2^3*3^1*5^1. All combinations will yield all divisors. 2^3 is four possibilities, 3^1 is two, and 5^1 is two.
            4*2*2 = 16. 

            Knowing this then, the maximum amount of prime factors you would need to make any number that has exactly 16 divisors would be 4, 2*2*2*2. Better yet,
            the maximum number of prime factors to make exactly 2^x divisors is x. So first we do a prime sieve to 10^7 for all primes. This gives us around 600k primes,
            which should be enough for 2^500500.

            Once you know how many prime factors you need, then it's simply a matter of which combination yields the lowest number. Suppose we are looking at 16 divisors.
            We could dump them all into one prime. If we dump them all in one prime, certainly we wouldn't dump them in any prime but the lowest. So one option would
            be 2^15 = 16384. Now suppose we use two primes, p1^x * p2^y where (x+1)*(y+1)=16. Obviously we would only use the lowest two primes. And obviously we wouldn't
            put more powers into a higher prime. So the only possibility that works is 2^7 * 3^1 = 384. 3 primes is 2^3 * 3^1 * 4^1 = 120, and 4 primes is
            2^1 * 3^1 * 4^1 * 5^1 = 210. It's clear that 3 primes is best, yielding 120.

            So then, let's start with the lowest number having 2^1 divisors. This is obviously 2^1 = 2; Then let's do 2^2 divisors. Either we double the power for 2, or 
            add prime 3. Which is the smaller? Well, this can be expressed as: 2^(3-1) > 3^1. So we would add the 3, making our new sum 2 * 3^1 = 6. Now let's do 2^3
            divisors. Either we double the power for 2 or we add a new power for 5 (we don't double the power of 3, because it's the same power as 2 and therefore
            we already know it would be higher). This yields the equation: 2^(3-1) < 5^1. So it's better to double the power of 2 instead of using 5. Our new sum then
            is 6 * 2^(3-1) = 24. We continue this logic until we get 2^500500 divisors. We  divide our sum by 50050007 and return the remainder each iteration, otherwise
            the number gets too large.

            To greatly increase the efficiency of this algorithm, we use a dictionary to maintain the number of times a power is used. Remember, if we currently are at
            2^3 * 3^3 * 5^3 * 7^3 * 11^1, after trying to double the power of 2, we don't double the power of 3 because we already know it would be higher than 2 since
            they have the same value. Instead of looping through and checking every subsequent prime (3, 5, & 7), we use the hash to know there are 3 primes that are
            using a power of 3 and we jump straight to 11.
        */

        public override string GetAnswer() {
            SievePrimes(10000000);
            return Solve(500500).ToString();
        }

        private void SievePrimes(uint max) {
            _notPrimes = new bool[max + 1];
            for (uint num = 2; num <= max; num++) {
                if (!_notPrimes[num]) {
                    _primes.Add(num);
                    for (uint composite = 2; composite * num <= max; composite++) {
                        _notPrimes[composite * num] = true;
                    }
                }
            }
            _notPrimes = null;
        }

        private double Solve(int max) {
            double sum = 1;
            _powers = new double[max];
            for (int count = 1; count <= max; count++) {
                double lastNum = -99;
                double lowestPower = double.MaxValue;
                int lowestIndex = 0;
                double increaseAmount = 0;
                int index = 0;
                do {
                    var power = _powers[index];
                    if (power != lastNum) {
                        double diff = 0;
                        if (power == 0) {
                            diff = 1;
                        } else {
                            diff = power + 1;
                        }
                        double diffInPower = Math.Pow(_primes[index], diff);
                        if (diffInPower < lowestPower) {
                            lowestPower = diffInPower;
                            lowestIndex = index;
                            increaseAmount = diff;
                        }
                        lastNum = power;
                    }
                    if (power == 0) {
                        break;
                    } else {
                        index += _hash[power];
                    }
                } while (true);
                AddToHash(lowestIndex, increaseAmount);
                _powers[lowestIndex] += increaseAmount;
                sum = (sum * Math.Pow(_primes[lowestIndex], increaseAmount)) % 500500507;
            }
            return sum;
        }

        private void AddToHash(int index, double increaseAmount) {
            double powerToLose = _powers[index];
            double powerToGain = powerToLose + increaseAmount;
            if (_hash.ContainsKey(powerToLose)) {
                _hash[powerToLose]--;
            }
            if (!_hash.ContainsKey(powerToGain)) {
                _hash.Add(powerToGain, 1);
            } else {
                _hash[powerToGain]++;
            }
        }
    }
}