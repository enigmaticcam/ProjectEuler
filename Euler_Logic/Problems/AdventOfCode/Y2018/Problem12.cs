using System.Collections.Generic;
using System.Linq;

namespace Euler_Logic.Problems.AdventOfCode.Y2018 {
    public class Problem12 : AdventOfCodeBase {
        public override string ProblemName {
            get { return "Advent of Code 2018: 12"; }
        }

        public override string GetAnswer() {
            return Answer2();
        }

        private string Answer1() {
            var state = GetState();
            ReplaceMissingMasks(state);
            PerformGeneration(state, 20);
            return Sum(state).ToString();
        }

        private string Answer2() {
            var state = GetState();
            ReplaceMissingMasks(state);
            var remaining = FindConverge(state);
            var sum = Sum(state);
            var count = Count(state);
            return GetNextCount((ulong)sum, (ulong)count, 50000000000 - remaining).ToString();
        }

        private int Sum(State state) {
            int sum = 0;
            foreach (var pot in state.PotObjects) {
                sum += pot.Value.HasPlant * pot.Value.Id;
            }
            return sum;
        }

        private int Count(State state) {
            return state.PotObjects.Values.Select(x => x.HasPlant).Sum();
        }

        private ulong GetNextCount(ulong sum, ulong count, ulong remaining) {
            return remaining * count + sum;
        }

        private void PerformGeneration(State state, ulong count) {
            for (ulong sub = 1; sub <= count; sub++) {
                PerformGeneration(state);
            }
        }

        private ulong FindConverge(State state) {
            ulong count = 0;
            string last = PrintOutput(state);
            do {
                count++;
                PerformGeneration(state);
                Trim(state);
                var next = PrintOutput(state);
                if (next == last) {
                    return count;
                }
                last = next;
            } while (true);
        }

        private void Trim(State state) {
            while (state.PotObjects[state.LowestId].HasPlant == 0) {
                state.PotObjects.Remove(state.LowestId);
                state.LowestId++;
            }
        }

        private List<string> _lines = new List<string>();
        private void Test(State state) {
            PerformGeneration(state);
            _lines.Add(PrintOutput(state));
        }

        private string PrintOutput(State state) {
            int min = state.PotObjects.Keys.Min();
            int max = state.PotObjects.Keys.Max();
            var result = new char[max - min + 1];
            for (int id = min; id <= max; id++) {
                if (state.PotObjects[id].HasPlant == 1) {
                    result[id + (min * -1)] = '#';
                } else {
                    result[id + (min * -1)] = '.';
                }
            }
            return new string(result);
        }

        private void PerformGeneration(State state) {
            int mask = 0;

            // Existing pots
            foreach (var pot in state.PotObjects) {
                int powerOf2 = 1;
                mask = 0;
                for (int id = pot.Key - 2; id <= pot.Key + 2; id++) {
                    if (state.PotObjects.ContainsKey(id)) {
                        mask += powerOf2 * state.PotObjects[id].HasPlant;
                    }
                    powerOf2 *= 2;
                }
                pot.Value.NextHasPlant = state.PotMasks[mask];
            }

            // pots before and after existing
            int lowestId = state.LowestId;
            mask = state.PotObjects[state.LowestId].HasPlant * 8
                + state.PotObjects[state.LowestId + 1].HasPlant * 16;
            if (state.PotMasks[mask] == 1) {
                lowestId--;
                state.PotObjects.Add(lowestId, new Pot() {
                    Id = lowestId,
                    NextHasPlant = 1
                });
            }

            mask = state.PotObjects[state.LowestId].HasPlant * 16;
            if (state.PotMasks[mask] == 1) {
                lowestId--;
                state.PotObjects.Add(lowestId, new Pot() {
                    Id = lowestId,
                    NextHasPlant = 1
                });
            }

            int highestId = state.HighestId;
            mask = state.PotObjects[highestId].HasPlant * 2
                + state.PotObjects[highestId - 1].HasPlant;
            if (state.PotMasks[mask] == 1) {
                highestId++;
                state.PotObjects.Add(highestId, new Pot() {
                    Id = highestId,
                    NextHasPlant = 1
                });
            }

            mask = state.PotObjects[state.HighestId].HasPlant;
            if (state.PotMasks[mask] == 1) {
                highestId++;
                state.PotObjects.Add(highestId, new Pot() {
                    Id = highestId,
                    NextHasPlant = 1
                });
            }

            // Set Pots
            foreach (var pot in state.PotObjects.Values) {
                pot.HasPlant = pot.NextHasPlant;
            }
            state.HighestId = highestId;
            state.LowestId = lowestId;
        }

        private void ReplaceMissingMasks(State state) {
            for (int num = 0; num <= 31; num++) {
                if (!state.PotMasks.ContainsKey(num)) {
                    state.PotMasks.Add(num, 0);
                }
            }
        }

        private State GetState() {
            var input = Input();
            var initial = input[0];
            var potObjects = new List<Pot>();
            var potMasks = new List<PotMask>();
            for (int index = 15; index < initial.Length; index++) {
                potObjects.Add(new Pot() {
                    HasPlant = ((char)initial[index] == '#' ? 1 : 0),
                    Id = index - 15
                });
            }
            for (int index = 2; index < input.Count; index++) {
                var line = input[index];
                int bitMask = (line[0] == '#' ? 1 : 0);
                bitMask += (line[1] == '#' ? 1 : 0) * 2;
                bitMask += (line[2] == '#' ? 1 : 0) * 4;
                bitMask += (line[3] == '#' ? 1 : 0) * 8;
                bitMask += (line[4] == '#' ? 1 : 0) * 16;
                potMasks.Add(new PotMask() {
                    BitMask = bitMask,
                    HasPot = (line[9] == '#' ? 1 : 0)
                });
            }
            return new State() {
                PotMasks = potMasks.ToDictionary(x => x.BitMask, x => x.HasPot),
                PotObjects = potObjects.ToDictionary(x => x.Id, x => x),
                LowestId = potObjects.Select(x => x.Id).Min(),
                HighestId = potObjects.Select(x => x.Id).Max()
            };
        }

        public List<string> TestInput() {
            return new List<string>() {
                "initial state: #..#.#..##......###...###",
                "",
                "...## => #",
                "..#.. => #",
                ".#... => #",
                ".#.#. => #",
                ".#.## => #",
                ".##.. => #",
                ".#### => #",
                "#.#.# => #",
                "#.### => #",
                "##.#. => #",
                "##.## => #",
                "###.. => #",
                "###.# => #",
                "####. => #",
            };
        }

        private class Pot {
            public int Id { get; set; }
            public int HasPlant { get; set; }
            public int NextHasPlant { get; set; }
        }

        private class State {
            public Dictionary<int, int> PotMasks { get; set; }
            public Dictionary<int, Pot> PotObjects { get; set; }
            public int LowestId { get; set; }
            public int HighestId { get; set; }
        }

        private class PotMask {
            public int BitMask { get; set; }
            public int HasPot { get; set; }
        }
    }
}
