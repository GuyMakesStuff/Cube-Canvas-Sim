using System.Collections.Generic;
using Experiments.Global.Audio;
using UnityEngine;
using TMPro;

namespace Experiments.CubeArt.Interface.Widgets
{
    public class DropdownWidget : MonoBehaviour
    {
        public string SettingName;
        public List<string> Options;
        int PrevValue;

        [Space]
        public TMP_Dropdown DropdownComponent;
        public TMP_Text SettingLabel;

        private void Awake()
        {
            SetOptions(Options);
        }
        private void Update()
        {
            SettingLabel.text = SettingName + ":";
            if(DropdownComponent.value != PrevValue) { AudioManager.Instance.PlaySelectSound(); PrevValue = DropdownComponent.value; }
        }

        public void SetValue(int NewValue)
        {
            int ClampedValue = Mathf.Clamp(NewValue, 0, Options.Count - 1);
            DropdownComponent.value = ClampedValue;
            DropdownComponent.RefreshShownValue();
            PrevValue = ClampedValue;
        }
        public int GetValue() { return DropdownComponent.value; }
        public void SetOptions(List<string> NewOptions)
        {
            Options = NewOptions;
            DropdownComponent.ClearOptions();
            DropdownComponent.AddOptions(Options);
        }
    }
}