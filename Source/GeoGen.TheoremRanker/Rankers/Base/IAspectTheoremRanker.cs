﻿using GeoGen.Core;
using GeoGen.TheoremProver;

namespace GeoGen.TheoremRanker
{
    /// <summary>
    /// Represents a ranker of particular <see cref="RankedAspect"/>.
    /// </summary>
    public interface IAspectTheoremRanker
    {
        /// <summary>
        /// The aspect of theorems that this ranker ranks.
        /// </summary>
        RankedAspect RankedAspect { get; }

        /// <summary>
        /// Ranks a given theorem, potentially using all given provided context.
        /// </summary>
        /// <param name="theorem">The theorem to be ranked.</param>
        /// <param name="configuration">The configuration where the theorem holds.</param>
        /// <param name="allTheorems">The map of all theorems of the configuration.</param>
        /// <param name="proverOutput">The output from the theorem prover for all the theorems of the configuration.</param>
        /// <returns>A number representing the ranking of the theorem. The range of its values depends on the implementation.</returns>
        double Rank(Theorem theorem, Configuration configuration, TheoremMap allTheorems, TheoremProverOutput proverOutput);
    }
}
