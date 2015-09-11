using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem19 : IProblem {
        public string ProblemName {
            get { return "19: Counting Sundays"; }
        }

        public string GetAnswer() {
            return CountSundays().ToString();
        }

        private int CountSundays() {
            int weekDay = 2;
            int day = 1;
            int month = 1;
            int year = 1900;
            int sundayCount = 0;
            int daysOfMonth = GetDaysOfMonth(year, month);

            do {
                if (year >= 1901 && day == 1 && weekDay == 1) {
                    sundayCount++;
                }
                day += 1;
                if (day > daysOfMonth) {
                    day = 1;
                    month += 1;
                    if (month > 12) {
                        month = 1;
                        year += 1;
                    }
                    daysOfMonth = GetDaysOfMonth(year, month);
                }
                weekDay = IncrementWeekDay(weekDay);
                
            } while (year < 2001);
            return sundayCount;
        }

        private int IncrementWeekDay(int weekDay) {
            weekDay += 1;
            if (weekDay > 7) {
                weekDay = 1;
            }
            return weekDay;
        }

        private int GetDaysOfMonth(int year, int month) {
            switch (month) {
                case 1:
                    return 31;
                case 2:
                    if (year % 4 != 0) {
                        return 28;
                    } else {
                        if (year % 100 == 0) {
                            if (year % 400 == 0) {
                                return 29;
                            } else {
                                return 28;
                            }
                        } else {
                            return 29;
                        }
                    }
                case 3:
                    return 31;
                case 4:
                    return 30;
                case 5:
                    return 31;
                case 6:
                    return 30;
                case 7:
                    return 31;
                case 8:
                    return 31;
                case 9:
                    return 30;
                case 10:
                    return 31;
                case 11:
                    return 30;
                case 12:
                    return 31;
                default:
                    throw new Exception("uh oh");
            }
        }
    }
}
