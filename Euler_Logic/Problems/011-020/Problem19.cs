using System;

namespace Euler_Logic.Problems {
    public class Problem19 : ProblemBase {
        public override string ProblemName {
            get { return "19: Counting Sundays"; }
        }

        public override string GetAnswer() {
            return Solve().ToString();
        }

        private int Solve() {
            var date = DateTime.Parse("1901-01-01");
            var last = DateTime.Parse("2000-12-31");
            int count = 0;
            do {
                if (date.DayOfWeek == DayOfWeek.Sunday && date.Day == 1) count++;
                date = date.AddDays(1);
            } while (date <= last);
            return count;
        }
    }
}
