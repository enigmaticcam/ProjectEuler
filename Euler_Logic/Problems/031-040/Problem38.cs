using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem38 : ProblemBase {
        private Dictionary<string, int> _textToBits = new Dictionary<string, int>();
        private Dictionary<string, bool> _isTextGood = new Dictionary<string, bool>();
        private int _answer = 1022;

        public override string ProblemName {
            get { return "38: Pandigital multiples"; }
        }

        public override string GetAnswer() {
            BuildInitialBits();
            return GetLargestPandigital();
        }

        private string GetLargestPandigital() {
            string best = "";
            for (int i = 2; i <= 10000; i++) {
                string pandigital = GetPandigital(i);
                if (pandigital.Length > 0 && string.Compare(pandigital, best) > 0) {
                    best = pandigital;
                }
            }
            return best;
        }

        private string GetPandigital(int num) {
            int factor = 1;
            int bits = 0;
            string answer = "";
            while (true) {
                int result = num * factor;
                if (result > 987654321) {
                    return "";
                }
                string resultAsString = result.ToString();
                answer += resultAsString;
                if (IsGood(resultAsString) && (_textToBits[resultAsString] & bits) == 0) {
                    bits += _textToBits[resultAsString];
                } else {
                    return "";
                }
                if (bits == _answer) {
                    return answer;
                }
                factor++;
            }
        }

        private bool IsGood(string num) {
            if (!_isTextGood.ContainsKey(num)) {
                bool isGood = true;
                int bits = 0;
                for (int index = 0; index < num.Length; index++) {
                    string digit = num.Substring(index, 1);
                    if ((_textToBits[digit] & bits) == _textToBits[digit]) {
                        isGood = false;
                    } else {
                        bits += _textToBits[digit];
                    }
                }
                _isTextGood.Add(num, isGood);
                if (isGood) {
                    _textToBits.Add(num, bits);
                }
            }
            return _isTextGood[num];
        }

        private void BuildInitialBits() {
            for (int digit = 0; digit <= 9; digit++) {
                _textToBits.Add(digit.ToString(), Convert.ToInt32(Math.Pow(2, digit)));
                _isTextGood.Add(digit.ToString(), true);
            }
        }
    }
}
