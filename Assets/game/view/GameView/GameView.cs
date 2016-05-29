using UnityEngine;
using System.Collections;

public class GameView : TerrariumElement
{
	public int cameraPanSideOffset;
	public Camera camera;

	private Vector3 targetCameraPosition;

	void Start ()
	{
		camera = Camera.main;
	}

	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			app.Notify (TerrariumNotification.LeftKeyPressed, this);
		
			ResetCameraPosition ();
			targetCameraPosition.x = -cameraPanSideOffset;
		}

		if (Input.GetKeyDown (KeyCode.RightArrow)) {
			ResetCameraPosition ();
			targetCameraPosition.x = cameraPanSideOffset;
		}

		if (Input.GetKeyDown (KeyCode.UpArrow)) {
			ResetCameraPosition ();
			targetCameraPosition.y = 6;
		}

		if (Input.GetKeyDown (KeyCode.Space)) {
			ResetCameraPosition ();
			targetCameraPosition.z = -5;
		}

		if (!(Input.GetKey (KeyCode.LeftArrow)
		    || Input.GetKey (KeyCode.RightArrow)
			|| Input.GetKey (KeyCode.UpArrow)
			|| Input.GetKey (KeyCode.DownArrow)
			|| Input.GetKey (KeyCode.Space))) {

			ResetCameraPosition ();
		}
        
		if (Vector3.Distance (camera.transform.position, targetCameraPosition) > 0.3f) {
			camera.transform.position =
				Vector3.Lerp (camera.transform.position,
				targetCameraPosition,
				Time.deltaTime * 3);
		}
	}

	private void ResetCameraPosition ()
	{
		targetCameraPosition = Vector3.zero;
		targetCameraPosition.y = 3;
		targetCameraPosition.z = -10;
	}
}