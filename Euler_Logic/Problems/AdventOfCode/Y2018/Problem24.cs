using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems.AdventOfCode.Y2018 {
    public class Problem24 : AdventOfCodeBase {
        private List<Army> _armies;
        private int _whoWon;
        private int _boost;

        public override string ProblemName {
            get { return "Advent of Code 2018: 24"; }
        }

        public override string GetAnswer() {
            return Answer2(Input()).ToString();
        }

        private int Answer1(List<string> input) {
            _boost = 0;
            GetArmies(input);
            return Simulate();
        }

        private int Answer2(List<string> input) {
            GetArmies(input);
            return FindBoost();
        }

        private int FindBoost() {
            do {
                _boost++;
                _armies.ForEach(x => x.UnitCount = x.StartingUnitCount);
                var count = Simulate();
                if (_whoWon == 1) {
                    return count;
                }
            } while (true);
        }

        private int Simulate() {
            int count = 0;
            do {
                count++;
                SetTargets();
                var didDealDamage = PerformAttacks();
                int immuneUnits = _armies.Where(x => x.IsImmuneSystem).Select(x => x.UnitCount).Sum();
                int infectionUnits = _armies.Where(x => !x.IsImmuneSystem).Select(x => x.UnitCount).Sum();
                if (!didDealDamage) {
                    _whoWon = 0;
                    return Math.Max(immuneUnits, infectionUnits);
                } else if (immuneUnits == 0) {
                    _whoWon = -1;
                    return infectionUnits;
                } else if (infectionUnits == 0) {
                    _whoWon = 1;
                    return immuneUnits;
                }
            } while (true);
        }

        private bool PerformAttacks() {
            bool didDealDamage = false;
            foreach (var army in _armies.OrderByDescending(x => x.Initiative)) {
                var target = army.Target;
                if (army.UnitCount > 0 && target != null) {
                    var damage = GetDamage(army, target);
                    var unitsLost = target.UnitCount - Math.Max(0, target.UnitCount - (damage / target.HitPoints));
                    didDealDamage = (didDealDamage | unitsLost > 0);
                    target.UnitCount = Math.Max(0, target.UnitCount - (damage / target.HitPoints));
                }
            }
            return didDealDamage;
        }

        private void SetTargets() {
            _armies.ForEach(army => {
                army.IsTargeted = false;
                army.Target = null;
            });
            foreach (var attackArmy in _armies.Where(x => x.UnitCount > 0).OrderByDescending(x => x.EffectivePower()).ThenByDescending(x => x.Initiative)) {
                Army bestTarget = null;
                int bestDamage = 0;
                foreach (var defendArmy in _armies.Where(x => x.IsImmuneSystem != attackArmy.IsImmuneSystem && !x.IsTargeted && x.UnitCount > 0)) {
                    var attackDamage = GetDamage(attackArmy, defendArmy);
                    bool isBest = false;
                    if (bestTarget == null) {
                        isBest = true;
                    } else if (attackDamage > bestDamage) {
                        isBest = true;
                    } else if (attackDamage == bestDamage && defendArmy.EffectivePower() > bestTarget.EffectivePower()) {
                        isBest = true;
                    } else if (attackDamage == bestDamage && defendArmy.EffectivePower() == bestTarget.EffectivePower() && defendArmy.Initiative > bestTarget.Initiative) {
                        isBest = true;
                    }
                    if (isBest && attackDamage > 0) {
                        bestTarget = defendArmy;
                        bestDamage = attackDamage;
                    }
                }
                attackArmy.Target = bestTarget;
                if (bestTarget != null) {
                    bestTarget.IsTargeted = true;
                }
            }
        }

        private int GetDamage(Army attack, Army defend) {
            if (defend.Immunities.Contains(attack.AttackType)) {
                return 0;
            }
            if (defend.Weaknesses.Contains(attack.AttackType)) {
                return attack.EffectivePower() * 2;
            } else {
                return attack.EffectivePower();
            }
        }

        private void GetArmies(List<string> input) {
            _armies = new List<Army>();
            bool isImmuneSystem = true;
            int count = 0;
            string name = "";
            foreach (var line in input) {
                if (line == "Infection:") {
                    isImmuneSystem = false;
                    count = 0;
                    name = "Infection group";
                } else if (line == "Immune System:") {
                    isImmuneSystem = true;
                    count = 0;
                    name = "Immune System group";
                } else if (line != "") {
                    count++;
                    var army = new Army() { IsImmuneSystem = isImmuneSystem };
                    var split = line.Split(' ');
                    army.UnitCount = Convert.ToInt32(split[0]);
                    army.HitPoints = Convert.ToInt32(split[4]);
                    int index = 7;
                    do {
                        if (split[index] == "with") break;
                        if (split[index] == "(immune") {
                            index = GetStatuses(split, index + 2, army.Immunities);
                        } else if (split[index] == "(weak") {
                            index = GetStatuses(split, index + 2, army.Weaknesses);
                        } else if (split[index] == "immune") {
                            index = GetStatuses(split, index + 2, army.Immunities);
                        } else if (split[index] == "weak") {
                            index = GetStatuses(split, index + 2, army.Weaknesses);
                        }
                    } while (true);
                    army.AttackDamage = Convert.ToInt32(split[index + 5]);
                    army.AttackType = split[index + 6];
                    army.Initiative = Convert.ToInt32(split.Last());
                    army.Name = name + " " + count;
                    if (isImmuneSystem) {
                        army.EffectivePower = () => army.UnitCount * (army.AttackDamage + _boost);
                    } else {
                        army.EffectivePower = () => army.UnitCount * army.AttackDamage;
                    }
                    army.StartingUnitCount = army.UnitCount;
                    _armies.Add(army);
                }
            }
        }

        private int GetStatuses(string[] split, int index, HashSet<string> status) {
            var statuses = new HashSet<string>();
            do {
                status.Add(split[index].Replace(";", "").Replace(")", "").Replace(",", ""));
                if (split[index].Last() == ';' || split[index].Last() == ')') {
                    return index + 1;
                }
                index++;
            } while (true);
        }

        private List<string> TestInput() {
            return new List<string>() {
                "Immune System:",
                "17 units each with 5390 hit points (weak to radiation, bludgeoning) with an attack that does 4507 fire damage at initiative 2",
                "989 units each with 1274 hit points (immune to fire; weak to bludgeoning, slashing) with an attack that does 25 slashing damage at initiative 3",
                "",
                "Infection:",
                "801 units each with 4706 hit points (weak to radiation) with an attack that does 116 bludgeoning damage at initiative 1",
                "4485 units each with 2961 hit points (immune to radiation; weak to fire, cold) with an attack that does 12 slashing damage at initiative 4"
            };
        }

        private class Army {
            public Army() {
                Weaknesses = new HashSet<string>();
                Immunities = new HashSet<string>();
            }
            public int StartingUnitCount { get; set; }
            public int UnitCount { get; set; }
            public int HitPoints { get; set; }
            public int AttackDamage { get; set; }
            public int Initiative { get; set; }
            public string AttackType { get; set; }
            public HashSet<string> Weaknesses { get; set; }
            public HashSet<string> Immunities { get; set; }
            public Army Target { get; set; }
            public bool IsTargeted { get; set; }
            public bool IsImmuneSystem { get; set; }
            public Func<int> EffectivePower { get; set; }
            public string Name { get; set; }
        }
    }
}
