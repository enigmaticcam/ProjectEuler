using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem17 : ProblemBase {
        private Dictionary<int, int> _numToTextCount = new Dictionary<int, int>();

        public override string ProblemName {
            get { return "17: Number letter counts"; }
        }

        public override string GetAnswer() {
            BuildOneToTen();
            BuildElevenToNineteen();
            BuildTwentyToNinetyNine();
            BuildOneHundredToOneThousand();
            return CountAll().ToString();
        }

        public void BuildOneToTen() {
            _numToTextCount.Add(1, 3); // one
            _numToTextCount.Add(2, 3); // two
            _numToTextCount.Add(3, 5); // three
            _numToTextCount.Add(4, 4); // four
            _numToTextCount.Add(5, 4); // five
            _numToTextCount.Add(6, 3); // six
            _numToTextCount.Add(7, 5); // seven
            _numToTextCount.Add(8, 5); // eight
            _numToTextCount.Add(9, 4); // nine
            _numToTextCount.Add(10, 3); // ten
        }

        public void BuildElevenToNineteen() {
            _numToTextCount.Add(11, 6); // eleven
            _numToTextCount.Add(12, 6); // twelve
            _numToTextCount.Add(13, 8); // thirteen
            _numToTextCount.Add(14, 8); // fourteen
            _numToTextCount.Add(15, 7); // fifteen
            _numToTextCount.Add(16, 7); // sixteen
            _numToTextCount.Add(17, 9); // seventeen
            _numToTextCount.Add(18, 8); // eighteen
            _numToTextCount.Add(19, 8); // nineteen
        }

        public void BuildTwentyToNinetyNine() {
            _numToTextCount.Add(20, 6); // twenty
            _numToTextCount.Add(30, 6); // thirty
            _numToTextCount.Add(40, 5); // forty
            _numToTextCount.Add(50, 5); // fifty
            _numToTextCount.Add(60, 5); // sixty
            _numToTextCount.Add(70, 7); // seventy
            _numToTextCount.Add(80, 6); // eighty
            _numToTextCount.Add(90, 6); // ninety
            for (int a = 1; a <= 9; a++) {
                for (int b = 20; b <= 90; b += 10) {
                    _numToTextCount[a + b] = _numToTextCount[a] + _numToTextCount[b]; // don't count hyphen
                }
            }
        }

        public void BuildOneHundredToOneThousand() {
            for (int a = 1; a <= 9; a++) {
                _numToTextCount[a * 100] = _numToTextCount[a] + 7; // x hundred
            }
            _numToTextCount[1000] = 11; //one thousand
            for (int a = 1; a <= 99; a++) {
                for (int b = 1; b <= 9; b++) {
                    _numToTextCount[a + (b * 100)] = _numToTextCount[a] + _numToTextCount[b * 100] + 3; // 3 for the and
                }
            }
        }

        private int CountAll() {
            int sum = 0;
            for (int a = 1; a <= 1000; a++) {
                sum += _numToTextCount[a];
            }
            return sum;
        }
    }
}
