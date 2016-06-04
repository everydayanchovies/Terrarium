using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.GameDB.DataModel
{
    [Serializable]
    public class Tree
    {
        /// <summary>
        /// Contains properties of a tree.
        /// </summary>
        /// <param name="seed">Random seed on which the generation is based</param>
        /// <param name="maxNumVertices">Maximum number of vertices for the tree mesh (1024, 65000)</param>
        /// <param name="numberOfSides">Number of sides for tree (3, 32)</param>
        /// <param name="baseRadius">Base radius in meters (0.25f, 4f)</param>
        /// <param name="radiusStep">Controls how quickly radius decreases (0.75f, 0.95f)</param>
        /// <param name="minRadius">Minimum radius for the tree's smallest branches (0.01f, 0.2f)</param>
        /// <param name="branchRoundness">Controls how round branches are (0f, 1f)</param>
        /// <param name="segmentLength">Length of branch segments (0.01f, 0.5f)</param>
        /// <param name="twisting">How much branches twist (0f, 40f)</param>
        /// <param name="branchProbability">Branch probability (0f, 1f)</param>
        public Tree(int seed, int maxNumVertices, int numberOfSides, float baseRadius, float radiusStep,
            float minRadius, float branchRoundness, float segmentLength, float twisting, float branchProbability)
        {
            Seed = seed;
            MaxNumVertices = maxNumVertices;
            NumberOfSides = numberOfSides;
            BaseRadius = baseRadius;
            RadiusStep = radiusStep;
            MinimumRadius = minRadius;
            BranchRoundness = branchRoundness;
            SegmentLength = segmentLength;
            Twisting = twisting;
            BranchProbability = branchProbability;
        }

        public int Seed { get; set; }
        // Random seed on which the generation is based
        public int MaxNumVertices { get; set; }
        // Maximum number of vertices for the tree mesh
        public int NumberOfSides { get; set; }
        // Number of sides for tree
        public float BaseRadius { get; set; }
        // Base radius in meters
        public float RadiusStep { get; set; }
        // Controls how quickly radius decreases
        public float MinimumRadius { get; set; }
        // Minimum radius for the tree's smallest branches
        public float BranchRoundness { get; set; }
        // Controls how round branches are
        public float SegmentLength { get; set; }
        // Length of branch segments
        public float Twisting { get; set; }
        // How much branches twist
        public float BranchProbability { get; set; }
        // Branch probability
        public float Progress { get; set; }
    }
}
