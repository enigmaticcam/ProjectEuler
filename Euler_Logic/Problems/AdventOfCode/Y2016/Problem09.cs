using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems.AdventOfCode.Y2016 {
    public class Problem09 : AdventOfCodeBase {
        public override string ProblemName => "Advent of Code 2016: 9";

        public override string GetAnswer() {
            return Answer2(Input()).ToString();
        }

        private int Answer1(List<string> input) {
            return GetLengthVersion1(input[0]);
        }

        private ulong Answer2(List<string> input) {
            return GetLengthVersion2(input[0]);
        }

        private int GetLengthVersion1(string line) {
            int count = 0;
            int index = 0;
            do {
                if (line[index] != '(') {
                    index++;
                    count++;
                } else {
                    var endPar = line.IndexOf(')', index + 1);
                    var code = line.Substring(index + 1, endPar - (index + 1));
                    var split = code.Split('x');
                    var length = Convert.ToInt32(split[0]);
                    var times = Convert.ToInt32(split[1]);
                    count += length * times;
                    index = endPar + length + 1;
                }
            } while (index < line.Length);
            return count;
        }

        private ulong GetLengthVersion2(string line) {
            ulong count = 0;
            int index = 0;
            do {
                var nextPar = line.IndexOf('(', index);
                if (nextPar == -1) {
                    return count + (ulong)(line.Length - index);
                }
                count += (ulong)(nextPar - index);
                var endPar = line.IndexOf(')', nextPar + 1);
                var code = line.Substring(nextPar + 1, endPar - (nextPar + 1));
                var split = code.Split('x');
                var length = Convert.ToInt32(split[0]);
                var times = Convert.ToUInt64(split[1]);
                count += times * Recursive(line, endPar + 1, length);
                index = endPar + length + 1;
            } while (true);
        }

        private ulong Recursive(string line, int startIndex, int length) {
            int index = startIndex;
            var nextPar = line.IndexOf('(', index);
            if (nextPar == -1 || nextPar >= index + length) {
                return (ulong)length;
            }
            ulong count = 0;
            do {
                count += (ulong)(nextPar - index);
                var endPar = line.IndexOf(')', nextPar + 1);
                var code = line.Substring(nextPar + 1, endPar - (nextPar + 1));
                var split = code.Split('x');
                var subLength = Convert.ToInt32(split[0]);
                var times = Convert.ToUInt64(split[1]);
                count += times * Recursive(line, endPar + 1, subLength);
                index = endPar + subLength + 1;
                if (index >= line.Length) {
                    break;
                }
                nextPar = line.IndexOf('(', index);
            } while (nextPar != -1 && nextPar < startIndex + length);
            return count;
        }
    }
}
