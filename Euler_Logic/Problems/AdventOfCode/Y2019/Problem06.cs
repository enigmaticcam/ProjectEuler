using System.Collections.Generic;
using System.Linq;

namespace Euler_Logic.Problems.AdventOfCode.Y2019 {
    public class Problem06 : AdventOfCodeBase {
        public override string ProblemName {
            get { return "Advent of Code 2019: 6"; }
        }

        public override string GetAnswer() {
            _counts = new Dictionary<string, int>();
            return Answer1().ToString();
        }

        public override string GetAnswer2() {
            _counts = new Dictionary<string, int>();
            return Answer2().ToString();
        }

        public int Answer1() {
            var orbits = GetOrbits();
            int count = 0;
            orbits.ForEach(x => count += CountOrbits(x));
            return count;
        }

        public int Answer2() {
            var orbits = GetOrbits();
            var orbitFrom = orbits.Where(x => x.Name == "YOU").First().PrimeOrbit;
            var orbitTo = orbits.Where(x => x.Name == "SAN").First();
            return FindPath(orbitFrom, orbitTo);
        }

        private int FindPath(Orbit orbitFrom, Orbit orbitTo) {
            var current = new List<Orbit> { orbitFrom };
            var visited = new HashSet<string>() { orbitFrom.Name };
            int count = 0;
            do {
                var temp = new List<Orbit>();
                foreach (var next in current) {
                    if (next == orbitTo) {
                        return count - 1;
                    }
                    visited.Add(next.Name);
                    if (next.PrimeOrbit != null && !visited.Contains(next.PrimeOrbit.Name)) {
                        temp.Add(next.PrimeOrbit);
                    }
                    foreach (var child in next.ChildOrbits) {
                        if (!visited.Contains(child.Name)) {
                            temp.Add(child);
                        }
                    }
                }
                current = temp;
                count++;
            } while (true);
        }

        private Dictionary<string, int> _counts;
        private int CountOrbits(Orbit orbit) {
            if (!_counts.ContainsKey(orbit.Name)) {
                int count = orbit.ChildOrbits.Count;
                orbit.ChildOrbits.ForEach(x => count += CountOrbits(x));
                _counts.Add(orbit.Name, count);
            }
            return _counts[orbit.Name];
        }

        private List<Orbit> GetOrbits() {
            var input = Input();
            var orbits = new Dictionary<string, Orbit>();
            foreach (var orbit in input) {
                Orbit parent = null;
                Orbit child = null;
                var split = orbit.Split(')');
                if (!orbits.ContainsKey(split[1])) {
                    child = new Orbit() { Name = split[1] };
                    orbits.Add(child.Name, child);
                } else {
                    child = orbits[split[1]];
                }
                if (!orbits.ContainsKey(split[0])) {
                    parent = new Orbit() { Name = split[0] };
                    orbits.Add(parent.Name, parent);
                } else {
                    parent = orbits[split[0]];
                }
                parent.ChildOrbits.Add(child);
                child.PrimeOrbit = parent;
            }
            return orbits.Values.ToList();
        }

        private class Orbit {
            public Orbit() {
                ChildOrbits = new List<Orbit>();
            }

            public string Name { get; set; }
            public List<Orbit> ChildOrbits { get; set; }
            public Orbit PrimeOrbit { get; set; }
        }
    }
}
