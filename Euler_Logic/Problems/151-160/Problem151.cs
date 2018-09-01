using System;
using System.Collections.Generic;

namespace Euler_Logic.Problems {
    public class Problem151 : ProblemBase {
        private Dictionary<int, Dictionary<int, int>> _papers = new Dictionary<int, Dictionary<int, int>>();
        private decimal _totalProbability = 0;

        /*
            If you brute force every possible variation, there are only 40,040 unique ways to play out a week. However, not every way has the same probability.
            If you imagine a tree starting at size A1 and branching out, the probability would start at 100% and divide by the count at each branch as you
            travel down. For example, after cutting A1, you are left with four papers. Therefore each branch following A1 has a 25% of ocurring. Suppose you
            then cut A2 - that now leaves you with two A3's, two A4's, and two A5's. With six papers, each paper would then be 25% / 6, or 4.167%. However,
            since there are duplicates of paper sizes, you only need to consider the resulting branch of cutting an additional size. Since there are
            three sizes, that yields three branches where each branch is 25% / (2 / 6), or 8.333%. Follow the unique branches, count the times you are left
            with just one paper, and add the final probability once you reach the last job. Return the final probability round up to six digits.
         */

        public override string ProblemName {
            get { return "151: Paper sheets of standard sizes: an expected-value problem"; }
        }

        public override string GetAnswer() {
            Initialize();
            Solve(0, 1, 1);
            return Math.Round(_totalProbability, 6).ToString();
        }

        private void Initialize() {
            for (int job = 1; job <= 15; job++) {
                _papers.Add(job, new Dictionary<int, int>());
                for (int index = 1; index <= 5; index++) {
                    _papers[job].Add(index, 0);
                }
            }
            _papers[1][1] = 1;
        }

        private void Solve(decimal currentSum, int currentJob, decimal currentProbability) {
            decimal paperCount = 0;
            for (int paper = 1; paper <= 5; paper++) {
                paperCount += _papers[currentJob][paper];
            }
            currentSum += (paperCount == 1 ? 1 : 0);
            if (currentJob == 15) {
                _totalProbability += (currentSum - 1) * currentProbability;
            } else {
                for (int paper = 1; paper <= 5; paper++) {
                    if (_papers[currentJob][paper] > 0) {
                        for (int nextJobPaper = 1; nextJobPaper <= 5; nextJobPaper++) {
                            int offset = 0;
                            if (nextJobPaper == paper) {
                                offset = -1;
                            } else if (nextJobPaper > paper) {
                                offset += 1;
                            }
                            _papers[currentJob + 1][nextJobPaper] = _papers[currentJob][nextJobPaper] + offset;
                        }
                        Solve(currentSum, currentJob + 1, (currentProbability * _papers[currentJob][paper]) / paperCount);
                    }
                }
            }
        }
    }
}