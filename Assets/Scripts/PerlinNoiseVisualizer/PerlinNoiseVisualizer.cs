using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PerlinNoiseVisualizer
{
    public class PerlinNoiseVisualizer : MonoBehaviour
    {
        [SerializeField] private PerlinNoiseUI perlinNoiseUI;

        [Header("Initialization")]
        [SerializeField] private GameObject cubePrefab;
        [SerializeField] private int worldXSize;
        [SerializeField] private int worldZSize;
        [SerializeField] private float gridOffset;

        [Header("Variables")]
        [Range(0f, 10f)]
        [SerializeField] private float heightMultiplier = 5f;
        [Range(0f, 1f)]
        [SerializeField] private float noiseScale = 0.3f;

        private List<Cube> cubesList = new();

        public UnityAction<float> OnUpdateNoiseScale;
        public UnityAction<float> OnUpdateHeightMultiplier;

        private void Awake()
        {
            OnUpdateNoiseScale += UpdateNoiseScale;
            OnUpdateHeightMultiplier += UpdateHeightMultiplier;
        }

        private void OnDestroy()
        {
            OnUpdateNoiseScale -= UpdateNoiseScale;
            OnUpdateHeightMultiplier -= UpdateHeightMultiplier;
        }

        private void Start()
        {
            GenerateGrid();
            UpdateCubePositions();

            UpdateSliderValues();
            UpdateVariableUILabels();
        }

        private void OnValidate()
        {
            UpdateCubePositions();
            UpdateVariableUILabels();
        }

        private void UpdateVariableUILabels()
        {
            perlinNoiseUI.OnUpdateNoiseScaleLabel?.Invoke(noiseScale);
            perlinNoiseUI.OnUpdateHeightMultiplierLabel?.Invoke(heightMultiplier);
        }

        private void UpdateSliderValues()
        {
            perlinNoiseUI.OnUpdateSliderValues?.Invoke(noiseScale, heightMultiplier);
        }

        private void GenerateGrid()
        {
            for (int x = 0; x < worldXSize; x++)
            {
                for (int z = 0; z < worldZSize; z++)
                {
                    Vector3 pos = new Vector3(x + gridOffset, 0, z + gridOffset);
                    Cube cube = new Cube();
                    cube.cubeObject = Instantiate(cubePrefab, pos, Quaternion.identity, this.transform);
                    cube.xPos = x;
                    cube.zPos = z;
                    cubesList.Add(cube);
                }
            }
        }

        private void UpdateNoiseScale(float _noiseScale)
        {
            noiseScale = _noiseScale;
            UpdateCubePositions();
            UpdateVariableUILabels();
        }

        private void UpdateHeightMultiplier(float _heightMultiplier)
        {
            heightMultiplier = _heightMultiplier;
            UpdateCubePositions();
            UpdateVariableUILabels();
        }

        public void UpdateCubePositions()
        {
            foreach (Cube cube in cubesList)
            {
                cube.UpdatePosition(noiseScale, heightMultiplier);
            }
        }
    }

    [Serializable]
    public class Cube
    {
        public int xPos;
        public int zPos;

        public GameObject cubeObject;

        public void UpdatePosition(float noiseScale, float heightMultiplier)
        {
            float y = Mathf.PerlinNoise(xPos * noiseScale, zPos * noiseScale) * heightMultiplier;

            cubeObject.transform.position = new Vector3(xPos, y, zPos);
        }
    }
}
