using BlackjackGraphs.Models.StochasticModels.Finite;
using BlackjackGraphs.Models.StochasticModels.Infinite;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace BlackjackGraphs.Models.StochasticModels
{
    public class DealerSimulator
    {
        public Dealer Dealer { get; set; } = new DealerInfinite();
        public Dictionary<int, int> TimesPlayed { get; set; } = new Dictionary<int, int>();
        public Dictionary<int, decimal> GamesThat17 { get; set; } = new Dictionary<int, decimal>();
        public Dictionary<int, decimal> GamesThat18 { get; set; } = new Dictionary<int, decimal>();
        public Dictionary<int, decimal> GamesThat19 { get; set; } = new Dictionary<int, decimal>();
        public Dictionary<int, decimal> GamesThat20 { get; set; } = new Dictionary<int, decimal>();
        public Dictionary<int, decimal> GamesThat21 { get; set; } = new Dictionary<int, decimal>();
        public Dictionary<int, decimal> GamesThatBust { get; set; } = new Dictionary<int, decimal>();
        public DealerSimulator()
        {
            InitializeSimulation();
        }
        public DealerSimulator(Dealer dealer)
        {
            Dealer = dealer;
            InitializeSimulation();
        }
        public void InitializeSimulation()
        {
            TimesPlayed.Clear();
            GamesThat17.Clear();
            GamesThat18.Clear();
            GamesThat19.Clear();
            GamesThat20.Clear();
            GamesThat21.Clear();
            GamesThatBust.Clear();
            for (int card = 1; card < 11; card++)
            {
                TimesPlayed[card] = 0;
                GamesThat17[card] = 0m;
                GamesThat18[card] = 0m;
                GamesThat19[card] = 0m;
                GamesThat20[card] = 0m;
                GamesThat21[card] = 0m;
                GamesThatBust[card] = 0m;
            }
            Dealer.InitializeDealer();
        }

        public void Simulate(int repetitions, bool printResults = true)
        {
            for (int i = 0; i < repetitions; i++)
            {
                Dealer.Play();
                var upcard = Dealer.Hand[0];
                var total = Dealer.GetTotal();
                TimesPlayed[upcard] += 1;
                GamesThat17[upcard] += total == 17 ? 1m : 0m;
                GamesThat18[upcard] += total == 18 ? 1m : 0m;
                GamesThat19[upcard] += total == 19 ? 1m : 0m;
                GamesThat20[upcard] += total == 20 ? 1m : 0m;
                GamesThat21[upcard] += total == 21 ? 1m : 0m;
                GamesThatBust[upcard] += total > 21 ? 1m : 0m;
            }
            for (int card = 1; card < 11; card++)
            {
                GamesThat17[card] /= TimesPlayed[card] > 0 ? TimesPlayed[card] : 1;
                GamesThat18[card] /= TimesPlayed[card] > 0 ? TimesPlayed[card] : 1;
                GamesThat19[card] /= TimesPlayed[card] > 0 ? TimesPlayed[card] : 1;
                GamesThat20[card] /= TimesPlayed[card] > 0 ? TimesPlayed[card] : 1;
                GamesThat21[card] /= TimesPlayed[card] > 0 ? TimesPlayed[card] : 1;
                GamesThatBust[card] /= TimesPlayed[card] > 0 ? TimesPlayed[card] : 1;
            }
            if (printResults)
            {
                PrintResults(repetitions);
            }
        }

        public void PrintResults(int repetitions)
        {
            string decks = "infinitely many";
            if (Dealer is DealerFinite dfin)
            {
                decks = dfin.Decks.ToString();
            }
            System.Console.WriteLine($"\nStochastic probabilities with {decks} deck(s) ({repetitions} repetitions):");
            var headers = " Hand      | P(17)   | P(18)   | P(19)   | P(20)   | P(21)   | P(bust) | Total";
            System.Console.WriteLine(headers);
            System.Console.WriteLine(new String('=', headers.Length));
            for (int i = 1; i < 11; i++)
            {
                var omega = GamesThat17[i] + GamesThat18[i] + GamesThat19[i] + GamesThat20[i] + GamesThat21[i] + GamesThatBust[i];
                System.Console.WriteLine($"{{ {i,2},  ? }} | {Math.Round((decimal)GamesThat17[i],5),7} | {Math.Round((decimal)GamesThat18[i],5),7} | {Math.Round((decimal)GamesThat19[i],5),7} | {Math.Round((decimal)GamesThat20[i],5),7} | {Math.Round((decimal)GamesThat21[i],5),7} | {Math.Round((decimal)GamesThatBust[i],5),7} | {omega}");
            }
        }
    }
}