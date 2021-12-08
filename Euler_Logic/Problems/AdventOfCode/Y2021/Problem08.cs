using System;
using System.Collections.Generic;
using System.Linq;

namespace Euler_Logic.Problems.AdventOfCode.Y2021 {
    public class Problem08 : AdventOfCodeBase {
        private List<Display> _displays;
        private Dictionary<char, ulong> _charToBit;
        private ulong _maxBits;
        private Dictionary<ulong, int> _bitsToNum;

        public override string ProblemName {
            get { return "Advent of Code 2021: 07"; }
        }

        public override string GetAnswer() {
            SetCharToBit();
            GetDisplays(Input());
            return Answer2().ToString();
        }

        private int Answer1() {
            int count = 0;
            foreach (var display in _displays) {
                count += display.Final.Where(x => x.Length == 2 || x.Length == 3 || x.Length == 4 || x.Length == 7).Count();
            }
            return count;
        }

        private int Answer2() {
            int sum = 0;
            SetBitsToNum();
            foreach (var display in _displays) {
                var hash = DeduceDisplay(display);
                sum += CalcNum(display, hash);
            }
            return sum;
        }

        private int CalcNum(Display display, Dictionary<char, ulong> hash) {
            int final = 0;
            int powerOf10 = 1;
            for (int index = display.Final.Count - 1; index >= 0; index--) {
                var num = display.Final[index];
                ulong bits = 0;
                foreach (var digit in num) {
                    bits += hash[digit];
                }
                final += powerOf10 * _bitsToNum[bits];
                powerOf10 *= 10;
            }
            return final;
        }

        private Dictionary<char, ulong> DeduceDisplay(Display display) {
            var hash = GetHash();
            var counts = GetCounts(display);
            
            // e,g,f can be known immediately
            AddCharToHash(counts.Where(x => x.Value == 4).First().Key, _charToBit['e'], hash);
            AddCharToHash(counts.Where(x => x.Value == 6).First().Key, _charToBit['b'], hash);
            AddCharToHash(counts.Where(x => x.Value == 9).First().Key, _charToBit['f'], hash);

            // d, g can be either/or
            foreach (var digit in _charToBit.Keys) {
                if (counts[digit] == 7) {
                    hash[digit] = _charToBit['d'] + _charToBit['g'];
                } else if (counts[digit] == 8) {
                    hash[digit] = _charToBit['a'] + _charToBit['c'];
                } else {
                    var bitToRemove = hash[digit];
                    if ((bitToRemove & _charToBit['d']) != 0) {
                        bitToRemove -= _charToBit['d'];
                    }
                    if ((bitToRemove & _charToBit['g']) != 0) {
                        bitToRemove -= _charToBit['g'];
                    }
                    if ((bitToRemove & _charToBit['a']) != 0) {
                        bitToRemove -= _charToBit['a'];
                    }
                    if ((bitToRemove & _charToBit['c']) != 0) {
                        bitToRemove -= _charToBit['c'];
                    }
                    hash[digit] = bitToRemove;
                }
            }

            // find a based on pairs A and C
            var digitB = hash.Where(x => x.Value == _charToBit['b']).Select(x => x.Key).First();
            var digitAorC = hash.Where(x => x.Value == 5).Select(x => x.Key).First();
            var bits = _charToBit[digitB] + _charToBit[digitAorC];
            var pairs = display.Bits.Where(x => (x & bits) == bits).Count();
            if (pairs == 5) {
                AddCharToHash(digitAorC, _charToBit['a'], hash);
            } else {
                AddCharToHash(digitAorC, _charToBit['c'], hash);
            }

            // find a based on pairs D and G
            var digitE = hash.Where(x => x.Value == _charToBit['e']).Select(x => x.Key).First();
            var digitDorG = hash.Where(x => x.Value == 72).Select(x => x.Key).First();
            bits = _charToBit[digitE] + _charToBit[digitDorG];
            pairs = display.Bits.Where(x => (x & bits) == bits).Count();
            if (pairs == 3) {
                AddCharToHash(digitDorG, _charToBit['d'], hash);
            } else {
                AddCharToHash(digitDorG, _charToBit['g'], hash);
            }

            return hash;
        }

