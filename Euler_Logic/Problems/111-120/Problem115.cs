using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem115 : IProblem {
        private List<int> _data = new List<int>();

        public string ProblemName {
            get { return "115: Counting block combinations II"; }
        }

        public string GetAnswer() {
            Initialize();
            return Solve().ToString();
        }

        private void Initialize() {
            for (int count = 0; count < 50; count++) {
                _data.Add(0);
            }
        }

        private int Solve() {
            int length = 49;
            do {
                _data.Add(0);
                length++;
                BuildData(50, length);
            } while (_data[_data.Count - 1] < 1000000);
            return length;
        }

        private void BuildData(int minLength, int length) {
            for (int blockLength = minLength; blockLength <= length; blockLength++) {
                for (int position = 0; position < length - blockLength; position++) {
                    _data[length] += 1 + _data[length - blockLength - position - 1];
                }
                _data[length] += 1;
            }
        }
    }
}
