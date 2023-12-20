using Euler_Logic.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Euler_Logic.Problems.AdventOfCode.Y2023;

public class Problem20 : AdventOfCodeBase
{
    public override string ProblemName => "Advent of Code 2023: 20";

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
        AddButtonModule(state);
        PushButton(state, 1000);
        return state.Queue.HighPulseCount * state.Queue.LowPulseCount;
    }

    private ulong Answer2(List<string> input)
    {
        var state = GetState(input);
        AddButtonModule(state);
        return PushButton(state);
    }
    private ulong PushButton(State state)
    {
        state.LookingFor = new List<LookFor>()
        {
            new LookFor() { Name = "rk" },
            new LookFor() { Name = "cd" },
            new LookFor() { Name = "zf" },
            new LookFor() { Name = "qx" }
        };
        do
        {
            PushButton(state, true);
            if (!state.LookingFor.Any(x => !x.Found))
            {
                return CalcLCM(state.LookingFor.Select(x => x.ButtonCount));
            }
        } while (true);
    }

    private ulong CalcLCM(IEnumerable<ulong> nums)
    {
        ulong num = nums.First();
        foreach (var next in nums.Skip(1))
        {
            num = LCM.GetLCM(num, next);
        }
        return num;
    }
    
    private void PushButton(State state, int total)
    {
        for (int count = 1; count <= total; count++)
        {
            PushButton(state, false);
        }
    }

    private bool PushButton(State state, bool lookForRx)
    {
        state.ButtonCount++;
        state.Modules["button"].ReceivePulse(false, "", state);
        while (state.Queue.Queue.Count > 0)
        {
            var first = state.Queue.Queue.First.Value;
            AnalyzePulse(state, first);
            if (first.ToModule == "rx")
            {
                if (!first.IsHigh && lookForRx)
                    return true;
            }
            else
            {
                var module = state.Modules[first.ToModule];
                module.ReceivePulse(first.IsHigh, first.FromModule, state);
            }
            
            state.Queue.Queue.RemoveFirst();
        }
        return false;
    }

    private void AnalyzePulse(State state, Pulse pulse)
    {
        if (state.LookingFor != null && !pulse.IsHigh)
        {
            foreach (var look in state.LookingFor)
            {
                if (!look.Found && pulse.ToModule == look.Name)
                {
                    look.Found = true;
                    look.ButtonCount = state.ButtonCount;
                }
            }
        }
    }

    private void AddButtonModule(State state)
    {
        var button = new Module_Button()
        {
            Connections = new List<string>() { "broadcaster" },
            Name = "button"
        };
        state.Modules.Add(button.Name, button);
    }

    private State GetState(List<string> input)
    {
        var queue = new PulseQueue() { Queue = new LinkedList<Pulse>() };
        var modules = new Dictionary<string, Module>();
        foreach (var line in input)
        {
            var split = line.Replace(",", "").Split(' ');
            var module = GetModule(split[0]);
            module.Connections = split
                .Skip(2)
                .ToList();
            modules.Add(module.Name, module);
        }
        return new State()
        {
            Modules = modules,
            Queue = queue
        };
    }

    private Module GetModule(string name)
    {
        if (name[0] == '%')
        {
            return new Module_FlipFlop() { Name = name.Substring(1) };
        }
        else if (name[0] == '&')
        {
            return new Module_Conjuction() { Name = name.Substring(1) };
        }
        else if (name == "broadcaster")
        {
            return new Module_Broadcast() { Name = name };
        }
        else if (name == "button")
        {
            return new Module_Button() { Name = name };
        }
        else
        {
            throw new Exception();
        }
    }

    private abstract class Module
    {
        public abstract void ReceivePulse(bool isHigh, string fromModule, State state);
        public string Name { get; set; }
        public List<string> Connections { get; set; }
    }

    private class Module_FlipFlop : Module
    {
        private bool _isOn;
        public override void ReceivePulse(bool isHigh, string fromModule, State state)
        {
            if (!isHigh)
            {
                foreach (var connection in Connections)
                {
                    state.Queue.AddPulse(!_isOn, Name, connection);
                }
                _isOn = !_isOn;
            }
        }
    }

    private class Module_Conjuction : Module
    {
        private bool _initialized;
        private Dictionary<string, bool> _inputs;
        public override void ReceivePulse(bool isHigh, string fromModule, State state)
        {
            if (!_initialized)
            {
                _initialized = true;
                _inputs = new Dictionary<string, bool>();
                foreach (var module in state.Modules.Values)
                {
                    if (module.Connections.Any(x => x == Name))
                        _inputs.Add(module.Name, false);
                }
            }
            _inputs[fromModule] = isHigh;
            bool sendHighPulse = _inputs.Values.Count(x => x) != _inputs.Count;
            foreach (var connection in Connections)
            {
                state.Queue.AddPulse(sendHighPulse, Name, connection);
            }
        }

    }

    private class Module_Broadcast : Module
    {
        public override void ReceivePulse(bool isHigh, string fromModule, State state)
        {
            foreach (var connection in Connections)
            {
                state.Queue.AddPulse(isHigh, Name, connection);
            }
        }

    }

    private class Module_Button : Module
    {
        public override void ReceivePulse(bool isHigh, string fromModule, State state)
        {
            state.Queue.AddPulse(false, Name, "broadcaster");
        }
    }

    private class PulseQueue
    {
        public ulong LowPulseCount { get; set; }
        public ulong HighPulseCount { get; set; }
        public void AddPulse(bool isHigh, string fromModule, string toModule)
        {
            Queue.AddLast(new Pulse()
            {
                FromModule = fromModule,
                IsHigh = isHigh,
                ToModule = toModule
            });
            if (isHigh)
            {
                HighPulseCount++;
            }
            else
            {
                LowPulseCount++;
            }
        }
        public LinkedList<Pulse> Queue { get; set; }
    }

    private class Pulse
    {
        public string FromModule { get; set; }
        public string ToModule { get; set; }
        public bool IsHigh { get; set; }
    }

    private class State
    {
        public Dictionary<string, Module> Modules { get; set; }
        public PulseQueue Queue { get; set; }
        public ulong ButtonCount { get; set; }
        public List<LookFor> LookingFor { get; set; }
    }

    private class LookFor
    {
        public string Name { get; set; }
        public bool Found { get; set; }
        public ulong ButtonCount { get; set; }
    }
}
