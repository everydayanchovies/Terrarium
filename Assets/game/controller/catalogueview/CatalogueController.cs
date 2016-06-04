using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Object = UnityEngine.Object;

namespace Assets.game.controller.catalogueview
{
    class CatalogueController : TerrariumController
    {
        public override void OnNotification(string p_event_path, Object p_target, params object[] p_data)
        {
            switch (p_event_path)
            {
                case TreeNotification.TreeGrewSignificantly:
                    break;
            }
        }
    }
}
