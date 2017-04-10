using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem114 : ProblemBase {
        private ulong[] _data;

        public override string ProblemName {
            get { return "114: Counting block combinations I"; }
        }

        public override string GetAnswer() {
            ulong minLength = 3;
            ulong maxUnits = 50;
            _data = new ulong[maxUnits + 1];
            Solve(minLength, maxUnits);
            return _data[maxUnits].ToString();
        }

        private void Solve(ulong minLength, ulong maxUnits) {
            for (ulong length = minLength; length <= maxUnits; length++) {
                for (ulong blockLength = minLength; blockLength <= length; blockLength++) {
                    for (ulong position = 0; position < length - blockLength; position++) {
                        _data[length] += 1 + _data[length - blockLength - position - 1];
                    }
                    _data[length] += 1;
                }
            }
            _data[maxUnits] += 1;
        }
    }
}
