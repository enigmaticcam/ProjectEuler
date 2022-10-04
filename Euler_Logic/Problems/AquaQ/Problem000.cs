using System;
using System.Collections.Generic;

namespace Euler_Logic.Problems.AquaQ {
    public class Problem000 : AquaQBase {
        public override string ProblemName => "AquaQ: What's a numpad?";

        public override string GetAnswer() {
            return Solve(Input());
        }

        private string Solve(List<string> input) {
            var presses = GetPresses(input);
            var pad = GetNumPad();
            var result = new List<char>();
            foreach (var press in presses) {
                result.Add(pad[press.Item1][press.Item2 - 1]);
            }
            return new string(result.ToArray());
        }

        private List<Tuple<int, int>> GetPresses(List<string> input) {
            var presses = new List<Tuple<int, int>>();
            foreach (var line in input) {
                var split = line.Split(' ');
                presses.Add(new Tuple<int, int>(Convert.ToInt32(split[0]), Convert.ToInt32(split[1])));
            }
            return presses;
        }

        private List<char[]> GetNumPad() {
            var pad = new List<char[]>();
            pad.Add(new char[1] { ' ' });
            pad.Add(new char[0]);
            pad.Add(new char[3] { 'a', 'b', 'c' });
            pad.Add(new char[3] { 'd', 'e', 'f' });
            pad.Add(new char[3] { 'g', 'h', 'i' });
            pad.Add(new char[3] { 'j', 'k', 'l' });
            pad.Add(new char[3] { 'm', 'n', 'o' });
            pad.Add(new char[4] { 'p', 'q', 'r', 's' });
            pad.Add(new char[3] { 't', 'u', 'v' });
            pad.Add(new char[4] { 'w', 'x', 'y', 'z' });
            return pad;
        }
    }
}
