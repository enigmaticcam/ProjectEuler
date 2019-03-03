using System;

namespace Euler_Logic.Problems {
    public class Problem277 : ProblemBase {

        /*
            Starting with number 2, continue to check each number with an offset of 1 until you can
            match the first sequence digit. Once you do that, multiply your offset by 3 until you
            find the next sequence digit. Then multiply by 3 again, and continue to do this until
            you reach a number that exceeds 10^15.
         */

        public override string ProblemName {
            get { return "277: A Modified Collatz sequence"; }
        }

        public override string GetAnswer() {
            string sequence = "UDDDUdddDDUDDddDdDddDDUDDdUUDd";
            ulong smallest = (ulong)Math.Pow(10, 15);
            return Solve(ConvertSequence(sequence), smallest).ToString();
        }

        private ulong Solve(ulong[] sequence, ulong smallest) {
            ulong num = 2;
            do {
                ulong remainder = num;
                int sIndex = 0;
                int highestIndex = 0;
                ulong offset = 1;
                do {
                    var next = remainder % 3;
                    if (next == sequence[sIndex]) {
                        if (next == 0) {
                            remainder /= 3;
                        } else if (next == 1) {
                            remainder = (4 * remainder + 2) / 3;
                        } else {
                            remainder = (2 * remainder - 1) / 3;
                        }
                    } else {
                        break;
                    }
                    sIndex++;
                    if (sIndex == sequence.Length) {
                        if (num >= smallest) {
                            return num;
                        } else {
                            break;
                        }
                    } else if (sIndex > highestIndex) {
                        highestIndex = sIndex;
                        offset *= 3;
                    }
                } while (remainder != 1);
                num += offset;
            } while (true);
        }

        private ulong[] ConvertSequence(string sequence) {
            ulong[] result = new ulong[sequence.Length];
            for (int index = 0; index < sequence.Length; index++) {
                var text = sequence.Substring(index, 1);
                switch (text) {
                    case "D":
                        result[index] = 0;
                        break;
                    case "U":
                        result[index] = 1;
                        break;
                    case "d":
                        result[index] = 2;
                        break;
                }
            }
            return result;
        }
    }
}