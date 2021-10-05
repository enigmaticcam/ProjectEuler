using System.Collections.Generic;

namespace Euler_Logic.Problems {
    public class Problem186 : ProblemBase {
        private ulong _pm = 0;
        private ulong _perc = 0;

        /*
            The key realization here is that although there might be lots of different isolated networks,
            all we care about is whether a number is in the network or not. If it's not in the network,
            we don't care about its connectivity.

            So what I do is look for the prime minister, keeping track only of which numbers can be found
            in which calls (not their connectivity). When I find the PM, then I look for only those
            numbers connected to the PM by means of which calls they can be found in. And then I look for
            other numbers in those calls until I've got them all.

            From this point, it's simply of matter of looking for calls where one is in the network with
            the PM and the other is not. Then I do the same thing for the number that is not. Continue
            doing this until the total number of users in the network is more than or equal to 99%.
         */

        public override string ProblemName {
            get { return "186: Connectedness of a network"; }
        }

        public override string GetAnswer() {
            return Solve1(524287, 99).ToString();
        }

        private Dictionary<ulong, List<ulong>> _calls = new Dictionary<ulong, List<ulong>>();
        private bool[] _isInNetwork;
        private ulong _countInNetwork = 1;
        private ulong _callCount = 0;
        private ulong Solve1(ulong pm, ulong percent) {
            _pm = pm;
            _perc = 1000000 * percent / 100;
            _isInNetwork = new bool[1000000];
            var n = FindPrimeMinister();
            return LookFor99(n + 2);
        }

        private ulong FindPrimeMinister() {
            ulong n = 1;
            do {
                var caller = S(n);
                var callee = S(n + 1);
                if (caller != callee) {
                    AddCall(caller, n);
                    AddCall(callee, n);
                    _callCount++;
                    if (caller == _pm || callee == _pm) {
                        _isInNetwork[caller] = true;
                        _isInNetwork[callee] = true;
                        _countInNetwork = 2;
                        if (caller == _pm) {
                            BuildPMNetwork(callee);
                        } else if (callee == _pm) {
                            BuildPMNetwork(caller);
                        }
                        return n;
                    }
                }
                n += 2;
            } while (true);
        }

        private void BuildPMNetwork(ulong callOut) {
            var callOuts = new List<ulong>() { callOut };
            do {
                var newCallOuts = new List<ulong>();
                foreach (var nextCallout in callOuts) {
                    foreach (var call in _calls[nextCallout]) {
                        var a = S(call);
                        var b = S(call + 1);
                        var other = (a == nextCallout ? b : a);
                        if (!_isInNetwork[other]) {
                            _isInNetwork[other] = true;
                            _countInNetwork++;
                            newCallOuts.Add(other);
                        }
                    }
                }
                callOuts = newCallOuts;
            } while (callOuts.Count > 0);
        }

        private ulong LookFor99(ulong startN) {
            ulong n = startN;
            do {
                var caller = S(n);
                var callee = S(n + 1);
                if (callee != caller) {
                    _callCount++;
                    AddCall(caller, n);
                    AddCall(callee, n);
                    if (_isInNetwork[caller] != _isInNetwork[callee]) {
                        ulong callIn = 0;
                        ulong callOut = 0;
                        if (_isInNetwork[caller]) {
                            callIn = caller;
                            callOut = callee;
                        } else {
                            callIn = callee;
                            callOut = caller;
                        }
                        _isInNetwork[callOut] = true;
                        _countInNetwork++;
                        BuildPMNetwork(callOut);
                        if (_countInNetwork >= _perc) {
                            return _callCount;
                        }
                    }
                }
                n += 2;
            } while (true);
        }

        private void AddCall(ulong call, ulong n) {
            if (!_calls.ContainsKey(call)) {
                _calls.Add(call, new List<ulong>());
            }
            _calls[call].Add(n);
        }

        private Dictionary<ulong, ulong> _s = new Dictionary<ulong, ulong>();
        private ulong S(ulong n) {
            if (!_s.ContainsKey(n)) {
                if (n <= 55) {
                    _s.Add(n, (100003 - 200003 * n + 300007 * n * n * n) % 1000000);
                } else {
                    _s.Add(n, (S(n - 24) + S(n - 55)) % 1000000);
                }
            }
            return _s[n];
        }
    }
}
