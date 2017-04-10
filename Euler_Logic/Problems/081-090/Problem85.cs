using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem85 : ProblemBase {
        private Dictionary<ulong, ulong> _colCountsPerRow = new Dictionary<ulong, ulong>();
        private ulong _maxColCount = 1;
        private HashSet<ulong> _seenBefore = new HashSet<ulong>();

        public override string ProblemName {
            get { return "85: Counting rectangles"; }
        }

        public override string GetAnswer() {
            _colCountsPerRow.Add(1, 1);
            return FindSquare().ToString();
        }

        private ulong FindSquare() {
            ulong colCount = FindFirstToTwoMill();
            ulong bestCount = 2000000 - _colCountsPerRow[colCount];
            ulong bestArea = colCount;
            ulong rowCount = 1;
            do {
                rowCount++;
                for (; colCount > 0; colCount--) {
                    ulong score = GetRectangleCount(rowCount, colCount);
                    if (score > 2000000) {
                        if (score - 2000000 < bestCount) {
                            bestCount = score - 2000000;
                            bestArea = rowCount * colCount;
                        }
                    } else {
                        if (2000000 - score < bestCount) {
                            bestCount = 2000000 - score;
                            bestArea = rowCount * colCount;
                        }
                        break;
                    }
                }
            } while (colCount > 0);
            return bestArea;
        }

        private ulong FindFirstToTwoMill() {
            ulong colCount = 2;
            do {
                ulong rectCount = GetRectangleCount(1, colCount);
                if (rectCount > 2000000) {
                    break;
                }
                colCount++;
            } while (true);
            if (_colCountsPerRow[colCount] - 2000000 > 2000000 - _colCountsPerRow[colCount - 1]) {
                return colCount;
            } else {
                return colCount - 1;
            }
        }

        private ulong GetRectangleCount(ulong rowMax, ulong colMax) {
            ulong count = 0;
            if (colMax > _maxColCount) {
                BuildRowCounts(colMax);
            }
            for (ulong height = 1; height <= rowMax; height++) {
                count += (_colCountsPerRow[colMax] * (rowMax - height + 1));
            }
            return count;
        }

        private void BuildRowCounts(ulong max) {
            for (ulong colCount = _maxColCount + 1; colCount <= max; colCount++) {
                _colCountsPerRow.Add(colCount, _colCountsPerRow[colCount - 1] + colCount);
            }
            _maxColCount = max;
        }
    }
}
