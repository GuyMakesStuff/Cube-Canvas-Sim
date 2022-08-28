using System;
using Experiments.Global.IO;
using Experiments.Global.Audio;
using System.Collections.Generic;
using Experiments.Global.Managers;
using UnityEngine.SceneManagement;
using Experiments.CubeArt.SIM;
using UnityEngine;

namespace Experiments.CubeArt.Managers
{
    public class SettingsManager : Manager<SettingsManager>
    {
        [System.Serializable]
        public class SliderSetting
        {
            public string SettingName;
            public float MinValue;
            public float MaxValue;
            public float Value;
        }
        [System.Serializable]
        public class DropdownSetting
        {
            public string SettingName;
            public List<string> Options;
            public int Value;
        }
        [System.Serializable]
        public class ToggleSetting
        {
            public string SettingName;
            public bool Value;
        }
        [System.Serializable]
        public class Settings : SaveFile
        {
            [Space]
            public SliderSetting[] SliderSettings;
            public DropdownSetting[] DropdownSettings;
            public ToggleSetting[] ToggleSettings;
        }
        [Space]
        public Settings settings;
        Resolution[] AvailableResolutions;
        int PrevResIndex;
        CameraNavigate CamNav;

        // Start is called before the first frame update
        void Start()
        {
            Init(this);

            Settings LoadedSettings = Saver.Load(settings) as Settings;
            if(LoadedSettings != null) { settings = LoadedSettings; }

            AvailableResolutions = Screen.resolutions;
            List<string> Res2String = new List<string>();
            Resolution CurRes = Screen.currentResolution;
            int CurResIndex = 0;
            for (int R = 0; R < AvailableResolutions.Length; R++)
            {
                Resolution Res = AvailableResolutions[R];
                Res2String.Add(Res.ToString());
                if(CurRes.width == Res.width && CurRes.height == Res.height && CurRes.refreshRate == Res.refreshRate)
                {
                    CurResIndex = R;
                }
            }
            DropdownSetting ResSetting = GetDropdownSetting("Resolution");
            ResSetting.Options = Res2String;
            if(LoadedSettings == null) { ResSetting.Value = CurResIndex; }
            else { ResSetting.Value = Mathf.Clamp(ResSetting.Value, 0, Res2String.Count - 1); }
            PrevResIndex = -1;
            

            RelocateCamera();
            SceneManager.sceneLoaded += new UnityEngine.Events.UnityAction<Scene, LoadSceneMode>(delegate { RelocateCamera(); });

            ApplySettings();
        }

        // Update is called once per frame
        void Update()
        {
            ApplySettings();
        }

        void ApplySettings()
        {
            // Sound Settings
            AudioManager.Instance.Mute = !GetToggleSetting("Enable Sound").Value;
            // Graphical Settings
            QualitySettings.SetQualityLevel(GetDropdownSetting("Quality").Value);
            Screen.fullScreen = GetToggleSetting("Fullscreen").Value;
            int ResIndex = GetDropdownSetting("Resolution").Value;
            if(ResIndex != PrevResIndex)
            {
                PrevResIndex = ResIndex;
                Resolution Res = AvailableResolutions[ResIndex];
                Screen.SetResolution(Res.width, Res.height, Screen.fullScreen, Res.refreshRate);
            }
            // Input Settings
            CamNav.Sens = GetSliderSetting("Mouse Sensitivity").Value;
            CamNav.BaseSpeed = GetSliderSetting("Movement Speed").Value;
            CamNav.InputReciever.LockMouse = GetToggleSetting("Lock Mouse").Value;
            CamNav.InputReciever.InvertMouse = GetToggleSetting("Invert Mouse").Value;
            CamNav.InputReciever.Smooth = GetToggleSetting("Smooth Inputs").Value;

            settings.Save();
        }

        public SliderSetting GetSliderSetting(string Name)
        {
            return Array.Find(settings.SliderSettings, SliderSetting => SliderSetting.SettingName == Name);
        }
        public DropdownSetting GetDropdownSetting(string Name)
        {
            return Array.Find(settings.DropdownSettings, DropdownSetting => DropdownSetting.SettingName == Name);
        }
        public ToggleSetting GetToggleSetting(string Name)
        {
            return Array.Find(settings.ToggleSettings, ToggleSetting => ToggleSetting.SettingName == Name);
        }

        public void RelocateCamera()
        {
            CamNav = FindObjectOfType<CameraNavigate>();
        }
    }
}