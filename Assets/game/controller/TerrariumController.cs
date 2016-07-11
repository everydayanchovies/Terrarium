using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// Controls the app workflow.
public class TerrariumController : TerrariumElement, Controller
{
	public Text energyText;

	void Start ()
	{
		app.RegisterController (this);

		app.model.energy = 300;

		app.Notify(TerrariumNotification.EnergyChanged, this);
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

		case TerrariumNotification.EnergyChanged:
			int energy = app.model.energy;

			energyText.text = "Energy: " + energy;
			break;
		}	
	}
}
