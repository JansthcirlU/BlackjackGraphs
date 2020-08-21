using System;
using System.Collections.Generic;
using System.Linq;

namespace BlackjackGraphs.Models.StochasticModels.Infinite
{
    public class DealerInfinite : Dealer
    {
        public override void InitializeDeck()
        {
            Deck.Clear();
            for (int card = 1; card < 10; card++)
            {
                Deck[card] = 1;
            }
            Deck[10] = 4;
            UpdateDeckList();
        }
        public override void Draw()
        {
            Hand.Add(DeckList[Rng.Next(13)]);
        }
    }
}