using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem8 : IProblem {
        private StringBuilder _series = new StringBuilder();

        public string ProblemName {
            get { return "8: Largest product in a series"; }
        }

        public string GetAnswer() {
            BuildSeries();
            return FindProduct(13).ToString();
        }

        private void BuildSeries() {
            _series.Append("73167176531330624919225119674426574742355349194934");
            _series.Append("96983520312774506326239578318016984801869478851843");
            _series.Append("85861560789112949495459501737958331952853208805511");
            _series.Append("12540698747158523863050715693290963295227443043557");
            _series.Append("66896648950445244523161731856403098711121722383113");
            _series.Append("62229893423380308135336276614282806444486645238749");
            _series.Append("30358907296290491560440772390713810515859307960866");
            _series.Append("70172427121883998797908792274921901699720888093776");
            _series.Append("65727333001053367881220235421809751254540594752243");
            _series.Append("65727333001053367881220235421809751254540594752243");
            _series.Append("53697817977846174064955149290862569321978468622482");
            _series.Append("53697817977846174064955149290862569321978468622482");
            _series.Append("82166370484403199890008895243450658541227588666881");
            _series.Append("16427171479924442928230863465674813919123162824586");
            _series.Append("17866458359124566529476545682848912883142607690042");
            _series.Append("24219022671055626321111109370544217506941658960408");
            _series.Append("07198403850962455444362981230987879927244284909188");
            _series.Append("84580156166097919133875499200524063689912560717606");
            _series.Append("05886116467109405077541002256983155200055935729725");
            _series.Append("71636269561882670428252483600823257530420752963450");
        }

        private decimal FindProduct(decimal digits) {
            string series = _series.ToString();
            decimal best = 0;
            for (int i = 0; i <= series.Length - digits - 1; i++) {
                decimal product = 1;
                for (int nums = 0; nums < digits; nums++) {
                    product *= Convert.ToDecimal(series.Substring(i + nums, 1));                    
                }
                if (product > best) {
                    best = product;
                }
            }
            return best;
        }
    }
}
