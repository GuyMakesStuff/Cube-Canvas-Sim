using Experiments.Global.Audio;
using UnityEngine.UI;
using Experiments.Global.Managers;
using Experiments.CubeArt.Interface;
using UnityEngine;
using TMPro;

namespace Experiments.CubeArt.Managers
{
    public class MenuManager : Manager<MenuManager>
    {
        [System.Serializable]
        public class GameMenu { public string MenuName; [HideInInspector]public int Index; public KeyCode Key; public Menu MenuInstance; }
        [Space]
        public GameMenu[] Menus;
        [HideInInspector]
        public bool InMenu;
        int CurMenuIndex;

        [Space]
        public GameObject LoadingScreen;
        public TMP_Text LoadingText;
        public Slider LoadingBar;

        // Start is called before the first frame update
        void Awake()
        {
            Init(this);
            CurMenuIndex = -1;
            for (int M = 0; M < Menus.Length; M++)
            {
                Menus[M].Index = M;
            }
        }

        // Update is called once per frame
        void Update()
        {
            for (int M = 0; M < Menus.Length; M++)
            {
                if(M == CurMenuIndex) { Menus[M].MenuInstance.UpdateMenu(); }

                if(Input.GetKeyDown(Menus[M].Key) && !InMenu)
                {
                    OpenMenu(Menus[M]);
                    AudioManager.Instance.InteractWithSFX("Open Menu", SoundEffectBehaviour.Play);
                }
            }

            if(CanvasSettingsManager.Instance.LoadSceneOperation != null)
            {
                LoadingScreen.SetActive(!CanvasSettingsManager.Instance.LoadSceneOperation.isDone);
                LoadingText.text = (CanvasSettingsManager.Instance.LoadSceneOperation.progress * 100f).ToString("0") + "%";
                LoadingBar.value = CanvasSettingsManager.Instance.LoadSceneOperation.progress;
            }
        }
        public void OpenMenuByName(string menuName)
        {
            GameMenu gameMenu = System.Array.Find(Menus, GameMenu => GameMenu.MenuName == menuName);
            OpenMenu(gameMenu);
        }
        public void OpenMenu(GameMenu menu)
        {
            menu.MenuInstance.gameObject.SetActive(true);
            menu.MenuInstance.OnMenuOpen();
            CurMenuIndex = menu.Index;
            InMenu = true;
        }
        public void CloseMenu()
        {
            Menus[CurMenuIndex].MenuInstance.gameObject.SetActive(false);
            CurMenuIndex = -1;
            InMenu = false;
        }
    }
}