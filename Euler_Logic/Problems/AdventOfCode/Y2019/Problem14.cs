using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems.AdventOfCode.Y2019 {
    public class Problem14 : AdventOfCodeBase {
        public override string ProblemName {
            get { return "Advent of Code 2019: 14"; }
        }

        public override string GetAnswer() {
            _reactions = new Dictionary<string, Reaction>();
            _bucket = new Dictionary<string, int>();
            return Answer2(Input()).ToString();
        }

        private int Answer1(List<string> input) {
            BuildReactions(input);
            return GetOreCount();
        }

        private int Answer2(List<string> input) {
            BuildReactions(input);
            ulong remaining = 1000000000000;
            int count = 0;
            do {
                _ore = 0;
                GetOreCount();
                if ((ulong)_ore > remaining) {
                    return count;
                }
                remaining -= (ulong)_ore;
                count++;
            } while (true);
        }

        private Dictionary<string, int> _bucket;
        private int _ore = 0;
        private int GetOreCount() {
            MakeChemical("FUEL", 1);
            _bucket["FUEL"]--;
            return _ore;
        }

        private bool MakeChemical(string chemical, int count) {
            if (!_bucket.ContainsKey(chemical)) {
                _bucket.Add(chemical, 0);
            } 
            if (_bucket[chemical] < count) {
                if (chemical == "ORE") {
                    _ore += count - _bucket[chemical];
                    _bucket[chemical] = count;
                    return true;
                }
                while (_bucket[chemical] < count) {
                    var reaction = _reactions[chemical];
                    foreach (var ingredient in reaction.Ingredients) {
                        MakeChemical(ingredient.Chemical, ingredient.Count);
                        _bucket[ingredient.Chemical] -= ingredient.Count;
                    }
                    _bucket[chemical] += reaction.Count;
                }
                return true;
            }
            return false;
        }

        private Dictionary<string, Reaction> _reactions;
        private void BuildReactions(List<string> input) {
            int space = 0;
            foreach (var reaction in input) {
                var next = new Reaction() { Ingredients = new List<Ingredient>() };
                var middle = reaction.IndexOf("=>");
                var left = reaction.Substring(0, middle - 1).Split(',');
                foreach (var ingredient in left) {
                    var toAdd = new Ingredient();
                    var trimmed = ingredient.Trim();
                    space = trimmed.IndexOf(' ');
                    toAdd.Count = Convert.ToInt32(trimmed.Substring(0, space));
                    toAdd.Chemical = trimmed.Substring(space, trimmed.Length - space).Trim();
                    next.Ingredients.Add(toAdd);
                }
                var right = reaction.Substring(middle + 2, reaction.Length - middle - 2).Trim();
                space = right.IndexOf(' ');
                next.Count = Convert.ToInt32(right.Substring(0, space));
                next.Chemical = right.Substring(space, right.Length - space).Trim();
                _reactions.Add(next.Chemical, next);
            }
        }

        private List<string> Test1Input() {
            return new List<string>() {
                "10 ORE => 10 A",
                "1 ORE => 1 B",
                "7 A, 1 B => 1 C",
                "7 A, 1 C => 1 D",
                "7 A, 1 D => 1 E",
                "7 A, 1 E => 1 FUEL"
            };
        }

        private List<string> Test2Input() {
            return new List<string>() {
                "9 ORE => 2 A",
                "8 ORE => 3 B",
                "7 ORE => 5 C",
                "3 A, 4 B => 1 AB",
                "5 B, 7 C => 1 BC",
                "4 C, 1 A => 1 CA",
                "2 AB, 3 BC, 4 CA => 1 FUEL"
            };
        }

        private List<string> Test3Input() {
            return new List<string>() {
                "157 ORE => 5 NZVS",
                "165 ORE => 6 DCFZ",
                "44 XJWVT, 5 KHKGT, 1 QDVJ, 29 NZVS, 9 GPVTF, 48 HKGWZ => 1 FUEL",
                "12 HKGWZ, 1 GPVTF, 8 PSHF => 9 QDVJ",
                "179 ORE => 7 PSHF",
                "177 ORE => 5 HKGWZ",
                "7 DCFZ, 7 PSHF => 2 XJWVT",
                "165 ORE => 2 GPVTF",
                "3 DCFZ, 7 NZVS, 5 HKGWZ, 10 PSHF => 8 KHKGT"
            };
        }

        private List<string> Test4Input() {
            return new List<string>() {
                "2 VPVL, 7 FWMGM, 2 CXFTF, 11 MNCFX => 1 STKFG",
                "17 NVRVD, 3 JNWZP => 8 VPVL",
                "53 STKFG, 6 MNCFX, 46 VJHF, 81 HVMC, 68 CXFTF, 25 GNMV => 1 FUEL",
                "22 VJHF, 37 MNCFX => 5 FWMGM",
                "139 ORE => 4 NVRVD",
                "144 ORE => 7 JNWZP",
                "5 MNCFX, 7 RFSQX, 2 FWMGM, 2 VPVL, 19 CXFTF => 3 HVMC",
                "5 VJHF, 7 MNCFX, 9 VPVL, 37 CXFTF => 6 GNMV",
                "145 ORE => 6 MNCFX",
                "1 NVRVD => 8 CXFTF",
                "1 VJHF, 6 MNCFX => 4 RFSQX",
                "176 ORE => 6 VJHF"
            };
        }

        private List<string> Test5Input() {
            return new List<string>() {
                "171 ORE => 8 CNZTR",
                "7 ZLQW, 3 BMBT, 9 XCVML, 26 XMNCP, 1 WPTQ, 2 MZWV, 1 RJRHP => 4 PLWSL",
                "114 ORE => 4 BHXH",
                "14 VRPVC => 6 BMBT",
                "6 BHXH, 18 KTJDG, 12 WPTQ, 7 PLWSL, 31 FHTLT, 37 ZDVW => 1 FUEL",
                "6 WPTQ, 2 BMBT, 8 ZLQW, 18 KTJDG, 1 XMNCP, 6 MZWV, 1 RJRHP => 6 FHTLT",
                "15 XDBXC, 2 LTCX, 1 VRPVC => 6 ZLQW",
                "13 WPTQ, 10 LTCX, 3 RJRHP, 14 XMNCP, 2 MZWV, 1 ZLQW => 1 ZDVW",
                "5 BMBT => 4 WPTQ",
                "189 ORE => 9 KTJDG",
                "1 MZWV, 17 XDBXC, 3 XCVML => 2 XMNCP",
                "12 VRPVC, 27 CNZTR => 2 XDBXC",
                "15 KTJDG, 12 BHXH => 5 XCVML",
                "3 BHXH, 2 VRPVC => 7 MZWV",
                "121 ORE => 7 VRPVC",
                "7 XCVML => 6 RJRHP",
                "5 BHXH, 4 VRPVC => 5 LTCX"
            };
        }

        private class Reaction {
            public string Chemical { get; set; }
            public int Count { get; set; }
            public List<Ingredient> Ingredients { get; set; }
        }

        private class Ingredient {
            public string Chemical { get; set; }
            public int Count { get; set; }
        }
    }
}
