using System.Collections.Generic;
using Experiments.CubeArt.Interface.Widgets;
using Experiments.CubeArt.Managers;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace Experiments.CubeArt.Interface.Menus
{
    public class ImageMenu : Menu
    {
        public PrevNextMenu ImageSelect;
        public RawImage ImagePreview;
        public TMP_Text ImageNameText;

        [Space]
        public ToggleWidget CenterToggle;
        public ToggleWidget BaseToggle;
        public SliderWidget ScaleDivSlider;
        public DropdownWidget EnvironmentDropdown;
        public ToggleWidget PhysicsToggle;

        public override void OnMenuOpen()
        {
            ImageSelect.MinValue = 0;
            ImageSelect.MaxValue = CanvasSettingsManager.Instance.Images.Count - 1;
            ImageSelect.Value = CanvasSettingsManager.Instance.ImageCanvasSettings.ImageIndex;

            CenterToggle.SetValue(CanvasSettingsManager.Instance.ImageCanvasSettings.AlignFromCenter);
            BaseToggle.SetValue(CanvasSettingsManager.Instance.ImageCanvasSettings.CreateBase);
            ScaleDivSlider.SetValue(CanvasSettingsManager.Instance.ImageCanvasSettings.ScaleReducion);
            PhysicsToggle.SetValue(CanvasSettingsManager.Instance.ImageCanvasSettings.UsePhysics);

            List<string> EnvironmentNames = new List<string>();
            for (int E = 0; E < CanvasSettingsManager.Instance.Environments.Length; E++)
            {
                EnvironmentNames.Add(CanvasSettingsManager.Instance.Environments[E].name);
            }
            EnvironmentDropdown.SetOptions(EnvironmentNames);
            EnvironmentDropdown.SetValue(CanvasSettingsManager.Instance.ImageCanvasSettings.EnvironmentIndex);
        }

        public override void UpdateMenu()
        {
            ImagePreview.texture = CanvasSettingsManager.Instance.Images[ImageSelect.Value];
            ImageNameText.text = CanvasSettingsManager.Instance.Images[ImageSelect.Value].name;
        }

        public void Apply()
        {
            CanvasSettingsManager.Instance.ImageCanvasSettings.ImageIndex = ImageSelect.Value;

            CanvasSettingsManager.Instance.ImageCanvasSettings.AlignFromCenter = CenterToggle.GetValue();
            CanvasSettingsManager.Instance.ImageCanvasSettings.CreateBase = BaseToggle.GetValue();
            CanvasSettingsManager.Instance.ImageCanvasSettings.ScaleReducion = Mathf.RoundToInt(ScaleDivSlider.GetValue());
            CanvasSettingsManager.Instance.ImageCanvasSettings.EnvironmentIndex = EnvironmentDropdown.GetValue();
            CanvasSettingsManager.Instance.ImageCanvasSettings.UsePhysics = PhysicsToggle.GetValue();

            CanvasSettingsManager.Instance.ReloadScene();
        }
    }
}