using UnityEngine;
using System.Collections;
using Wasabimole.ProceduralTree;

namespace Assets.game.view.soil
{
    public class TreeView : TerrariumElement
    {
        public ProceduralTree proceduralTree;

        public float TwistAmount;

        private TreeController treeController;

        // Use this for initialization
        void Start()
        {
            treeController = gameObject.AddComponent<TreeController>();

            proceduralTree.Seed = Random.Range(0, 65536);

            twist = proceduralTree.Twisting;
        }

        float twist;

        // Update is called once per frame
        void Update()
        {
            proceduralTree.Twisting = Mathf.Lerp(twist - TwistAmount, twist + TwistAmount,
  Mathf.SmoothStep(0f, 1f,
   Mathf.PingPong(Time.time / 10, 1f)
 ));
        }
    }
}