        private void AddCharToHash(char digit, ulong bit, Dictionary<char, ulong> hash) {
            foreach (var key in _charToBit.Keys) {
                if (key == digit) {
                    hash[key] = bit;
                } else if ((hash[key] & bit) == bit) {
                    hash[key] -= bit;
                }
            }
        }

        private Dictionary<char, ulong> GetHash() {
            var hash = new Dictionary<char, ulong>();
            hash.Add('a', _maxBits);
            hash.Add('b', _maxBits);
            hash.Add('c', _maxBits);
            hash.Add('d', _maxBits);
            hash.Add('e', _maxBits);
            hash.Add('f', _maxBits);
            hash.Add('g', _maxBits);
            return hash;
        }

        private Dictionary<char, int> GetCounts(Display display) {
            var counts = new Dictionary<char, int>();
            foreach (var word in display.Digits) {
                foreach (var letter in word) {
                    if (!counts.ContainsKey(letter)) {
                        counts.Add(letter, 1);
                    } else {
                        counts[letter]++;
                    }
                }
            }
            return counts;
        }

        private void GetDisplays(List<string> input) {
            _displays = new List<Display>();
            foreach (var line in input) {
                var split = line.Split(' ');
                var display = new Display() { Digits = new List<string>(), Final = new List<string>(), Bits = new List<ulong>() };
                for (int count = 0; count < 10; count++) {
                    display.Digits.Add(split[count]);
                    ulong bits = 0;
                    foreach (var digit in split[count]) {
                        bits += _charToBit[digit];
                    }
                    display.Bits.Add(bits);
                }
                for (int count = 0; count < 4; count++) {
                    display.Final.Add(split[11 + count]);
                }
                _displays.Add(display);
            }
        }

        private void SetBitsToNum() {
            _bitsToNum = new Dictionary<ulong, int>();
            _bitsToNum.Add(_charToBit['a'] + _charToBit['b'] + _charToBit['c'] + _charToBit['e'] + _charToBit['f'] + _charToBit['g'], 0);
            _bitsToNum.Add(_charToBit['c'] + _charToBit['f'], 1);
            _bitsToNum.Add(_charToBit['a'] + _charToBit['c'] + _charToBit['d'] + _charToBit['e'] + _charToBit['g'], 2);
            _bitsToNum.Add(_charToBit['a'] + _charToBit['c'] + _charToBit['d'] + _charToBit['f'] + _charToBit['g'], 3);
            _bitsToNum.Add(_charToBit['b'] + _charToBit['c'] + _charToBit['d'] + _charToBit['f'], 4);
            _bitsToNum.Add(_charToBit['a'] + _charToBit['b'] + _charToBit['d'] + _charToBit['f'] + _charToBit['g'], 5);
            _bitsToNum.Add(_charToBit['a'] + _charToBit['b'] + _charToBit['d'] + _charToBit['e'] + _charToBit['f'] + _charToBit['g'], 6);
            _bitsToNum.Add(_charToBit['a'] + _charToBit['c'] + _charToBit['f'], 7);
            _bitsToNum.Add(_charToBit['a'] + _charToBit['b'] + _charToBit['c'] + _charToBit['d'] + _charToBit['e'] + _charToBit['f'] + _charToBit['g'], 8);
            _bitsToNum.Add(_charToBit['a'] + _charToBit['b'] + _charToBit['c'] + _charToBit['d'] + _charToBit['f'] + _charToBit['g'], 9);
        }

        private void SetCharToBit() {
            _charToBit = new Dictionary<char, ulong>();
            _charToBit.Add('a', 1);
            _charToBit.Add('b', 2);
            _charToBit.Add('c', 4);
            _charToBit.Add('d', 8);
            _charToBit.Add('e', 16);
            _charToBit.Add('f', 32);
            _charToBit.Add('g', 64);
            _maxBits = (ulong)Math.Pow(2, 7) - 1;
        }

        private class Display {
            public List<string> Digits { get; set; }
            public List<string> Final { get; set; }
            public List<ulong> Bits { get; set; }
        }
    }
}
