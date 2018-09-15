using System.Collections.Generic;

namespace Euler_Logic.Problems {
    public class Problem84 : ProblemBase {
        private decimal[,] _p;
        private List<Roll> _rolls = new List<Roll>();
        private int _indexC1 = 11;
        private int _indexE3 = 24;
        private int _indexH2 = 39;
        private int _indexR1 = 5;
        private int _indexJail = 10;
        private int _indexGoToJail = 30;
        private decimal _fourteenOver16 = (decimal)14 / (decimal)16;
        private decimal _oneOver16 = (decimal)1 / (decimal)16;
        private decimal _twoOver16 = (decimal)2 / (decimal)16;
        private decimal _sixOver16 = (decimal)6 / (decimal)16;
        private HashSet<int> _indexCC = new HashSet<int>() { 2, 17, 33 };
        private HashSet<int> _indexCH = new HashSet<int>() { 7, 22, 36 };

        /*
            I had originally solved this using a simulation. But after having learned how dynamic programming can be used to solve these kinds of problems,
            I rewrote a new algorithm that's much more efficient. Essentially, the probability of reaching any square is the sum of the probabilities of all
            the different ways of arriving at that square. So I create a 40x3 array which represents the probability of arriving at each square having 
            rolled 0, 1, or 2 doubles. I create a list of rolls based on what kinds of dice I'm using. Then for each square for each double count (0-39, 0-2),
            I loop through all the rolls that can be performed and (1) send to jail if it's the 3rd double, (2) determine probability of staying or going to
            a new square if I land on Community Chest or Chance, (3) to go jail if I land on the Go to Jail square, or (4) I stay at my new square. If I landed
            on Chance, then I account for the possibility of rolling back 3 spaces by trying again after calculating other possibilities.

            I repeat this process 16 times, enough to fully utilize all cards, and then I return the top three squares.

            FYI, we never calculate any probabilities of rolling from square 30 (Go to Jail), since it's impossible to start a turn from that square.
         */

        public override string ProblemName {
            get { return "84: Monopoly Odds"; }
        }

        public override string GetAnswer() {
            _p = new decimal[40, 3];
            InitializeDice(4, 4);
            InitializeProbabilities();
            CalcProbabilities();
            return Solve();
        }

        private void InitializeDice(int maxDie1, int maxDie2) {
            Dictionary<int, Dictionary<bool, int>> rolls = new Dictionary<int, Dictionary<bool, int>>();
            for (int die1 = 1; die1 <= maxDie1; die1++) {
                for (int die2 = 1; die2 <= maxDie2; die2++) {
                    var sum = die1 + die2;
                    var isDouble = die1 == die2;
                    if (!rolls.ContainsKey(sum)) {
                        rolls.Add(sum, new Dictionary<bool, int>());
                    }
                    if (!rolls[sum].ContainsKey(isDouble)) {
                        rolls[sum].Add(isDouble, 1);
                    } else {
                        rolls[sum][isDouble]++;
                    }
                }
            }
            foreach (var sum in rolls.Keys) {
                foreach (var isDouble in rolls[sum].Keys) {
                    _rolls.Add(new Roll() {
                        Count = sum,
                        IsDouble = isDouble,
                        Probability = (decimal)rolls[sum][isDouble] / (maxDie1 * maxDie2)
                    });
                }
            }
        }

        private void InitializeProbabilities() {
            for (int square = 0; square < 40; square++) {
                for (int doubleCount = 0; doubleCount <= 2; doubleCount++) {
                    _p[square, doubleCount] = 1;
                }
            }
        }

