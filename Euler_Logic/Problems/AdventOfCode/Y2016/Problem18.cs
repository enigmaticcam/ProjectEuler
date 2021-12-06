using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems.AdventOfCode.Y2016 {
    public class Problem18 : AdventOfCodeBase {
        private HashSet<int> _trapBits;

        public override string ProblemName => "Advent of Code 2016: 18";

        public override string GetAnswer() {
            //return Answer1("..^^.", 2).ToString();
            //return Answer1(".^^.^.^^^^", 9).ToString();
            return Answer1(Input()[0], 400000 - 1).ToString();
        }

        private int Answer1(string input, int rowCount) {
            SetTrapBits();
            var count = GetInitialCount(input);
            return count + CountSafe(input.ToCharArray(), rowCount);
        }

        private int GetInitialCount(string input) {
            int count = 0;
            foreach (var digit in input) {
                count += (digit == '.' ? 1 : 0);
            }
            return count;
        }

        private int CountSafe(char[] tiles, int rowCount) {
            int count = 0;
            int trapBits = 0;
            for (int row = 1; row <= rowCount; row++) {
                var newTiles = new char[tiles.Length];
                for (int index = 0; index < tiles.Length; index++) {
                    trapBits = 0;
                    trapBits += (index < tiles.Length - 1 && tiles[index + 1] == '^' ? 1 : 0);
                    trapBits += (tiles[index] == '^' ? 2 : 0);
                    trapBits += (index > 0 && tiles[index - 1] == '^' ? 4 : 0);
                    newTiles[index] = (_trapBits.Contains(trapBits) ? '^' : '.');
                    count += (newTiles[index] == '.' ? 1 : 0);
                }
                tiles = newTiles;
            }
            return count;
        }

        private void SetTrapBits() {
            _trapBits = new HashSet<int>();
            _trapBits.Add(6);
            _trapBits.Add(3);
            _trapBits.Add(4);
            _trapBits.Add(1);
        }
    }
}
