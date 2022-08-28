using System.IO;
using Experiments.Global.IO;
using Experiments.Global.Audio;
using System.Collections.Generic;
using Experiments.Global.Managers;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace Experiments.CubeArt.Managers
{
    public class CanvasSettingsManager : Manager<CanvasSettingsManager>
    {
        [System.Serializable]
        public class ImageSettings
        {
            public int ImageIndex;
            [Range(1, 10)]
            public int ScaleReducion;
            public bool AlignFromCenter;
            public bool CreateBase;
            public bool UsePhysics;
            public int EnvironmentIndex;
        }
        [System.Serializable]
        public class PhysicsSettings
        {
            public float ExplotionRadius;
            public float ExplotionForce;
            public float ExplotionDelay;
        }
        [System.Serializable]
        public class CanvasSettings : SaveFile
        {
            [Space]
            public ImageSettings CanvasImage;
            public PhysicsSettings CanvasPhysics;
        }
        [Space]
        public CanvasSettings canvasSettings;
        [HideInInspector]
        public ImageSettings ImageCanvasSettings;
        [HideInInspector]
        public PhysicsSettings ImagePhysicsSettings;
        [HideInInspector]
        public List<Texture2D> Images;
        public Material[] Environments;
        public KeyCode ResetKey;
        [HideInInspector]
        public AsyncOperation LoadSceneOperation;
        [HideInInspector]
        public string ErrorMessage;
        [HideInInspector]
        public bool ErrorEncountered;

        // Start is called before the first frame update
        void Awake()
        {
            Init(this);

            CanvasSettings LoadedSettings = Saver.Load(canvasSettings) as CanvasSettings;
            if(LoadedSettings != null) { canvasSettings = LoadedSettings; }
            ImageCanvasSettings = canvasSettings.CanvasImage;
            ImagePhysicsSettings = canvasSettings.CanvasPhysics;
            if(LoadedSettings == null) { canvasSettings.Save(); }

            if(!Directory.Exists(Application.dataPath + "/Images"))
            {
                RaiseError("Images Directory Not Found! ([Installed Directory]/PixelCube SIM_Data/Images)");
                return;
            }
            string[] ImageFilePaths = Directory.GetFiles(Application.dataPath + "/Images", "*.png");
            if(ImageFilePaths.Length == 0)
            {
                RaiseError("No Images Found In Images Directory!\nPlease Put Your Images In [Installed Directory]/PixelCube SIM_Data/Images,\nAnd/Or Check That They Are In PNG Format Then Try Again!");
                return;
            }
            for (int I = 0; I < ImageFilePaths.Length; I++)
            {
                byte[] ImageData = File.ReadAllBytes(ImageFilePaths[I]);
                string ImageName = Path.GetFileNameWithoutExtension(ImageFilePaths[I]);
                Texture2D LoadedImage = new Texture2D(1, 1);
                ImageConversion.LoadImage(LoadedImage, ImageData, false);
                LoadedImage.name = ImageName;
                Images.Add(LoadedImage);
            }
            ImageCanvasSettings.ImageIndex = Mathf.Clamp(ImageCanvasSettings.ImageIndex, 0, Images.Count - 1);
        }

        // Update is called once per frame
        void Update()
        {
            if(Input.GetKeyDown(ResetKey))
            {
                ReloadScene();
            }

            canvasSettings.CanvasImage = ImageCanvasSettings;
            canvasSettings.CanvasPhysics = ImagePhysicsSettings;
            canvasSettings.Save();
        }

        public void ReloadScene()
        {
            AudioManager.Instance.InteractWithSFX("Reload", SoundEffectBehaviour.Play);
            LoadSceneOperation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        }

        void RaiseError(string Message)
        {
            ErrorEncountered = true;
            ErrorMessage = Message;
        }
    }
}