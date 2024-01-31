using System;
using System.Collections.Generic;

namespace Euler_Logic.Problems.AdventOfCode.Y2023;

public class Problem24 : AdventOfCodeBase{
    public override string ProblemName => "Advent of Code 2023: 24";

    public override string GetAnswer() {
        //return Answer1(Input_Test(1), new Point() { X = 7, Y = 7}, new Point() { X = 27, Y = 27}).ToString();
        return Answer1(Input(), new Point() { X = 200000000000000, Y = 200000000000000 }, new Point() { X = 400000000000000, Y = 400000000000000 }).ToString();
    }

    public override string GetAnswer2() {
        return Answer2(Input()).ToString();
    }

    private int Answer1(List<string> input, Point intersectStart, Point intersectEnd) {
        var stones = GetHailstones(input);
        return FindAllIntersections(stones, intersectStart, intersectEnd);
    }

    private string Answer2(List<string> input) {
        return "";
    }

    private int FindAllIntersections(List<Hail> stones, Point intersectStart, Point intersectEnd) {
        int count = 0;
        for (int index1 = 0; index1 < stones.Count; index1++) {
            var stone1 = stones[index1];
            for (int index2 = index1 + 1; index2 < stones.Count; index2++) {
                var stone2 = stones[index2];
                var intersection = CalcIntersection(stone1, stone2);
                if (IsIntersection(intersection, stone1, stone2, intersectStart, intersectEnd)
                    && IsIntersection(intersection, stone2, stone1, intersectStart, intersectEnd))
                    count++;
            }
        }
        return count;
    }

    private bool IsIntersection(Intersection intersection, Hail hail1, Hail hail2, Point intersectStart, Point intersectEnd) {
        if (intersection.IsParallel)
            return false;
        if (intersection.X >= intersectStart.X
            && intersection.X <= intersectEnd.X
            && intersection.Y >= intersectStart.Y
            && intersection.Y <= intersectEnd.Y) {
            bool lessThan0Intersect = intersection.X - hail1.Position.X < 0;
            bool lessThan0Velocity = hail1.Velocity.X < 0;
            return lessThan0Intersect == lessThan0Velocity;
        }
        return false;
    }

    private Intersection CalcIntersection(Hail hail1, Hail hail2) {
        var hail1Slope = CalcSlopeIntercept(hail1);
        var hail2Slope = CalcSlopeIntercept(hail2);
        var x = hail1Slope.Slope - hail2Slope.Slope;
        if (x == 0) {
            return new Intersection() { IsParallel = true };
        }
        var y = hail2Slope.Intercept - hail1Slope.Intercept;
        var finalX = y / x;
        var finalyY = hail1Slope.Slope * finalX + hail1Slope.Intercept;
        return new Intersection() {
            X = finalX,
            Y = finalyY
        };
    }

    private SlopeIntercept CalcSlopeIntercept(Hail hail) {
        var slope = hail.Velocity.Y / hail.Velocity.X;
        var intercept = hail.Position.Y - (hail.Position.X * slope);
        return new SlopeIntercept() {
            Intercept = intercept,
            Slope = slope
        };
    }

    private List<Hail> GetHailstones(List<string> input) {
        var stones = new List<Hail>();
        foreach (var line in input) {
            var split = line.Split("@");
            var pointSplit = split[0].Split(",");
            var velocitySplit = split[1].Split(",");
            var point = new Point() {
                X = Convert.ToDecimal(pointSplit[0].Trim()),
                Y = Convert.ToDecimal(pointSplit[1].Trim()),
                Z = Convert.ToDecimal(pointSplit[2].Trim())
            };
            var velocity = new Point() {
                X = Convert.ToDecimal(velocitySplit[0].Trim()),
                Y = Convert.ToDecimal(velocitySplit[1].Trim()),
                Z = Convert.ToDecimal(velocitySplit[2].Trim())
            };
            stones.Add(new Hail() {
                Position = point,
                Velocity = velocity
            });
        }
        return stones;
    }

    private class Point {
        public decimal X { get; set; }
        public decimal Y { get; set; }
        public decimal Z { get; set; }
    }

    private class Hail {
        public Point Position { get; set; }
        public Point Velocity { get; set; }
    }

    private class SlopeIntercept {
        public decimal Slope { get; set; }
        public decimal Intercept { get; set; }
    }

    private class Intersection {
        public decimal X { get; set; }
        public decimal Y { get; set; }
        public decimal Z { get; set; }
        public bool IsParallel { get; set; }
    }
}
