using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem78 : IProblem {
        private List<BigInteger> _primes = new List<BigInteger>();
        private List<Dictionary<BigInteger, BigInteger>> _primeCounts = new List<Dictionary<BigInteger, BigInteger>>();

        public string ProblemName {
            get { return "78: Coin Partitions"; }
        }

        public string GetAnswer() {
            _primeCounts.Add(new Dictionary<BigInteger, BigInteger>());
            return CountPiles().ToString();
        }

       private BigInteger CountPiles() {
            BigInteger num = 1;
            do {
                for (int primeIndex = 0; primeIndex < _primes.Count; primeIndex++) {
                    BuildSums(num, primeIndex, num);
                }
                _primes.Add(num);
                _primeCounts.Add(new Dictionary<BigInteger, BigInteger>());
                BuildSums(1, _primes.Count - 1, num);
                BigInteger count = GetCount(_primes.Count - 1, num);
                if (count != 0 && count % 1000000 == 0) {
                    return num;
                }
                num++;
            } while (true);
        }

       private void BuildSums(BigInteger weight, int primeIndex, BigInteger num) {
           for (BigInteger weightIndex = weight; weightIndex <= num; weightIndex++) {
               BigInteger tempWeight = 0;
                while (tempWeight <= weightIndex) {
                    BigInteger count = GetCount(primeIndex, weightIndex);
                    if (tempWeight == weightIndex) {
                        SetCount(primeIndex, weightIndex, count + 1);
                    } else {
                        SetCount(primeIndex, weightIndex, count + GetCount(primeIndex - 1, weightIndex - tempWeight));
                    }
                    tempWeight += _primes[primeIndex];
                }
            }
        }

       private BigInteger GetCount(int primeIndex, BigInteger weight) {
            primeIndex += 1;
            if (!_primeCounts[primeIndex].ContainsKey(weight)) {
                _primeCounts[primeIndex].Add(weight, 0);
            }
            return _primeCounts[primeIndex][weight];
        }

       private void SetCount(int primeIndex, BigInteger weight, BigInteger value) {
            primeIndex += 1;
            if (!_primeCounts[primeIndex].ContainsKey(weight)) {
                _primeCounts[primeIndex].Add(weight, value);
            } else {
                _primeCounts[primeIndex][weight] = value;
            }
        }
    }
}
