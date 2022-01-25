using System;
using System.Collections.Generic;
using System.Linq;

namespace Euler_Logic.Problems.AdventOfCode.Y2018 {
    public class Problem16 : AdventOfCodeBase {
        public override string ProblemName {
            get { return "Advent of Code 2018: 16"; }
        }

        public override string GetAnswer() {
            return Answer1();
        }

        public override string GetAnswer2() {
            return Answer2();
        }

        public string Answer1() {
            var input = GetInstructions(Input());
            var functions = GetFunctions();
            int count = 0;
            input.Instructions.ForEach(x => count += (Atleast3(x, functions) ? 1 : 0));
            return count.ToString();
        }

        private string Answer2() {
            var input = GetInstructions(Input());
            var functions = GetFunctions();
            var mapping = GetMapping(input.Instructions, functions);
            var ops = ReduceMappings(mapping);
            return RunOps(ops, input.Ops).ToString();
        }

        private int RunOps(InsFunction[] functions, List<int[]> ops) {
            Instruction current = new Instruction();
            current.After = new int[4];
            foreach (var op in ops) {
                var function = functions[op[0]];
                current.Before = current.After;
                current.Op = op;
                function.Perform(current);
            }
            return current.After[0];
        }

        private InsFunction[] ReduceMappings(Dictionary<Instruction, HashSet<InsFunction>> mapping) {
            var result = new InsFunction[16];
            do {
                var next = mapping.Where(x => x.Value.Count == 1).ToList();
                if (next.Count() == 0) {
                    return result;
                }
                foreach (var map in next) {
                    if (map.Value.Count > 0) {
                        var toRemove = map.Value.First();
                        result[map.Key.Op[0]] = toRemove;
                        foreach (var sub in mapping) {
                            if (sub.Value.Contains(toRemove)) {
                                sub.Value.Remove(toRemove);
                            }
                        }
                    }

                }
            } while (true);
        }

        private Dictionary<Instruction, HashSet<InsFunction>> GetMapping(List<Instruction> instructions, List<InsFunction> functions) {
            var hash = new Dictionary<Instruction, HashSet<InsFunction>>();
            foreach (var ins in instructions) {
                foreach (var function in functions) {
                    function.Perform(ins);
                    if (IsExpected(ins)) {
                        if (!hash.ContainsKey(ins)) {
                            hash.Add(ins, new HashSet<InsFunction>());
                        }
                        hash[ins].Add(function);
                    }
                }
            }
            return hash;
        }

