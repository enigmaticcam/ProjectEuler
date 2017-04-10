using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public interface IProblem {
        bool RequiresInputFile { get; }
        string ProblemName { get; }
        string GetAnswer();
        void UploadInputFile(string fileName);
        void DownloadOutputFile();
    }

    public abstract class ProblemBase : IProblem {
        public abstract string ProblemName { get; }
        public abstract string GetAnswer();
        public virtual void UploadInputFile(string fileName) { }
        public virtual void DownloadOutputFile() { }

        public virtual bool RequiresInputFile {
            get { return false; }
        }
    }
}
