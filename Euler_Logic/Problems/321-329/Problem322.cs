using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem322 : ProblemBase {
        public override string ProblemName {
            get { return "322: Binomial coefficients divisible by 10"; }
        }

        public override string GetAnswer() {
            //BruteForce((ulong)Math.Pow(10, 3), (ulong)Math.Pow(10, 2) - 10
            //return BruteForce((ulong)Math.Pow(10, 9), (ulong)Math.Pow(10, 7) - 10).ToString();


            //Solve1((ulong)Math.Pow(10, 9), (ulong)Math.Pow(10, 7) - 10);
            //Solve1((ulong)Math.Pow(10, 18), (ulong)Math.Pow(10, 12) - 10);
            return "";
        }

        //private void Solve1(ulong m, ulong y) {
        //    var base2 = GetBase(y, 2);
        //    var base5 = GetBase(y, 5);
        //    var top = GetBase(m, 5);
        //    for (ulong x = y; x < m; x++) {
        //        bool stop = true;
        //    }
        //}

        //private ulong BruteForce(ulong m, ulong y) {
        //    ulong count = 0;
        //    var top5 = GetBase(y, 5);
        //    for (ulong x = y; x < m; x++) {
        //        if (x - y == 100) {
        //            bool stop = true;
        //        }
        //        bool isGood = false;
        //        var root5 = GetBase(y, 5);
        //        if (CompareDigits(top5, root5)) {
        //            var top2 = GetBase(x, 2);
        //            var root2 = GetBase(y, 2);
        //            if (CompareDigits(top2, root2)) {
        //                isGood = true;
        //            }
        //        }
        //        if (isGood) {
        //            count++;
        //        }
        //    }
        //    return count;
        //}

        //private void Add(ulong[] digits, ulong numToAdd, ulong root, int index) {
        //    digits[index] += numToAdd;
            
        //}

        //private bool CompareDigits(ulong[] top, ulong[] bottom) {
        //    for (int index = 0; index < bottom.Length; index++) {
        //        if (bottom[index] > top[index]) {
        //            return true;
        //        }
        //    }
        //    return false;
        //}

        //private void GetBase(ulong num, ulong root, ulong[] digits) {
        //    var count = (ulong)Math.Log(num, root);
        //    var power = (ulong)Math.Pow(root, count);
        //    int index = 0;
        //    for (long digit = (long)count; digit >= 0; digit--) {
        //        digits[index] = num / power;
        //        num -= digits[index] * power;
        //        power /= root;
        //        index++;
        //    }
        //}
    }
}
