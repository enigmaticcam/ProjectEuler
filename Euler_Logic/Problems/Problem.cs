using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public interface IProblem {
        string ProblemName { get; }
        string GetAnswer();
    }
}
