using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Helpers {
    public class BigNumber {
        public enum enumSign {
            Positive,
            Negative
        }

        public enumSign Sign { get; set; }

        private List<int> _digits = new List<int>();
        public IEnumerable<int> Digits {
            get { return _digits; }
            set { _digits = value.ToList(); }
        }

        public int this[int index] {
            get {
                if (_digits.Count <= index) {
                    AddZeros(index);
                }
                return _digits[index];
            }
            set {
                if (_digits.Count <= index) {
                    AddZeros(index);
                }
                _digits[index] = value;
            }
        }

        public void Clear() {
            _digits.Clear();
        }

        public int Count {
            get { return _digits.Count; }
        }

        private void AddZeros(int toIndex) {
            for (int index = this.Count - 1; index <= toIndex; index++) {
                _digits.Add(0);
            }
        }

        public void MultiplyBy(BigNumber num) {
            BigNumber thisOriginal = new BigNumber();
            thisOriginal.Digits = this.Digits;
            for (int index = 0; index < num.Count; index++) {
                if (num[index] > 0) {
                    MultiplyRecurive(index, num[index], thisOriginal);
                }
            }
            if (num.Sign != this.Sign) {
                this.Sign = enumSign.Negative;
            } else if (this.Sign == enumSign.Negative) {
                this.Sign = enumSign.Positive;
            }
        }

        private void MultiplyRecurive(int index, int count, BigNumber num) {
            for (int i = 1; i <= count; i++) {
                if (index == 0) {
                    if (i != 1) {
                        AbsoluteAddTo(num);
                    }
                } else {
                    MultiplyRecurive(index - 1, 10, num);
                }
            }
        }

        public void AddTo(BigNumber num) {
            if (num.Sign != this.Sign) {
                int compare = num.CompareAbsolute(this);
                if (compare < 0) {
                    AbsoluteSubtractFrom(num, this);
                    this.Sign = num.Sign;
                } else if (compare > 0) {
                    AbsoluteSubtractFrom(this, num);
                    this.Digits = num.Digits;
                } else {
                    this.Clear();
                    this.Sign = enumSign.Positive;
                }
            } else {
                AbsoluteAddTo(num);
            }
        }

        private void AbsoluteAddTo(BigNumber from) {
            int carryOver = 0;
            int digit = 0;
            while (digit < from.Count || carryOver > 0) {
                int sum = 0;
                if (digit < from.Count) {
                    sum = from[digit] + this[digit] + carryOver;
                } else {
                    sum = this[digit] + carryOver;
                }
                if (sum > 9) {
                    string sumText = sum.ToString();
                    this[digit] = Convert.ToInt32(sumText.Substring(1, 1));
                    carryOver = Convert.ToInt32(sumText.Substring(0, 1));
                } else {
                    this[digit] = sum;
                    carryOver = 0;
                }
                digit++;
            }
        }

        public void SubtractFrom(BigNumber num) {
            if (num.Sign != this.Sign) {
                AbsoluteAddTo(num);
                if (this.Sign == enumSign.Negative) {
                    this.Sign = enumSign.Positive;
                } else {
                    this.Sign = enumSign.Negative;
                }
            } else {
                int compare = num.CompareAbsolute(this);
                if (compare < 0) {
                    AbsoluteSubtractFrom(num, this);
                } else if (compare > 0) {
                    AbsoluteSubtractFrom(this, num);
                    this.Digits = num.Digits;
                    if (this.Sign == enumSign.Negative) {
                        this.Sign = enumSign.Positive;
                    } else {
                        this.Sign = enumSign.Negative;
                    }
                } else {
                    this.Clear();
                    this.Sign = enumSign.Positive;
                }
            }
            
        }

        private void AbsoluteSubtractFrom(BigNumber from, BigNumber to) {
            for (int index = 0; index < from.Count; index++) {
                if (from[index] < to[index]) {
                    BorrowFromNextDigit(from, index + 1);
                    from[index] += 10;
                }
                to[index] = from[index] - to[index];
            }
        }

        private void BorrowFromNextDigit(BigNumber num, int index) {
            if (num[index] == 0) {
                BorrowFromNextDigit(num, index + 1);
                num[index] = 9;
            } else {
                num[index] -= 1;
            }
        }

        public int CompareAbsolute(BigNumber num) {
            for (int index = Math.Max(this.Count, num.Count); index >= 0; index--) {
                if (num[index] > this[index]) {
                    return 1;
                } else if (num[index] < this[index]) {
                    return -1;
                }
            }
            return 0;
        }

        public BigNumber() {
            
        }

        public BigNumber(List<int> number) {
            _digits = new List<int>(number);
        }
    }
}
