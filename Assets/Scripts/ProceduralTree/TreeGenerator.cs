using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.GameDB.DataModel;
using Random = UnityEngine.Random;

namespace Assets.Scripts.ProceduralTree
{
    class TreeGenerator
    {
        public static Tree GetBonsaiTree()
        {
            return new Tree(Random.Range(0, 6000), 1024 * 5, 10, 1.4f, 0.8f, 0.05f, 0.9f, 0.25f, 25, 0.85f);
        }
    }
}
