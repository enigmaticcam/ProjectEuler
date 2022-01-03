using Euler_Logic.Helpers;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace Euler_Logic.Problems {
    public class Problem000_DiscordBot2 : ProblemBase {
        private List<Item> _items;
        private int _bestValue = int.MaxValue;
        private List<Item> _bestOrder;
        private int _hours;
        private int _money;
        private int _lcm;
        private int[] _income;

        public override string ProblemName => "Sandbox";

        public override string GetAnswer() {
            Begin();
            return Output();
        }

        private string Output() {
            return string.Join(",", _bestOrder.Select(x => x.Name));
        }

        private void Begin() {
            _lcm = 1;
            _income = new int[240];
            AddItems();
            var order = new Item[_items.Count];
            order[0] = _items[0];
            Recursive(_items[0].Bit, 1, order);
        }
        
        private void Recursive(ulong bits, int index, Item[] order) {
            foreach (var item in _items) {
                if ((item.Bit & bits) == 0) {
                    order[index] = item;
                    var tempLCM = _lcm;
                    var tempMoney = _money;
                    var tempHours = _hours;
                    GetItem(item, index, order);
                    if (_hours < _bestValue) {
                        if (index == order.Length - 1) {
                            _bestValue = _hours;
                            _bestOrder = new List<Item>(order);
                        } else {
                            Recursive(bits + item.Bit, index + 1, order);
                        }
                    }
                    _lcm = tempLCM;
                    _money = tempMoney;
                    _hours = tempHours;
                }
            }
        }

        private void GetItem(Item item, int index, Item[] order) {
            ClearIncome();
            var sumOfAllIncome = FillIncome(index - 1, order);
            int incomeIndex = _hours % _lcm;
            int totalRounds = item.Cost / sumOfAllIncome;
            _money += sumOfAllIncome * totalRounds;
            _hours += _lcm * totalRounds;
            while (_money < item.Cost) {
                incomeIndex = (incomeIndex + 1) % _lcm;
                _hours++;
                _money += _income[incomeIndex];
            }
            _money -= item.Cost;
            _money += item.Cash;
            item.PurchaseOffset = _hours % item.Hours;
            _lcm = LCM.GetLCM(_lcm, item.Hours);
        }

        private int FillIncome(int itemMaxIndex, Item[] order) {
            int sum = 0;
            for (int index = 0; index <= itemMaxIndex; index++) {
                var item = order[index];
                int sub = item.Hours - 1 - item.PurchaseOffset;
                while (sub < _lcm) {
                    _income[sub] += item.Cash;
                    sum += item.Cash;
                    sub += item.Hours;
                }
            }
            return sum;
        }

        private void ClearIncome() {
            for (int index = 0; index < _lcm; index++) {
                _income[index] = 0;
            }
        }

        private void AddItems() {
            _items = new List<Item>();
            _items.Add(new Item() { Cash = 100, Cost = 0, Hours = 1, Name = "Work" });
            _items.Add(new Item() { Cash = 85, Cost = 5000, Hours = 1, Name = "Small Farm" });
            _items.Add(new Item() { Cash = 100, Cost = 6000, Hours = 2, Name = "Guard Tower" });
            _items.Add(new Item() { Cash = 550, Cost = 7500, Hours = 8, Name = "Blacksmith" });
            _items.Add(new Item() { Cash = 900, Cost = 8000, Hours = 40, Name = "Longship" });
            _items.Add(new Item() { Cash = 300, Cost = 14000, Hours = 2, Name = "Smokehouse" });
            _items.Add(new Item() { Cash = 500, Cost = 16500, Hours = 4, Name = "Barracks" });
            _items.Add(new Item() { Cash = 2500, Cost = 18500, Hours = 20, Name = "Alehouse" });
            _items.Add(new Item() { Cash = 3000, Cost = 23000, Hours = 24, Name = "Mercenary Work" });
            _items.Add(new Item() { Cash = 850, Cost = 38000, Hours = 3, Name = "Homestead" });
            _items.Add(new Item() { Cash = 8500, Cost = 46000, Hours = 40, Name = "Trading Port" });
            _items.Add(new Item() { Cash = 1400, Cost = 53000, Hours = 6, Name = "Stronghold" });
            _items.Add(new Item() { Cash = 11000, Cost = 61000, Hours = 48, Name = "Raiding Party" });
            ulong bit = 1;
            _items.ForEach(x => {
                x.Bit = bit;
                bit *= 2;
            });
        }

        private class Item {
            public string Name { get; set; }
            public int Cost { get; set; }
            public int Cash { get; set; }
            public int Hours { get; set; }
            public ulong Bit { get; set; }
            public int PurchaseOffset { get; set; }
        }
    }

    public class Problem000_DiscordBot : ProblemBase {
        private List<Item> _items;

        public override string ProblemName => "Sandbox";

        public override string GetAnswer() {

            AddItems();
            var order = new Item[_items.Count];
            order[0] = _items[0];
            Recursive(_items[0].Bit, 1, 400, 0, order);
            return _bestValue.ToString();
        }

        private decimal _bestValue = decimal.MaxValue;
        private List<Item> _bestOrder;
        private void Recursive(ulong bits, int index, decimal ratio, decimal hours, Item[] order) {
            foreach (var item in _items) {
                if ((item.Bit & bits) == 0) {
                    order[index] = item;
                    ShiftHours(bits + item.Bit, index + 1, ratio, hours, order, item);
                }
            }
        }

        private void ShiftHours(ulong bits, int index, decimal ratio, decimal hours, Item[] order, Item item) {
            var hourShift = Math.Round((item.Cost / ratio), 0) + hours;
            if ((item.Cost % ratio) != 0) {
                hourShift++;
            }
            if (hourShift < _bestValue) {
                if (index == order.Length) {
                    _bestValue = hourShift;
                    _bestOrder = new List<Item>(order);
                } else {
                    Recursive(bits, index, ratio + (item.Cash / item.Hours), hourShift, order);
                }
            }
        }

        private void AddItems() {
            _items = new List<Item>();
            _items.Add(new Item() { Cash = 400, Cost = 0, Hours = 1, Name = "Work" });
            _items.Add(new Item() { Cash = 85, Cost = 5000, Hours = 1, Name = "Small Farm" });
            _items.Add(new Item() { Cash = 100, Cost = 6000, Hours = 2, Name = "Guard Tower" });
            _items.Add(new Item() { Cash = 550, Cost = 7500, Hours = 8, Name = "Blacksmith" });
            _items.Add(new Item() { Cash = 900, Cost = 8000, Hours = 40, Name = "Longship" });
            _items.Add(new Item() { Cash = 300, Cost = 14000, Hours = 2, Name = "Smokehouse" });
            _items.Add(new Item() { Cash = 500, Cost = 16500, Hours = 4, Name = "Barracks" });
            _items.Add(new Item() { Cash = 2500, Cost = 18500, Hours = 20, Name = "Alehouse" });
            _items.Add(new Item() { Cash = 3000, Cost = 23000, Hours = 24, Name = "Mercenary Work" });
            _items.Add(new Item() { Cash = 850, Cost = 38000, Hours = 3, Name = "Homestead" });
            _items.Add(new Item() { Cash = 8500, Cost = 46000, Hours = 40, Name = "Trading Port" });
            _items.Add(new Item() { Cash = 1400, Cost = 53000, Hours = 6, Name = "Stronghold" });
            _items.Add(new Item() { Cash = 11000, Cost = 61000, Hours = 48, Name = "Raiding Party" });
            ulong bit = 1;
            _items.ForEach(x => {
                x.Bit = bit;
                bit *= 2;
            });
        }

        private class Item {
            public string Name { get; set; }
            public decimal Cost { get; set; }
            public decimal Cash { get; set; }
            public decimal Hours { get; set; }
            public ulong Bit { get; set; }
        }
    }

    public class Problem000_ArchiveKHFunds : ProblemBase {
        public override string ProblemName {
            get { return "Sandbox5"; }
        }

        public override string GetAnswer() {
            MoveFiles("2017 Archive");
            MoveFiles("2018 Archive");
            MoveFiles("2019 Archive");
            MoveFiles("2020 Archive");
            return "";
        }

        private void MoveFiles(string year) {
            var path = Path.Combine(@"C:\Users\Ctangen\Dropbox\Personal\KHFunds\", year);
            var directory = new DirectoryInfo(path);
            foreach (var subDirectory in directory.GetDirectories()) {
                bool fileFound = false;
                foreach (var file in subDirectory.GetFiles()) {
                    if (file.Name == "Accounts Sheet.pdf") {
                        File.Copy(file.FullName, @"C:\Temp\Accounts\" + subDirectory.Name + ".pdf");
                        fileFound = true;
                        break;
                    }
                }
                if (!fileFound) {
                    throw new System.Exception("No file found for " + subDirectory.Name);
                }
            }
        }
    }
}
