using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem61 : ProblemBase {
        private int[] _combos;
        private HashSet<int> _uniqueCombos;
        List<Dictionary<int, int>> _polygonals;
        List<Dictionary<string, List<int>>> _subs;

        public override string ProblemName {
            get { return "61: Cyclical figurate numbers"; }
        }

        public override string GetAnswer() {
            _polygonals = GetPolygonals();
            _subs = GetSubs(_polygonals);
            _combos = new int[6];
            _uniqueCombos = new HashSet<int>();

            HashSet<int> triedCombos = new HashSet<int>();
            triedCombos.Add(0);
            foreach (int key in _polygonals[0].Keys) {
                _combos[0] = key;
                _uniqueCombos.Add(_polygonals[0][key]);
                if (TryCombos(new HashSet<int>(triedCombos), 1)) {
                    int answer = _combos[0] + _combos[1] + _combos[2] + _combos[3] + _combos[4] + _combos[5];
                    return answer.ToString();
                }
                _uniqueCombos.Remove(_polygonals[0][key]);
            }

            return "";
        }

        private bool TryCombos(HashSet<int> triedCombos, int level) {
            string sub = _combos[level - 1].ToString().Substring(2, 2);
            for (int i = 1; i < 6; i++) {
                if (!triedCombos.Contains(i)) {
                    if (_subs[i].ContainsKey(sub)) {
                        foreach (int key in _subs[i][sub]) {
                            _combos[level] = key;
                            if (!_uniqueCombos.Contains(_polygonals[i][key])) {
                                if (level == 5) {
                                    if (_combos[level].ToString().Substring(2, 2) == _combos[0].ToString().Substring(0, 2)) {
                                        return true;
                                    }
                                } else {
                                    _uniqueCombos.Add(_polygonals[i][key]);
                                    HashSet<int> newTriedCombos = new HashSet<int>(triedCombos);
                                    newTriedCombos.Add(i);
                                    if (TryCombos(newTriedCombos, level + 1) == true) {
                                        return true;
                                    }
                                    _uniqueCombos.Remove(_polygonals[i][key]);
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }

        private List<Dictionary<string, List<int>>> GetSubs(List<Dictionary<int, int>> polygonals) {
            List<Dictionary<string, List<int>>> subs = new List<Dictionary<string, List<int>>>();

            int i = 0;
            foreach (Dictionary<int, int> polygonal in polygonals) {
                subs.Add(new Dictionary<string, List<int>>());
                foreach (int number in polygonal.Keys) {
                    string sub = number.ToString().Substring(0, 2);
                    if (!subs[i].ContainsKey(sub)) {
                        subs[i].Add(sub, new List<int>());
                    }
                    subs[i][sub].Add(number);
                }

                i++;
            }
            return subs;
        }

        private List<Dictionary<int, int>> GetPolygonals() {
            List<Dictionary<int, int>> polygonals = new List<Dictionary<int, int>>();
            polygonals.Add(GetPolygonalTriangle());
            polygonals.Add(GetPolygonalSquare());
            polygonals.Add(GetPolygonalPentagonal());
            polygonals.Add(GetPolygonalHexagonal());
            polygonals.Add(GetPolygonalHeptagonal());
            polygonals.Add(GetPolygonalOctagonal());
            return polygonals;
        }

        private Dictionary<int, int> GetPolygonalTriangle() {
            Dictionary<int, int> numbers = new Dictionary<int, int>();
            int index = 1;
            int number = 1;
            do {
                number = (index * (index + 1)) / 2;
                if (number > 999 && number < 10000) {
                    numbers.Add(number, index);
                }
                index++;
            } while (number < 10000);
            return numbers;
        }

        private Dictionary<int, int> GetPolygonalSquare() {
            Dictionary<int, int> numbers = new Dictionary<int, int>();
            int index = 1;
            int number = 1;
            do {
                number = index * index;
                if (number > 999 && number < 10000) {
                    numbers.Add(number, index);
                }
                index++;
            } while (number < 10000);
            return numbers;
        }

        private Dictionary<int, int> GetPolygonalPentagonal() {
            Dictionary<int, int> numbers = new Dictionary<int, int>();
            int index = 1;
            int number = 1;
            do {
                number = (index * ((3 * index) - 1)) / 2;
                if (number > 999 && number < 10000) {
                    numbers.Add(number, index);
                }
                index++;
            } while (number < 10000);
            return numbers;
        }

        private Dictionary<int, int> GetPolygonalHexagonal() {
            Dictionary<int, int> numbers = new Dictionary<int, int>();
            int index = 1;
            int number = 1;
            do {
                number = index * ((index * 2) - 1);
                if (number > 999 && number < 10000) {
                    numbers.Add(number, index);
                }
                index++;
            } while (number < 10000);
            return numbers;
        }

        private Dictionary<int, int> GetPolygonalHeptagonal() {
            Dictionary<int, int> numbers = new Dictionary<int, int>();
            int index = 1;
            int number = 1;
            do {
                number = (index * ((index * 5) - 3)) / 2;
                if (number > 999 && number < 10000) {
                    numbers.Add(number, index);
                }
                index++;
            } while (number < 10000);
            return numbers;
        }

        private Dictionary<int, int> GetPolygonalOctagonal() {
            Dictionary<int, int> numbers = new Dictionary<int, int>();
            int index = 1;
            int number = 1;
            do {
                number = index * ((index * 3) - 2);
                if (number > 999 && number < 10000) {
                    numbers.Add(number, index);
                }
                index++;
            } while (number < 10000);
            return numbers;
        }
    }
}
