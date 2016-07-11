using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.game.view.overview;

// Controls the app workflow.
using Wasabimole.ProceduralTree;


public class TreeController : TerrariumElement, Controller
{
	void Start ()
	{
		app.RegisterController (this);
	}

	// Handles the ball hit event
	void Controller.OnNotification (string p_event_path, Object p_target, params object[] p_data)
	{
		switch (p_event_path) {
		case TreeNotification.TreeGrewSignificantly:
			Debug.Log ("Tree grew significantly!");
			//StartCoroutine (FlashScreen ());
			//StartCoroutine (RematerializeTree ());
			break;
		case TreeNotification.TreeHasEvolved:
			Debug.Log ("Tree has evolved!");
			//StartCoroutine (FlashScreen ());
			//StartCoroutine (RematerializeTree ());
			break;
		case TreeNotification.TargetItemClicked:
			int index = (int)p_data [0];
			ModularTree.ModularTree tree = GameObject.FindObjectOfType<ModularTree.ModularTree> ();
			Debug.Log ("Target item clicked! (" + index + ")");

			tree.ToggleBranch (index);
			break;
		}
	}

	IEnumerator FlashScreen ()
	{
		SimpleBlit simpleBlit = Camera.main.GetComponent<SimpleBlit> ();
		Material material = simpleBlit.TransitionMaterial;

		float value = 1;
		for (int i = 0; i < 5; i++) {
			value = 1 - value;
			material.SetFloat ("_Fade", value);
			yield return new WaitForSeconds (0.3f);
		}
	}

	public void RematerializeTree ()
	{
		StartCoroutine (RematerializeTreeIEnumerator ());
	}

	public bool isRematerializingTree = false;

	IEnumerator RematerializeTreeIEnumerator ()
	{
		bool twistTree = true;

		if (isRematerializingTree) {
			return false;
		}

		isRematerializingTree = true;

		TreeView treeView = GetComponent<TreeView> ();
		ProceduralTree proceduralTree = GetComponentInChildren<ProceduralTree> ();
		Material material = proceduralTree.GetMaterial ();

		int speed = 3;

		for (int i = 1; i < 80 / speed; i++) {
			material.SetFloat ("_Cutoff", i * speed / 100f);
			yield return new WaitForSeconds (0.0001f);
		}

		material.SetFloat ("_Cutoff", 1f);

		int twist = (int)proceduralTree.Twisting;
		int twistAmount = 150;

		if (twistTree) {
			for (int i = twist; i < twistAmount - twist; i++) {
				proceduralTree.Twisting = i;
				yield return new WaitForSeconds (0.00001f);
			}
		}

		yield return new WaitForSeconds (0.8f);

		treeView.GrowTree ();
		//treeView.EvolveTree (1);
		//yield return new WaitForSeconds (0.9f);

		if (twistTree) {
			for (int i = twist; i < twistAmount - twist; i++) {
				proceduralTree.Twisting = twistAmount - i;
				yield return new WaitForSeconds (0.00001f);
			}
		}

		for (int i = 1; i < 100 / speed; i++) {
			material.SetFloat ("_Cutoff", (100 - i * speed) / 100f);
			yield return new WaitForSeconds (0.0001f);
		}

		material.SetFloat ("_Cutoff", 0f);

		isRematerializingTree = false;
	}
}
