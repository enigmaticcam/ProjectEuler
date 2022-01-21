using System;
using System.Collections.Generic;
using System.Linq;

namespace Euler_Logic.Problems.AdventOfCode.Y2016 {
    public class Problem04 : AdventOfCodeBase {
        public override string ProblemName {
            get { return "Advent of Code 2016: 4"; }
        }

        public override string GetAnswer() {
            return Answer1().ToString();
        }

        public override string GetAnswer2() {
            return Answer2().ToString();
        }

        public int Answer1() {
            var rooms = GetRooms();
            var hash = GetHash(rooms);
            var valids = GetValidRooms(rooms, hash);
            return valids.Select(x => x.Sector).Sum();
        }

        public int Answer2() {
            var rooms = GetRooms();
            var hash = GetHash(rooms);
            var valids = GetValidRooms(rooms, hash);
            return DecryptRooms(valids);
        }

        private int DecryptRooms(List<Room> rooms) {
            var offset = (int)'a';
            foreach (var room in rooms) {
                var name = new char[room.Name.Length];
                int index = 0;
                foreach (var text in room.Name) {
                    if (text == '-') {
                        name[index] = '-';
                    } else {
                        int code = (int)text;
                        code -= offset;
                        code = (code + room.Sector) % 26;
                        code += offset;
                        name[index] = (char)code;
                    }
                    index++;
                }
                var blah = new string(name);
                if (blah.Contains("pole")) {
                    return room.Sector;
                }
            }
            return 0;
        }

        private List<Room> GetValidRooms(List<Room> rooms, Dictionary<char, int> hash) {
            var valids = new List<Room>();
            var keys = hash.Keys.ToList();
            foreach (var room in rooms) {
                foreach (var key in keys) {
                    hash[key] = 0;
                }
                foreach (var text in room.Name) {
                    if (text != '-') {
                        hash[text]++;
                    }
                }
                var ordered = hash.OrderByDescending(x => x.Value).ThenBy(x => x.Key);
                if (ordered.ElementAt(0).Key == room.Checksum[0]
                    && ordered.ElementAt(1).Key == room.Checksum[1]
                    && ordered.ElementAt(2).Key == room.Checksum[2]
                    && ordered.ElementAt(3).Key == room.Checksum[3]
                    && ordered.ElementAt(4).Key == room.Checksum[4]) {
                    valids.Add(room);
                }
            }
            return valids;
        }

        private Dictionary<char, int> GetHash(List<Room> rooms) {
            var hash = new Dictionary<char, int>();
            foreach (var room in rooms) {
                foreach (var text in room.Name) {
                    if (text != '-' && !hash.ContainsKey(text)) {
                        hash.Add(text, 0);
                    }
                }
            }
            return hash;
        }

        private List<Room> GetRooms() {
            var input = Input();
            return input.Select(x => {
                int openBracket = x.IndexOf('[');
                int lastHyphen = x.LastIndexOf('-');
                var room = new Room();
                room.Name = x.Substring(0, lastHyphen);
                room.Sector = Convert.ToInt32(x.Substring(lastHyphen + 1, openBracket - lastHyphen - 1));
                room.Checksum = x.Substring(openBracket + 1, x.Length - openBracket - 2);
                return room;
            }).ToList();
        }

        private class Room {
            public string Name { get; set; }
            public int Sector { get; set; }
            public string Checksum { get; set; }
        }
    }
}
