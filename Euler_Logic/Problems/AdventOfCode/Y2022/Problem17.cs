using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Euler_Logic.Problems.AdventOfCode.Y2022 {
    public class Problem17 : AdventOfCodeBase {
        public override string ProblemName => "Advent of Code 2022: 17";

        public override string GetAnswer() {
            return Answer1(Input()).ToString();
        }

        public override string GetAnswer2() {
            return Answer2(Input()).ToString();
        }

        private int Answer1(List<string> jet) {
            var state = new State() { Jet = jet[0] };
            BuildShapes(state);
            BuildFloor(state);
            DropShapes(state, 2022);
            return FinalHeight(state);
        }

        private ulong Answer2(List<string> jet) {
            var state = new State() { Jet = jet[0] };
            BuildShapes(state);
            BuildFloor(state);
            return FindCycle(state);
        }

        private ulong FindCycle(State state) {
            var cycleSize = state.Jet.Length * state.Shapes.Count;
            var shapesAdded = (ulong)cycleSize;
            DropShapes(state, cycleSize);
            RemoveEmpty(state);
            var start = FindNextStart(state);
            var height = FindDuplicate(state, start);
            shapesAdded += (ulong)FindNextDuplicate(state, start, height);

            start = FindNextStart(state);
            height = FindDuplicate(state, start);
            int cycleHeight = state.Grid.Count;
            ulong cycleShapesAdded = (ulong)FindNextDuplicate(state, start, height);
            shapesAdded += cycleShapesAdded;
            cycleHeight = state.Grid.Count - cycleHeight;
            ulong cycleCount = (1000000000000 - shapesAdded) / cycleShapesAdded;
            ulong totalHeight = (ulong)state.Grid.Count + ((ulong)cycleHeight * cycleCount);
            ulong shapesRemaining = 1000000000000 - (shapesAdded + (cycleShapesAdded * cycleCount));
            var startHeight = (ulong)state.Grid.Count;
            DropShapes(state, (int)shapesRemaining);
            RemoveEmpty(state);
            return totalHeight + ((ulong)state.Grid.Count - startHeight) - 1;
        }

        private int FindNextDuplicate(State state, int start, int height) {
            var key = new List<int>(state.Grid.Skip(start).Take(height));
            int count = state.Grid.Count;
            int shapeCount = 0;
            do {
                DropShapeWithRoom(state);
                shapeCount++;
                if (state.Grid.Count - count >= height * 2) {
                    var nextStart = FindNextStart(state);
                    bool isGood = true;
                    for (int index = 0; index < key.Count; index++) {
                        if (key[index] != state.Grid[nextStart + index]) {
                            isGood = false;
                            break;
                        }
                    }
                    if (isGood) {
                        return shapeCount;
                    }
                }
                
            } while (true);
        }

        private int FindDuplicate(State state, int startIndex) {
            int height = 0;
            for (int subIndex = startIndex + 1; subIndex < state.Grid.Count; subIndex++) {
                height++;
                if (state.Grid[startIndex] == state.Grid[subIndex]) {
                    bool isGood = true;
                    for (int count = 1; count <= height; count++) {
                        if (state.Grid[startIndex + count] != state.Grid[subIndex + count]) {
                            isGood = false;
                            break;
                        }
                    }
                    if (isGood) return height;
                }
            }
            return -1;
        }

        private int FindNextStart(State state) {
            int bits = 0;
            for (int index = 0; index < state.Grid.Count; index++) {
                bits |= state.Grid[index];
                if (bits == 127) return index;
            }
            return -1;
        }

        private void RemoveEmpty(State state) {
            while (state.Grid[0] == 0) {
                state.Grid.RemoveAt(0);
            }
        }

        private int FinalHeight(State state) {
            RemoveEmpty(state);
            return state.Grid.Count - 1;
        }

        private void DropShapes(State state, int max) {
            for (int count = 1; count <= max; count++) {
                DropShapeWithRoom(state);
            }
        }

        private void DropShapeWithRoom(State state) {
            AddRoomForNewShape(state);
            DropShape(state);
            state.ShapeIndex = (state.ShapeIndex + 1) % state.Shapes.Count;
        }

        private void DropShape(State state) {
            var shape = new List<int>(state.Shapes[state.ShapeIndex].Bits);
            int shapeInGridIndex = shape.Count * -1;
            do {
                var direction = enumDirection.Left;
                if (state.Jet[state.JetIndex] == '>') direction = enumDirection.Right;
                if (CanMove(state, shape, shapeInGridIndex, direction)) {
                    Move(shape, direction);
                }
                state.JetIndex = (state.JetIndex + 1) % state.Jet.Length;
                if (CanMove(state, shape, shapeInGridIndex, enumDirection.Down)) {
                    shapeInGridIndex++;
                } else {
                    Solidify(state, shape, shapeInGridIndex);
                    break;
                }
            } while (true);
        }

        private void Solidify(State state, List<int> shape, int shapeInGridIndex) {
            while (shapeInGridIndex < 0) {
                state.Grid.Insert(0, 0);
                shapeInGridIndex++;
            }
            for (int index = 0; index < shape.Count; index++) {
                state.Grid[shapeInGridIndex + index] += shape[index];
            }
        }

        private void Move(List<int> shape, enumDirection direction) {
            for (int index = 0; index < shape.Count; index++) {
                if (direction == enumDirection.Left) {
                    shape[index] <<= 1;
                } else {
                    shape[index] >>= 1;
                }
            }
        }

        private bool CanMove(State state, List<int> shape, int shapeInGridIndex, enumDirection direction) {
            for (int index = 0; index < shape.Count; index++) {
                if (index + shapeInGridIndex >= 0) {
                    int bits = shape[index];
                    if (direction == enumDirection.Left) {
                        if ((bits & 64) != 0) return false;
                        bits <<= 1;
                        if ((bits & state.Grid[shapeInGridIndex + index]) != 0) return false;
                    } else if (direction == enumDirection.Right) {
                        if ((bits & 1) != 0) return false;
                        bits >>= 1;
                        if ((bits & state.Grid[shapeInGridIndex + index]) != 0) return false;
                    } else {
                        if ((bits & state.Grid[shapeInGridIndex + index + 1]) != 0) return false;
                    }
                }
            }
            return true;
        }

        private void AddRoomForNewShape(State state) {
            int toAdd = 3;
            for (int index = 0; index < 3; index++) {
                if (state.Grid.Count <= index) break;
                if (state.Grid[index] == 0) toAdd--;
            }
            for (int count = 1; count <= toAdd; count++) {
                state.Grid.Insert(0, 0);
            }
        }

        private void BuildFloor(State state) {
            state.Grid.Add(127);
        }

        private void BuildShapes(State state) {
            state.Shapes = new List<Shape> {
                new Shape() {
                    Bits = new List<int>() {
                        30
                    }
                },
                new Shape() {
                    Bits = new List<int>() {
                        8,
                        28,
                        8
                    }
                },
                new Shape() {
                    Bits = new List<int>() {
                        4,
                        4,
                        28
                    }
                },
                new Shape() {
                    Bits = new List<int>() {
                        16,
                        16,
                        16,
                        16
                    }
                },
                new Shape() {
                    Bits = new List<int>() {
                        24,
                        24
                    }
                }
            };
        }

        private string Output(State state) {
            var text = new StringBuilder();
            foreach (var line in state.Grid) {
                for (int bit = 64; bit >= 1; bit /= 2) {
                    if ((line & bit) == bit) {
                        text.Append("#");
                    } else {
                        text.Append(".");
                    }
                }
                text.AppendLine();
            }
            return text.ToString();
        }

        private class State {
            public List<Shape> Shapes { get; set; } = new List<Shape>();
            public int ShapeIndex { get; set; }
            public string Jet { get; set; }
            public int JetIndex { get; set; }
            public List<int> Grid { get; set; } = new List<int>();
        }

        private class Shape {
            public List<int> Bits { get; set; }
        }

        private enum enumDirection {
            Left,
            Right,
            Down
        }
    }
}
