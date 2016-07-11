using UnityEngine;
using System.Collections;

public class TargetItemView : TerrariumElement {

	public int index;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown()
	{
		app.Notify(TreeNotification.TargetItemClicked, this, index);
	}
}
