using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.game.view.overview
{
    class EarthDebrisView : TerrariumElement
    {
        public GameObject[] Prefabs;
        public int[] PrefabsSpawnCount;
        public GameObject[] Parents;

        public float HeightCutoff;

        void Start()
        {
            InstansiatePrefabs();
        }

        private void InstansiatePrefabs()
        {
            for (int i = 0; i < Prefabs.Length; i++)
            {
                for (int j = 0; j < PrefabsSpawnCount[i]; j++)
                {
                    Vector3 spawnPosition = Random.onUnitSphere * ((gameObject.transform.localScale.x / 2)) + gameObject.transform.position;

                    if (HeightCutoff < 0)
                    {
                        while (spawnPosition.y < HeightCutoff)
                        {
                            spawnPosition = Random.onUnitSphere *
                                            ((gameObject.transform.localScale.x / 2)) + gameObject.transform.position;
                        }
                    }

                    Quaternion spawnRotation = Quaternion.identity;
                    GameObject instance = Instantiate(Prefabs[i], spawnPosition, spawnRotation) as GameObject;
                    instance.transform.LookAt(gameObject.transform);
                    instance.transform.Rotate(-90, 0, 0);

                    //                    float randomX = Random.value * SpawnInArea.x;
                    //
                    //                    Vector3 spawnPosition = Random.onUnitSphere * (SpawnInRadius + 1 * 0.5f);
                    //                    GameObject instance = Instantiate(Prefabs[i], spawnPosition, Quaternion.identity) as GameObject;
                    //                    instance.transform.LookAt(gameObject.transform.position);
                    //                    instance.transform.Rotate(-90, 0, 0);
                    //
                    //                    Vector3 spawnPosition = new Vector3(randomX, SpawnInRadius, randomZ);
                    //                    GameObject instance = (GameObject) Instantiate(Prefabs[i], spawnPosition, Quaternion.identity);
                    instance.transform.parent = Parents[i].transform;
                }
            }
        }
    }
}
