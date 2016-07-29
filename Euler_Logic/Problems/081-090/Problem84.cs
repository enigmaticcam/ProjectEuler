using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem84 : IProblem {
        private Dictionary<bool, int> _isDiceDouble = new Dictionary<bool, int>();
        private Dictionary<int, int> _rollCountsDoubles = new Dictionary<int, int>();
        private Dictionary<int, int> _rollCountsNoDoubles = new Dictionary<int, int>();
        private Dictionary<int, enumSquareType> _squareTypes = new Dictionary<int, enumSquareType>();
        private Dictionary<int, int> _squareCounts = new Dictionary<int, int>();

        private enum enumSquareType {
            GotoJail,
            CC,
            Chance
        }

        public string ProblemName {
            get { return "84: Monopoly Odds"; }
        }

        public string GetAnswer() {
            int maxNumOnDice = 4;
            GenerateDiceDoubles(maxNumOnDice);
            AddSpecialCases();
            GenerateRollCounts(maxNumOnDice);
            GetCountsPerSquare();
            return "done";
        }

        public void GenerateDiceDoubles(int maxNumOnDice) {
            int numerator = maxNumOnDice * maxNumOnDice;
            int denominator = maxNumOnDice * maxNumOnDice * maxNumOnDice * maxNumOnDice;
            _isDiceDouble.Add(true, 1);
            _isDiceDouble.Add(false, (denominator / numerator) - 1);
        }

        private void AddSpecialCases() {
            _squareTypes.Add(30, enumSquareType.GotoJail);
            _squareTypes.Add(2, enumSquareType.CC);
            _squareTypes.Add(17, enumSquareType.CC);
            _squareTypes.Add(33, enumSquareType.CC);
            _squareTypes.Add(7, enumSquareType.Chance);
            _squareTypes.Add(22, enumSquareType.Chance);
            _squareTypes.Add(36, enumSquareType.Chance);
        }

        private void GenerateRollCounts(int maxNumOnDice) {
            for (int dice1 = 1; dice1 <= maxNumOnDice; dice1++) {
                for (int dice2 = 1; dice2 <= maxNumOnDice; dice2++) {
                    int sum = dice1 + dice2;
                    if (dice1 == dice2) {
                        if (!_rollCountsDoubles.ContainsKey(sum)) {
                            _rollCountsDoubles.Add(sum, 0);
                        }
                        _rollCountsDoubles[sum] += 1;
                    } else {
                        if (!_rollCountsNoDoubles.ContainsKey(sum)) {
                            _rollCountsNoDoubles.Add(sum, 0);
                        }
                        _rollCountsNoDoubles[sum] += 1;
                    }
                }
            }
        }

        private void GetCountsPerSquare() {
            for (int square = 0; square < 40; square++) {
                foreach (int rollNoDouble in _rollCountsNoDoubles.Keys) {
                    int newSquare = (square + rollNoDouble) % 40;
                    if (newSquare == 30) {
                        newSquare = 10;
                    }
                    if (!_squareCounts.ContainsKey(newSquare)) {
                        _squareCounts.Add(newSquare, 0);
                    }
                    _squareCounts[newSquare] += _rollCountsNoDoubles[rollNoDouble] * _isDiceDouble[false];
                    _squareCounts[newSquare] += _rollCountsNoDoubles[rollNoDouble] * _isDiceDouble[true];
                }
                foreach (int rollDouble in _rollCountsDoubles.Keys) {
                    int newSquare = (square + rollDouble) % 40;
                    if (newSquare == 30) {
                        newSquare = 10;
                    }
                    if (!_squareCounts.ContainsKey(newSquare)) {
                        _squareCounts.Add(newSquare, 0);
                    }
                    _squareCounts[newSquare] += _rollCountsDoubles[rollDouble] * _isDiceDouble[false];
                    _squareCounts[10] += _rollCountsDoubles[rollDouble] * _isDiceDouble[true];
                }
            }
        }
    }
}
