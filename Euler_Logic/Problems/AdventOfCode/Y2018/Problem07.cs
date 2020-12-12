using System;
using System.Collections.Generic;
using System.Linq;

namespace Euler_Logic.Problems.AdventOfCode.Y2018 {
    public class Problem07 : AdventOfCodeBase {
        public override string ProblemName {
            get { return "Advent of Code 2018: 7"; }
        }

        public override string GetAnswer() {
            return Answer2();
        }

        private string Answer1() {
            var steps = GetSteps();
            var priors = GetPriors(steps);
            return Rearrange(steps, priors);
        }

        private string Answer2() {
            var steps = GetSteps();
            var priors = GetPriors(steps);
            return GetHelp(steps, priors, 5, 60).ToString();
        }

        private HashSet<int> _queueFinished;
        private List<int> _queueUnfinished;
        private List<Worker> _workers;
        private int _subtract;
        private ulong GetHelp(List<StepOrder> steps, Dictionary<int, List<int>> priors, int workerCount, int timePerStep) {
            ulong seconds = 0;
            _queueFinished = new HashSet<int>();
            _queueUnfinished = GetQueueUnfinished(steps);
            _workers = GetWorkers(workerCount);
            _subtract = priors.Keys.Min() - 1;
            WorkerGetNextStep(_workers[0], priors, timePerStep);
            do {
                if (ReduceSeconds()) {
                    foreach (var worker in _workers) {
                        if (!worker.IsActive) {
                            WorkerGetNextStep(worker, priors, timePerStep);
                        }
                    }
                }
                seconds++;
            } while (_workers.Where(x => x.IsActive).Count() > 0);
            return seconds;
        }

        private bool ReduceSeconds() {
            bool didWorkerFinish = false;
            foreach (var worker in _workers) {
                if (worker.IsActive) {
                    worker.SecondsLeft--;
                    if (worker.SecondsLeft == 0) {
                        didWorkerFinish = true;
                        worker.IsActive = false;
                        _queueFinished.Add(worker.Step);
                    }
                }
            }
            return didWorkerFinish;
        }

        private void WorkerGetNextStep(Worker worker, Dictionary<int, List<int>> priors, int timePerStep) {
            bool canStart = false;
            foreach (var step in _queueUnfinished) {
                canStart = true;
                if (priors.ContainsKey(step)) {
                    foreach (var prior in priors[step]) {
                        if (!_queueFinished.Contains(prior)) {
                            canStart = false;
                            break;
                        }
                    }
                }
                if (canStart) {
                    worker.SecondsLeft = step - _subtract + timePerStep;
                    worker.Step = step;
                    worker.IsActive = true;
                    break;
                }
            }
            if (canStart) {
                _queueUnfinished.Remove(worker.Step);
            }
        }

        private List<int> GetQueueUnfinished(List<StepOrder> steps) {
            var hash = new HashSet<int>();
            steps.ForEach(x => {
                hash.Add(x.First);
                hash.Add(x.Last);
            });
            return hash.ToList();
        }

        private List<Worker> GetWorkers(int workerCount) {
            var workers = new List<Worker>();
            for (int count = 1; count <= workerCount; count++) {
                workers.Add(new Worker());
            }
            return workers;
        }

        private string Rearrange(List<StepOrder> steps, Dictionary<int, List<int>> priors) {
            var result = new char[26];
            var hash = new HashSet<int>();
            for (int count = 1; count <= 26; count++) {
                for (int step = (int)'A'; step <= (int)'Z'; step++) {
                    if (!hash.Contains(step)) {
                        bool isGood = true;
                        if (priors.ContainsKey(step)) {
                            foreach (var prior in priors[step]) {
                                if (!hash.Contains(prior)) {
                                    isGood = false;
                                    break;
                                }
                            }
                        }
                        if (isGood) {
                            result[count - 1] = (char)step;
                            hash.Add(step);
                            break;
                        }
                    }
                }
            }
            return new string(result);
        }

        private Dictionary<int, List<int>> GetPriors(List<StepOrder> steps) {
            var prior = new Dictionary<int, List<int>>();
            foreach (var step in steps) {
                if (!prior.ContainsKey(step.Last)) {
                    prior.Add(step.Last, new List<int>());
                }
                prior[step.Last].Add(step.First);
            }
            return prior;
        }

        private List<StepOrder> GetSteps() {
            return Input().Select(x => {
                return new StepOrder() {
                    First = x[5],
                    Last = x[36]
                };
            }).ToList();
        }

        private List<string> TestInput() {
            var input = new List<string>() {
                "Step C must be finished before step A can begin.",
                "Step C must be finished before step F can begin.",
                "Step A must be finished before step B can begin.",
                "Step A must be finished before step D can begin.",
                "Step B must be finished before step E can begin.",
                "Step D must be finished before step E can begin.",
                "Step F must be finished before step E can begin."
            };
            var random = new Random();
            for (int index = 0; index < input.Count; index++) {
                var swap = random.Next(0, index + 1);
                var temp = input[index];
                input[index] = input[swap];
                input[swap] = temp;
            }
            return input;
        }

        private class StepOrder {
            public int First { get; set; }
            public int Last { get; set; }
        }

        private class Worker {
            public int Step { get; set; }
            public int SecondsLeft { get; set; }
            public bool IsActive { get; set; }
        }
    }
}
