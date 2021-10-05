using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems.AdventOfCode.Y2015 {
    public class Problem22 : AdventOfCodeBase {
        private Entity _playerStart;
        private Entity _playerCurrent;
        private Entity _bossCurrent;
        private Entity _bossStart;
        private int _bestMana;
        private bool _hardMode;

        private enum enumResult {
            Undecided,
            Decided,
            NewBestFound
        }

        public override string ProblemName => "Advent of Code 2015: 22";

        public override string GetAnswer() {
            return Answer2().ToString();
        }

        private int Answer1() {
            _bestMana = int.MaxValue;
            SetStart();
            GetShortestMana();
            return _bestMana;
        }

        private int Answer2() {
            _bestMana = int.MaxValue;
            _hardMode = true;
            SetStart();
            GetShortestMana();
            return _bestMana;
        }

        private void GetShortestMana() {
            _playerCurrent = new Entity();
            _bossCurrent = new Entity();
            enumResult result = enumResult.Undecided;
            do {
                ResetPlayerAndBoss();
                result = TryAllPlayer();
            } while (result == enumResult.NewBestFound);
        }

        private enumResult TryAllPlayer() {
            var playerTemp = new Entity();
            var bossTemp = new Entity();
            PerformEffects();
            if (_hardMode) {
                _playerCurrent.Hitpoints--;
            }
            SaveToTemp(playerTemp, bossTemp);
            var result = CheckWinner();
            if (result != enumResult.Undecided) {
                return result;
            }
            if (CastRecharge()) {
                PerformEffects();
                PerformBoss();
                result = CheckWinner();
                if (result == enumResult.NewBestFound) {
                    return enumResult.NewBestFound;
                } else if (result == enumResult.Undecided) {
                    result = TryAllPlayer();
                    if (result == enumResult.NewBestFound) {
                        return enumResult.NewBestFound;
                    }
                }
                LoadFromTemp(playerTemp, bossTemp);
            }
            if (CastDrain()) {
                PerformEffects();
                PerformBoss();
                result = CheckWinner();
                if (result == enumResult.NewBestFound) {
                    return enumResult.NewBestFound;
                } else if (result == enumResult.Undecided) {
                    result = TryAllPlayer();
                    if (result == enumResult.NewBestFound) {
                        return enumResult.NewBestFound;
                    }
                }
                LoadFromTemp(playerTemp, bossTemp);
            }
            if (CastMagicMissile()) {
                PerformEffects();
                PerformBoss();
                result = CheckWinner();
                if (result == enumResult.NewBestFound) {
                    return enumResult.NewBestFound;
                } else if (result == enumResult.Undecided) {
                    result = TryAllPlayer();
                    if (result == enumResult.NewBestFound) {
                        return enumResult.NewBestFound;
                    }
                }
                LoadFromTemp(playerTemp, bossTemp);
            }
            if (CastPoison()) {
                PerformEffects();
                PerformBoss();
                result = CheckWinner();
                if (result == enumResult.NewBestFound) {
                    return enumResult.NewBestFound;
                } else if (result == enumResult.Undecided) {
                    result = TryAllPlayer();
                    if (result == enumResult.NewBestFound) {
                        return enumResult.NewBestFound;
                    }
                }
                LoadFromTemp(playerTemp, bossTemp);
            }
            if (CastShield()) {
                PerformEffects();
                PerformBoss();
                result = CheckWinner();
                if (result == enumResult.NewBestFound) {
                    return enumResult.NewBestFound;
                } else if (result == enumResult.Undecided) {
                    result = TryAllPlayer();
                    if (result == enumResult.NewBestFound) {
                        return enumResult.NewBestFound;
                    }
                }
                LoadFromTemp(playerTemp, bossTemp);
            }
            return enumResult.Decided;
        }

        private void PerformBoss() {
            var damage = Math.Max(1, _bossCurrent.Damage - _playerCurrent.Armor);
            _playerCurrent.Hitpoints -= damage;
        }

        private void SaveToTemp(Entity playerTemp, Entity bossTemp) {
            playerTemp.Armor = _playerCurrent.Armor;
            playerTemp.Hitpoints = _playerCurrent.Hitpoints;
            playerTemp.Mana = _playerCurrent.Mana;
            playerTemp.RechargeRemaining = _playerCurrent.RechargeRemaining;
            playerTemp.ShieldRemaining = _playerCurrent.ShieldRemaining;
            playerTemp.SpentMana = _playerCurrent.SpentMana;
            bossTemp.Hitpoints = _bossCurrent.Hitpoints;
            bossTemp.PoisonRemaining = _bossCurrent.PoisonRemaining;
            bossTemp.Damage = _bossCurrent.Damage;
        }

        private void LoadFromTemp(Entity playerTemp, Entity bossTemp) {
            _playerCurrent.Armor = playerTemp.Armor;
            _playerCurrent.Hitpoints = playerTemp.Hitpoints;
            _playerCurrent.Mana = playerTemp.Mana;
            _playerCurrent.RechargeRemaining = playerTemp.RechargeRemaining;
            _playerCurrent.ShieldRemaining = playerTemp.ShieldRemaining;
            _playerCurrent.SpentMana = playerTemp.SpentMana;
            _bossCurrent.Hitpoints = bossTemp.Hitpoints;
            _bossCurrent.PoisonRemaining = bossTemp.PoisonRemaining;
            _bossCurrent.Damage = bossTemp.Damage;
        }

        private enumResult CheckWinner() {
            if (_bossCurrent.Hitpoints <= 0) {
                if (_playerCurrent.SpentMana < _bestMana) {
                    _bestMana = _playerCurrent.SpentMana;
                    return enumResult.NewBestFound;
                } else {
                    return enumResult.Decided;
                }
            }
            if (_playerCurrent.Hitpoints <= 0) {
                return enumResult.Decided;
            }
            return enumResult.Undecided;
        }

        private void PerformEffects() {
            if (_playerCurrent.RechargeRemaining > 0) {
                _playerCurrent.Mana += 101;
                _playerCurrent.RechargeRemaining--;
            }
            if (_playerCurrent.ShieldRemaining > 0) {
                _playerCurrent.ShieldRemaining--;
            } else {
                _playerCurrent.Armor = 0;
            }
            if (_bossCurrent.PoisonRemaining > 0) {
                _bossCurrent.Hitpoints -= 3;
                _bossCurrent.PoisonRemaining--;
            }
        }

        private bool CastMagicMissile() {
            if (_playerCurrent.Mana < 53 || _playerCurrent.SpentMana + 53 > _bestMana) {
                return false;
            }
            _playerCurrent.Mana -= 53;
            _playerCurrent.SpentMana += 53;
            _bossCurrent.Hitpoints -= 4;
            return true;
        }

        private bool CastDrain() {
            if (_playerCurrent.Mana < 73 || _playerCurrent.SpentMana + 73 > _bestMana) {
                return false;
            }
            _playerCurrent.Mana -= 73;
            _playerCurrent.SpentMana += 73;
            _playerCurrent.Hitpoints = Math.Min(_playerStart.MaxHitpoints, _playerCurrent.Hitpoints + 2);
            _bossCurrent.Hitpoints -= 2;
            return true;
        }

        private bool CastShield() {
            if (_playerCurrent.Mana < 113 || _playerCurrent.ShieldRemaining > 0 || _playerCurrent.SpentMana + 113 > _bestMana) {
                return false;
            }
            _playerCurrent.Armor = 7;
            _playerCurrent.Mana -= 113;
            _playerCurrent.SpentMana += 113;
            _playerCurrent.ShieldRemaining = 6;
            return true;
        }

        private bool CastPoison() {
            if (_playerCurrent.Mana < 173 || _bossCurrent.PoisonRemaining > 0 || _playerCurrent.SpentMana + 173 > _bestMana) {
                return false;
            }
            _playerCurrent.Mana -= 173;
            _playerCurrent.SpentMana += 173;
            _bossCurrent.PoisonRemaining = 6;
            return true;
        }

        private bool CastRecharge() {
            if (_playerCurrent.Mana < 229 || _playerCurrent.RechargeRemaining > 0 || _playerCurrent.SpentMana + 229 > _bestMana) {
                return false;
            }
            _playerCurrent.Mana -= 229;
            _playerCurrent.SpentMana += 229;
            _playerCurrent.RechargeRemaining = 5;
            return true;
        }

        private void ResetPlayerAndBoss() {
            _playerCurrent.RechargeRemaining = _playerStart.RechargeRemaining;
            _playerCurrent.ShieldRemaining = _playerStart.ShieldRemaining;
            _playerCurrent.Hitpoints = _playerStart.MaxHitpoints;
            _playerCurrent.Mana = _playerStart.Mana;
            _playerCurrent.Armor = _playerStart.Armor;
            _playerCurrent.SpentMana = _playerStart.SpentMana;
            _bossCurrent.PoisonRemaining = _bossStart.PoisonRemaining;
            _bossCurrent.Hitpoints = _bossStart.Hitpoints;
            _bossCurrent.Damage = _bossStart.Damage;
        }

        private void SetStart() {
            _playerStart = new Entity() {
                Mana = 500,
                Hitpoints = 50,
                MaxHitpoints = 50
            };
            _bossStart = new Entity() {
                Armor = 0,
                Damage = 8,
                Hitpoints = 55
            };
        }

        private void SetStartTest1() {
            _playerStart = new Entity() {
                Mana = 250,
                Hitpoints = 10,
                MaxHitpoints = 10
            };
            _bossStart = new Entity() {
                Armor = 0,
                Damage = 8,
                Hitpoints = 13
            };
        }

        private void SetStartTest2() {
            _playerStart = new Entity() {
                Mana = 250,
                Hitpoints = 10,
                MaxHitpoints = 10
            };
            _bossStart = new Entity() {
                Armor = 0,
                Damage = 8,
                Hitpoints = 14
            };
        }

        private class Entity {
            public int Damage { get; set; }
            public int Armor { get; set; }
            public int MaxHitpoints { get; set; }
            public int Hitpoints { get; set; }
            public int Mana { get; set; }
            public int SpentMana { get; set; }
            public int ShieldRemaining { get; set; }
            public int PoisonRemaining { get; set; }
            public int RechargeRemaining { get; set; }
        }
    }
}
