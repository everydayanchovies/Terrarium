using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.game.controller
{
    class CameraController : TerrariumElement, Controller
    {
        void Start()
        {
            app.RegisterController(this);
        }

        // Handles the ball hit event
        void Controller.OnNotification(string p_event_path, Object p_target, params object[] p_data)
        {
            switch (p_event_path)
            {
                case TreeNotification.TreeGrewSignificantly:
                    break;
            }
        }
    }
}
