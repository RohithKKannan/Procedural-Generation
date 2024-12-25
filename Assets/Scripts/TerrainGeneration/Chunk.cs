using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TerrainGeneration
{
    public class Chunk : MonoBehaviour
    {
        [SerializeField] private GameObject cubePrefab;

        private float noiseScale = 0.3f;
        private float heightMultiplier = 0.3f;

        private List<Transform> cubes = new();

        private void Awake()
        {
            GenerateChunk();
            ResetChunk();
        }

        public void Init(float _noiseScale, float _heightMultiplier)
        {
            noiseScale = _noiseScale;
            heightMultiplier = _heightMultiplier;
        }

        public void ResetChunk()
        {
            this.gameObject.SetActive(false);
        }

        private void GenerateChunk()
        {
            for (int x = -Constants.chunkSize / 2; x < Constants.chunkSize / 2; x++)
            {
                for (int z = -Constants.chunkSize / 2; z < Constants.chunkSize / 2; z++)
                {
                    Vector3 pos = new Vector3(x + this.transform.position.x, 0, z + this.transform.position.z);
                    GameObject cube = Instantiate(cubePrefab, pos, Quaternion.identity, this.transform);
                    cubes.Add(cube.transform);
                }
            }
        }

        public void RepositionChunk(Vector3 newChunkPosition)
        {
            transform.position = newChunkPosition;

            SetChunkTerrain();
        }

        public void SetChunkTerrain()
        {
            foreach (Transform cube in cubes)
            {
                UpdatePosition(cube);
            }

            this.gameObject.SetActive(true);
        }

        public void UpdatePosition(Transform cube)
        {
            float y = Mathf.PerlinNoise(cube.position.x * noiseScale, cube.localPosition.z * noiseScale) * heightMultiplier;

            cube.position = new Vector3(cube.position.x, y, cube.position.z);
        }
    }
}