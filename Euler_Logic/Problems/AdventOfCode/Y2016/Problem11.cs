using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Euler_Logic.Problems.AdventOfCode.Y2016
{
    public class Problem11 : AdventOfCodeBase
    {
        private Dictionary<ulong, Item> _items;
        private int _shift;
        private ulong _floorMask;
        private ulong _solved;
        private int _best;
        private Dictionary<ulong, HashSet<int>> _hash = new Dictionary<ulong, HashSet<int>>();
        private StreamWriter _writer;

        public override string ProblemName => "Advent of Code 2016: 11";

        public override string GetAnswer()
        {
            return Answer1(Input_Test(1)).ToString();
            return Answer1(Input()).ToString();
        }

        public override string GetAnswer2()
        {
            return Answer2(Input_Test(1)).ToString();
            return Answer2(Input()).ToString();
        }

        private int Answer1(List<string> input)
        {
            LoadItems(input);
            PairItems();
            SetBad();
            var key = SetKey();
            _best = int.MaxValue;
            return BFS(key, 0, 0);
        }

        private int Answer2(List<string> input)
        {
            LoadItems(input);
            LoadItemsPart2();
            PairItems();
            SetBad();
            var key = SetKey();
            _best = int.MaxValue;
            return BFS(key, 0, 0);
        }
        private int BFS(ulong key, int step, int floor)
        {
            var tree = new LinkedList<State>();
            tree.AddFirst(new State()
            {
                Floor = floor,
                Key = key,
                Step = step
            });
            do
            {
                var temp = new LinkedList<State>();
                foreach (var state in tree)
                {
                    if (state.Key == _solved)
                        return state.Step;
                    if (!_hash.ContainsKey(state.Key))
                        _hash.Add(state.Key, new HashSet<int>());
                    _hash[state.Key].Add(floor);
                    var itemsInFloor = (state.Key >> (state.Floor * _shift)) & _floorMask;
                    for (ulong item1 = 1; item1 <= _floorMask; item1 *= 2)
                    {
                        if ((itemsInFloor & item1) != 0)
                        {
                            for (ulong item2 = item1 * 2; item2 <= _floorMask; item2 *= 2)
                            {
                                if ((itemsInFloor & item2) != 0)
                                {
                                    MoveElevator(state, item1 + item2, temp);
                                }
                            }
                            MoveElevator(state, item1, temp);
                        }
                    }
                }
                tree = temp;

            } while (true);
        }

        private void MoveElevator(State state, ulong items, LinkedList<State> tree)
        {
            var subKey = state.Key;
            subKey -= items << (state.Floor * _shift);
            if (IsValid(subKey, state.Step + 1, state.Floor))
            {
                if (state.Floor < 3)
                {
                    subKey = state.Key;
                    subKey -= items << (state.Floor * _shift);
                    subKey += items << ((state.Floor + 1) * _shift);
                    if (IsValid(subKey, state.Step + 1, state.Floor + 1))
                    {
                        tree.AddLast(new State()
                        {
                            Floor = state.Floor + 1,
                            Key = subKey,
                            Step = state.Step + 1
                        });
                    }
                }
                if (state.Floor > 0)
                {
                    subKey = state.Key;
                    subKey -= items << (state.Floor * _shift);
                    subKey += items << ((state.Floor - 1) * _shift);
                    if (IsValid(subKey, state.Step + 1, state.Floor - 1))
                    {
                        tree.AddLast(new State()
                        {
                            Floor = state.Floor - 1,
                            Key = subKey,
                            Step = state.Step + 1
                        });
                    }
                }
            }
        }

        private bool IsValid(ulong key, int step, int floor)
        {
            if (_hash.ContainsKey(key) && _hash[key].Contains(floor))
                return false;
            var itemsInFloor = (key >> (floor * _shift)) & _floorMask;
            for (ulong itemBit1 = 1; itemBit1 <= _floorMask; itemBit1 *= 2)
            {
                var item = _items[itemBit1];
                if ((itemsInFloor & itemBit1) != 0 && (itemsInFloor & item.Pair) == 0)
                {
                    if (item.IsChip && ((itemsInFloor & item.Bad) != 0))
                        return false;
                }

            }
            return true;
        }

        private ulong SetKey()
        {
            _shift = _items.Count;
            _floorMask = (ulong)Math.Pow(2, _shift) - 1;
            _solved = _floorMask << _shift * 3;
            ulong key = 0;
            foreach (var item in _items.Values)
            {
                key += item.Bit << (_shift * item.StartLevel);
            }
            return key;
        }

        private void SetBad()
        {
            ulong bad = 0;
            foreach (var item in _items.Values.Where(x => !x.IsChip))
            {
                bad += item.Bit;
            }
            foreach (var item in _items.Values.Where(x => x.IsChip))
            {
                item.Bad = bad - item.Pair;
            }
        }

        private void PairItems()
        {
            var items = _items.Values.ToList();
            for (int index1 = 0; index1 < items.Count; index1++)
            {
                var item1 = items[index1];
                if (item1.Pair == 0)
                {
                    for (int index2 = index1 + 1; index2 < items.Count; index2++)
                    {
                        var item2 = items[index2];
                        if (item2.Name == item1.Name)
                        {
                            item1.Pair = item2.Bit;
                            item2.Pair = item1.Bit;
                            break;
                        }
                    }
                }
            }
        }

        private void LoadItems(List<string> input)
        {
            _items = new Dictionary<ulong, Item>();
            ulong bit = 1;
            int startLevel = 0;
            foreach (var line in input)
            {
                var words = line
                    .Replace(",", "")
                    .Replace(".", "")
                    .Replace("-compatible", "")
                    .Split(' ');
                for (int index = 0; index < words.Length; index++)
                {
                    var word = words[index];
                    if (word == "generator" || word == "microchip")
                    {
                        _items.Add(bit, new Item()
                        {
                            Bit = bit,
                            IsChip = word == "microchip",
                            Name = words[index - 1],
                            StartLevel = startLevel,
                            Short = $"{words[index - 1].Substring(0, 2)}{(word == "microchip" ? "M" : "G")}"
                        });
                        bit *= 2;
                    }
                }
                startLevel++;
            }
        }

        private void LoadItemsPart2()
        {
            ulong bit = _items.Keys.Max() * 2;
            _items.Add(bit, new Item()
            {
                Bit = bit,
                IsChip = false,
                Name = "elerium",
                Pair = bit * 2,
                Short = "Gel",
                StartLevel = 0
            });
            bit *= 2;
            _items.Add(bit, new Item()
            {
                Bit = bit,
                IsChip = true,
                Name = "elerium",
                Pair = bit / 2,
                Short = "Mel",
                StartLevel = 0
            });
            bit *= 2;
            _items.Add(bit, new Item()
            {
                Bit = bit,
                IsChip = false,
                Name = "dilithium",
                Pair = bit * 2,
                Short = "Gdi",
                StartLevel = 0
            });
            bit *= 2;
            _items.Add(bit, new Item()
            {
                Bit = bit,
                IsChip = true,
                Name = "dilithium",
                Pair = bit / 2,
                Short = "Mdi",
                StartLevel = 0
            });
        }

        private string Output(ulong key, int floor)
        {
            var text = new StringBuilder();
            for (int subFloor = 3; subFloor >= 0; subFloor--)
            {
                text.Append("F");
                text.Append(subFloor);
                if (subFloor == floor)
                {
                    text.Append(" E");
                } else
                {
                    text.Append(" .");
                }
                var itemsInFloor = (key >> (subFloor * _shift)) & _floorMask;
                for (int itemIndex1 = 0; itemIndex1 < _items.Count; itemIndex1++)
                {
                    ulong bit = (ulong)Math.Pow(2, itemIndex1);
                    if ((itemsInFloor & bit) != 0)
                    {
                        text.Append($" {_items[bit].Short}");
                    } else
                    {
                        text.Append(" .  ");
                    }
                }
                text.AppendLine();
            }
            return text.ToString();
        }

        private class Item
        {
            public ulong Bit { get; set; }
            public ulong Pair { get; set; }
            public string Name { get; set; }
            public bool IsChip { get; set; }
            public int StartLevel { get; set; }
            public ulong Bad { get; set; }
            public string Short { get; set; }
        }

        private class State
        {
            public ulong Key { get; set; }
            public int Step { get; set; }
            public int Floor { get; set; }
        }
    }
}
