using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems.AdventOfCode.Y2018 {
    public class Problem05 : AdventOfCodeBase {
        public override string ProblemName {
            get { return "Advent of Code 2018: 5"; }
        }

        public override string GetAnswer() {
            BuildHash();
            return Answer2();
        }

        private string Answer1() {
            var poly = GetPoly(Input().First());
            poly = Collapse(poly);
            return Count(poly).ToString();
        }

        private string Answer2() {
            var start = (int)'A';
            int lowest = int.MaxValue;
            for (int count = 1; count <= 26; count++) {
                var poly = GetPoly(Input().First());
                poly = Remove(poly, (char)start);
                poly = Collapse(poly);
                int sum = Count(poly);
                if (sum < lowest) {
                    lowest = sum;
                }
                start++;
            }
            return lowest.ToString();
        }

        private string PrintPoly(Poly poly) {
            StringBuilder text = new StringBuilder();
            while (poly != null) {
                text.Append(poly.Name);
                poly = poly.Next;
            }
            return text.ToString();
        }

        private Poly GetPoly(string text) {
            Poly poly = null;
            for (int index = text.Length - 1; index >= 0; index--) {
                poly = new Poly() { Name = text[index], Next = poly };
            }
            return poly;
        }

        private Dictionary<char, char> _hash = new Dictionary<char, char>();
        private void BuildHash() {
            var start = (int)'a';
            var end = (int)'A';
            for (int count = 1; count <= 26; count++) {
                _hash.Add((char)start, (char)end);
                _hash.Add((char)end, (char)start);
                start++;
                end++;
            }
        }

        private Poly Collapse(Poly poly) {
            bool keepGoing = false;
            var top = poly;
            var current = top;
            Poly prior = null;
            do {
                keepGoing = false;
                current = top;
                do {
                    if (_hash[current.Name] == current.Next.Name) {
                        if (current == top) {
                            prior = null;
                            top = current.Next.Next;
                            current = top;
                        } else {
                            prior.Next = current.Next.Next;
                            current = prior.Next;
                        }
                        keepGoing = true;
                    } else {
                        prior = current;
                        current = current.Next;
                    }
                } while (current != null && current.Next != null);
            } while (keepGoing);
            return top;
        }

        private Poly Remove(Poly poly, char subToRemove) {
            var top = poly;
            while (poly.Name == subToRemove || poly.Name == _hash[subToRemove]) {
                top = poly.Next;
                poly = top;
            }
            var current = top;
            Poly prior = null;
            while (current != null) {
                if (current.Name == subToRemove || current.Name == _hash[subToRemove]) {
                    prior.Next = current.Next;
                    current = prior.Next;
                } else {
                    prior = current;
                    current = current.Next;
                }
            }
            return top;
        }

        private int Count(Poly poly) {
            int count = 0;
            while (poly != null) {
                count++;
                poly = poly.Next;
            }
            return count;
        }

        private List<string> TestInput() {
            return new List<string>() { "dabAcCaCBAcCcaDA" };
        }

        private class Poly {
            public char Name { get; set; }
            public Poly Next { get; set; }
        }
    }
}
