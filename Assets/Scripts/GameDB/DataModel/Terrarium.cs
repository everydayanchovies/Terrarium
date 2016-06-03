using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.GameDB.DataModel;
using UnityEngine;
using Object = System.Object;
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

        public Tree Tree { get; set; }
        public int GroundColor { get; set; }
        public int SkyColor { get; set; }
        public List<Debris> DebrisList { get; set; }
    }
}
