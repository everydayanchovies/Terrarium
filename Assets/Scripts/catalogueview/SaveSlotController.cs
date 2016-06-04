using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.game.notification;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.catalogueview
{
    class SaveSlotController : TerrariumElement
    {
        public int SlotId;

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform.position == gameObject.transform.position)
                    {
                        app.Notify(CatalogueViewNotifications.SaveSlotChosen, this, SlotId);
                        enabled = false;
                    }
                }
            }
        }
    }
}
