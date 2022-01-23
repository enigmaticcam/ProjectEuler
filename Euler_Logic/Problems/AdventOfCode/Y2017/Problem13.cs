using System;
using System.Collections.Generic;
using System.Linq;

namespace Euler_Logic.Problems.AdventOfCode.Y2017 {
    public class Problem13 : AdventOfCodeBase {
        private Dictionary<int, Layer> _layers;
        private List<Offset> _offsets;

        public override string ProblemName {
            get { return "Advent of Code 2017: 13"; }
        }

        public override string GetAnswer() {
            return Answer1(Input()).ToString();
        }

        public override string GetAnswer2() {
            return Answer2(Input()).ToString();
        }

        private int Answer1(List<string> input) {
            GetLayers(input);
            return Simulate();
        }

        private int Answer2(List<string> input) {
            GetLayers(input);
            SetOffsets();
            return FindLowest();
        }

        private int FindLowest() {
            int delay = 1;
            bool isGood;
            do {
                isGood = true;
                foreach (var offset in _offsets) {
                    if ((delay - offset.Start) % offset.Cycle == 0) {
                        isGood = false;
                        break;
                    }
                }
                if (isGood) return delay;
                delay++;
            } while (true);
        }

        private void SetOffsets() {
            _offsets = _layers.Values.Select(x => new Offset() {
                Cycle = (x.Length == 2 ? 2 : (x.Length - 2) * 2 + 2),
                Position = x.Number
            }).ToList();
            _offsets.ForEach(x => x.Start = x.Cycle - x.Position);
            _offsets.Where(x => x.Position == 0).First().Start = 0;
        }

        private int Simulate() {
            int severity = 0;
            var max = _layers.Keys.Max();
            for (int index = 0; index <= max; index++) {
                if (_layers.ContainsKey(index)) {
                    var layer = _layers[index];
                    if (layer.Position == 1) {
                        severity += layer.Number * layer.Length;
                    }
                }
                SimulateNext(index, max);
            }
            return severity;
        }

        private void SimulateNext(int startIndex, int max) {
            for (int next = startIndex; next <= max; next++) {
                if (_layers.ContainsKey(next)) {
                    var layer = _layers[next];
                    if (layer.Position == 1) {
                        layer.Direction = 1;
                    } else if (layer.Position == layer.Length) {
                        layer.Direction = -1;
                    }
                    layer.Position += layer.Direction;
                }
            }
        }

        private void GetLayers(List<string> input) {
            _layers = input.Select(line => {
                var split = line.Split(':');
                var layer = new Layer();
                layer.Number = Convert.ToInt32(split[0]);
                layer.Length = Convert.ToInt32(split[1].Trim());
                layer.Direction = 1;
                layer.Position = 1;
                return layer;
            }).ToDictionary(x => x.Number, x => x);
        }

        private class Offset {
            public int Start { get; set; }
            public int Cycle { get; set; }
            public int Position { get; set; }
        }

        private class Layer {
            public int Number { get; set; }
            public int Position { get; set; }
            public int Length { get; set; }
            public int Direction { get; set; }
        }
    }
}