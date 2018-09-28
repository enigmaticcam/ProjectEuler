using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem126 : ProblemBase {

        public override string ProblemName {
            get { return "126: Cuboid layers"; }
        }

        public override string GetAnswer() {
            int x = 1;
            do {
                for (int y = x; y >= 1; y--) {
                    for (int z = y; z >= 1; z--) {
                        Count(x, y, z);
                    }
                }
                x++;
            } while (!_results.ContainsKey(154) || _results[154].Count < 10);
            return "";
        }

        private void Count(int x, int y, int z) {
            Cuboid cuboid = new Cuboid();
            for (int blockX = 1; blockX <= x; blockX++) {
                cuboid.BlocksAll.Add(blockX, new Dictionary<int, Dictionary<int, Block>>());
                for (int blockY = 1; blockY <= y; blockY++) {
                    cuboid.BlocksAll[blockX].Add(blockY, new Dictionary<int, Block>());
                    for (int blockZ = 1; blockZ <= z; blockZ++) {
                        var block = new Block() { X = blockX, Y = blockY, Z = blockZ };
                        cuboid.BlocksAll[blockX][blockY].Add(blockZ, block);
                        cuboid.BlocksOuter.Add(block);
                    }
                }
            }
            AddLayer(cuboid);
            int first = cuboid.BlocksOuter.Count;
            AddLayer(cuboid);
            int second = cuboid.BlocksOuter.Count;
            AddResult(x, y, z, first);
            AddResult(x, y, z, second);
            int temp = 0;
            for (int index = 1; index <= 1000; index++) {
                temp = second;
                second = second - first + 8 + second;
                first = temp;
                AddResult(x, y, z, second);
                if (second >= 154) {
                    break;
                }
            }
        }

        private Dictionary<int, List<string>> _results = new Dictionary<int, List<string>>();
        private void AddResult(int x, int y, int z, int value) {
            if (!_results.ContainsKey(value)) {
                _results.Add(value, new List<string>());
            }
            _results[value].Add(x + "," + y + "," + z);
        }

        private void AddLayer(Cuboid cuboid) {
            List<Block> blocksOuter = new List<Block>();
            foreach (var block in cuboid.BlocksOuter) {
                if (!cuboid.BlocksAll.ContainsKey(block.X + 1)
                    || !cuboid.BlocksAll[block.X + 1].ContainsKey(block.Y)
                    || !cuboid.BlocksAll[block.X + 1][block.Y].ContainsKey(block.Z)) {
                    Block newBlock = new Block() { X = block.X + 1, Y = block.Y, Z = block.Z };
                    cuboid.AddBlock(newBlock);
                    blocksOuter.Add(newBlock);
                }
                if (!cuboid.BlocksAll.ContainsKey(block.X)
                    || !cuboid.BlocksAll[block.X].ContainsKey(block.Y + 1)
                    || !cuboid.BlocksAll[block.X][block.Y + 1].ContainsKey(block.Z)) {
                    Block newBlock = new Block() { X = block.X, Y = block.Y + 1, Z = block.Z };
                    cuboid.AddBlock(newBlock);
                    blocksOuter.Add(newBlock);
                }
                if (!cuboid.BlocksAll.ContainsKey(block.X)
                    || !cuboid.BlocksAll[block.X].ContainsKey(block.Y)
                    || !cuboid.BlocksAll[block.X][block.Y].ContainsKey(block.Z + 1)) {
                    Block newBlock = new Block() { X = block.X, Y = block.Y, Z = block.Z + 1 };
                    cuboid.AddBlock(newBlock);
                    blocksOuter.Add(newBlock);
                }
                if (!cuboid.BlocksAll.ContainsKey(block.X - 1)
                    || !cuboid.BlocksAll[block.X - 1].ContainsKey(block.Y)
                    || !cuboid.BlocksAll[block.X - 1][block.Y].ContainsKey(block.Z)) {
                    Block newBlock = new Block() { X = block.X - 1, Y = block.Y, Z = block.Z };
                    cuboid.AddBlock(newBlock);
                    blocksOuter.Add(newBlock);
                }
                if (!cuboid.BlocksAll.ContainsKey(block.X)
                    || !cuboid.BlocksAll[block.X].ContainsKey(block.Y - 1)
                    || !cuboid.BlocksAll[block.X][block.Y - 1].ContainsKey(block.Z)) {
                    Block newBlock = new Block() { X = block.X, Y = block.Y - 1, Z = block.Z };
                    cuboid.AddBlock(newBlock);
                    blocksOuter.Add(newBlock);
                }
                if (!cuboid.BlocksAll.ContainsKey(block.X)
                    || !cuboid.BlocksAll[block.X].ContainsKey(block.Y)
                    || !cuboid.BlocksAll[block.X][block.Y].ContainsKey(block.Z - 1)) {
                    Block newBlock = new Block() { X = block.X, Y = block.Y, Z = block.Z - 1 };
                    cuboid.AddBlock(newBlock);
                    blocksOuter.Add(newBlock);
                }
            }
            cuboid.BlocksOuter = blocksOuter;
        }

        private class Block {
            public int X;
            public int Y;
            public int Z;
        }

        private class Cuboid {
            public Dictionary<int, Dictionary<int, Dictionary<int, Block>>> BlocksAll = new Dictionary<int, Dictionary<int, Dictionary<int, Block>>>();
            public List<Block> BlocksOuter = new List<Block>();

            public void AddBlock(Block block) {
                if (!BlocksAll.ContainsKey(block.X)) {
                    BlocksAll.Add(block.X, new Dictionary<int, Dictionary<int, Block>>());
                }
                if (!BlocksAll[block.X].ContainsKey(block.Y)) {
                    BlocksAll[block.X].Add(block.Y, new Dictionary<int, Block>());
                }
                BlocksAll[block.X][block.Y].Add(block.Z, block);
            }
        }
    }
}
