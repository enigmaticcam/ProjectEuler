using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem93 : ProblemBase {
        private Token[] _numbersMaster = new Token[10];
        private Token[] _operatorsMaster = new Token[4];
        private Token _parenthesisOpen = new TokenParenthesisOpen();
        private Token _parenthesisClose = new TokenParenthesisClose();
        private Token[] _numbersCurrent = new Token[4];
        private Token[] _operatorsCurrent = new Token[3];
        private Token[] _equation = new Token[11];
        private Dictionary<int, HashSet<double>> _numbersFound = new Dictionary<int, HashSet<double>>();

        public override string ProblemName {
            get { return "93: Arithmetic expressions"; }
        }

        public override string GetAnswer() {
            Initialize();
            FindAllNumbers(0, 3, 0);
            return FindLongest();
        }

        private string FindLongest() {
            int bestBits = 0;
            double bestScore = 0;
            foreach (int bits in _numbersFound.Keys) {
                double num = 1;
                while (_numbersFound[bits].Contains(num)) {
                    num++;
                }
                if (num > bestScore) {
                    bestScore = num;
                    bestBits = bits;
                }
            }
            string text = "";
            for (int num = 0; num <= 9; num++) {
                if (((int)Math.Pow(2, num) & bestBits) == (int)Math.Pow(2, num)) {
                    text += num.ToString();
                }
            }
            return text;
        }

        private void FindAllNumbers(int start, int end, int bits) {
            for (int index = 0; index <= _numbersMaster.GetUpperBound(0); index++) {
                int bit = (int)Math.Pow(2, index);
                if ((bits & bit) == 0) {
                    bits += bit;
                    _numbersCurrent[start] = _numbersMaster[index];
                    if (start != end) {
                        FindAllNumbers(start + 1, end, bits);
                    } else {
                        FindAllOperators(0, 2, bits);
                    }
                    bits -= bit;
                }
            }
        }

        private void FindAllOperators(int start, int end, int bits) {
            for (int index = 0; index <= _operatorsMaster.GetUpperBound(0); index++) {
                _operatorsCurrent[start] = _operatorsMaster[index];
                if (start != end) {
                    FindAllOperators(start + 1, end, bits);
                } else {
                    FindAllParanthesis(bits);
                }
            }
        }

        private void FindAllParanthesis(int bits) {
            if (!_numbersFound.ContainsKey(bits)) {
                _numbersFound.Add(bits, new HashSet<double>());
            }

            // a + b + c + d
            _equation[0] = _numbersCurrent[0];
            _equation[1] = _operatorsCurrent[0];
            _equation[2] = _numbersCurrent[1];
            _equation[3] = _operatorsCurrent[1];
            _equation[4] = _numbersCurrent[2];
            _equation[5] = _operatorsCurrent[2];
            _equation[6] = _numbersCurrent[3];
            _numbersFound[bits].Add(CalculateEquation(0, 6));

            // (a + b) + c + d
            _equation[0] = _parenthesisOpen;
            _equation[1] = _numbersCurrent[0];
            _equation[2] = _operatorsCurrent[0];
            _equation[3] = _numbersCurrent[1];
            _equation[4] = _parenthesisClose;
            _equation[5] = _operatorsCurrent[1];
            _equation[6] = _numbersCurrent[2];
            _equation[7] = _operatorsCurrent[2];
            _equation[8] = _numbersCurrent[3];
            _numbersFound[bits].Add(CalculateEquation(0, 8));

            // ((a + b) + c) + d
            _equation[0] = _parenthesisOpen;
            _equation[1] = _parenthesisOpen;
            _equation[2] = _numbersCurrent[0];
            _equation[3] = _operatorsCurrent[0];
            _equation[4] = _numbersCurrent[1];
            _equation[5] = _parenthesisClose;
            _equation[6] = _operatorsCurrent[1];
            _equation[7] = _numbersCurrent[2];
            _equation[8] = _parenthesisClose;
            _equation[9] = _operatorsCurrent[2];
            _equation[10] = _numbersCurrent[3];
            _numbersFound[bits].Add(CalculateEquation(0, 10));

            // (a + b) + (c + d)
            _equation[0] = _parenthesisOpen;
            _equation[1] = _numbersCurrent[0];
            _equation[2] = _operatorsCurrent[0];
            _equation[3] = _numbersCurrent[1];
            _equation[4] = _parenthesisClose;
            _equation[5] = _operatorsCurrent[1];
            _equation[6] = _parenthesisOpen;
            _equation[7] = _numbersCurrent[2];
            _equation[8] = _operatorsCurrent[2];
            _equation[9] = _numbersCurrent[3];
            _equation[10] = _parenthesisClose;
            _numbersFound[bits].Add(CalculateEquation(0, 10));

            // a + (b + c) + d
            _equation[0] = _numbersCurrent[0];
            _equation[1] = _operatorsCurrent[0];
            _equation[2] = _parenthesisOpen;
            _equation[3] = _numbersCurrent[1];
            _equation[4] = _operatorsCurrent[1];
            _equation[5] = _numbersCurrent[2];
            _equation[6] = _parenthesisClose;
            _equation[7] = _operatorsCurrent[2];
            _equation[8] = _numbersCurrent[3];
            _numbersFound[bits].Add(CalculateEquation(0, 8));

            // (a + (b + c)) + d
            _equation[0] = _parenthesisOpen;
            _equation[1] = _numbersCurrent[0];
            _equation[2] = _operatorsCurrent[0];
            _equation[3] = _parenthesisOpen;
            _equation[4] = _numbersCurrent[1];
            _equation[5] = _operatorsCurrent[1];
            _equation[6] = _numbersCurrent[2];
            _equation[7] = _parenthesisClose;
            _equation[8] = _parenthesisClose;
            _equation[9] = _operatorsCurrent[2];
            _equation[10] = _numbersCurrent[3];
            _numbersFound[bits].Add(CalculateEquation(0, 10));

            // a + ((b + c) + d)
            _equation[0] = _numbersCurrent[0];
            _equation[1] = _operatorsCurrent[0];
            _equation[2] = _parenthesisOpen;
            _equation[3] = _parenthesisOpen;
            _equation[4] = _numbersCurrent[1];
            _equation[5] = _operatorsCurrent[1];
            _equation[6] = _numbersCurrent[2];
            _equation[7] = _parenthesisClose;
            _equation[8] = _operatorsCurrent[2];
            _equation[9] = _numbersCurrent[3];
            _equation[10] = _parenthesisClose;
            _numbersFound[bits].Add(CalculateEquation(0, 10));

            // a + b + (c + d)
            _equation[0] = _numbersCurrent[0];
            _equation[1] = _operatorsCurrent[0];
            _equation[2] = _numbersCurrent[1];
            _equation[3] = _operatorsCurrent[1];
            _equation[4] = _parenthesisOpen;
            _equation[5] = _numbersCurrent[2];
            _equation[6] = _operatorsCurrent[2];
            _equation[7] = _numbersCurrent[3];
            _equation[8] = _parenthesisClose;
            _numbersFound[bits].Add(CalculateEquation(0, 8));

            // a + (b + (c + d))
            _equation[0] = _numbersCurrent[0];
            _equation[1] = _operatorsCurrent[0];
            _equation[2] = _parenthesisOpen;
            _equation[3] = _numbersCurrent[1];
            _equation[4] = _operatorsCurrent[1];
            _equation[5] = _parenthesisOpen;
            _equation[6] = _numbersCurrent[2];
            _equation[7] = _operatorsCurrent[2];
            _equation[8] = _numbersCurrent[3];
            _equation[9] = _parenthesisClose;
            _equation[10] = _parenthesisClose;
            _numbersFound[bits].Add(CalculateEquation(0, 10));

            // (a + b + c) + d
            _equation[0] = _parenthesisOpen;
            _equation[1] = _numbersCurrent[0];
            _equation[2] = _operatorsCurrent[0];
            _equation[3] = _numbersCurrent[1];
            _equation[4] = _operatorsCurrent[1];
            _equation[5] = _numbersCurrent[2];
            _equation[6] = _parenthesisClose;
            _equation[7] = _operatorsCurrent[2];
            _equation[8] = _numbersCurrent[3];
            _numbersFound[bits].Add(CalculateEquation(0, 8));

            // a + (b + c + d)
            _equation[0] = _numbersCurrent[0];
            _equation[1] = _operatorsCurrent[0];
            _equation[2] = _parenthesisOpen;
            _equation[3] = _numbersCurrent[1];
            _equation[4] = _operatorsCurrent[1];
            _equation[5] = _numbersCurrent[2];
            _equation[6] = _operatorsCurrent[2];
            _equation[7] = _numbersCurrent[3];
            _equation[8] = _parenthesisClose;
            _numbersFound[bits].Add(CalculateEquation(0, 8));
        }

        private double CalculateEquation(int start, int end) {
            int lastOperator = -1;
            double num = 0;
            while (start <= end) {

                // Paranthesis
                if (_equation[start].IsParenthesis && _equation[start].IsOpen) {
                    int next = start;
                    int parCount = 1;
                    do {
                        next++;
                        if (_equation[next].IsParenthesis) {
                            if (_equation[next].IsOpen) {
                                parCount++;
                            } else {
                                parCount--;
                            }
                        }
                    } while (parCount > 0);
                    if (lastOperator == -1) {
                        num = CalculateEquation(start + 1, next - 1);
                    } else if (_equation[lastOperator].Operator == "*") {
                        num *= CalculateEquation(start + 1, next - 1);
                    } else if (_equation[lastOperator].Operator == "/") {
                        num /= CalculateEquation(start + 1, next - 1);
                    } else if (_equation[lastOperator].Operator == "+") {
                        num += CalculateEquation(start + 1, next - 1);
                    } else if (_equation[lastOperator].Operator == "-") {
                        num -= CalculateEquation(start + 1, next - 1);
                    }
                    start = next + 1;
                }
                if (start > end) {
                    break;
                }

                // Operator
                if (_equation[start].IsOperator) {
                    lastOperator = start;
                } 
                
                // Number
                if (_equation[start].IsNumber) {
                    if (lastOperator == -1) {
                        num = _equation[start].Number;
                    } else if (_equation[lastOperator].Operator == "*") {
                        num *= _equation[start].Number;
                    } else if (_equation[lastOperator].Operator == "/") {
                        num /= _equation[start].Number;
                    } else if (_equation[lastOperator].Operator == "+") {
                        num += CalculateEquation(start, end);
                        start = end;
                    } else if (_equation[lastOperator].Operator == "-") {
                        num -= CalculateEquation(start, end);
                        start = end;
                    }
                }
                start++;
            }
            return num;
        }

        private void Initialize() {
            _numbersMaster[0] = new TokenNumber0();
            _numbersMaster[1] = new TokenNumber1();
            _numbersMaster[2] = new TokenNumber2();
            _numbersMaster[3] = new TokenNumber3();
            _numbersMaster[4] = new TokenNumber4();
            _numbersMaster[5] = new TokenNumber5();
            _numbersMaster[6] = new TokenNumber6();
            _numbersMaster[7] = new TokenNumber7();
            _numbersMaster[8] = new TokenNumber8();
            _numbersMaster[9] = new TokenNumber9();
            _operatorsMaster[0] = new TokenOperatorAdd();
            _operatorsMaster[1] = new TokenOperatorSubtract();
            _operatorsMaster[2] = new TokenOperatorMultiply();
            _operatorsMaster[3] = new TokenOperatorDivide();
        }

        private double Test1() {
            // Return 16
            _equation[0] = new TokenParenthesisOpen();
            _equation[1] = new TokenNumber3();
            _equation[2] = new TokenOperatorSubtract();
            _equation[3] = new TokenNumber1();
            _equation[4] = new TokenParenthesisClose();
            _equation[5] = new TokenOperatorMultiply();
            _equation[6] = new TokenParenthesisOpen();
            _equation[7] = new TokenNumber4();
            _equation[8] = new TokenOperatorMultiply();
            _equation[9] = new TokenNumber2();
            _equation[10] = new TokenParenthesisClose();
            return CalculateEquation(0, 10);

            // Return 8
            _equation[0] = new TokenParenthesisOpen();
            _equation[1] = new TokenNumber4();
            _equation[2] = new TokenOperatorMultiply();
            _equation[3] = new TokenParenthesisOpen();
            _equation[4] = new TokenNumber1();
            _equation[5] = new TokenOperatorAdd();
            _equation[6] = new TokenNumber3();
            _equation[7] = new TokenParenthesisClose();
            _equation[8] = new TokenParenthesisClose();
            _equation[9] = new TokenOperatorDivide();
            _equation[10] = new TokenNumber2();
            return CalculateEquation(0, 10);

            // Return 14
            _equation[0] = new TokenNumber4();
            _equation[1] = new TokenOperatorMultiply();
            _equation[2] = new TokenParenthesisOpen();
            _equation[3] = new TokenNumber3();
            _equation[4] = new TokenOperatorAdd();
            _equation[5] = new TokenNumber1();
            _equation[6] = new TokenOperatorDivide();
            _equation[7] = new TokenNumber2();
            _equation[8] = new TokenParenthesisClose();
            return CalculateEquation(0, 8);

            // Return 19
            _equation[0] = new TokenNumber4();
            _equation[1] = new TokenOperatorMultiply();
            _equation[2] = new TokenParenthesisOpen();
            _equation[3] = new TokenNumber2();
            _equation[4] = new TokenOperatorAdd();
            _equation[5] = new TokenNumber3();
            _equation[6] = new TokenParenthesisClose();
            _equation[7] = new TokenOperatorSubtract();
            _equation[8] = new TokenNumber1();
            return CalculateEquation(0, 8);

            // Return 36
            _equation[0] = new TokenNumber3();
            _equation[1] = new TokenOperatorMultiply();
            _equation[2] = new TokenNumber4();
            _equation[3] = new TokenOperatorMultiply();
            _equation[4] = new TokenParenthesisOpen();
            _equation[5] = new TokenNumber2();
            _equation[6] = new TokenOperatorAdd();
            _equation[7] = new TokenNumber1();
            _equation[8] = new TokenParenthesisClose();
            return CalculateEquation(0, 8);
        }

        private abstract class Token {
            public virtual bool IsParenthesis {
                get { return false; }
            }

            public virtual bool IsOperator {
                get { return false; }
            }

            public virtual bool IsNumber {
                get { return false; }
            }

            public virtual bool IsOpen {
                get { return false; }
            }
            public virtual string Operator {
                get { return ""; }
            }

            public virtual double Number {
                get { return 0; }
            }
        }

        private class TokenParenthesisOpen : Token {
            public override bool IsParenthesis {
                get { return true; }
            }
            public override bool IsOpen {
                get { return true; }
            }
        }

        private class TokenParenthesisClose : Token {
            public override bool IsParenthesis {
                get { return true; }
            }
            public override bool IsOpen {
                get { return false; }
            }
        }

        private class TokenOperatorAdd : Token {
            public override bool IsOperator {
                get { return true; }
            }
            public override string Operator {
                get { return "+"; }
            }
        }

        private class TokenOperatorSubtract : Token {
            public override bool IsOperator {
                get { return true; }
            }
            public override string Operator {
                get { return "-"; }
            }
        }

        private class TokenOperatorMultiply : Token {
            public override bool IsOperator {
                get { return true; }
            }
            public override string Operator {
                get { return "*"; }
            }
        }

        private class TokenOperatorDivide : Token {
            public override bool IsOperator {
                get { return true; }
            }
            public override string Operator {
                get { return "/"; }
            }
        }

        private class TokenNumber0 : Token {
            public override bool IsNumber {
                get { return true; }
            }
            public override double Number {
                get { return 0; }
            }
        }

        private class TokenNumber1 : Token {
            public override bool IsNumber {
                get { return true; }
            }
            public override double Number {
                get { return 1; }
            }
        }

        private class TokenNumber2 : Token {
            public override bool IsNumber {
                get { return true; }
            }
            public override double Number {
                get { return 2; }
            }
        }

        private class TokenNumber3 : Token {
            public override bool IsNumber {
                get { return true; }
            }
            public override double Number {
                get { return 3; }
            }
        }

        private class TokenNumber4 : Token {
            public override bool IsNumber {
                get { return true; }
            }
            public override double Number {
                get { return 4; }
            }
        }

        private class TokenNumber5 : Token {
            public override bool IsNumber {
                get { return true; }
            }
            public override double Number {
                get { return 5; }
            }
        }

        private class TokenNumber6 : Token {
            public override bool IsNumber {
                get { return true; }
            }
            public override double Number {
                get { return 6; }
            }
        }

        private class TokenNumber7 : Token {
            public override bool IsNumber {
                get { return true; }
            }
            public override double Number {
                get { return 7; }
            }
        }

        private class TokenNumber8 : Token {
            public override bool IsNumber {
                get { return true; }
            }
            public override double Number {
                get { return 8; }
            }
        }

        private class TokenNumber9 : Token {
            public override bool IsNumber {
                get { return true; }
            }
            public override double Number {
                get { return 9; }
            }
        }
    }
}
