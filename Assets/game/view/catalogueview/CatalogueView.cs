using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.game.model;
using UnityEngine;
using Wasabimole.ProceduralTree;
using Random = UnityEngine.Random;

namespace Assets.game.view.catalogueview
{
    class CatalogueView : TerrariumView
    {
        private CatalogueModel model;
        private List<List<ProceduralTree>> trees;

        void Start()
        {
            model = app.GetModel<CatalogueModel>();

            Vector2 treeCount = model.TerrariumCount;
            int x = (int)treeCount.x;
            int y = (int)treeCount.y;

            trees = new List<List<ProceduralTree>>();
            for (int i = 0; i < x; i++)
            {
                trees.Add(new List<ProceduralTree>());

                for (int j = 0; j < y; j++)
                {
                    trees[i].Add(null);
                }
            }

            PlaceTrees();

            model.TreesContainer.transform.localPosition = new Vector3(-x - 1, 0.5f, -y - 1);
        }

        private void PlaceTrees()
        {
            Scripts.GameDB.DataModel.Tree treeProperties = new Scripts.GameDB.DataModel.Tree(
                0, 1024 * 4, 3, 0, 0, 0, 0, 0, 0, 0
                );

            for (int x = 0; x < trees.Count; x++)
            {
                for (int y = 0; y < trees[x].Count; y++)
                {
                    Vector3 pos = new Vector3(x * 4, 0, y * 4);
                    GameObject treePotGameObject = (GameObject)Instantiate(model.TreePot, Vector3.zero, Quaternion.identity);
                    treePotGameObject.transform.localPosition = pos;
                    treePotGameObject.transform.SetParent(model.TreesContainer.transform);

                    GameObject treeGameObject = new GameObject(String.Format("Tree ({0}, {1})", x, y));
                    treeGameObject.transform.SetParent(treePotGameObject.transform.FindChild("TreeContainer"));
                    treeGameObject.transform.localPosition = Vector3.zero;
                    treeGameObject.transform.localScale = Vector3.one * 0.3f;
                    treeGameObject.AddComponent<ProceduralTree>();
                    treeGameObject.GetComponent<Renderer>().material = model.TreeMaterial;

                    ProceduralTree tree = treeGameObject.GetComponent<ProceduralTree>();

                    tree.Seed = Random.Range(0, 6000);
                    tree.BranchRoundness = treeProperties.BranchRoundness;
                    tree.MaxNumVertices = treeProperties.MaxNumVertices;
                    tree.NumberOfSides = treeProperties.NumberOfSides;
                    tree.Progress = Random.Range(0.7f, 1f);

                    trees[x][y] = tree;
                }
            }
        }
    }
}
