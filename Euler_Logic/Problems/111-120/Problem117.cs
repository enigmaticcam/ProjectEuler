using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem117 : ProblemBase {
        private List<ulong> _blocks = new List<ulong>();
        private ulong[] _data;

        public override string ProblemName {
            get { return "117: Red, green, and blue tiles"; }
        }

        public override string GetAnswer() {
            BuildBlocks();
            return Solve(50).ToString();
        }

        private void BuildBlocks() {
            _blocks.Add(2);
            _blocks.Add(3);
            _blocks.Add(4);
        }

        private ulong Solve(ulong maxLength) {
            _data = new ulong[maxLength + 1];
            for (ulong length = 1; length <= maxLength; length++) {
                for (int blockIndex = 0; blockIndex < _blocks.Count; blockIndex++) {
                    if (_blocks[blockIndex] <= length) {
                        for (ulong position = 0; position < length - _blocks[blockIndex]; position++) {
                            _data[length] += 1 + _data[length - _blocks[blockIndex] - position];
                        }
                        _data[length] += 1;
                    }
                }
            }
            return _data[maxLength] + 1;
        }
    }
}
