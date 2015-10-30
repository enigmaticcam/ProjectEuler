using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem45 : IProblem {
        private decimal _triangleNum;
        private decimal _triangleIndex;
        private HashSet<decimal> _triangles = new HashSet<decimal>();
        private decimal _pentagonNum;
        private decimal _pentagonIndex;
        private HashSet<decimal> _pentagons = new HashSet<decimal>();
        private decimal _hexagonNum;
        private decimal _hexagonIndex;
        private HashSet<decimal> _hexagons = new HashSet<decimal>();

        public string ProblemName {
            get { return "45: Triangular, pentagonal, and hexagonal"; }
        }

        public string GetAnswer() {
            return FindSame(3).ToString();
        }

        private decimal FindSame(int maxCount) {
            int count = 0;
            decimal lastFound = 0;
            do {
                _hexagonIndex++;
                _hexagonNum = _hexagonIndex * ((_hexagonIndex * 2) - 1);
                GetNextTriangle(_hexagonNum);
                GetNextPentagon(_hexagonNum);
                if (_triangles.Contains(_hexagonNum) && _pentagons.Contains(_hexagonNum)) {
                    count++;
                    lastFound = _hexagonNum;
                }
            } while (count < maxCount);
            return lastFound;
        }

        private void GetNextTriangle(decimal max) {
            do {
                _triangleIndex++;
                _triangleNum = (_triangleIndex * (_triangleIndex + 1)) / 2;
                _triangles.Add(_triangleNum);
            } while (_triangleNum < max);
        }

        private void GetNextPentagon(decimal max) {
            do {
                _pentagonIndex++;
                _pentagonNum = (_triangleIndex * ((_triangleIndex * 3) - 1)) / 2;
                _pentagons.Add(_pentagonNum);
            } while (_pentagonNum < max);
        }
    }
}
