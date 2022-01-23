namespace Euler_Logic.Problems.AdventOfCode.Y2016 {
    public class Problem19 : AdventOfCodeBase {
        private Person _persons;

        public override string ProblemName => "Advent of Code 2016: 19";

        public override string GetAnswer() {
            return Answer1(3018458).ToString();
        }

        public override string GetAnswer2() {
            return Answer2(3018458).ToString();
        }

        private int Answer1(int input) {
            GetPersons(input);
            return GetLast();
        }

        private int Answer2(int input) {
            GetPersons(input);
            return GetLast2(input);
        }

        private int GetLast2(int maxCount) {
            var person = _persons;
            var toRemove = maxCount / 2;
            var currentToRemove = toRemove;
            int index = 0;
            for (int count = 1; count < toRemove; count++) {
                person = person.Next;
            }
            do {
                person.Next = person.Next.Next;
                maxCount--;
                if (index >= toRemove) {
                    index--;
                }
                index = (index + 1) % maxCount;
                toRemove = (maxCount / 2 + index) % maxCount;
                while (currentToRemove != toRemove) {
                    currentToRemove = (currentToRemove + 1) % maxCount;
                    person = person.Next;
                }
            } while (maxCount > 1);
            return person.Id;
        }

        private int GetLast() {
            var person = _persons;
            do {
                person.Presents += person.Next.Presents;
                person.Next.Presents = 0;
                person.Next = person.Next.Next;
                person = person.Next;
                while (person.Presents == 0) {
                    person = person.Next;
                }
            } while (person.Next != person);
            return person.Id;
        }

        private void GetPersons(int maxCount) {
            var lastPerson = new Person() {
                Id = 1,
                Presents = 1
            };
            _persons = lastPerson;
            for (int count = 2; count <= maxCount; count++) {
                var next = new Person() {
                    Id = count,
                    Next = null,
                    Presents = 1
                };
                lastPerson.Next = next;
                lastPerson = next;
            }
            lastPerson.Next = _persons;
        }

        private class Person {
            public int Id { get; set; }
            public Person Next { get; set; }
            public int Presents { get; set; }
        }
    }
}
