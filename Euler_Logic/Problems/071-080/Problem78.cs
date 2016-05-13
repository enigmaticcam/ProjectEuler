using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem78 : IProblem {
        private List<int> _primes = new List<int>();
        private List<Dictionary<int, int>> _primeCounts = new List<Dictionary<int, int>>();

        public string ProblemName {
            get { return "78: Coin Partitions"; }
        }

        public string GetAnswer() {
            _primeCounts.Add(new Dictionary<int, int>());
            return CountPiles().ToString();
        }

       private int CountPiles() {
            int num = 1;
            do {
                for (int primeIndex = 0; primeIndex < _primes.Count; primeIndex++) {
                    BuildSums(num, primeIndex, num);
                }
                _primes.Add(num);
                _primeCounts.Add(new Dictionary<int, int>());
                BuildSums(1, _primes.Count - 1, num);
                int count = GetCount(_primes.Count - 1, num);
                if (count == 0) {
                    return num;
                }
                num++;
            } while (true);
        }

       private void BuildSums(int weight, int primeIndex, int num) {
           for (int weightIndex = weight; weightIndex <= num; weightIndex++) {
               int tempWeight = 0;
                while (tempWeight <= weightIndex) {
                    int count = GetCount(primeIndex, weightIndex);
                    if (tempWeight == weightIndex) {
                        SetCount(primeIndex, weightIndex, count + 1);
                    } else {
                        SetCount(primeIndex, weightIndex, count + GetCount(primeIndex - 1, weightIndex - tempWeight));
                    }
                    tempWeight += _primes[primeIndex];
                }
            }
        }

       private int GetCount(int primeIndex, int weight) {
            primeIndex += 1;
            if (!_primeCounts[primeIndex].ContainsKey(weight)) {
                _primeCounts[primeIndex].Add(weight, 0);
            }
            return _primeCounts[primeIndex][weight];
        }

       private void SetCount(int primeIndex, int weight, int value) {
            primeIndex += 1;
            if (!_primeCounts[primeIndex].ContainsKey(weight)) {
                _primeCounts[primeIndex].Add(weight, value % 1000000);
            } else {
                _primeCounts[primeIndex][weight] = value % 1000000;
            }
        }
    }
}
