using Experiments.CubeArt.Managers;
using Experiments.CubeArt.Interface.Widgets;
using UnityEngine;

namespace Experiments.CubeArt.Interface.Menus
{
    public class SettingsMenu : Menu
    {
        [System.Serializable]
        public class SettingProperty
        {
            public string PropertyName;
            public enum SettingTypes { Slider, Dropdown, Toggle }
            public SettingTypes SettingType;
            GameObject OBJ;

            public void Construct(SettingsMenu menu, float YPos)
            {
                OBJ = Instantiate(menu.RefrenceSettings[(int)SettingType], Vector2.zero, Quaternion.identity, menu.SettingsContainer);
                OBJ.GetComponent<RectTransform>().anchoredPosition = Vector2.down * YPos;
                OBJ.SetActive(true);

                AssignValue();
            }

            public void AssignValue()
            {
                switch (SettingType)
                {
                    case SettingTypes.Slider:
                    {
                        SliderWidget slider = OBJ.GetComponent<SliderWidget>();
                        SettingsManager.SliderSetting setting = SettingsManager.Instance.GetSliderSetting(PropertyName);
                        slider.MinValue = setting.MinValue;
                        slider.MaxValue = setting.MaxValue;
                        slider.ReassignMinMax();
                        slider.SetValue(setting.Value);
                        slider.SettingName = PropertyName;
                        break;
                    }
                    case SettingTypes.Dropdown:
                    {
                        DropdownWidget dropdown = OBJ.GetComponent<DropdownWidget>();
                        SettingsManager.DropdownSetting setting = SettingsManager.Instance.GetDropdownSetting(PropertyName);
                        dropdown.SetOptions(setting.Options);
                        dropdown.SetValue(setting.Value);
                        dropdown.SettingName = PropertyName;
                        break;
                    }
                    case SettingTypes.Toggle:
                    {
                        ToggleWidget toggle = OBJ.GetComponent<ToggleWidget>();
                        SettingsManager.ToggleSetting setting = SettingsManager.Instance.GetToggleSetting(PropertyName);
                        toggle.SetValue(setting.Value);
                        toggle.SettingName = PropertyName;
                        break;
                    }
                }
            }
            public void UpdateSetting()
            {
                switch (SettingType)
                {
                    case SettingTypes.Slider:
                    {
                        SliderWidget slider = OBJ.GetComponent<SliderWidget>();
                        SettingsManager.SliderSetting setting = SettingsManager.Instance.GetSliderSetting(PropertyName);
                        setting.Value = slider.GetValue();
                        break;
                    }
                    case SettingTypes.Dropdown:
                    {
                        DropdownWidget dropdown = OBJ.GetComponent<DropdownWidget>();
                        SettingsManager.DropdownSetting setting = SettingsManager.Instance.GetDropdownSetting(PropertyName);
                        setting.Value = dropdown.GetValue();
                        break;
                    }
                    case SettingTypes.Toggle:
                    {
                        ToggleWidget toggle = OBJ.GetComponent<ToggleWidget>();
                        SettingsManager.ToggleSetting setting = SettingsManager.Instance.GetToggleSetting(PropertyName);
                        setting.Value = toggle.GetValue();
                        break;
                    }
                }
            }
        }
        public SettingProperty[] Settings;
        public RectTransform SettingsContainer;
        public GameObject[] RefrenceSettings;
        public float StartSettingYPos;
        public float SettingsGaps;
        bool CreatedMenu;

        public override void OnMenuOpen()
        {
            if(!CreatedMenu)
            {
                float SettingYPos = StartSettingYPos;
                for (int S = 0; S < Settings.Length; S++)
                {
                    SettingYPos += SettingsGaps;
                    Settings[S].Construct(this, SettingYPos);
                }
                float SettingsContainerHeight = (SettingsGaps * Settings.Length) + (SettingsGaps / 2f);
                SettingsContainer.sizeDelta = Vector3.up * SettingsContainerHeight;
                SettingsContainer.anchoredPosition = Vector3.down * (SettingsContainerHeight / 2f);

                CreatedMenu = true;
            }
            else
            {
                foreach (SettingProperty SP in Settings)
                {
                    SP.AssignValue();
                }
            }
        }

        public override void UpdateMenu()
        {
            foreach (SettingProperty SP in Settings)
            {
                SP.UpdateSetting();
            }
        }
    }
}