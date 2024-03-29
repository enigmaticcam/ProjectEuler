﻿using Euler_Logic.Helpers;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace Euler_Logic.Problems {

    public abstract class EventSequence {
        public int Count() {
            return Count(1);
        }
        protected abstract int Count(int depth);
    }

    public class EventSequence_Sequences : EventSequence {
        private LinkedList<EventSequence> Sequences { get; set; }

        protected override int Count(int depth) {
            if (depth >= 1000) throw new Exception("Infinite loop detected in Count");
            int sum = 0;
            foreach (var sequence in Sequences) {
                //sum += sequence.Count(depth + 1);
            }
            return sum;
        }
    }

    public class EventSequence_Events : EventSequence {
        private LinkedList<GameEvent> Events { get; set; }

        protected override int Count(int depth) {
            return Events.Count;
        }
    }

    public class GameEvent {

    }

    public class UseCase {
        public void Main(EventSequence sequence) {
            int count = sequence.Count();
        }
    }


    public class Problem000_Life : ProblemBase {
        public override string ProblemName => "Life";

        public override string GetAnswer() {
            var random = new Random();
            decimal nums = 0;
            decimal total = 1000000;
            for (decimal count = 1; count <= total; count++) {
                var next = GetCountBase(random); // 24.72
                //var next = GetCountStops(random); // 27.71
                nums += next;
            }
            return (nums / total).ToString();
        }

        private decimal GetCountBase(Random random) {
            return GetCount(random, 132);
        }

        private decimal GetCountStops(Random random) {
            return GetCount(random, 11, 11, 11, 39, 60);
        }

        private decimal GetCount(Random random, params int[] maxes) {
            decimal count = 0;
            foreach (var max in maxes) {
                count += GetCount(random, max);
            }
            return count;
        }

        private decimal GetCount(Random random, int max) {
            int num = 0;
            decimal count = 0;
            do {
                int next = random.Next(1, 11);
                num += next;
                count++;
            } while (num <= max);
            return count;
        }
    }

    public class Problem000_4sum : ProblemBase {
        public override string ProblemName => "";
        public override string GetAnswer() {
            Solve(new int[] { 1000000000, 1000000000, 1000000000, 1000000000 }, -294967296);
            return "";
        }

        private IList<IList<int>> Solve(int[] nums, int target) {
            nums = nums.OrderBy(x => x).ToArray();
            var hash = new HashSet<string>();
            var list = new List<IList<int>>();
            int lastA = int.MinValue;
            int lastB = int.MinValue;
            int lastC = int.MinValue;
            int lastD = int.MinValue;
            for (int a = 0; a < nums.Length; a++) {
                if (nums[a] != lastA) {
                    lastA = nums[a];
                    for (int b = a + 1; b < nums.Length; b++) {
                        if (nums[b] != lastB) {
                            lastB = nums[b];
                            for (int c = b + 1; c < nums.Length; c++) {
                                if (nums[c] != lastC) {
                                    lastC = nums[c];
                                    for (int d = c + 1; d < nums.Length; d++) {
                                        if (nums[d] != lastD) {
                                            lastD = nums[d];
                                            if (nums[a] + nums[b] + nums[c] + nums[d] == target) {
                                                var key = $"{nums[a]},{nums[b]},{nums[c]},{nums[d]}";
                                                if (!hash.Contains(key)) {
                                                    hash.Add(key);
                                                    list.Add(new List<int>() { nums[a], nums[b], nums[c], nums[d] });
                                                }
                                            }
                                        }
                                    }
                                    lastD = int.MinValue;
                                }
                            }
                            lastC = int.MinValue;
                        }
                    }
                    lastB = int.MinValue;
                }
                lastA = int.MinValue;
            }
            return list;
        }

        private void Recursive(int[] nums, int targetRemaining, int startIndex, int currentIndex) {
            int last = int.MinValue;
            for (int index = startIndex; index < nums.Length; index++) {
                var value = nums[index];
                //if (value != last && value ) {
                //    last = value;

                //}
            }
        }
    }

    public class Problem000_LargestPrimeFactor : ProblemBase {
        public override string ProblemName => "Prime Factor";
        public override string GetAnswer() {
            var max = (ulong)Math.Sqrt(600851475143);
            for (ulong num = max; num >= 2; num--) {
                if (600851475143 % num == 0) return num.ToString();
            }
            return "";
        }
    }

    public class Problem000_Herm : ProblemBase {
        public override string ProblemName => "herm";
        private List<int> _numbers;

        public override string GetAnswer() {
            Initialize(100000);
            return LookForDupes1().ToString();
        }

        private int LookForDupes1() {
            int count = 0;
            var sorted = _numbers.OrderBy(x => x).ToList();
            var hash = new HashSet<int>(_numbers);
            var highest = sorted.Last();
            for (int index1 = 0; index1 < sorted.Count; index1++) {
                for (int index2 = index1 + 1; index2 < sorted.Count; index2++) {
                    var sum = sorted[index1] + sorted[index2];
                    if (sum > highest) break;
                    if (hash.Contains(sum)) {
                        count++;
                    }
                }
            }
            return count;
        }

        private int LookForDupes2() {
            int count = 0;
            var sorted = new SortedList<int, int>();
            _numbers.ForEach(x => {
                if (!sorted.ContainsKey(x)) sorted.Add(x, x);
            });
            var highest = sorted.Last().Key;
            for (int index1 = 0; index1 < sorted.Count; index1++) {
                var num1 = sorted.Keys[index1];
                for (int index2 = index1 + 1; index2 < sorted.Count; index2++) {
                    var num2 = sorted.Keys[index2];
                    var sum = num1 + num2;
                    if (sum > highest) break;
                    if (sorted.ContainsKey(sum)) {
                        count++;
                    }
                }
            }
            return count;
        }

        private void Initialize(int size) {
            _numbers = new List<int>(size);
            var random = new Random();
            for (int count = 1; count <= size; count++) {
                _numbers.Add((int)random.Next(0, 10000000));
            }
        }
    }

    public class Problem000_Wordle : ProblemBase {
        private List<Word> _words;
        private List<DigitAttempt[]> _attempts;
        private ulong _noneAll;
        private ulong _knownAll;
        private ulong[] _noneDigit;
        private char[] _known;
        private bool[] _isKnown;

        private enum enumAttempt {
            None,
            WrongPlace,
            RightPlace
        }

        public override string ProblemName => "Sandbox";

        public override string GetAnswer() {
            GetWords();
            SetAttempts();
            SetKnowns();
            return FindOne();
        }

        private string FindOne() {
            var good = new List<string>();
            foreach (var word in _words) {
                if (IsGood(word)) good.Add(word.Text);
            }
            if (good.Count > 1) {
                var random = new Random();
                int index = random.Next(good.Count);
                var bestOrder = HighestVowelCount(good);
                return bestOrder.ElementAt(index) + " (" + good.Count + ")";
            } else if (good.Count == 1) {
                return good[0];
            } else {
                return "None found";
            }
        }

        private IEnumerable<string> HighestVowelCount(List<string> words) {
            var counts = new List<Tuple<string, int>>();
            foreach (var word in words) {
                int count = 0;
                foreach (var digit in word) {
                    if (digit == 'a' || digit == 'e' || digit == 'i' || digit == 'o' || digit == 'u') count++;
                }
                counts.Add(new Tuple<string, int>(word, count));
            }
            return counts.OrderByDescending(x => x.Item2).Select(x => x.Item1);
        }

        private bool IsGood(Word word) {
            if ((word.Bits & _noneAll) != 0) return false; // Has some letters we know are not used
            if ((word.Bits & _knownAll) != _knownAll) return false; // Does not have all letters we know it should
            for (int index = 0; index < 5; index++) {
                var digit = word.Text[index];
                var bit = GetBit(digit);
                if (_isKnown[index] && digit != _known[index]) return false; // Does not have letter we know is in correct place
                if ((_noneDigit[index] & bit) == bit) return false; // Has letter in position we know is in word but not that position
            }
            return true;
        }

        private void SetKnowns() {
            _noneDigit = new ulong[5];
            _known = new char[5];
            _isKnown = new bool[5];
            foreach (var attempt in _attempts) {
                for (int index = 0; index < 5; index++) {
                    var result = attempt[index];
                    var bit = GetBit(result.Digit);
                    switch (result.Attempt) {
                        case enumAttempt.None:
                            if ((_noneAll & bit) == 0) _noneAll += bit;
                            break;
                        case enumAttempt.WrongPlace:
                            if ((_noneDigit[index] & bit) == 0) _noneDigit[index] += bit;
                            if ((_knownAll & bit) == 0) _knownAll += bit;
                            break;
                        case enumAttempt.RightPlace:
                            _known[index] = result.Digit;
                            _isKnown[index] = true;
                            if ((_knownAll & bit) == 0) _knownAll += bit;
                            break;
                    }
                }
            }
        }

        private void SetAttempts() {
            _attempts = new List<DigitAttempt[]>();
            _attempts.Add(new DigitAttempt[5] {
                new DigitAttempt() { Attempt = enumAttempt.WrongPlace, Digit = 'a' },
                new DigitAttempt() { Attempt = enumAttempt.None, Digit = 'd' },
                new DigitAttempt() { Attempt = enumAttempt.None, Digit = 'i' },
                new DigitAttempt() { Attempt = enumAttempt.None, Digit = 'e' },
                new DigitAttempt() { Attempt = enumAttempt.WrongPlace, Digit = 'u' }
            });
            _attempts.Add(new DigitAttempt[5] {
                new DigitAttempt() { Attempt = enumAttempt.None, Digit = 'q' },
                new DigitAttempt() { Attempt = enumAttempt.RightPlace, Digit = 'u' },
                new DigitAttempt() { Attempt = enumAttempt.WrongPlace, Digit = 'a' },
                new DigitAttempt() { Attempt = enumAttempt.None, Digit = 'c' },
                new DigitAttempt() { Attempt = enumAttempt.None, Digit = 'k' }
            });
            _attempts.Add(new DigitAttempt[5] {
                new DigitAttempt() { Attempt = enumAttempt.None, Digit = 'm' },
                new DigitAttempt() { Attempt = enumAttempt.RightPlace, Digit = 'u' },
                new DigitAttempt() { Attempt = enumAttempt.WrongPlace, Digit = 'r' },
                new DigitAttempt() { Attempt = enumAttempt.RightPlace, Digit = 'a' },
                new DigitAttempt() { Attempt = enumAttempt.None, Digit = 'l' }
            });
            //_attempts.Add(new DigitAttempt[5] {
            //    new DigitAttempt() { Attempt = enumAttempt.None, Digit = 'l' },
            //    new DigitAttempt() { Attempt = enumAttempt.RightPlace, Digit = 'i' },
            //    new DigitAttempt() { Attempt = enumAttempt.WrongPlace, Digit = 'e' },
            //    new DigitAttempt() { Attempt = enumAttempt.None, Digit = 'g' },
            //    new DigitAttempt() { Attempt = enumAttempt.RightPlace, Digit = 'e' }
            //});
        }

        private void GetWords() {
            _words = new List<Word>();
            using (var reader = new StreamReader(@"C:\Temp\Words.txt")) {
                while (!reader.EndOfStream) {
                    var line = reader.ReadLine();
                    if (line.Length == 5) _words.Add(GetWord(line));
                }
            }
        }

        private Word GetWord(string text) {
            var word = new Word() { Text = text };
            word.Bits = GetBits(text);
            return word;
        }

        private ulong GetBits(string text) {
            ulong bits = 0;
            foreach (var digit in text) {
                var bit = GetBit(digit);
                if ((bits & bit) == 0) bits += bit;
            }
            return bits;
        }

        private ulong GetBit(char digit) {
            int asc = digit;
            return (ulong)Math.Pow(2, asc - 97);
        }

        private class Word {
            public string Text { get; set; }
            public ulong Bits { get; set; }
        }

        private class DigitAttempt {
            public char Digit { get; set; }
            public enumAttempt Attempt { get; set; }
        }
    }

    public class Problem000_CFSimulation : ProblemBase {
        public override string ProblemName => "Sandbox";

        public override string GetAnswer() {
            _100Count = 0;
            long maxCount = 10000000;
            long cash = 0;
            var random = new Random();
            for (long count = 1; count <= maxCount; count++) {
                cash += Simulate(random, 300);
            }
            //return ((decimal)cash / (decimal)maxCount).ToString();
            return ((decimal)_deathTurns / (decimal)maxCount).ToString();
        }

        private long _100Count;
        private long _deathTurns;
        private long Simulate(Random random, long bet) {
            long cash = -50;
            int win = 54;
            long turns = 1;
            do {
                cash -= bet;
                var next = random.Next(100);
                if (next < win) {
                    cash += bet * 2;
                    win = (Math.Min(win + 1, 80));
                } else {
                    _deathTurns += turns;
                    break;
                }
                turns++;
            } while (true);
            return cash;
        }
    }

    public class Problem000_Dictionary : ProblemBase {
        public override string ProblemName => "Sandbox";
        public override string GetAnswer() {
            ulong count = 0;
            using (var reader = new StreamReader(@"C:\Temp\Words.txt")) {
                while (!reader.EndOfStream) {
                    var line = reader.ReadLine();
                    if (line.ToUpper().IndexOf("EE") > -1) count++;
                }
            }
            return count.ToString();
        }
    }

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
                if ((item.Bit & bits) == 0 && (item.Requirements & bits) == item.Requirements) {
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
            _items.Add(new Item() { Cash = 900, Cost = 8000, Hours = 16, Name = "Longship" });
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
            _items[5].Requirements += _items[1].Bit;
            _items[6].Requirements += _items[2].Bit;
            _items[7].Requirements += _items[3].Bit;
            _items[8].Requirements += _items[4].Bit;
            _items[9].Requirements += _items[5].Bit;
            _items[10].Requirements += _items[7].Bit;
            _items[11].Requirements += _items[6].Bit;
            _items[12].Requirements += _items[8].Bit;
        }

        private class Item {
            public string Name { get; set; }
            public int Cost { get; set; }
            public int Cash { get; set; }
            public int Hours { get; set; }
            public ulong Bit { get; set; }
            public int PurchaseOffset { get; set; }
            public ulong Requirements { get; set; }
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
