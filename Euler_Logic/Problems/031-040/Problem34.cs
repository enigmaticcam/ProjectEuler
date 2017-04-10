using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem34 : ProblemBase {
        private Dictionary<string, decimal> _factorials;

        public override string ProblemName {
            get { return "34: Digit factorials"; }
        }

        public override string GetAnswer() {
            GenerateFactorials();
            return GetDigitFactSum().ToString();
        }

        private decimal GetDigitFactSum() {
            decimal sum = 0;
            decimal num = 3;
            while (num <= 9999999) {
                string numAsString = num.ToString();
                decimal factorial = 0;
                for (int index = 0; index < numAsString.Length; index++) {
                    factorial += _factorials[numAsString.Substring(index, 1)];
                }
                if (factorial == num) {
                    sum += factorial;
                }
                num++;
            }
            return sum;
        }

        private decimal GetFactorial(decimal num) {
            decimal sum = num;
            for (decimal a = num - 1; a > 1; a--) {
                sum *= a;
            }
            return sum;
        }

        private void GenerateFactorials() {
            _factorials = new Dictionary<string, decimal>();
            _factorials.Add("0", 1);
            for (decimal num = 1; num <= 9; num++) {
                _factorials.Add(num.ToString(), GetFactorial(num));
            }
        }
    }
}
