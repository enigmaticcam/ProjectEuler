using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem48 : ProblemBase {
        

        public override string ProblemName {
            get { return "48: Self powers"; }
        }

        public override string GetAnswer() {
            return StripCharacters(CalcAllSelfPowers(1000)).ToString();
        }

        public decimal CalcAllSelfPowers(decimal max) {
            decimal sum = 0;
            for (int index = 1; index <= max; index++) {
                sum += CalcSelfPower(index);
            }
            return sum;
        }

        private decimal CalcSelfPower(decimal max) {
            decimal num = max;
            for (decimal index = 2; index <= max; index++) {
                num *= max;
                num = StripCharacters(num);
            }
            return num;
        }

        private decimal Multiply(decimal a, decimal b) {
            decimal sum = a;
            for (int count = 1; count < b; count++) {
                sum += a;
                sum = StripCharacters(sum);
            }
            return sum;
        }

        private decimal StripCharacters(decimal num) {
            if (num < 10000000000) {
                return num;
            } else {
                string text = num.ToString();
                decimal power = 0;
                for (int index = 1; index <= 10; index++) {
                    power += Convert.ToDecimal(text.Substring(text.Length - index, 1)) * (decimal)Math.Pow(10, index - 1);
                }
                string powerAsString = power.ToString();
                if (powerAsString.Length > 10) {
                    return Convert.ToInt32(powerAsString.Substring(powerAsString.Length - 10, 10));
                } else {
                    return power;
                }
            }
        }
    }
}
