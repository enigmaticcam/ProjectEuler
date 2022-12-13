using System;
using System.Collections.Generic;
using System.Linq;

namespace Euler_Logic.Problems.AdventOfCode.Y2022 {
    public class Problem13 : AdventOfCodeBase {
        public override string ProblemName => "Advent of Code 2022: 13";

        public override string GetAnswer() {
            return Answer1(Input()).ToString();
        }

        public override string GetAnswer2() {
            return Answer2(Input()).ToString();
        }

        private int Answer1(List<string> input) {
            var pairs = GetPairs(input);
            return GetSum(pairs);
        }

        private int Answer2(List<string> input) {
            input.Add("[[2]]");
            input.Add("[[6]]");
            var items = GetAll(input);
            var divider1 = items[items.Count - 1];
            var divider2 = items[items.Count - 2];
            divider1.IsDivider = true;
            divider2.IsDivider = true;
            items = items.OrderBy(x => x, new ItemComparer()).ToList();
            return (items.IndexOf(divider1) + 1) * (items.IndexOf(divider2) + 1);
        }

        private int GetSum(List<Item[]> pairs) {
            int index = 1;
            int sum = 0;
            var comparer = new ItemComparer();
            foreach (var pair in pairs) {
                var order = comparer.Compare(pair[0], pair[1]);
                if (order == -1) sum += index;
                index++;
            }
            return sum;
        }

        private List<Item[]> GetPairs(List<string> input) {
            var pairs = new List<Item[]>();
            int index = 0;
            while (index < input.Count) {
                var item1 = GetList(input[index].Substring(1, input[index].Length - 2));
                var item2 = GetList(input[index + 1].Substring(1, input[index + 1].Length - 2));
                pairs.Add(new Item[] { item1, item2 });
                index += 3;
            }
            return pairs;
        }

        private List<Item> GetAll(List<string> input) {
            var items = new List<Item>();
            foreach (var line in input) {
                if (!string.IsNullOrEmpty(line)) items.Add(GetList(line.Substring(1, line.Length - 2)));
            }
            return items;
        }

        private Item GetList(string text) {
            Item list = new ItemList() { Items = new List<Item>() };
            var highest = list;
            List<char> currentNum = null;
            foreach (var digit in text) {
                if (digit == '[') {
                    var newList = new ItemList() { Items = new List<Item>(), Parent = list };
                    ((ItemList)list).Items.Add(newList);
                    list = newList;
                } else if (digit == ']') {
                    if (currentNum != null) {
                        ((ItemList)list).Items.Add(new ItemNumber() { Parent = list, Number = Convert.ToInt32(new string(currentNum.ToArray())) });
                        currentNum = null;
                    }
                    list = list.Parent;
                } else if (digit == ',') {
                    if (currentNum != null) {
                        ((ItemList)list).Items.Add(new ItemNumber() { Parent = list, Number = Convert.ToInt32(new string(currentNum.ToArray())) });
                        currentNum = null;
                    }
                } else {
                    if (currentNum == null) currentNum = new List<char>();
                    currentNum.Add(digit);
                }
            }
            if (currentNum != null) {
                ((ItemList)list).Items.Add(new ItemNumber() { Parent = list, Number = Convert.ToInt32(new string(currentNum.ToArray())) });
            }
            return highest;
        }

        private class ItemComparer : IComparer<Item> {
            public int Compare(Item x, Item y) {
                if (x.IsList != y.IsList) {
                    if (!x.IsList) {
                        var newList = new ItemList() { Items = new List<Item>(), Parent = x.Parent };
                        var number = new ItemNumber() { Number = ((ItemNumber)x).Number, Parent = newList };
                        newList.Items.Add(number);
                        x = newList;
                    } else {
                        var newList = new ItemList() { Items = new List<Item>(), Parent = y.Parent };
                        var number = new ItemNumber() { Number = ((ItemNumber)y).Number, Parent = newList };
                        newList.Items.Add(number);
                        y = newList;
                    }
                }
                if (!x.IsList && !y.IsList) {
                    var num1 = ((ItemNumber)x).Number;
                    var num2 = ((ItemNumber)y).Number;
                    if (num1 > num2) {
                        return 1;
                    } else if (num1 < num2) {
                        return -1;
                    }
                } else {
                    var list1 = ((ItemList)x).Items;
                    var list2 = ((ItemList)y).Items;
                    int index = 0;
                    do {
                        if (index == list1.Count && index == list2.Count) {
                            return 0;
                        } else if (index == list1.Count) {
                            return -1;
                        } else if (index == list2.Count) {
                            return 1;
                        } else {
                            var order = Compare(list1[index], list2[index]);
                            if (order != 0) return order;
                        }
                        index++;
                    } while (true);
                }
                return 0;
            }
        }

        private abstract class Item {
            public abstract bool IsList { get; }
            public Item Parent { get; set; }
            public bool IsDivider { get; set; }
        }

        private class ItemList : Item {
            public List<Item> Items { get; set; }
            public override bool IsList => true;
        }

        private class ItemNumber : Item {
            public int Number { get; set; }
            public override bool IsList => false;
        }
    }
}
