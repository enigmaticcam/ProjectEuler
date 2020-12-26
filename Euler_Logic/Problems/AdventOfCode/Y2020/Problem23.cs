using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems.AdventOfCode.Y2020 {
    public class Problem23 : AdventOfCodeBase {
        private Cup _cups;
        private Dictionary<int, Cup> _hash;
        private int _lowest = int.MaxValue;
        private int _highest = 0;

        public override string ProblemName => "Advent of Code 2020: 23";

        public override string GetAnswer() {
            //return Answer2("389125467").ToString();
            return Answer2("368195742").ToString();
        }

        private string Answer1(string input) {
            CreateCups(input, false);
            PlayGame(100);
            return GetResult1();
        }

        private ulong Answer2(string input) {
            CreateCups(input, true);
            PlayGame(10000000);
            return GetResult2();
        }

        private string GetResult1() {
            var result = "";
            var current = _hash[1].Next;
            do {
                result += current.Value;
                current = current.Next;
            } while (current.Value != 1);
            return result;
        }

        private ulong GetResult2() {
            var one = _hash[1].Next;
            return (ulong)one.Value * (ulong)one.Next.Value;
        }

        private void PlayGame(int moves) {
            var current = _cups;
            for (int turn = 1; turn <= moves; turn++) {
                var removedFirst = current.Next;
                var removedLast = removedFirst.Next.Next;
                current.Next = removedLast.Next;
                removedFirst.InList = false;
                removedFirst.Next.InList = false;
                removedLast.InList = false;
                var next = current.Value - 1;
                if (next < _lowest) {
                    next = _highest;
                }
                while (!_hash[next].InList) {
                    next--;
                    if (next < _lowest) {
                        next = _highest;
                    }
                }
                removedFirst.InList = true;
                removedFirst.Next.InList = true;
                removedLast.InList = true;
                var destination = _hash[next];
                removedLast.Next = destination.Next;
                destination.Next = removedFirst;
                current = current.Next;
            }
        }

        private Cup _lastCup = null;
        private void CreateCups(string input, bool addRemainingMillion) {
            _hash = new Dictionary<int, Cup>();
            foreach (var digit in input) {
                var num = Convert.ToInt32(digit.ToString());
                AddCup(num);
            }
            for (int num = _highest + 1; num <= 1000000; num++) {
                AddCup(num);
            }
            _lastCup.Next = _cups;
        }

        private void AddCup(int num) {
            var cup = new Cup() {
                Value = num,
                InList = true
            };
            if (_lastCup != null) {
                _lastCup.Next = cup;
            } else {
                _cups = cup;
            }
            _lastCup = cup;
            _hash.Add(num, cup);
            if (num < _lowest) {
                _lowest = num;
            }
            if (num > _highest) {
                _highest = num;
            }
        }

        private class Cup {
            public Cup Next { get; set; }
            public int Value { get; set; }
            public bool InList { get; set; }
        }
    }
}
