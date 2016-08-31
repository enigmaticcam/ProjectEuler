using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem116 : IProblem {
        private ulong[] _data;

        public string ProblemName {
            get { return "116: Red, green or blue tiles"; }
        }

        public string GetAnswer() {
            return Solve(50).ToString();
        }

        private ulong Solve(ulong maxUnits) {
            ulong sum = 0;
            ulong minLength = 2;
            _data = new ulong[maxUnits + 1];
            BuildData(minLength, maxUnits);
            sum += _data[maxUnits];

            minLength = 3;
            _data = new ulong[maxUnits + 1];
            BuildData(minLength, maxUnits);
            sum += _data[maxUnits];

            minLength = 4;
            _data = new ulong[maxUnits + 1];
            BuildData(minLength, maxUnits);
            sum += _data[maxUnits];
            return sum;
        }

        private void BuildData(ulong blockLength, ulong maxUnits) {
            for (ulong length = blockLength; length <= maxUnits; length++) {
                for (ulong position = 0; position < length - blockLength; position++) {
                    _data[length] += 1 + _data[length - blockLength - position];
                }
                _data[length] += 1;
            }
        }
    }
}
