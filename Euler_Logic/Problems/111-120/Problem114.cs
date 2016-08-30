using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem114 : IProblem {
        private ulong[] _data;

        public string ProblemName {
            get { return "114: Counting block combinations I"; }
        }

        public string GetAnswer() {
            ulong minLength = 3;
            ulong maxUnits = 7;
            _data = new ulong[maxUnits + 1];
            return Solve(minLength, maxUnits).ToString();
        }

        private ulong Solve(ulong minLength, ulong maxUnits) {
            ulong sum = 0;
            for (ulong length = minLength; length <= maxUnits; length++) {
                
            }
            return 0;
        }


        //private List<ulong[]> _data = new List<ulong[]>();
        //private ulong _blockMinLength;
        //private ulong _maxUnitLength;

        //public string ProblemName {
        //    get { return "114: Counting block combinations I"; }
        //}

        //public string GetAnswer() {
        //    Initialize(7, 3);
        //    return Solve().ToString();
        //}

        //private void Initialize(ulong unitLength, ulong blockMinLength) {
        //    _maxUnitLength = unitLength;
        //    _blockMinLength = blockMinLength;
        //    _data.Add(new ulong[_maxUnitLength + 1]);
        //}

        //private ulong Solve() {
        //    ulong sum = 0;
        //    int blockCount = 1;
        //    do {
        //        _data.Add(new ulong[_maxUnitLength + 1]);
        //        for (ulong unitLength = _blockMinLength; unitLength <= _maxUnitLength; unitLength++) {
        //            for (ulong blockLength = _blockMinLength; blockLength <= unitLength; blockLength++) {
        //                for (ulong position = 0; position < unitLength - blockLength; position++) {
        //                    _data[blockCount][unitLength] += _data[blockCount - 1][unitLength - blockLength - 1];
        //                }
        //                _data[blockCount][unitLength - blockLength] += 1;
        //            }
        //        }
        //        blockCount += 1;
        //    } while ((ulong)blockCount * _blockMinLength + (ulong)blockCount - 1 <= _maxUnitLength);
        //    return sum + 1;
        //}
    }
}
