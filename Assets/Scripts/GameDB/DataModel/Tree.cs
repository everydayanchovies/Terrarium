using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.GameDB.DataModel
{
    [Serializable]
    public class Tree
    {
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
