using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems.AdventOfCode.Y2017 {
    public class Problem13 : AdventOfCodeBase {
        private Dictionary<int, Layer> _layers;

        public override string ProblemName {
            get { return "Advent of Code 2017: 13"; }
        }

        public override string GetAnswer() {
            return Answer1(Input()).ToString();
        }

        private int Answer1(List<string> input) {
            GetLayers(input);
            return Simulate();
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
                for (int next = index; next <= max; next++) {
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
            return severity;
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

        private List<string> TestInput() {
            return new List<string>() {
                "0: 3",
                "1: 2",
                "4: 4",
                "6: 4"
            };
        }

        private class Layer {
            public int Number { get; set; }
            public int Position { get; set; }
            public int Length { get; set; }
            public int Direction { get; set; }
        }
    }
}

