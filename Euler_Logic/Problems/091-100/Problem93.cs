using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem93 : IProblem {
        private List<int[]> _operatorCombos = new List<int[]>();
        private List<List<Paranthesis>> _paranthesisCombos = new List<List<Paranthesis>>();
        private List<int[]> _equations = new List<int[]>();

        public string ProblemName {
            get { return "93: Arithmetic expressions"; }
        }

        public string GetAnswer() {
            BuildOperatorCombos(new int[3], 0);
            BuildParatnehsisCombos();
            BuildEquations();
            BuildNumberCombos(new int[4], 0, -1);
            return "done";
        }

        private void BuildNumberCombos(int[] combo, int index, int lastNum) {
            for (int num = lastNum + 1; num <= 9; num++) {
                combo[index] = num;
                if (index == 3) {
                    BuildValues(combo);
                } else {
                    BuildNumberCombos(combo, index + 1, num);
                }
            }
        }

        private void BuildValues(int[] nums) {
            foreach (int[] equation in _equations) {
                int value = GetValue(nums, equation, 0);
            }
        }

        private int GetValue(int[] nums, int[] equation, int startIndex) {
            /*
             Operators
             0 = +
             1 = -
             2 = *
             3 = /
             
             Equation
             0 = operator
             1 = paranthesis
            */
            if (_operatorCombos[equation[0]][0] > 1) {
                
            }
        }

        private void BuildOperatorCombos(int[] combo, int index) {
            for (int num = 0; num < 3; num++) {
                combo[index] = num;
                if (index == 2) {
                    int[] newCombo = new int[3];
                    Array.Copy(combo, newCombo, 3);
                    _operatorCombos.Add(newCombo);
                } else {
                    BuildOperatorCombos(combo, index + 1);
                }
            }
        }

        private void BuildParatnehsisCombos() {
            List<Paranthesis> combo;

            // a + b + c + d
            _paranthesisCombos.Add(new List<Paranthesis>());

            // (a + b) + c + d
            combo = new List<Paranthesis>();
            combo.Add(new Paranthesis(0, 1));
            _paranthesisCombos.Add(combo);

            // ((a + b) + c) + d
            combo = new List<Paranthesis>();
            combo.Add(new Paranthesis(0, 1));
            combo.Add(new Paranthesis(0, 2));
            _paranthesisCombos.Add(combo);

            // (a + b) + (c + d)
            combo = new List<Paranthesis>();
            combo.Add(new Paranthesis(0, 1));
            combo.Add(new Paranthesis(2, 3));
            _paranthesisCombos.Add(combo);

            // a + (b + c) + d
            combo = new List<Paranthesis>();
            combo.Add(new Paranthesis(1, 2));
            _paranthesisCombos.Add(combo);

            // (a + (b + c)) + d
            combo = new List<Paranthesis>();
            combo.Add(new Paranthesis(1, 2));
            combo.Add(new Paranthesis(0, 2));
            _paranthesisCombos.Add(combo);

            // a + ((b + c) + d)
            combo = new List<Paranthesis>();
            combo.Add(new Paranthesis(1, 2));
            combo.Add(new Paranthesis(1, 3));
            _paranthesisCombos.Add(combo);

            // a + b + (c + d)
            combo = new List<Paranthesis>();
            combo.Add(new Paranthesis(2, 3));
            _paranthesisCombos.Add(combo);

            // a + (b + (c + d))
            combo = new List<Paranthesis>();
            combo.Add(new Paranthesis(2, 3));
            combo.Add(new Paranthesis(1, 3));
            _paranthesisCombos.Add(combo);
        }

        private void BuildEquations() {
            for (int op = 0; op < _operatorCombos.Count; op++) {
                for (int par = 0; par < _paranthesisCombos.Count; par++) {
                    _equations.Add(new int[2] { op, par });
                }
            }
        }

        private class Paranthesis {
            public int Start { get; set; }
            public int End { get; set; }

            public Paranthesis(int start, int end) {
                this.Start = start;
                this.End = end;
            }
        }
    }
}