        private void CalcProbabilities() {
            for (int count = 1; count <= 16; count++) {
                decimal[,] p = new decimal[40, 3];
                for (int square = 0; square < 40; square++) {
                    if (square != 30) {
                        for (int doubleCount = 0; doubleCount <= 2; doubleCount++) {
                            foreach (var roll in _rolls) {
                                int newDoubleCount = (roll.IsDouble ? doubleCount + 1 : 0);
                                decimal probabilityNextSquare = roll.Probability * _p[square, doubleCount];
                                int nextSquare = (square + roll.Count) % 40;
                                bool tryAgain = false;
                                do {
                                    tryAgain = false;
                                    if (roll.IsDouble && newDoubleCount == 3) {

                                        // Go to jail after three doubles
                                        p[_indexJail, 0] += probabilityNextSquare;

                                    } else if (_indexCC.Contains(nextSquare)) {

                                        // Community chest

                                        // Stay on square, go to Go, to to Jail
                                        p[nextSquare, newDoubleCount] += _fourteenOver16 * probabilityNextSquare;
                                        p[0, newDoubleCount] += _oneOver16 * probabilityNextSquare;
                                        p[_indexJail, 0] += _oneOver16 * probabilityNextSquare;
                                    } else if (_indexCH.Contains(nextSquare)) {

                                        // Chance

                                        // Stay on square, go to Go, go to Jail
                                        p[nextSquare, newDoubleCount] += _sixOver16 * probabilityNextSquare;
                                        p[0, newDoubleCount] += _oneOver16 * probabilityNextSquare;
                                        p[_indexJail, 0] += _oneOver16 * probabilityNextSquare;

                                        // Go to C1, E3, H2, R1
                                        p[_indexC1, newDoubleCount] += _oneOver16 * probabilityNextSquare;
                                        p[_indexE3, newDoubleCount] += _oneOver16 * probabilityNextSquare;
                                        p[_indexH2, newDoubleCount] += _oneOver16 * probabilityNextSquare;
                                        p[_indexR1, newDoubleCount] += _oneOver16 * probabilityNextSquare;

                                        // Go to next R
                                        int nextR = 0;
                                        if (nextSquare > 35) {
                                            nextR = 5;
                                        } else if (nextSquare > 25) {
                                            nextR = 35;
                                        } else if (nextSquare > 15) {
                                            nextR = 25;
                                        } else {
                                            nextR = 15;
                                        }
                                        p[nextR, newDoubleCount] += _twoOver16 * probabilityNextSquare;

                                        // Go to next U
                                        int nextU = 0;
                                        if (nextSquare > 28) {
                                            nextU = 12;
                                        } else {
                                            nextU = 28;
                                        }
                                        p[nextU, newDoubleCount] += _oneOver16 * probabilityNextSquare;

                                        // Go back 3 spaces
                                        probabilityNextSquare *= _oneOver16;
                                        nextSquare = (nextSquare - 3) % 40;
                                        tryAgain = true;
                                    } else if (nextSquare == _indexGoToJail) {

                                        // Go to jail
                                        p[_indexJail, 0] += probabilityNextSquare;

                                    } else {


                                        // All other moves
                                        p[nextSquare, newDoubleCount] += probabilityNextSquare;
                                    }
                                } while (tryAgain);
                            }
                        }
                    }
                }
                _p = p;
            }
        }

        private string Solve() {
            decimal[] sumP = new decimal[40];
            for (int square = 0; square < 40; square++) {
                for (int doubleCount = 0; doubleCount <= 2; doubleCount++) {
                    sumP[square] += _p[square, doubleCount];
                }
            }

            int[] bestSquare = new int[3];
            decimal[] bestScore = new decimal[3];
            for (int square = 0; square < 40; square++) {
                if (bestScore[0] < sumP[square]) {
                    bestScore[2] = bestScore[1];
                    bestScore[1] = bestScore[0];
                    bestScore[0] = sumP[square];
                    bestSquare[2] = bestSquare[1];
                    bestSquare[1] = bestSquare[0];
                    bestSquare[0] = square;
                } else if (bestScore[1] < sumP[square]) {
                    bestScore[2] = bestScore[1];
                    bestScore[1] = sumP[square];
                    bestSquare[2] = bestSquare[1];
                    bestSquare[1] = square;
                } else if (bestScore[2] < sumP[square]) {
                    bestScore[2] = sumP[square];
                    bestSquare[2] = square;
                }
            }
            return bestSquare[0].ToString().PadLeft(2, '0') + bestSquare[1].ToString().PadLeft(2, '0') + bestSquare[2].ToString().PadLeft(2, '0');
        }

        private class Roll {
            public int Count { get; set; }
            public decimal Probability { get; set; }
            public bool IsDouble { get; set; }
        }
    }
}