using Euler_Logic.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Euler_Logic.Problems.AdventOfCode.Y2022 {
    public class Problem16 : AdventOfCodeBase {
        public override string ProblemName => "Advent of Code 2022: 16";

        public override string GetAnswer() {
            return Answer1(Input()).ToString();
        }

        public override string GetAnswer2() {
            return Answer2(Input()).ToString();
        }

        private int Answer1(List<string> input) {
            var state = new State() { 
                WhereYouAre = "AA",
                MinutesLeft = 30,
                Key = new LinkedList<string>(),
                Hash = new Dictionary<string, HashSave>()
            };
            SetValves(input, state);
            SetDistances(state);
            Recursive(state);
            return state.Best;
        }

        private int Answer2(List<string> input) {
            var state = new State() {
                WhereYouAre = "AA",
                MinutesLeft = 26,
                Key = new LinkedList<string>(),
                Hash = new Dictionary<string, HashSave>()
            };
            SetValves(input, state);
            SetDistances(state);
            Recursive(state);
            return FindBest(state);
        }

        private int FindBest(State state) {
            int best = 0;
            var values = state.Hash.ToList();
            for (int index1 = 0; index1 < values.Count; index1++) {
                var hash1 = values[index1];
                for (int index2 = index1 + 1; index2 < values.Count; index2++) {
                    var hash2 = values[index2];
                    if ((hash1.Value.Bits & hash2.Value.Bits) == 0) {
                        var next = hash1.Value.Released + hash2.Value.Released;
                        if (next > best) best = next;
                    }
                }
            }
            return best;
        }

        private void Recursive(State state) {
            if (state.AllReleased == state.ReleasedBits || state.MinutesLeft == 0) {
                Recursive_CheckFinal(state);
            } else {
                var current = state.Valves[state.WhereYouAre];
                foreach (var next in state.DistanceOrder) {
                    if (current != next && !next.IsReleased) {
                        var distance = current.Distances[next.Name];
                        if (distance < state.MinutesLeft - 1) {
                            Recursive_Next(state, next, distance);
                        }
                    }
                }
                Recursive_CheckFinal(state);
            }
        }

        private void Recursive_Next(State state, Valve valve, int distance) {
            state.MinutesLeft -= distance + 1;
            state.Released += state.Flow * (distance + 1);
            state.Flow += valve.Flow;
            state.ReleasedBits += valve.Bit;
            state.UnreleasedFlow -= valve.Flow;
            var prior = state.WhereYouAre;
            state.WhereYouAre = valve.Name;
            valve.IsReleased = true;
            state.Key.AddLast(valve.Name);
            SaveToHash(state);
            Recursive(state);
            state.Key.RemoveLast();
            valve.IsReleased = false;
            state.WhereYouAre = prior;
            state.UnreleasedFlow += valve.Flow;
            state.ReleasedBits -= valve.Bit;
            state.Flow -= valve.Flow;
            state.Released -= state.Flow * (distance + 1);
            state.MinutesLeft += distance + 1;
        }

        private void SaveToHash(State state) {
            var final = state.Released + (state.Flow * state.MinutesLeft);
            var key = string.Join("", state.Key);
            if (!state.Hash.ContainsKey(key)) {
                state.Hash.Add(key, new HashSave() {
                    Bits = state.ReleasedBits,
                    Released = final
                });
            }
        }

        private void Recursive_CheckFinal(State state) {
            var final = state.Released + (state.Flow * state.MinutesLeft);
            if (final > state.Best) {
                state.Best = final;
            }
        }

        private void SetDistances(State state) {
            var valves = state.Valves.Values.ToList();
            for (int index1 = 0; index1 < valves.Count; index1++) {
                var valve1 = valves[index1];
                for (int index2 = index1 + 1; index2 < valves.Count; index2++) {
                    var valve2 = valves[index2];
                    SetDistance(state, valve1, valve2);
                }
            }
            state.DistanceOrder = state.Valves
                .Where(x => x.Value.Flow != 0)
                .OrderByDescending(x => x.Value.Flow)
                .Select(x => x.Value).ToList();
        }

        private void SetDistance(State state, Valve start, Valve end) {
            var heap = new BinaryHeap_Min(state.Valves.Count);
            foreach (var valve in state.Valves.Values) {
                if (valve == start) {
                    valve.Num = 0;
                } else {
                    valve.Num = int.MaxValue;
                }
                heap.Add(valve);
            }
            heap.Reset();
            do {
                var current = (Valve)heap.Top;
                if (current == end) {
                    start.Distances.Add(end.Name, current.Num);
                    end.Distances.Add(start.Name, current.Num);
                    break;
                }
                foreach (var nextKey in current.Valves) {
                    var nextValve = state.Valves[nextKey];
                    if (nextValve.Num > current.Num + 1) {
                        nextValve.Num = current.Num + 1;
                        heap.Adjust(nextValve);
                    }
                }
                heap.Remove(current);
            } while (true);
        }

        private void SetValves(List<string> input, State state) {
            ulong bit = 1;
            state.Valves = input.Select(line => {
                var valve = new Valve() { Valves = new List<string>(), Distances = new Dictionary<string, int>() };
                var split = line.Split(' ');
                valve.Name = split[1];
                valve.Flow = Convert.ToInt32(split[4].Substring(5).Replace(";", ""));
                int index = 9;
                while (index < split.Length) {
                    var name = split[index].Replace(",", "");
                    valve.Valves.Add(name);
                    index++;
                }
                valve.Bit = bit;
                bit *= 2;
                state.UnreleasedFlow += valve.Flow;
                if (valve.Flow > 0) state.AllReleased += valve.Bit;
                return valve;
            }).ToDictionary(x => x.Name, x => x);
        }

        private class State {
            public Dictionary<string, Valve> Valves { get; set; }
            public List<Valve> DistanceOrder { get; set; }
            public int Released { get; set; }
            public int Flow { get; set; }
            public string WhereYouAre { get; set; }
            public string WhereElephantIs { get; set; }
            public int MinutesLeft { get; set; }
            public ulong ReleasedBits { get; set; }
            public ulong AllReleased { get; set; }
            public int Best { get; set; }
            public int UnreleasedFlow { get; set; }
            public Dictionary<string, HashSave> Hash { get; set; }
            public LinkedList<string> Key { get; set; }
        }

        private class Valve : BinaryHeap_Min.Node {
            public Dictionary<string, int> Distances { get; set; }
            public string Name { get; set; }
            public int Flow { get; set; }
            public List<string> Valves { get; set; }
            public ulong Bit { get; set; }
            public bool IsReleased { get; set; }
        }

        private class HashSave {
            public ulong Bits { get; set; }
            public int Released { get; set; }
        }
    }
}
