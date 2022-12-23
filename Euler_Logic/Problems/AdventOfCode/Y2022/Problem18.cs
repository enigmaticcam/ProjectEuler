using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems.AdventOfCode.Y2022 {
    public class Problem18 : AdventOfCodeBase {
        public override string ProblemName => "Advent of Code 2022: 18";

        public override string GetAnswer() {
            return Answer1(Input()).ToString();
        }

        public override string GetAnswer2() {
            return Answer2(Input()).ToString();
        }

        private int Answer1(List<string> input) {
            var state = new State();
            SetPoints(input, state);
            SetSides(state);
            return CountSides(state);
        }
        
        private int Answer2(List<string> input) {
            var state = new State();
            SetPoints(input, state);
            SetSides(state);
            SetMin(state);
            return CountOutside(state);
        }

        private int CountOutside(State state) {
            state.Outside = new HashSet<Tuple<int, int, int>>();
            foreach (var side in state.Min.Sides) {
                if (IsOutside(state, side)) {
                    Recursive(state, side);
                }
            }

            int count = 0;
            foreach (var point in state.Outside) {
                var sides = GetSides(point);
                foreach (var side in sides) {
                    if (state.Hash.Contains(side)) count++;
                }
            }
            return count;
        }

        private void Recursive(State state, Tuple<int, int, int> startPoint) {
            var list = new LinkedList<Tuple<int, int, int>>();
            list.AddFirst(startPoint);
            state.Outside.Add(startPoint);
            do {
                var point = list.First.Value;

                // Easy sides
                var sides = GetSides(point);
                foreach (var side in sides) {
                    if (IsOutside(state, side) && !state.Outside.Contains(side)) {
                        state.Outside.Add(side);
                        list.AddLast(side);
                    }
                }

                // Tricky Diagonals
                var key1 = new Tuple<int, int, int>(point.Item1 + 1, point.Item2 + 1, point.Item3);
                var key2 = new Tuple<int, int, int>(point.Item1 + 1, point.Item2, point.Item3);
                var key3 = new Tuple<int, int, int>(point.Item1, point.Item2 + 1, point.Item3);
                if (!state.Hash.Contains(key2) || !state.Hash.Contains(key3)) {
                    if (IsOutside(state, key1) && !state.Outside.Contains(key1)) {
                        state.Outside.Add(key1);
                        list.AddLast(key1);
                    }
                }

                key1 = new Tuple<int, int, int>(point.Item1 + 1, point.Item2 - 1, point.Item3);
                key2 = new Tuple<int, int, int>(point.Item1 + 1, point.Item2, point.Item3);
                key3 = new Tuple<int, int, int>(point.Item1, point.Item2 - 1, point.Item3);
                if (!state.Hash.Contains(key2) || !state.Hash.Contains(key3)) {
                    if (IsOutside(state, key1) && !state.Outside.Contains(key1)) {
                        state.Outside.Add(key1);
                        list.AddLast(key1);
                    }
                }

                key1 = new Tuple<int, int, int>(point.Item1 - 1, point.Item2 + 1, point.Item3);
                key2 = new Tuple<int, int, int>(point.Item1 - 1, point.Item2, point.Item3);
                key3 = new Tuple<int, int, int>(point.Item1, point.Item2 + 1, point.Item3);
                if (!state.Hash.Contains(key2) || !state.Hash.Contains(key3)) {
                    if (IsOutside(state, key1) && !state.Outside.Contains(key1)) {
                        state.Outside.Add(key1);
                        list.AddLast(key1);
                    }
                }

                key1 = new Tuple<int, int, int>(point.Item1 - 1, point.Item2 - 1, point.Item3);
                key2 = new Tuple<int, int, int>(point.Item1 - 1, point.Item2, point.Item3);
                key3 = new Tuple<int, int, int>(point.Item1, point.Item2 - 1, point.Item3);
                if (!state.Hash.Contains(key2) || !state.Hash.Contains(key3)) {
                    if (IsOutside(state, key1) && !state.Outside.Contains(key1)) {
                        state.Outside.Add(key1);
                        list.AddLast(key1);
                    }
                }

                key1 = new Tuple<int, int, int>(point.Item1 + 1, point.Item2, point.Item3 + 1);
                key2 = new Tuple<int, int, int>(point.Item1 + 1, point.Item2, point.Item3);
                key3 = new Tuple<int, int, int>(point.Item1, point.Item2, point.Item3 + 1);
                if (!state.Hash.Contains(key2) || !state.Hash.Contains(key3)) {
                    if (IsOutside(state, key1) && !state.Outside.Contains(key1)) {
                        state.Outside.Add(key1);
                        list.AddLast(key1);
                    }
                }

                key1 = new Tuple<int, int, int>(point.Item1 + 1, point.Item2, point.Item3 - 1);
                key2 = new Tuple<int, int, int>(point.Item1 + 1, point.Item2, point.Item3);
                key3 = new Tuple<int, int, int>(point.Item1, point.Item2, point.Item3 - 1);
                if (!state.Hash.Contains(key2) || !state.Hash.Contains(key3)) {
                    if (IsOutside(state, key1) && !state.Outside.Contains(key1)) {
                        state.Outside.Add(key1);
                        list.AddLast(key1);
                    }
                }

                key1 = new Tuple<int, int, int>(point.Item1 - 1, point.Item2, point.Item3 + 1);
                key2 = new Tuple<int, int, int>(point.Item1 - 1, point.Item2, point.Item3);
                key3 = new Tuple<int, int, int>(point.Item1, point.Item2, point.Item3 + 1);
                if (!state.Hash.Contains(key2) || !state.Hash.Contains(key3)) {
                    if (IsOutside(state, key1) && !state.Outside.Contains(key1)) {
                        state.Outside.Add(key1);
                        list.AddLast(key1);
                    }
                }

                key1 = new Tuple<int, int, int>(point.Item1 - 1, point.Item2, point.Item3 - 1);
                key2 = new Tuple<int, int, int>(point.Item1 - 1, point.Item2, point.Item3);
                key3 = new Tuple<int, int, int>(point.Item1, point.Item2, point.Item3 - 1);
                if (!state.Hash.Contains(key2) || !state.Hash.Contains(key3)) {
                    if (IsOutside(state, key1) && !state.Outside.Contains(key1)) {
                        state.Outside.Add(key1);
                        list.AddLast(key1);
                    }
                }

                key1 = new Tuple<int, int, int>(point.Item1, point.Item2 + 1, point.Item3 + 1);
                key2 = new Tuple<int, int, int>(point.Item1, point.Item2 + 1, point.Item3);
                key3 = new Tuple<int, int, int>(point.Item1, point.Item2, point.Item3 + 1);
                if (!state.Hash.Contains(key2) || !state.Hash.Contains(key3)) {
                    if (IsOutside(state, key1) && !state.Outside.Contains(key1)) {
                        state.Outside.Add(key1);
                        list.AddLast(key1);
                    }
                }

                key1 = new Tuple<int, int, int>(point.Item1, point.Item2 + 1, point.Item3 - 1);
                key2 = new Tuple<int, int, int>(point.Item1, point.Item2 + 1, point.Item3);
                key3 = new Tuple<int, int, int>(point.Item1, point.Item2, point.Item3 - 1);
                if (!state.Hash.Contains(key2) || !state.Hash.Contains(key3)) {
                    if (IsOutside(state, key1) && !state.Outside.Contains(key1)) {
                        state.Outside.Add(key1);
                        list.AddLast(key1);
                    }
                }

                key1 = new Tuple<int, int, int>(point.Item1, point.Item2 - 1, point.Item3 + 1);
                key2 = new Tuple<int, int, int>(point.Item1, point.Item2 - 1, point.Item3);
                key3 = new Tuple<int, int, int>(point.Item1, point.Item2, point.Item3 + 1);
                if (!state.Hash.Contains(key2) || !state.Hash.Contains(key3)) {
                    if (IsOutside(state, key1) && !state.Outside.Contains(key1)) {
                        state.Outside.Add(key1);
                        list.AddLast(key1);
                    }
                }

                key1 = new Tuple<int, int, int>(point.Item1, point.Item2 - 1, point.Item3 - 1);
                key2 = new Tuple<int, int, int>(point.Item1, point.Item2 - 1, point.Item3);
                key3 = new Tuple<int, int, int>(point.Item1, point.Item2, point.Item3 - 1);
                if (!state.Hash.Contains(key2) || !state.Hash.Contains(key3)) {
                    if (IsOutside(state, key1) && !state.Outside.Contains(key1)) {
                        state.Outside.Add(key1);
                        list.AddLast(key1);
                    }
                }
                list.RemoveFirst();
            } while (list.Count > 0);
        }

        private bool IsOutside(State state, Tuple<int, int, int> point) {
            if (state.Hash.Contains(point)) return false;
            var sides = GetSides(point);
            return sides.Any(x => state.Hash.Contains(x));
        }

        private void SetMin(State state) {
            var lowest = state.Points
                .OrderBy(x => x.Key.Item1)
                .ThenBy(x => x.Key.Item2)
                .ThenBy(x => x.Key.Item3)
                .First();
            state.Min = lowest;
        }

        private int CountSides(State state) {
            int count = 0;
            foreach (var point in state.Points) {
                foreach (var side in point.Sides) {
                    if (!state.Hash.Contains(side)) count++;
                }
            }
            return count;
        }

        private void SetSides(State state) {
            foreach (var point in state.Points) {
                point.Sides = GetSides(point.Key);
            }
        }

        private List<Tuple<int, int, int>> GetSides(Tuple<int, int,int> point) {
            var sides = new List<Tuple<int, int, int>>();
            sides.Add(new Tuple<int, int, int>(point.Item1 + 1, point.Item2, point.Item3));
            sides.Add(new Tuple<int, int, int>(point.Item1 - 1, point.Item2, point.Item3));
            sides.Add(new Tuple<int, int, int>(point.Item1, point.Item2 + 1, point.Item3));
            sides.Add(new Tuple<int, int, int>(point.Item1, point.Item2 - 1, point.Item3));
            sides.Add(new Tuple<int, int, int>(point.Item1, point.Item2, point.Item3 + 1));
            sides.Add(new Tuple<int, int, int>(point.Item1, point.Item2, point.Item3 - 1));
            return sides;
        }

        private void SetPoints(List<string> input, State state) {
            state.Points = input.Select(line => {
                var split = line.Split(',');
                return new Point() {
                    Key = new Tuple<int, int, int>(Convert.ToInt32(split[0]), Convert.ToInt32(split[1]), Convert.ToInt32(split[2])),
                    Sides = new List<Tuple<int, int, int>>()
                };
            }).ToList();
            state.Hash = new HashSet<Tuple<int, int, int>>(state.Points.Select(x => x.Key));
        }

        private class State {
            public List<Point> Points { get; set; }
            public Point Min { get; set; }
            public HashSet<Tuple<int, int, int>> Hash { get; set; }
            public HashSet<Tuple<int, int, int>> Outside { get; set; }
        }

        public class Point {
            public Tuple<int, int, int> Key { get; set; }
            public List<Tuple<int, int, int>> Sides { get; set; }
        }
    }
}
