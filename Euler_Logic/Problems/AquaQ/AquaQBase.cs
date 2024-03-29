﻿using System.Collections.Generic;
using System.IO;

namespace Euler_Logic.Problems.AquaQ {
    public abstract class AquaQBase : ProblemBase {
        public List<string> Input() {
            return LoadFile(@"C:\Temp\AquaQ\input.txt");
        }

        private List<string> LoadFile(string file) {
            var input = new List<string>();
            using (var reader = new StreamReader(file)) {
                while (!reader.EndOfStream) {
                    input.Add(reader.ReadLine());
                }
                reader.Close();
            }
            return input;
        }
    }
}
