using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Euler_Logic.Problems.AdventOfCode.Y2023;

public class Problem25 : AdventOfCodeBase {
    public override string ProblemName => "Advent of Code 2023: 25";

    public override string GetAnswer() {
        //return Answer1(Input_Test(1)).ToString();
        return Answer1(Input()).ToString();
    }

    public override string GetAnswer2() {
        return Answer2(Input()).ToString();
    }

    private int Answer1(List<string> input) {
        var connections = GetConnections(input);
        var shorts = GetShortConnects(connections);
        //Save(shorts);
        Complete(connections);
        return Solve(connections, shorts);
    }

    private void Save(List<string[]> shorts) {
        using (var stream = new StreamWriter(@"c:\temp\temp.txt")) {
            foreach (var sho in shorts) {
                stream.WriteLine($"{sho[0]} {sho[1]}");
            }
        }
    }

    private string Answer2(List<string> input) {
        return "";
    }

    private int Solve(Dictionary<string, Connection> connections, List<string[]> shorts) {
        for (int index1 = 0; index1 < shorts.Count; index1++) {
            RemoveShort(connections, shorts[index1]);
            for (int index2 = index1 + 1; index2 < shorts.Count; index2++) {
                RemoveShort(connections, shorts[index2]);
                CountGroups(connections);
                AddShort(connections, shorts[index2]);
            }
            AddShort(connections, shorts[index1]);
        }
        CountGroups(connections);
        return 0;
    }

    private void RemoveShort(Dictionary<string, Connection> connections, string[] shor) {
        connections[shor[0]].End.Remove(shor[1]);
        connections[shor[1]].End.Remove(shor[0]);
    }

    private void AddShort(Dictionary<string, Connection> connections, string[] shor) {
        connections[shor[0]].End.Add(shor[1]);
        connections[shor[1]].End.Add(shor[0]);
    }

    private int[] _counts = new int[2];
    private bool CountGroups(Dictionary<string, Connection> connections) {
        _counts[0] = 0;
        _counts[1] = 0;
        int index = 0;
        bool isGood = false;
        var hash = new HashSet<string>();
        foreach (var connection in connections.Values) {
            if (!hash.Contains(connection.Start)) {
                _counts[index] = BFS(connections, hash, connection.Start);
                if (hash.Count == connections.Count)
                    break;
                index++;
                isGood = true;
            }
        }
        return isGood;
    }

    private int BFS(Dictionary<string, Connection> connections, HashSet<string> hash, string start) {
        int count = 0;
        var list = new LinkedList<string>();
        list.AddFirst(start);
        hash.Add(start);
        do {
            count++;
            var connection = connections[list.First.Value];
            foreach (var end in connection.End) {
                if (!hash.Contains(end)) {
                    list.AddLast(end);
                    hash.Add(end);
                }
            }
            list.RemoveFirst();
        } while (list.Count > 0);
        return count;
    }

    private void Complete(Dictionary<string, Connection> connections) {
        foreach (var connection in connections.Values.ToList()) {
            foreach (var end in connection.End) {
                if (!connections.ContainsKey(end))
                    connections.Add(end, new Connection() {
                        Start = end,
                        End = new HashSet<string>()
                    });
                connections[end].End.Add(connection.Start);
            }
        }
    }

    private List<string[]> GetShortConnects(Dictionary<string, Connection> connections) {
        var list = new List<string[]>();
        foreach (var connect in connections) {
            foreach (var end in connect.Value.End) {
                list.Add(new string[2] { connect.Value.Start, end });
            }
        }
        return list;
    }

    private Dictionary<string, Connection> GetConnections(List<string> input) {
        var hash = new Dictionary<string, Connection>();
        foreach (var line in input) {
            var split = line.Split(' ');
            var connect = new Connection() { 
                Start = split[0].Replace(":", ""),
                End = new HashSet<string>()
            };
            for (int index = 1; index < split.Length; index++) {
                connect.End.Add(split[index]);
            }
            hash.Add(connect.Start, connect);
        }
        return hash;
    }

    private class Connection {
        public string Start { get; set; }
        public HashSet<string> End { get; set; }
    }
}
