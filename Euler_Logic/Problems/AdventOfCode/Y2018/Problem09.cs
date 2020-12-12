using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems.AdventOfCode.Y2018 {
    public class Problem09 : AdventOfCodeBase {
        public override string ProblemName {
            get { return "Advent of Code 2018: 9"; }
        }

        public override string GetAnswer() {
            return Answer2();
        }

        private string Answer1() {
            PerformGame(459, 71319);
            return _scores.Values.Max().ToString();
        }

        private string Answer2() {
            PerformGame(459, 7131900);
            return _scores.Values.Max().ToString();
        }

        private Dictionary<ulong, ulong> _scores;
        private void PerformGame(ulong playerCount, ulong marbleCount) {
            InitializeScores(playerCount);
            var current = new Marble();
            current.Num = 0;
            current.Next = current;
            current.Prior = current;
            ulong player = 0;
            for (ulong num = 1; num < marbleCount; num++) {
                current = AddMarble(current, num, player);
                player = (player + 1) % playerCount;
            }
        }

        private Marble AddMarble(Marble current, ulong num, ulong player) {
            if (num % 23 == 0) {
                _scores[player] += num;
                for (ulong count = 1; count <= 7; count++) {
                    current = current.Prior;
                }
                _scores[player] += current.Num;
                current.Prior.Next = current.Next;
                current.Next.Prior = current.Prior;
                current = current.Next;
            } else {
                current = current.Next;
                var marble = new Marble();
                marble.Num = num;
                marble.Next = current.Next;
                marble.Prior = current;
                current.Next = marble;
                marble.Next.Prior = marble;
                current = marble;
            }
            return current;
        }

        private string PrulongChain(Marble marble) {
            string text = "";
            ulong current = marble.Num;
            do {
                text += marble.Num + ";";
                marble = marble.Next;
            } while (marble.Num != current);
            return text;
        }

        private void InitializeScores(ulong playerCount) {
            _scores = new Dictionary<ulong, ulong>();
            Enumerable.Range(1, (int)playerCount).ToList().ForEach(x => _scores.Add((ulong)x - 1, 0));
        }

        private class Marble {
            public ulong Num { get; set; }
            public Marble Next { get; set; }
            public Marble Prior { get; set; }
        }
    }
}
