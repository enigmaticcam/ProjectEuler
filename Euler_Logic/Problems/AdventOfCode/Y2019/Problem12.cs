using Euler_Logic.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Euler_Logic.Problems.AdventOfCode.Y2019 {
    public class Problem12 : AdventOfCodeBase {
        public override string ProblemName {
            get { return "Advent of Code 2019: 12"; }
        }

        public override string GetAnswer() {
            return Answer2(Input()).ToString();
        }

        private int Answer1(List<string> input, int total) {
            var moons = GetMoons(input);
            ApplyGravityAndVelocity(moons, total);
            CalcEnergy(moons);
            return moons.Select(x => x.EnergyTotal).Sum();
        }

        private ulong Answer2(List<string> input) {
            var moons = GetMoons(input);
            ulong count = 1;
            do {
                count++;
                ApplyGravity(moons);
                ApplyVelocity(moons);
                LookForOriginals(moons, count);
                if (_originalDoneX && _originalDoneY && _originalDoneZ) {
                    return LCM.GetLCM(_originalStepsX, LCM.GetLCM(_originalStepsY, _originalStepsZ));
                }
            } while (true);
        }

        private bool _originalDoneX;
        private bool _originalDoneY;
        private bool _originalDoneZ;
        private ulong _originalStepsX;
        private ulong _originalStepsY;
        private ulong _originalStepsZ;
        private void LookForOriginals(List<Moon> moons, ulong count) {
            if (!_originalDoneX) {
                if (moons[0].Original.X == moons[0].Position.X
                    && moons[1].Original.X == moons[1].Position.X
                    && moons[2].Original.X == moons[2].Position.X
                    && moons[3].Original.X == moons[3].Position.X)
                _originalDoneX = true;
                _originalStepsX = count;
            }
            if (!_originalDoneY) {
                if (moons[0].Original.Y == moons[0].Position.Y
                    && moons[1].Original.Y == moons[1].Position.Y
                    && moons[2].Original.Y == moons[2].Position.Y
                    && moons[3].Original.Y == moons[3].Position.Y)
                _originalDoneY = true;
                _originalStepsY = count;
            }
            if (!_originalDoneZ) {
                if (moons[0].Original.Z == moons[0].Position.Z
                    && moons[1].Original.Z == moons[1].Position.Z
                    && moons[2].Original.Z == moons[2].Position.Z
                    && moons[3].Original.Z == moons[3].Position.Z)
                _originalDoneZ = true;
                _originalStepsZ = count;
            }
        }

        private void ApplyGravityAndVelocity(List<Moon> moons, int total) {
            for (int count = 1; count <= total; count++) {
                ApplyGravity(moons);
                ApplyVelocity(moons);
            }
        }

        private void CalcEnergy(List<Moon> moons) {
            foreach (var moon in moons) {
                moon.EnergyKinetic = Math.Abs(moon.Velocity.X) + Math.Abs(moon.Velocity.Y) + Math.Abs(moon.Velocity.Z);
                moon.EnergyPotential = Math.Abs(moon.Position.X) + Math.Abs(moon.Position.Y) + Math.Abs(moon.Position.Z);
                moon.EnergyTotal = moon.EnergyKinetic * moon.EnergyPotential;
            }
        }

        private void ApplyGravity(List<Moon> moons) {
            for (int index1 = 0; index1 < moons.Count; index1++) {
                var moon1 = moons[index1];
                for (int index2 = index1 + 1; index2 < moons.Count; index2++) {
                    var moon2 = moons[index2];
                    if (moon1.Position.X < moon2.Position.X) {
                        moon1.Velocity.X++;
                        moon2.Velocity.X--;
                    } else if (moon1.Position.X > moon2.Position.X) {
                        moon1.Velocity.X--;
                        moon2.Velocity.X++;
                    }
                    if (moon1.Position.Y < moon2.Position.Y) {
                        moon1.Velocity.Y++;
                        moon2.Velocity.Y--;
                    } else if (moon1.Position.Y > moon2.Position.Y) {
                        moon1.Velocity.Y--;
                        moon2.Velocity.Y++;
                    }
                    if (moon1.Position.Z < moon2.Position.Z) {
                        moon1.Velocity.Z++;
                        moon2.Velocity.Z--;
                    } else if (moon1.Position.Z > moon2.Position.Z) {
                        moon1.Velocity.Z--;
                        moon2.Velocity.Z++;
                    }
                }
            }
        }

        private void ApplyVelocity(List<Moon> moons) {
            foreach (var moon in moons) {
                moon.Position.X += moon.Velocity.X;
                moon.Position.Y += moon.Velocity.Y;
                moon.Position.Z += moon.Velocity.Z;
            }
        }

        private List<Moon> GetMoons(List<string> input) {
            var moons = new List<Moon>();
            foreach (var next in input) {
                var split = next.Split(',');
                var moon = new Moon() {
                    Position = new Point(
                        x: Convert.ToInt32(split[0].Replace("<x=", "")),
                        y: Convert.ToInt32(split[1].Replace("y=", "")),
                        z: Convert.ToInt32(split[2].Replace("z=", "").Replace(">", ""))
                    ),
                    Velocity = new Point()
                };
                moon.Original = new Point(moon.Position.X, moon.Position.Y, moon.Position.Z);
                moons.Add(moon);
            }
            return moons;
        }

        private List<string> Test1Input() {
            return new List<string>() {
                "<x=-1, y=0, z=2>",
                "<x=2, y=-10, z=-7>",
                "<x=4, y=-8, z=8>",
                "<x=3, y=5, z=-1>"
            };
        }

        private List<string> Test2Input() {
            return new List<string>() {
                "<x=-8, y=-10, z=0>",
                "<x=5, y=5, z=10>",
                "<x=2, y=-7, z=3>",
                "<x=9, y=-8, z=-3> "
            };
        }

        private class Moon {
            public Point Position { get; set; }
            public Point Velocity { get; set; }
            public int EnergyTotal { get; set; }
            public int EnergyKinetic { get; set; }
            public int EnergyPotential { get; set; }
            public Point Original { get; set; }
        }

        public class Point {
            public Point() { }
            public Point(int x, int y, int z) {
                X = x;
                Y = y;
                Z = z;
            }
            public int X { get; set; }
            public int Y { get; set; }
            public int Z { get; set; }
        }
    }
}
