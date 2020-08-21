using BlackjackGraphs.Models.GraphModels;
using BlackjackGraphs.Models.GraphModels.Finite;
using BlackjackGraphs.Models.GraphModels.Infinite;
using BlackjackGraphs.Models.StochasticModels;
using BlackjackGraphs.Models.StochasticModels.Finite;
using BlackjackGraphs.Models.StochasticModels.Infinite;
using System.Diagnostics;

namespace BlackjackGraphs
{
    class Program
    {
        static void Main(string[] args)
        {
            #region Graph theoretic (infinitely many decks)
            var gInf = new DealerHandsGraphInfinite();
            gInf.PrintAllUpcardProbabilities();
            #endregion
            #region Stochastic (infinitely many decks)
            var dealerInf = new DealerInfinite();
            var simInf = new DealerSimulator(dealerInf);
            simInf.Simulate(10000000);
            #endregion
            #region Graph theoretic (finitely many decks)
            // var gFin = new DealerHandsGraphFinite();
            // gFin.PrintAllUpcardProbabilities();
            #endregion
            #region Stochastic (finitely many decks)
            // var dealerFin = new DealerFinite();
            // var simFin = new DealerSimulator(dealerFin);
            // simFin.Simulate(1000); // Can take a long time
            #endregion
        }
    }
}
