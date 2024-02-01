using System;
using System.Collections.Generic;
using System.Linq;

namespace Euler_Logic.Problems.AdventOfCode.Y2023;

public class Problem25 : AdventOfCodeBase {
    public override string ProblemName => "Advent of Code 2023: 25";

    public override string GetAnswer() {
        return Answer1(Input()).ToString();
    }

    private int Answer1(List<string> input) {
        var connections = GetConnections(input);
        Complete(connections);
        return Solve(connections);
    }

    private int Solve(Dictionary<string, Connection> connections) {
        FindPathCommon(connections);
        RemoveCommon(connections);
        return CountGroups(connections);
    }

    private void RemoveCommon(Dictionary<string, Connection> connections) {
        var toRemove = _counts.OrderByDescending(x => x.Value).Take(3);
        foreach (var remove in toRemove) {
            var split = remove.Key.Split(',');
            connections[split[0]].End.Remove(split[1]);
            connections[split[1]].End.Remove(split[0]);
        }
    }

    private void FindPathCommon(Dictionary<string, Connection> connections) {
        var random = new Random();
        for (int count = 1; count <= 200; count++) {
            var first = connections.Keys.ElementAt(random.Next(connections.Count));
            string second;
            do {
                second = connections.Keys.ElementAt(random.Next(connections.Count));
            } while (second == first);
            FindPath(connections, first, second);
        }
    }

    private Dictionary<string, int> _counts = new();
    private void FindPath(Dictionary<string, Connection> connections, string start, string end) {
        var list = new LinkedList<string>();
        var hash = new HashSet<string>();
        list.AddLast(start);
        hash.Add(start);
        connections[start].From = null;
        do {
            var current = connections[list.First.Value];
            if (current.Start == end)
                break;
            foreach (var next in connections[current.Start].End) {
                if (!hash.Contains(next)) {
                    hash.Add(next);
                    list.AddLast(next);
                    connections[next].From = current;
                }
            }
            list.RemoveFirst();
        } while (true);
        var path = end;
        do {
            string node1 = path;
            path = connections[path].From.Start;
            string node2 = path;
            if (path != start)
                AddToCount(node1, node2);
        } while (path != start);
    }

    private void AddToCount(string node1, string node2) {
        string first = node1;
        string second = node2;
        if (string.Compare(first, second) > 0) {
            first = node2;
            second = node1;
        }
        var key = $"{first},{second}";
        if (!_counts.ContainsKey(key)) {
            _counts.Add(key, 1);
        } else {
            _counts[key]++;
        }
    }

    private int CountGroups(Dictionary<string, Connection> connections) {
        var counts = new int[2];
        counts[0] = 0;
        counts[1] = 0;
        int index = 0;
        var hash = new HashSet<string>();
        foreach (var connection in connections.Values) {
            if (!hash.Contains(connection.Start)) {
                counts[index] = BFS(connections, hash, connection.Start);
                if (hash.Count == connections.Count)
                    break;
                index++;
            }
        }
        return counts[0] * counts[1];
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
        public Connection From { get; set; }
    }
}
