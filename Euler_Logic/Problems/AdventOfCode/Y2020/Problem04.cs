using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems.AdventOfCode.Y2020 {
    public class Problem04 : AdventOfCodeBase {
        private List<string> _requiredKeys;

        public override string ProblemName => "Advent of Code 2020: 4";

        public override string GetAnswer() {
            return Answer2(Input()).ToString();
        }

        private int Answer1(List<string> input) {
            SetRequiredKeys();
            var passports = GetPassports(input);
            int count = 0;
            foreach (var passport in passports) {
                if (HasRequiredKeys(passport)) {
                    count++;
                }
            }
            return count;
        }

        private int Answer2(List<string> input) {
            SetRequiredKeys();
            var passports = GetPassports(input);
            int count = 0;
            foreach (var passport in passports) {
                if (HasRequiredKeys(passport) && IsValid(passport)) {
                    count++;
                }
            }
            return count;
        }

        private bool IsValid(Dictionary<string, string> passport) {
            if (!IsValidBirthYear(passport["byr"])) {
                return false;
            }
            if (!IsValidIssueYear(passport["iyr"])) {
                return false;
            }
            if (!IsValidExpirationYear(passport["eyr"])) {
                return false;
            }
            if (!IsValidHeight(passport["hgt"])) {
                return false;
            }
            if (!IsValidHairColor(passport["hcl"])) {
                return false;
            }
            if (!IsValidEyeColor(passport["ecl"])) {
                return false;
            }
            if (!IsValidPassportId(passport["pid"])) {
                return false;
            }
            return true;
        }

        private bool IsValidBirthYear(string value) {
            if (value.Length != 4) {
                return false;
            }
            var valueNum = Convert.ToInt32(value);
            if (valueNum < 1920 || valueNum > 2002) {
                return false;
            }
            return true;
        }

        private bool IsValidIssueYear(string value) {
            if (value.Length != 4) {
                return false;
            }
            var valueNum = Convert.ToInt32(value);
            if (valueNum < 2010 || valueNum > 2020) {
                return false;
            }
            return true;
        }

        private bool IsValidExpirationYear(string value) {
            if (value.Length != 4) {
                return false;
            }
            var valueNum = Convert.ToInt32(value);
            if (valueNum < 2020 || valueNum > 2030) {
                return false;
            }
            return true;
        }

        private bool IsValidHeight(string value) {
            var height = Convert.ToInt32(value.Substring(0, value.Length - 2));
            var scale = value.Substring(value.Length - 2, 2);
            if (scale == "cm") {
                return height >= 150 && height <= 193;
            } else if (scale == "in") {
                return height >= 59 && height <= 76;
            }
            return false;
        }

        private bool IsValidHairColor(string value) {
            int index = 0;
            foreach (char digit in value) {
                if (index == 0) {
                    if (digit != '#') {
                        return false;
                    }
                } else {
                    switch (digit) {
                        case '0':
                        case '1':
                        case '2':
                        case '3':
                        case '4':
                        case '5':
                        case '6':
                        case '7':
                        case '8':
                        case '9':
                        case 'a':
                        case 'b':
                        case 'c':
                        case 'd':
                        case 'e':
                        case 'f':
                            break;
                        default:
                            return false;
                    }
                }
                index++;
            }
            return true;
        }

        private bool IsValidEyeColor(string value) {
            switch (value) {
                case "amb":
                case "blu":
                case "brn":
                case "gry":
                case "grn":
                case "hzl":
                case "oth":
                    break;
                default:
                    return false;
            }
            return true;
        }

        private bool IsValidPassportId(string value) {
            if (value.Length != 9) {
                return false;
            }
            foreach (char digit in value) {
                switch (digit) {
                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                        break;
                    default:
                        return false;
                }
            }
            return true;
        }

        private bool HasRequiredKeys(Dictionary<string, string> passport) {
            foreach (var key in _requiredKeys) {
                if (!passport.ContainsKey(key)) {
                    return false;
                }
            }
            return true;
        }

        private List<Dictionary<string, string>> GetPassports(List<string> input) {
            var passports = new List<Dictionary<string, string>>();
            var current = new Dictionary<string, string>();
            passports.Add(current);
            foreach (var line in input) {
                if (line == "") {
                    current = new Dictionary<string, string>();
                    passports.Add(current);
                } else {
                    var split = line.Split(' ');
                    foreach (var set in split) {
                        var nextSplit = set.Split(':');
                        current.Add(nextSplit[0], nextSplit[1]);
                    }
                }
            }
            return passports;
        }

        private void SetRequiredKeys() {
            _requiredKeys = new List<string>() {
                "byr",
                "iyr",
                "eyr",
                "hgt",
                "hcl",
                "ecl",
                "pid"
            };
        }

        private List<string> Test1Input() {
            return new List<string>() {
                "ecl:gry pid:860033327 eyr:2020 hcl:#fffffd",
                "byr:1937 iyr:2017 cid:147 hgt:183cm",
                "",
                "iyr:2013 ecl:amb cid:350 eyr:2023 pid:028048884",
                "hcl:#cfa07d byr:1929",
                "",
                "hcl:#ae17e1 iyr:2013",
                "eyr:2024",
                "ecl:brn pid:760753108 byr:1931",
                "hgt:179cm",
                "",
                "hcl:#cfa07d eyr:2025 pid:166559648",
                "iyr:2011 ecl:brn hgt:59in"
            };
        }

        private List<string> Test2Input() {
            return new List<string>() {
                "eyr:1972 cid:100",
                "hcl:#18171d ecl:amb hgt:170 pid:186cm iyr:2018 byr:1926",
                "",
                "iyr:2019",
                "hcl:#602927 eyr:1967 hgt:170cm",
                "ecl:grn pid:012533040 byr:1946",
                "",
                "hcl:dab227 iyr:2012",
                "ecl:brn hgt:182cm pid:021572410 eyr:2020 byr:1992 cid:277",
                "",
                "hgt:59cm ecl:zzz",
                "eyr:2038 hcl:74454a iyr:2023",
                "pid:3556412378 byr:2007"
            };
        }

        private List<string> Test3Input() {
            return new List<string>() {
                "pid:087499704 hgt:74in ecl:grn iyr:2012 eyr:2030 byr:1980",
                "hcl:#623a2f",
                "",
                "eyr:2029 ecl:blu cid:129 byr:1989",
                "iyr:2014 pid:896056539 hcl:#a97842 hgt:165cm",
                "",
                "hcl:#888785",
                "hgt:164cm byr:2001 iyr:2015 cid:88",
                "pid:545766238 ecl:hzl",
                "eyr:2022",
                "",
                "iyr:2010 hgt:158cm hcl:#b6652a ecl:blu byr:1944 eyr:2021 pid:093154719"
            };
        }
    }
}
