using System;
using System.Collections.Generic;
using System.Linq;

namespace Euler_Logic.Problems {
    public class Problem185 : ProblemBase {
        private List<List<ulong>> _answers = new List<List<ulong>>();
        private List<Guess> _guess = new List<Guess>();
        private Dictionary<int, List<ulong>> _possibilities = new Dictionary<int, List<ulong>>();
        private Dictionary<int, ulong> _numToBit = new Dictionary<int, ulong>();
        private ulong _currentPossibility;
        private bool _answerFound = false;

        public override string ProblemName {
            get { return "185: Number Mind"; }
        }

        /*
            This is a trial by guess using bitwise notation to represent the possibilities of each digit. If the first sequence has one correct digit, then assume
            the first digit is correct. This means none of the other digits are correct. Move on to the second sequence and do the same. Continue until a digit
            has been removed of all possibilities (in which case we backtrack a guess and try another), or we reach the last sequence and find the solution. Not
            very efficient, takes about 10 minutes to run. Oh well.
         */

        public override string GetAnswer() {
            InitializeActual();
            BuildNumToBit();
            BuildPossibilities();
            InitializeAnswers();
            PerformGuessing(0);
            return Answer();
        }

        private string Answer() {
            string answer = "";
            foreach (ulong digit in _answers.Last()) {
                answer += Math.Log(digit, 2);
            }
            return answer;
        }

        private int _highestIndex;
        private void PerformGuessing(int guessIndex) {
            if (guessIndex > _highestIndex) {
                _highestIndex = guessIndex;
            }
            if (guessIndex >= _guess.Count) {
                _answerFound = true;
            } else {
                var guess = _guess[guessIndex];
                if (guess.Correct != 0) {
                    PerformGuessingLookForPossibilities(guessIndex, guess);
                } else {
                    PerformGuessingSetAllNotCorrect(guessIndex, guess);
                }
            }
        }

        private void PerformGuessingLookForPossibilities(int guessIndex, Guess guess) {
            foreach (var possibility in _possibilities[guess.Correct]) {
                ResetAnswer(guessIndex + 1);
                bool keepGuessing = true;
                for (int digitIndex = 0; digitIndex < guess.Digits.Count; digitIndex++) {
                    ulong bit = _numToBit[guess.Digits[digitIndex]];
                    bool hasBit = (_answers[guessIndex + 1][digitIndex] & bit) > 0;
                    if ((possibility & _numToBit[digitIndex]) > 0) {
                        if (hasBit) {
                            _answers[guessIndex + 1][digitIndex] = bit;
                        } else {
                            keepGuessing = false;
                            break;
                        }
                    } else if (hasBit) {
                        _answers[guessIndex + 1][digitIndex] -= bit;
                        if (_answers[guessIndex + 1][digitIndex] == 0) {
                            keepGuessing = false;
                        }
                    }
                }
                if (keepGuessing) {
                    _currentPossibility += possibility;
                    PerformGuessing(guessIndex + 1);
                    if (_answerFound) {
                        break;
                    }
                    _currentPossibility -= possibility;
                }
            }
        }

        private void PerformGuessingSetAllNotCorrect(int guessIndex, Guess guess) {
            ResetAnswer(guessIndex + 1);
            bool keepGuessing = true;
            for (int digitIndex = 0; digitIndex < guess.Digits.Count; digitIndex++) {
                ulong bit = _numToBit[guess.Digits[digitIndex]];
                bool hasBit = (_answers[guessIndex + 1][digitIndex] & bit) > 0;
                bool previouslySet = (_numToBit[digitIndex] & _currentPossibility) > 0;
                if (hasBit) {
                    if (previouslySet) {
                        keepGuessing = false;
                        break;
                    }
                    _answers[guessIndex + 1][digitIndex] -= bit;
                    if (_answers[guessIndex + 1][digitIndex] == 0) {
                        keepGuessing = false;
                        break;
                    }
                }
            }
            if (keepGuessing) {
                PerformGuessing(guessIndex + 1);
            }
        }

