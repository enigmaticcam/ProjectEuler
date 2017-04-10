using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem205 : ProblemBase {

        public override string ProblemName {
            get { return "205: Dice Game"; }
        }

        public override string GetAnswer() {
            Dictionary<decimal, decimal> pete = new Dictionary<decimal, decimal>();
            Dictionary<decimal, decimal> colin = new Dictionary<decimal, decimal>();
            UpdateCountsRecursive(pete, 4, 8, 0);
            UpdateCountsRecursive(colin, 6, 5, 0);
            return CalcDifference(pete, colin).ToString();
        }

        private decimal CalcDifference(Dictionary<decimal, decimal> setA, Dictionary<decimal, decimal> setB) {
            decimal winCount = 0;
            decimal loseCount = 0;
            decimal drawCount = 0;
            foreach (decimal numA in setA.Keys) {
                foreach (decimal numB in setB.Keys) {
                    if (numA > numB) {
                        winCount += (setB[numB] * setA[numA]);
                    } else if (numA == numB) {
                        drawCount += (setB[numB] * setA[numA]);
                    } else {
                        loseCount += (setB[numB] * setA[numA]);
                    }
                }
            }
            return winCount / (winCount + loseCount + drawCount);
        }

        private void UpdateCountsRecursive(Dictionary<decimal, decimal> rollCounts, decimal diceSize, decimal diceRemaining, decimal sum) {
            for (int die = 1; die <= diceSize; die++) {
                if (diceRemaining > 0) {
                    UpdateCountsRecursive(rollCounts, diceSize, diceRemaining - 1, sum + die);
                } else {
                    AddToCount(sum + die, rollCounts);
                }
            }
        }

        private void AddToCount(decimal number, Dictionary<decimal, decimal> rollCounts) {
            if (rollCounts.ContainsKey(number)) {
                rollCounts[number] += 1;
            } else {
                rollCounts.Add(number, 1);
            }
        }
    }
}
