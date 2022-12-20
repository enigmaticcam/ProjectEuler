using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems.AdventOfCode.Y2022 {
    public class Problem15 : AdventOfCodeBase {
        public override string ProblemName => "Advent of Code 2022: 15";

        public override string GetAnswer() {
            return Answer1(Input(), 2000000).ToString();
        }

        public override string GetAnswer2() {
            return Answer2(Input(), 4000000).ToString();
        }

        private int Answer1(List<string> input, int y) {
            var state = new State();
            SetSensors(input, state);
            SetMinMax(state);
            return CountRow(y, state);
        }

        private ulong Answer2(List<string> input, int max) {
            var state = new State();
            SetSensors(input, state);
            for (int y = 0; y <= max; y++) {
                var count = CountRow2(y, state);
                if (count != -1) {
                    return (ulong)count * (ulong)4000000 + (ulong)y;
                }
            }
            return 0;
        }

        private int CountRow2(int y, State state) {
            var lines = new List<Line>();
            foreach (var sensor in state.Sensors) {
                var diff = Math.Abs(y - sensor.Location.Y);
                if (diff <= sensor.Distance) {
                    lines.Add(new Line() {
                        Start = sensor.Location.X - (sensor.Distance - diff),
                        End = sensor.Location.X + (sensor.Distance - diff)
                    });
                }
            }
            Merge(lines);
            Merge(lines);
            if (lines.Count == 1) {
                return -1;
            } else {
                if (lines[0].Start < lines[1].Start) {
                    return lines[0].End + 1;
                } else {
                    return lines[1].End + 1;
                }
            }
        }

        private void Merge(List<Line> lines) {
            int index = 0;
            do {
                var line = lines[index];
                for (int nextIndex = index + 1; nextIndex < lines.Count; nextIndex++) {
                    if (nextIndex != index) {
                        var nextLine = lines[nextIndex];
                        if (line.Start <= nextLine.End && line.End >= nextLine.Start) {
                            line.Start = Math.Min(line.Start, nextLine.Start);
                            line.End = Math.Max(line.End, nextLine.End);
                            lines.RemoveAt(nextIndex);
                            nextIndex--;
                        }
                    }
                }
                index++;
            } while (index < lines.Count);
        }

        private int CountRow(int y, State state) {
            var hash = new HashSet<int>();
            foreach (var sensor in state.Sensors) {
                var diff = Math.Abs(y - sensor.Location.Y);
                if (diff <= sensor.Distance) {
                    hash.Add(sensor.Location.X);
                    int count = 1;
                    while (diff + count <= sensor.Distance) {
                        hash.Add(sensor.Location.X - count);
                        hash.Add(sensor.Location.X + count);
                        count++;
                    }
                }
            }
            foreach (var sensor in state.Sensors) {
                if (sensor.Location.Y == y && hash.Contains(sensor.Location.X)) hash.Remove(sensor.Location.X);
                if (sensor.Beacon.Y == y && hash.Contains(sensor.Beacon.X)) hash.Remove(sensor.Beacon.X);
            }
            return hash.Count;
        }

        private void SetMinMax(State state) {
            var sensorMin = new Point() {
                X = state.Sensors.Select(x => x.Location.X).Min(),
                Y = state.Sensors.Select(x => x.Location.Y).Min()
            };
            var sensorMax = new Point() {
                X = state.Sensors.Select(x => x.Location.X).Max(),
                Y = state.Sensors.Select(x => x.Location.Y).Max()
            };
            var beaconMin = new Point() {
                X = state.Sensors.Select(x => x.Beacon.X).Min(),
                Y = state.Sensors.Select(x => x.Beacon.Y).Min()
            };
            var beaconMax = new Point() {
                X = state.Sensors.Select(x => x.Beacon.X).Max(),
                Y = state.Sensors.Select(x => x.Beacon.Y).Max()
            };
        }

        private void SetSensors(List<string> input, State state) {
            state.Sensors = input.Select(line => {
                var split = line.Split(' ');
                var sensor = new Sensor() { Beacon = new Point(), Location = new Point() };
                sensor.Location.X = Convert.ToInt32(split[2].Substring(2).Replace(",",""));
                sensor.Location.Y = Convert.ToInt32(split[3].Substring(2).Replace(":", ""));
                sensor.Beacon.X = Convert.ToInt32(split[8].Substring(2).Replace(",", ""));
                sensor.Beacon.Y = Convert.ToInt32(split[9].Substring(2));
                sensor.Distance = GetDistance(sensor.Location, sensor.Beacon);
                return sensor;
            }).ToList();
        }

        private int GetDistance(Point point1, Point point2) {
            return Math.Abs(point1.X - point2.X) + Math.Abs(point1.Y - point2.Y);
        }

        private bool IsSame(Point point1, Point point2) {
            return point1.X == point2.X && point1.Y == point2.Y;
        }

        private class State {
            public List<Sensor> Sensors { get; set; }
        }

        private class Sensor {
            public Point Location { get; set; }
            public Point Beacon { get; set; }
            public int Distance { get; set; }
        }

        private class Point {
            public int X { get; set; }
            public int Y { get; set; }
        }

        private class Line {
            public int Start { get; set; }
            public int End { get; set; }
        }
    }
}