        private bool Atleast3(Instruction ins, List<InsFunction> functions) {
            int count = 0;
            foreach (var insFunc in functions) {
                insFunc.Perform(ins);
                if (IsExpected(ins)) {
                    count++;
                    if (count == 3) {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool IsExpected(Instruction ins) {
            return ins.After[0] == ins.Exepcted[0]
                && ins.After[1] == ins.Exepcted[1]
                && ins.After[2] == ins.Exepcted[2]
                && ins.After[3] == ins.Exepcted[3];
        }

        private List<InsFunction> GetFunctions() {
            var functions = new List<InsFunction>();
            functions.Add(new InsFunctionAddr());
            functions.Add(new InsFunctionAddi());
            functions.Add(new InsFunctionMulr());
            functions.Add(new InsFunctionMuli());
            functions.Add(new InsFunctionBanr());
            functions.Add(new InsFunctionBani());
            functions.Add(new InsFunctionBorr());
            functions.Add(new InsFunctionBori());
            functions.Add(new InsFunctionSetr());
            functions.Add(new InsFunctionSeti());
            functions.Add(new InsFunctionGtir());
            functions.Add(new InsFunctionGtri());
            functions.Add(new InsFunctionGtrr());
            functions.Add(new InsFunctionEqir());
            functions.Add(new InsFunctionEqri());
            functions.Add(new InsFunctionEqrr());
            return functions;
        }

        private Inputs GetInstructions(List<string> input) {
            var inputs = new Inputs();
            int index = 0;
            do {
                var ins = new Instruction();
                ins.Before = input[index].Replace("Before: [", "").Replace("]", "").Split(',').Select(x => Convert.ToInt32(x.Trim())).ToArray();
                ins.Op = input[index + 1].Split(' ').Select(x => Convert.ToInt32(x)).ToArray();
                ins.Exepcted = input[index + 2].Replace("After:  [", "").Replace("]", "").Split(',').Select(x => Convert.ToInt32(x.Trim())).ToArray();
                ins.After = new int[4];
                index += 4;
                inputs.Instructions.Add(ins);
                if (input[index] == "") {
                    break;
                }
            } while (true);
            index += 2;
            do {
                inputs.Ops.Add(input[index].Split(' ').Select(x => Convert.ToInt32(x)).ToArray());
                index++;
            } while (index < input.Count);
            return inputs;
        }

        private class Inputs {
            public Inputs() {
                Instructions = new List<Instruction>();
                Ops = new List<int[]>();
            }
            public List<Instruction> Instructions { get; set; }
            public List<int[]> Ops { get; set; }
        }

        private class Instruction {
            public int[] Before { get; set; }
            public int[] Op { get; set; }
            public int[] After { get; set; }
            public int[] Exepcted { get; set; }
        }

        private abstract class InsFunction {
            public abstract void Perform(Instruction ins);
            protected void Reset(Instruction ins) {
                ins.After[0] = ins.Before[0];
                ins.After[1] = ins.Before[1];
                ins.After[2] = ins.Before[2];
                ins.After[3] = ins.Before[3];
            }
        }

        private class InsFunctionAddr : InsFunction {
            public override void Perform(Instruction ins) {
                Reset(ins);
                ins.After[ins.Op[3]] = ins.Before[ins.Op[1]] + ins.Before[ins.Op[2]];
            }
        }

        private class InsFunctionAddi : InsFunction {
            public override void Perform(Instruction ins) {
                Reset(ins);
                ins.After[ins.Op[3]] = ins.Before[ins.Op[1]] + ins.Op[2];
            }
        }

        private class InsFunctionMulr : InsFunction {
            public override void Perform(Instruction ins) {
                Reset(ins);
                ins.After[ins.Op[3]] = ins.Before[ins.Op[1]] * ins.Before[ins.Op[2]];
            }
        }

        private class InsFunctionMuli : InsFunction {
            public override void Perform(Instruction ins) {
                Reset(ins);
                ins.After[ins.Op[3]] = ins.Before[ins.Op[1]] * ins.Op[2];
            }
        }

        private class InsFunctionBanr : InsFunction {
            public override void Perform(Instruction ins) {
                Reset(ins);
                ins.After[ins.Op[3]] = ins.Before[ins.Op[1]] & ins.Before[ins.Op[2]];
            }
        }

        private class InsFunctionBani : InsFunction {
            public override void Perform(Instruction ins) {
                Reset(ins);
                ins.After[ins.Op[3]] = ins.Before[ins.Op[1]] & ins.Op[2];
            }
        }

        private class InsFunctionBorr : InsFunction {
            public override void Perform(Instruction ins) {
                Reset(ins);
                ins.After[ins.Op[3]] = ins.Before[ins.Op[1]] | ins.Before[ins.Op[2]];
            }
        }

        private class InsFunctionBori : InsFunction {
            public override void Perform(Instruction ins) {
                Reset(ins);
                ins.After[ins.Op[3]] = ins.Before[ins.Op[1]] | ins.Op[2];
            }
        }

        private class InsFunctionSetr : InsFunction {
            public override void Perform(Instruction ins) {
                Reset(ins);
                ins.After[ins.Op[3]] = ins.Before[ins.Op[1]];
            }
        }

        private class InsFunctionSeti : InsFunction {
            public override void Perform(Instruction ins) {
                Reset(ins);
                ins.After[ins.Op[3]] = ins.Op[1];
            }
        }

        private class InsFunctionGtir : InsFunction {
            public override void Perform(Instruction ins) {
                Reset(ins);
                ins.After[ins.Op[3]] = (ins.Op[1] > ins.Before[ins.Op[2]] ? 1 : 0);
            }
        }

        private class InsFunctionGtri : InsFunction {
            public override void Perform(Instruction ins) {
                Reset(ins);
                ins.After[ins.Op[3]] = (ins.Before[ins.Op[1]] > ins.Op[2] ? 1 : 0);
            }
        }

        private class InsFunctionGtrr : InsFunction {
            public override void Perform(Instruction ins) {
                Reset(ins);
                ins.After[ins.Op[3]] = (ins.Before[ins.Op[1]] > ins.Before[ins.Op[2]] ? 1 : 0);
            }
        }

        private class InsFunctionEqir : InsFunction {
            public override void Perform(Instruction ins) {
                Reset(ins);
                ins.After[ins.Op[3]] = (ins.Op[1] == ins.Before[ins.Op[2]] ? 1 : 0);
            }
        }

        private class InsFunctionEqri : InsFunction {
            public override void Perform(Instruction ins) {
                Reset(ins);
                ins.After[ins.Op[3]] = (ins.Before[ins.Op[1]] == ins.Op[2] ? 1 : 0);
            }
        }

        private class InsFunctionEqrr : InsFunction {
            public override void Perform(Instruction ins) {
                Reset(ins);
                ins.After[ins.Op[3]] = (ins.Before[ins.Op[1]] == ins.Before[ins.Op[2]] ? 1 : 0);
            }
        }
    }
}
