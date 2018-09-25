using System.Collections.Generic;
using System.Linq;

namespace Euler_Logic.Problems {
    public class Problem109: ProblemBase {
        private List<Hit> _hits = new List<Hit>();
        private Dictionary<int, Hit> _doubles = new Dictionary<int, Hit>();
        private List<Hit> _dart1;
        private List<List<Hit>> _dart2;

        /*
            Unusually easy for a problem rated 45%. The total number of ways of checking out for any score
            is the sum of the total number of ways of checking out with 1 dart, plus with 2 darts, plus with
            3 darts. So we just need to calculate these three individual values for each score 2 to 99.

            If the current score is N, the number of ways of checking out with 1 dart is any double equal
            to N. The number of ways of checking out with 2 darts is for all darts K where N - K is equal
            to some dart that is double. 

            For calculating with 3 darts, we have to consider the constraint of what makes a distinct way.
            A way is distinct from another way if (1) it ends on a different double, or (2) the first two 
            darts are not the same between the two ways. This is easily solved by simply looping through
            each dart for the first dart, and then for each dart on the second dart starting with the first
            dart (instead of beginning).
         */

        public override string ProblemName {
            get { return "109: Darts"; }
        }

        public override string GetAnswer() {
            Initialize();
            return Solve().ToString();
        }

        private void Initialize() {
            int power = 0;
            for (int num = 1; num <= 20; num++) {
                _hits.Add(new Hit() {
                    IsDouble = false,
                    Value = num,
                    Key = "S" + num
                });
                _hits.Add(new Hit() {
                    IsDouble = true,
                    Value = num * 2,
                    Key = "D" + num
                });
                _doubles.Add(num * 2, _hits.Last());
                _hits.Add(new Hit() {
                    IsDouble = false,
                    Value = num * 3,
                    Key = "T" + num
                });
                power += 3;
            }
            _hits.Add(new Hit() {
                IsDouble = false,
                Value = 25,
                Key = "S25"
            });
            _hits.Add(new Hit() {
                IsDouble = true,
                Value = 50,
                Key = "D25"
            });
            _doubles.Add(50, _hits.Last());
        }

        private int Solve() {
            int count = 0;
            for (int remaining = 2; remaining < 100; remaining++) {
                count += Solve(remaining);
            }
            return count;
        }

        private int Solve(int remaining) {
            int count = 0;
            _dart1 = new List<Hit>();
            _dart2 = new List<List<Hit>>();
            count += (_doubles.ContainsKey(remaining) ? 1 : 0);
            count += SolveDart1(remaining);
            count += SolveDart2(remaining);
            return count;
        }

        private int SolveDart1(int remaining) {
            foreach (var hit in _hits) {
                var final = remaining - hit.Value;
                if (_doubles.ContainsKey(final)) {
                    var doub = _doubles[final];
                    _dart1.Add(hit);
                }
            }
            return _dart1.Count;
        }

        private int SolveDart2(int remaining) {
            for (int hit1Index = 0; hit1Index < _hits.Count; hit1Index++) {
                for (int hit2Index = hit1Index; hit2Index < _hits.Count; hit2Index++) {
                    var hit1 = _hits[hit1Index];
                    var hit2 = _hits[hit2Index];
                    if (_doubles.ContainsKey(remaining - hit1.Value - hit2.Value)) {
                        _dart2.Add(new List<Hit>());
                        _dart2.Last().Add(hit1);
                        _dart2.Last().Add(hit2);
                    }
                }
            }
            return _dart2.Count;
        }

        private class Hit {
            public int Value { get; set; }
            public bool IsDouble { get; set; }
            public string Key { get; set; }
        }
    }
}
