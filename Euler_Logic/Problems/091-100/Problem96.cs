using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem96 : ProblemBase {
        private List<List<string>> _games = new List<List<string>>();
        private Dictionary<string, int> _numToBits = new Dictionary<string, int>();
        private List<Dictionary<int, int>> _rowColToRegion = new List<Dictionary<int, int>>();
        private HashSet<int> _solvedCell = new HashSet<int>();
        private int[] _rows;
        private int[] _cols;
        private int[] _regions;
        private int[][] _grid;
        private bool _isBad;
        private int _sum;

        public override string ProblemName {
            get { return "96: Su Doku"; }
        }

        public override string GetAnswer() {
            LoadAllGames();
            Initialize();
            return FindSum().ToString();
        }

        private void Initialize() {
            _grid = new int[9][];
            _rowColToRegion = new List<Dictionary<int, int>>();
            for (int index = 0; index <=8; index++) {
                _grid[index] = new int[9];
                _rowColToRegion.Add(new Dictionary<int, int>());
            }
            _rows = new int[9];
            _cols = new int[9];
            _regions = new int[9];
            BuildNumToBits();
            BuildRowColToRegion();
            BuildSolvedCell();
        }

        private void BuildNumToBits() {
            _numToBits.Add("1", 1);
            _numToBits.Add("2", 2);
            _numToBits.Add("3", 4);
            _numToBits.Add("4", 8);
            _numToBits.Add("5", 16);
            _numToBits.Add("6", 32);
            _numToBits.Add("7", 64);
            _numToBits.Add("8", 128);
            _numToBits.Add("9", 256);
        }

        private void BuildRowColToRegion() {
            for (int row = 0; row <= 8; row++) {
                for (int col = 0; col <= 8; col++) {
                    if (row < 3) {
                        if (col < 3) {
                            _rowColToRegion[row].Add(col, 0);
                        } else if (col < 6) {
                            _rowColToRegion[row].Add(col, 1);
                        } else {
                            _rowColToRegion[row].Add(col, 2);
                        }
                    } else if (row < 6) {
                        if (col < 3) {
                            _rowColToRegion[row].Add(col, 3);
                        } else if (col < 6) {
                            _rowColToRegion[row].Add(col, 4);
                        } else {
                            _rowColToRegion[row].Add(col, 5);
                        }
                    } else {
                        if (col < 3) {
                            _rowColToRegion[row].Add(col, 6);
                        } else if (col < 6) {
                            _rowColToRegion[row].Add(col, 7);
                        } else {
                            _rowColToRegion[row].Add(col, 8);
                        }
                    }
                }
            }
        }

        private void BuildSolvedCell() {
            _solvedCell.Add(1);
            _solvedCell.Add(2);
            _solvedCell.Add(4);
            _solvedCell.Add(8);
            _solvedCell.Add(16);
            _solvedCell.Add(32);
            _solvedCell.Add(64);
            _solvedCell.Add(128);
            _solvedCell.Add(256);
        }

        private int FindSum() {
            _sum = 0;
            foreach (List<string> game in _games) {
                SolveGame(game);
            }
            return _sum;
        }

        private void SolveGame(List<string> game) {
            ResetArrays();
            PopulateGrid(game);
            Solve(_rows, _cols, _regions, _grid);
        }

        private void ResetArrays() {
            for (int index = 0; index <= 8; index++) {
                _rows[index] = 0;
                _cols[index] = 0;
                _regions[index] = 0;
            }
        }

        private void PopulateGrid(List<string> game) {
            for (int row = 0; row <= 8; row++) {
                for (int col = 0; col <= 8; col++) {
                    _grid[row][col] = 0;
                    string digit = game[row].Substring(col, 1);
                    if (digit != "0") {
                        int bits = _numToBits[digit];
                        AddNumber(bits, row, col, _rows, _cols, _regions, _grid);
                    }
                }
            }
        }

        private void AddNumber(int bits, int row, int col, int[] rows, int[] cols, int[] regions, int[][] grid) {
            rows[row] += bits;
            cols[col] += bits;
            regions[_rowColToRegion[row][col]] += bits;
            grid[row][col] = bits;
            
        }

        private bool Solve(int[] rows, int[] cols, int[] regions, int[][] grid) {
            bool isSolved = Deduce(rows, cols, regions, grid);
            if (isSolved) {
                _sum += (((int)Math.Log(grid[0][0], 2) + 1) * 100) + (((int)Math.Log(grid[0][1], 2) + 1) * 10) + ((int)Math.Log(grid[0][2], 2) + 1);
                return true;
            } else if (!_isBad) {
                int row = 0;
                int col = 0;
                do {
                    if (!_solvedCell.Contains(grid[row][col])) {
                        int oldGrid = grid[row][col];
                        int oldRows = rows[row];
                        int oldCols = cols[col];
                        int oldRegion = regions[_rowColToRegion[row][col]];
                        foreach (int numToTry in _solvedCell) {
                            if ((numToTry & grid[row][col]) != 0) {
                                AddNumber(numToTry, row, col, rows, cols, regions, grid);
                                isSolved = Solve((int[])rows.Clone(), (int[])cols.Clone(), (int[])regions.Clone(), GridCopy(grid));
                                if (isSolved) {
                                    return true;
                                } else {
                                    grid[row][col] = oldGrid;
                                    rows[row] = oldRows;
                                    cols[col] = oldCols;
                                    regions[_rowColToRegion[row][col]] = oldRegion;
                                }
                            }
                        }
                        if (!isSolved) {
                            return false;
                        }
                    }
                    if (col == 8) {
                        col = 0;
                        row++;
                    } else {
                        col++;
                    }
                } while (true);
            } else {
                return false;
            }
        }

        private bool Deduce(int[] rows, int[] cols, int[] regions, int[][] grid) {
            bool isSolved = true;
            bool wasAbleToDeduce = false;
            do {
                _isBad = false;
                isSolved = true;
                wasAbleToDeduce = false;
                for (int row = 0; row <= 8; row++) {
                    for (int col = 0; col <= 8; col++) {
                        if (!_solvedCell.Contains(grid[row][col])) {
                            grid[row][col] = (511 ^ ((rows[row] | cols[col]) | regions[_rowColToRegion[row][col]]));
                            if (_solvedCell.Contains(grid[row][col])) {
                                AddNumber(grid[row][col], row, col, rows, cols, regions, grid);
                                wasAbleToDeduce = true;
                            } else if (grid[row][col] == 0 || (grid[row][col] & rows[row]) != 0 || (grid[row][col] & cols[col]) != 0 || (grid[row][col] & regions[_rowColToRegion[row][col]]) != 0) {
                                _isBad = true;
                                return false;
                            } else if (isSolved && !_solvedCell.Contains(grid[row][col])) {
                                isSolved = false;
                            }
                        }
                    }
                }
            } while (wasAbleToDeduce);
            return isSolved;
        }

        private int[][] GridCopy(int[][] grid) {
            int[][] newGrid = new int[9][];
            for (int row = 0; row <= 8; row++) {
                newGrid[row] = new int[9];
                for (int col = 0; col <= 8; col++) {
                    newGrid[row][col] = grid[row][col];
                }
            }
            return newGrid;
        }

        private string Output(int[][] grid) {
            StringBuilder text = new StringBuilder();
            for (int row = 0; row <= 8; row++) {
                for (int col = 0; col <= 8; col++) {
                    if (_solvedCell.Contains(grid[row][col])) {
                        text.Append("\t" + (Math.Log(grid[row][col], 2) + 1).ToString());
                    } else {
                        text.Append("\t0");
                    }
                }
                text.AppendLine("");
            }
            return text.ToString();
        }

        private void LoadTestGame() {
            _games.Add(new List<string>());
            _games[_games.Count - 1].Add("003020600");
            _games[_games.Count - 1].Add("900305001");
            _games[_games.Count - 1].Add("001806400");
            _games[_games.Count - 1].Add("008102900");
            _games[_games.Count - 1].Add("700000008");
            _games[_games.Count - 1].Add("006708200");
            _games[_games.Count - 1].Add("002609500");
            _games[_games.Count - 1].Add("800203009");
            _games[_games.Count - 1].Add("005010300");
        }

        private void LoadAllGames() {
            _games.Add(new List<string>());
            _games[_games.Count - 1].Add("003020600");
            _games[_games.Count - 1].Add("900305001");
            _games[_games.Count - 1].Add("001806400");
            _games[_games.Count - 1].Add("008102900");
            _games[_games.Count - 1].Add("700000008");
            _games[_games.Count - 1].Add("006708200");
            _games[_games.Count - 1].Add("002609500");
            _games[_games.Count - 1].Add("800203009");
            _games[_games.Count - 1].Add("005010300");
            _games.Add(new List<string>());
            _games[_games.Count - 1].Add("200080300");
            _games[_games.Count - 1].Add("060070084");
            _games[_games.Count - 1].Add("030500209");
            _games[_games.Count - 1].Add("000105408");
            _games[_games.Count - 1].Add("000000000");
            _games[_games.Count - 1].Add("402706000");
            _games[_games.Count - 1].Add("301007040");
            _games[_games.Count - 1].Add("720040060");
            _games[_games.Count - 1].Add("004010003");
            _games.Add(new List<string>());
            _games[_games.Count - 1].Add("000000907");
            _games[_games.Count - 1].Add("000420180");
            _games[_games.Count - 1].Add("000705026");
            _games[_games.Count - 1].Add("100904000");
            _games[_games.Count - 1].Add("050000040");
            _games[_games.Count - 1].Add("000507009");
            _games[_games.Count - 1].Add("920108000");
            _games[_games.Count - 1].Add("034059000");
            _games[_games.Count - 1].Add("507000000");
            _games.Add(new List<string>());
            _games[_games.Count - 1].Add("030050040");
            _games[_games.Count - 1].Add("008010500");
            _games[_games.Count - 1].Add("460000012");
            _games[_games.Count - 1].Add("070502080");
            _games[_games.Count - 1].Add("000603000");
            _games[_games.Count - 1].Add("040109030");
            _games[_games.Count - 1].Add("250000098");
            _games[_games.Count - 1].Add("001020600");
            _games[_games.Count - 1].Add("080060020");
            _games.Add(new List<string>());
            _games[_games.Count - 1].Add("020810740");
            _games[_games.Count - 1].Add("700003100");
            _games[_games.Count - 1].Add("090002805");
            _games[_games.Count - 1].Add("009040087");
            _games[_games.Count - 1].Add("400208003");
            _games[_games.Count - 1].Add("160030200");
            _games[_games.Count - 1].Add("302700060");
            _games[_games.Count - 1].Add("005600008");
            _games[_games.Count - 1].Add("076051090");
            _games.Add(new List<string>());
            _games[_games.Count - 1].Add("100920000");
            _games[_games.Count - 1].Add("524010000");
            _games[_games.Count - 1].Add("000000070");
            _games[_games.Count - 1].Add("050008102");
            _games[_games.Count - 1].Add("000000000");
            _games[_games.Count - 1].Add("402700090");
            _games[_games.Count - 1].Add("060000000");
            _games[_games.Count - 1].Add("000030945");
            _games[_games.Count - 1].Add("000071006");
            _games.Add(new List<string>());
            _games[_games.Count - 1].Add("043080250");
            _games[_games.Count - 1].Add("600000000");
            _games[_games.Count - 1].Add("000001094");
            _games[_games.Count - 1].Add("900004070");
            _games[_games.Count - 1].Add("000608000");
            _games[_games.Count - 1].Add("010200003");
            _games[_games.Count - 1].Add("820500000");
            _games[_games.Count - 1].Add("000000005");
            _games[_games.Count - 1].Add("034090710");
            _games.Add(new List<string>());
            _games[_games.Count - 1].Add("480006902");
            _games[_games.Count - 1].Add("002008001");
            _games[_games.Count - 1].Add("900370060");
            _games[_games.Count - 1].Add("840010200");
            _games[_games.Count - 1].Add("003704100");
            _games[_games.Count - 1].Add("001060049");
            _games[_games.Count - 1].Add("020085007");
            _games[_games.Count - 1].Add("700900600");
            _games[_games.Count - 1].Add("609200018");
            _games.Add(new List<string>());
            _games[_games.Count - 1].Add("000900002");
            _games[_games.Count - 1].Add("050123400");
            _games[_games.Count - 1].Add("030000160");
            _games[_games.Count - 1].Add("908000000");
            _games[_games.Count - 1].Add("070000090");
            _games[_games.Count - 1].Add("000000205");
            _games[_games.Count - 1].Add("091000050");
            _games[_games.Count - 1].Add("007439020");
            _games[_games.Count - 1].Add("400007000");
            _games.Add(new List<string>());
            _games[_games.Count - 1].Add("001900003");
            _games[_games.Count - 1].Add("900700160");
            _games[_games.Count - 1].Add("030005007");
            _games[_games.Count - 1].Add("050000009");
            _games[_games.Count - 1].Add("004302600");
            _games[_games.Count - 1].Add("200000070");
            _games[_games.Count - 1].Add("600100030");
            _games[_games.Count - 1].Add("042007006");
            _games[_games.Count - 1].Add("500006800");
            _games.Add(new List<string>());
            _games[_games.Count - 1].Add("000125400");
            _games[_games.Count - 1].Add("008400000");
            _games[_games.Count - 1].Add("420800000");
            _games[_games.Count - 1].Add("030000095");
            _games[_games.Count - 1].Add("060902010");
            _games[_games.Count - 1].Add("510000060");
            _games[_games.Count - 1].Add("000003049");
            _games[_games.Count - 1].Add("000007200");
            _games[_games.Count - 1].Add("001298000");
            _games.Add(new List<string>());
            _games[_games.Count - 1].Add("062340750");
            _games[_games.Count - 1].Add("100005600");
            _games[_games.Count - 1].Add("570000040");
            _games[_games.Count - 1].Add("000094800");
            _games[_games.Count - 1].Add("400000006");
            _games[_games.Count - 1].Add("005830000");
            _games[_games.Count - 1].Add("030000091");
            _games[_games.Count - 1].Add("006400007");
            _games[_games.Count - 1].Add("059083260");
            _games.Add(new List<string>());
            _games[_games.Count - 1].Add("300000000");
            _games[_games.Count - 1].Add("005009000");
            _games[_games.Count - 1].Add("200504000");
            _games[_games.Count - 1].Add("020000700");
            _games[_games.Count - 1].Add("160000058");
            _games[_games.Count - 1].Add("704310600");
            _games[_games.Count - 1].Add("000890100");
            _games[_games.Count - 1].Add("000067080");
            _games[_games.Count - 1].Add("000005437");
            _games.Add(new List<string>());
            _games[_games.Count - 1].Add("630000000");
            _games[_games.Count - 1].Add("000500008");
            _games[_games.Count - 1].Add("005674000");
            _games[_games.Count - 1].Add("000020000");
            _games[_games.Count - 1].Add("003401020");
            _games[_games.Count - 1].Add("000000345");
            _games[_games.Count - 1].Add("000007004");
            _games[_games.Count - 1].Add("080300902");
            _games[_games.Count - 1].Add("947100080");
            _games.Add(new List<string>());
            _games[_games.Count - 1].Add("000020040");
            _games[_games.Count - 1].Add("008035000");
            _games[_games.Count - 1].Add("000070602");
            _games[_games.Count - 1].Add("031046970");
            _games[_games.Count - 1].Add("200000000");
            _games[_games.Count - 1].Add("000501203");
            _games[_games.Count - 1].Add("049000730");
            _games[_games.Count - 1].Add("000000010");
            _games[_games.Count - 1].Add("800004000");
            _games.Add(new List<string>());
            _games[_games.Count - 1].Add("361025900");
            _games[_games.Count - 1].Add("080960010");
            _games[_games.Count - 1].Add("400000057");
            _games[_games.Count - 1].Add("008000471");
            _games[_games.Count - 1].Add("000603000");
            _games[_games.Count - 1].Add("259000800");
            _games[_games.Count - 1].Add("740000005");
            _games[_games.Count - 1].Add("020018060");
            _games[_games.Count - 1].Add("005470329");
            _games.Add(new List<string>());
            _games[_games.Count - 1].Add("050807020");
            _games[_games.Count - 1].Add("600010090");
            _games[_games.Count - 1].Add("702540006");
            _games[_games.Count - 1].Add("070020301");
            _games[_games.Count - 1].Add("504000908");
            _games[_games.Count - 1].Add("103080070");
            _games[_games.Count - 1].Add("900076205");
            _games[_games.Count - 1].Add("060090003");
            _games[_games.Count - 1].Add("080103040");
            _games.Add(new List<string>());
            _games[_games.Count - 1].Add("080005000");
            _games[_games.Count - 1].Add("000003457");
            _games[_games.Count - 1].Add("000070809");
            _games[_games.Count - 1].Add("060400903");
            _games[_games.Count - 1].Add("007010500");
            _games[_games.Count - 1].Add("408007020");
            _games[_games.Count - 1].Add("901020000");
            _games[_games.Count - 1].Add("842300000");
            _games[_games.Count - 1].Add("000100080");
            _games.Add(new List<string>());
            _games[_games.Count - 1].Add("003502900");
            _games[_games.Count - 1].Add("000040000");
            _games[_games.Count - 1].Add("106000305");
            _games[_games.Count - 1].Add("900251008");
            _games[_games.Count - 1].Add("070408030");
            _games[_games.Count - 1].Add("800763001");
            _games[_games.Count - 1].Add("308000104");
            _games[_games.Count - 1].Add("000020000");
            _games[_games.Count - 1].Add("005104800");
            _games.Add(new List<string>());
            _games[_games.Count - 1].Add("000000000");
            _games[_games.Count - 1].Add("009805100");
            _games[_games.Count - 1].Add("051907420");
            _games[_games.Count - 1].Add("290401065");
            _games[_games.Count - 1].Add("000000000");
            _games[_games.Count - 1].Add("140508093");
            _games[_games.Count - 1].Add("026709580");
            _games[_games.Count - 1].Add("005103600");
            _games[_games.Count - 1].Add("000000000");
            _games.Add(new List<string>());
            _games[_games.Count - 1].Add("020030090");
            _games[_games.Count - 1].Add("000907000");
            _games[_games.Count - 1].Add("900208005");
            _games[_games.Count - 1].Add("004806500");
            _games[_games.Count - 1].Add("607000208");
            _games[_games.Count - 1].Add("003102900");
            _games[_games.Count - 1].Add("800605007");
            _games[_games.Count - 1].Add("000309000");
            _games[_games.Count - 1].Add("030020050");
            _games.Add(new List<string>());
            _games[_games.Count - 1].Add("005000006");
            _games[_games.Count - 1].Add("070009020");
            _games[_games.Count - 1].Add("000500107");
            _games[_games.Count - 1].Add("804150000");
            _games[_games.Count - 1].Add("000803000");
            _games[_games.Count - 1].Add("000092805");
            _games[_games.Count - 1].Add("907006000");
            _games[_games.Count - 1].Add("030400010");
            _games[_games.Count - 1].Add("200000600");
            _games.Add(new List<string>());
            _games[_games.Count - 1].Add("040000050");
            _games[_games.Count - 1].Add("001943600");
            _games[_games.Count - 1].Add("009000300");
            _games[_games.Count - 1].Add("600050002");
            _games[_games.Count - 1].Add("103000506");
            _games[_games.Count - 1].Add("800020007");
            _games[_games.Count - 1].Add("005000200");
            _games[_games.Count - 1].Add("002436700");
            _games[_games.Count - 1].Add("030000040");
            _games.Add(new List<string>());
            _games[_games.Count - 1].Add("004000000");
            _games[_games.Count - 1].Add("000030002");
            _games[_games.Count - 1].Add("390700080");
            _games[_games.Count - 1].Add("400009001");
            _games[_games.Count - 1].Add("209801307");
            _games[_games.Count - 1].Add("600200008");
            _games[_games.Count - 1].Add("010008053");
            _games[_games.Count - 1].Add("900040000");
            _games[_games.Count - 1].Add("000000800");
            _games.Add(new List<string>());
            _games[_games.Count - 1].Add("360020089");
            _games[_games.Count - 1].Add("000361000");
            _games[_games.Count - 1].Add("000000000");
            _games[_games.Count - 1].Add("803000602");
            _games[_games.Count - 1].Add("400603007");
            _games[_games.Count - 1].Add("607000108");
            _games[_games.Count - 1].Add("000000000");
            _games[_games.Count - 1].Add("000418000");
            _games[_games.Count - 1].Add("970030014");
            _games.Add(new List<string>());
            _games[_games.Count - 1].Add("500400060");
            _games[_games.Count - 1].Add("009000800");
            _games[_games.Count - 1].Add("640020000");
            _games[_games.Count - 1].Add("000001008");
            _games[_games.Count - 1].Add("208000501");
            _games[_games.Count - 1].Add("700500000");
            _games[_games.Count - 1].Add("000090084");
            _games[_games.Count - 1].Add("003000600");
            _games[_games.Count - 1].Add("060003002");
            _games.Add(new List<string>());
            _games[_games.Count - 1].Add("007256400");
            _games[_games.Count - 1].Add("400000005");
            _games[_games.Count - 1].Add("010030060");
            _games[_games.Count - 1].Add("000508000");
            _games[_games.Count - 1].Add("008060200");
            _games[_games.Count - 1].Add("000107000");
            _games[_games.Count - 1].Add("030070090");
            _games[_games.Count - 1].Add("200000004");
            _games[_games.Count - 1].Add("006312700");
            _games.Add(new List<string>());
            _games[_games.Count - 1].Add("000000000");
            _games[_games.Count - 1].Add("079050180");
            _games[_games.Count - 1].Add("800000007");
            _games[_games.Count - 1].Add("007306800");
            _games[_games.Count - 1].Add("450708096");
            _games[_games.Count - 1].Add("003502700");
            _games[_games.Count - 1].Add("700000005");
            _games[_games.Count - 1].Add("016030420");
            _games[_games.Count - 1].Add("000000000");
            _games.Add(new List<string>());
            _games[_games.Count - 1].Add("030000080");
            _games[_games.Count - 1].Add("009000500");
            _games[_games.Count - 1].Add("007509200");
            _games[_games.Count - 1].Add("700105008");
            _games[_games.Count - 1].Add("020090030");
            _games[_games.Count - 1].Add("900402001");
            _games[_games.Count - 1].Add("004207100");
            _games[_games.Count - 1].Add("002000800");
            _games[_games.Count - 1].Add("070000090");
            _games.Add(new List<string>());
            _games[_games.Count - 1].Add("200170603");
            _games[_games.Count - 1].Add("050000100");
            _games[_games.Count - 1].Add("000006079");
            _games[_games.Count - 1].Add("000040700");
            _games[_games.Count - 1].Add("000801000");
            _games[_games.Count - 1].Add("009050000");
            _games[_games.Count - 1].Add("310400000");
            _games[_games.Count - 1].Add("005000060");
            _games[_games.Count - 1].Add("906037002");
            _games.Add(new List<string>());
            _games[_games.Count - 1].Add("000000080");
            _games[_games.Count - 1].Add("800701040");
            _games[_games.Count - 1].Add("040020030");
            _games[_games.Count - 1].Add("374000900");
            _games[_games.Count - 1].Add("000030000");
            _games[_games.Count - 1].Add("005000321");
            _games[_games.Count - 1].Add("010060050");
            _games[_games.Count - 1].Add("050802006");
            _games[_games.Count - 1].Add("080000000");
            _games.Add(new List<string>());
            _games[_games.Count - 1].Add("000000085");
            _games[_games.Count - 1].Add("000210009");
            _games[_games.Count - 1].Add("960080100");
            _games[_games.Count - 1].Add("500800016");
            _games[_games.Count - 1].Add("000000000");
            _games[_games.Count - 1].Add("890006007");
            _games[_games.Count - 1].Add("009070052");
            _games[_games.Count - 1].Add("300054000");
            _games[_games.Count - 1].Add("480000000");
            _games.Add(new List<string>());
            _games[_games.Count - 1].Add("608070502");
            _games[_games.Count - 1].Add("050608070");
            _games[_games.Count - 1].Add("002000300");
            _games[_games.Count - 1].Add("500090006");
            _games[_games.Count - 1].Add("040302050");
            _games[_games.Count - 1].Add("800050003");
            _games[_games.Count - 1].Add("005000200");
            _games[_games.Count - 1].Add("010704090");
            _games[_games.Count - 1].Add("409060701");
            _games.Add(new List<string>());
            _games[_games.Count - 1].Add("050010040");
            _games[_games.Count - 1].Add("107000602");
            _games[_games.Count - 1].Add("000905000");
            _games[_games.Count - 1].Add("208030501");
            _games[_games.Count - 1].Add("040070020");
            _games[_games.Count - 1].Add("901080406");
            _games[_games.Count - 1].Add("000401000");
            _games[_games.Count - 1].Add("304000709");
            _games[_games.Count - 1].Add("020060010");
            _games.Add(new List<string>());
            _games[_games.Count - 1].Add("053000790");
            _games[_games.Count - 1].Add("009753400");
            _games[_games.Count - 1].Add("100000002");
            _games[_games.Count - 1].Add("090080010");
            _games[_games.Count - 1].Add("000907000");
            _games[_games.Count - 1].Add("080030070");
            _games[_games.Count - 1].Add("500000003");
            _games[_games.Count - 1].Add("007641200");
            _games[_games.Count - 1].Add("061000940");
            _games.Add(new List<string>());
            _games[_games.Count - 1].Add("006080300");
            _games[_games.Count - 1].Add("049070250");
            _games[_games.Count - 1].Add("000405000");
            _games[_games.Count - 1].Add("600317004");
            _games[_games.Count - 1].Add("007000800");
            _games[_games.Count - 1].Add("100826009");
            _games[_games.Count - 1].Add("000702000");
            _games[_games.Count - 1].Add("075040190");
            _games[_games.Count - 1].Add("003090600");
            _games.Add(new List<string>());
            _games[_games.Count - 1].Add("005080700");
            _games[_games.Count - 1].Add("700204005");
            _games[_games.Count - 1].Add("320000084");
            _games[_games.Count - 1].Add("060105040");
            _games[_games.Count - 1].Add("008000500");
            _games[_games.Count - 1].Add("070803010");
            _games[_games.Count - 1].Add("450000091");
            _games[_games.Count - 1].Add("600508007");
            _games[_games.Count - 1].Add("003010600");
            _games.Add(new List<string>());
            _games[_games.Count - 1].Add("000900800");
            _games[_games.Count - 1].Add("128006400");
            _games[_games.Count - 1].Add("070800060");
            _games[_games.Count - 1].Add("800430007");
            _games[_games.Count - 1].Add("500000009");
            _games[_games.Count - 1].Add("600079008");
            _games[_games.Count - 1].Add("090004010");
            _games[_games.Count - 1].Add("003600284");
            _games[_games.Count - 1].Add("001007000");
            _games.Add(new List<string>());
            _games[_games.Count - 1].Add("000080000");
            _games[_games.Count - 1].Add("270000054");
            _games[_games.Count - 1].Add("095000810");
            _games[_games.Count - 1].Add("009806400");
            _games[_games.Count - 1].Add("020403060");
            _games[_games.Count - 1].Add("006905100");
            _games[_games.Count - 1].Add("017000620");
            _games[_games.Count - 1].Add("460000038");
            _games[_games.Count - 1].Add("000090000");
            _games.Add(new List<string>());
            _games[_games.Count - 1].Add("000602000");
            _games[_games.Count - 1].Add("400050001");
            _games[_games.Count - 1].Add("085010620");
            _games[_games.Count - 1].Add("038206710");
            _games[_games.Count - 1].Add("000000000");
            _games[_games.Count - 1].Add("019407350");
            _games[_games.Count - 1].Add("026040530");
            _games[_games.Count - 1].Add("900020007");
            _games[_games.Count - 1].Add("000809000");
            _games.Add(new List<string>());
            _games[_games.Count - 1].Add("000900002");
            _games[_games.Count - 1].Add("050123400");
            _games[_games.Count - 1].Add("030000160");
            _games[_games.Count - 1].Add("908000000");
            _games[_games.Count - 1].Add("070000090");
            _games[_games.Count - 1].Add("000000205");
            _games[_games.Count - 1].Add("091000050");
            _games[_games.Count - 1].Add("007439020");
            _games[_games.Count - 1].Add("400007000");
            _games.Add(new List<string>());
            _games[_games.Count - 1].Add("380000000");
            _games[_games.Count - 1].Add("000400785");
            _games[_games.Count - 1].Add("009020300");
            _games[_games.Count - 1].Add("060090000");
            _games[_games.Count - 1].Add("800302009");
            _games[_games.Count - 1].Add("000040070");
            _games[_games.Count - 1].Add("001070500");
            _games[_games.Count - 1].Add("495006000");
            _games[_games.Count - 1].Add("000000092");
            _games.Add(new List<string>());
            _games[_games.Count - 1].Add("000158000");
            _games[_games.Count - 1].Add("002060800");
            _games[_games.Count - 1].Add("030000040");
            _games[_games.Count - 1].Add("027030510");
            _games[_games.Count - 1].Add("000000000");
            _games[_games.Count - 1].Add("046080790");
            _games[_games.Count - 1].Add("050000080");
            _games[_games.Count - 1].Add("004070100");
            _games[_games.Count - 1].Add("000325000");
            _games.Add(new List<string>());
            _games[_games.Count - 1].Add("010500200");
            _games[_games.Count - 1].Add("900001000");
            _games[_games.Count - 1].Add("002008030");
            _games[_games.Count - 1].Add("500030007");
            _games[_games.Count - 1].Add("008000500");
            _games[_games.Count - 1].Add("600080004");
            _games[_games.Count - 1].Add("040100700");
            _games[_games.Count - 1].Add("000700006");
            _games[_games.Count - 1].Add("003004050");
            _games.Add(new List<string>());
            _games[_games.Count - 1].Add("080000040");
            _games[_games.Count - 1].Add("000469000");
            _games[_games.Count - 1].Add("400000007");
            _games[_games.Count - 1].Add("005904600");
            _games[_games.Count - 1].Add("070608030");
            _games[_games.Count - 1].Add("008502100");
            _games[_games.Count - 1].Add("900000005");
            _games[_games.Count - 1].Add("000781000");
            _games[_games.Count - 1].Add("060000010");
            _games.Add(new List<string>());
            _games[_games.Count - 1].Add("904200007");
            _games[_games.Count - 1].Add("010000000");
            _games[_games.Count - 1].Add("000706500");
            _games[_games.Count - 1].Add("000800090");
            _games[_games.Count - 1].Add("020904060");
            _games[_games.Count - 1].Add("040002000");
            _games[_games.Count - 1].Add("001607000");
            _games[_games.Count - 1].Add("000000030");
            _games[_games.Count - 1].Add("300005702");
            _games.Add(new List<string>());
            _games[_games.Count - 1].Add("000700800");
            _games[_games.Count - 1].Add("006000031");
            _games[_games.Count - 1].Add("040002000");
            _games[_games.Count - 1].Add("024070000");
            _games[_games.Count - 1].Add("010030080");
            _games[_games.Count - 1].Add("000060290");
            _games[_games.Count - 1].Add("000800070");
            _games[_games.Count - 1].Add("860000500");
            _games[_games.Count - 1].Add("002006000");
            _games.Add(new List<string>());
            _games[_games.Count - 1].Add("001007090");
            _games[_games.Count - 1].Add("590080001");
            _games[_games.Count - 1].Add("030000080");
            _games[_games.Count - 1].Add("000005800");
            _games[_games.Count - 1].Add("050060020");
            _games[_games.Count - 1].Add("004100000");
            _games[_games.Count - 1].Add("080000030");
            _games[_games.Count - 1].Add("100020079");
            _games[_games.Count - 1].Add("020700400");
            _games.Add(new List<string>());
            _games[_games.Count - 1].Add("000003017");
            _games[_games.Count - 1].Add("015009008");
            _games[_games.Count - 1].Add("060000000");
            _games[_games.Count - 1].Add("100007000");
            _games[_games.Count - 1].Add("009000200");
            _games[_games.Count - 1].Add("000500004");
            _games[_games.Count - 1].Add("000000020");
            _games[_games.Count - 1].Add("500600340");
            _games[_games.Count - 1].Add("340200000");
            _games.Add(new List<string>());
            _games[_games.Count - 1].Add("300200000");
            _games[_games.Count - 1].Add("000107000");
            _games[_games.Count - 1].Add("706030500");
            _games[_games.Count - 1].Add("070009080");
            _games[_games.Count - 1].Add("900020004");
            _games[_games.Count - 1].Add("010800050");
            _games[_games.Count - 1].Add("009040301");
            _games[_games.Count - 1].Add("000702000");
            _games[_games.Count - 1].Add("000008006");

        }
    }
}
