using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems.AdventOfCode.Y2020 {
    public class Problem12 : AdventOfCodeBase {
        private long _shipX;
        private long _shipY;

        public override string ProblemName => "Advent of Code 2020: 12";

        public override string GetAnswer() {
            return Answer2(Input()).ToString();
        }

        private long Answer1(List<string> input) {
            var commands = GetCommands(input);
            Perform1(commands, "E");
            return Math.Abs(_shipX) + Math.Abs(_shipY);
        }

        private long Answer2(List<string> input) {
            var commands = GetCommands(input);
            Perform2(commands);
            return Math.Abs(_shipX) + Math.Abs(_shipY);
        }

        private void Perform1(List<Command> commands, string startDirection) {
            long direction = GetStartDirection(startDirection);
            foreach (var command in commands) {
                switch (command.Code) {
                    case "N":
                        _shipY += command.Value;
                        break;
                    case "S":
                        _shipY -= command.Value;
                        break;
                    case "E":
                        _shipX += command.Value;
                        break;
                    case "W":
                        _shipX -= command.Value;
                        break;
                    case "L":
                        direction = direction - (command.Value / 90);
                        if (direction < 0) {
                            direction = 4 + direction;
                        }
                        break;
                    case "R":
                        direction = (direction + (command.Value / 90)) % 4;
                        break;
                    case "F":
                        switch (direction) {
                            case 0:
                                _shipY += command.Value;
                                break;
                            case 2:
                                _shipY -= command.Value;
                                break;
                            case 1:
                                _shipX += command.Value;
                                break;
                            case 3:
                                _shipX -= command.Value;
                                break;
                        }
                        break;
                }
            }
        }

        private void Perform2(List<Command> commands) {
            long waypointX = 10;
            long waypointY = 1;
            long temp = 0;
            foreach (var command in commands) {
                switch (command.Code) {
                    case "N":
                        waypointY += command.Value;
                        break;
                    case "S":
                        waypointY -= command.Value;
                        break;
                    case "E":
                        waypointX += command.Value;
                        break;
                    case "W":
                        waypointX -= command.Value;
                        break;
                    case "R":
                        switch (command.Value) {
                            case 90:
                                temp = waypointX;
                                waypointX = waypointY;
                                waypointY = temp * -1;
                                break;
                            case 180:
                                waypointX *= -1;
                                waypointY *= -1;
                                break;
                            case 270:
                                temp = waypointY;
                                waypointY = waypointX;
                                waypointX = temp * -1;
                                break;
                        }
                        break;
                    case "L":
                        switch (command.Value) {
                            case 90:
                                temp = waypointY;
                                waypointY = waypointX;
                                waypointX = temp * -1;
                                break;
                            case 180:
                                waypointX *= -1;
                                waypointY *= -1;
                                break;
                            case 270:
                                temp = waypointX;
                                waypointX = waypointY;
                                waypointY = temp * -1;
                                break;
                        }
                        break;
                    case "F":
                        _shipX += waypointX * command.Value;
                        _shipY += waypointY * command.Value;
                        break;
                }
            }
        }

        private long GetStartDirection(string code) {
            switch (code) {
                case "S":
                    return 2;
                case "E":
                    return 1;
                case "W":
                    return 3;
                case "N":
                    return 0;
                default:
                    throw new Exception();
            }
        }

        private List<Command> GetCommands(List<string> input) {
            return input.Select(line => {
                var command = new Command();
                command.Code = line.Substring(0, 1);
                command.Value = Convert.ToInt64(line.Substring(1, line.Length - 1));
                return command;
            }).ToList();
        }

        private List<string> TestInput() {
            return new List<string>() {
                "F10",
                "N3",
                "F7",
                "R90",
                "F11"
            };
        }

        private class Command {
            public string Code { get; set; }
            public long Value { get; set; }
        }
    }
}
