using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Euler_Logic.Problems.AdventOfCode.Y2022 {
    public class Problem17 : AdventOfCodeBase {
        public override string ProblemName => "Advent of Code 2022: 17";
        /*
            1554117647080 - too high
            1554117647067 - not right
            1554117647066 - not right
            1554117646867 - not right
         */

        public override string GetAnswer() {
            //return Answer1(Input_Test(1)).ToString();
            return Answer1(Input()).ToString();
        }

        public override string GetAnswer2() {
            //return Answer2(Input_Test(1)).ToString();
            return Answer2(Input()).ToString();
        }

        private int Answer1(List<string> input) {
            var state = new State() { JetIndex = -1 };
            SetState(state, input);
            Simulate(state, 2022);
            return state.Grid.Count;
        }

        private ulong Answer2(List<string> input) {
            var state = new State() { JetIndex = -1 };
            SetState(state, input);
            FindCycle(state, input);
            state.ShapeCountCycleStart = ShapeCountForKey(state, state.GridKeyToCycleStart);
            state.ShapeCountCycleEnd = ShapeCountForKey(state, state.GridKeyToCycleEnd);
            var jetIndex = state.JetIndex;
            var shapeIndex = state.ShapeIndex;

            var shapesPerCycle = state.ShapeCountCycleEnd - state.ShapeCountCycleStart;
            var cycleCount = ((ulong)1000000000000 - (ulong)state.ShapeCountCycleStart) / (ulong)shapesPerCycle;
            var remaining = (int)(1000000000000 - (cycleCount * (ulong)shapesPerCycle) - (ulong)state.ShapeCountCycleStart);
            state.Grid = new List<int>(state.GridKeyToCycleEnd);
            state.JetIndex = jetIndex;
            state.ShapeIndex = shapeIndex;
            Simulate(state, remaining);

            var heightPerCycle = state.GridKeyToCycleEnd.Count - state.GridKeyToCycleStart.Count;
            var heightForAllCycles = (ulong)heightPerCycle * cycleCount;
            var final = (ulong)state.Grid.Count - (ulong)state.GridKeyToCycleEnd.Count + heightForAllCycles + (ulong)state.GridKeyToCycleStart.Count;
            return final;
        }

        private int ShapeCountForKey(State state, List<int> key) {
            state.Grid = new List<int>();
            state.JetIndex = -1;
            state.ShapeIndex = 0;
            int count = 0;
            do {
                count++;
                AddShape(state);
                if (DoesMatch(key, state.Grid)) return count;
            } while (true);
        }

        private bool DoesMatch(List<int> key, List<int> grid) {
            if (grid.Count < key.Count) return false;
            for (int index = 0; index < key.Count; index++) {
                if (key[index] != grid[index]) return false;
            }
            return true;
        }

        private bool FindCycle(State state, List<string> input) {
            Simulate(state, state.Shapes.Count * input[0].Length);
            var hash = new Dictionary<int, List<int>>();
            for (int index = 0; index < state.Grid.Count; index++) {
                var line = state.Grid[index];
                if (!hash.ContainsKey(line)) hash.Add(line, new List<int>());
                hash[line].Add(index);
            }
            for (int index = 0; index < state.Grid.Count; index++) {
                foreach (var start in hash[state.Grid[index]]) {
                    if (start > index + 10) {
                        var vector = new int[3] { index, start, (start - index) + start };
                        if (vector[2] < state.Grid.Count) {
                            bool isGood = true;
                            do {
                                if (state.Grid[vector[0]] != state.Grid[vector[1]] || state.Grid[vector[1]] != state.Grid[vector[2]]) {
                                    isGood = false;
                                    break;
                                }
                                vector[0]++;
                                vector[1]++;
                                vector[2]++;
                                if (vector[2] >= state.Grid.Count) {
                                    isGood = false;
                                    break;
                                }
                            } while (start > vector[0]);
                            if (isGood) {
                                state.CycleStart = index;
                                state.CycleEnd = start - 1;
                                state.GridKeyToCycleStart = state.Grid.Take(index).ToList();
                                state.GridKeyToCycleEnd = state.Grid.Take(start).ToList();
                                return false;
                            }
                        }
                    }
                }
            }
            return false;
        }

        private void Simulate(State state, int totalPieces) {
            for (int count = 1; count <= totalPieces; count++) {
                AddShape(state);
            }
        }

        private void AddShape(State state) {
            var shape = new List<int>(state.Shapes[state.ShapeIndex].Select(x => x << 1));
            state.ShapeIndex = (state.ShapeIndex + 1) % state.Shapes.Count;
            MoveShape(state, shape);
        }

        private void MoveShape(State state, List<int> shape) {
            int start = state.Grid.Count + 2 + shape.Count;
            do {
                state.JetIndex = (state.JetIndex + 1) % state.Jet.Length;
                if (state.Jet[state.JetIndex] == '<') {
                    MoveHorizontal(state, shape, start, -1);
                } else {
                    MoveHorizontal(state, shape, start, 1);
                }
                if (!MoveVertical(state, shape, start)) {
                    break;
                }
                start--;
            } while (true);
        }

        private void MoveHorizontal(State state, List<int> shape, int start, int direction) {
            bool canMove = true;
            int shapeIndex = shape.Count - 1;
            for (int gridIndex = start - shape.Count + 1; gridIndex <= start; gridIndex++) {
                int shift = direction == -1 ? shape[shapeIndex] << 1 : shape[shapeIndex] >> 1;
                int end = direction == -1 ? 64 : 1;
                if ((shape[shapeIndex] & end) != 0) {
                    canMove = false;
                    break;
                }
                if (gridIndex < state.Grid.Count && (state.Grid[gridIndex] & shift) != 0) {
                    canMove = false;
                    break;
                }
                shapeIndex--;
            }
            if (canMove) {
                for (int index = 0; index < shape.Count; index++) {
                    if (direction == -1) {
                        shape[index] <<= 1;
                    } else {
                        shape[index] >>= 1;
                    }
                }
            }
        }

        private bool MoveVertical(State state, List<int> shape, int start) {
            bool canMove = start - shape.Count >= 0;
            if (canMove) {
                int shapeIndex = shape.Count - 1;
                for (int gridIndex = start - shape.Count; gridIndex < start; gridIndex++) {
                    if (gridIndex < state.Grid.Count && (state.Grid[gridIndex] & shape[shapeIndex]) != 0) {
                        canMove = false;
                        break;
                    }
                    shapeIndex--;
                }
            }
            if (!canMove) {
                int shapeIndex = shape.Count - 1;
                for (int gridIndex = start - shape.Count + 1; gridIndex <= start; gridIndex++) {
                    while (gridIndex >= state.Grid.Count) state.Grid.Add(0);
                    state.Grid[gridIndex] |= shape[shapeIndex];
                    shapeIndex--;
                }
            }
            return canMove;
        }

        private void SetState(State state, List<string> input) {
            state.Grid = new List<int>();
            state.Jet = input[0];
            state.Shapes = new List<List<int>>();
            state.Shapes.Add(new List<int>() { 15 });
            state.Shapes.Add(new List<int>() { 4, 14, 4 });
            state.Shapes.Add(new List<int>() { 2, 2, 14 });
            state.Shapes.Add(new List<int>() { 8, 8, 8, 8 });
            state.Shapes.Add(new List<int>() { 12, 12 });
        }

        private void OutputToFile(State state) {
            var text = Output(state, true);
            File.WriteAllText(@"C:\temp\output.txt", text);
        }

        private string Output(State state, bool reverse) {
            var text = new StringBuilder();
            int index = 0;
            int direction = 1;
            int stop = state.Grid.Count;
            if (!reverse) {
                index = state.Grid.Count - 1;
                direction = -1;
                stop = -1;
            }
            while (index != stop) {
                var line = state.Grid[index];
                int powerOf2 = 64;
                do {
                    if ((line & powerOf2) != 0) {
                        text.Append("#");
                    } else {
                        text.Append(".");
                    }
                    powerOf2 /= 2;
                } while (powerOf2 >= 1);
                text.AppendLine("");
                index += direction;
            }
            return text.ToString();
        }

        private class State {
            public List<int> Grid { get; set; }
            public List<List<int>> Shapes { get; set; }
            public int ShapeIndex { get; set; }
            public string Jet { get; set; }
            public int JetIndex { get; set; }
            public int CycleStart { get; set; }
            public int CycleEnd { get; set; }
            public List<int> GridKeyToCycleStart { get; set; }
            public List<int> GridKeyToCycleEnd { get; set; }
            public int ShapeCountCycleStart { get; set; }
            public int ShapeCountCycleEnd { get; set; }
        }
    }
}
