using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem112 : ProblemBase {
        private enum enumBouncyStatus {
            Undefined,
            Ascending,
            Descending,
            Both
        }

        private Dictionary<string, enumBouncyStatus> _bouncyReference = new Dictionary<string, enumBouncyStatus>();

        public override string ProblemName {
            get { return "112: Bouncy numbers"; }
        }

        public override string GetAnswer() {
            BuildBouncyReference();
            return FindNumWherePercentageIs(99).ToString();
        }

        private ulong FindNumWherePercentageIs(ulong percentage) {
            ulong number = 1;
            ulong count = 0;
            do {
                if (IsBouncy(number)) {
                    count++;
                }
                if ((count * 100) >= (number * percentage)) {
                    return number;
                }
                number++;
            } while (true);
        }

        private bool IsBouncy(decimal number) {
            if (number < 100) {
                return false;
            } else {
                string text = number.ToString();
                enumBouncyStatus status = enumBouncyStatus.Undefined;
                for (int index = 0; index < text.Length - 1; index++) {
                    enumBouncyStatus subStatus = _bouncyReference[text.Substring(index, 2)];
                    if (subStatus != enumBouncyStatus.Both) {
                        if (status == enumBouncyStatus.Undefined) {
                            status = subStatus;
                        } else if (status != subStatus && subStatus != enumBouncyStatus.Both && status != enumBouncyStatus.Both) {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private void BuildBouncyReference() {
            _bouncyReference.Add("00", enumBouncyStatus.Both);
            for (int number = 1; number <= 99; number++) {
                string text = number.ToString();
                if (text.Length == 1) {
                    text = "0" + text;
                }
                int compare = string.Compare(text.Substring(0, 1), text.Substring(1, 1));
                if (compare > 0) {
                    _bouncyReference.Add(text, enumBouncyStatus.Descending);
                } else if (compare < 0) {
                    _bouncyReference.Add(text, enumBouncyStatus.Ascending);
                } else {
                    _bouncyReference.Add(text, enumBouncyStatus.Both);
                }
            }
        }
    }
}
