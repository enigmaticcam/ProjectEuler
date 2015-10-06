using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem79 : IProblem {
        private List<string> _codes = new List<string>();

        public string ProblemName {
            get { return "79: Passcode derivation"; }
        }

        public string GetAnswer() {
            LoadCodes();
            return DeducePasscode();
        }

        private string DeducePasscode() {
            ulong num = 1000;
            do {
                string text = num.ToString();
                bool isGood = false;
                foreach (string code in _codes) {
                    int position = 0;
                    isGood = true;
                    for (int index = 0; index < code.Length; index++) {
                        int indexOf = text.IndexOf(code.Substring(index, 1), position);
                        if (indexOf != -1) {
                            position = indexOf + 1;
                        } else {
                            isGood = false;
                            break;
                        }
                    }
                    if (!isGood) {
                        break;
                    }
                }
                if (isGood) {
                    return num.ToString();
                }
                num++;
            } while (true);
        }

        private void LoadCodes() {
            _codes.Add("319");
            _codes.Add("680");
            _codes.Add("180");
            _codes.Add("690");
            _codes.Add("129");
            _codes.Add("620");
            _codes.Add("762");
            _codes.Add("689");
            _codes.Add("762");
            _codes.Add("318");
            _codes.Add("368");
            _codes.Add("710");
            _codes.Add("720");
            _codes.Add("710");
            _codes.Add("629");
            _codes.Add("168");
            _codes.Add("160");
            _codes.Add("689");
            _codes.Add("716");
            _codes.Add("731");
            _codes.Add("736");
            _codes.Add("729");
            _codes.Add("316");
            _codes.Add("729");
            _codes.Add("729");
            _codes.Add("710");
            _codes.Add("769");
            _codes.Add("290");
            _codes.Add("719");
            _codes.Add("680");
            _codes.Add("318");
            _codes.Add("389");
            _codes.Add("162");
            _codes.Add("289");
            _codes.Add("162");
            _codes.Add("718");
            _codes.Add("729");
            _codes.Add("319");
            _codes.Add("790");
            _codes.Add("680");
            _codes.Add("890");
            _codes.Add("362");
            _codes.Add("319");
            _codes.Add("760");
            _codes.Add("316");
            _codes.Add("729");
            _codes.Add("380");
            _codes.Add("319");
            _codes.Add("728");
            _codes.Add("716");
        }
    }
}
