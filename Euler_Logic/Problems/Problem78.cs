using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem78 : IProblem {
        private UInt64[][][] _data;

        public string ProblemName {
            get { return "78: Coin Partitions"; }
        }

        public string GetAnswer() {
            UInt64 max = 1;
            do {
                max++;
                BuildSummations(max);
                if (_data[max][max][max] < 0) {
                    throw new Exception("uh oh");
                }
            } while (_data[max][max][max] % 1000000 != 0);
            return max.ToString();
        }

        private void BuildSummations(UInt64 max) {
            _data = GetArray(max);
            for (UInt64 fill = 1; fill <= max; fill++) {
                for (UInt64 count = 0; count <= max; count++) {
                    for (UInt64 weight = 1; weight <= max; weight++) {
                        if (count == 0) {
                            _data[fill][count][weight] = _data[fill - 1][max][weight];
                        } else {
                            if (fill * count <= weight) {
                                _data[fill][count][weight] = _data[fill][count - 1][weight] + _data[fill - 1][max][weight - (fill * count)];
                                if (weight - (fill * count) == 0) {
                                    _data[fill][count][weight]++;
                                }
                            } else {
                                _data[fill][count][weight] = _data[fill][count - 1][weight];
                            }
                        }
                    }
                }
            }
        }

        private UInt64[][][] GetArray(UInt64 max) {
            UInt64[][][] data = new UInt64[max + 1][][];
            for (UInt64 a = 0; a <= max; a++) {
                data[a] = new UInt64[max + 1][];
                for (UInt64 b = 0; b <= max; b++) {
                    data[a][b] = new UInt64[max + 1];
                }
            }
            return data;
        }
    }
}
