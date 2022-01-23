using System;
using System.Collections.Generic;
using System.Linq;

namespace Euler_Logic.Problems.AdventOfCode.Y2018 {
    public class Problem04 : AdventOfCodeBase {
        public override string ProblemName {
            get { return "Advent of Code 2018: 4"; }
        }

        public override string GetAnswer() {
            return Answer1();
        }

        public override string GetAnswer2() {
            return Answer2();
        }

        private string Answer1() {
            var guards = GetGuards();
            return MostSlept(guards).Answer1.ToString();
        }

        private string Answer2() {
            var guards = GetGuards();
            return MostSlept(guards).Answer2.ToString();
        }

        private Result MostSlept(List<Guard> guards) {
            int mostMinutes = 0;
            int highestCount = 0;
            var result = new Result();
            guards.ForEach(guard => {
                var minuteCounts = new int[60];
                int minutes = 0;
                DateTime lastSleep = guard.Entries[0].TimeStamp;
                foreach (var entry in guard.Entries) {
                    switch (entry.Status) {
                        case enumStatus.BeginShift:
                            break;
                        case enumStatus.FallAsleep:
                            lastSleep = entry.TimeStamp;
                            break;
                        case enumStatus.WakeUp:
                            minutes += (entry.TimeStamp - lastSleep).Minutes;
                            for (int minute = lastSleep.Minute; minute < entry.TimeStamp.Minute; minute++) {
                                minuteCounts[minute]++;
                            }
                            break;
                    }
                }
                int bestMinuteCount = 0;
                int bestMinuteNum = 0;
                for (int minute = 0; minute <= 59; minute++) {
                    if (minuteCounts[minute] > bestMinuteCount) {
                        bestMinuteCount = minuteCounts[minute];
                        bestMinuteNum = minute;
                    }
                }
                if (minutes > mostMinutes) {
                    mostMinutes = minutes;
                    result.Answer1 = guard.Id * bestMinuteNum;
                }
                if (bestMinuteCount > highestCount) {
                    highestCount = bestMinuteCount;
                    result.Answer2 = guard.Id * bestMinuteNum;
                }
            });
            return result;
        }

        private DateTime EndOfMidnight(DateTime time) {
            var nextDay = time.AddDays(1);
            return new DateTime(nextDay.Year, nextDay.Month, nextDay.Day, 1, 0, 0);
        }

        private List<Guard> GetGuards() {
            int id = 0;
            var guards = new Dictionary<int, Guard>();
            Guard current = null;
            foreach (var entry in GetEntries()) {
                var sharpIndex = entry.Text.IndexOf("#");
                if (sharpIndex >= 0) {
                    id = Convert.ToInt32(entry.Text.Substring(sharpIndex + 1, entry.Text.IndexOf(" ", sharpIndex + 1) - (sharpIndex + 1)));
                    if (!guards.ContainsKey(id)) {
                        current = new Guard();
                        current.Id = id;
                        guards.Add(id, current);
                    } else {
                        current = guards[id];
                    }
                }
                current.Entries.Add(entry);
            }
            return guards.Values.ToList();
        }

        private List<Entry> GetEntries() {
            return Input().Select(x => {
                var split = x.Split(']');
                var status = enumStatus.BeginShift;
                if (split[1].Contains("falls")) {
                    status = enumStatus.FallAsleep;
                } else if (split[1].Contains("wakes")) {
                    status = enumStatus.WakeUp;
                }
                return new Entry() {
                    Status = status,
                    Text = split[1].Trim(),
                    TimeStamp = DateTime.Parse(split[0].Replace("[", ""))
                };
            }).OrderBy(x => x.TimeStamp).ToList();
        }

        private class Entry {
            public enumStatus Status { get; set; }
            public DateTime TimeStamp { get; set; }
            public string Text { get; set; }
        }

        private class Guard {
            public Guard() {
                Entries = new List<Entry>();
            }
            public int Id { get; set; }
            public List<Entry> Entries { get; set; }
        }

        private enum enumStatus {
            BeginShift,
            FallAsleep,
            WakeUp
        }

        private class Result {
            public int Answer1 { get; set; }
            public int Answer2 { get; set; }
        }
    }
}
