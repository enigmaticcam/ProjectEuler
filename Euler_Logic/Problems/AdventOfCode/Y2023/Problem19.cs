using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems.AdventOfCode.Y2023
{
    public class Problem19 : AdventOfCodeBase
    {
        public override string ProblemName => "Advent of Code 2023: 19";

        public override string GetAnswer()
        {
            //return Answer1(Input_Test(1)).ToString();
            return Answer1(Input()).ToString();
        }

        public override string GetAnswer2()
        {
            //return Answer2(Input_Test(1)).ToString();
            return Answer2(Input()).ToString();
        }

        private ulong Answer1(List<string> input)
        {
            var state = GetState(input);
            return ApplyRules(state);
        }

        private ulong Answer2(List<string> input)
        {
            var state = GetState(input);
            return BruteForce(state);
        }

        private ulong BruteForce(State state)
        {
            var range = new Range()
            {
                A = new ulong[2] { 1, 4000 },
                M = new ulong[2] { 1, 4000 },
                S = new ulong[2] { 1, 4000 },
                X = new ulong[2] { 1, 4000 }
            };
            var inEx = Range.Copy(range);
            Recursive(state, state.RuleSets["in"], range);
            return CalcAllRanges();
        }

        private ulong CalcAllRanges()
        {
            ulong sum = 0;
            foreach (var range in _ranges)
            {
                sum +=
                    (range.A[1] - range.A[0] + 1)
                    * (range.M[1] - range.M[0] + 1)
                    * (range.S[1] - range.S[0] + 1)
                    * (range.X[1] - range.X[0] + 1);
            }
            return sum;
        }

        private string Output()
        {
            var text = new StringBuilder();
            foreach (var range in _ranges)
            {
                text.AppendLine($"(x){range.X[0]}-{range.X[1]},(m){range.M[0]}-{range.M[1]},(a){range.A[0]}-{range.A[1]},(s){range.S[0]}-{range.S[1]}");
            }
            return text.ToString();
        }

        private List<Range> _ranges = new List<Range>();
        private void Recursive(State state, RuleSet ruleSet, Range range)
        {
            var subRange = Range.Copy(range);
            foreach (var rule in ruleSet.Rules.Take(ruleSet.Rules.Count - 1))
            {
                if (rule.Result == enumResult.Accepted)
                {
                    var newRange = Range.Copy(subRange);
                    AdjustRange(rule, newRange, false);
                    _ranges.Add(newRange);
                }
                else if (rule.Result == enumResult.SendToWorkflow)
                {
                    var newRange = Range.Copy(subRange);
                    AdjustRange(rule, newRange, false);
                    Recursive(state, state.RuleSets[rule.ToWorkFlow], newRange);
                }
                AdjustRange(rule, subRange, true);
            }
            var last = ruleSet.Rules.Last();
            if (last.Result == enumResult.Accepted)
            {
                _ranges.Add(Range.Copy(subRange));
            }
            else if (last.Result == enumResult.SendToWorkflow)
            {
                Recursive(state, state.RuleSets[last.ToWorkFlow], Range.Copy(subRange));
            }
        }

        private void AdjustRange(Rule rule, Range range, bool reverse)
        {
            switch (rule.PropName)
            {
                case "x":
                    AdjustRange(rule, range, reverse, range.X);
                    break;
                case "m":
                    AdjustRange(rule, range, reverse, range.M);
                    break;
                case "a":
                    AdjustRange(rule, range, reverse, range.A);
                    break;
                case "s":
                    AdjustRange(rule, range, reverse, range.S);
                    break;
            }
        }

        private void AdjustRange(Rule rule, Range range, bool reverse, ulong[] prop)
        {
            if (!reverse)
            {
                if (rule.Comparison == enumComparison.GreaterThan)
                {
                    if (prop[0] < rule.Value + 1)
                        prop[0] = rule.Value + 1;
                } 
                else
                {
                    if (prop[1] > rule.Value - 1)
                        prop[1] = rule.Value - 1;
                }
            }
            else
            {
                if (rule.Comparison == enumComparison.GreaterThan)
                {
                    if (prop[1] > rule.Value)
                        prop[1] = rule.Value;
                }
                else
                {
                    if (prop[0] < rule.Value)
                        prop[0] = rule.Value;
                }
            }
            
        }

        private ulong ApplyRules(State state)
        {
            ulong sum = 0;
            var firstFlow = state.RuleSets["in"];
            foreach (var part in state.Parts)
            {
                var currentFlow = firstFlow;
                bool keepGoing = true;
                do
                {
                    foreach (var rule in currentFlow.Rules)
                    {
                        var result = rule.RuleFunc(part);
                        if (result.ResultValue == enumResult.Accepted)
                        {
                            sum += part.A + part.M + part.S + part.X;
                            keepGoing = false;
                            break;
                        }
                        else if (result.ResultValue == enumResult.Rejected)
                        {
                            keepGoing = false;
                            break;
                        }
                        else if (result.ResultValue == enumResult.SendToWorkflow)
                        {
                            currentFlow = state.RuleSets[result.Workflow];
                            break;
                        }
                    }
                } while (keepGoing);
            }
            return sum;
        }

        private State GetState(List<string> input)
        {
            var state = new State()
            {
                Parts = new List<Part>(),
                RuleSets = new Dictionary<string, RuleSet>()
            };
            bool isRules = true;
            foreach (var line in input)
            {
                if (isRules)
                {
                    if (line == "")
                    {
                        isRules = false;
                    }
                    else
                    {
                        var ruleSet = GetRuleSet(line);
                        state.RuleSets.Add(ruleSet.Name, ruleSet);
                    }
                }
                else
                {
                    state.Parts.Add(GetPart(line));
                }
            }
            return state;
        }

        private Part GetPart(string line)
        {
            var part = new Part();
            line = line.Substring(1, line.Length - 2);
            var split = line.Split(',');
            foreach (var subValue in split)
            {
                var subSplit = subValue.Split('=');
                var value = Convert.ToUInt64(subSplit[1]);
                switch (subSplit[0])
                {
                    case "a":
                        part.A = value;
                        break;
                    case "x":
                        part.X = value;
                        break;
                    case "m":
                        part.M = value;
                        break;
                    case "s":
                        part.S = value;
                        break;
                    default: throw new Exception();
                }
            }
            return part;
        }

        private RuleSet GetRuleSet(string line)
        {
            var set = new RuleSet();
            var firstBrace = line.IndexOf('{');
            set.Name = line.Substring(0, firstBrace);
            var split = line.Substring(firstBrace + 1, line.Length - firstBrace - 2).Split(',');
            set.Rules = split
                .Take(split.Length - 1)
                .Select(x => GetRule(x))
                .ToList();
            set.Rules.Add(GetLastRule(split.Last(), set));
            return set;
        }

        private Rule GetLastRule(string line, RuleSet parent)
        {
            if (line == "A")
            {
                return new Rule()
                {
                    IsLastRule = true,
                    Parent = parent,
                    Result = enumResult.Accepted,
                    RuleFunc = x => Result.ToAccepted
                };
            }
            else if (line == "R")
            {
                return new Rule()
                {
                    IsLastRule = true,
                    IsRejected = true,
                    Parent = parent,
                    Result = enumResult.Rejected,
                    RuleFunc = x => Result.ToRejected
                };
            }
            else
            {
                return new Rule()
                {
                    IsLastRule = true,
                    Parent = parent,
                    Result = enumResult.SendToWorkflow,
                    RuleFunc = x => Result.ToWorkFlow(line),
                    ToWorkFlow = line
                };
            }
        }

        private Rule GetRule(string line)
        {
            // get value
            Func<Part, ulong> getValue;
            string propName = "";
            switch (line[0])
            {
                case 'x':
                    getValue = x => x.X;
                    propName = "x";
                    break;
                case 'm':
                    getValue = x => x.M;
                    propName = "m";
                    break;
                case 'a':
                    getValue = x => x.A;
                    propName = "a";
                    break;
                case 's':
                    getValue = x => x.S;
                    propName = "s";
                    break;
                default: throw new Exception();
            }

            // test value
            var colonIndex = line.IndexOf(':');
            var value = Convert.ToUInt64(line.Substring(2, colonIndex - 2));
            Func<ulong, bool> testValue;
            enumComparison comparison;
            if (line[1] == '<')
            {
                comparison = enumComparison.LessThan;
                testValue = x => x < value;
            }
            else
            {
                comparison = enumComparison.GreaterThan;
                testValue = x => x > value;
            }

            // result
            var result = line.Substring(colonIndex + 1);
            if (result == "A")
            {
                return new Rule()
                {
                    Comparison = comparison,
                    PropName = propName,
                    Result = enumResult.Accepted,
                    RuleFunc = x => testValue(getValue(x)) ? Result.ToAccepted : Result.ToPass,
                    Value = value
                };
            }
            else if (result == "R")
            {
                return new Rule()
                {
                    Comparison = comparison,
                    IsRejected = true,
                    PropName = propName,
                    Result = enumResult.Rejected,
                    RuleFunc = x => testValue(getValue(x)) ? Result.ToRejected : Result.ToPass,
                    Value = value
                };
            }
            else
            {
                return new Rule()
                {
                    Comparison = comparison,
                    PropName = propName,
                    Result = enumResult.SendToWorkflow,
                    RuleFunc = x => testValue(getValue(x)) ? Result.ToWorkFlow(result) : Result.ToPass,
                    Value = value,
                    ToWorkFlow = result
                };
            }
        }

        private class State
        {
            public Dictionary<string, RuleSet> RuleSets { get; set; }
            public List<Part> Parts { get; set; }
        }

        private class Rule
        {
            public Func<Part, Result> RuleFunc { get; set; }
            public string PropName { get; set; }
            public bool IsRejected { get; set; }
            public bool IsLastRule { get; set; }
            public enumComparison Comparison { get; set; }
            public enumResult Result { get; set; }
            public string ToWorkFlow { get; set; }
            public ulong Value { get; set; }
            public RuleSet Parent { get; set; }
        }

        private class Result
        {
            public static Result ToAccepted => new Result() { ResultValue = enumResult.Accepted };
            public static Result ToRejected => new Result() { ResultValue = enumResult.Rejected };
            public static Result ToWorkFlow(string workFlow) => new Result() { ResultValue = enumResult.SendToWorkflow, Workflow = workFlow };
            public static Result ToPass => new Result() { ResultValue = enumResult.Pass };
            public enumResult ResultValue { get; set; }
            public string Workflow { get; set; }
        }

        private class RuleSet
        {
            public string Name { get; set; }
            public List<Rule> Rules { get; set; }
        }

        private class Part
        {
            public ulong X { get; set; }
            public ulong M { get; set; }
            public ulong A { get; set; }
            public ulong S { get; set; }
        }

        private class Range
        {
            public static Range Copy(Range from)
            {
                var range = new Range()
                {
                    A = new ulong[2],
                    M = new ulong[2],
                    S = new ulong[2],
                    X = new ulong[2]
                };
                Array.Copy(from.A, range.A, from.A.Length);
                Array.Copy(from.M, range.M, from.M.Length);
                Array.Copy(from.S, range.S, from.S.Length);
                Array.Copy(from.X, range.X, from.X.Length);
                return range;
            }

            public ulong[] X { get; set; }
            public ulong[] M { get; set; }
            public ulong[] A { get; set; }
            public ulong[] S { get; set; }
        }

        private class RangeLimiter
        {
            public string PropName { get; set; }
            public enumComparison Comparison { get; set; }
            public ulong Value { get; set; }
        }

        private enum enumResult
        {
            SendToWorkflow,
            Accepted,
            Rejected,
            Pass
        }

        private enum enumComparison
        {
            LessThan,
            GreaterThan
        }
    }
}
