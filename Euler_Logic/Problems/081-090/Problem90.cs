using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem90 : ProblemBase {
        private List<HashSet<int>> _combos = new List<HashSet<int>>();
        private List<int[]> _squares = new List<int[]>();

        public override string ProblemName {
            get { return "90: Cube digit pairs"; }
        }

        public override string GetAnswer() {
            BuildSquares();
            BuildCombos(new int[6], 0, -1);
            return Solve().ToString();
        }

        private void BuildSquares() {
            _squares.Add(new int[2] { 0, 1 });
            _squares.Add(new int[2] { 0, 4 });
            _squares.Add(new int[2] { 0, 9 });
            _squares.Add(new int[2] { 1, 6 });
            _squares.Add(new int[2] { 2, 5 });
            _squares.Add(new int[2] { 3, 6 });
            _squares.Add(new int[2] { 4, 9 });
            _squares.Add(new int[2] { 6, 4 });
            _squares.Add(new int[2] { 8, 1 });
        }

        private void BuildCombos(int[] combo, int index, int lastNum) {
            for (int num = lastNum + 1; num <= 9; num++) {
                combo[index] = num;
                if (index == 5) {
                    int[] newCombo = new int[6];
                    Array.Copy(combo, newCombo, 6);
                    HashSet<int> hashCombo = new HashSet<int>(newCombo);
                    if (hashCombo.Contains(9)) {
                        hashCombo.Add(6);
                    }
                    if (hashCombo.Contains(6)) {
                        hashCombo.Add(9);
                    }
                    _combos.Add(hashCombo);
                    
                } else {
                    BuildCombos(combo, index + 1, num);
                }
            }
        }

        private int Solve() {
            int sum = 0;
            for (int dice1 = 0; dice1 < _combos.Count - 1; dice1++) {
                for (int dice2 = dice1 + 1; dice2 < _combos.Count; dice2++) {
                    bool isGood = true;
                    foreach (int[] square in _squares) {
                        if (_combos[dice1].Contains(square[0]) && _combos[dice2].Contains(square[1])) {
                            // do nothing
                        } else if (_combos[dice2].Contains(square[0]) && _combos[dice1].Contains(square[1])) {
                            // do nothing
                        } else {
                            isGood = false;
                            break;
                        }
                    }
                    if (isGood) {
                        sum++;
                    }
                }
            }
            return sum;
        }
    }
}
