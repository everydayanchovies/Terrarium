using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ModularTree
{
	public class TargetItem
	{
		public GameObject gameObject{ get; set; }

		public int index{ get; set; }

		public bool branch{ get; set; }
	}

	[ExecuteInEditMode]
	public class ModularTree : TerrariumElement
	{
		public const int CurrentVersion = 103;

		// ---------------------------------------------------------------------------------------------------------------------------
		// Tree parameters (can be changed real-time in editor or game)
		// ---------------------------------------------------------------------------------------------------------------------------


		public int Seed;
		// Random seed on which the generation is based
		[Range (1024, 65000)]
		public int MaxNumVertices = 65000;
		// Maximum number of vertices for the tree mesh
		[Range (3, 32)]
		public int NumberOfSides = 16;
		// Number of sides for tree
		[Range (0.25f, 4f)]
		public float BaseRadius = 2f;
		// Base radius in meters
		[Range (0.75f, 0.95f)]
		public float RadiusStep = 0.9f;
		// Controls how quickly radius decreases
		[Range (0.01f, 0.2f)]
		public float MinimumRadius = 0.02f;
		// Minimum radius for the tree's smallest branches
		[Range (0f, 1f)]
		public float BranchRoundness = 0.8f;
		// Controls how round branches are
		[Range (0.01f, 0.5f)]
		public float SegmentLength = 0.2f;
		// Length of branch segments
		[Range (0f, 40f)]
		public float Twisting = 20f;
		// How much branches twist

		private float Progress = 1f;

		public bool RenderTree = true;

		// ---------------------------------------------------------------------------------------------------------------------------

		float checksum;
		// Serialized & Non-Serialized checksums for tree rebuilds only on undo operations, or when parameters change (mesh kept on scene otherwise)
		[SerializeField, HideInInspector]
		float checksumSerialized;

		//[HideInInspector]
		public int BranchCount;

		[HideInInspector]
		public List<Vector3> vertexList;
		// Vertex list
		List<Vector2> uvList;
		// UV list
		List<int> triangleList;
		// Triangle list

		float[] ringShape;
		// Tree ring shape array

		[HideInInspector, System.NonSerialized]
		public MeshRenderer Renderer;
		// MeshRenderer component

		MeshFilter filter;
		// MeshFilter component

		#if UNITY_EDITOR
		[HideInInspector]
		public string MeshInfo;
		// Used in ProceduralTreeEditor to show info about the tree mesh
		#endif

		// ---------------------------------------------------------------------------------------------------------------------------
		// Initialise object, make sure it has MeshFilter and MeshRenderer components
		// ---------------------------------------------------------------------------------------------------------------------------

		void OnEnable ()
		{
			if (filter != null && Renderer != null)
				return;

			gameObject.isStatic = true;

			if (RenderTree) {
				filter = gameObject.GetComponent<MeshFilter> ();
				if (filter == null)
					filter = gameObject.AddComponent<MeshFilter> ();
				if (filter.sharedMesh != null)
					checksum = checksumSerialized;
				Renderer = gameObject.GetComponent<MeshRenderer> ();
				if (Renderer == null)
					Renderer = gameObject.AddComponent<MeshRenderer> ();
			}
		}

		// ---------------------------------------------------------------------------------------------------------------------------
		// Generate tree (only called when parameters change, or there's an undo operation)
		// ---------------------------------------------------------------------------------------------------------------------------

		public void GenerateTree ()
		{
			gameObject.isStatic = false;

			var originalRotation = transform.localRotation;
			var originalSeed = Random.seed;

			if (vertexList == null
			    || uvList == null
			    || triangleList == null) { // Create lists for holding generated vertices
				vertexList = new List<Vector3> ();
				uvList = new List<Vector2> ();
				triangleList = new List<int> ();
			} else { // Clear lists for holding generated vertices
				vertexList.Clear ();
				uvList.Clear ();
				triangleList.Clear ();
			}

			targets = new List<TargetItem> ();

			GameObject[] targetHolders = GameObject.FindGameObjectsWithTag ("targetsHolder");
			if (targetHolders != null && targetHolders.Length > 0) {
				foreach (var targetHolder in targetHolders) {
					GameObject.DestroyImmediate (targetHolder);
				}
			}

			targetsHolder = new GameObject ();
			targetsHolder.name = "Targets holder";
			targetsHolder.tag = "targetsHolder";
			targetsHolder.transform.SetParent (transform.parent);

			SetTreeRingShape (); // Init shape array for current number of sides

			Random.seed = Seed;

			BranchCount = 0;

			// Main recursive call, starts creating the ring of vertices in the trunk's base
			Branch (new Quaternion (), Vector3.zero, -1, BaseRadius * Progress, 0f);

			Random.seed = originalSeed;

			transform.localRotation = originalRotation; // Restore original object rotation

			SetTreeMesh (); // Create/Update MeshFilter's mesh
		}

		// ---------------------------------------------------------------------------------------------------------------------------
		// Set the tree mesh from the generated vertex lists (vertexList, uvList, triangleLists)
		// ---------------------------------------------------------------------------------------------------------------------------

		private void SetTreeMesh ()
		{
			if (RenderTree) {
				// Get mesh or create one
				var mesh = filter.sharedMesh;
				if (mesh == null)
					mesh = filter.sharedMesh = new Mesh ();
				else
					mesh.Clear ();

				// Assign vertex data
				mesh.vertices = vertexList.ToArray ();
				mesh.uv = uvList.ToArray ();
				mesh.triangles = triangleList.ToArray ();

				// Update mesh
				mesh.RecalculateNormals ();
				mesh.RecalculateBounds ();
				mesh.Optimize (); // Do not call this if we are going to change the mesh dynamically!
			}

			#if UNITY_EDITOR
			MeshInfo = "Mesh has " + vertexList.Count + " vertices and " + triangleList.Count / 3 + " triangles";
			#endif
		}

		// ---------------------------------------------------------------------------------------------------------------------------
		// Main branch recursive function to generate tree
		// ---------------------------------------------------------------------------------------------------------------------------
		public GameObject targetPrefab;
		private List<TargetItem> targets;
		public TargetItem[] Targets;
		private GameObject targetsHolder;

		void Branch (Quaternion quaternion, Vector3 position, int lastRingVertexIndex, float radius, float texCoordV)
		{
			var offset = Vector3.zero;
			var texCoord = new Vector2 (0f, texCoordV);
			var textureStepU = 1f / NumberOfSides;
			var angInc = 2f * Mathf.PI * textureStepU;
			var ang = 0f;

			// Add ring vertices
			for (var n = 0; n <= NumberOfSides; n++, ang += angInc) {
				var r = (ringShape [n] * radius) * Progress;
				offset.x = r * Mathf.Cos (ang); // Get X, Z vertex offsets
				offset.z = r * Mathf.Sin (ang);
				vertexList.Add (position + quaternion * offset); // Add Vertex position
				uvList.Add (texCoord); // Add UV coord
				texCoord.x += textureStepU;
			}

			if (lastRingVertexIndex >= 0) { // After first base ring is added ...
				// Create new branch segment quads, between last two vertex rings
				for (var currentRingVertexIndex = vertexList.Count - NumberOfSides - 1; currentRingVertexIndex < vertexList.Count - 1; currentRingVertexIndex++, lastRingVertexIndex++) {
					triangleList.Add (lastRingVertexIndex + 1); // Triangle A
					triangleList.Add (lastRingVertexIndex);
					triangleList.Add (currentRingVertexIndex);
					triangleList.Add (currentRingVertexIndex); // Triangle B
					triangleList.Add (currentRingVertexIndex + 1);
					triangleList.Add (lastRingVertexIndex + 1);
				}
			}

			// Do we end current branch?
			radius *= Mathf.Sqrt (RadiusStep * Progress);
			if (radius < MinimumRadius || vertexList.Count + NumberOfSides >= MaxNumVertices) { // End branch if reached minimum radius, or ran out of vertices
				// Create a cap for ending the branch
				vertexList.Add (position); // Add central vertex
				uvList.Add (texCoord + Vector2.one); // Twist UVs to get rings effect
				for (var n = vertexList.Count - NumberOfSides - 2; n < vertexList.Count - 2; n++) { // Add cap
					triangleList.Add (n);
					triangleList.Add (vertexList.Count - 1);
					triangleList.Add (n + 1);
				}
				return; 
			}

			var segmentLength = SegmentLength;

			// Trunk
			if (lastRingVertexIndex == 0) {
				segmentLength *= 2;
			}

			// Continue current branch (randomizing the angle)
			texCoordV += 0.0625f * (SegmentLength + SegmentLength / radius);
			position += quaternion * new Vector3 (0f, SegmentLength, 0f);
			transform.rotation = quaternion; 
			var x = (Random.value - 0.5f) * Twisting;
			var z = (Random.value - 0.5f) * Twisting;
			transform.Rotate (x, 0f, z);
			lastRingVertexIndex = vertexList.Count - NumberOfSides - 1;
			Branch (transform.rotation, position, lastRingVertexIndex, radius, texCoordV); // Next segment

			if (lastRingVertexIndex % 5 == 0) {
				GameObject targetGO = (GameObject)Instantiate (targetPrefab, Vector3.zero, quaternion);
				targetGO.transform.SetParent (targetsHolder.transform);
				targetGO.transform.position = position + transform.position;
				TargetItemView targetItemView = targetGO.GetComponent<TargetItemView> ();
				targetItemView.index = lastRingVertexIndex;

				TargetItem targetItem = new TargetItem ();
				targetItem.gameObject = targetGO;
				targetItem.index = lastRingVertexIndex;
				targetItem.branch = false;
				targets.Add (targetItem);
			}

			// Do we branch?
//			if (vertexList.Count + NumberOfSides >= MaxNumVertices || Random.value > (BranchProbability * 0.25f * 0.5f))
			if (BranchCount > 30)
				return;

			bool shouldBranch = false;
			if (Targets != null) {
				foreach (var target in Targets) {
					if (target.index == lastRingVertexIndex) {
						shouldBranch = target.branch;
					}
				}
			}

			if (!shouldBranch) {
				return;
			}

			BranchCount++;

			// Yes, add a new branch
			transform.rotation = quaternion;
			x = Random.value * 70f - 35f;
			x += x > 0 ? 10f : -10f;
			z = Random.value * 70f - 35f;
			z += z > 0 ? 10f : -10f;
			transform.Rotate (x, 0f, z);
			Branch (transform.rotation, position, lastRingVertexIndex, radius, texCoordV);
		}

		// ---------------------------------------------------------------------------------------------------------------------------
		// Try to get shared mesh for new prefab instances
		// ---------------------------------------------------------------------------------------------------------------------------

		#if UNITY_EDITOR
		bool CanGetPrefabMesh ()
		{
			// Return false if we are not instancing a new procedural tree prefab
			if (PrefabUtility.GetPrefabType (this) != PrefabType.PrefabInstance)
				return false;
			if (filter.sharedMesh != null)
				return true;

			// Try to get mesh from an existing instance
			var parentPrefab = PrefabUtility.GetPrefabParent (this);
			var list = (ModularTree[])FindObjectsOfType (typeof(ModularTree));
			foreach (var go in list)
				if (go != this && PrefabUtility.GetPrefabParent (go) == parentPrefab) {
					filter.sharedMesh = go.filter.sharedMesh;
					return true;
				}
			return false;
		}
		#endif

		// ---------------------------------------------------------------------------------------------------------------------------
		// Set tree shape, by computing a random offset for every ring vertex
		// ---------------------------------------------------------------------------------------------------------------------------

		private void SetTreeRingShape ()
		{
			ringShape = new float[NumberOfSides + 1];
			var k = (1f - BranchRoundness) * 0.5f;
			// Randomize the vertex offsets, according to BranchRoundness
			Random.seed = Seed;
			for (var n = 0; n < NumberOfSides; n++)
				ringShape [n] = 1f - (Random.value - 0.5f) * k;
			ringShape [NumberOfSides] = ringShape [0];
		}

		public void Start ()
		{
			GenerateTree ();
		}

		// ---------------------------------------------------------------------------------------------------------------------------
		// Update function will return, unless the tree parameters have changed
		// ---------------------------------------------------------------------------------------------------------------------------

		public void Update ()
		{
			// Tree parameter checksum (add any new parameters here!)
			var newChecksum = (Seed & 0xFFFF) + NumberOfSides + SegmentLength + BaseRadius + MaxNumVertices +
			                  RadiusStep + MinimumRadius + Twisting + BranchRoundness;

			// Return (do nothing) unless tree parameters change
			if (newChecksum == checksum &&
			    (RenderTree && filter.sharedMesh != null))
				return;

			checksumSerialized = checksum = newChecksum;

			#if UNITY_EDITOR
			if (!CanGetPrefabMesh ()) 
			#endif
			{
				GenerateTree (); // Update tree mesh
			}
		}

		// ---------------------------------------------------------------------------------------------------------------------------
		// Destroy procedural mesh when object is deleted
		// ---------------------------------------------------------------------------------------------------------------------------

		#if UNITY_EDITOR
		void OnDisable ()
		{
			if (RenderTree) {
				if (filter.sharedMesh == null)
					return; // If tree has a mesh
			}
			if (PrefabUtility.GetPrefabType (this) == PrefabType.PrefabInstance) { // If it's a prefab instance, look for siblings
				var parentPrefab = PrefabUtility.GetPrefabParent (this);
				var list = (ModularTree[])FindObjectsOfType (typeof(ModularTree));
				foreach (var go in list)
					if (go != this && PrefabUtility.GetPrefabParent (go) == parentPrefab)
						return; // Return if there's another prefab instance still using the mesh
			}

			if (RenderTree) {
				DestroyImmediate (filter.sharedMesh, true); // Delete procedural mesh
			}
		}
		#endif

		public Material GetMaterial ()
		{
			if (!RenderTree) {
				return null;
			}

			Renderer renderer = GetComponent<Renderer> ();
			return renderer.material;
		}

		public void ResetTargets ()
		{
			Targets = targets.ToArray ();
		}

		public void UpdateTargets ()
		{
			var _targets = Targets;
			ResetTargets ();
			foreach (var tar1 in Targets) {
				foreach (var tar2 in _targets) {
					if (tar1.index == tar2.index) {
						tar1.branch = tar2.branch;
					}
				}
			}
		}

		public Color targetColourSelected;
		public Color targetColourIdle;

		public void ToggleBranch (int index)
		{
			if (Targets == null) {
				ResetTargets ();
			}

			foreach (var target in Targets) {
				if (target.index == index) {
					
					int cost = (int)((1f / 100f / ((float)index / MaxNumVertices)) * 5f);

					if (app.model.energy - cost < 0) {
						return;
					}

					if (target.gameObject.transform.localScale.x > 1) {
						// Actually branch
						target.branch = !target.branch;

						app.model.energy -= cost;

						app.Notify(TerrariumNotification.EnergyChanged, this);
					} else {
						// Verify branch
						foreach (Transform tar1 in targetsHolder.transform) {
							if (tar1.position == target.gameObject.transform.position) {
								tar1.localScale = 1.3f * Vector3.one;
								tar1.gameObject.GetComponent<Renderer> ().material.color = targetColourSelected;
							} else {
								tar1.localScale = 0.7f * Vector3.one;
								tar1.gameObject.GetComponent<Renderer> ().material.color = targetColourIdle;
							}
						}

						return;
					}
				}
			}

			GenerateTree ();

			UpdateTargets ();
		}
	}
}