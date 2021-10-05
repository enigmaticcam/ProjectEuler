using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems.AdventOfCode.Y2017 {
    public class Problem16 : AdventOfCodeBase {
        private List<string> _moves;
        private int _index = 0;
        private char[] _programs;

        public override string ProblemName {
            get { return "Advent of Code 2017: 16"; }
        }

        public override string GetAnswer() {
            //return Answer1(TestInput(), "abcde").ToString();
            return Answer2(Input(), "abcdefghijklmnop").ToString();
        }

        private string Answer1(List<string> input, string programs) {
            SetPrograms(programs);
            GetMoves(input);
            PerformMoves();
            return new string(GetFinal());
        }

        private string Answer2(List<string> input, string programs) {
            SetPrograms(programs);
            GetMoves(input);
            return Find();
        }

        private string Find() {
            var results = new Dictionary<string, int>();
            int count = 0;
            do {
                var key = new string(_programs);
                if (results.ContainsKey(key)) {
                    var leftover = 1000000000 % count;
                    return results.Where(x => x.Value == leftover).First().Key;
                } else {
                    results.Add(key, count);
                }
                PerformMoves();
                count++;
            } while (true);
        }

        private char[] GetFinal() {
            var final = new char[_programs.Length];
            for (int count = 0; count < _programs.Length; count++) {
                var index = (_index + count) % _programs.Length;
                final[count] = _programs[index];
            }
            return final;
        }

        private void SetPrograms(string programs) {
            _programs = programs.ToCharArray();
        }
        
        private void PerformMoves() {
            foreach (var move in _moves) {
                if (move[0] == 's') {
                    var count = Convert.ToInt32(move.Substring(1));
                    _index -= count;
                    if (_index < 0) {
                        _index += _programs.Length;
                    }
                } else if (move[0] == 'x') {
                    var split = move.Substring(1).Split('/');
                    var positionA = (Convert.ToInt32(split[0]) + _index) % _programs.Length;
                    var positionB = (Convert.ToInt32(split[1]) + _index) % _programs.Length;
                    var temp = _programs[positionA];
                    _programs[positionA] = _programs[positionB];
                    _programs[positionB] = temp;
                } else {
                    var split = move.Substring(1).Split('/');
                    int positionA = -1;
                    int positionB = -1;
                    for (int index = 0; index < _programs.Length; index++) {
                        if (_programs[index] == split[0][0]) {
                            positionA = index;
                            if (positionB > -1) {
                                break;
                            }
                        } else if (_programs[index] == split[1][0]) {
                            positionB = index;
                            if (positionA > -1) {
                                break;
                            }
                        }
                    }
                    var temp = _programs[positionA];
                    _programs[positionA] = _programs[positionB];
                    _programs[positionB] = temp;
                }
            }
        }

        private void GetMoves(List<string> input) {
            _moves = input[0].Split(',').ToList();
        }

        private List<string> TestInput() {
            return new List<string>() {
                "s1,x3/4,pe/b"
            };
        }
    }
}
