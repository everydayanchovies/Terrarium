using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.GameDB.DataModel;
using UnityEngine;

namespace Assets.Scripts.GameDB
{
    public class DBHelper
    {
        private const string TERRARIUM_KEY = "TERRARIUM";

        public static Terrarium GetTerrarium(int id)
        {
            Terrarium terrarium = Serializer.Load<Terrarium>(TERRARIUM_KEY + id);

            return terrarium;
        }

        public static bool SaveTerrarium(Terrarium terrarium, int id)
        {
            try
            {
                Serializer.Save<Terrarium>(TERRARIUM_KEY + id, terrarium);

                return true;
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
                return false;
            }
        }

        public static bool DoesTerrariumExist(int id)
        {
            return GetTerrarium(id) == null;
        }
    }
}
