using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TerrainGeneration
{
    public class TerrainGenerator : MonoBehaviour
    {
        [SerializeField] private Transform playerTransform;

        [Header("Initialization")]
        [SerializeField] private ChunkGenerator chunkGeneratorPrefab;

        [Header("Variables")]
        [SerializeField] private float noiseScale = 0.3f;
        [SerializeField] private float heightMultiplier = 0.3f;

        private Vector3 currentPlayerPosition;
        private Vector3 prevPlayerPosition = Vector3.positiveInfinity;

        private Stack<ChunkGenerator> chunkGeneratorPool = new();
        private List<ChunkGenerator> activeChunkGenerators = new();

        private void Awake()
        {
            for (int i = 0; i < 5; i++)
            {
                ChunkGenerator chunkGenerator = GameObject.Instantiate<ChunkGenerator>(chunkGeneratorPrefab);
                chunkGenerator.transform.SetParent(this.transform, true);
                chunkGenerator.Init(noiseScale, heightMultiplier);
                chunkGeneratorPool.Push(chunkGenerator);
            }
        }

        private ChunkGenerator GetChunkGenerator()
        {
            if (chunkGeneratorPool.Count < 1)
            {
                ChunkGenerator chunkGenerator = GameObject.Instantiate<ChunkGenerator>(chunkGeneratorPrefab);
                chunkGenerator.transform.SetParent(this.transform, true);
                chunkGenerator.Init(noiseScale, heightMultiplier);
                return chunkGenerator;
            }

            return chunkGeneratorPool.Pop();
        }

        private void ReturnChunkGenerator(ChunkGenerator _chunkGenerator)
        {
            _chunkGenerator.ResetChunk();

            chunkGeneratorPool.Push(_chunkGenerator);
        }

        private void Update()
        {
            currentPlayerPosition = RoundToNearestChunkPosition(playerTransform.position, Constants.chunkSize);

            if (prevPlayerPosition == currentPlayerPosition)
                return;

            PlaceChunksAroundPlayer();

            prevPlayerPosition = currentPlayerPosition;
        }

        private Vector3 RoundToNearestChunkPosition(Vector3 position, float multiple)
        {
            return new Vector3(
            Mathf.Round(position.x / multiple) * multiple,
            0,
            Mathf.Round(position.z / multiple) * multiple
            );
        }

        private void PlaceChunksAroundPlayer()
        {
            if (activeChunkGenerators.Count > 0)
            {
                foreach (ChunkGenerator chunkGen in activeChunkGenerators)
                {
                    ReturnChunkGenerator(chunkGen);
                }

                activeChunkGenerators.Clear();
            }

            ChunkGenerator chunkGenerator = GetChunkGenerator();

            chunkGenerator.RepositionChunk(currentPlayerPosition);

            activeChunkGenerators.Add(chunkGenerator);
        }
    }
}
