using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem62 : ProblemBase {
        private Dictionary<string, List<decimal>> _cubes;
        private decimal _lastCubeRoot;
        private int _lastDigitCount;

        public override string ProblemName {
            get { return "62: Cubic permutations"; }
        }

        public override string GetAnswer() {
            _lastCubeRoot = 1;
            GenerateCubes();
            do {
                foreach (string key in _cubes.Keys) {
                    if (_cubes[key].Count == 5) {
                        return _cubes[key][0].ToString();
                    }
                }
                GenerateCubes();
            } while (true);
        }

        private void GenerateCubes() {
            _cubes = new Dictionary<string, List<decimal>>();
            _lastDigitCount++;
            decimal currentCubeRoot = _lastCubeRoot;
            decimal currentCube = currentCubeRoot * currentCubeRoot * currentCubeRoot;
            do {
                string permutationKey = "";
                string currentCubeAsString = currentCube.ToString();
                for (int i = 0; i <= 9; i++) {
                    permutationKey += (currentCubeAsString.Length - currentCubeAsString.Replace(i.ToString(), "").Length).ToString();
                }
                if (!_cubes.ContainsKey(permutationKey)) {
                    _cubes.Add(permutationKey, new List<decimal>());
                }
                _cubes[permutationKey].Add(currentCube);
                currentCubeRoot += 1;
                currentCube = currentCubeRoot * currentCubeRoot * currentCubeRoot;
            } while (currentCube.ToString().Length <= _lastDigitCount);
            _lastCubeRoot = currentCubeRoot;
        }
    }
}
