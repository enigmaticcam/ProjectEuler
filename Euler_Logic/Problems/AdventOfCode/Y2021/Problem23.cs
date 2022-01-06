using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Euler_Logic.Problems.AdventOfCode.Y2021 {
    public class Problem23 : AdventOfCodeBase {
        private List<Pod> _pods;
        private Pod[] _locations;
        private Dictionary<ulong, ulong> _hash;
        private ulong _best = ulong.MaxValue;
        private HomeBucket[] _homeBuckets;
        private List<Route> _routes;

        private enum enumRouteType {
            HomeToHome,
            HomeToHallway,
            HallwayToHome
        }

        public override string ProblemName {
            get { return "Advent of Code 2021: 23"; }
        }

        public override string GetAnswer() {
            return Answer1(Input_Test(1)).ToString();
        }

        private ulong Answer1(List<string> input) {
            _locations = new Pod[7];
            _hash = new Dictionary<ulong, ulong>();
            SetHomeBuckets(true);
            GetPods(input, true);
            SetRoutes2();
            return GetRecursiveCount2(BuildKey(), 0, 1);
        }

        private ulong GetRecursiveCount2(ulong key, ulong prior, int moveCount) {
            ulong best = ulong.MaxValue;
            _hash.Add(key, 0);
            foreach (var route in _routes) {
                Action undo = null;
                bool didPerform = false;
                Pod pod = null;
                ulong additionalSteps = 0;
                if (moveCount == 1 && route.RouteType == enumRouteType.HomeToHallway && route.Start == 3 && route.End == 6) {
                    bool stop = true;
                }
                if (moveCount == 2 && route.RouteType == enumRouteType.HomeToHallway && route.Start == 3 && route.End == 0) {
                    bool stop = true;
                }
                if (moveCount == 3 && route.RouteType == enumRouteType.HomeToHallway && route.Start == 2 && route.End == 5) {
                    bool stop = true;
                }
                switch (route.RouteType) {
                    case enumRouteType.HallwayToHome:
                        if (CanPerformHallwayToHome(route)) {
                            additionalSteps =  3 - (ulong)(_homeBuckets[route.End].NextOpen);
                            didPerform = true;
                            pod = PerformHallwayToHome(route.Start, route.End);
                            undo = () => PerformHomeToHallway(route.End, route.Start);
                        }
                        break;
                    case enumRouteType.HomeToHallway:
                        if (CanPerformHomeToHallway(route)) {
                            additionalSteps = 3 - (ulong)(_homeBuckets[route.Start].NextOpen - 1);
                            didPerform = true;
                            pod = PerformHomeToHallway(route.Start, route.End);
                            undo = () => PerformHallwayToHome(route.End, route.Start);
                        }
                        break;
                    case enumRouteType.HomeToHome:
                        if (CanPerformHomeToHome(route)) {
                            additionalSteps = (3 - (ulong)(_homeBuckets[route.Start].NextOpen - 1)) + (3 - (ulong)(_homeBuckets[route.End].NextOpen));
                            didPerform = true;
                            pod = PerformHomeToHome(route.Start, route.End);
                            undo = () => PerformHomeToHome(route.End, route.Start);
                        }
                        break;
                }
                if (didPerform) {
                    ulong sub = pod.Energy * (route.Steps + additionalSteps);
                    if (IsSolved()) {
                        if (sub < best) {
                            best = sub;
                        }
                        if (sub + prior < _best) {
                            _best = sub + prior;
                        }
                    } else {
                        var nextKey = BuildKey();
                        ulong next = 0;
                        if (!_hash.ContainsKey(nextKey)) {
                            next = GetRecursiveCount2(nextKey, sub + prior, moveCount + 1);
                        } else {
                            next = _hash[nextKey];
                        }
                        if (next != 0 && next != ulong.MaxValue && sub + next < best) {
                            best = sub + next;
                            if (prior + best < _best) {
                                _best = prior + best;
                            }
                        }
                    }
                    undo();
                }
            }
            _hash[key] = best;
            return _hash[key];
        }

        private bool IsSolved() {
            return _homeBuckets[0].IsComplete && _homeBuckets[1].IsComplete && _homeBuckets[2].IsComplete && _homeBuckets[3].IsComplete;
        }

        private bool CanPerformHallwayToHome(Route route) {
            var end = _homeBuckets[route.End];
            Pod pod = _locations[route.Start];

            // No pod at position
            if (pod == null) return false;

            // End bucket is not home bucket for pod
            if (route.End != pod.HomeBucket) return false;

            // End bucket has pods that don't belong in that bucket
            foreach (var endPod in end.Pods) {
                if (endPod != null && endPod.HomeBucket != route.End) return false;
            }

            // End bucket is already complete
            if (end.IsComplete) return false;

            // Route is blocked
            foreach (var space in route.SpacesCovered) {
                if (_locations[space] != null) return false;
            }

            return true;
        }

        private bool CanPerformHomeToHallway(Route route) {
            var start = _homeBuckets[route.Start];

            // Start bucket is complete
            if (start.IsComplete) return false;

            // Start bucket is empty
            if (start.NextOpen == 0) return false;

            // route is blocked
            foreach (var space in route.SpacesCovered) {
                if (_locations[space] != null) return false;
            }

            return true;
        }

        private bool CanPerformHomeToHome(Route route) {
            var start = _homeBuckets[route.Start];
            var end = _homeBuckets[route.End];
            Pod pod = null;

            // Start bucket is empty
            if (_homeBuckets[route.Start].NextOpen == 0) return false;
            pod = start.Pods[start.NextOpen - 1];

            // End bucket is not home bucket for pod
            if (route.End != pod.HomeBucket) return false;

            // End bucket has pods that don't belong in that bucket
            foreach (var endPod in end.Pods) {
                if (endPod != null && endPod.HomeBucket != route.End) return false;
            }

            // End bucket is already complete
            if (end.IsComplete) return false;

            // Route is blocked
            foreach (var space in route.SpacesCovered) {
                if (_locations[space] != null) return false;
            }

            return true;
        }

        private Pod PerformHallwayToHome(int hallwayStart, int homeEnd) {
            var end = _homeBuckets[homeEnd];
            var pod = _locations[hallwayStart];
            _locations[hallwayStart] = null;
            end.Pods[end.NextOpen] = pod;
            end.NextOpen++;
            end.IsComplete = end.NextOpen > end.MaxIndex && end.Pods[0].HomeBucket == homeEnd && end.Pods[1].HomeBucket == homeEnd && end.Pods[2].HomeBucket == homeEnd && end.Pods[3].HomeBucket == homeEnd;
            pod.Position = end.Positions[end.NextOpen - 1];
            return pod;
        }

        private Pod PerformHomeToHome(int homeStart, int homeEnd) {
            var start = _homeBuckets[homeStart];
            var end = _homeBuckets[homeEnd];
            end.Pods[end.NextOpen] = start.Pods[start.NextOpen - 1];
            end.NextOpen++;
            start.NextOpen--;
            start.Pods[start.NextOpen] = null;
            start.IsComplete = false;
            end.IsComplete = end.NextOpen > end.MaxIndex && end.Pods[0].HomeBucket == homeEnd && end.Pods[1].HomeBucket == homeEnd && end.Pods[2].HomeBucket == homeEnd && end.Pods[3].HomeBucket == homeEnd;
            end.Pods[end.NextOpen - 1].Position = end.Positions[end.NextOpen - 1];
            return end.Pods[end.NextOpen - 1];
        }

        private Pod PerformHomeToHallway(int homeStart, int hallwayEnd) {
            var start = _homeBuckets[homeStart];
            _locations[hallwayEnd] = start.Pods[start.NextOpen - 1];
            start.NextOpen--;
            start.Pods[start.NextOpen] = null;
            _locations[hallwayEnd].Position = (ulong)hallwayEnd;
            start.IsComplete = false;
            return _locations[hallwayEnd];
        }

        private void SetRoutes2() {
            _routes = new List<Route>();
            _routes.Add(new Route() { RouteType = enumRouteType.HomeToHome, Start = 0, End = 1, SpacesCovered = new List<ulong>() { 2 }, Steps = 4 }); // Start & End are Homebucket indexes
            _routes.Add(new Route() { RouteType = enumRouteType.HomeToHome, Start = 0, End = 2, SpacesCovered = new List<ulong>() { 2, 3 }, Steps = 6 });
            _routes.Add(new Route() { RouteType = enumRouteType.HomeToHome, Start = 0, End = 3, SpacesCovered = new List<ulong>() { 2, 3, 4 }, Steps = 8 });
            _routes.Add(new Route() { RouteType = enumRouteType.HomeToHome, Start = 1, End = 0, SpacesCovered = new List<ulong>() { 2 }, Steps = 4 });
            _routes.Add(new Route() { RouteType = enumRouteType.HomeToHome, Start = 1, End = 2, SpacesCovered = new List<ulong>() { 3 }, Steps = 4 });
            _routes.Add(new Route() { RouteType = enumRouteType.HomeToHome, Start = 1, End = 3, SpacesCovered = new List<ulong>() { 3, 4 }, Steps = 6 });
            _routes.Add(new Route() { RouteType = enumRouteType.HomeToHome, Start = 2, End = 0, SpacesCovered = new List<ulong>() { 3, 2 }, Steps = 6 });
            _routes.Add(new Route() { RouteType = enumRouteType.HomeToHome, Start = 2, End = 1, SpacesCovered = new List<ulong>() { 3 }, Steps = 4 });
            _routes.Add(new Route() { RouteType = enumRouteType.HomeToHome, Start = 2, End = 3, SpacesCovered = new List<ulong>() { 4 }, Steps = 4 });
            _routes.Add(new Route() { RouteType = enumRouteType.HomeToHome, Start = 3, End = 0, SpacesCovered = new List<ulong>() { 4, 3, 2 }, Steps = 8 });
            _routes.Add(new Route() { RouteType = enumRouteType.HomeToHome, Start = 3, End = 1, SpacesCovered = new List<ulong>() { 4, 3 }, Steps = 6 });
            _routes.Add(new Route() { RouteType = enumRouteType.HomeToHome, Start = 3, End = 2, SpacesCovered = new List<ulong>() { 4 }, Steps = 4 });

            _routes.Add(new Route() { RouteType = enumRouteType.HomeToHallway, Start = 0, End = 0, SpacesCovered = new List<ulong>() { 1, 0 }, Steps = 3 }); // Start is HomeBucket index, End is position
            _routes.Add(new Route() { RouteType = enumRouteType.HomeToHallway, Start = 0, End = 1, SpacesCovered = new List<ulong>() { 1 }, Steps = 2 });
            _routes.Add(new Route() { RouteType = enumRouteType.HomeToHallway, Start = 0, End = 2, SpacesCovered = new List<ulong>() { 2 }, Steps = 2 });
            _routes.Add(new Route() { RouteType = enumRouteType.HomeToHallway, Start = 0, End = 3, SpacesCovered = new List<ulong>() { 2, 3 }, Steps = 4 });
            _routes.Add(new Route() { RouteType = enumRouteType.HomeToHallway, Start = 0, End = 4, SpacesCovered = new List<ulong>() { 2, 3, 4 }, Steps = 6 });
            _routes.Add(new Route() { RouteType = enumRouteType.HomeToHallway, Start = 0, End = 5, SpacesCovered = new List<ulong>() { 2, 3, 4, 5 }, Steps = 8 });
            _routes.Add(new Route() { RouteType = enumRouteType.HomeToHallway, Start = 0, End = 6, SpacesCovered = new List<ulong>() { 2, 3, 4, 5, 6 }, Steps = 9 });
            _routes.Add(new Route() { RouteType = enumRouteType.HomeToHallway, Start = 1, End = 0, SpacesCovered = new List<ulong>() { 2, 1, 0 }, Steps = 5 });
            _routes.Add(new Route() { RouteType = enumRouteType.HomeToHallway, Start = 1, End = 1, SpacesCovered = new List<ulong>() { 2, 1 }, Steps = 4 });
            _routes.Add(new Route() { RouteType = enumRouteType.HomeToHallway, Start = 1, End = 2, SpacesCovered = new List<ulong>() { 2 }, Steps = 2 });
            _routes.Add(new Route() { RouteType = enumRouteType.HomeToHallway, Start = 1, End = 3, SpacesCovered = new List<ulong>() { 3 }, Steps = 2 });
            _routes.Add(new Route() { RouteType = enumRouteType.HomeToHallway, Start = 1, End = 4, SpacesCovered = new List<ulong>() { 3, 4 }, Steps = 4 });
            _routes.Add(new Route() { RouteType = enumRouteType.HomeToHallway, Start = 1, End = 5, SpacesCovered = new List<ulong>() { 3, 4, 5 }, Steps = 6 });
            _routes.Add(new Route() { RouteType = enumRouteType.HomeToHallway, Start = 1, End = 6, SpacesCovered = new List<ulong>() { 3, 4, 5, 6 }, Steps = 7 });
            _routes.Add(new Route() { RouteType = enumRouteType.HomeToHallway, Start = 2, End = 0, SpacesCovered = new List<ulong>() { 3, 2, 1, 0 }, Steps = 7 });
            _routes.Add(new Route() { RouteType = enumRouteType.HomeToHallway, Start = 2, End = 1, SpacesCovered = new List<ulong>() { 3, 2, 1 }, Steps = 6 });
            _routes.Add(new Route() { RouteType = enumRouteType.HomeToHallway, Start = 2, End = 2, SpacesCovered = new List<ulong>() { 3, 2 }, Steps = 4 });
            _routes.Add(new Route() { RouteType = enumRouteType.HomeToHallway, Start = 2, End = 3, SpacesCovered = new List<ulong>() { 3 }, Steps = 2 });
            _routes.Add(new Route() { RouteType = enumRouteType.HomeToHallway, Start = 2, End = 4, SpacesCovered = new List<ulong>() { 4 }, Steps = 2 });
            _routes.Add(new Route() { RouteType = enumRouteType.HomeToHallway, Start = 2, End = 5, SpacesCovered = new List<ulong>() { 4, 5 }, Steps = 4 });
            _routes.Add(new Route() { RouteType = enumRouteType.HomeToHallway, Start = 2, End = 6, SpacesCovered = new List<ulong>() { 4, 5, 6 }, Steps = 5 });
            _routes.Add(new Route() { RouteType = enumRouteType.HomeToHallway, Start = 3, End = 0, SpacesCovered = new List<ulong>() { 4, 3, 2, 1, 0 }, Steps = 9 });
            _routes.Add(new Route() { RouteType = enumRouteType.HomeToHallway, Start = 3, End = 1, SpacesCovered = new List<ulong>() { 4, 3, 2, 1 }, Steps = 8 });
            _routes.Add(new Route() { RouteType = enumRouteType.HomeToHallway, Start = 3, End = 2, SpacesCovered = new List<ulong>() { 4, 3, 2 }, Steps = 6 });
            _routes.Add(new Route() { RouteType = enumRouteType.HomeToHallway, Start = 3, End = 3, SpacesCovered = new List<ulong>() { 4, 3 }, Steps = 4 });
            _routes.Add(new Route() { RouteType = enumRouteType.HomeToHallway, Start = 3, End = 4, SpacesCovered = new List<ulong>() { 4 }, Steps = 2 });
            _routes.Add(new Route() { RouteType = enumRouteType.HomeToHallway, Start = 3, End = 5, SpacesCovered = new List<ulong>() { 5 }, Steps = 2 });
            _routes.Add(new Route() { RouteType = enumRouteType.HomeToHallway, Start = 3, End = 6, SpacesCovered = new List<ulong>() { 5, 6 }, Steps = 3 });

            foreach (var route in _routes.Where(x => x.RouteType == enumRouteType.HomeToHallway).ToList()) { // Start is position, End is HomeBucket Index
                var routeToAdd = new Route() { RouteType = enumRouteType.HallwayToHome, Start = route.End, End = route.Start, SpacesCovered = new List<ulong>(route.SpacesCovered), Steps = route.Steps };
                routeToAdd.SpacesCovered.Remove((ulong)routeToAdd.Start);
                _routes.Add(routeToAdd);
                //_routes.Add(new Route() { RouteType = enumRouteType.HallwayToHome, Start = route.End, End = route.Start, SpacesCovered = route.SpacesCovered, Steps = route.Steps });
            }
            for (int index = 0; index < _routes.Count; index++) {
                _routes[index].Id = index;
            }
            _routes = _routes.OrderBy(x => {
                if (x.RouteType == enumRouteType.HomeToHome) {
                    return 0;
                } else if (x.RouteType == enumRouteType.HallwayToHome) {
                    return 1;
                } else {
                    return 2;
                }
            }).ThenBy(x => x.Steps).ToList();
        }

        private void SetHomeBuckets(bool insertMiddle) {
            int count = (insertMiddle ? 4 : 2);
            _homeBuckets = new HomeBucket[4];
            for (int index = 0; index < 4; index++) {
                _homeBuckets[index] = new HomeBucket() { Pods = new Pod[count], Positions = new ulong[count], NextOpen = count, MaxIndex = count - 1 };
            }
        }

        private void GetPods(List<string> input, bool insertMiddle) {
            _pods = new List<Pod>();
            if (!insertMiddle) {
                _pods.Add(new Pod() { BitShift = 0, Name = input[2][3], Position = 7 });
                _pods.Add(new Pod() { BitShift = 4, Name = input[2][5], Position = 9 });
                _pods.Add(new Pod() { BitShift = 8, Name = input[2][7], Position = 11 });
                _pods.Add(new Pod() { BitShift = 12, Name = input[2][9], Position = 13 });
                _pods.Add(new Pod() { BitShift = 16, Name = input[3][3], Position = 8 });
                _pods.Add(new Pod() { BitShift = 20, Name = input[3][5], Position = 10 });
                _pods.Add(new Pod() { BitShift = 24, Name = input[3][7], Position = 12 });
                _pods.Add(new Pod() { BitShift = 28, Name = input[3][9], Position = 14 });
            } else {
                _pods.Add(new Pod() { BitShift = 0, Name = input[2][3], Position = 7 });
                _pods.Add(new Pod() { BitShift = 4, Name = input[2][5], Position = 11 });
                _pods.Add(new Pod() { BitShift = 8, Name = input[2][7], Position = 15 });
                _pods.Add(new Pod() { BitShift = 12, Name = input[2][9], Position = 19 });
                _pods.Add(new Pod() { BitShift = 16, Name = input[3][3], Position = 10 });
                _pods.Add(new Pod() { BitShift = 20, Name = input[3][5], Position = 14 });
                _pods.Add(new Pod() { BitShift = 24, Name = input[3][7], Position = 18 });
                _pods.Add(new Pod() { BitShift = 28, Name = input[3][9], Position = 22 });
                _pods.Add(new Pod() { BitShift = 32, Name = 'D', Position = 8 });
                _pods.Add(new Pod() { BitShift = 36, Name = 'D', Position = 9 });
                _pods.Add(new Pod() { BitShift = 40, Name = 'C', Position = 12 });
                _pods.Add(new Pod() { BitShift = 44, Name = 'B', Position = 13 });
                _pods.Add(new Pod() { BitShift = 48, Name = 'B', Position = 16 });
                _pods.Add(new Pod() { BitShift = 52, Name = 'A', Position = 17 });
                _pods.Add(new Pod() { BitShift = 56, Name = 'A', Position = 20 });
                _pods.Add(new Pod() { BitShift = 60, Name = 'C', Position = 21 });
            }
            foreach (var pod in _pods) {
                SetPodSpecifics(pod);
                if (insertMiddle) {
                    if (pod.Position <= 10) {
                        _homeBuckets[0].Pods[3 - (pod.Position - 7)] = pod;
                        _homeBuckets[0].Positions[3 - (pod.Position - 7)] = pod.Position;
                    } else if (pod.Position <= 14) {
                        _homeBuckets[1].Pods[3 - (pod.Position - 11)] = pod;
                        _homeBuckets[1].Positions[3 - (pod.Position - 11)] = pod.Position;
                    } else if (pod.Position <= 18) {
                        _homeBuckets[2].Pods[3 - (pod.Position - 15)] = pod;
                        _homeBuckets[2].Positions[3 - (pod.Position - 15)] = pod.Position;
                    } else {
                        _homeBuckets[3].Pods[3 - (pod.Position - 19)] = pod;
                        _homeBuckets[3].Positions[3 - (pod.Position - 19)] = pod.Position;
                    }
                } else {
                    throw new Exception();
                }
            }
        }

        private class HomeBucket {
            public Pod[] Pods { get; set; }
            public ulong[] Positions { get; set; }
            public int NextOpen { get; set; }
            public bool IsComplete { get; set; }
            public int MaxIndex { get; set; }
        }

        private ulong BuildKey() {
            ulong key = 0;
            foreach (var pod in _pods) {
                var subKey = pod.Position;
                subKey <<= pod.BitShift;
                key += subKey;
            }
            return key;
        }

        private void SetPodSpecifics(Pod pod) {
            switch (pod.Name) {
                case 'A':
                    pod.Energy = 1;
                    pod.HomeBucket = 0;
                    break;
                case 'B':
                    pod.Energy = 10;
                    pod.HomeBucket = 1;
                    break;
                case 'C':
                    pod.Energy = 100;
                    pod.HomeBucket = 2;
                    break;
                case 'D':
                    pod.Energy = 1000;
                    pod.HomeBucket = 3;
                    break;
            }
        }

        private string Output() {
            var text = new StringBuilder();
            text.AppendLine("#############");
            text.Append("#");
            if (_locations[0] != null) {
                text.Append(_locations[0].Name);
            } else {
                text.Append(".");
            }
            if (_locations[1] != null) {
                text.Append(_locations[1].Name);
            } else {
                text.Append(".");
            }
            text.Append(".");
            if (_locations[2] != null) {
                text.Append(_locations[2].Name);
            } else {
                text.Append(".");
            }
            text.Append(".");
            if (_locations[3] != null) {
                text.Append(_locations[3].Name);
            } else {
                text.Append(".");
            }
            text.Append(".");
            if (_locations[4] != null) {
                text.Append(_locations[4].Name);
            } else {
                text.Append(".");
            }
            text.Append(".");
            if (_locations[5] != null) {
                text.Append(_locations[5].Name);
            } else {
                text.Append(".");
            }
            if (_locations[6] != null) {
                text.Append(_locations[6].Name);
            } else {
                text.Append(".");
            }
            text.AppendLine("#");
            text.Append("###");
            for (int index = 0; index < 4; index++) {
                if (_homeBuckets[index].Pods[3] != null) {
                    text.Append(_homeBuckets[index].Pods[3].Name);
                } else {
                    text.Append(".");
                }
                text.Append("#");
            }
            text.AppendLine("##");
            for (int count = 2; count >= 0; count--) {
                text.Append("  #");
                for (int index = 0; index < 4; index++) {
                    if (_homeBuckets[index].Pods[count] != null) {
                        text.Append(_homeBuckets[index].Pods[count].Name);
                    } else {
                        text.Append(".");
                    }
                    text.Append("#");
                }
                text.AppendLine();
            }
            text.AppendLine("  #########");
            return text.ToString();
        }

        private class Pod {
            public char Name { get; set; }
            public ulong Position { get; set; }
            public int BitShift { get; set; }
            public ulong Energy { get; set; }
            public bool IsHome { get; set; }
            public int HomeBucket { get; set; }
        }

        private class Route {
            public int Start { get; set; }
            public int End { get; set; }
            public ulong Steps { get; set; }
            public List<ulong> SpacesCovered { get; set; }
            public enumRouteType RouteType { get; set; }
            public int Id { get; set; }
        }
    }
}
