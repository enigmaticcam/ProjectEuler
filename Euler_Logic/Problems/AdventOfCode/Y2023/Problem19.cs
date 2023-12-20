using System;
using System.Collections.Generic;
using System.Linq;

namespace Euler_Logic.Problems.AdventOfCode.Y2023
{
    public class Problem19 : AdventOfCodeBase
    {
        public override string ProblemName => "Advent of Code 2023: 19";

        public override string GetAnswer()
        {
            return Answer1(Input()).ToString();
        }

        public override string GetAnswer2()
        {
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
            return ApplyRanges(state);
        }

        private ulong ApplyRanges(State state)
        {
            var range = new Range()
            {
                A = new ulong[2] { 1, 4000 },
                M = new ulong[2] { 1, 4000 },
                S = new ulong[2] { 1, 4000 },
                X = new ulong[2] { 1, 4000 }
            };
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
            foreach (var part in state.Parts)
            {
                var currentFlow = state.RuleSets["in"];
                bool keepGoing = true;
                do
                {
                    foreach (var rule in currentFlow.Rules)
                    {
                        bool isLast = rule == currentFlow.Rules.Last();
                        var result = ApplyRule(state, rule, part, isLast);
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

        private Result ApplyRule(State state, Rule rule, Part part, bool isLast)
        {
            bool condition = true;
            if (!isLast)
            {
                var value = GetValue(rule, part);
                condition = GetCondition(rule, part, value);
            }   
            if (condition)
            {
                if (rule.Result == enumResult.Accepted)
                {
                    return Result.ToAccepted;
                }
                else if (rule.Result == enumResult.Rejected)
                {
                    return Result.ToRejected;
                }
                else
                {
                    return Result.ToWorkFlow(rule.ToWorkFlow);
                }
            }
            return Result.ToPass;
        }

        private ulong GetValue(Rule rule, Part part)
            => rule.PropName switch
            {
                "a" => part.A,
                "m" => part.M,
                "s" => part.S,
                "x" => part.X,
                _ => throw new Exception()
            };

        private bool GetCondition(Rule rule, Part part, ulong value)
        {
            if (rule.Comparison == enumComparison.GreaterThan)
            {
                return value > rule.Value;
            }
            else
            {
                return value < rule.Value;
            }
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
                .Select(x => GetRule(x, false))
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
                    Result = enumResult.Accepted
                };
            } else if (line == "R")
            {
                return new Rule()
                {
                    Result = enumResult.Rejected
                };
            } else
            {
                return new Rule()
                {
                    Result = enumResult.SendToWorkflow,
                    ToWorkFlow = line
                };
            }
        }

        private Rule GetRule(string line, bool isLast)
        {
            var rule = new Rule();

            // get value
            rule.PropName = line.Substring(0, 1);

            // test value
            var colonIndex = line.IndexOf(':');
            rule.Value = Convert.ToUInt64(line.Substring(2, colonIndex - 2));
            if (line[1] == '<')
            {
                rule.Comparison = enumComparison.LessThan;
            }
            else
            {
                rule.Comparison = enumComparison.GreaterThan;
            }

            // result
            var result = line.Substring(colonIndex + 1);
            if (result == "A")
            {
                rule.Result = enumResult.Accepted;
            }
            else if (result == "R")
            {
                rule.Result = enumResult.Rejected;
            }
            else
            {
                rule.Result = enumResult.SendToWorkflow;
                rule.ToWorkFlow = result;
            }
            return rule;
        }

        private class State
        {
            public Dictionary<string, RuleSet> RuleSets { get; set; }
            public List<Part> Parts { get; set; }
        }

        private class Rule
        {
            public string PropName { get; set; }
            public enumComparison Comparison { get; set; }
            public enumResult Result { get; set; }
            public string ToWorkFlow { get; set; }
            public ulong Value { get; set; }
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
