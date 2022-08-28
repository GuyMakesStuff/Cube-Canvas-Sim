using TMPro;
using Experiments.CubeArt.Managers;

namespace Experiments.CubeArt.Interface.Menus
{
    public class ErrorMenu : Menu
    {
        public TMP_Text ErrorText;

        public override void OnMenuOpen()
        {
            ErrorText.text = CanvasSettingsManager.Instance.ErrorMessage;
        }
    }
}