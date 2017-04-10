using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public abstract class GoogleBase : ProblemBase {
        public override bool RequiresInputFile {
            get { return true; }
        }
        protected string[] _tests;
        protected List<string> _results = new List<string>();

        public override void UploadInputFile(string fileName) {
            StreamReader file = new StreamReader(fileName);
            int testCount = Convert.ToInt32(file.ReadLine());
            _tests = new string[testCount];
            for (int testIndex = 1; testIndex <= testCount; testIndex++) {
                _tests[testIndex - 1] = file.ReadLine();
            }
            file.Close();
        }

        public override void DownloadOutputFile() {
            string fileName = "C:\\Users\\ctangen\\Downloads\\Answer.out";
            StreamWriter file = new StreamWriter(fileName, false);
            for (int index = 1; index <= _results.Count; index++) {
                file.WriteLine("Case #" + index + ": " + _results[index - 1]);
            }
            file.Close();
            System.Diagnostics.Process.Start("explorer.exe", "/select," + fileName);
        }
    }
}
