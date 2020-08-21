using System;
using System.Collections.Generic;
using System.Linq;

namespace BlackjackGraphs.Models.GraphModels
{
    public class FiniteDealerHandNode : DealerHandNode
    {
        // Constructors
        public FiniteDealerHandNode()
        {
            
        }
        public FiniteDealerHandNode(int card1, int card2, Dictionary<int, int> deck) : base(card1, card2)
        {
            Deck = deck;
        }

        // Properties
        public Dictionary<int, int> Deck { get; set; }
    }
}