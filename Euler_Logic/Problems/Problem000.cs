using System.IO;

namespace Euler_Logic.Problems {
    public class Problem000 : ProblemBase {
        public override string ProblemName {
            get { return "Sandbox5"; }
        }

        public override string GetAnswer() {
            MoveFiles("2017 Archive");
            MoveFiles("2018 Archive");
            MoveFiles("2019 Archive");
            MoveFiles("2020 Archive");
            return "";
        }

        private void MoveFiles(string year) {
            var path = Path.Combine(@"C:\Users\Ctangen\Dropbox\Personal\KHFunds\", year);
            var directory = new DirectoryInfo(path);
            foreach (var subDirectory in directory.GetDirectories()) {
                bool fileFound = false;
                foreach (var file in subDirectory.GetFiles()) {
                    if (file.Name == "Accounts Sheet.pdf") {
                        File.Copy(file.FullName, @"C:\Temp\Accounts\" + subDirectory.Name + ".pdf");
                        fileFound = true;
                        break;
                    }
                }
                if (!fileFound) {
                    throw new System.Exception("No file found for " + subDirectory.Name);
                }
            }
        }
    }
}
