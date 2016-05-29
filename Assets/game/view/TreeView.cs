using UnityEngine;
using System.Collections;
using Wasabimole.ProceduralTree;

public class TreeView : TerrariumElement
{

	// Num of vertices considered significant to tree growth
	[Range (32, 1024)]
	public int SignificantNumVertices = 32;

	public ProceduralTree proceduralTree;
	public ProceduralTree secondProceduralTree;

	private TreeController treeController;

	// Use this for initialization
	void Start ()
	{
		treeController = gameObject.AddComponent<TreeController> ();

		proceduralTree.Seed = Random.Range(0, 65536);

		secondProceduralTree.BaseRadius = proceduralTree.BaseRadius;
		secondProceduralTree.BranchProbability = proceduralTree.BranchProbability;
		secondProceduralTree.BranchRoundness = proceduralTree.BranchRoundness;
		secondProceduralTree.MaxNumVertices = proceduralTree.MaxNumVertices;
		secondProceduralTree.MinimumRadius = proceduralTree.MinimumRadius;
		secondProceduralTree.NumberOfSides = proceduralTree.NumberOfSides;
		secondProceduralTree.RadiusStep = proceduralTree.RadiusStep;
		secondProceduralTree.Seed = proceduralTree.Seed;
		secondProceduralTree.SegmentLength = proceduralTree.SegmentLength;
		secondProceduralTree.Twisting = proceduralTree.Twisting;
		secondProceduralTree.Progress = proceduralTree.Progress;
	}

	float interval = 2f;
	float nextTime = 0;

	int dir = 1;

	// Update is called once per frame
	void Update ()
	{
		if (proceduralTree.Twisting >= 7
		    || proceduralTree.Twisting <= 2) {
			dir *= -1;
		} 

		proceduralTree.Twisting += Time.deltaTime * dir * 0.3f;

		if (Time.time >= nextTime) {
			if (!treeController.isRematerializingTree) {
				GrowSecondTree (50);

				StartCoroutine (UpdateTreeWithDelay (1f));

				nextTime += interval; 
			}
		}
	}

	IEnumerator UpdateTreeWithDelay (float delay)
	{
		if (!treeController.isRematerializingTree) {
			yield return new WaitForSeconds (delay);

			if (secondProceduralTree.BranchCount >= proceduralTree.BranchCount) {
				if (Mathf.Abs (secondProceduralTree.BranchCount - proceduralTree.BranchCount) >= 3) {
					app.Notify (TreeNotification.TreeHasEvolved, this);
				}

				if (Mathf.Abs (secondProceduralTree.vertexList.Count - proceduralTree.vertexList.Count) > SignificantNumVertices) {
					app.Notify (TreeNotification.TreeGrewSignificantly, this);

					treeController.RematerializeTree ();
				} else {
					GrowTree ();
				}
			}
		}
	}

	public bool WillTreeChangeSignificantly (float nextStep)
	{
		if (treeController.isRematerializingTree) {
			return false;
		}

		PredictNextStep (nextStep);

		if (Mathf.Abs (secondProceduralTree.vertexList.Count - proceduralTree.vertexList.Count) > SignificantNumVertices) {
			app.Notify (TreeNotification.TreeGrewSignificantly, this);
			Debug.Log (secondProceduralTree.vertexList.Count + " - " + proceduralTree.vertexList.Count + " = " + Mathf.Abs (secondProceduralTree.vertexList.Count - proceduralTree.vertexList.Count) + "");
			return true;
		}

		return false;
	}

	private void GrowSecondTree (float steps)
	{
		float newProgress = secondProceduralTree.Progress + steps * 0.001f;

		if (newProgress >= 1f)
			newProgress = 1f;

		secondProceduralTree.Progress = newProgress;
	}

	private void PredictNextStep (float steps)
	{
		float newProgress = proceduralTree.Progress + steps * 0.001f;

		if (newProgress >= 1f)
			newProgress = 1f;
		
		secondProceduralTree.Progress = newProgress;
	}

	public void EvolveTree (float steps)
	{
		float newProgress = proceduralTree.Progress + steps * 0.001f;

		if (newProgress >= 1f)
			newProgress = 1f;

		proceduralTree.Progress = newProgress;
	}

	public void GrowTree ()
	{
		proceduralTree.Progress = secondProceduralTree.Progress;
	}
}
