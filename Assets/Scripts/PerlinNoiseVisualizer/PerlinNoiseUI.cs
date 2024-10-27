using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace PerlinNoiseVisualizer
{
    public class PerlinNoiseUI : MonoBehaviour
    {
        [SerializeField] private PerlinNoiseVisualizer visualizer;

        [Header("Noise Scale")]
        [SerializeField] private TMP_Text noiseScaleLabel;
        [SerializeField] private Slider noiseScaleSlider;

        [Header("Height Multiplier")]
        [SerializeField] private TMP_Text heightMultiplierLabel;
        [SerializeField] private Slider heightMultiplierSlider;

        public UnityAction<float> OnUpdateNoiseScaleLabel;
        public UnityAction<float> OnUpdateHeightMultiplierLabel;

        public UnityAction<float, float> OnUpdateSliderValues;

        private void Awake()
        {
            OnUpdateNoiseScaleLabel += UpdateNoiseScaleLabel;
            OnUpdateHeightMultiplierLabel += UpdateHeightMultiplierLabel;

            OnUpdateSliderValues += UpdateSliderValues;
        }

        private void Start()
        {
            noiseScaleSlider.onValueChanged.AddListener(visualizer.OnUpdateNoiseScale);
            heightMultiplierSlider.onValueChanged.AddListener(visualizer.OnUpdateHeightMultiplier);
        }

        private void OnDestroy()
        {
            noiseScaleSlider.onValueChanged.RemoveAllListeners();
            heightMultiplierSlider.onValueChanged.RemoveAllListeners();

            OnUpdateNoiseScaleLabel -= UpdateNoiseScaleLabel;
            OnUpdateHeightMultiplierLabel -= UpdateHeightMultiplierLabel;

            OnUpdateSliderValues -= UpdateSliderValues;
        }

        private void UpdateNoiseScaleLabel(float _noiseScale)
        {
            noiseScaleLabel.text = _noiseScale.ToString("F2");
        }

        private void UpdateHeightMultiplierLabel(float _heightMultiplier)
        {
            heightMultiplierLabel.text = _heightMultiplier.ToString("F2");
        }

        private void UpdateSliderValues(float _noiseScale, float _heightMultiplier)
        {
            noiseScaleSlider.value = _noiseScale;
            heightMultiplierSlider.value = _heightMultiplier;
        }
    }
}
