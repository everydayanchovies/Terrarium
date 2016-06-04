using System;
using UnityEngine;
using System.Collections;

// Base class for all elements in this application.
using System.Collections.Generic;
using Assets.game;
using Object = UnityEngine.Object;


public class TerrariumElement : MonoBehaviour
{
    // Gives access to the application and all instances.
    public TerrariumApplication app { get { return GameObject.FindObjectOfType<TerrariumApplication>(); } }
}

// 10 Terrarium Entry Point.
public class TerrariumApplication : MonoBehaviour
{
    // Reference to the root instances of the MVC.
    public TerrariumModel Model;
    public TerrariumView View;
    public TerrariumController Controller;

    public IList<TerrariumController> controllerList;

    // Init things here
    void Start()
    {
        //gameObject.AddComponent<TerrariumController>();
    }

    // Iterates all Controllers and delegates the notification data
    // This method can easily be found because every class is “TerrariumElement” and has an “app”
    // instance.
    public void Notify(string p_event_path, Object p_target, params object[] p_data)
    {
        if (controllerList == null)
        {
            return;
        }

        if (controllerList.Count <= 0)
        {
            Debug.LogWarning("No controllers registered: " + controllerList.Count);
            return;
        }

        foreach (TerrariumController c in controllerList)
        {
            c.OnNotification(p_event_path, p_target, p_data);
        }
    }

    public void RegisterController(TerrariumController controller)
    {
        if (controllerList == null)
        {
            controllerList = new List<TerrariumController>();
        }

        if (controllerList.Contains(controller))
        {
            controllerList.Remove(controller);
        }

        controllerList.Add(controller);

        Debug.Log("New controller registered: " + controller.GetType().Name);
    }

    public T GetModel<T>()
    {
        return (T)Convert.ChangeType(Model, typeof(T));
    }

    public T GetView<T>()
    {
        return (T)Convert.ChangeType(View, typeof(T));
    }

    public T GetController<T>()
    {
        return (T)Convert.ChangeType(Controller, typeof(T));
    }
}