using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems.AdventOfCode.Y2021 {
    public class Problem23 : AdventOfCodeBase {
        private List<Pod> _pods;
        private Pod[] _locations;
        private Dictionary<ulong, List<Route>> _routes; 
        private Dictionary<ulong, ulong> _hash;
        private ulong _best = ulong.MaxValue;
        private HomeBucket[] _homeBuckets;
        private List<Route> _routesHomeToHome;
        private List<Route> _routesHomeToHallway;
        private List<Route> _routesHallwayToHome;

        public override string ProblemName {
            get { return "Advent of Code 2021: 23"; }
        }

        public override string GetAnswer() {
            return Answer1(Input()).ToString();
        }

        //private ulong Answer1(List<string> input) {
        //    _hash = new Dictionary<ulong, ulong>();
        //    SetPods(input);
        //    SetLocations();
        //    SetRoutes();
        //    var result = GetRecurisveCount(BuildKey(), 0, 1);
        //    return result;
        //}

        private ulong Answer1(List<string> input) {
            _locations = new Pod[7];
            SetHomeBuckets(false);
            GetPods(input, false);
            SetRoutes2();
            return GetRecursiveCount2(BuildKey(), 0);
        }

        private ulong GetRecursiveCount2(ulong key, ulong prior) {
            //ulong best = ulong.MaxValue;
            //_hash.Add(key, 0);
            //foreach (var pod in _pods) {
            //    foreach (var route in _routes[pod.Position]) {
            //        if (CanPerformMove(pod, route)) {
            //            PerformMove(pod, route);
            //            ulong sub = pod.Energy * route.Steps;
            //            if (IsSolved()) {
            //                if (sub < best) {
            //                    best = sub;
            //                }
            //                if (sub + prior < _best) {
            //                    _best = sub + prior;
            //                }
            //            } else {
            //                var nextKey = BuildKey();
            //                ulong next = 0;
            //                if (!_hash.ContainsKey(nextKey)) {
            //                    next = GetRecurisveCount(nextKey, sub + prior, count + 1);
            //                } else {
            //                    next = _hash[nextKey];
            //                }
            //                if (next != 0 && next != ulong.MaxValue && sub + next < best) {
            //                    best = sub + next;
            //                    if (prior + best < _best) {
            //                        _best = prior + best;
            //                    }
            //                }
            //            }
            //            UndoMove(pod, route);
            //        }
            //    }
            //}
            //_hash[key] = best;
            //return _hash[key];
            ulong best = ulong.MaxValue;
            _hash.Add(key, 0);
            
            foreach (var route in _routesHomeToHome) {
                bool canMove = false;
                //var pod = _homeBuckets[route.Start].Pods[]
                //if (!_homeBuckets[route.Start].IsComplete && !_homeBuckets[route.End].IsComplete && _homeBuckets[route.Start].Pods[)
            }
            return 0;
        }

        private void SetRoutes2() {
            _routesHomeToHome = new List<Route>();
            _routesHomeToHome.Add(new Route() { Start = 0, End = 1, SpacesCovered = new List<ulong>() { 2 }, Steps = 4 }); // Start & End are Homebucket indexes
            _routesHomeToHome.Add(new Route() { Start = 0, End = 2, SpacesCovered = new List<ulong>() { 2, 3 }, Steps = 6 });
            _routesHomeToHome.Add(new Route() { Start = 0, End = 3, SpacesCovered = new List<ulong>() { 2, 3, 4 }, Steps = 8 });
            _routesHomeToHome.Add(new Route() { Start = 1, End = 0, SpacesCovered = new List<ulong>() { 2 }, Steps = 4 });
            _routesHomeToHome.Add(new Route() { Start = 1, End = 2, SpacesCovered = new List<ulong>() { 3 }, Steps = 4 });
            _routesHomeToHome.Add(new Route() { Start = 1, End = 3, SpacesCovered = new List<ulong>() { 3, 4 }, Steps = 6 });
            _routesHomeToHome.Add(new Route() { Start = 2, End = 0, SpacesCovered = new List<ulong>() { 3, 2 }, Steps = 6 });
            _routesHomeToHome.Add(new Route() { Start = 2, End = 1, SpacesCovered = new List<ulong>() { 3 }, Steps = 4 });
            _routesHomeToHome.Add(new Route() { Start = 2, End = 3, SpacesCovered = new List<ulong>() { 4 }, Steps = 4 });
            _routesHomeToHome.Add(new Route() { Start = 3, End = 0, SpacesCovered = new List<ulong>() { 4, 3, 2 }, Steps = 8 });
            _routesHomeToHome.Add(new Route() { Start = 3, End = 1, SpacesCovered = new List<ulong>() { 4, 3 }, Steps = 6 });
            _routesHomeToHome.Add(new Route() { Start = 3, End = 2, SpacesCovered = new List<ulong>() { 4 }, Steps = 4 });

            _routesHomeToHallway = new List<Route>();
            _routesHomeToHallway.Add(new Route() { Start = 0, End = 0, SpacesCovered = new List<ulong>() { 1, 0 }, Steps = 3 }); // Start is HomeBucket index, End is position
            _routesHomeToHallway.Add(new Route() { Start = 0, End = 1, SpacesCovered = new List<ulong>() { 1 }, Steps = 2 });
            _routesHomeToHallway.Add(new Route() { Start = 0, End = 2, SpacesCovered = new List<ulong>() { 2 }, Steps = 2 });
            _routesHomeToHallway.Add(new Route() { Start = 0, End = 3, SpacesCovered = new List<ulong>() { 2, 3 }, Steps = 4 });
            _routesHomeToHallway.Add(new Route() { Start = 0, End = 4, SpacesCovered = new List<ulong>() { 2, 3, 4 }, Steps = 6 });
            _routesHomeToHallway.Add(new Route() { Start = 0, End = 5, SpacesCovered = new List<ulong>() { 2, 3, 4, 5 }, Steps = 8 });
            _routesHomeToHallway.Add(new Route() { Start = 0, End = 6, SpacesCovered = new List<ulong>() { 2, 3, 4, 5, 6 }, Steps = 9 });
            _routesHomeToHallway.Add(new Route() { Start = 1, End = 0, SpacesCovered = new List<ulong>() { 2, 1, 0 }, Steps = 5 });
            _routesHomeToHallway.Add(new Route() { Start = 1, End = 1, SpacesCovered = new List<ulong>() { 2, 1 }, Steps = 4 });
            _routesHomeToHallway.Add(new Route() { Start = 1, End = 2, SpacesCovered = new List<ulong>() { 2 }, Steps = 2 });
            _routesHomeToHallway.Add(new Route() { Start = 1, End = 3, SpacesCovered = new List<ulong>() { 3 }, Steps = 2 });
            _routesHomeToHallway.Add(new Route() { Start = 1, End = 4, SpacesCovered = new List<ulong>() { 3, 4 }, Steps = 4 });
            _routesHomeToHallway.Add(new Route() { Start = 1, End = 5, SpacesCovered = new List<ulong>() { 3, 4, 5 }, Steps = 6 });
            _routesHomeToHallway.Add(new Route() { Start = 1, End = 6, SpacesCovered = new List<ulong>() { 3, 4, 5, 6 }, Steps = 7 });
            _routesHomeToHallway.Add(new Route() { Start = 2, End = 0, SpacesCovered = new List<ulong>() { 3, 2, 1, 0 }, Steps = 7 });
            _routesHomeToHallway.Add(new Route() { Start = 2, End = 1, SpacesCovered = new List<ulong>() { 3, 2, 1 }, Steps = 6 });
            _routesHomeToHallway.Add(new Route() { Start = 2, End = 2, SpacesCovered = new List<ulong>() { 3, 2 }, Steps = 4 });
            _routesHomeToHallway.Add(new Route() { Start = 2, End = 3, SpacesCovered = new List<ulong>() { 3 }, Steps = 2 });
            _routesHomeToHallway.Add(new Route() { Start = 2, End = 4, SpacesCovered = new List<ulong>() { 4 }, Steps = 2 });
            _routesHomeToHallway.Add(new Route() { Start = 2, End = 5, SpacesCovered = new List<ulong>() { 4, 5 }, Steps = 4 });
            _routesHomeToHallway.Add(new Route() { Start = 2, End = 6, SpacesCovered = new List<ulong>() { 4, 5, 6 }, Steps = 5 });
            _routesHomeToHallway.Add(new Route() { Start = 3, End = 0, SpacesCovered = new List<ulong>() { 4, 3, 2, 1, 0 }, Steps = 9 });
            _routesHomeToHallway.Add(new Route() { Start = 3, End = 1, SpacesCovered = new List<ulong>() { 4, 3, 2, 1 }, Steps = 8 });
            _routesHomeToHallway.Add(new Route() { Start = 3, End = 2, SpacesCovered = new List<ulong>() { 4, 3, 2 }, Steps = 6 });
            _routesHomeToHallway.Add(new Route() { Start = 3, End = 3, SpacesCovered = new List<ulong>() { 4, 3 }, Steps = 4 });
            _routesHomeToHallway.Add(new Route() { Start = 3, End = 4, SpacesCovered = new List<ulong>() { 4 }, Steps = 2 });
            _routesHomeToHallway.Add(new Route() { Start = 3, End = 5, SpacesCovered = new List<ulong>() { 5 }, Steps = 2 });
            _routesHomeToHallway.Add(new Route() { Start = 3, End = 6, SpacesCovered = new List<ulong>() { 5, 5 }, Steps = 3 });

            _routesHallwayToHome = new List<Route>();
            foreach (var route in _routesHomeToHallway) { // Start is position, End is HomeBucket Index
                _routesHallwayToHome.Add(new Route() { Start = route.End, End = route.Start, SpacesCovered = route.SpacesCovered, Steps = route.Steps });
            }

        }

        private void SetHomeBuckets(bool insertMiddle) {
            int count = (insertMiddle ? 4 : 2);
            _homeBuckets = new HomeBucket[4];
            for (int index = 0; index < 4; index++) {
                _homeBuckets[index] = new HomeBucket() { Pods = new Pod[count], Positions = new ulong[count], NextOpen = -1 };
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
                _pods.Add(new Pod() { BitShift = 4, Name = input[3][3], Position = 8 });
                _pods.Add(new Pod() { BitShift = 8, Name = input[4][3], Position = 9 });
                _pods.Add(new Pod() { BitShift = 12, Name = input[5][3], Position = 10 });
                _pods.Add(new Pod() { BitShift = 16, Name = input[2][5], Position = 11 });
                _pods.Add(new Pod() { BitShift = 20, Name = input[3][5], Position = 12 });
                _pods.Add(new Pod() { BitShift = 24, Name = input[4][5], Position = 13 });
                _pods.Add(new Pod() { BitShift = 28, Name = input[5][5], Position = 14 });
                _pods.Add(new Pod() { BitShift = 32, Name = input[2][7], Position = 15 });
                _pods.Add(new Pod() { BitShift = 36, Name = input[3][7], Position = 16 });
                _pods.Add(new Pod() { BitShift = 40, Name = input[4][7], Position = 17 });
                _pods.Add(new Pod() { BitShift = 44, Name = input[5][7], Position = 18 });
                _pods.Add(new Pod() { BitShift = 48, Name = input[2][9], Position = 19 });
                _pods.Add(new Pod() { BitShift = 52, Name = input[3][9], Position = 20 });
                _pods.Add(new Pod() { BitShift = 56, Name = input[4][9], Position = 21 });
                _pods.Add(new Pod() { BitShift = 60, Name = input[5][9], Position = 22 });
            }
            int pod = 0;
            int max = (insertMiddle ? 4 : 2);
            for (int bucket = 0; bucket < max; bucket++) {
                for (int spot = 0; spot < max; spot++) {
                    _homeBuckets[bucket].Pods[spot] = _pods[pod];
                    _homeBuckets[bucket].Positions[spot] = _pods[pod].Position;
                    pod++;
                }
            }
        }

        private class HomeBucket {
            public Pod[] Pods { get; set; }
            public ulong[] Positions { get; set; }
            public int NextOpen { get; set; }
            public bool IsComplete { get; set; }
        }

        /*
         ---------------------------
                    Part 1
        ---------------------------
         */

        //private ulong GetRecurisveCount(ulong key, ulong prior, int count) {
        //    ulong best = ulong.MaxValue;
        //    _hash.Add(key, 0);
        //    foreach (var pod in _pods) {
        //        foreach (var route in _routes[pod.Position]) {
        //            if (CanPerformMove(pod, route)) {
        //                PerformMove(pod, route);
        //                ulong sub = pod.Energy * route.Steps;
        //                if (IsSolved()) {
        //                    if (sub < best) {
        //                        best = sub;
        //                    }
        //                    if (sub + prior < _best) {
        //                        _best = sub + prior;
        //                    }
        //                } else {
        //                    var nextKey = BuildKey();
        //                    ulong next = 0;
        //                    if (!_hash.ContainsKey(nextKey)) {
        //                        next = GetRecurisveCount(nextKey, sub + prior, count + 1);
        //                    } else {
        //                        next = _hash[nextKey];
        //                    }
        //                    if (next != 0 && next != ulong.MaxValue && sub + next < best) {
        //                        best = sub + next;
        //                        if (prior + best < _best) {
        //                            _best = prior + best;
        //                        }
        //                    }
        //                }
        //                UndoMove(pod, route);
        //            }
        //        }
        //    }
        //    _hash[key] = best;
        //    return _hash[key];
        //}

        //public bool IsSolved() {
        //    var result =
        //        _locations[7] != null && _locations[7].Name == 'A'
        //        && _locations[8] != null && _locations[8].Name == 'A'
        //        && _locations[9] != null && _locations[9].Name == 'B'
        //        && _locations[10] != null && _locations[10].Name == 'B'
        //        && _locations[11] != null && _locations[11].Name == 'C'
        //        && _locations[12] != null && _locations[12].Name == 'C'
        //        && _locations[13] != null && _locations[13].Name == 'D'
        //        && _locations[14] != null && _locations[14].Name == 'D';
        //    return result;
        //}

        //private void PerformMove(Pod pod, Route route) {
        //    _locations[route.Start] = null;
        //    _locations[route.End] = pod;
        //    pod.Position = route.End;
        //    pod.IsHome = pod.Position == pod.PositionHome1 || pod.Position == pod.PositionHome2;
        //}

        //private void UndoMove(Pod pod, Route route) {
        //    _locations[route.Start] = pod;
        //    _locations[route.End] = null;
        //    pod.Position = route.Start;
        //    pod.IsHome = pod.Position == pod.PositionHome1 || pod.Position == pod.PositionHome2;
        //}

        //private bool CanPerformMove(Pod pod, Route route) {
        //    if (route.End >= 7 && route.End != pod.PositionHome1 && route.End != pod.PositionHome2) {
        //        return false;
        //    }
        //    if (route.End == pod.PositionHome1 && (_locations[pod.PositionHome2] == null || !_locations[pod.PositionHome2].IsHome)) {
        //        return false;
        //    }
        //    if (route.Start == pod.PositionHome1 && _locations[pod.PositionHome2] != null && _locations[pod.PositionHome2].IsHome) {
        //        return false;
        //    }
        //    foreach (var space in route.SpacesCovered) {
        //        if (_locations[space] != null) {
        //            return false;
        //        }
        //    }
        //    return true;
        //}

        //private void SetRoutes() {
        //    _routes = new Dictionary<ulong, List<Route>>();
        //    for (ulong position = 0; position <= 14; position++) {
        //        _routes.Add(position, new List<Route>());
        //    }
        //    _routes[0].Add(new Route() { Start = 0, End = 7, SpacesCovered = new List<ulong>() { 1, 7 }, Steps = 3 });
        //    _routes[0].Add(new Route() { Start = 0, End = 8, SpacesCovered = new List<ulong>() { 1, 7, 8 }, Steps = 4 });
        //    _routes[0].Add(new Route() { Start = 0, End = 9, SpacesCovered = new List<ulong>() { 1, 2, 9 }, Steps = 5 });
        //    _routes[0].Add(new Route() { Start = 0, End = 10, SpacesCovered = new List<ulong>() { 1, 2, 9, 10 }, Steps = 6 });
        //    _routes[0].Add(new Route() { Start = 0, End = 11, SpacesCovered = new List<ulong>() { 1, 2, 3, 11 }, Steps = 7 });
        //    _routes[0].Add(new Route() { Start = 0, End = 12, SpacesCovered = new List<ulong>() { 1, 2, 3, 11, 12 }, Steps = 8 });
        //    _routes[0].Add(new Route() { Start = 0, End = 13, SpacesCovered = new List<ulong>() { 1, 2, 3, 4, 13 }, Steps = 9 });
        //    _routes[0].Add(new Route() { Start = 0, End = 13, SpacesCovered = new List<ulong>() { 1, 2, 3, 4, 13, 14 }, Steps = 10 });

        //    _routes[1].Add(new Route() { Start = 1, End = 7, SpacesCovered = new List<ulong>() { 7 }, Steps = 2 });
        //    _routes[1].Add(new Route() { Start = 1, End = 8, SpacesCovered = new List<ulong>() { 7, 8 }, Steps = 3 });
        //    _routes[1].Add(new Route() { Start = 1, End = 9, SpacesCovered = new List<ulong>() { 2, 9 }, Steps = 4 });
        //    _routes[1].Add(new Route() { Start = 1, End = 10, SpacesCovered = new List<ulong>() { 2, 9, 10 }, Steps = 5 });
        //    _routes[1].Add(new Route() { Start = 1, End = 11, SpacesCovered = new List<ulong>() { 2, 3, 11 }, Steps = 6 });
        //    _routes[1].Add(new Route() { Start = 1, End = 12, SpacesCovered = new List<ulong>() { 2, 3, 11, 12 }, Steps = 7 });
        //    _routes[1].Add(new Route() { Start = 1, End = 13, SpacesCovered = new List<ulong>() { 2, 3, 4, 13 }, Steps = 8 });
        //    _routes[1].Add(new Route() { Start = 1, End = 14, SpacesCovered = new List<ulong>() { 2, 3, 4, 13, 14 }, Steps = 9 });

        //    _routes[2].Add(new Route() { Start = 2, End = 7, SpacesCovered = new List<ulong>() { 7 }, Steps = 2 });
        //    _routes[2].Add(new Route() { Start = 2, End = 8, SpacesCovered = new List<ulong>() { 7, 8 }, Steps = 3 });
        //    _routes[2].Add(new Route() { Start = 2, End = 9, SpacesCovered = new List<ulong>() { 9 }, Steps = 2 });
        //    _routes[2].Add(new Route() { Start = 2, End = 10, SpacesCovered = new List<ulong>() { 9, 10 }, Steps = 3 });
        //    _routes[2].Add(new Route() { Start = 2, End = 11, SpacesCovered = new List<ulong>() { 3, 11 }, Steps = 4 });
        //    _routes[2].Add(new Route() { Start = 2, End = 12, SpacesCovered = new List<ulong>() { 3, 11, 12 }, Steps = 5 });
        //    _routes[2].Add(new Route() { Start = 2, End = 13, SpacesCovered = new List<ulong>() { 3, 4, 13 }, Steps = 6 });
        //    _routes[2].Add(new Route() { Start = 2, End = 14, SpacesCovered = new List<ulong>() { 3, 4, 13, 14 }, Steps = 7 });

        //    _routes[3].Add(new Route() { Start = 3, End = 7, SpacesCovered = new List<ulong>() { 2, 7 }, Steps = 4 });
        //    _routes[3].Add(new Route() { Start = 3, End = 8, SpacesCovered = new List<ulong>() { 2, 7, 8 }, Steps = 5 });
        //    _routes[3].Add(new Route() { Start = 3, End = 9, SpacesCovered = new List<ulong>() { 9 }, Steps = 2 });
        //    _routes[3].Add(new Route() { Start = 3, End = 10, SpacesCovered = new List<ulong>() { 9, 10 }, Steps = 3 });
        //    _routes[3].Add(new Route() { Start = 3, End = 11, SpacesCovered = new List<ulong>() { 11 }, Steps = 2 });
        //    _routes[3].Add(new Route() { Start = 3, End = 12, SpacesCovered = new List<ulong>() { 11, 12 }, Steps = 3 });
        //    _routes[3].Add(new Route() { Start = 3, End = 13, SpacesCovered = new List<ulong>() { 4, 13 }, Steps = 4 });
        //    _routes[3].Add(new Route() { Start = 3, End = 14, SpacesCovered = new List<ulong>() { 4, 13, 14 }, Steps = 5 });

        //    _routes[4].Add(new Route() { Start = 4, End = 7, SpacesCovered = new List<ulong>() { 3, 2, 7 }, Steps = 6 });
        //    _routes[4].Add(new Route() { Start = 4, End = 8, SpacesCovered = new List<ulong>() { 3, 2, 7, 8}, Steps = 7 });
        //    _routes[4].Add(new Route() { Start = 4, End = 9, SpacesCovered = new List<ulong>() { 3, 9 }, Steps = 4 });
        //    _routes[4].Add(new Route() { Start = 4, End = 10, SpacesCovered = new List<ulong>() { 3, 9, 10 }, Steps = 5 });
        //    _routes[4].Add(new Route() { Start = 4, End = 11, SpacesCovered = new List<ulong>() { 11 }, Steps = 2 });
        //    _routes[4].Add(new Route() { Start = 4, End = 12, SpacesCovered = new List<ulong>() { 11, 12 }, Steps = 3 });
        //    _routes[4].Add(new Route() { Start = 4, End = 13, SpacesCovered = new List<ulong>() { 13 }, Steps = 2 });
        //    _routes[4].Add(new Route() { Start = 4, End = 14, SpacesCovered = new List<ulong>() { 13, 14 }, Steps = 3 });

        //    _routes[5].Add(new Route() { Start = 5, End = 7, SpacesCovered = new List<ulong>() { 4, 3, 2, 7 }, Steps = 8 });
        //    _routes[5].Add(new Route() { Start = 5, End = 8, SpacesCovered = new List<ulong>() { 4, 3, 2, 7, 8 }, Steps = 9 });
        //    _routes[5].Add(new Route() { Start = 5, End = 9, SpacesCovered = new List<ulong>() { 4, 3, 9 }, Steps = 6 });
        //    _routes[5].Add(new Route() { Start = 5, End = 10, SpacesCovered = new List<ulong>() { 4, 3, 9, 10 }, Steps = 7 });
        //    _routes[5].Add(new Route() { Start = 5, End = 11, SpacesCovered = new List<ulong>() { 4, 11 }, Steps = 4 });
        //    _routes[5].Add(new Route() { Start = 5, End = 12, SpacesCovered = new List<ulong>() { 4, 11, 12 }, Steps = 5 });
        //    _routes[5].Add(new Route() { Start = 5, End = 13, SpacesCovered = new List<ulong>() { 13 }, Steps = 2 });
        //    _routes[5].Add(new Route() { Start = 5, End = 14, SpacesCovered = new List<ulong>() { 13, 14 }, Steps = 3 });

        //    _routes[6].Add(new Route() { Start = 6, End = 7, SpacesCovered = new List<ulong>() { 5, 4, 3, 2, 7 }, Steps = 9 });
        //    _routes[6].Add(new Route() { Start = 6, End = 8, SpacesCovered = new List<ulong>() { 5, 4, 3, 2, 7, 8 }, Steps = 10 });
        //    _routes[6].Add(new Route() { Start = 6, End = 9, SpacesCovered = new List<ulong>() { 5, 4, 3, 9 }, Steps = 7 });
        //    _routes[6].Add(new Route() { Start = 6, End = 10, SpacesCovered = new List<ulong>() { 5, 4, 3, 9, 10 }, Steps = 8 });
        //    _routes[6].Add(new Route() { Start = 6, End = 11, SpacesCovered = new List<ulong>() { 5, 4, 11 }, Steps = 5 });
        //    _routes[6].Add(new Route() { Start = 6, End = 12, SpacesCovered = new List<ulong>() { 5, 4, 11, 12}, Steps = 6 });
        //    _routes[6].Add(new Route() { Start = 6, End = 13, SpacesCovered = new List<ulong>() { 5, 13 }, Steps = 3 });
        //    _routes[6].Add(new Route() { Start = 6, End = 14, SpacesCovered = new List<ulong>() { 5, 13, 14}, Steps = 4 });

        //    foreach (var route in _routes.Values.SelectMany(x => x)) {
        //        var newRoute = new Route() {
        //            Start = route.End,
        //            End = route.Start,
        //            SpacesCovered = new List<ulong>(route.SpacesCovered),
        //            Steps = route.Steps
        //        };
        //        newRoute.SpacesCovered.Remove(route.End);
        //        newRoute.SpacesCovered.Add(route.Start);
        //        if (!_routes.ContainsKey(newRoute.Start)) {
        //            _routes.Add(newRoute.Start, new List<Route>());
        //        }
        //        _routes[newRoute.Start].Add(newRoute);
        //    }

        //    _routes[7].Add(new Route() { Start = 7, End = 9, SpacesCovered = new List<ulong>() { 2, 9 }, Steps = 4 });
        //    _routes[7].Add(new Route() { Start = 7, End = 10, SpacesCovered = new List<ulong>() { 2, 9, 10 }, Steps = 5 });
        //    _routes[7].Add(new Route() { Start = 7, End = 11, SpacesCovered = new List<ulong>() { 2, 3, 11 }, Steps = 6 });
        //    _routes[7].Add(new Route() { Start = 7, End = 12, SpacesCovered = new List<ulong>() { 2, 3, 11, 12 }, Steps = 7 });
        //    _routes[7].Add(new Route() { Start = 7, End = 13, SpacesCovered = new List<ulong>() { 2, 3, 4, 13 }, Steps = 8 });
        //    _routes[7].Add(new Route() { Start = 7, End = 14, SpacesCovered = new List<ulong>() { 2, 3, 4, 13, 14 }, Steps = 9 });

        //    _routes[8].Add(new Route() { Start = 8, End = 9, SpacesCovered = new List<ulong>() { 7, 2, 9 }, Steps = 5 });
        //    _routes[8].Add(new Route() { Start = 8, End = 10, SpacesCovered = new List<ulong>() { 7, 2, 9, 10 }, Steps = 6 });
        //    _routes[8].Add(new Route() { Start = 8, End = 11, SpacesCovered = new List<ulong>() { 7, 2, 3, 11 }, Steps = 7 });
        //    _routes[8].Add(new Route() { Start = 8, End = 12, SpacesCovered = new List<ulong>() { 7, 2, 3, 11, 12 }, Steps = 8 });
        //    _routes[8].Add(new Route() { Start = 8, End = 13, SpacesCovered = new List<ulong>() { 7, 2, 3, 4, 13 }, Steps = 9 });
        //    _routes[8].Add(new Route() { Start = 8, End = 14, SpacesCovered = new List<ulong>() { 7, 2, 3, 4, 13, 14 }, Steps = 10 });

        //    _routes[9].Add(new Route() { Start = 9, End = 7, SpacesCovered = new List<ulong>() { 2, 7 }, Steps = 4 });
        //    _routes[9].Add(new Route() { Start = 9, End = 8, SpacesCovered = new List<ulong>() { 2, 7, 8 }, Steps = 5 });
        //    _routes[9].Add(new Route() { Start = 9, End = 11, SpacesCovered = new List<ulong>() { 3, 11 }, Steps = 4 });
        //    _routes[9].Add(new Route() { Start = 9, End = 12, SpacesCovered = new List<ulong>() { 3, 11, 12 }, Steps = 5 });
        //    _routes[9].Add(new Route() { Start = 9, End = 13, SpacesCovered = new List<ulong>() { 3, 4, 13 }, Steps = 6 });
        //    _routes[9].Add(new Route() { Start = 9, End = 14, SpacesCovered = new List<ulong>() { 3, 4, 13, 14 }, Steps = 7 });

        //    _routes[10].Add(new Route() { Start = 10, End = 7, SpacesCovered = new List<ulong>() { 9, 2, 7 }, Steps = 5 });
        //    _routes[10].Add(new Route() { Start = 10, End = 8, SpacesCovered = new List<ulong>() { 9, 2, 7, 8 }, Steps = 6 });
        //    _routes[10].Add(new Route() { Start = 10, End = 11, SpacesCovered = new List<ulong>() { 9, 3, 11 }, Steps = 5 });
        //    _routes[10].Add(new Route() { Start = 10, End = 12, SpacesCovered = new List<ulong>() { 9, 3, 11, 12 }, Steps = 6 });
        //    _routes[10].Add(new Route() { Start = 10, End = 13, SpacesCovered = new List<ulong>() { 9, 3, 4, 13 }, Steps = 7 });
        //    _routes[10].Add(new Route() { Start = 10, End = 14, SpacesCovered = new List<ulong>() { 9, 3, 4, 13, 14 }, Steps = 8 });

        //    _routes[11].Add(new Route() { Start = 11, End = 7, SpacesCovered = new List<ulong>() { 3, 2, 7 }, Steps = 6 });
        //    _routes[11].Add(new Route() { Start = 11, End = 8, SpacesCovered = new List<ulong>() { 3, 2, 7, 8 }, Steps = 7 });
        //    _routes[11].Add(new Route() { Start = 11, End = 9, SpacesCovered = new List<ulong>() { 3, 9 }, Steps = 4 });
        //    _routes[11].Add(new Route() { Start = 11, End = 10, SpacesCovered = new List<ulong>() { 3, 9, 10 }, Steps = 5 });
        //    _routes[11].Add(new Route() { Start = 11, End = 13, SpacesCovered = new List<ulong>() { 4, 13 }, Steps = 4 });
        //    _routes[11].Add(new Route() { Start = 11, End = 14, SpacesCovered = new List<ulong>() { 4, 13, 14 }, Steps = 5 });

        //    _routes[12].Add(new Route() { Start = 12, End = 7, SpacesCovered = new List<ulong>() { 11, 3, 2, 7 }, Steps = 7 });
        //    _routes[12].Add(new Route() { Start = 12, End = 8, SpacesCovered = new List<ulong>() { 11, 3, 2, 7, 8 }, Steps = 8 });
        //    _routes[12].Add(new Route() { Start = 12, End = 9, SpacesCovered = new List<ulong>() { 11, 3, 9 }, Steps = 5 });
        //    _routes[12].Add(new Route() { Start = 12, End = 10, SpacesCovered = new List<ulong>() { 11, 3, 9, 10 }, Steps = 6 });
        //    _routes[12].Add(new Route() { Start = 12, End = 13, SpacesCovered = new List<ulong>() { 11, 4, 13 }, Steps = 5 });
        //    _routes[12].Add(new Route() { Start = 12, End = 14, SpacesCovered = new List<ulong>() { 11, 4, 13, 14 }, Steps = 6 });

        //    _routes[13].Add(new Route() { Start = 13, End = 7, SpacesCovered = new List<ulong>() { 4, 3, 2, 7 }, Steps = 8 });
        //    _routes[13].Add(new Route() { Start = 13, End = 8, SpacesCovered = new List<ulong>() { 4, 3, 2, 7, 8 }, Steps = 9 });
        //    _routes[13].Add(new Route() { Start = 13, End = 9, SpacesCovered = new List<ulong>() { 4, 3, 9 }, Steps = 6 });
        //    _routes[13].Add(new Route() { Start = 13, End = 10, SpacesCovered = new List<ulong>() { 4, 3, 9, 10}, Steps = 7 });
        //    _routes[13].Add(new Route() { Start = 13, End = 11, SpacesCovered = new List<ulong>() { 4, 11 }, Steps = 4 });
        //    _routes[13].Add(new Route() { Start = 13, End = 12, SpacesCovered = new List<ulong>() { 4, 11, 12 }, Steps = 5 });

        //    _routes[14].Add(new Route() { Start = 14, End = 7, SpacesCovered = new List<ulong>() { 13, 4, 3, 2, 7 }, Steps = 9 });
        //    _routes[14].Add(new Route() { Start = 14, End = 8, SpacesCovered = new List<ulong>() { 13, 4, 3, 2, 7, 8 }, Steps = 10 });
        //    _routes[14].Add(new Route() { Start = 14, End = 9, SpacesCovered = new List<ulong>() { 13, 4, 3, 9 }, Steps = 7 });
        //    _routes[14].Add(new Route() { Start = 14, End = 10, SpacesCovered = new List<ulong>() { 13, 4, 3, 9, 10 }, Steps = 8 });
        //    _routes[14].Add(new Route() { Start = 14, End = 11, SpacesCovered = new List<ulong>() { 13, 4, 11 }, Steps = 5 });
        //    _routes[14].Add(new Route() { Start = 14, End = 12, SpacesCovered = new List<ulong>() { 13, 4, 11, 12 }, Steps = 6 });

        //    foreach (var key in _routes.Keys.ToList()) {
        //        _routes[key] = _routes[key].OrderBy(x => x.Steps).ToList();
        //    }

        //}

        private ulong BuildKey() {
            ulong key = 0;
            foreach (var pod in _pods) {
                var subKey = pod.Position;
                subKey <<= pod.BitShift;
                key += subKey;
            }
            return key;
        }

        //private void SetLocations() {
        //    _locations = new Pod[15];
        //    foreach (var pod in _pods) {
        //        _locations[pod.Position] = pod;
        //    }
        //}

        //private void SetPods(List<string> input) {
        //    _pods = new List<Pod>();
        //    _pods.Add(new Pod() { BitShift = 0, Name = input[2][3], Position = 7 });
        //    _pods.Add(new Pod() { BitShift = 4, Name = input[2][5], Position = 9 });
        //    _pods.Add(new Pod() { BitShift = 8, Name = input[2][7], Position = 11 });
        //    _pods.Add(new Pod() { BitShift = 12, Name = input[2][9], Position = 13 });
        //    _pods.Add(new Pod() { BitShift = 16, Name = input[3][3], Position = 8 });
        //    _pods.Add(new Pod() { BitShift = 20, Name = input[3][5], Position = 10 });
        //    _pods.Add(new Pod() { BitShift = 24, Name = input[3][7], Position = 12 });
        //    _pods.Add(new Pod() { BitShift = 28, Name = input[3][9], Position = 14 });
        //    _pods.ForEach(x => GetNameSpecifics(x));
        //    _pods = _pods.OrderBy(x => x.Name).ToList();
        //}

        private void GetNameSpecifics(Pod pod) {
            switch (pod.Name) {
                case 'A':
                    pod.Energy = 1;
                    pod.PositionHome1 = 7;
                    pod.PositionHome2 = 8;
                    break;
                case 'B':
                    pod.Energy = 10;
                    pod.PositionHome1 = 9;
                    pod.PositionHome2 = 10;
                    break;
                case 'C':
                    pod.Energy = 100;
                    pod.PositionHome1 = 11;
                    pod.PositionHome2 = 12;
                    break;
                case 'D':
                    pod.Energy = 1000;
                    pod.PositionHome1 = 13;
                    pod.PositionHome2 = 14;
                    break;
            }
        }

        private class Pod {
            public char Name { get; set; }
            public ulong Position { get; set; }
            public int BitShift { get; set; }
            public ulong Energy { get; set; }
            public ulong PositionHome1 { get; set; }
            public ulong PositionHome2 { get; set; }
            public bool IsHome { get; set; }
        }

        private class Route {
            public ulong Start { get; set; }
            public ulong End { get; set; }
            public ulong Steps { get; set; }
            public List<ulong> SpacesCovered { get; set; }
        }
    }
}
