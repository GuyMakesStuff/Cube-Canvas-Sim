using Experiments.CubeArt.Managers;
using Experiments.Global.Audio;
using UnityEngine;

namespace Experiments.CubeArt.Interface
{
    public class Menu : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        public virtual void OnMenuOpen() { }
        public virtual void UpdateMenu() { }
        public void CloseMenu()
        {
            AudioManager.Instance.PlaySelectSound();
            MenuManager.Instance.CloseMenu();
        }
        public void QuitGame()
        {
            AudioManager.Instance.PlaySelectSound();
            Application.Quit();
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.ExitPlaymode();
            #endif
        }
    }
}