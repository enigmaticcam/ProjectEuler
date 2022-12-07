using System;
using System.Collections.Generic;
using System.Linq;

namespace Euler_Logic.Problems.AdventOfCode.Y2022 {
    public class Problem07 : AdventOfCodeBase {
        public override string ProblemName => "Advent of Code 2022: 7";
        private Dir _currentDir;
        private List<Dir> _allDirs;

        public override string GetAnswer() {
            return Answer1(Input()).ToString();
        }

        public override string GetAnswer2() {
            return Answer2(Input()).ToString();
        }

        private ulong Answer1(List<string> input) {
            _allDirs = new List<Dir>();
            Process(input);
            NavigateToRoot();
            CalcSize(_currentDir);
            return FindMaxSum(100000);
        }

        private ulong Answer2(List<string> input) {
            _allDirs = new List<Dir>();
            Process(input);
            NavigateToRoot();
            CalcSize(_currentDir);
            return FindDelete();
        }

        private void Process(List<string> commands) {
            _currentDir = new Dir("root", null);
            int index = 0;
            while (index < commands.Count) {
                var line = commands[index];
                if (line[2] == 'l') {
                    index = Process_ls(commands, index);
                } else {
                    index = Process_cd(commands, index);
                }
            }
        }

        private int Process_ls(List<string> commands, int index) {
            index++;
            while (index < commands.Count) {
                var split = commands[index].Split(' ');
                if (split[0] == "dir") {
                    _currentDir.ChildrenDir.Add(split[1], new Dir(split[1], _currentDir));
                    _allDirs.Add(_currentDir.ChildrenDir[split[1]]);
                } else if (split[0][0] != '$') {
                    _currentDir.Files.Add(new Fil() {
                        Name = split[1],
                        Size = Convert.ToUInt64(split[0])
                    });
                } else {
                    return index;
                }
                index++;
            }
            return index;
        }

        private int Process_cd(List<string> commands, int index) {
            var split = commands[index].Split(' ');
            if (split[2] == "..") {
                _currentDir = _currentDir.ParentDir;
            } else if (split[2] == "/") {
                NavigateToRoot();
            } else {
                if (!_currentDir.ChildrenDir.ContainsKey(split[2])) {
                    _currentDir.ChildrenDir.Add(split[2], new Dir(split[2], _currentDir));
                    _allDirs.Add(_currentDir.ChildrenDir[split[2]]);
                }
                _currentDir = _currentDir.ChildrenDir[split[2]];
            }
            return index + 1;
        }

        private void NavigateToRoot() {
            while (_currentDir.ParentDir != null) {
                _currentDir = _currentDir.ParentDir;
            }
        }

        private ulong CalcSize(Dir dir) {
            foreach (var child in dir.ChildrenDir.Values) {
                dir.Size += CalcSize(child);
            }
            foreach (var file in dir.Files) {
                dir.Size += file.Size;
            }
            return dir.Size;
        }

        private ulong FindMaxSum(ulong maxSum) {
            ulong sum = 0;
            foreach (var dir in _allDirs) {
                if (dir.Size <= maxSum) sum += dir.Size;
            }
            return sum;
        }

        private ulong FindDelete() {
            ulong total = 70000000;
            ulong need = 30000000;
            var used = _currentDir.Size;
            var delete = need - (total - used);
            var best = _allDirs
                .Where(x => x.Size >= delete)
                .OrderBy(x => x.Size)
                .First();
            return best.Size;

        }

        private class Dir {
            public Dir(string name, Dir parent) {
                Name = name;
                ParentDir = parent;
                ChildrenDir = new Dictionary<string, Dir>();
                Files = new List<Fil>();
            }
            public Dir ParentDir { get; set; }
            public Dictionary<string, Dir> ChildrenDir { get; set; }
            public List<Fil> Files { get; set; }
            public string Name { get; set; }
            public ulong Size { get; set; }
        }

        private class Fil {
            public string Name { get; set; }
            public ulong Size { get; set; }
        }
    }
}
