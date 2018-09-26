using System.Collections.Generic;
using System.Linq;

namespace Euler_Logic.Problems {
    public class Problem122 : ProblemBase {
        private Dictionary<int, int> _best = new Dictionary<int, int>();
        private List<HashSet<int>>[] _sequencesRef;
        private List<HashSet<int>> _sequencesList = new List<HashSet<int>>();

        /*
            There are multiple ways of reaching the most efficent value for k. But
            you can't settle on just one method and never account for any others,
            as each one has the possibility of adding a new number to the sequence 
            that is more efficient than what's previously been found for the new number
            (77 is a good example).            

            So we need to maintain a list of all sequences that give us the best
            value for all k. Starting with a sequence with one element (1), for
            each sequence I've found I loop through all possibilities of adding a
            new numer. For example, if my current sequence is 1-2-3, then I can do:
            2(1+1), 3(1+2), 4(1+3), 4(2+2), 5(2+3), and 6(3+3). I then check to see
            if any of these new numbers have been found before. If not, assume they're
            the best. If we have, first I compare the lengths. If the lenghts of these
            new sequences is more than what we've found before, I ignore them. If
            they're the same, then I look to see if this new sequence is unique and
            add it if it is. If it's less, I remove all previous sequences found and
            add this.

            Once I've looped through the sequences and generated a new list, I can
            toss the previous list and start from only the new list. I continue to
            do this until a best value has been found for all of k.
         */

        public override string ProblemName {
            get { return "122: Efficient exponentiation"; }
        }

        public override string GetAnswer() {
            int max = 200;
            Initialize(max);
            Solve(max);
            return _best.Values.Sum().ToString();
        }

        private void Initialize(int max) {
            _sequencesRef = new List<HashSet<int>>[max + 1];
            _best.Add(1, 0);
            _sequencesRef[1] = new List<HashSet<int>>();
            _sequencesRef[1].Add(new HashSet<int>() { 1 });
            _sequencesList.Add(_sequencesRef[1][0]);
        }

        private void Solve(int max) {
            do {
                var nextSequence = new List<HashSet<int>>();
                for (int index = 0; index <= _sequencesList.Count - 1; index++) {
                    var sequence = _sequencesList[index];
                    for (int index1 = 0; index1 < sequence.Count; index1++) {
                        for (int index2 = index1; index2 < sequence.Count; index2++) {
                            var value1 = sequence.ElementAt(index1);
                            var value2 = sequence.ElementAt(index2);
                            var value = value1 + value2;
                            if (value <= max) {
                                bool add = false;
                                if (!_best.ContainsKey(value)) {
                                    _best.Add(value, sequence.Count);
                                    add = true;
                                } else if (_best[value] > sequence.Count) {
                                    _best[value] = sequence.Count;
                                    _sequencesRef[value] = new List<HashSet<int>>();
                                    add = true;
                                } else if (_best[value] == sequence.Count) {
                                    foreach (var subSequence in _sequencesRef[value]) {
                                        foreach (var subValue in sequence) {
                                            if (!subSequence.Contains(subValue)) {
                                                add = true;
                                                break;
                                            }
                                        }
                                        if (add) {
                                            break;
                                        }
                                    }
                                }
                                if (add) {
                                    var newSequence = new HashSet<int>(sequence);
                                    newSequence.Add(value);
                                    nextSequence.Add(newSequence);
                                    if (_sequencesRef[value] == null) {
                                        _sequencesRef[value] = new List<HashSet<int>>();
                                    }
                                    _sequencesRef[value].Add(newSequence);
                                }
                            }
                        }
                    }
                }
                _sequencesList = nextSequence;
            } while (_best.Keys.Count < max);
        }
    }
}
