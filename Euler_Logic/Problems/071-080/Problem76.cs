using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem76 : ProblemBase {
        private int[][][] _data;

        public override string ProblemName {
            get { return "76: Counting Summations"; }
        }

        public override string GetAnswer() {
            int max = 100;
            BuildSummations(max);
            return (_data[max][max][max] - 1).ToString();
        }

        private void BuildSummations(int max) {
            _data = GetArray(max);
            for (int fill = 1; fill <= max; fill++) {
                for (int count = 0; count <= max; count++) {
                    for (int weight = 1; weight <= max; weight++) {
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

        private int[][][] GetArray(int max) {
            int[][][] data = new int[max + 1][][];
            for (int a = 0; a <= max; a++) {
                data[a] = new int[max + 1][];
                for (int b = 0; b <= max; b++) {
                    data[a][b] = new int[max + 1];
                }
            }
            return data;
        }
    }
}
