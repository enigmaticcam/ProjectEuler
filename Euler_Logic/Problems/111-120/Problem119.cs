using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem119 : IProblem {
        private List<decimal> _powers = new List<decimal>();

        public string ProblemName {
            get { return "119: Digit power sum"; }
        }

        public string GetAnswer() {
            Solve();
            return "done";
        }

        private void Solve() {
            Power start = new Power();
            start.Root = 2;
            start.Num = 4;
            _powers.Add(4);
            Power next = new Power();
            next.Root = 3;
            next.Num = 9;
            start.Next = next;
            decimal nextRoot = 4;
            decimal last = 4;
            do {
                start.Num *= start.Root;
                if (start.Num > start.Next.Num) {
                    start = FindNewSpot(start);
                }
                if (nextRoot * nextRoot < start.Num) {
                    Power newPower = new Power();
                    newPower.Root = nextRoot;
                    newPower.Num = nextRoot * nextRoot;
                    newPower.Next = start;
                    start = newPower;
                    nextRoot++;
                }
                if (start.Num != last) {
                    _powers.Add(start.Num);
                    last = start.Num;
                }
            } while (true);
        }

        private Power FindNewSpot(Power power) {
            Power returnNext = power.Next;
            Power next = power.Next;
            power.Next = null;
            do {
                if (next.Next == null) {
                    next.Next = power;
                    return returnNext;
                } else if (next.Next.Num > power.Num) {
                    power.Next = next.Next;
                    next.Next = power;
                    return returnNext;
                }
            } while (true);
        }

        private class Power {
            public decimal Root { get; set; }
            public decimal Num { get; set; }
            public Power Next { get; set; }
        }
    }
}
