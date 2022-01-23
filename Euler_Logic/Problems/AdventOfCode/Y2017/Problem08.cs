using System;
using System.Collections.Generic;
using System.Linq;

namespace Euler_Logic.Problems.AdventOfCode.Y2017 {
    public class Problem08 : AdventOfCodeBase {
        public override string ProblemName {
            get { return "Advent of Code 2017: 8"; }
        }

        public override string GetAnswer() {
            return Answer1(Input());
        }

        public override string GetAnswer2() {
            return Answer2(Input());
        }

        private Dictionary<string, int> _registers = new Dictionary<string, int>();
        private string Answer1(List<string> input) {
            foreach (string text in input) {
                Register register = new Register(text);
                switch (register.IfOp) {
                    case "!=":
                        if (GetRegisterValue(register.IfName) != register.IfValue) {
                            if (!_registers.ContainsKey(register.Name)) {
                                _registers.Add(register.Name, 0);
                            }
                            _registers[register.Name] += register.Offset;
                        }
                        break;
                    case ">=":
                        if (GetRegisterValue(register.IfName) >= register.IfValue) {
                            if (!_registers.ContainsKey(register.Name)) {
                                _registers.Add(register.Name, 0);
                            }
                            _registers[register.Name] += register.Offset;
                        }
                        break;
                    case ">":
                        if (GetRegisterValue(register.IfName) > register.IfValue) {
                            if (!_registers.ContainsKey(register.Name)) {
                                _registers.Add(register.Name, 0);
                            }
                            _registers[register.Name] += register.Offset;
                        }
                        break;
                    case "<=":
                        if (GetRegisterValue(register.IfName) <= register.IfValue) {
                            if (!_registers.ContainsKey(register.Name)) {
                                _registers.Add(register.Name, 0);
                            }
                            _registers[register.Name] += register.Offset;
                        }
                        break;
                    case "<":
                        if (GetRegisterValue(register.IfName) < register.IfValue) {
                            if (!_registers.ContainsKey(register.Name)) {
                                _registers.Add(register.Name, 0);
                            }
                            _registers[register.Name] += register.Offset;
                        }
                        break;
                    case "==":
                        if (GetRegisterValue(register.IfName) == register.IfValue) {
                            if (!_registers.ContainsKey(register.Name)) {
                                _registers.Add(register.Name, 0);
                            }
                            _registers[register.Name] += register.Offset;
                        }
                        break;
                }
            }
            return _registers.Values.Max().ToString();
        }

        private string Answer2(List<string> input) {
            int maxValue = int.MinValue;
            foreach (string text in input) {
                Register register = new Register(text);
                if (!_registers.ContainsKey(register.Name)) {
                    _registers.Add(register.Name, 0);
                }
                switch (register.IfOp) {
                    case "!=":
                        if (GetRegisterValue(register.IfName) != register.IfValue) {
                            _registers[register.Name] += register.Offset;
                        }
                        break;
                    case ">=":
                        if (GetRegisterValue(register.IfName) >= register.IfValue) {
                            _registers[register.Name] += register.Offset;
                        }
                        break;
                    case ">":
                        if (GetRegisterValue(register.IfName) > register.IfValue) {
                            _registers[register.Name] += register.Offset;
                        }
                        break;
                    case "<=":
                        if (GetRegisterValue(register.IfName) <= register.IfValue) {
                            _registers[register.Name] += register.Offset;
                        }
                        break;
                    case "<":
                        if (GetRegisterValue(register.IfName) < register.IfValue) {
                            _registers[register.Name] += register.Offset;
                        }
                        break;
                    case "==":
                        if (GetRegisterValue(register.IfName) == register.IfValue) {
                            _registers[register.Name] += register.Offset;
                        }
                        break;
                }
                if (_registers[register.Name] > maxValue) {
                    maxValue = _registers[register.Name];
                }
            }
            return maxValue.ToString();
        }

        private int GetRegisterValue(string registerName) {
            if (!_registers.ContainsKey(registerName)) {
                _registers.Add(registerName, 0);
            }
            return _registers[registerName];
        }

        private int GetOffset(string offset) {
            if (offset == "inc") {
                return 1;
            } else {
                return -1;
            }
        }

        private class Register {
            public string Name { get; set; }
            public int Offset { get; set; }
            public string IfName { get; set; }
            public string IfOp { get; set; }
            public int IfValue { get; set; }

            public Register(string text) {
                string[] particles = text.Split(' ');
                this.Name = particles[0];
                this.Offset = (particles[1] == "inc" ? 1 : -1) * Convert.ToInt32(particles[2]);
                this.IfName = particles[4];
                this.IfOp = particles[5];
                this.IfValue = Convert.ToInt32(particles[6]);
            }
        }
    }
}
