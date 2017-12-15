using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems.AdventOfCode.Y2017 {
    public class Problem03 : ProblemBase {
        public override string ProblemName {
            get { return "Advent of Code 2017: 3"; }
        }

        public override string GetAnswer() {
            return Answer2(325489);
        }

        private int _x;
        private int _y;
        private int _offset;
        private int _num;
        private string Answer1(int number) {
            int direction = 0;
            int beforeNum = 0;
            _num = 1;
            do {
                beforeNum = _num;
                Next(direction);
                if (_num >= number) {
                    return FindNum(direction, number).ToString();
                }
                direction = (direction + 1) % 4;
            } while (true);
        }

        private void Next(int direction) {
            switch (direction) {
                case 0:
                    _offset++;
                    _x += _offset;
                    break;
                case 1:
                    _y -= _offset;
                    break;
                case 2:
                    _offset++;
                    _x -= _offset;
                    break;
                case 3:
                    _y += _offset;
                    break;
            }
            _num += _offset;
        }

        private int FindNum(int direction, int num) {
            switch (direction) {
                case 0:
                    return Math.Abs(_x - (_num - num)) + Math.Abs(_y);
                case 1:
                    return Math.Abs(_y + (_num - num)) + Math.Abs(_x);
                case 2:
                    return Math.Abs(_x + (_num - num)) + Math.Abs(_y);
                case 3:
                    return Math.Abs(_y - (_num - num)) + Math.Abs(_y);
                default:
                    throw new Exception();
            }
        }
        
        private int _minX = 0;
        private int _maxX = 0;
        private int _minY = 0;
        private int _maxY = 0;
        private int _direction = 0;
        private Dictionary<string, int> _grid = new Dictionary<string, int>();
        private string Answer2(int max) {
            _grid.Add("0:0", 1);
            do {
                MoveNext();
                int sum = 0;
                if (_grid.ContainsKey((_x - 1).ToString() + ":" + (_y - 1).ToString())) {
                    sum += _grid[(_x - 1).ToString() + ":" + (_y - 1)];
                }
                if (_grid.ContainsKey((_x).ToString() + ":" + (_y - 1).ToString())) {
                    sum += _grid[(_x).ToString() + ":" + (_y - 1)];
                }
                if (_grid.ContainsKey((_x + 1).ToString() + ":" + (_y - 1).ToString())) {
                    sum += _grid[(_x + 1).ToString() + ":" + (_y - 1)];
                }
                if (_grid.ContainsKey((_x - 1).ToString() + ":" + (_y).ToString())) {
                    sum += _grid[(_x - 1).ToString() + ":" + (_y)];
                }
                if (_grid.ContainsKey((_x + 1).ToString() + ":" + (_y).ToString())) {
                    sum += _grid[(_x + 1).ToString() + ":" + (_y)];
                }
                if (_grid.ContainsKey((_x - 1).ToString() + ":" + (_y + 1).ToString())) {
                    sum += _grid[(_x - 1).ToString() + ":" + (_y + 1)];
                }
                if (_grid.ContainsKey((_x).ToString() + ":" + (_y + 1).ToString())) {
                    sum += _grid[(_x).ToString() + ":" + (_y + 1)];
                }
                if (_grid.ContainsKey((_x + 1).ToString() + ":" + (_y + 1).ToString())) {
                    sum += _grid[(_x + 1).ToString() + ":" + (_y + 1)];
                }
                _grid.Add(_x + ":" + _y, sum);
                if (sum > max) {
                    return sum.ToString();
                }
            } while (true);
        }

        private void MoveNext() {
            switch (_direction) {
                case 0:
                    _x++;
                    if (_x > _maxX) {
                        _maxX++;
                        _direction = 1;
                    }
                    break;
                case 1:
                    _y--;
                    if (_y < _minY) {
                        _minY--;
                        _direction = 2;
                    }
                    break;
                case 2:
                    _x--;
                    if (_x < _minX) {
                        _minX--;
                        _direction = 3;
                    }
                    break;
                case 3:
                    _y++;
                    if (_y > _maxY) {
                        _maxY++;
                        _direction = 0;
                    }
                    break;
            }
        }
        
    }
}
