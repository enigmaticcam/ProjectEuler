using System;
using System.Collections.Generic;
using System.Linq;

namespace Euler_Logic.Problems.AdventOfCode.Y2017 {
    public class Problem07 : AdventOfCodeBase {
        public override string ProblemName {
            get { return "Advent of Code 2017: 7"; }
        }

        public override string GetAnswer() {
            return Answer1(Input());
        }

        public override string GetAnswer2() {
            return Answer2(Input());
        }

        private Dictionary<string, Tower> _towers = new Dictionary<string, Tower>();
        private string Answer1(List<string> input) {
            OrderTowers(input);
            return GetHighestTower().Name;
        }

        private Dictionary<Tower, int> _badTowers = new Dictionary<Tower, int>();
        private string Answer2(List<string> input) {
            OrderTowers(input);
            foreach (Tower tower in _towers.Values) {
                Dictionary<int, int> weightCounts = new Dictionary<int, int>();
                foreach (Tower child in tower.Children) {
                    int weight = GetTotalWeight(child);
                    if (weightCounts.ContainsKey(weight)) {
                        weightCounts[weight]++;
                    } else {
                        weightCounts.Add(weight, 1);
                    }
                }
                if (weightCounts.Count > 1) {
                    if (weightCounts[weightCounts.Keys.ElementAt(0)] > weightCounts[weightCounts.Keys.ElementAt(1)]) {
                        Tower badTower = tower.Children.Where(x => x.TotalWeight == weightCounts.Keys.ElementAt(1)).First();
                        _badTowers.Add(badTower, badTower.Weight + (weightCounts.Keys.ElementAt(1) - weightCounts.Keys.ElementAt(0)));
                    } else {
                        Tower badTower = tower.Children.Where(x => x.TotalWeight == weightCounts.Keys.ElementAt(0)).First();
                        _badTowers.Add(badTower, badTower.Weight + (weightCounts.Keys.ElementAt(1) - weightCounts.Keys.ElementAt(0)));
                    }
                }
            }
            return _badTowers[GetBadTower()].ToString();
        }

        private Tower GetBadTower() {
            foreach (Tower towerA in _badTowers.Keys) {
                bool hasChild = false;
                foreach (Tower towerB in _badTowers.Keys) {
                    if (towerB.Name != towerA.Name && towerB.Parent != null && towerB.Parent.Name == towerA.Name) {
                        hasChild = true;
                        break;
                    }
                }
                if (!hasChild) {
                    return towerA;
                }
            }
            throw new Exception();
        }

        private void OrderTowers(List<string> input) {
            foreach (string text in input) {
                string name = GetTowerName(text);
                Tower tower = GetTower(name);
                tower.Weight = GetTowerWeight(text);
                string[] children = GetTowerChildren(text);
                foreach (string child in children) {
                    Tower towerChild = GetTower(child);
                    towerChild.Parent = tower;
                    tower.Children.Add(towerChild);
                }
            }
        }

        private int GetTotalWeight(Tower tower) {
            if (tower.DidGetTotalWeight) {
                return tower.TotalWeight;
            }
            int sum = tower.Weight;
            foreach (Tower child in tower.Children) {
                sum += GetTotalWeight(child);
            }
            tower.TotalWeight = sum;
            tower.DidGetTotalWeight = true;
            return sum;
        }

        private Tower GetHighestTower() {
            Tower tower = _towers[_towers.Keys.ElementAt(0)];
            do {
                if (tower.Parent != null) {
                    tower = tower.Parent;
                } else {
                    return tower;
                }
            } while (true);
        }

        private string GetTowerName(string text) {
            return text.Substring(0, text.IndexOf(' '));
        }

        private string[] GetTowerChildren(string text) {
            int arrow = text.IndexOf("->");
            if (arrow > -1) {
                return text.Substring(arrow + 3, text.Length - arrow - 3).Split(new string[] { ", " }, StringSplitOptions.None);
            } else {
                return new string[0];
            }
        }

        private int GetTowerWeight(string text) {
            int start = text.IndexOf('(') + 1;
            int length = text.IndexOf(')') - start;
            return Convert.ToInt32(text.Substring(start, length));
        }

        private Tower GetTower(string name) {
            if (_towers.ContainsKey(name)) {
                return _towers[name];
            } else {
                Tower tower = new Tower();
                tower.Name = name;
                _towers.Add(name, tower);
                return tower;
            }
        }

        private class Tower {
            public Tower() {
                this.Children = new List<Tower>();
            }

            public string Name { get; set; }
            public Tower Parent { get; set; }
            public int Weight { get; set; }
            public List<Tower> Children { get; set; }
            public int TotalWeight { get; set; }
            public bool DidGetTotalWeight { get; set; }
        }
    }
}
