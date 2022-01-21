using System;
using System.Collections.Generic;

namespace Euler_Logic.Problems.AdventOfCode.Y2015 {
    public class Problem21 : AdventOfCodeBase {
        private List<Item> _weapons;
        private List<Item> _armors;
        private List<Item> _rings;
        private Item _selectedWeapon;
        private Item _selectedArmor;
        private Item _selectedRing1;
        private Item _selectedRing2;
        private Entity _player;
        private Entity _boss;
        private int _bestCost = 0;

        public override string ProblemName => "Advent of Code 2015: 21";

        public override string GetAnswer() {
            return Answer1(Input()).ToString();
        }

        public override string GetAnswer2() {
            return Answer2(Input()).ToString();
        }

        private int Answer1(List<string> input) {
            _bestCost = int.MaxValue;
            _player = new Entity();
            _boss = new Entity();
            GetItems();
            LoopThroughWeapon(input, false);
            return _bestCost;
        }

        private int Answer2(List<string> input) {
            _bestCost = 0;
            _player = new Entity();
            _boss = new Entity();
            GetItems();
            LoopThroughWeapon(input, true);
            return _bestCost;
        }

        private void LoopThroughWeapon(List<string> input, bool part2) {
            foreach (var weapon in _weapons) {
                _selectedWeapon = weapon;
                LoopThroughArmor(input, part2);
            }
        }

        private void LoopThroughArmor(List<string> input, bool part2) {
            _selectedArmor = null;
            LoopThroughRings(input, part2);
            foreach (var armor in _armors) {
                _selectedArmor = armor;
                LoopThroughRings(input, part2);
            }
        }

        private void LoopThroughRings(List<string> input, bool part2) {
            _selectedRing1 = null;
            _selectedRing2 = null;
            Simulate(input, part2);
            foreach (var ring1 in _rings) {
                _selectedRing1 = ring1;
                _selectedRing2 = null;
                Simulate(input, part2);
                foreach (var ring2 in _rings) {
                    if (ring1 != ring2) {
                        _selectedRing2 = ring2;
                        Simulate(input, part2);
                    }
                }
            }
        }

        private void Simulate(List<string> input, bool part2) {
            SetPlayer();
            SetBoss(input);
            int playerDamageDealt = Math.Max(1, _player.Damage - _boss.Armor);
            int bossDamageDealt = Math.Max(1, _boss.Damage - _player.Armor);
            int playerAttacks = _boss.Hitpoints / playerDamageDealt + (_boss.Hitpoints % playerDamageDealt != 0 ? 1 : 0);
            int bossAttacks = _player.Hitpoints / bossDamageDealt + (_player.Hitpoints % bossDamageDealt != 0 ? 1 : 0);
            if (!part2) {
                if (playerAttacks <= bossAttacks && _player.Cost < _bestCost) {
                    _bestCost = _player.Cost;
                }
            } else {
                if (playerAttacks > bossAttacks && _player.Cost > _bestCost) {
                    _bestCost = _player.Cost;
                }
            }
        }

        private void SetPlayer() {
            int damage = 0;
            int armor = 0;
            int cost = 0;
            damage += _selectedWeapon.Damage;
            cost += _selectedWeapon.Cost;
            if (_selectedArmor != null) {
                armor += _selectedArmor.Armor;
                cost += _selectedArmor.Cost;
            }
            if (_selectedRing1 != null) {
                damage += _selectedRing1.Damage;
                armor += _selectedRing1.Armor;
                cost += _selectedRing1.Cost;
            }
            if (_selectedRing2 != null) {
                damage += _selectedRing2.Damage;
                armor += _selectedRing2.Armor;
                cost += _selectedRing2.Cost;
            }
            _player.Damage = damage;
            _player.Armor = armor;
            _player.Cost = cost;
            _player.Hitpoints = 100;
        }

        private void SetBoss(List<string> input) {
            var hitPoints = input[0].Split(' ');
            var damage = input[1].Split(' ');
            var armor = input[2].Split(' ');
            _boss.Hitpoints = Convert.ToInt32(hitPoints[2]);
            _boss.Damage = Convert.ToInt32(damage[1]);
            _boss.Armor = Convert.ToInt32(armor[1]);
        }

        private void GetItems() {
            _weapons = new List<Item>() {
                new Item() { Armor = 0, Cost = 8, Damage = 4, Name = "Dagger" },
                new Item() { Armor = 0, Cost = 10, Damage = 5, Name = "Shortsword" },
                new Item() { Armor = 0, Cost = 25, Damage = 6, Name = "Warhammer" },
                new Item() { Armor = 0, Cost = 40, Damage = 7, Name = "Longsword" },
                new Item() { Armor = 0, Cost = 74, Damage = 8, Name = "Greataxe" }
            };
            _armors = new List<Item>() {
                new Item() { Armor = 1, Cost = 13, Damage = 0, Name = "Leather" },
                new Item() { Armor = 2, Cost = 31, Damage = 0, Name = "Chainmail" },
                new Item() { Armor = 3, Cost = 53, Damage = 0, Name = "Splintmail" },
                new Item() { Armor = 4, Cost = 75, Damage = 0, Name = "Bandedmail" },
                new Item() { Armor = 5, Cost = 102, Damage = 0, Name = "Platemail" },
            };
            _rings = new List<Item>() {
                new Item() { Armor = 0, Cost = 25, Damage = 1, Name = "Damage +1" },
                new Item() { Armor = 0, Cost = 50, Damage = 2, Name = "Damage +2" },
                new Item() { Armor = 0, Cost = 100, Damage = 3, Name = "Damage +3" },
                new Item() { Armor = 1, Cost = 20, Damage = 0, Name = "Defense +1" },
                new Item() { Armor = 2, Cost = 40, Damage = 0, Name = "Defense +2" },
                new Item() { Armor = 3, Cost = 80, Damage = 0, Name = "Defense +3" },
            };
        }

        private class Item {
            public string Name { get; set; }
            public int Cost { get; set; }
            public int Damage { get; set; }
            public int Armor { get; set; }
        }

        private class Entity {
            public int Damage { get; set; }
            public int Armor { get; set; }
            public int Hitpoints { get; set; }
            public int Cost { get; set; }
        }
    }
}
