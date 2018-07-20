using System.Collections.Generic;

namespace Euler_Logic.Problems {
    public class Problem491 : ProblemBase {
        private List<int> _digitCount = new List<int>();
        private Dictionary<ulong, ulong> _factorials = new Dictionary<ulong, ulong>();
        private ulong _total;
        private ulong _two = 2;
        private ulong _one = 1;

        public override string ProblemName {
            get { return "491: Double pandigital number divisible by 11"; }
        }

        /*
            If sum of the even digits minus the sum of the add digits is divisible by 11, then that number is divisible by 11.
            Knowing this, generate all the possibilities of arranging the digits 0-9 twice as either an addition or subtraction.
            A digit can be used 0, 1, or 2 times as an addition or subtraction. If the number of digits used is 20, then check
            if the sum is divisible by 11. If it is, calculate all the possible variations of arranging the additions and multiply
            all variations of subtractions. Consider that the number of additions cannot include those which have a leading zero.
            The total of either without zeroes is 10!. Divide that by half for each number that is duplicated.
         */

        public override string GetAnswer() {
            Initialize();
            BuildSets(10, 0);
            return _total.ToString();
        }

        private void Initialize() {
            for (int index = 1; index <= 10; index++) {
                _digitCount.Add(0);
            }
        }

        private void BuildSets(int addsLeft, int index) {
            // Add 0
            _digitCount[index] = 0;
            if (index == 9) {
                Check();
            } else {
                BuildSets(addsLeft, index + 1);
            }

            // Add 1
            _digitCount[index] = 1;
            if (index == 9) {
                Check();
            } else if (addsLeft > 0) {
                BuildSets(addsLeft - 1, index + 1);
            }

            // Add 2
            _digitCount[index] = 2;
            if (index == 9) {
                Check();
            } else if (addsLeft > 1) {
                BuildSets(addsLeft - 2, index + 1);
            }
        }

        private void Check() {
            if (_digitCount[0]
                + _digitCount[1]
                + _digitCount[2]
                + _digitCount[3]
                + _digitCount[4]
                + _digitCount[5]
                + _digitCount[6]
                + _digitCount[7]
                + _digitCount[8]
                + _digitCount[9] == 10) {
                int sum = (_digitCount[0] * 0)
                    + (_digitCount[1] * 1)
                    + (_digitCount[2] * 2)
                    + (_digitCount[3] * 3)
                    + (_digitCount[4] * 4)
                    + (_digitCount[5] * 5)
                    + (_digitCount[6] * 6)
                    + (_digitCount[7] * 7)
                    + (_digitCount[8] * 8)
                    + (_digitCount[9] * 9)
                    - ((2 - _digitCount[0]) * 0)
                    - ((2 - _digitCount[1]) * 1)
                    - ((2 - _digitCount[2]) * 2)
                    - ((2 - _digitCount[3]) * 3)
                    - ((2 - _digitCount[4]) * 4)
                    - ((2 - _digitCount[5]) * 5)
                    - ((2 - _digitCount[6]) * 6)
                    - ((2 - _digitCount[7]) * 7)
                    - ((2 - _digitCount[8]) * 8)
                    - ((2 - _digitCount[9]) * 9);
                if (sum % 11 == 0) {
                    ulong additions = ((ulong)(10 - _digitCount[0])) * Factorial(9)
                        / (_digitCount[0] == 2 ? _two : _one)
                        / (_digitCount[1] == 2 ? _two : _one)
                        / (_digitCount[2] == 2 ? _two : _one)
                        / (_digitCount[3] == 2 ? _two : _one)
                        / (_digitCount[4] == 2 ? _two : _one)
                        / (_digitCount[5] == 2 ? _two : _one)
                        / (_digitCount[6] == 2 ? _two : _one)
                        / (_digitCount[7] == 2 ? _two : _one)
                        / (_digitCount[8] == 2 ? _two : _one)
                        / (_digitCount[9] == 2 ? _two : _one);
                    ulong subtractions = Factorial(10)
                        / (_digitCount[0] == 2 ? _two : _one)
                        / (_digitCount[1] == 2 ? _two : _one)
                        / (_digitCount[2] == 2 ? _two : _one)
                        / (_digitCount[3] == 2 ? _two : _one)
                        / (_digitCount[4] == 2 ? _two : _one)
                        / (_digitCount[5] == 2 ? _two : _one)
                        / (_digitCount[6] == 2 ? _two : _one)
                        / (_digitCount[7] == 2 ? _two : _one)
                        / (_digitCount[8] == 2 ? _two : _one)
                        / (_digitCount[9] == 2 ? _two : _one);
                    _total += additions * subtractions;
                }
            }
        }

        private ulong Factorial(ulong max) {
            if (!_factorials.ContainsKey(max)) {
                ulong sum = max;
                for (ulong num = max - 1; num >= 1; num--) {
                    sum *= num;
                }
                _factorials.Add(max, sum);
            }
            return _factorials[max];
        }
    }
}