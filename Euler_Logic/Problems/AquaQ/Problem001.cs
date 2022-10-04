using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems.AquaQ {
    public class Problem001 : AquaQBase {
        public override string ProblemName => "AquaA: ";

        public override string GetAnswer() {
            return Solve(Input()[0]);
        }

        private string Solve(string input) {
            input = Pad(input);
            input = SetToValid(input);
            return GetFinal(input);
        }

        private string GetFinal(string text) {
            var set = new List<string>();
            var length = text.Length / 3;
            set.Add(text.Substring(0, length));
            set.Add(text.Substring(length, length));
            set.Add(text.Substring(length * 2));
            var final = set[0].Substring(length - 2);
            final += set[1].Substring(length - 2);
            final += set[2].Substring(length - 2);
            return final;
        }

        private string SetToValid(string text) {
            var result = text.ToCharArray();
            var valid = GetValidHex();
            for (int index = 0; index < text.Length; index++) {
                if (!valid.Contains(result[index])) result[index] = '0';
            }
            return new string(result);
        }

        private string Pad(string text) {
            if (text.Length % 3 != 0) {
                return text.PadLeft((3 - text.Length % 3) + text.Length, '0');
            } else {
                return text;
            }
        }

        private HashSet<char> GetValidHex() {
            var valid = new HashSet<char>();
            valid.Add('0');
            valid.Add('1');
            valid.Add('2');
            valid.Add('3');
            valid.Add('4');
            valid.Add('5');
            valid.Add('6');
            valid.Add('7');
            valid.Add('8');
            valid.Add('9');
            valid.Add('a');
            valid.Add('b');
            valid.Add('c');
            valid.Add('d');
            valid.Add('e');
            valid.Add('f');
            return valid;
        }
    }
}
