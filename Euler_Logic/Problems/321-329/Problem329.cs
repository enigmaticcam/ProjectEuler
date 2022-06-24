using Euler_Logic.Helpers;
using System;
using System.Collections.Generic;

namespace Euler_Logic.Problems {
    public class Problem329 : ProblemBase {
        private PrimeSieveSimple _primes = new PrimeSieveSimple();
        private LCMULong _lcm = new LCMULong();
        private Dictionary<bool, ulong[]> _numerators = new Dictionary<bool, ulong[]>();
        private Dictionary<bool, ulong[]> _denominators = new Dictionary<bool, ulong[]>();
        private bool _lastSet;

        /*
            A simple dynamic programing algorithm will solve this. The chances of getting "P" on a 
            single square is of course 2/3 if it's prime or 1/3 if it's not. The chances therefore
            of getting "P" on the first croak is the sum of getting "P" on each individual square 
            divided by 500. For the second iteration, we take the probabily of arriving at each 
            square after having croaked a "P" and then multiply that by the chances of croaking
            another "P". The chances of arriving at each square is 50%  ofthe result of croaking
            a "P" from square - 1 plus 50% of the result of croaking a "P" from square + 1. Small 
            adjustments are made for the first and last two squares. Then just multiple that by 
            the chances of croaking another "P". Do this for all iterations, sum the results,
            and simplify.

            I use only two sets of fractions since at any one time we only care about the current
            set and the previous set.
         */

        public override string ProblemName {
            get { return "329: Prime Frog"; }
        }

        public override string GetAnswer() {
            _primes.SievePrimes(500);
            Initialize();
            Solve("PPPPNNPPPNPPNPN");
            return GetSum();
        }

        private void Initialize() {
            _numerators.Add(true, new ulong[500]);
            _denominators.Add(true, new ulong[500]);
            _numerators.Add(false, new ulong[500]);
            _denominators.Add(false, new ulong[500]);
            for (int index = 0; index < 500; index++) {
                _numerators[true][index] = 1;
                _numerators[false][index] = 1;
                _denominators[true][index] = 1;
                _denominators[false][index] = 1;
            }
        }

        private void Solve(string sequence) {
            bool firstTime = true;
            bool set = false;
            for (int charIndex = 0; charIndex < sequence.Length; charIndex++) {
                string digit = sequence.Substring(charIndex, 1);
                for (int spot = 1; spot <= 500; spot++) {

                    // Set precalculated for the first round
                    if (firstTime) {
                        if (_primes.IsPrime(spot)) {
                            _denominators[!set][spot - 1] = (digit == "P" ? (ulong)750 : 1500);
                        } else {
                            _denominators[!set][spot - 1] = (digit == "P" ? (ulong)1500 : 750);
                        }
                    } else {
                        ulong numerator = 0;
                        ulong denominator = 1;

                        if (spot == 2) {
                            // 100% chance of landing on spot 2 from spot 1
                            numerator = _numerators[set][spot - 2];
                            denominator = _denominators[set][spot - 2];
                        } else if (spot > 2) {
                            // 50% chance of landing on spot x from spot x - 1
                            numerator = _numerators[set][spot - 2];
                            denominator = _denominators[set][spot - 2] * 2;
                        }
                        if (spot == 499) {
                            // 100% chance of landing on spot 499 from 500. Add to previous fraction
                            ulong lcm = _lcm.LCM(denominator, _denominators[set][spot]);
                            numerator = ((lcm / denominator) * numerator) + ((lcm / _denominators[set][spot]) * _numerators[set][spot]);
                            denominator = lcm;
                        } else if (spot < 499) {
                            // 50% chance of landing on spot x form spot x + 1. Add to previous fraction
                            ulong lcm = _lcm.LCM(denominator, _denominators[set][spot] * 2);
                            numerator = ((lcm / denominator) * numerator) + ((lcm / (_denominators[set][spot] * 2)) * _numerators[set][spot]);
                            denominator = lcm;
                        }

                        // Multiple the chance of getting to this spot by the chance of hitting the next target
                        if (_primes.IsPrime(spot)) {
                            numerator *= (digit == "P" ? (ulong)2 : 1);
                        } else {
                            numerator *= (digit == "P" ? (ulong)1 : 2);
                        }
                        denominator *= 3;

                        // Simplify the fraction
                        ulong gcd = GCDULong.GCD(numerator, denominator);
                        numerator /= gcd;
                        denominator /= gcd;
                        _numerators[!set][spot - 1] = numerator;
                        _denominators[!set][spot - 1] = denominator;
                    }
                }
                firstTime = false;
                set = !set;
            }
            _lastSet = set;
        }

        private string GetSum() {
            ulong numerator = _numerators[_lastSet][0];
            ulong denominator = _denominators[_lastSet][0];
            for (int spot = 1; spot < 500; spot++) {
                ulong lcm = _lcm.LCM(denominator, _denominators[_lastSet][spot]);
                numerator = ((lcm / denominator) * numerator) + ((lcm / _denominators[_lastSet][spot]) * _numerators[_lastSet][spot]);
                denominator = lcm;
            }

            ulong gcd = GCDULong.GCD(numerator, denominator);
            numerator /= gcd;
            denominator /= gcd;
            return numerator.ToString() + "/" + denominator.ToString();
        }
    }
}