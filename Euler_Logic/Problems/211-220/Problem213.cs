using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem213 : IProblem {
        private List<Point> _fleas = new List<Point>();
        private PossibleMoves[,] _moves = new PossibleMoves[31, 31];
        private int[,] _fleaCounts = new int[31, 31];

        public string ProblemName {
            get { return "213: Flea Circus"; }
        }

        public string GetAnswer() {
            Initialize();
            return Go().ToString();
        }

        private void Initialize() {
            for (int x = 1; x <= 30; x++) {
                for (int y = 1; y <= 30; y++) {
                    _fleas.Add(new Point(x, y));
                    _fleaCounts[x, y] = 1;
                    if (x == 1 && y == 1) {
                        _moves[x, y] = new PossibleMovesBottomLeft();
                    } else if (x == 30 && y == 1) {
                        _moves[x, y] = new PossibleMovesBottomRight();
                    } else if (x == 1 && y == 30) {
                        _moves[x, y] = new PossibleMovesTopLeft();
                    } else if (x == 30 && y == 30) {
                        _moves[x, y] = new PossibleMovesTopRight();
                    } else if (x == 1) {
                        _moves[x, y] = new PossibleMovesLeft();
                    } else if (x == 30) {
                        _moves[x, y] = new PossibleMovesRight();
                    } else if (y == 1) {
                        _moves[x, y] = new PossibleMovesBottom();
                    } else if (y == 30) {
                        _moves[x, y] = new PossibleMovesTop();
                    } else {
                        _moves[x, y] = new PossibleMovesMiddle();
                    }
                }
            }
        }

        private double Go() {
            int total = 100;
            double sum = 0;
            for (int count = 1; count <= total; count++) {
                Random random = new Random();
                for (int ring = 1; ring <= 50; ring++) {
                    foreach (Point flea in _fleas) {
                        _moves[flea.X, flea.Y].MakeRandomMove(flea, _fleaCounts, random);
                    }
                }
                for (int x = 1; x <= 30; x++) {
                    for (int y = 1; y <= 30; y++) {
                        if (_fleaCounts[x, y] == 0) {
                            sum += 1;
                        }
                    }
                }
            }
            sum /= total;
            return sum;
        }

        private class Point {
            public int X { get; set; }
            public int Y { get; set; }

            public Point(int x, int y) {
                this.X = x;
                this.Y = y;
            }
        }

        private abstract class PossibleMoves {
            public abstract void MakeRandomMove(Point point, int[,] fleaCounts, Random random);
        }

        private class PossibleMovesMiddle : PossibleMoves {
            public override void MakeRandomMove(Point point, int[,] fleaCounts, Random random) {
                fleaCounts[point.X, point.Y] -= 1;
                int direction = random.Next(1, 5);
                if (direction == 1) {
                    point.Y += 1;
                } else if (direction == 2) {
                    point.X += 1;
                } else if (direction == 3) {
                    point.Y -= 1;
                } else if (direction == 4) {
                    point.X -= 1;
                } else {
                    throw new Exception("no");
                }
                fleaCounts[point.X, point.Y] += 1;
            }
        }

        private class PossibleMovesLeft : PossibleMoves {
            public override void MakeRandomMove(Point point, int[,] fleaCounts, Random random) {
                fleaCounts[point.X, point.Y] -= 1;
                int direction = random.Next(1, 4);
                if (direction == 1) {
                    point.Y += 1;
                } else if (direction == 2) {
                    point.X += 1;
                } else if (direction == 3) {
                    point.Y -= 1;
                } else {
                    throw new Exception("no");
                }
                fleaCounts[point.X, point.Y] += 1;
            }
        }

        private class PossibleMovesRight : PossibleMoves {
            public override void MakeRandomMove(Point point, int[,] fleaCounts, Random random) {
                fleaCounts[point.X, point.Y] -= 1;
                int direction = random.Next(1, 4);
                if (direction == 1) {
                    point.Y += 1;
                } else if (direction == 2) {
                    point.X -= 1;
                } else if (direction == 3) {
                    point.Y -= 1;
                } else {
                    throw new Exception("no");
                }
                fleaCounts[point.X, point.Y] += 1;
            }
        }

        private class PossibleMovesTop : PossibleMoves {
            public override void MakeRandomMove(Point point, int[,] fleaCounts, Random random) {
                fleaCounts[point.X, point.Y] -= 1;
                int direction = random.Next(1, 4);
                if (direction == 1) {
                    point.X += 1;
                } else if (direction == 2) {
                    point.Y -= 1;
                } else if (direction == 3) {
                    point.X -= 1;
                } else {
                    throw new Exception("no");
                }
                fleaCounts[point.X, point.Y] += 1;
            }
        }

        private class PossibleMovesBottom : PossibleMoves {
            public override void MakeRandomMove(Point point, int[,] fleaCounts, Random random) {
                fleaCounts[point.X, point.Y] -= 1;
                int direction = random.Next(1, 4);
                if (direction == 1) {
                    point.X += 1;
                } else if (direction == 2) {
                    point.Y += 1;
                } else if (direction == 3) {
                    point.X -= 1;
                } else {
                    throw new Exception("no");
                }
                fleaCounts[point.X, point.Y] += 1;
            }
        }

        private class PossibleMovesTopLeft : PossibleMoves {
            public override void MakeRandomMove(Point point, int[,] fleaCounts, Random random) {
                fleaCounts[point.X, point.Y] -= 1;
                int direction = random.Next(1, 3);
                if (direction == 1) {
                    point.X += 1;
                } else if (direction == 2) {
                    point.Y -= 1;
                } else {
                    throw new Exception("no");
                }
                fleaCounts[point.X, point.Y] += 1;
            }
        }

        private class PossibleMovesTopRight : PossibleMoves {
            public override void MakeRandomMove(Point point, int[,] fleaCounts, Random random) {
                fleaCounts[point.X, point.Y] -= 1;
                int direction = random.Next(1, 3);
                if (direction == 1) {
                    point.X -= 1;
                } else if (direction == 2) {
                    point.Y -= 1;
                } else {
                    throw new Exception("no");
                }
                fleaCounts[point.X, point.Y] += 1;
            }
        }

        private class PossibleMovesBottomLeft : PossibleMoves {
            public override void MakeRandomMove(Point point, int[,] fleaCounts, Random random) {
                fleaCounts[point.X, point.Y] -= 1;
                int direction = random.Next(1, 3);
                if (direction == 1) {
                    point.X += 1;
                } else if (direction == 2) {
                    point.Y += 1;
                } else {
                    throw new Exception("no");
                }
                fleaCounts[point.X, point.Y] += 1;
            }
        }

        private class PossibleMovesBottomRight : PossibleMoves {
            public override void MakeRandomMove(Point point, int[,] fleaCounts, Random random) {
                fleaCounts[point.X, point.Y] -= 1;
                int direction = random.Next(1, 3);
                if (direction == 1) {
                    point.X -= 1;
                } else if (direction == 2) {
                    point.Y += 1;
                } else {
                    throw new Exception("no");
                }
                fleaCounts[point.X, point.Y] += 1;
            }
        }
    }
}
