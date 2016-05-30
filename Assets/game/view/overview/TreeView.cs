using UnityEngine;
using System.Collections;
using Wasabimole.ProceduralTree;

namespace Assets.game.view.overview
{
    public class TreeView : TerrariumElement
    {
        // Num of vertices considered significant to tree growth
        [Range(32, 1024)]
        public int SignificantNumVertices = 32;

        public ProceduralTree proceduralTree;
        public ProceduralTree secondProceduralTree;
        public ProgressView progressView;
        public float TwistAmount;

        private TreeController treeController;

        // Use this for initialization
        void Start()
        {
            treeController = gameObject.AddComponent<TreeController>();

            proceduralTree.Seed = Random.Range(0, 65536);

            twist = proceduralTree.Twisting;

            if (secondProceduralTree != null)
            {
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
        }

        float interval = 2f;
        float nextTime = 0;

        int dir = 1;

        float twist;

        // Update is called once per frame
        void Update()
        {
            proceduralTree.Twisting = Mathf.Lerp(twist - TwistAmount, twist + TwistAmount,
  Mathf.SmoothStep(0f, 1f,
   Mathf.PingPong(Time.time / 10, 1f)
 ));

            if (Time.time >= nextTime)
            {
                if (!treeController.isRematerializingTree)
                {
                    GrowSecondTree(5);

                    StartCoroutine(UpdateTreeWithDelay(1f));

                    nextTime += interval;
                }
            }
        }

        public int SignificantNumBranches;

        IEnumerator UpdateTreeWithDelay(float delay)
        {
            if (treeController != null
                && secondProceduralTree != null
                && !treeController.isRematerializingTree)
            {
                yield return new WaitForSeconds(delay);

                int changeInBranches = Mathf.Abs(secondProceduralTree.BranchCount - proceduralTree.BranchCount);
                int changeInVertices = Mathf.Abs(secondProceduralTree.vertexList.Count - proceduralTree.vertexList.Count);

                if (changeInBranches >= SignificantNumBranches)
                {
                    app.Notify(TreeNotification.TreeHasEvolved, this);
                }

                if (changeInVertices > SignificantNumVertices)
                {
                    app.Notify(TreeNotification.TreeGrewSignificantly, this);
                }

                //if (secondProceduralTree.BranchCount - proceduralTree.BranchCount < 10) {
                if (changeInBranches >= SignificantNumBranches
                    || changeInBranches >= SignificantNumBranches)
                {
                    treeController.RematerializeTree();
                }
                else {
                    GrowTree();
                }
                //}
            }
        }

        public bool WillTreeChangeSignificantly(float nextStep)
        {
            if (treeController.isRematerializingTree)
            {
                return false;
            }

            PredictNextStep(nextStep);

            if (Mathf.Abs(secondProceduralTree.vertexList.Count - proceduralTree.vertexList.Count) > SignificantNumVertices)
            {
                app.Notify(TreeNotification.TreeGrewSignificantly, this);
                Debug.Log(secondProceduralTree.vertexList.Count + " - " + proceduralTree.vertexList.Count + " = " + Mathf.Abs(secondProceduralTree.vertexList.Count - proceduralTree.vertexList.Count) + "");
                return true;
            }

            return false;
        }

        private void GrowSecondTree(float steps)
        {
            if (secondProceduralTree == null)
            {
                return;
            }

            float newProgress = secondProceduralTree.Progress + steps * 0.001f;

            if (newProgress >= 1f)
                newProgress = 1f;

            secondProceduralTree.Progress = newProgress;

            progressView.SetProgress(newProgress * 100);
        }

        private void PredictNextStep(float steps)
        {
            float newProgress = proceduralTree.Progress + steps * 0.001f;

            if (newProgress >= 1f)
                newProgress = 1f;

            secondProceduralTree.Progress = newProgress;
        }

        public void EvolveTree(float steps)
        {
            float newProgress = proceduralTree.Progress + steps * 0.001f;

            if (newProgress >= 1f)
                newProgress = 1f;

            proceduralTree.Progress = newProgress;
        }

        public void GrowTree()
        {
            proceduralTree.Progress = secondProceduralTree.Progress;
        }
    }
}