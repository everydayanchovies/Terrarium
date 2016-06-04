using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.game.notifications;
using Assets.Scripts.GameDB.DataModel;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.game.controller
{
    public class GameController : TerrariumController
    {
        public override void OnNotification(string p_event_path, Object p_target, params object[] p_data)
        {
            switch (p_event_path)
            {

                case GameNotification.TerrariumLoaded:
                    Terrarium terrarium = (Terrarium)p_data[0];
                    //Debug.Log("Terrarium loaded! Tree seed: " + terrarium.Tree.Seed);
                    break;
            }
        }
    }
}
