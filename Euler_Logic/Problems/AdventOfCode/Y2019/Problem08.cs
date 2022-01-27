using System.Collections.Generic;
using System.Text;

namespace Euler_Logic.Problems.AdventOfCode.Y2019 {
    public class Problem08 : AdventOfCodeBase {
        public override string ProblemName {
            get { return "Advent of Code 2019: 8"; }
        }

        public override string GetAnswer() {
            return Answer1().ToString();
        }

        public override string GetAnswer2() {
            return Answer2().ToString();
        }

        public int Answer1() {
            var layers = GetLayers(25, 6, Input()[0]);
            var counts = GetCounts(layers);
            var fewestZero = FindFewestZero(counts);
            return fewestZero.Counts['1'] * fewestZero.Counts['2'];
        }

        public string Answer2() {
            int width = 25;
            int height = 6;
            var layers = GetLayers(width, height, Input()[0]);
            var image = GetImage(layers, width, height);
            //OutputImage(image);
            return "PCULA";
        }

        private string OutputImage(enumColor[,] image) {
            var text = new StringBuilder();
            for (int x = 0; x <= image.GetUpperBound(1); x++) {
                var line = new char[image.GetUpperBound(0) + 1];
                for (int y = 0; y <= image.GetUpperBound(0); y++) {
                    var pixel = image[y, x];
                    if (pixel == enumColor.Black) {
                        line[y] = ' ';
                    } else {
                        line[y] = '#';
                    }
                }
                text.AppendLine(new string(line));
            }
            return text.ToString();
        }

        private enumColor[,] GetImage(List<string> layers, int length, int height) {
            var image = new enumColor[length, height];
            foreach (var layer in layers) {
                int x = 1;
                int y = 1;
                foreach (var pixel in layer) {
                    if (pixel != '2' && image[x - 1, y - 1] == enumColor.Transparent) {
                        if (pixel == '0') {
                            image[x - 1, y - 1] = enumColor.Black;
                        } else {
                            image[x - 1, y - 1] = enumColor.White;
                        }
                    }
                    x++;
                    if (x > length) {
                        x = 1;
                        y++;
                    }
                }
            }
            return image;
        }

        private LayerCounts FindFewestZero(List<LayerCounts> counts) {
            LayerCounts lowest = null;
            foreach (var count in counts) {
                if (lowest == null) {
                    lowest = count;
                } else if (lowest.Counts['0'] > count.Counts['0']) {
                    lowest = count;
                }
            }
            return lowest;
        }

        private List<LayerCounts> GetCounts(List<string> layers) {
            var counts = new List<LayerCounts>();
            foreach (var layer in layers) {
                var count = new LayerCounts() {
                    Counts = GetDigitCounts(layer),
                    Layer = layer
                };
                counts.Add(count);
            }
            return counts;
        }

        private Dictionary<char, int> GetDigitCounts(string layer) {
            var counts = new Dictionary<char, int>();
            foreach (char pixel in layer) {
                if (!counts.ContainsKey(pixel)) {
                    counts.Add(pixel, 1);
                } else {
                    counts[pixel]++;
                }
            }
            return counts;
        }

        private List<string> GetLayers(int length, int height, string input) {
            var layers = new List<string>();
            int x = 1;
            int y = 1;
            var current = new char[length * height];
            int index = 0;
            foreach (char pixel in input) {
                current[index] = pixel;
                index++;
                x++;
                if (x > length) {
                    x = 1;
                    y++;
                    if (y > height) {
                        y = 1;
                        index = 0;
                        layers.Add(new string(current));
                        current = new char[length * height];
                    }
                }
            }
            return layers;
        }

        private enum enumColor {
            Transparent = 0,
            Black,
            White
        }

        private class LayerCounts {
            public Dictionary<char, int> Counts { get; set; }
            public string Layer { get; set; }
        }
    }
}
