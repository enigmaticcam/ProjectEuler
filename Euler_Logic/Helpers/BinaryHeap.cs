using System;
using System.Collections.Generic;

namespace Euler_Logic.Helpers {
    public class BinaryHeap_Min {
        private List<Node> _nums = new List<Node>();
        private int _lastDeletedIndex;

        public Node Top => _nums[0];

        public void Reset() {
            _lastDeletedIndex = _nums.Count;
        }

        public Node Add(int num) {
            var node = new Node() { Index = _nums.Count, Num = num };
            _nums.Add(node);
            MoveUp(node.Index);
            return node;
        }

        public void Add(Node node) {
            node.Index = _nums.Count;
            _nums.Add(node);
            MoveUp(node.Index);
        }

        public void Remove(int index) {
            if (_lastDeletedIndex == 0) {
                _lastDeletedIndex = _nums.Count - 1;
            } else {
                _lastDeletedIndex--;
            }
            var temp = _nums[index];
            _nums[index] = _nums[_lastDeletedIndex];
            _nums[_lastDeletedIndex] = temp;
            _nums[index].Index = index;
            _nums[_lastDeletedIndex].Index = _lastDeletedIndex;
            if (_lastDeletedIndex > 0) {
                MoveDown(index);
            }
        }

        private bool MoveUp(int index) {
            var item = _nums[index];
            var parentIndex = (index - 1) / 2;
            var parent = _nums[parentIndex];
            if (parent.Num > item.Num) {
                _nums[index] = parent;
                _nums[parentIndex] = item;
                item.Index = parentIndex;
                parent.Index = index;
                MoveUp(parentIndex);
                return true;
            }
            return false;
        }

        private bool MoveDown(int index) {
            var item = _nums[index];
            var childIndex1 = index * 2 + 1;
            var childIndex2 = childIndex1 + 1;
            Node child1 = null;
            Node child2 = null;
            if (childIndex1 < _lastDeletedIndex) {
                child1 = _nums[childIndex1];
                if (childIndex2 < _lastDeletedIndex) {
                    child2 = _nums[childIndex2];
                }
            }
            Node smaller = child1;
            Node bigger = child2;
            if (child1 != null && child2 != null && child2.Num < child1.Num) {
                smaller = child2;
                bigger = child1;
            }
            if (smaller != null && item.Num > smaller.Num) {
                _nums[smaller.Index] = item;
                _nums[index] = smaller;
                var temp = smaller.Index;
                smaller.Index = index;
                item.Index = temp;
                MoveDown(item.Index);
                return true;
            } else if (bigger != null && item.Num > bigger.Num) {
                _nums[bigger.Index] = item;
                _nums[index] = bigger;
                var temp = bigger.Index;
                bigger.Index = index;
                item.Index = temp;
                MoveDown(item.Index);
                return true;
            }
            return false;
        }

        public void Adjust(int index) {
            if (!MoveUp(index)) {
                MoveDown(index);
            }
        }

        public int Validate() {
            int count = 0;
            for (int index = 0; index < _nums.Count; index++) {
                var item = _nums[index];
                if (item.Index != index) {
                    throw new Exception("Index Validation Failed");
                }
                var childIndex = index * 2 + 1;
                if (childIndex < _nums.Count) {
                    var child = _nums[childIndex];
                    if (child.Num < item.Num) {
                        count++;
                    }
                }
                childIndex++;
                if (childIndex < _nums.Count) {
                    var child = _nums[childIndex];
                    if (child.Num < item.Num) {
                        count++;
                    }
                }
            }
            return count;
        }

        public class Node {
            public int Num { get; set; }
            public int Index { get; set; }
        }

        public static void PerformTest(int maxCount) {
            var heap = new BinaryHeap_Min();
            var random = new Random();
            var nums = new List<Node>();
            for (int count = 1; count <= maxCount; count++) {
                nums.Add(heap.Add(random.Next(1000)));
            }
            var result = heap.Validate();
            foreach (var num in nums) {
                num.Num = random.Next(1000);
                heap.Adjust(num.Index);
                result = heap.Validate();
            }
        }
    }
}
