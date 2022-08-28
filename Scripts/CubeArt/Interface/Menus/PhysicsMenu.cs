using System.Collections;
using Experiments.CubeArt.Managers;
using Experiments.CubeArt.Interface.Widgets;

namespace Experiments.CubeArt.Interface.Menus
{
    public class PhysicsMenu : Menu
    {
        public SliderWidget RadiusSlider;
        public SliderWidget ForceSlider;
        public SliderWidget DelaySlider;

        public override void OnMenuOpen()
        {
            base.OnMenuOpen();

            RadiusSlider.SetValue(CanvasSettingsManager.Instance.ImagePhysicsSettings.ExplotionRadius);
            ForceSlider.SetValue(CanvasSettingsManager.Instance.ImagePhysicsSettings.ExplotionForce);
            DelaySlider.SetValue(CanvasSettingsManager.Instance.ImagePhysicsSettings.ExplotionDelay);
        }

        public override void UpdateMenu()
        {
            base.UpdateMenu();
            
            CanvasSettingsManager.Instance.ImagePhysicsSettings.ExplotionRadius = RadiusSlider.GetValue();
            CanvasSettingsManager.Instance.ImagePhysicsSettings.ExplotionForce = ForceSlider.GetValue();
            CanvasSettingsManager.Instance.ImagePhysicsSettings.ExplotionDelay = DelaySlider.GetValue();
        }
    }
}