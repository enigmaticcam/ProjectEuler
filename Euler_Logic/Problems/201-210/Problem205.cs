using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem205 : IProblem {
        private Dictionary<decimal, decimal> _counts = new Dictionary<decimal, decimal>();
        private decimal _diceRollCount;

        public string ProblemName {
            get { return "205: Dice Game"; }
        }

        public string GetAnswer() {
            decimal a = GetProbability(4, 9);
            decimal b = GetProbability(6, 6);

            return GetProbability(6, 2).ToString();
        }

        private decimal GetProbability(decimal diceSize, decimal diceCount) {
            _counts = new Dictionary<decimal, decimal>();
            _diceRollCount = 0;
            UpdateCountsRecursive(diceSize, diceCount - 1, 0);
            return CalcProbability();
        }

        private void UpdateCountsRecursive(decimal diceSize, decimal diceRemaining, decimal sum) {
            for (int die = 1; die <= diceSize; die++) {
                if (diceRemaining > 0) {
                    UpdateCountsRecursive(diceSize, diceRemaining - 1, sum + die);
                } else {
                    AddToCount(sum + die);
                }
            }
        }

        private void AddToCount(decimal number) {
            _diceRollCount++;
            if (_counts.ContainsKey(number)) {
                _counts[number] += 1;
            } else {
                _counts.Add(number, 1);
            }
        }

        private decimal CalcProbability() {
            decimal probability = 0;
            foreach (decimal num in _counts.Keys) {
                probability += (_counts[num] / _diceRollCount) * num;
            }
            return probability;
        }
    }
}
