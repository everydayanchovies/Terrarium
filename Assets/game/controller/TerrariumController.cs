using UnityEngine;
using System.Collections;

// Controls the app workflow.
public class TerrariumController : TerrariumElement, Controller
{
	void Start ()
	{
		app.RegisterController (this);
	}

	// Handles the ball hit event
	void Controller.OnNotification (string p_event_path, Object p_target, params object[] p_data)
	{
		switch (p_event_path) {
         
		case TerrariumNotification.GameComplete:
			Debug.Log ("Victory!!");
			break;
         
		case TerrariumNotification.LeftKeyPressed:
			Debug.Log ("Left key");
			break;
		}	
	}
}
