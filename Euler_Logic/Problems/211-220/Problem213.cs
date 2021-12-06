using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem213 : ProblemBase {
        private Square[,] _grid;
        private decimal[,] _counts;

        public override string ProblemName {
            get { return "213: Flea Circus"; }
        }

        public override string GetAnswer() {
            int size = 30;
            InitializeGrid(size);
            return "";
        }

        private void InitializeGrid(int size) {
            _grid = new Square[size, size];
            _counts = new decimal[size, size];
            for (int x = 0; x < size; x++) {
                for (int y = 0; y < size; y++) {
                    _grid[x, y] = new Square();
                    _grid[x, y].AtLeastOne = 1;
                    _grid[x, y].None = 0;
                    _counts[x, y] += (x > 0 ? 1 : 0);
                    _counts[x, y] += (x < size - 1 ? 1 : 0);
                    _counts[x, y] += (y > 0 ? 1 : 0);
                    _counts[x, y] += (y < size - 1 ? 1 : 0);
                }
            }
        }

        private void Bong(int size) {
            var newGrid = new Square[size, size];
            for (int x = 0; x < size; x++) {
                for (int y = 0; y < size; y++) {
                    
                }
            }
        }

        private class Square {
            public decimal AtLeastOne { get; set; }
            public decimal None { get; set; }
        }
    }
}
