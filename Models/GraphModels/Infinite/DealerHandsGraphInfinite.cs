using System;
using System.Collections.Generic;
using System.Linq;

namespace BlackjackGraphs.Models.GraphModels.Infinite
{
    public class DealerHandsGraphInfinite
    {
        public List<DealerHandNode> Nodes { get; set; } = new List<DealerHandNode>();
        public List<Edge> Edges { get; set; } = new List<Edge>();
        public int[,] AdjacencyMatrix { get; set; }
        public DealerHandsGraphInfinite()
        {
            PopulateGraph();
            CalculateChancesInfinite();
        }

        public void PopulateGraph()
        {
            for (int i = 1; i < 11; i++)
            {
                for (int j = 1; j < 11; j++)
                {
                    var newHand = new DealerHandNode(i,j);
                    if (!Nodes.Contains(newHand))
                    {
                        Nodes.Add(newHand);
                    }
                }
            }
            PopulateAdjacencyMatrix();
        }

        public void PopulateAdjacencyMatrix()
        {
            ConnectNodes();
            var n = Nodes.Count;
            AdjacencyMatrix = new int[n,n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    var edge = GetEdgeBetween(Nodes[i], Nodes[j]);
                    AdjacencyMatrix[i,j] = edge != null ? edge.Weight : 0;
                }
            }
        }

        public void ConnectNodes()
        {
            foreach (var node in Nodes)
            {
                for (int i = 1; i < 11; i++)
                {
                    var newNode = new DealerHandNode(node.Card1, node.Card2);
                    newNode.AddCard(i);
                    var weight = i < 10 ? 1 : 4;
                    if (Nodes.Contains(newNode) && node.Hit)
                    {
                        AddEdge(node, Nodes.Find(x => x.Equals(newNode)), weight);
                    }
                }
            }
            foreach (var node in Nodes)
            {
                node.EdgesFrom = GetEdgesFrom(node);
                node.EdgesTo = GetEdgesTo(node);
            }
        }

        public void CalculateChancesInfinite()
        {
            foreach (var node in Nodes)
            {
                CalculateBustChanceInfinite(node);
                for (int i = 17; i < 22; i++)
                {
                    CalculateTotalChanceInfinite(node, i);
                }
            }
        }

        public void CalculateInitialBustChanceInfinite(DealerHandNode node)
        {
            node.Checked = false;
            var sum = node.Total;
            if (11 < sum && sum < 17 && !node.Soft)
            {
                node.Bust = (sum - 8) / 13m;
            }
            else
            {
                node.Bust = 0;
            }
        }

        public void CalculateBustChanceInfinite(DealerHandNode node)
        {
            CalculateInitialBustChanceInfinite(node);
            if (!node.Checked)
            {
                foreach (var edge in node.EdgesFrom)
                {
                    if (!edge.End.Checked)
                    {
                        CalculateBustChanceInfinite(edge.End);
                    }
                    node.Bust += edge.End.Bust * edge.Weight / 13m;
                }
            }
            node.Checked = true;
        }

        public void CalculateInitialTotalChanceInfinite(DealerHandNode node, int total)
        {
            node.ChancesChecked[total] = false;
            // Check if node has not already reached a total
            if (total == 17)
            {
                node.Chances[total] = node.Total == total && !node.Soft ? 1 : 0;
            }
            else
            {
                node.Chances[total] = node.Total == total ? 1 : 0;
            }
        }

        public void CalculateTotalChanceInfinite(DealerHandNode node, int total)
        {
            CalculateInitialTotalChanceInfinite(node, total);
            foreach (var edge in node.EdgesFrom)
            {
                if (!edge.End.ChancesChecked[total])
                {
                    CalculateTotalChanceInfinite(edge.End, total);
                }
                node.Chances[total] += edge.Weight * edge.End.Chances[total] / 13m;
            }
            node.ChancesChecked[total] = true;
        }

        public List<Edge> GetEdgesFrom(DealerHandNode node)
        {
            return Edges.Where(x => x.Start.Equals(node)).ToList();
        }

