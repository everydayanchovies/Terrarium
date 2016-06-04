using UnityEngine;
using System.Collections;

public abstract class TerrariumController : TerrariumElement
{
    void Start()
    {
        app.RegisterController(this);
    }

    public abstract void OnNotification (string p_event_path, Object p_target, params object[] p_data);
}
