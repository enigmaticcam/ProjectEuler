using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem31 : ProblemBase {
        private List<int> _coinTypes = new List<int>();
        private int[][] _sums;

        public override string ProblemName {
            get { return "31: Coin Sums"; }
        }

        public override string GetAnswer() {
            CreateCoinTypes();
            _sums = new int[_coinTypes.Count + 1][];
            return CoinSums(100).ToString();
        }

        private int CoinSums(int top) {
            _sums[0] = new int[top];
            for (int coin = 0; coin < _coinTypes.Count; coin++) {
                _sums[coin + 1] = new int[top];
                for (int max = 1; max <= top; max++) {
                    int weight = 0;
                    while (weight <= max) {
                        if (weight == max) {
                            _sums[coin + 1][max - 1] += 1;
                        } else {
                            _sums[coin + 1][max - 1] += _sums[coin][max - weight - 1];
                        }
                        weight += _coinTypes[coin];
                    }
                }
            }
            return _sums[_coinTypes.Count][top - 1];
        }

        private void CreateCoinTypes() {
            _coinTypes.Add(1);
            _coinTypes.Add(5);
            _coinTypes.Add(10);
            _coinTypes.Add(25);
            _coinTypes.Add(50);
            _coinTypes.Add(100);
        }
    }
}
