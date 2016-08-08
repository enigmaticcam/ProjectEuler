using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem84 : IProblem {
        private Dictionary<int, enumSquareType> _squareTypes = new Dictionary<int, enumSquareType>();
        private Random _random = new Random();
        private Square[] _squareCounts;
        private int _ccIndex = 15;
        private int _chanceIndex = 15;
        private List<Card> _cardsCC = new List<Card>();
        private List<Card> _cardsChance = new List<Card>();

        private enum enumSquareType {
            GotoJail,
            CC,
            Chance
        }

        public string ProblemName {
            get { return "84: Monopoly Odds"; }
        }

        public string GetAnswer() {
            BuildSquares();
            BuildSquareTypes();
            BuildCards();
            Simulate(10000000, 4);
            return GetAverage().ToString();
        }

        private void BuildSquares() {
            _squareCounts = new Square[40];
            _squareCounts[0] = new Square("00");
            _squareCounts[1] = new Square("01");
            _squareCounts[2] = new Square("02");
            _squareCounts[3] = new Square("03");
            _squareCounts[4] = new Square("04");
            _squareCounts[5] = new Square("05");
            _squareCounts[6] = new Square("06");
            _squareCounts[7] = new Square("07");
            _squareCounts[8] = new Square("08");
            _squareCounts[9] = new Square("09");
            _squareCounts[10] = new Square("10");
            _squareCounts[11] = new Square("11");
            _squareCounts[12] = new Square("12");
            _squareCounts[13] = new Square("13");
            _squareCounts[14] = new Square("14");
            _squareCounts[15] = new Square("15");
            _squareCounts[16] = new Square("16");
            _squareCounts[17] = new Square("17");
            _squareCounts[18] = new Square("18");
            _squareCounts[19] = new Square("19");
            _squareCounts[20] = new Square("20");
            _squareCounts[21] = new Square("21");
            _squareCounts[22] = new Square("22");
            _squareCounts[23] = new Square("23");
            _squareCounts[24] = new Square("24");
            _squareCounts[25] = new Square("25");
            _squareCounts[26] = new Square("26");
            _squareCounts[27] = new Square("27");
            _squareCounts[28] = new Square("28");
            _squareCounts[29] = new Square("29");
            _squareCounts[30] = new Square("30");
            _squareCounts[31] = new Square("31");
            _squareCounts[32] = new Square("32");
            _squareCounts[33] = new Square("33");
            _squareCounts[34] = new Square("34");
            _squareCounts[35] = new Square("35");
            _squareCounts[36] = new Square("36");
            _squareCounts[37] = new Square("37");
            _squareCounts[38] = new Square("38");
            _squareCounts[39] = new Square("39");
        }

        private void BuildSquareTypes() {
            _squareTypes.Add(30, enumSquareType.GotoJail);
            _squareTypes.Add(2, enumSquareType.CC);
            _squareTypes.Add(17, enumSquareType.CC);
            _squareTypes.Add(33, enumSquareType.CC);
            _squareTypes.Add(7, enumSquareType.Chance);
            _squareTypes.Add(22, enumSquareType.Chance);
            _squareTypes.Add(36, enumSquareType.Chance);
        }

        private void BuildCards() {
            _cardsCC.Add(new CardGoToGo());
            _cardsCC.Add(new CardGoToJail());
            _cardsCC.Add(new CardDoNothing());
            _cardsCC.Add(new CardDoNothing());
            _cardsCC.Add(new CardDoNothing());
            _cardsCC.Add(new CardDoNothing());
            _cardsCC.Add(new CardDoNothing());
            _cardsCC.Add(new CardDoNothing());
            _cardsCC.Add(new CardDoNothing());
            _cardsCC.Add(new CardDoNothing());
            _cardsCC.Add(new CardDoNothing());
            _cardsCC.Add(new CardDoNothing());
            _cardsCC.Add(new CardDoNothing());
            _cardsCC.Add(new CardDoNothing());
            _cardsCC.Add(new CardDoNothing());
            _cardsCC.Add(new CardDoNothing());
            _cardsChance.Add(new CardGoToGo());
            _cardsChance.Add(new CardGoToJail());
            _cardsChance.Add(new CardGoToC1());
            _cardsChance.Add(new CardGoToE3());
            _cardsChance.Add(new CardGoToH2());
            _cardsChance.Add(new CardGoToR1());
            _cardsChance.Add(new CardGoToNearestR());
            _cardsChance.Add(new CardGoToNearestR());
            _cardsChance.Add(new CardGoToNearestU());
            _cardsChance.Add(new CardGoBack3());
            _cardsChance.Add(new CardDoNothing());
            _cardsChance.Add(new CardDoNothing());
            _cardsChance.Add(new CardDoNothing());
            _cardsChance.Add(new CardDoNothing());
            _cardsChance.Add(new CardDoNothing());
            _cardsChance.Add(new CardDoNothing());
        }

        private void Simulate(int maxCount, int maxDieNum) {
            int square = 0;
            int doubleCount = 0;
            for (int count = 0; count < maxCount; count++) {
                int die1 = _random.Next(1, maxDieNum + 1);
                int die2 = _random.Next(1, maxDieNum + 1);
                int newSquare = (die1 + die2 + square) % 40;
                if (die1 == die2) {
                    doubleCount++;
                    if (doubleCount == 3) {
                        newSquare = 10;
                        doubleCount = 0;
                    }
                } else {
                    doubleCount = 0;
                }
                int oldSquare = -1;
                while (_squareTypes.ContainsKey(newSquare) && oldSquare != newSquare) {
                    oldSquare = newSquare;
                    newSquare = PerformSquareRedirect(newSquare);
                }
                square = newSquare;
                _squareCounts[newSquare].Count += 1;
            }
        }

        private int PerformSquareRedirect(int currentSquare) {
            int newSquare = currentSquare;
            switch (_squareTypes[currentSquare]) {
                case enumSquareType.GotoJail:
                    newSquare = 10;
                    break;
                case enumSquareType.CC:
                    _ccIndex = (_ccIndex + 1) % 16;
                    newSquare = _cardsCC[_ccIndex].Redirect(currentSquare);
                    break;
                case enumSquareType.Chance:
                    _chanceIndex = (_chanceIndex + 1) % 16;
                    newSquare = _cardsChance[_chanceIndex].Redirect(currentSquare);
                    break;
            }
            return newSquare;
        }

        private string GetAverage() {
            var squareCounts = _squareCounts.OrderByDescending(x => x.Count);
            return squareCounts.ElementAt(0).ShortCode + squareCounts.ElementAt(1).ShortCode + squareCounts.ElementAt(2).ShortCode;
        }

        private class Square {
            public double Count { get; set; }
            public string ShortCode { get; set; }

            public Square(string shortCode) {
                this.ShortCode = shortCode;
            }
        }

        private abstract class Card {
            public abstract int Redirect(int currentSquare);
        }

        private class CardDoNothing : Card {
            public override int Redirect(int currentSquare) {
                return currentSquare;
            }
        }

        private class CardGoToGo : Card {
            public override int Redirect(int currentSquare) {
                return 0;
            }
        }

        private class CardGoToJail : Card {
            public override int Redirect(int currentSquare) {
                return 10;
            }
        }

        private class CardGoToC1 : Card {
            public override int Redirect(int currentSquare) {
                return 11;
            }
        }

        private class CardGoToE3 : Card {
            public override int Redirect(int currentSquare) {
                return 24;
            }
        }

        private class CardGoToH2 : Card {
            public override int Redirect(int currentSquare) {
                return 39;
            }
        }

        private class CardGoToR1 : Card {
            public override int Redirect(int currentSquare) {
                return 5;
            }
        }

        private class CardGoToNearestR : Card {
            public override int Redirect(int currentSquare) {
                switch (currentSquare) {
                    case 7:
                        return 15;
                    case 22:
                        return 25;
                    case 36:
                        return 5;
                    default:
                        throw new Exception("This shouldn't happen");
                }
            }
        }

        private class CardGoToNearestU : Card {
            public override int Redirect(int currentSquare) {
                switch (currentSquare) {
                    case 7:
                        return 12;
                    case 22:
                        return 28;
                    case 36:
                        return 12;
                    default:
                        throw new Exception("This shouldn't happen");

                }
            }
        }

        private class CardGoBack3 : Card {
            public override int Redirect(int currentSquare) {
                switch (currentSquare) {
                    case 7:
                        return 4;
                    case 22:
                        return 19;
                    case 36:
                        return 33;
                    default:
                        throw new Exception("This shouldn't happen");

                }
            }
        }
    }
}