        private void ResetAnswer(int answerIndex) {
            for (int digitIndex = 0; digitIndex < _answers[0].Count; digitIndex++) {
                _answers[answerIndex][digitIndex] = _answers[answerIndex - 1][digitIndex];
            }
        }

        private void InitializeAnswers() {
            List<ulong> digits = new List<ulong>();
            for (int index = 0; index < _guess[0].Digits.Count; index++) {
                digits.Add(1023);
            }
            _guess.ForEach(x => _answers.Add(new List<ulong>(digits)));
            _answers.Add(new List<ulong>(digits));
        }

        private void BuildNumToBit() {
            for (int num = 0; num <= 16; num++) {
                _numToBit.Add(num, (ulong)Math.Pow(2, num));
            }
        }

        private void BuildPossibilities() {
            _possibilities.Add(0, new List<ulong>());
            BuildPossibilitesOnes();
            BuildPossibilitiesMore(2);
            BuildPossibilitiesMore(3);
        }

        private void BuildPossibilitesOnes() {
            List<ulong> ones = new List<ulong>();
            for (int countIndex = 0; countIndex < _guess[0].Digits.Count; countIndex++) {
                ones.Add(_numToBit[countIndex]);
            }
            _possibilities.Add(1, ones);
        }

        private void BuildPossibilitiesMore(int count) {
            List<ulong> more = new List<ulong>();
            foreach (var one in _possibilities[count - 1]) {
                for (int countIndex = 0; countIndex < _guess[0].Digits.Count; countIndex++) {
                    AddBit(more, one, _numToBit[countIndex]);
                }
            }
            _possibilities.Add(count, more);
        }

        private void AddBit(List<ulong> current, ulong bits, ulong bitToAdd) {
            if ((bits & bitToAdd) == 0 && bitToAdd > bits) {
                current.Add(bitToAdd + bits);
            }
        }

        private void InitializeTest() {
            _guess.Add(new Guess() {
                Correct = 2,
                Digits = new List<int>() { 9, 0, 3, 4, 2 }
            });
            _guess.Add(new Guess() {
                Correct = 0,
                Digits = new List<int>() { 7, 0, 7, 9, 4 }
            });
            _guess.Add(new Guess() {
                Correct = 2,
                Digits = new List<int>() { 3, 9, 4, 5, 8 }
            });
            _guess.Add(new Guess() {
                Correct = 1,
                Digits = new List<int>() { 3, 4, 1, 0, 9 }
            });
            _guess.Add(new Guess() {
                Correct = 2,
                Digits = new List<int>() { 5, 1, 5, 4, 5 }
            });
            _guess.Add(new Guess() {
                Correct = 1,
                Digits = new List<int>() { 1, 2, 5, 3, 1 }
            });
            _guess = _guess.OrderBy(x => x.Correct).ToList();
        }

