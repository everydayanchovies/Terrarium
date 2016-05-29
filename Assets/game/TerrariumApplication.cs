using UnityEngine;
using System.Collections;

// Base class for all elements in this application.
using System.Collections.Generic;


public class TerrariumElement : MonoBehaviour
{
	// Gives access to the application and all instances.
	public TerrariumApplication app { get { return GameObject.FindObjectOfType<TerrariumApplication> (); } }
}

// 10 Terrarium Entry Point.
public class TerrariumApplication : MonoBehaviour
{
	// Reference to the root instances of the MVC.
	public TerrariumModel model;
	public TerrariumView view;

	public IList<Controller> controllerList;

	// Init things here
	void Start ()
	{
		//gameObject.AddComponent<TerrariumController>();
	}
   
	// Iterates all Controllers and delegates the notification data
	// This method can easily be found because every class is “TerrariumElement” and has an “app”
	// instance.
	public void Notify (string p_event_path, Object p_target, params object[] p_data)
	{
		if (controllerList == null) {
			return;
		}

		if (controllerList.Count <= 0) {
			Debug.LogWarning ("No controllers registered: " + controllerList.Count);
			return;
		}

		foreach (Controller c in controllerList) {
			c.OnNotification (p_event_path, p_target, p_data);
		}
	}

	public void RegisterController (Controller controller)
	{
		if (controllerList == null) {
			controllerList = new List<Controller> ();
		}

		if (controllerList.Contains (controller)) {
			controllerList.Remove (controller);
		}

		controllerList.Add (controller);

		Debug.Log ("New controller registered: " + controllerList.Count);
	}
}