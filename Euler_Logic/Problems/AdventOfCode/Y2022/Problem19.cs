using System;
using System.Collections.Generic;
using System.Linq;

namespace Euler_Logic.Problems.AdventOfCode.Y2022 {
    public class Problem19 : AdventOfCodeBase {
        public override string ProblemName => "Advent of Code 2022: 19";

        public override string GetAnswer() {
            return Answer1(Input()).ToString();
        }

        public override string GetAnswer2() {
            return Answer2(Input()).ToString();
        }

        private int Answer1(List<string> input) {
            var blueprints = GetBlueprints(input);
            return SumQuality(blueprints, 24);
        }

        private int Answer2(List<string> input) {
            var blueprints = GetBlueprints(input).Take(3).ToList();
            return All(blueprints, 32);
        }

        private int All(List<Blueprint> blueprints, int minutesRemaining) {
            int product = 1;
            foreach (var print in blueprints) {
                var data = new RecursiveData() {
                    MinutesRemaining = minutesRemaining,
                    Materials = new int[4],
                    Robots = new int[4]
                };
                data.Robots[Blueprint.MaterialIndexOre] = 1;
                Recursive(print, data, false);
                product *= data.Best;
            }
            return product;
        }

        private int SumQuality(List<Blueprint> blueprints, int minutesRemaining) {
            int quality = 0;
            foreach (var print in blueprints) {
                var data = new RecursiveData() { 
                    MinutesRemaining = minutesRemaining,
                    Materials = new int[4],
                    Robots = new int[4]
                };
                data.Robots[Blueprint.MaterialIndexOre] = 1;
                Recursive(print, data, false);
                quality += data.Best * print.Id;
            }
            return quality;
        }

        private void Recursive(Blueprint print, RecursiveData data, bool hasConstruction) {
            if (Continue(data)) {
                int minutes = MinutesToMake(data, print.GeodeRobotCost);
                if (((minutes == 0 && !hasConstruction) | (minutes > 0)) && minutes <= data.MinutesRemaining) {
                    Recursive_MakeRobot(print, data, Blueprint.MaterialIndexGeode, print.GeodeRobotCost, minutes, "Geode");
                }
                minutes = MinutesToMake(data, print.ObsidianRobotCost);
                if (((minutes == 0 && !hasConstruction) | (minutes > 0)) && minutes <= data.MinutesRemaining) {
                    Recursive_MakeRobot(print, data, Blueprint.MaterialIndexObsidian, print.ObsidianRobotCost, minutes, "Obsidian");
                }
                minutes = MinutesToMake(data, print.ClayRobotCost);
                if (((minutes == 0 && !hasConstruction) | (minutes > 0)) && minutes <= data.MinutesRemaining) {
                    Recursive_MakeRobot(print, data, Blueprint.MaterialIndexClay, print.ClayRobotCost, minutes, "Clay");
                }
                minutes = MinutesToMake(data, print.OreRobotCost);
                if (((minutes == 0 && !hasConstruction) | (minutes > 0)) && minutes <= data.MinutesRemaining) {
                    Recursive_MakeRobot(print, data, Blueprint.MaterialIndexOre, print.OreRobotCost, minutes, "Ore");
                }
            }
        }

        private void Recursive_MakeRobot(Blueprint print, RecursiveData data, int materialIndex, int[] cost, int minutes, string robot) {
            data.MinutesRemaining -= minutes;
            for (int index = 0; index < data.Materials.Length; index++) {
                data.Materials[index] += data.Robots[index] * minutes;
                data.Materials[index] -= cost[index];
            }
            data.Robots[materialIndex]++;
            CheckBest(data);
            Recursive(print, data, true);
            data.Robots[materialIndex]--;
            for (int index = 0; index < data.Materials.Length; index++) {
                data.Materials[index] += cost[index];
                data.Materials[index] -= data.Robots[index] * minutes;
            }
            data.MinutesRemaining += minutes;
        }

        private int MinutesToMake(RecursiveData data, int[] robotCost) {
            int highest = 1;
            for (int index = 0; index < robotCost.Length; index++) {
                if (robotCost[index] > 0 && (data.Robots[index] == 0)) return -1;
                if (data.Materials[index] < robotCost[index]) {
                    int minutes = 0;
                    int robots = data.Robots[index];
                    int materials = data.Materials[index];
                    if (materials < robotCost[index]) {
                        minutes += (robotCost[index] - materials) / robots;
                        int total = minutes * robots + materials;
                        if (total < robotCost[index]) minutes++;
                    }
                    minutes++;
                    if (minutes > highest) highest = minutes;
                }
            }
            return highest;
        }

        private void CheckBest(RecursiveData data) {
            int total = data.Robots[Blueprint.MaterialIndexGeode] * data.MinutesRemaining + data.Materials[Blueprint.MaterialIndexGeode];
            if (total > data.Best) data.Best = total;
        }

        private bool Continue(RecursiveData data) {
            if (data.Best == 0) return true;
            int total = data.Materials[Blueprint.MaterialIndexGeode];
            total += GetSum(data.MinutesRemaining);
            total += data.Robots[Blueprint.MaterialIndexGeode] * data.MinutesRemaining;
            return total > data.Best;
        }

        private int GetSum(int remaining) {
            if (remaining % 2 == 0) {
                return remaining * (remaining / 2 - 1) + (remaining / 2);
            } else {
                return remaining * ((remaining - 1) / 2);
            }
        }

        private List<Blueprint> GetBlueprints(List<string> input) {
            return input.Select(line => {
                var split = line.Split(' ');
                var blueprint = new Blueprint() {
                    ClayRobotCost = new int[4] { Convert.ToInt32(split[12]), 0, 0, 0 },
                    GeodeRobotCost = new int[4] { Convert.ToInt32(split[27]), 0, Convert.ToInt32(split[30]), 0 },
                    Id = Convert.ToInt32(split[1].Replace(":", "")),
                    ObsidianRobotCost = new int[4] { Convert.ToInt32(split[18]), Convert.ToInt32(split[21]), 0, 0 },
                    OreRobotCost = new int[4] { Convert.ToInt32(split[6]), 0, 0, 0 }
                };
                return blueprint;
            }).ToList();
        }

        private class RecursiveData {
            public int MinutesRemaining { get; set; }
            public int[] Materials { get; set; }
            public int[] Robots { get; set; }
            public int Best { get; set; }
        }

        private class Blueprint {
            public int Id { get; set; }
            public int[] OreRobotCost { get; set; }
            public int[] ClayRobotCost { get; set; }
            public int[] ObsidianRobotCost { get; set; }
            public int[] GeodeRobotCost { get; set; }

            public static int MaterialIndexOre => 0;
            public static int MaterialIndexClay => 1;
            public static int MaterialIndexObsidian => 2;
            public static int MaterialIndexGeode => 3;
        }

        private class MaterialCost {
            public int MaterialIndex { get; set; }
            public int Cost { get; set; }
        }
    }
}
