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
        [SerializeField] private Chunk ChunkPrefab;

        [Header("Variables")]
        [SerializeField] private float noiseScale = 0.3f;
        [SerializeField] private float heightMultiplier = 0.3f;

        private Vector3 currentPlayerPosition;
        private Vector3 prevPlayerPosition = Vector3.positiveInfinity;

        private Stack<Chunk> ChunkPool = new();
        private Hashtable activeChunks = new();

        private List<Vector3> activePositions = new();
        private List<Vector3> currentPositions = new();

        private void Awake()
        {
            for (int i = 0; i < 5; i++)
            {
                Chunk Chunk = GameObject.Instantiate<Chunk>(ChunkPrefab);
                Chunk.transform.SetParent(this.transform, true);
                Chunk.Init(noiseScale, heightMultiplier);
                ChunkPool.Push(Chunk);
            }
        }

        #region Chunk pool

        private Chunk GetChunk()
        {
            if (ChunkPool.Count < 1)
            {
                Chunk Chunk = GameObject.Instantiate<Chunk>(ChunkPrefab);
                Chunk.transform.SetParent(this.transform, true);
                Chunk.Init(noiseScale, heightMultiplier);
                return Chunk;
            }

            return ChunkPool.Pop();
        }

        private void ReturnChunk(Chunk _Chunk)
        {
            _Chunk.ResetChunk();

            ChunkPool.Push(_Chunk);
        }

        #endregion

        #region Terrain Update

        private void CheckForTerrainUpdate()
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
            currentPositions.Clear();

            for (int x = -Constants.renderDistance; x <= Constants.renderDistance; x++)
            {
                for (int z = -Constants.renderDistance; z <= Constants.renderDistance; z++)
                {
                    Vector3 chunkPosition = currentPlayerPosition + new Vector3(x * Constants.chunkSize, 0, z * Constants.chunkSize);

                    currentPositions.Add(chunkPosition);

                    if (activeChunks.Contains(chunkPosition))
                        continue;

                    Chunk Chunk = GetChunk();
                    Chunk.RepositionChunk(chunkPosition);

                    activeChunks.Add(chunkPosition, Chunk);
                }
            }

            foreach (Vector3 pos in activePositions)
            {
                if (!currentPositions.Contains(pos))
                {
                    ReturnChunk((Chunk)activeChunks[pos]);
                    activeChunks.Remove(pos);
                }
            }

            activePositions.Clear();
            activePositions.AddRange(currentPositions);
        }

        #endregion

        private void Update()
        {
            CheckForTerrainUpdate();
        }
    }
}
