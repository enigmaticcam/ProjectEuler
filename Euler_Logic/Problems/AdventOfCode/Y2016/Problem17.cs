using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Euler_Logic.Problems.AdventOfCode.Y2016 {
    public class Problem17 : AdventOfCodeBase {
        private HashSet<char> _open;
        private string _bestPath = "";
        private int _bestPathLength = int.MaxValue;

        public override string ProblemName => "Advent of Code 2016: 17";

        public override string GetAnswer() {
            return Answer1("pslxynzg").ToString();
        }

        public override string GetAnswer2() {
            return Answer2("pslxynzg").ToString();
        }

        private string Answer1(string input) {
            SetOpen();
            using (var md5 = MD5.Create()) {
                FindShortest(input, md5, 0, 0);
            }
            return _bestPath.Substring(input.Length);
        }

        private int Answer2(string input) {
            _bestPathLength = 0;
            SetOpen();
            using (var md5 = MD5.Create()) {
                FindLongest(input, md5, 0, 0);
            }
            return _bestPath.Length - input.Length;
        }

        private void SetOpen() {
            _open = new HashSet<char>();
            _open.Add('B');
            _open.Add('C');
            _open.Add('D');
            _open.Add('E');
            _open.Add('F');
        }

        private void FindLongest(string path, MD5 md5, int x, int y) {
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(path);
            byte[] hashBytes = md5.ComputeHash(inputBytes);
            var roomSet1 = hashBytes[0].ToString("X2");
            var roomSet2 = hashBytes[1].ToString("X2");

            // down
            if (y < 3 && _open.Contains(roomSet1[1])) {
                if (x == 3 && y + 1 == 3) {
                    if (path.Length > _bestPath.Length) {
                        _bestPath = path + "D";
                    }
                } else {
                    FindLongest(path + "D", md5, x, y + 1);
                }
            }

            // right
            if (x < 3 && _open.Contains(roomSet2[1])) {
                if (x + 1 == 3 && y == 3) {
                    if (path.Length > _bestPath.Length) {
                        _bestPath = path + "R";
                    }
                } else {
                    FindLongest(path + "R", md5, x + 1, y);
                }
            }

            // left
            if (x > 0 && _open.Contains(roomSet2[0])) {
                FindLongest(path + "L", md5, x - 1, y);
            }

            // up
            if (y > 0 && _open.Contains(roomSet1[0])) {
                FindLongest(path + "U", md5, x, y - 1);
            }
        }

        private void FindShortest(string path, MD5 md5, int x, int y) {
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(path);
            byte[] hashBytes = md5.ComputeHash(inputBytes);
            var roomSet1 = hashBytes[0].ToString("X2");
            var roomSet2 = hashBytes[1].ToString("X2");

            // down
            if (_bestPathLength > path.Length && y < 3 && _open.Contains(roomSet1[1])) {
                if (x == 3 && y + 1 == 3) {
                    _bestPath = path + "D";
                    _bestPathLength = path.Length + 1;
                } else {
                    FindShortest(path + "D", md5, x, y + 1);
                }
            }

            // right
            if (_bestPathLength > path.Length && x < 3 && _open.Contains(roomSet2[1])) {
                if (x + 1 == 3 && y == 3) {
                    _bestPath = path + "R";
                    _bestPathLength = path.Length + 1;
                } else {
                    FindShortest(path + "R", md5, x + 1, y);
                }
            }

            // left
            if (_bestPathLength > path.Length && x > 0 && _open.Contains(roomSet2[0])) {
                FindShortest(path + "L", md5, x - 1, y);
            }

            // up
            if (_bestPathLength > path.Length && y > 0 && _open.Contains(roomSet1[0])) {
                FindShortest(path + "U", md5, x, y - 1);
            }
        }
    }
}
