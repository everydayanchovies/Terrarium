using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.GameDB.DataModel;
using Assets.Scripts.ProceduralTree;
using UnityEngine;
using Object = System.Object;
using Random = UnityEngine.Random;
using Tree = UnityEngine.Tree;

namespace Assets.Scripts.GameDB.DataModel
{
    [Serializable]
    public class Terrarium
    {
        public Terrarium(Tree tree, int groundColor, int skyColor)
        {
            Tree = tree;
            GroundColor = groundColor;
            SkyColor = skyColor;
        }

        private Tree _tree;

        public Tree Tree
        {
            get
            {
                if (_tree == null)
                {
                    // Terrarium doesn't yet have a tree
                    // Create a new bonsai tree
                    _tree = TreeGenerator.GetBonsaiTree();
                }
                return _tree;
            }
            set { _tree = value; }
        }

        public int GroundColor { get; set; }
        public int SkyColor { get; set; }
        public List<Debris> DebrisList { get; set; }
    }
}
