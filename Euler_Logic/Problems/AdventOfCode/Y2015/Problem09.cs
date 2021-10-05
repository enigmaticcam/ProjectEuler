using Euler_Logic.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems.AdventOfCode.Y2015 {
    public class Problem09 : AdventOfCodeBase {
        private Dictionary<string, City> _cities;
        private List<Edge> _edges;
        private PowerAll _powerOf2;

        public override string ProblemName => "Advent of Code 2015: 9";

        public override string GetAnswer() {
            _powerOf2 = new PowerAll(2);
            return Answer2(Input()).ToString();
        }

        private int Answer1(List<string> input) {
            GetEdges(input);
            GetCities();
            TryAll(0, _cities.Count, 0, null);
            return _lowest;
        }

        private int Answer2(List<string> input) {
            GetEdges(input);
            GetCities();
            TryAll(0, _cities.Count, 0, null);
            return _highest;
        }

        private int _lowest = int.MaxValue;
        private int _highest = 0;
        private void TryAll(ulong bits, int remaining, int distance, City lastCity) {
            for (int index = 0; index < _cities.Values.Count; index++) {
                var bit = _powerOf2.GetPower(index);
                if ((bits & bit) == 0) {
                    var thisCity = _cities.Values.ElementAt(index);
                    bits += bit;
                    if (lastCity != null) {
                        distance += lastCity.Edges[thisCity.Name];
                    }
                    if (remaining > 1) {
                        TryAll(bits, remaining - 1, distance, thisCity);
                    } else {
                        if (distance < _lowest) {
                            _lowest = distance;
                        }
                        if (distance > _highest) {
                            _highest = distance;
                        }
                    }
                    if (lastCity != null) {
                        distance -= lastCity.Edges[thisCity.Name];
                    }
                    bits -= bit;
                }
            }
        }

        private void GetCities() {
            _cities = new Dictionary<string, City>();
            foreach (var edge in _edges) {
                if (!_cities.ContainsKey(edge.City1)) {
                    var city = new City();
                    city.Edges = new Dictionary<string, int>();
                    city.Name = edge.City1;
                    _cities.Add(city.Name, city);
                    city.Edges.Add(edge.City2, edge.Distance);
                } else {
                    var city = _cities[edge.City1];
                    city.Edges.Add(edge.City2, edge.Distance);
                }
                if (!_cities.ContainsKey(edge.City2)) {
                    var city = new City();
                    city.Edges = new Dictionary<string, int>();
                    city.Name = edge.City2;
                    _cities.Add(city.Name, city);
                    city.Edges.Add(edge.City1, edge.Distance);
                } else {
                    var city = _cities[edge.City2];
                    city.Edges.Add(edge.City1, edge.Distance);
                }
            }
        }

        private void GetEdges(List<string> input) {
            _edges = input.Select(line => {
                var edge = new Edge();
                var split = line.Split(' ');
                edge.City1 = split[0];
                edge.City2 = split[2];
                edge.Distance = Convert.ToInt32(split[4]);
                return edge;
            }).ToList();
        }

        private List<string> TestInput() {
            return new List<string>() {
                "London to Dublin = 464",
                "London to Belfast = 518",
                "Dublin to Belfast = 141"
            };
        }

        private class Edge {
            public string City1 { get; set; }
            public string City2 { get; set; }
            public int Distance { get; set; }
        }

        private class City {
            public string Name { get; set; }
            public Dictionary<string, int> Edges { get; set; }
        }
    }
}
