using System;
using System.Collections.Generic;

namespace BlackjackGraphs.Models.GraphModels
{
    public class Edge : IEquatable<Edge>
    {
        public DealerHandNode Start { get; set; }
        public DealerHandNode End { get; set; }
        public int Weight { get; set; } = 1;
        public Edge(DealerHandNode start, DealerHandNode end)
        {
            Start = start;
            End = end;
        }

        public bool Equals(Edge other)
        {
            return Start == other.Start && End == other.End; // Same total means equivalent hand
        }

        public override string ToString()
        {
            return $"{Start} -> {End} ({Weight})";
        }
    }
}