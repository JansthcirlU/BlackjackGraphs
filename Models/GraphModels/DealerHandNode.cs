using System;
using System.Collections.Generic;
using System.Linq;

namespace BlackjackGraphs.Models.GraphModels
{
    public class DealerHandNode : IEquatable<DealerHandNode>
    {
        // Constructors
        public DealerHandNode()
        {
            
        }
        public DealerHandNode(int card1, int card2)
        {
            Card1 = card1;
            Card2 = card2;
            Total = GetTotal();
        }

        // Properties
        public int Card1 { get; set; }
        public int Card2 { get; set; }
        public int Total { get; set; }
        public List<Edge> EdgesTo { get; set; }
        public List<Edge> EdgesFrom { get; set; }
        public bool Hit { get; set; } // If the dealer has this hand, true means hit, false means stand
        public Dictionary<int,decimal?> Chances { get; set; } = new Dictionary<int, decimal?>()
        {
            { 17, 0m },
            { 18, 0m },
            { 19, 0m },
            { 20, 0m },
            { 21, 0m },
        };
        public Dictionary<int, bool> ChancesChecked { get; set; } = new Dictionary<int, bool>()
        {
            { 17, false },
            { 18, false },
            { 19, false },
            { 20, false },
            { 21, false },
        };
        public decimal? Bust { get; set; } = null; // Should receive a positive value when evaluating the graph
        public bool Soft { get; set; }
        public bool Checked { get; set; } = false;
        
        // Methods
        public void AddCard(int card)
        {
            if (card > 1)
            {
                if (Soft && Card2 + card < 11)
                {
                    Card2 += card;
                }
                else
                {
                    Card1 += card;
                }
            }
            else
            {
                if (Card1 + Card2 < 11)
                {
                    Card2 += Card1;
                    Card1 = 1;
                }
                else
                {
                    Card1++; // Same as Card1 += card if card == 1
                }
            }
            Total = GetTotal();
        }

        public int GetTotal()
        {
            var sum = Card1 + Card2;
            if ((Card1 == 1 || Card2 == 1) && sum + 10 < 22)
            {
                Soft = true;
                sum += 10;
            }
            else
            {
                Soft = false;
            }
            // Set 'Hit'
            if (sum < 17)
            {
                Hit = true;
            }
            else if (sum == 17)
            {
                Hit = Soft;
            }
            else
            {
                Hit = false;
            }
            return sum;
        }

        public bool Equals(DealerHandNode other)
        {
            if (Total != 21)
            {
                return Total == other.Total && Soft == other.Soft;
            }
            else
            {
                return Total == other.Total;
            }
        }

        public override string ToString()
        {
            return $"{{ {Card1,2}, {Card2,2} }}";
        }
    }
}