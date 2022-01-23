using System;
using System.Collections.Generic;
using System.Linq;

namespace Euler_Logic.Problems.AdventOfCode.Y2017 {
    public class Problem20 : AdventOfCodeBase {
        private List<Particle> _particles;

        public override string ProblemName {
            get { return "Advent of Code 2017: 20"; }
        }

        public override string GetAnswer() {
            return Answer1(Input()).ToString();
        }

        public override string GetAnswer2() {
            return Answer2(Input()).ToString();
        }

        private int Answer1(List<string> input) {
            GetParticles(input);
            return FindClosestToZero();
        }

        private int Answer2(List<string> input) {
            GetParticles(input);
            return FindLastOne();
        }

        private int FindLastOne() {
            int count = 0;
            do {
                for (int index = 0; index < _particles.Count; index++) {
                    var particle = _particles[index];
                    if (!particle.IsRemoved) {
                        particle.VelocityX += particle.AccelerationX;
                        particle.VelocityY += particle.AccelerationY;
                        particle.VelocityZ += particle.AccelerationZ;
                        particle.X += particle.VelocityX;
                        particle.Y += particle.VelocityY;
                        particle.Z += particle.VelocityZ;
                        for (int priorIndex = 0; priorIndex < index; priorIndex++) {
                            var prior = _particles[priorIndex];
                            if (!prior.IsRemoved && prior.X == particle.X && prior.Y == particle.Y && prior.Z == particle.Z) {
                                prior.WillBeRemoved = true;
                                particle.WillBeRemoved = true;
                            }
                        }
                    }
                }
                foreach (var particle in _particles) {
                    if (particle.WillBeRemoved) {
                        particle.IsRemoved = true;
                        particle.WillBeRemoved = false;
                    }
                }
                count++;
            } while (count <= 1000);
            return _particles.Where(x => !x.IsRemoved).Count();
        }

        private int FindClosestToZero() {
            long distance = 0;
            long bestDistance = 0;
            int bestParticle = 0;
            int currentParticle = 0;
            int lastBestParticle = -1;
            int noChangeCount = 0;
            do {
                bestDistance = long.MaxValue;
                bestParticle = 0;
                currentParticle = 0;
                foreach (var particle in _particles) {
                    particle.VelocityX += particle.AccelerationX;
                    particle.VelocityY += particle.AccelerationY;
                    particle.VelocityZ += particle.AccelerationZ;
                    particle.X += particle.VelocityX;
                    particle.Y += particle.VelocityY;
                    particle.Z += particle.VelocityZ;
                    distance = Math.Abs(particle.X) + Math.Abs(particle.Y) + Math.Abs(particle.Z);
                    if (distance < bestDistance) {
                        bestDistance = distance;
                        bestParticle = currentParticle;
                    }
                    currentParticle++;
                }
                if (bestParticle == lastBestParticle) {
                    noChangeCount++;
                } else {
                    noChangeCount = 0;
                }
                lastBestParticle = bestParticle;
            } while (noChangeCount < 1000);
            return bestParticle;
        }

        private void GetParticles(List<string> input) {
            _particles = input.Select(line => {
                var split = line
                    .Replace("p=<", "")
                    .Replace(">, v=<", ",")
                    .Replace(">, a=<", ",")
                    .Replace(">", "")
                    .Split(',');
                return new Particle() {
                    X = Convert.ToInt32(split[0]),
                    Y = Convert.ToInt32(split[1]),
                    Z = Convert.ToInt32(split[2]),
                    VelocityX = Convert.ToInt32(split[3]),
                    VelocityY = Convert.ToInt32(split[4]),
                    VelocityZ = Convert.ToInt32(split[5]),
                    AccelerationX = Convert.ToInt32(split[6]),
                    AccelerationY = Convert.ToInt32(split[7]),
                    AccelerationZ = Convert.ToInt32(split[8])
                };
            }).ToList();
        }

        private class Particle {
            public long X { get; set; }
            public long Y { get; set; }
            public long Z { get; set; }
            public long VelocityX { get; set; }
            public long VelocityY { get; set; }
            public long VelocityZ { get; set; }
            public long AccelerationX { get; set; }
            public long AccelerationY { get; set; }
            public long AccelerationZ { get; set; }
            public bool WillBeRemoved { get; set; }
            public bool IsRemoved { get; set; }
        }
    }
}