        public List<Edge> GetEdgesTo(DealerHandNode node)
        {
            return Edges.Where(x => x.End.Equals(node)).ToList();
        }

        public Edge GetEdgeBetween(DealerHandNode start, DealerHandNode end)
        {
            return Edges.Where(x => x.Start.Equals(start) && x.End.Equals(end)).FirstOrDefault();
        }

        public DealerHandNode FindLastNode()
        {
            foreach (var node in Nodes)
            {
                if (GetEdgesFrom(node).Count == 0)
                {
                    return node;
                }
            }
            return null; // Shouldn't happen but who knows
        }

        public void AddEdge(DealerHandNode start, DealerHandNode end, int weight)
        {
            if (!Nodes.Contains(start))
            {
                Nodes.Add(start);
            }
            if (!Nodes.Contains(end))
            {
                Nodes.Add(end);
            }
            var edge = new Edge(start, end) { Weight = weight };
            if (!Edges.Contains(edge))
            {
                Edges.Add(edge);
            }
        }

        public void PrintAdjacencyMatrix()
        {
            var n = Nodes.Count;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    System.Console.Write(AdjacencyMatrix[i,j]);
                    System.Console.Write(", ");
                }
                System.Console.WriteLine();
            }
        }

        public void PrintProbabilityTable()
        {
            var headers = " Hand      | P(17)   | P(18)   | P(19)   | P(20)   | P(21)   | P(bust) | Total";
            System.Console.WriteLine(headers);
            System.Console.WriteLine(new String('=', headers.Length));
            foreach (var node in Nodes)
            {
                decimal? total = 0;
                System.Console.Write(node);
                System.Console.Write(" | ");
                foreach (var key in node.Chances.Keys)
                {
                    total += node.Chances[key];
                    System.Console.Write($"{Math.Round((decimal)node.Chances[key],5),7}");
                    System.Console.Write(" | ");
                }
                total += node.Bust;
                System.Console.Write($"{Math.Round((decimal)node.Bust, 5),7}");
                System.Console.Write(" | ");
                System.Console.WriteLine(total);
            }
        }

        public decimal Draw(int card)
        {
            return card == 10 ? 4 / 13m : 1 / 13m;
        }

        public void PrintUpcardProbabilities(int upcard)
        {
            // Prints probabilities based on the dealer's upcard
            var weighted17 = 0m;
            var weighted18 = 0m;
            var weighted19 = 0m;
            var weighted20 = 0m;
            var weighted21 = 0m;
            var weightedBust = 0m;
            for (int i = 1; i < 11; i++)
            {
                var tempHand = Nodes.Find(x => x.Equals(new DealerHandNode(i, upcard)));
                weighted17 += (decimal)tempHand.Chances[17] * Draw(i);
                weighted18 += (decimal)tempHand.Chances[18] * Draw(i);
                weighted19 += (decimal)tempHand.Chances[19] * Draw(i);
                weighted20 += (decimal)tempHand.Chances[20] * Draw(i);
                weighted21 += (decimal)tempHand.Chances[21] * Draw(i);
                weightedBust += (decimal)tempHand.Bust * Draw(i);
            }
            var total = weighted17 + weighted18 + weighted19 + weighted20 + weighted21 + weightedBust;
            System.Console.WriteLine($"{{ {upcard,2},  ? }} | {Math.Round((decimal)weighted17,5)} | {Math.Round((decimal)weighted18,5)} | {Math.Round((decimal)weighted19,5)} | {Math.Round((decimal)weighted20,5)} | {Math.Round((decimal)weighted21,5)} | {Math.Round((decimal)weightedBust,5)} | {total}");
        }

        public void PrintAllUpcardProbabilities()
        {
            System.Console.WriteLine("Graph theoretic probabilities with infinitely many decks:");
            var headers = " Hand      | P(17)   | P(18)   | P(19)   | P(20)   | P(21)   | P(bust) | Total";
            System.Console.WriteLine(headers);
            System.Console.WriteLine(new String('=', headers.Length));
            for (int i = 1; i < 11; i++)
            {
                PrintUpcardProbabilities(i);
            }
        }
    }
}