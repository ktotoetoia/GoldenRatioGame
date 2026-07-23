using System.Collections.Generic;
using UnityEngine;

namespace IM.Map
{
    /// <summary>
    /// Base implementation of <see cref="IPropPlacementStrategy"/> that handles candidate collection,
    /// dedup, and lazy yielding. Concrete strategies (edge bias, clustering, top-row-only, ...) only
    /// need to override <see cref="IsCandidateValid"/> to filter and/or <see cref="SelectIndex"/> to
    /// weight — they never touch the iteration mechanics.
    /// </summary>
    public abstract class PropPlacementStrategyBase : IPropPlacementStrategy
    {
        protected readonly struct Candidate
        {
            public Vector2Int BottomLeftCell { get; }
            public Vector2 WorldCenter { get; }

            public Candidate(Vector2Int bottomLeftCell, Vector2 worldCenter)
            {
                BottomLeftCell = bottomLeftCell;
                WorldCenter = worldCenter;
            }
        }

        public IEnumerable<PropPlacementInfo> GetPlacements(RoomDecorationContext context, Vector2Int propSize)
        {
            var yielded = new HashSet<Vector2Int>();

            while (true)
            {
                List<Candidate> candidates = CollectCandidates(context, propSize, yielded);
                if (candidates.Count == 0) yield break;

                int index = SelectIndex(candidates, context, propSize);
                Candidate chosen = candidates[index];
                yielded.Add(chosen.BottomLeftCell);

                yield return PropPlacementInfo.FromOrigin(chosen.BottomLeftCell, propSize);
            }
        }

        private List<Candidate> CollectCandidates(RoomDecorationContext context, Vector2Int propSize, HashSet<Vector2Int> excluded)
        {
            var result = new List<Candidate>();

            foreach (Vector2Int cell in context.GetAvailableTiles())
            {
                if (excluded.Contains(cell)) continue;

                var placement = PropPlacementInfo.FromOrigin(cell, propSize);
                if (!context.CanPlace(placement)) continue;

                var candidate = new Candidate(cell, context.GetPlacementWorldCenter(placement));
                if (!IsCandidateValid(candidate, context, propSize)) continue;

                result.Add(candidate);
            }

            return result;
        }

        /// <summary>Override to filter candidates by rule (e.g. "only top row", "must hug a wall").</summary>
        protected virtual bool IsCandidateValid(Candidate candidate, RoomDecorationContext context, Vector2Int propSize) => true;

        /// <summary>Override to weight candidates (e.g. edge bias, clustering). Default: uniform random.</summary>
        protected virtual int SelectIndex(IReadOnlyList<Candidate> candidates, RoomDecorationContext context, Vector2Int propSize)
        {
            return Random.Range(0, candidates.Count);
        }

        protected static int WeightedRandomIndex(IReadOnlyList<float> weights)
        {
            float total = 0f;
            foreach (float w in weights) total += w;
            if (total <= 0f) return Random.Range(0, weights.Count);

            float roll = Random.value * total;
            float cumulative = 0f;
            for (int i = 0; i < weights.Count; i++)
            {
                cumulative += weights[i];
                if (roll <= cumulative) return i;
            }
            return weights.Count - 1;
        }
    }
}