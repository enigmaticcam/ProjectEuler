using Euler_Logic.Helpers;

namespace Euler_Logic.Problems {
    public class Problem128 : ProblemBase {
        private PrimeSieveWithPrimeListULong _primes = new PrimeSieveWithPrimeListULong();

        /*
            If you imagine each added new layer forms the outline of a hexagon,
            then you can see there are six points (top, top left, bottom left,
            bottom bottom right, top right), and there are some amount of spots
            that form lines to connect the points. For example, in the diagram 
            given for the problem, the outermost layer's six points would be 20, 
            23, 26, 29, 32, and 35. The remaining spots in the outer layer form 
            the lines.

            If you examine just the spots on the edges, you can see that each one
            has two adjacent spots that have a difference of 1. Clearly those would
            be ignored since 1 is not a prime. Notice however that the remaining
            adjacent spots are two sets of sequential numbers. For example, spot
            9 is part of the edge in the second outermost layer that connects the
            points 10 and 8. 10 and 8 would be ignored. The remaining sets would be
            21 and 22, and 2 and 3. Given this, it can be seen that it's impossible
            to make 3 primes from these remaining 4 squares, because there will always
            be two odd and two even differences. Thus, we can conclude that we never
            need to check an edge - we only need to check the outermost points. The
            one exception is the edge immediately on the bottom right of the top
            point, e.g. 7, 19, 37.

            If we keep an increment (i) and increase it by one for each layer, then the
            numbers for the points can calculated likewise:

            t = 6i + (last t)
            tl = 6i + (last tl) + 1
            bl = 6i + (last bl) + 2
            b = 6i + (last b) + 3
            br = 6i + (last br) + 4
            tr = 6i + (last tr) + 5

            Once we calculate a new point on a new edge, we just calculate the numbers
            on the adjacent squares and check which differences yield a prime. Continue
            to do this until we found 2000 that have 3 primes.

         */

        public override string ProblemName {
            get { return "128: Hexagonal tile differences"; }
        }

        public override string GetAnswer() {
            _primes.SievePrimes(1000000);
            return Solve(2000).ToString();
        }

        private ulong Solve(int max) {
            int count = 2;
            ulong lastT = 2;
            ulong lastTL = 3;
            ulong lastBL = 4;
            ulong lastB = 5;
            ulong lastBR = 6;
            ulong lastTR = 7;
            ulong increment = 1;

            do {

                // Top
                ulong topMinus1BL = lastT;
                lastT += increment * 6;
                ulong tl = lastT + ((increment + 1) * 6) + 1;
                ulong tr = tl - 1 + ((increment + 2) * 6) - 1;
                ulong br = tl - 2;

                int prime = 0;
                prime += (_primes.IsPrime(tl - lastT) ? 1 : 0);
                prime += (_primes.IsPrime(tr - lastT) ? 1 : 0);
                prime += (_primes.IsPrime(br - lastT) ? 1 : 0);
                count += (prime == 3 ? 1 : 0);
                if (count == max) {
                    return lastT;
                }

                // Top - 1
                ulong tMinus1 = br;
                ulong t = tr;
                tr = t - 1;
                tl = lastT;
                ulong b = tl - 1;

                prime = 0;
                prime += (_primes.IsPrime(tr - tMinus1) ? 1 : 0);
                prime += (_primes.IsPrime(t - tMinus1) ? 1 : 0);
                prime += (_primes.IsPrime(tMinus1 - tl) ? 1 : 0);
                prime += (_primes.IsPrime(tMinus1 - topMinus1BL) ? 1 : 0);
                prime += (_primes.IsPrime(tMinus1 - b) ? 1 : 0);
                count += (prime == 3 ? 1 : 0);
                if (count == max) {
                    return tMinus1;
                }

                // Top left
                br = lastTL;
                lastTL += (increment * 6) + 1;
                t = lastTL + ((increment + 1) * 6);
                tl = t + 1;
                ulong bl = t + 2;

                prime = 0;
                prime += (_primes.IsPrime(t - lastTL) ? 1 : 0);
                prime += (_primes.IsPrime(tl - lastTL) ? 1 : 0);
                prime += (_primes.IsPrime(bl - lastTL) ? 1 : 0);
                prime += (_primes.IsPrime(lastTL - br) ? 1 : 0);
                count += (prime == 3 ? 1 : 0);
                if (count == max) {
                    return lastTL;
                }

                // Bottom left
                tr = lastBL;
                lastBL += (increment * 6) + 2;
                bl = ((increment + 1) * 6) + 2 + lastBL;
                tl = bl - 1;
                b = bl + 1;

                prime = 0;
                prime += (_primes.IsPrime(lastBL - tr) ? 1 : 0);
                prime += (_primes.IsPrime(tl - lastBL) ? 1 : 0);
                prime += (_primes.IsPrime(bl - lastBL) ? 1 : 0);
                prime += (_primes.IsPrime(b - lastBL) ? 1 : 0);
                count += (prime == 3 ? 1 : 0);
                if (count == max) {
                    return lastBL;
                }

                // Bottom
                t = lastB;
                lastB += (increment * 6) + 3;
                b = ((increment + 1) * 6) + 3 + lastB;
                bl = b - 1;
                br = b + 1;

                prime = 0;
                prime += (_primes.IsPrime(lastB - t) ? 1 : 0);
                prime += (_primes.IsPrime(bl - lastB) ? 1 : 0);
                prime += (_primes.IsPrime(b - lastB) ? 1 : 0);
                prime += (_primes.IsPrime(br - lastB) ? 1 : 0);
                count += (prime == 3 ? 1 : 0);
                if (count == max) {
                    return lastB;
                }

                // Bottom right
                tl = lastBR;
                lastBR += (increment * 6) + 4;
                br = ((increment + 1) * 6) + 4 + lastBR;
                b = br - 1;
                tr = br + 1;

                prime = 0;
                prime += (_primes.IsPrime(lastBR - tl) ? 1 : 0);
                prime += (_primes.IsPrime(br - lastBR) ? 1 : 0);
                prime += (_primes.IsPrime(b - lastBR) ? 1 : 0);
                prime += (_primes.IsPrime(tr - lastBR) ? 1 : 0);
                count += (prime == 3 ? 1 : 0);
                if (count == max) {
                    return lastBR;
                }

                // Top right
                bl = lastTR;
                lastTR += (increment * 6) + 5;
                tr = ((increment + 1) * 6) + 5 + lastTR;
                t = tr + 1;
                br = tr - 1;

                prime = 0;
                prime += (_primes.IsPrime(lastTR - bl) ? 1 : 0);
                prime += (_primes.IsPrime(tr - lastTR) ? 1 : 0);
                prime += (_primes.IsPrime(t - lastTR) ? 1 : 0);
                prime += (_primes.IsPrime(br - lastTR) ? 1 : 0);
                count += (prime == 3 ? 1 : 0);
                if (count == max) {
                    return lastTR;
                }

                increment++;
            } while (true);
        }
    }
}
