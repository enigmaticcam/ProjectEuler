using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems.AdventOfCode.Y2020 {
    public class Problem22 : AdventOfCodeBase {
        private List<LinkedList<int>> _cards;

        public override string ProblemName => "Advent of Code 2020: 22";

        public override string GetAnswer() {
            return Answer2(Input()).ToString();
        }

        private ulong Answer1(List<string> input) {
            GetCards(input);
            PlayGame1();
            return GetScore();
        }

        private ulong Answer2(List<string> input) {
            GetCards(input);
            PlayGame2();
            return GetScore();
        }

        private ulong GetScore() {
            ulong score = 0;
            var cards = _cards[0];
            if (cards.Count == 0) {
                cards = _cards[1];
            }
            ulong count = (ulong)cards.Count;
            foreach (var card in cards) {
                score += (ulong)card * count;
                count--;
            }
            return score;
        }

        private void PlayGame1() {
            int total = _cards[0].Count + _cards[1].Count;
            int[] totals = new int[2];
            totals[0] = _cards[0].Count;
            totals[1] = _cards[1].Count;
            do {
                var first = _cards[0].First;
                var second = _cards[1].First;
                if (first.Value > second.Value) {
                    _cards[0].RemoveFirst();
                    _cards[1].RemoveFirst();
                    _cards[0].AddLast(first);
                    _cards[0].AddLast(second);
                    totals[0]++;
                    totals[1]--;
                } else {
                    _cards[0].RemoveFirst();
                    _cards[1].RemoveFirst();
                    _cards[1].AddLast(second);
                    _cards[1].AddLast(first);
                    totals[0]--;
                    totals[1]++;
                }
            } while (totals[0] != 0 && totals[1] != 0);
        }

        private void PlayGame2() {
            int total = _cards[0].Count + _cards[1].Count;
            int[] totals = new int[2];
            totals[0] = _cards[0].Count;
            totals[1] = _cards[1].Count;
            Recursion(_cards, totals);
        }

        private bool Recursion(List<LinkedList<int>> cards, int[] totals) {
            var hash = new HashSet<string>();
            do {
                var key = GetKey(cards);
                if (hash.Contains(key)) {
                    return true;
                }
                hash.Add(key);
                var first = cards[0].First;
                var second = cards[1].First;
                bool player1Wins = true;
                if (totals[0] > first.Value && totals[1] > second.Value) {
                    var newCards = new List<LinkedList<int>>() { new LinkedList<int>(), new LinkedList<int>() };
                    var card = first.Next;
                    for (int count = 1; count <= first.Value; count++) {
                        newCards[0].AddLast(card.Value);
                        card = card.Next;
                    }
                    card = second.Next;
                    for (int count = 1; count <= second.Value; count++) {
                        newCards[1].AddLast(card.Value);
                        card = card.Next;
                    }
                    var newTotals = new int[2];
                    newTotals[0] = first.Value;
                    newTotals[1] = second.Value;
                    player1Wins = Recursion(newCards, newTotals);
                } else if (first.Value < second.Value) {
                    player1Wins = false;
                }
                if (player1Wins) {
                    cards[0].RemoveFirst();
                    cards[1].RemoveFirst();
                    cards[0].AddLast(first);
                    cards[0].AddLast(second);
                    totals[0]++;
                    totals[1]--;
                } else {
                    cards[0].RemoveFirst();
                    cards[1].RemoveFirst();
                    cards[1].AddLast(second);
                    cards[1].AddLast(first);
                    totals[0]--;
                    totals[1]++;
                }
            } while (totals[0] != 0 && totals[1] != 0);
            return totals[0] != 0;
        }

        private string GetKey(List<LinkedList<int>> cards) {
            var key = new StringBuilder();
            foreach (var card in cards[0]) {
                key.Append(card);
            }
            key.Append(" ");
            foreach (var card in cards[1]) {
                key.Append(card);
            }
            return key.ToString();
        }

        private void GetCards(List<string> input) {
            _cards = new List<LinkedList<int>>();
            var cards = new LinkedList<int>();
            foreach (var line in input) {
                if (line == "") {
                    _cards.Add(cards);
                    cards = new LinkedList<int>();
                } else if (line[0] != 'P') {
                    cards.AddLast(Convert.ToInt32(line));
                }
            }
            _cards.Add(cards);
        }

        private List<string> TestInput() {
            return new List<string>() {
                "Player 1:",
                "9",
                "2",
                "6",
                "3",
                "1",
                "",
                "Player 2:",
                "5",
                "8",
                "4",
                "7",
                "10"
            };
        }
    }
}
