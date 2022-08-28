using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace Experiments.CubeArt.Interface.Widgets
{
    public class SliderWidget : MonoBehaviour
    {
        public string SettingName;
        public float MinValue;
        public float MaxValue;

        [Space]
        public Slider SliderComponent;
        public TMP_Text SettingLabel;
        public TMP_Text ValueText;

        private void Awake()
        {
            ReassignMinMax();
        }
        private void Update()
        {
            ReassignMinMax();
            SettingLabel.text = SettingName + ":";
            ValueText.text = GetValue().ToString("0.00");
        }

        public void ReassignMinMax()
        {
            SliderComponent.minValue = MinValue;
            SliderComponent.maxValue = MaxValue;
        }
        public void SetValue(float NewValue) { SliderComponent.value = NewValue; }
        public float GetValue() { return SliderComponent.value; }
    }
}