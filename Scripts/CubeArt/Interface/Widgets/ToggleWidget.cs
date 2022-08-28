using Experiments.Global.Audio;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace Experiments.CubeArt.Interface.Widgets
{
    public class ToggleWidget : MonoBehaviour
    {
        public string SettingName;
        bool PrevValue;

        [Space]
        public Toggle ToggleComponent;
        public TMP_Text SettingLabel;

        private void Awake()
        {

        }
        private void Update()
        {
            SettingLabel.text = SettingName;
            if(ToggleComponent.isOn != PrevValue) { AudioManager.Instance.PlaySelectSound(); PrevValue = ToggleComponent.isOn; }
        }

        public void SetValue(bool NewValue) { ToggleComponent.isOn = NewValue; PrevValue = NewValue; }
        public bool GetValue() { return ToggleComponent.isOn; }
    }
}