using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem213 : IProblem {
        private List<Point> _fleas = new List<Point>();
        private PossibleMoves[,] _moves;
        private int[,] _fleaCounts;

        public string ProblemName {
            get { return "213: Flea Circus"; }
        }

        public string GetAnswer() {
            int gridSize = 5;
            int bellCount = 1;
            return Solve(gridSize, bellCount).ToString();
        }

        private void Initialize(int gridSize) {
            _moves = new PossibleMoves[gridSize + 1, gridSize + 1];
            _fleaCounts = new int[gridSize + 1, gridSize + 1];
            _fleas = new List<Point>();
            for (int x = 1; x <= gridSize; x++) {
                for (int y = 1; y <= gridSize; y++) {
                    _fleas.Add(new Point(x, y));
                    _fleaCounts[x, y] = 1;
                    if (x == 1 && y == 1) {
                        _moves[x, y] = new PossibleMovesBottomLeft();
                    } else if (x == gridSize && y == 1) {
                        _moves[x, y] = new PossibleMovesBottomRight();
                    } else if (x == 1 && y == gridSize) {
                        _moves[x, y] = new PossibleMovesTopLeft();
                    } else if (x == gridSize && y == gridSize) {
                        _moves[x, y] = new PossibleMovesTopRight();
                    } else if (x == 1) {
                        _moves[x, y] = new PossibleMovesLeft();
                    } else if (x == gridSize) {
                        _moves[x, y] = new PossibleMovesRight();
                    } else if (y == 1) {
                        _moves[x, y] = new PossibleMovesBottom();
                    } else if (y == gridSize) {
                        _moves[x, y] = new PossibleMovesTop();
                    } else {
                        _moves[x, y] = new PossibleMovesMiddle();
                    }
                }
            }
        }

        private double Solve(int gridSize, int bellCount) {
            int total = 100000;
            double sum = 0;
            Random random = new Random();
            for (int count = 1; count <= total; count++) {
                Initialize(gridSize);
                List<string> log = new List<string>();
                for (int ring = 1; ring <= bellCount; ring++) {
                    foreach (Point flea in _fleas) {
                        _moves[flea.X, flea.Y].MakeRandomMove(flea, _fleaCounts, random, log);
                    }
                }
                for (int x = 1; x <= _moves.GetUpperBound(0); x++) {
                    for (int y = 1; y <= _moves.GetUpperBound(0); y++) {
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
            public abstract void MakeRandomMove(Point point, int[,] fleaCounts, Random random, List<string> log);
            public string Output(int[,] fleaCounts) {
                int count = 0;
                StringBuilder output = new StringBuilder();
                for (int y = fleaCounts.GetUpperBound(0); y >= 1; y--) {
                    for (int x = 1; x <= fleaCounts.GetUpperBound(0); x++) {
                        output.Append(fleaCounts[x, y]);
                        count += fleaCounts[x, y];
                    }
                    output.AppendLine("");
                }
                output.AppendLine("Total: " + count);
                return output.ToString();
            }
        }

        private class PossibleMovesMiddle : PossibleMoves {
            public override void MakeRandomMove(Point point, int[,] fleaCounts, Random random, List<string> log) {
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
            public override void MakeRandomMove(Point point, int[,] fleaCounts, Random random, List<string> log) {
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
            public override void MakeRandomMove(Point point, int[,] fleaCounts, Random random, List<string> log) {
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
            public override void MakeRandomMove(Point point, int[,] fleaCounts, Random random, List<string> log) {
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
            public override void MakeRandomMove(Point point, int[,] fleaCounts, Random random, List<string> log) {
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
            public override void MakeRandomMove(Point point, int[,] fleaCounts, Random random, List<string> log) {
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
            public override void MakeRandomMove(Point point, int[,] fleaCounts, Random random, List<string> log) {
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
            public override void MakeRandomMove(Point point, int[,] fleaCounts, Random random, List<string> log) {
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
            public override void MakeRandomMove(Point point, int[,] fleaCounts, Random random, List<string> log) {
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
