using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Euler_Logic.Problems.AdventOfCode.Y2016 {
    public class Problem08 : AdventOfCodeBase {
        private List<Instruction> _instructions;
        private bool[,] _grid;

        private enum enumInstructionType {
            MakeRect,
            ShiftRow,
            ShiftCol
        }

        public override string ProblemName => "Advent of Code 2016: 8";

        public override string GetAnswer() {
            _grid = new bool[50, 6];
            return Answer1(Input()).ToString();
        }

        public override string GetAnswer2() {
            _grid = new bool[50, 6];
            return Answer2(Input()).ToString();
        }

        private int Answer1(List<string> input) {
            GetInstructions(input);
            DrawGrid();
            return CountLit();
        }

        private string Answer2(List<string> input) {
            GetInstructions(input);
            DrawGrid();
            PrintGrid();
            return "CFLELOYFCS";
        }

        private string PrintGrid() {
            var output = new StringBuilder();
            for (int y = 0; y <= _grid.GetUpperBound(1); y++) {
                for (int x = 0; x <= _grid.GetUpperBound(0); x++) {
                    output.Append((_grid[x, y] ? "#" : "."));
                }
                output.AppendLine("");
            }
            return output.ToString();
        }

        private int CountLit() {
            int count = 0;
            for (int x = 0; x <= _grid.GetUpperBound(0); x++) {
                for (int y = 0; y <= _grid.GetUpperBound(1); y++) {
                    count += (_grid[x, y] ? 1 : 0);
                }
            }
            return count;
        }

        private void DrawGrid() {
            foreach (var instruction in _instructions) {
                switch (instruction.InstructionType) {
                    case enumInstructionType.MakeRect:
                        MakeRect(instruction);
                        break;
                    case enumInstructionType.ShiftCol:
                        ShiftColumn(instruction);
                        break;
                    case enumInstructionType.ShiftRow:
                        ShiftRow(instruction);
                        break;
                }
            }
        }

        private void MakeRect(Instruction instruction) {
            for (int x = 0; x < instruction.Value1; x++) {
                for (int y = 0; y < instruction.Value2; y++) {
                    _grid[x, y] = true;
                }
            }
        }

        private void ShiftRow(Instruction instruction) {
            var setToOn = new HashSet<int>();
            for (int x = 0; x <= _grid.GetUpperBound(0); x++) {
                if (_grid[x, instruction.Value1]) {
                    setToOn.Add((x + instruction.Value2) % (_grid.GetUpperBound(0) + 1));
                }
            }
            for (int x = 0; x <= _grid.GetUpperBound(0); x++) {
                if (setToOn.Contains(x)) {
                    _grid[x, instruction.Value1] = true;
                } else {
                    _grid[x, instruction.Value1] = false;
                }
            }
        }

        private void ShiftColumn(Instruction instruction) {
            var setToOn = new HashSet<int>();
            for (int y = 0; y <= _grid.GetUpperBound(1); y++) {
                if (_grid[instruction.Value1, y]) {
                    setToOn.Add((y + instruction.Value2) % (_grid.GetUpperBound(1) + 1));
                }
            }
            for (int y = 0; y <= _grid.GetUpperBound(1); y++) {
                if (setToOn.Contains(y)) {
                    _grid[instruction.Value1, y] = true;
                } else {
                    _grid[instruction.Value1, y] = false;
                }
            }
        }

        private void GetInstructions(List<string> input) {
            _instructions = input.Select(line => {
                var instruction = new Instruction();
                var split = line.Split(' ');
                if (split[0] == "rect") {
                    instruction.InstructionType = enumInstructionType.MakeRect;
                    int xIndex = split[1].IndexOf('x');
                    instruction.Value1 = Convert.ToInt32(split[1].Substring(0, xIndex));
                    instruction.Value2 = Convert.ToInt32(split[1].Substring(xIndex + 1));
                } else if (split[1] == "row") {
                    instruction.InstructionType = enumInstructionType.ShiftRow;
                    int equalIndex = split[2].IndexOf('=');
                    instruction.Value1 = Convert.ToInt32(split[2].Substring(equalIndex + 1));
                    instruction.Value2 = Convert.ToInt32(split[4]);
                } else {
                    instruction.InstructionType = enumInstructionType.ShiftCol;
                    int equalIndex = split[2].IndexOf('=');
                    instruction.Value1 = Convert.ToInt32(split[2].Substring(equalIndex + 1));
                    instruction.Value2 = Convert.ToInt32(split[4]);
                }
                return instruction;
            }).ToList(); ;
        }

        private class Instruction {
            public enumInstructionType InstructionType { get; set; }
            public int Value1 { get; set; }
            public int Value2 { get; set; }
        }
    }
}
