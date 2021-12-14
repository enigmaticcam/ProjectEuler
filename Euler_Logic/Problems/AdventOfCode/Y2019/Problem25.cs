using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems.AdventOfCode.Y2019 {
    public class Problem25 : AdventOfCodeBase {
        private List<string> _inputs;

        public override string ProblemName {
            get { return "Advent of Code 2019: 25"; }
        }

        public override string GetAnswer() {
            return Answer1().ToString();
        }

        private string Answer1() {
            ulong bits = 1;
            do {
                var comp = new IntComputer();
                SetInputs(comp, bits);
                comp.Run(Input(), () => ProvideInput(comp), () => { });
                var result = CheckOutput(comp);
                if (result.Length > 0) {
                    return result;
                }
                bits++;
            } while (true);
        }

        private string CheckOutput(IntComputer comp) {
            var output = Output(comp);
            if (output.IndexOf("Droids on this ship are") == -1) {
                var index = output.IndexOf("typing");
                var nextSpace = output.IndexOf(" ", index + 7);
                var result = output.Substring(index + 7, nextSpace - (index + 7));
                return result;
            }
            return "";
        }

        private void SetInputs(IntComputer comp, ulong bits) {
            _inputs = new List<string>();
            _inputs.Add("north");
            if ((bits & 1) == 1) {
                _inputs.Add("take festive hat");
            }
            _inputs.Add("south");
            _inputs.Add("south");
            _inputs.Add("south");
            _inputs.Add("south");
            _inputs.Add("east");
            _inputs.Add("east");
            if ((bits & 2) == 2) {
                _inputs.Add("take planetoid");
            }
            _inputs.Add("west");
            _inputs.Add("west");
            _inputs.Add("north");
            _inputs.Add("north");
            _inputs.Add("east");
            if ((bits & 4) == 4) {
                _inputs.Add("take space heater");
            }
            _inputs.Add("west");
            _inputs.Add("north");
            _inputs.Add("north");
            _inputs.Add("west");
            if ((bits & 8) == 8) {
                _inputs.Add("take dark matter");
            }
            _inputs.Add("north");
            _inputs.Add("east");
            if ((bits & 16) == 16) {
                _inputs.Add("take semiconductor");
            }
            _inputs.Add("east");
            if ((bits & 32) == 32) {
                _inputs.Add("take sand");
            }
            _inputs.Add("west");
            _inputs.Add("west");
            _inputs.Add("south");
            _inputs.Add("east");
            _inputs.Add("south");
            _inputs.Add("east");
            if ((bits & 64) == 64) {
                _inputs.Add("take spool of cat6");
            }
            _inputs.Add("north");
            _inputs.Add("north");
            if ((bits & 128) == 128) {
                _inputs.Add("take hypercube");
            }
            _inputs.Add("south");
            _inputs.Add("south");
            _inputs.Add("west");
            _inputs.Add("north");
            _inputs.Add("west");
            _inputs.Add("north");
            _inputs.Add("east");
            _inputs.Add("east");
            _inputs.Add("north");
            _inputs.Add("west");
            SetInput(_inputs[0], comp);
        }

        private int ProvideInput(IntComputer comp) {
            if (_inputIndex == _input.Length - 1) {
                _inputs.RemoveAt(0);
                if (_inputs.Count == 0) {
                    comp.PerformFinish = true;
                    return 0;
                } else {
                    SetInput(_inputs[0], comp);
                }
            }
            _inputIndex++;
            return (int)_input[_inputIndex];
        }

        private void SetInput(string input, IntComputer comp, bool saveToHistory) {
            _inputs.Add(input);
            SetInput(_inputs[0], comp);
        }

        private string _input;
        private int _inputIndex;
        private void SetInput(string input, IntComputer comp) {
            _input = input += (char)10;
            _inputIndex = -1;
            comp.Output.Clear();
        }

        private string Output(IntComputer comp) {
            var text = new StringBuilder();
            foreach (var digit in comp.Output) {
                text.Append((char)digit);
            }
            return text.ToString();
        }
    }
}
