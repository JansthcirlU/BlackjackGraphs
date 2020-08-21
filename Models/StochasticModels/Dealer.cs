using System;
using System.Collections.Generic;
using System.Linq;

namespace BlackjackGraphs.Models.StochasticModels
{
    public abstract class Dealer
    {
        public Dictionary<int,int> Deck { get; set; } = new Dictionary<int, int>();
        public List<int> DeckList { get; set; } = new List<int>();
        public List<int> Hand { get; set; } = new List<int>();
        public bool Hit { get; set; } = true;
        public bool Soft { get; set; }
        public bool HitsSoft17 { get; set; } = true;
        public Random Rng { get; set; } = new Random();

        public void InitializeDealer()
        {
            Hand.Clear();
            InitializeDeck();
            UpdateDeckList();
            Draw();
            Draw();
            GetTotal();
        }
        public abstract void InitializeDeck();
        public void UpdateDeckList()
        {
            DeckList.Clear();
            foreach (var key in Deck.Keys)
            {
                for (int i = 0; i < Deck[key]; i++)
                {
                    DeckList.Add(key);
                }
            }
        }
        public abstract void Draw();
        public void Play()
        {
            InitializeDealer();
            while (Hit)
            {
                Draw();
                GetTotal();
            }
        }
        public int GetTotal()
        {
            var sum = Hand.Sum();
            // Determine if hard or soft total
            if (sum + 10 < 22 && Hand.Contains(1))
            {
                sum += 10;
                Soft = true;
            }
            else
            {
                Soft = false;
            }

            // Dealer hits soft 17
            if (sum > 17)
            {
                Hit = false;
            }
            else if (sum == 17)
            {
                Hit = Soft;
            }
            else
            {
                Hit = true;
            }
            return sum;
        }
    }
}