using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem144 : ProblemBase {
        public override string ProblemName {
            get { return "144: Investigating multiple reflections of a laser beam"; }
        }

        public override string GetAnswer() {
            Test();
            return "";
        }

        private void Test() {
            decimal startX = 0;
            decimal startY = (decimal)10.1;
            decimal endX = (decimal)1.4;
            decimal endY = (decimal)-9.6;

            decimal normalX = -4 * endX;
            decimal normalY = endY;

            decimal length = (decimal)Math.Sqrt((double)normalX * (double)normalX + (double)normalY * (double)normalY);

            decimal directionX = endX - startX;
            decimal directionY = endY - startY;

            decimal dot = directionX * normalX + directionY * normalY;
            decimal reflectX = directionX - 2 * dot * normalX;
            decimal reflectY = directionY - 2 * dot * normalY;
            decimal slope = reflectY / reflectX;
            bool stop = true;
        }
    }
}
