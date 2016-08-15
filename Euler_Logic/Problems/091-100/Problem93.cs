using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem93 : IProblem {
        private Token[] _numbersMaster = new Token[10];
        private Token[] _operatorsMaster = new Token[4];
        private Token _parenthesisOpen = new TokenParenthesisOpen();
        private Token _parenthesisClose = new TokenParenthesisOpen();
        private Token[] _numbersCurrent = new Token[4];
        private Token[] _operatorsCurrent = new Token[3];
        private Token[] _equation = new Token[11];

        public string ProblemName {
            get { return "93: Arithmetic expressions"; }
        }

        public string GetAnswer() {
            Initialize();
            FindAllNumbers( 0, 3, -1);
            return "done";
        }

        private void FindAllNumbers(int start, int end, int lastIndex) {
            for (int index = lastIndex + 1; index <= _numbersMaster.GetUpperBound(0); index++) {
                _numbersCurrent[start] = _numbersMaster[index];
                if (start != end) {
                    FindAllNumbers(start + 1, end, index);
                } else {
                    FindAllOperators(0, 2);
                }
            }
        }

        private void FindAllOperators(int start, int end) {
            for (int index = 0; index <= _operatorsMaster.GetUpperBound(0); index++) {
                _operatorsCurrent[start] = _operatorsMaster[index];
                if (start != end) {
                    FindAllOperators(start + 1, end);
                } else {
                    bool stophere = true;
                }
            }
        }

        private void FindAllParanthesis() {
            // a + b + c + d
            //_equation[0] = _numbersCurrent[0];
            //_equation[1] = _operatorsCurrent[0];
            //_equation[2] = _numbersCurrent[1];
            //_equation[3] = _operatorsCurrent[1];
            //_equation[4] = _numbersCurrent[2];
            //_equation[5] = _operatorsCurrent[2];
            //_equation[6] = _numbersCurrent[3];

            // (a + b) + c + d
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
            CalculateEquation(0, 10);

            // (a + b) + (c + d)

            // a + (b + c) + d
            // (a + (b + c)) + d

            // a + ((b + c) + d)

            // a + b + (c + d)

            // a + (b + (c + d))

            // (a + b + c) + d

            // a + (b + c + d)
        }

        private int CalculateEquation(int start, int end) {
            
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

            public virtual int Number {
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
            public override int Number {
                get { return 0; }
            }
        }

        private class TokenNumber1 : Token {
            public override bool IsNumber {
                get { return true; }
            }
            public override int Number {
                get { return 1; }
            }
        }

        private class TokenNumber2 : Token {
            public override bool IsNumber {
                get { return true; }
            }
            public override int Number {
                get { return 2; }
            }
        }

        private class TokenNumber3 : Token {
            public override bool IsNumber {
                get { return true; }
            }
            public override int Number {
                get { return 3; }
            }
        }

        private class TokenNumber4 : Token {
            public override bool IsNumber {
                get { return true; }
            }
            public override int Number {
                get { return 4; }
            }
        }

        private class TokenNumber5 : Token {
            public override bool IsNumber {
                get { return true; }
            }
            public override int Number {
                get { return 5; }
            }
        }

        private class TokenNumber6 : Token {
            public override bool IsNumber {
                get { return true; }
            }
            public override int Number {
                get { return 6; }
            }
        }

        private class TokenNumber7 : Token {
            public override bool IsNumber {
                get { return true; }
            }
            public override int Number {
                get { return 7; }
            }
        }

        private class TokenNumber8 : Token {
            public override bool IsNumber {
                get { return true; }
            }
            public override int Number {
                get { return 8; }
            }
        }

        private class TokenNumber9 : Token {
            public override bool IsNumber {
                get { return true; }
            }
            public override int Number {
                get { return 9; }
            }
        }
    }
}
