using System.Linq;

namespace Euler_Logic.Problems {
    public class Problem121 : ProblemBase {
        private bool[] _win;
        private ulong _count;

        /*
            The total number of ways to win is the total ways to win all
            15 draws, plus the total ways to win 14 draws, plus the total
            ways to win 13 draws, until you reach 7 draws. 

            If winning all 15 draws, there is only one way.

            If winning 14 draws, then that means you lose one draw.
            There are 15 draws to lose. But for each draw you lose, the 
            number of ways to lose is equal to the number of red discs
            availale for that draw. Since you're only losing one draw,
            winning 14 draws is 2 + 3 + 4 + 5 + 6, etc.

            If winning 13 draws, then that means you lose two draws.
            This time we loop through each unique way of losing two draws.
            Lose 1-2, 1-3, 1-4, 1-5...1-15, 2-3, 2-4, 2-5...2-15. For each,
            multiply the total number of red discs avaiable at each draw lost.
            Sum the result.

            Once you calculate the total number of ways to win (a), you then
            calculate the total number of ways to play the game, which is 15! (b). Then
            to get the maximum prize fund: f = int(1 / (a / b))
         */

        public override string ProblemName {
            get { return "121: Disc game prize fund"; }
        }

        public override string GetAnswer() {
            return Solve(15).ToString();
        }

        private int Solve(int turns) {
            _count = 1;
            for (int winByAmount = turns - 1; winByAmount > turns / 2; winByAmount--) {
                _win = new bool[turns];
                Count(winByAmount, 0);
            }
            return GetMaxPrizeFund(turns);
        }

        private void Count(int remaining, int currentIndex) {
            if (remaining == 0) {
                Sum();
            } else {
                for (int index = currentIndex; index < _win.Length; index++) {
                    _win[index] = true;
                    Count(remaining - 1, index + 1);
                    _win[index] = false;
                }
            }
        }

        private void Sum() {
            ulong product = 1;
            for (int index = 0; index < _win.Length; index++) {
                if (!_win[index]) {
                    product *= (ulong)index + 1;
                }
            }
            _count += product;
        }

        private int GetMaxPrizeFund(int turns) {
            decimal a = _count;
            decimal b = 1;
            Enumerable.Range(2, turns).ToList().ForEach(x => b *= x);
            int result = (int)((decimal)1 / (a / b));
            return result;
        }
    }
}
