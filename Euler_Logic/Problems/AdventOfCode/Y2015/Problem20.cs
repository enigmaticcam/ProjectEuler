using Euler_Logic.Helpers;
using System;
using System.Collections.Generic;

namespace Euler_Logic.Problems.AdventOfCode.Y2015 {
    public class Problem20 : AdventOfCodeBase {
        private PrimeSieve _primes;

        public override string ProblemName => "Advent of Code 2015: 20";

        public override string GetAnswer() {
            return Answer1(Input()).ToString();
        }

        public override string GetAnswer2() {
            return Answer2(Input()).ToString();
        }

        private ulong Answer1(List<string> input) {
            _primes = new PrimeSieve(10000000);
            return FindFirst(36000000);
        }

        private int Answer2(List<string> input) {
            return Simulate2();
        }

        private int Simulate2() {
            var houses = new Dictionary<int, int>();
            int num = 1;
            do {
                for (int next = 0; next < 50; next++) {
                    var nextHouse = num + (num * next);
                    if (!houses.ContainsKey(nextHouse)) {
                        houses.Add(nextHouse, num * 11);
                    } else {
                        houses[nextHouse] += num * 11;
                    }
                }
                if (houses[num] >= 36000000) {
                    return num;
                }
                num++;
            } while (true);
        }

        private ulong FindFirst(ulong numToFind) {
            ulong num = 2;
            do {
                var count = GetCount(num);
                if (count * 10 >= numToFind) {
                    return num;
                }
                num++;
            } while (true);
        }

        private ulong GetCount(ulong num) {
            ulong count = 1;
            ulong maxPrime = (ulong)Math.Sqrt(num);
            foreach (var prime in _primes.Enumerate) {
                if (prime > maxPrime) {
                    break;
                }
                if (num % prime == 0) {
                    ulong subCount = 1;
                    ulong power = prime;
                    do {
                        num /= prime;
                        subCount += power;
                        power *= prime;
                    } while (num % prime == 0);
                    count *= subCount;
                }
                if (_primes.IsPrime(num)) {
                    count *= 1 + num;
                    break;
                }
            }
            return count;
        }
    }
}
