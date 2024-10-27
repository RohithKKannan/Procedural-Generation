using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TerrainGeneration
{
    public class TerrainGenerator : MonoBehaviour
    {
        [Header("Initialization")]
        [SerializeField] private GameObject cubePrefab;
        [SerializeField] private int worldXSize;
        [SerializeField] private int worldZSize;
        [SerializeField] private float gridOffset;

        [Header("Variables")]
        [Range(0.1f, 20f)]
        [SerializeField] private float detailScale = 8f;

        private List<Cube> cubesList = new();

        private void Start()
        {
            // GenerateGrid();
            // UpdateCubePositions();
        }

        private void OnValidate()
        {
            UpdateCubePositions();
        }

        [ContextMenu("Generate Grid!")]
        private void GenerateGrid()
        {
            for (int x = 0; x < worldXSize; x++)
            {
                for (int z = 0; z < worldZSize; z++)
                {
                    Vector3 pos = new Vector3(x * gridOffset, 0, z * gridOffset);

                    Cube cube = new Cube();
                    cube.cubeObject = Instantiate(cubePrefab);
                    cube.cubeObject.transform.SetParent(this.transform);
                    cube.cubeObject.transform.localPosition = pos;
                    cube.xPos = x;
                    cube.zPos = z;
                    cubesList.Add(cube);
                }
            }

            UpdateCubePositions();
        }

        public void UpdateCubePositions()
        {
            foreach (Cube cube in cubesList)
            {
                cube.UpdatePosition(detailScale, transform.position);
            }
        }
    }

    public class Cube
    {
        public float xPos;
        public float zPos;

        public GameObject cubeObject;

        public void UpdatePosition(float detailScale, Vector3 generatorPos)
        {
            float xNoise = (xPos + generatorPos.x) / detailScale;
            float zNoise = (zPos + generatorPos.z) / detailScale;

            float y = Mathf.PerlinNoise(xNoise, zNoise);

            cubeObject.transform.position = new Vector3(cubeObject.transform.position.x, y, cubeObject.transform.position.z);
        }
    }
}
