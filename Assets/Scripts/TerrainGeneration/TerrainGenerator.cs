using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TerrainGeneration
{
    public class TerrainGenerator : MonoBehaviour
    {
        [SerializeField] private PlayerController playerController;

        [Header("Initialization")]
        [SerializeField] private GameObject cubePrefab;
        [SerializeField] private int worldXSize;
        [SerializeField] private int worldZSize;

        [Header("Variables")]
        [SerializeField] private float noiseScale = 0.3f;
        [SerializeField] private float heightMultiplier = 3f;

        private Hashtable cubeContainer = new();

        private Vector2Int playerLocation = new();

        private void Start()
        {
            GenerateGrid();
        }

        [ContextMenu("Generate Grid!")]
        private void GenerateGrid()
        {
            playerLocation = playerController.CurrentPosition;

            for (int x = -worldXSize; x < worldXSize; x++)
            {
                for (int z = -worldZSize; z < worldZSize; z++)
                {
                    float y = Mathf.PerlinNoise((x + playerLocation.x) * noiseScale, (z + playerLocation.y) * noiseScale) * heightMultiplier;
                    Vector2Int cubeLocation = new Vector2Int((x + playerLocation.x), (z + playerLocation.y));
                    Vector3 pos = new Vector3(x + playerLocation.x, y, z + playerLocation.y);

                    GameObject newCube = Instantiate(cubePrefab, pos, Quaternion.identity);
                    newCube.transform.SetParent(this.transform, false);
                    cubeContainer.Add(cubeLocation, newCube);
                }
            }
        }

        private void PlayerMoved()
        {
            Debug.Log(playerController.CurrentPosition);

            if (playerLocation == playerController.CurrentPosition)
                return;

            playerLocation = playerController.CurrentPosition;

            for (int x = -worldXSize; x < worldXSize; x++)
            {
                for (int z = -worldZSize; z < worldZSize; z++)
                {
                    float y = Mathf.PerlinNoise((x + playerLocation.x) * noiseScale, (z + playerLocation.y) * noiseScale) * heightMultiplier;
                    Vector2Int cubeLocation = new Vector2Int(x + playerLocation.x, z + playerLocation.y);
                    Vector3 pos = new Vector3(x + playerLocation.x, y, z + playerLocation.y);

                    if (!cubeContainer.ContainsKey(pos))
                    {
                        GameObject newCube = Instantiate(cubePrefab, pos, Quaternion.identity);
                        newCube.transform.SetParent(this.transform, false);
                        cubeContainer.Add(pos, newCube);
                    }
                }
            }
        }

        private void Update()
        {
            PlayerMoved();
        }
    }

    public class Cube
    {
        public float xPos;
        public float zPos;

        public GameObject cubeObject;
    }
}