        private void InitializeActual() {
            _guess.Add(new Guess() {
                Correct = 2,
                Digits = new List<int>() { 5, 6, 1, 6, 1, 8, 5, 6, 5, 0, 5, 1, 8, 2, 9, 3 }
            });
            _guess.Add(new Guess() {
                Correct = 1,
                Digits = new List<int>() { 3, 8, 4, 7, 4, 3, 9, 6, 4, 7, 2, 9, 3, 0, 4, 7 }
            });
            _guess.Add(new Guess() {
                Correct = 3,
                Digits = new List<int>() { 5, 8, 5, 5, 4, 6, 2, 9, 4, 0, 8, 1, 0, 5, 8, 7 }
            });
            _guess.Add(new Guess() {
                Correct = 3,
                Digits = new List<int>() { 9, 7, 4, 2, 8, 5, 5, 5, 0, 7, 0, 6, 8, 3, 5, 3 }
            });
            _guess.Add(new Guess() {
                Correct = 3,
                Digits = new List<int>() { 4, 2, 9, 6, 8, 4, 9, 6, 4, 3, 6, 0, 7, 5, 4, 3 }
            });
            _guess.Add(new Guess() {
                Correct = 1,
                Digits = new List<int>() { 3, 1, 7, 4, 2, 4, 8, 4, 3, 9, 4, 6, 5, 8, 5, 8 }
            });
            _guess.Add(new Guess() {
                Correct = 2,
                Digits = new List<int>() { 4, 5, 1, 3, 5, 5, 9, 0, 9, 4, 1, 4, 6, 1, 1, 7 }
            });
            _guess.Add(new Guess() {
                Correct = 3,
                Digits = new List<int>() { 7, 8, 9, 0, 9, 7, 1, 5, 4, 8, 9, 0, 8, 0, 6, 7 }
            });
            _guess.Add(new Guess() {
                Correct = 1,
                Digits = new List<int>() { 8, 1, 5, 7, 3, 5, 6, 3, 4, 4, 1, 1, 8, 4, 8, 3 }
            });
            _guess.Add(new Guess() {
                Correct = 2,
                Digits = new List<int>() { 2, 6, 1, 5, 2, 5, 0, 7, 4, 4, 3, 8, 6, 8, 9, 9 }
            });
            _guess.Add(new Guess() {
                Correct = 3,
                Digits = new List<int>() { 8, 6, 9, 0, 0, 9, 5, 8, 5, 1, 5, 2, 6, 2, 5, 4 }
            });
            _guess.Add(new Guess() {
                Correct = 1,
                Digits = new List<int>() { 6, 3, 7, 5, 7, 1, 1, 9, 1, 5, 0, 7, 7, 0, 5, 0 }
            });
            _guess.Add(new Guess() {
                Correct = 1,
                Digits = new List<int>() { 6, 9, 1, 3, 8, 5, 9, 1, 7, 3, 1, 2, 1, 3, 6, 0 }
            });
            _guess.Add(new Guess() {
                Correct = 2,
                Digits = new List<int>() { 6, 4, 4, 2, 8, 8, 9, 0, 5, 5, 0, 4, 2, 7, 6, 8 }
            });
            _guess.Add(new Guess() {
                Correct = 0,
                Digits = new List<int>() { 2, 3, 2, 1, 3, 8, 6, 1, 0, 4, 3, 0, 3, 8, 4, 5 }
            });
            _guess.Add(new Guess() {
                Correct = 2,
                Digits = new List<int>() { 2, 3, 2, 6, 5, 0, 9, 4, 7, 1, 2, 7, 1, 4, 4, 8 }
            });
            _guess.Add(new Guess() {
                Correct = 2,
                Digits = new List<int>() { 5, 2, 5, 1, 5, 8, 3, 3, 7, 9, 6, 4, 4, 3, 2, 2 }
            });
            _guess.Add(new Guess() {
                Correct = 3,
                Digits = new List<int>() { 1, 7, 4, 8, 2, 7, 0, 4, 7, 6, 7, 5, 8, 2, 7, 6 }
            });
            _guess.Add(new Guess() {
                Correct = 1,
                Digits = new List<int>() { 4, 8, 9, 5, 7, 2, 2, 6, 5, 2, 1, 9, 0, 3, 0, 6 }
            });
            _guess.Add(new Guess() {
                Correct = 3,
                Digits = new List<int>() { 3, 0, 4, 1, 6, 3, 1, 1, 1, 7, 2, 2, 4, 6, 3, 5 }
            });
            _guess.Add(new Guess() {
                Correct = 3,
                Digits = new List<int>() { 1, 8, 4, 1, 2, 3, 6, 4, 5, 4, 3, 2, 4, 5, 8, 9 }
            });
            _guess.Add(new Guess() {
                Correct = 2,
                Digits = new List<int>() { 2, 6, 5, 9, 8, 6, 2, 6, 3, 7, 3, 1, 6, 8, 6, 7 }
            });
            _guess = _guess.OrderBy(x => x.Correct).ToList();
        }

        private class Guess {
            public List<int> Digits { get; set; }
            public int Correct { get; set; }
        }
    }
}