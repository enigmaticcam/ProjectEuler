using Euler_Logic.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems.AdventOfCode.Y2020 {
    public class Problem16 : AdventOfCodeBase {
        private List<Rule> _rules;
        private int[] _yourTicket;
        private List<int[]> _nearbyTickets;
        private PowerAll _powerOf2;

        public override string ProblemName => "Advent of Code 2020: 16";

        public override string GetAnswer() {
            return Answer2(Input()).ToString();
        }

        private int Answer1(List<string> input) {
            LoadInput(input);
            return GetTicketScanningErrorRate();
        }

        private ulong Answer2(List<string> input) {
            LoadInput(input);
            return FindFieldOrder();
        }

        private ulong FindFieldOrder() {
            _powerOf2 = new PowerAll(2);
            var tickets = _nearbyTickets.Where(ticket => IsTicketValid(ticket)).ToList();
            tickets.Add(_yourTicket);
            var possibilities = tickets[0].Select(num => _powerOf2.GetPower(tickets[0].Length) - 1).ToList();
            Reduce(tickets, possibilities);
            Remove(possibilities);
            return GetDeparture(possibilities);
        }

        private void Reduce(List<int[]> tickets, List<ulong> possibilities) {
            foreach (var ticket in tickets) {
                var numIndex = 0;
                foreach (var num in ticket) {
                    int index = 0;
                    foreach (var rule in _rules) {
                        bool isGood = false;
                        if (num >= rule.RangeMin1 && num <= rule.RangeMax1) {
                            isGood = true;
                        } else if (num >= rule.RangeMin2 && num <= rule.RangeMax2) {
                            isGood = true;
                        }
                        if (!isGood) {
                            var power = _powerOf2.GetPower(numIndex);
                            if ((possibilities[index] & power) == power) {
                                possibilities[index] -= power;
                            }
                        }
                        index++;
                    }
                    numIndex++;
                }
            }
        }

        private void Remove(List<ulong> possibilites) {
            var hash = new Dictionary<ulong, bool>();
            for (int power = 0; power < possibilites.Count; power++) {
                hash.Add(_powerOf2.GetPower(power), false);
            }
            bool isGood = false;
            do {
                for (int index = 0; index < possibilites.Count; index++) {
                    if (hash.ContainsKey(possibilites[index]) && !hash[possibilites[index]]) {
                        hash[possibilites[index]] = true;
                        for (int next = 0; next < possibilites.Count; next++) {
                            if (next != index && (possibilites[next] & possibilites[index]) == possibilites[index]) {
                                possibilites[next] -= possibilites[index];
                            }
                        }
                    }
                }
                isGood = true;
                foreach (var value in hash.Values) {
                    if (!value) {
                        isGood = false;
                        break;
                    }
                }
            } while (!isGood);
        }

        private ulong GetDeparture(List<ulong> possibilities) {
            ulong sum = 1;
            var indexes = possibilities.Select(poss => _powerOf2.GetLog(poss)).ToList();
            for (int index = 0; index < 6; index++) {
                sum *= (ulong)_yourTicket[indexes[index]];
            }
            return sum;
        }

        private int GetTicketScanningErrorRate() {
            var invalids = new List<int>();
            foreach (var ticket in _nearbyTickets) {
                if (!IsTicketValid(ticket)) {
                    invalids.Add(_invalidNum);
                }
            }
            return invalids.Sum();
        }

        private int _invalidNum;
        private bool IsTicketValid(int[] ticket) {
            foreach (var num in ticket) {
                bool isGood = false;
                foreach (var rule in _rules) {
                    if (num >= rule.RangeMin1 && num <= rule.RangeMax1) {
                        isGood = true;
                        break;
                    } else if (num >= rule.RangeMin2 && num <= rule.RangeMax2) {
                        isGood = true;
                        break;
                    }
                }
                if (!isGood) {
                    _invalidNum = num;
                    return false;
                }
            }
            return true;
        }

        private void LoadInput(List<string> input) {
            var index = GetRules(input);
            index = GetYourTicket(input, index + 1);
            GetNearbyTickets(input, index + 1);
        }

        private int GetRules(List<string> input) {
            var rules = new List<Rule>();
            var index = 0;
            foreach (var line in input) {
                if (line == "") {
                    break;
                }
                var ruleset = line.Skip(line.IndexOf(':') + 2).ToArray();
                var split = new string(ruleset).Split(' ');
                var rule = new Rule();
                rule.Id = line.Substring(0, line.IndexOf(':'));
                rule.RangeMin1 = Convert.ToInt32(split[0].Substring(0, split[0].IndexOf('-')));
                rule.RangeMax1 = Convert.ToInt32(new string(split[0].Skip(split[0].IndexOf('-') + 1).ToArray()));
                rule.RangeMin2 = Convert.ToInt32(split[2].Substring(0, split[2].IndexOf('-')));
                rule.RangeMax2 = Convert.ToInt32(new string(split[2].Skip(split[2].IndexOf('-') + 1).ToArray()));
                rules.Add(rule);
                index++;
            }
            _rules = rules;
            return index;
        }

        private int GetYourTicket(List<string> input, int startIndex) {
            _yourTicket = input[startIndex + 1].Split(',').Select(num => Convert.ToInt32(num)).ToArray();
            return startIndex + 2;
        }

        private void GetNearbyTickets(List<string> input, int startIndex) {
            var nearbyTickets = new List<int[]>();
            for (int index = startIndex + 1; index < input.Count; index++) {
                nearbyTickets.Add(input[index].Split(',').Select(num => Convert.ToInt32(num)).ToArray());
            }
            _nearbyTickets = nearbyTickets;
        }

        private List<string> Test1Input() {
            return new List<string>() {
                "class: 1-3 or 5-7",
                "row: 6-11 or 33-44",
                "seat: 13-40 or 45-50",
                "",
                "your ticket:",
                "7,1,14",
                "",
                "nearby tickets:",
                "7,3,47",
                "40,4,50",
                "55,2,20",
                "38,6,12"
            };
        }

        private List<string> Test2Input() {
            return new List<string>() {
                "class: 0-1 or 4-19",
                "row: 0-5 or 8-19",
                "seat: 0-13 or 16-19",
                "",
                "your ticket:",
                "11,12,13",
                "",
                "nearby tickets:",
                "3,9,18",
                "15,1,5",
                "5,14,9"
            };
        }

        private class Rule {
            public string Id { get; set; }
            public int RangeMin1 { get; set; }
            public int RangeMin2 { get; set; }
            public int RangeMax1 { get; set; }
            public int RangeMax2 { get; set; }
        }
    }
}
