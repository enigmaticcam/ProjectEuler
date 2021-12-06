using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems.AdventOfCode.Y2020 {
    public class Problem21 : AdventOfCodeBase {
        private List<Food> _foods;
        private HashSet<string> _ingredients;
        private List<string> _noAllergens;
        private Dictionary<string, HashSet<string>> _allergens;

        public override string ProblemName => "Advent of Code 2020: 21";

        public override string GetAnswer() {
            return Answer2(Input()).ToString();
        }

        private int Answer1(List<string> input) {
            GetFoods(input);
            GetIngredients();
            FindAllergens();
            FindNoAllergens();
            return GetCountNoAppear();
        }

        private string Answer2(List<string> input) {
            GetFoods(input);
            GetIngredients();
            FindAllergens();
            Reduce();
            return GetCanonicalList();
        }

        private string GetCanonicalList() {
            var ordered = _allergens.Select(x => x.Key).OrderBy(x => x);
            return string.Join(",", ordered.Select(x => _allergens[x].First()));
        }

        private void Reduce() {
            bool keepGoing = false;
            var finished = new HashSet<string>();
            do {
                keepGoing = false;
                foreach (var allergen in _allergens) {
                    if (!finished.Contains(allergen.Key) && allergen.Value.Count == 1) {
                        keepGoing = true;
                        foreach (var toRemove in _allergens) {
                            if (toRemove.Key != allergen.Key) {
                                toRemove.Value.Remove(allergen.Value.First());
                            }
                        }
                        finished.Add(allergen.Key);
                    }
                }
            } while (keepGoing);
        }

        private int GetCountNoAppear() {
            int count = 0;
            foreach (var ingredient in _noAllergens) {
                foreach (var food in _foods) {
                    if (food.Ingredients.Contains(ingredient)) {
                        count++;
                    }
                }
            }
            return count;
        }

        private void FindAllergens() {
            _allergens = new Dictionary<string, HashSet<string>>();
            foreach (var food in _foods) {
                foreach (var allergen in food.Allergens) {
                    if (!_allergens.ContainsKey(allergen)) {
                        _allergens.Add(allergen, new HashSet<string>(food.Ingredients));
                    } else {
                        var toRemove = new List<string>();
                        foreach (var ingredient in _allergens[allergen]) {
                            if (!food.Ingredients.Contains(ingredient)) {
                                toRemove.Add(ingredient);
                            }
                        }
                        toRemove.ForEach(ingredient => _allergens[allergen].Remove(ingredient));
                    }
                }
            }
        }

        private void FindNoAllergens() {
            var ingredients = new HashSet<string>(_ingredients);
            foreach (var ingredientSet in _allergens.Values) {
                foreach (var ingredient in ingredientSet) {
                    ingredients.Remove(ingredient);
                }
            }
            _noAllergens = new List<string>(ingredients);
        }

        private void GetFoods(List<string> input) {
            _foods = input.Select(line => {
                var food = new Food() { Allergens = new HashSet<string>(), Ingredients = new HashSet<string>() };
                var split = line.Split(' ');
                bool isAllergen = false;
                foreach (var text in split) {
                    if (!isAllergen && text.IndexOf('(') > -1) {
                        isAllergen = true;
                    }
                    if (!isAllergen) {
                        food.Ingredients.Add(text);
                    } else {
                        food.Allergens.Add(text.Replace("(", "").Replace(")", "").Replace(",", ""));
                    }
                }
                food.Allergens.Remove("contains");
                return food;
            }).ToList();
        }

        private void GetIngredients() {
            _ingredients = new HashSet<string>();
            foreach (var food in _foods) {
                foreach (var ingredient in food.Ingredients) {
                    _ingredients.Add(ingredient);
                }
            }
        }

        private List<string> TestInput() {
            return new List<string>() {
                "mxmxvkd kfcds sqjhc nhms (contains dairy, fish)",
                "trh fvjkl sbzzf mxmxvkd (contains dairy)",
                "sqjhc fvjkl (contains soy)",
                "sqjhc mxmxvkd sbzzf (contains fish)"
            };
        }

        private class Food {
            public HashSet<string> Ingredients { get; set; }
            public HashSet<string> Allergens { get; set; }
        }
    }
}
