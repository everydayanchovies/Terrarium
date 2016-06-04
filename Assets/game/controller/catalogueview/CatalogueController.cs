using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.game.notification;
using Assets.game.view.catalogueview;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.game.controller.catalogueview
{
    class CatalogueController : TerrariumController
    {
        public override void OnNotification(string p_event_path, Object p_target, params object[] p_data)
        {
            switch (p_event_path)
            {
                case CatalogueViewNotifications.SaveSlotChosen:
                    int slotId = (int) p_data[0];
                    LoadTerrarium(slotId);
                    break;
            }
        }

        private void LoadTerrarium(int slotId)
        {
            // TODO: screen transitions

            app.GetView<CatalogueView>().OpenTerrarium(slotId);
        }
    }
}
