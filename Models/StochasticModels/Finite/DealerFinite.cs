using System;
using System.Collections.Generic;
using System.Linq;

namespace BlackjackGraphs.Models.StochasticModels.Finite
{
    public class DealerFinite : Dealer
    {
        public int Decks { get; set; } = 8;
        public DealerFinite(int decks = 8, bool hitsSoft17 = true)
        {
            Decks = decks;
            HitsSoft17 = hitsSoft17;
            InitializeDeck();
        }
        public override void InitializeDeck()
        {
            Deck.Clear();
            for (int card = 1; card < 10; card++)
            {
                Deck[card] = 4 * Decks;
            }
            Deck[10] = 16 * Decks;
        }
        public override void Draw()
        {
            var card = DeckList[Rng.Next(Deck.Values.Sum())];
            Deck[card] -= 1;
            UpdateDeckList();
            Hand.Add(card);
        }
    }
}