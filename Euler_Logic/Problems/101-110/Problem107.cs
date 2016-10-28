using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem107 : IProblem {
        public string ProblemName {
            get { return "107: Minimal network"; }
        }

        public string GetAnswer() {
            return "done";
        }

        public bool IsConnected(Graph graph) {
            HashSet<string> nodes = new HashSet<string>();

        }

        private Graph GetTestGraph() {
            Graph graph = new Graph();

            graph.Connections["A"].Add("B", 16);
            graph.Connections["A"].Add("C", 12);
            graph.Connections["A"].Add("D", 21);
            graph.Connections["B"].Add("A", 16);
            graph.Connections["B"].Add("D", 17);
            graph.Connections["B"].Add("E", 20);
            graph.Connections["C"].Add("A", 12);
            graph.Connections["C"].Add("D", 28);
            graph.Connections["C"].Add("F", 31);
            graph.Connections["D"].Add("A", 21);
            graph.Connections["D"].Add("B", 17);
            graph.Connections["D"].Add("C", 28);
            graph.Connections["D"].Add("E", 18);
            graph.Connections["D"].Add("F", 19);
            graph.Connections["D"].Add("G", 23);
            graph.Connections["E"].Add("B", 20);
            graph.Connections["E"].Add("D", 18);
            graph.Connections["E"].Add("E", 11);
            graph.Connections["F"].Add("C", 31);
            graph.Connections["F"].Add("D", 19);
            graph.Connections["F"].Add("G", 27);
            graph.Connections["G"].Add("D", 23);
            graph.Connections["G"].Add("E", 11);
            graph.Connections["G"].Add("F", 27);
            return graph;
        }

        private class Node {
            public string Name { get; set; }
        }

        private class Graph {
            public Dictionary<string, Node> Nodes = new Dictionary<string, Node>();
            public Dictionary<string, Dictionary<string, int>> Connections { get; set; }

            public Graph() {
                this.Connections.Add("A", new Dictionary<string, int>());
                this.Connections.Add("B", new Dictionary<string, int>());
                this.Connections.Add("C", new Dictionary<string, int>());
                this.Connections.Add("D", new Dictionary<string, int>());
                this.Connections.Add("E", new Dictionary<string, int>());
                this.Connections.Add("F", new Dictionary<string, int>());
                this.Connections.Add("G", new Dictionary<string, int>());

                Node A = new Node();
                this.Nodes.Add("A", A);
                A.Name = "A";

                Node B = new Node();
                this.Nodes.Add("B", B));
                B.Name = "B";

                Node C = new Node();
                this.Nodes.Add("C", C));
                C.Name = "C";

                Node D = new Node();
                this.Nodes.Add("D", D));
                D.Name = "D";

                Node E = new Node();
                this.Nodes.Add("E", E));
                E.Name = "E";

                Node F = new Node();
                this.Nodes.Add("F", F));
                F.Name = "F";

                Node G = new Node();
                this.Nodes.Add("G", G));
                G.Name = "G";
            }
        }
    }
}
