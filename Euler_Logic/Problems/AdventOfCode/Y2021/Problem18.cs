using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Euler_Logic.Problems.AdventOfCode.Y2021 {
    public class Problem18 : AdventOfCodeBase {
        public override string ProblemName {
            get { return "Advent of Code 2021: 18"; }
        }

        public override string GetAnswer() {
            return Answer2(Input()).ToString();
        }

        private int Answer1(List<string> input) {
            var pair = GetSinglePair(input[0]);
            for (int index = 1; index < input.Count; index++) {
                var next = GetSinglePair(input[index]);
                var parent = GetNewPair();
                parent.NumberLeft = pair;
                parent.NumberRight = next;
                pair.Parent = parent;
                next.Parent = parent;
                pair = parent;
                var left = GetLowestLeft(pair.NumberRight);
                var right = GetLowestRight(pair.NumberLeft);
                left.ClosestNumberLeft = right;
                right.ClosestNumberRight = left;
                ReduceAddition(pair);
            }
            return GetMagnitude(pair);
        }

        private int Answer2(List<string> input) {
            int best = 0;
            for (int index1 = 0; index1 < input.Count; index1++) {
                for (int index2 = 0; index2 < input.Count; index2++) {
                    if (index1 != index2) {
                        var pair = GetSinglePair(input[index1]);
                        var next = GetSinglePair(input[index2]);
                        var parent = GetNewPair();
                        parent.NumberLeft = pair;
                        parent.NumberRight = next;
                        pair.Parent = parent;
                        next.Parent = parent;
                        pair = parent;
                        var left = GetLowestLeft(pair.NumberRight);
                        var right = GetLowestRight(pair.NumberLeft);
                        left.ClosestNumberLeft = right;
                        right.ClosestNumberRight = left;
                        ReduceAddition(pair);
                        int magnitude = GetMagnitude(pair);
                        if (magnitude > best) {
                            best = magnitude;
                        }
                    }
                    
                }
            }
            return best;
        }

        private int GetMagnitude(Pair pair) {
            int sum = 0;
            if (pair.IsNumber) {
                return pair.NumberSingle;
            }
            return GetMagnitude(pair.NumberLeft) * 3 + GetMagnitude(pair.NumberRight) * 2;
        }

        private Pair GetLowestRight(Pair pair) {
            do {
                if (pair.NumberRight == null && pair.NumberLeft == null) {
                    return pair;
                }
                if (pair.NumberRight != null) {
                    pair = pair.NumberRight;
                } else {
                    pair = pair.NumberLeft;
                }
            } while (true);
        }

        private Pair GetLowestLeft(Pair pair) {
            do {
                if (pair.NumberRight == null && pair.NumberLeft == null) {
                    return pair;
                }
                if (pair.NumberLeft != null) {
                    pair = pair.NumberLeft;
                } else {
                    pair = pair.NumberRight;
                }
            } while (true);
        }

        private int _actionId;
        private void ReduceAddition(Pair pair) {
            var action = GetFirstAction(pair, 0, true);
            if (action == null) {
                action = GetFirstAction(pair, 0, false);
            }
            while (action != null) {
                _actionId++;
                PerformAction(action);
                action = GetFirstAction(pair, 0, true);
                if (action == null) {
                    action = GetFirstAction(pair, 0, false);
                }
            }
        }

        private Pair GetFirstAction(Pair pair, int level, bool explosionFirst) {
            if (level >= 4 && pair.NumberLeft != null && pair.NumberLeft.IsNumber && pair.NumberRight != null && pair.NumberRight.IsNumber) {
                pair.DoesExplode = true;
                return pair;
            }
            if (!explosionFirst && pair.IsNumber && pair.NumberSingle > 9) {
                pair.DoesExplode = false;
                return pair;
            }
            if (pair.NumberLeft != null) {
                var result = GetFirstAction(pair.NumberLeft, level + 1, explosionFirst);
                if (result != null) {
                    return result;
                }
            }
            if (pair.NumberRight != null) {
                var result = GetFirstAction(pair.NumberRight, level + 1, explosionFirst);
                if (result != null) {
                    return result;
                }
            }
            return null;
        }

        private void PerformAction(Pair pair) {
            if (pair.DoesExplode) {
                Explode(pair);
            } else {
                Split(pair);
            }
        }

        private void Explode(Pair pair) {
            var zeroPair = GetNewPair();
            zeroPair.IsNumber = true;
            zeroPair.Parent = pair.Parent;
            int leftNum = pair.NumberLeft.NumberSingle;
            int rightNum = pair.NumberRight.NumberSingle;
            var closestLeftNum = pair.NumberLeft.ClosestNumberLeft;
            var closestRightNum = pair.NumberRight.ClosestNumberRight;
            if (closestLeftNum != null) {
                closestLeftNum.NumberSingle += leftNum;
                closestLeftNum.ClosestNumberRight = zeroPair;
                zeroPair.ClosestNumberLeft = closestLeftNum;
            }
            if (closestRightNum != null) {
                closestRightNum.NumberSingle += rightNum;
                closestRightNum.ClosestNumberLeft = zeroPair;
                zeroPair.ClosestNumberRight = closestRightNum;
            }
            if (zeroPair.Parent != null) {
                if (zeroPair.Parent.NumberLeft.Id == pair.Id) {
                    zeroPair.Parent.NumberLeft = zeroPair;
                } else if (zeroPair.Parent.NumberRight.Id == pair.Id) {
                    zeroPair.Parent.NumberRight = zeroPair;
                } else {
                    throw new Exception();
                }
            }
        }

        private void Split(Pair pair) {
            var left = GetNewPair();
            left.ClosestNumberLeft = pair.ClosestNumberLeft;
            left.IsNumber = true;
            left.NumberSingle = pair.NumberSingle / 2;
            left.Parent = pair;
            var right = GetNewPair();
            right.ClosestNumberRight = pair.ClosestNumberRight;
            right.IsNumber = true;
            right.NumberSingle = pair.NumberSingle / 2 + (pair.NumberSingle % 2);
            right.Parent = pair;
            left.ClosestNumberRight = right;
            right.ClosestNumberLeft = left;
            pair.IsNumber = false;
            pair.NumberLeft = left;
            pair.NumberRight = right;
            if (left.ClosestNumberLeft != null) {
                left.ClosestNumberLeft.ClosestNumberRight = left;
            }
            if (right.ClosestNumberRight != null) {
                right.ClosestNumberRight.ClosestNumberLeft = right;
            }
        }

        private Pair GetSinglePair(string line) {
            Pair current = GetNewPair();
            Pair left = null;
            for (int index = 1; index < line.Length - 1; index++) {
                var digit = line[index];
                if (digit == '[') {
                    var next = GetNewPair();
                    next.Parent = current;
                    AddChildPair(current, next);
                    current = next;
                } else if (digit == ']') {
                    current = current.Parent;
                } else if (digit == ',') {

                } else {
                    var number = Convert.ToInt32(new string(new char[1] { digit }));
                    var next = GetNewPair();
                    next.IsNumber = true;
                    next.NumberSingle = number;
                    next.Parent = current;
                    AddChildPair(current, next);
                    next.ClosestNumberLeft = left;
                    if (left != null) {
                        left.ClosestNumberRight = next;
                    }
                    left = next;
                }
            }
            return current;
        }

        private void AddChildPair(Pair parent, Pair child) {
            if (parent.NumberLeft == null) {
                parent.NumberLeft = child;
            } else {
                parent.NumberRight = child;
            }
        }

        private string Output(Pair pair) {
            if (pair.IsNumber) {
                return pair.NumberSingle.ToString();
            }
            if (pair.NumberRight == null) {
                return Output(pair.NumberLeft);
            }
            return "[" + Output(pair.NumberLeft) + "," + Output(pair.NumberRight) + "]";
        }

        private string OutputClosest(Pair pair) {
            var text = new StringBuilder();
            var left = GetLowestLeft(pair);
            while (left != null) {
                text.Append(left.NumberSingle.ToString() + ",");
                left = left.ClosestNumberRight;
                if (left != null && !left.IsNumber) {
                    throw new Exception();
                }
            }
            return text.ToString();
        }

        private bool Validate(Pair pair) {
            var full = Output(pair).Split(',').ToList();
            var closest = OutputClosest(pair).Split(',').ToList();
            closest.RemoveAt(closest.Count - 1);
            int index = 0;
            foreach (var digit in full) {
                var format = digit.Replace("[", "").Replace("]", "");
                if (format != closest[index]) {
                    return false;
                }
                index++;
            }
            return true;
        }

        private List<Pair> _pairs = new List<Pair>();
        private Pair GetNewPair() {
            var pair = new Pair();
            pair.Id = _pairs.Count;
            _pairs.Add(pair);
            return pair;
        }

        private class Pair {
            public bool IsAddition { get; set; }
            public bool IsNumber { get; set; }
            public Pair NumberLeft { get; set; }
            public Pair NumberRight { get; set; }
            public Pair Parent { get; set; }
            public int NumberSingle { get; set; }
            public bool DoesExplode { get; set; }
            public Pair ClosestNumberRight { get; set; }
            public Pair ClosestNumberLeft { get; set; }
            public int Id { get; set; }
        }
    }
}
