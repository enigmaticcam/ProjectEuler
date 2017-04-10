using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem18 : ProblemBase {
        private ulong[][] _nums;
        List<List<ulong>> _paths = new List<List<ulong>>();

        public override string ProblemName {
            get { return "18: Maximum path sum I"; }
        }

        public override string GetAnswer() {
            //BuildTestNumbers();
            BuildRealNumbers();
            BuildPaths();
            return GetMaxPath().ToString();
        }

        private void BuildPaths() {
            for (int a = 0; a <= _nums.GetUpperBound(0); a++) {
                _paths.Add(new List<ulong>());
                for (int b = 0; b <= _nums[a].GetUpperBound(0); b++ ) {
                    if (a == 0) {
                        _paths[a].Add(_nums[a][b]);
                    } else if (b == 0) {
                        _paths[a].Add(_paths[a - 1][0] + _nums[a][b]);
                    } else if (b == _nums[a].GetUpperBound(0)) {
                        _paths[a].Add(_paths[a - 1][b - 1] + _nums[a][b]);
                    } else if (_paths[a - 1][b - 1] > _paths[a - 1][b]) {
                        _paths[a].Add(_paths[a - 1][b - 1] + _nums[a][b]);
                    } else {
                        _paths[a].Add(_paths[a - 1][b] + _nums[a][b]);
                    }
                }
            }
        }

        private ulong GetMaxPath() {
            ulong best = 0;
            int max = _paths.Count - 1;
            for (int a = 0; a < _paths[max].Count; a++) {
                if (_paths[max][a] > best) {
                    best = _paths[max][a];
                }
            }
            return best;
        }

        private void BuildTestNumbers() {
            _nums = new ulong[4][];
            _nums[0] = "3".Split(' ').Select(ulong.Parse).ToList().ToArray();
            _nums[1] = "7 4".Split(' ').Select(ulong.Parse).ToList().ToArray();
            _nums[2] = "2 4 6".Split(' ').Select(ulong.Parse).ToList().ToArray();
            _nums[3] = "8 5 9 3".Split(' ').Select(ulong.Parse).ToList().ToArray();
        }

        private void BuildRealNumbers() {
            _nums = new ulong[15][];
            _nums[0] = "75".Split(' ').Select(ulong.Parse).ToList().ToArray();
            _nums[1] = "95 64".Split(' ').Select(ulong.Parse).ToList().ToArray();
            _nums[2] = "17 47 82".Split(' ').Select(ulong.Parse).ToList().ToArray();
            _nums[3] = "18 35 87 10".Split(' ').Select(ulong.Parse).ToList().ToArray();
            _nums[4] = "20 04 82 47 65".Split(' ').Select(ulong.Parse).ToList().ToArray();
            _nums[5] = "19 01 23 75 03 34".Split(' ').Select(ulong.Parse).ToList().ToArray();
            _nums[6] = "88 02 77 73 07 63 67".Split(' ').Select(ulong.Parse).ToList().ToArray();
            _nums[7] = "99 65 04 28 06 16 70 92".Split(' ').Select(ulong.Parse).ToList().ToArray();
            _nums[8] = "41 41 26 56 83 40 80 70 33".Split(' ').Select(ulong.Parse).ToList().ToArray();
            _nums[9] = "41 48 72 33 47 32 37 16 94 29".Split(' ').Select(ulong.Parse).ToList().ToArray();
            _nums[10] = "53 71 44 65 25 43 91 52 97 51 14".Split(' ').Select(ulong.Parse).ToList().ToArray();
            _nums[11] = "70 11 33 28 77 73 17 78 39 68 17 57".Split(' ').Select(ulong.Parse).ToList().ToArray();
            _nums[12] = "91 71 52 38 17 14 91 43 58 50 27 29 48".Split(' ').Select(ulong.Parse).ToList().ToArray();
            _nums[13] = "63 66 04 68 89 53 67 30 73 16 69 87 40 31".Split(' ').Select(ulong.Parse).ToList().ToArray();
            _nums[14] = "04 62 98 27 23 09 70 98 73 93 38 53 60 04 23".Split(' ').Select(ulong.Parse).ToList().ToArray();
        }
    }
}
