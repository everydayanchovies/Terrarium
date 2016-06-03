using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.GameDB.DataModel
{
    [Serializable]
    public class Debris
    {
        public enum DebrisType
        {
            Rock, Bush
        }

        public DebrisType Type { get; set; }
        public float Progress { get; set; }
        public Vector3 Position { get; set; }
        public Quaternion Rotation { get; set; }
    }
}
