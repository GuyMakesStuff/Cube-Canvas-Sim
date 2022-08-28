using System.IO;
using Experiments.Global.Managers;
using Experiments.Global.Audio;
using Experiments.CubeArt.Managers;
using Experiments.CubeArt.Interface;
using System.Collections;
using UnityEngine;

namespace Experiments.CubeArt.SIM
{
    public class CanvasCreator : MonoBehaviour
    {
        [HideInInspector]
        public Texture2D Image;
        public Transform CanvasContainer;
        public Vector2 ExtraBaseSize;
        [Range(0f, 1f)]
        public float MinAlphaThreshold;
        [HideInInspector]
        public CanvasSettingsManager.ImageSettings imageSettings;
        [HideInInspector]
        public CanvasSettingsManager.PhysicsSettings physicsSettings;
        [HideInInspector]
        public Vector2Int ScaledImageSize;
        [HideInInspector]
        public int NumDiscarded;

        [Header("Screenshot Capturing")]
        public CaptureText captureText;
        public KeyCode ScreenshotCaptureKey;

        // Start is called before the first frame update
        void Start()
        {
            imageSettings = CanvasSettingsManager.Instance.ImageCanvasSettings;
            physicsSettings = CanvasSettingsManager.Instance.ImagePhysicsSettings;

            RenderSettings.skybox = CanvasSettingsManager.Instance.Environments[imageSettings.EnvironmentIndex];
            if(CanvasSettingsManager.Instance.ErrorEncountered)
            {
                MenuManager.Instance.OpenMenuByName("Error");
                return;
            }
            Image = CanvasSettingsManager.Instance.Images[imageSettings.ImageIndex];
            if(imageSettings.CreateBase) { CreateCanvasBase(); }
            CreateCanvas();
        }
        void CreateCanvasBase()
        {
            Vector3 ImageSize = new Vector3(Image.width / imageSettings.ScaleReducion, Image.height / imageSettings.ScaleReducion, 0f);
            Vector3 ImageOffset = (imageSettings.AlignFromCenter) ? (ImageSize / 2f) : Vector3.zero;
            GameObject Base = GameObject.CreatePrimitive(PrimitiveType.Cube);
            Base.name = "Canvas Base";
            Base.transform.SetParent(CanvasContainer);
            Base.transform.localPosition = ((Vector3.right * (ImageSize.x / 2f)) - ImageOffset) + Vector3.down;
            Base.transform.localScale = new Vector3(ImageSize.x + ExtraBaseSize.x, 1f, 1f + ExtraBaseSize.y);
        }
        void CreateCanvas()
        {
            Vector3 ImageSize = new Vector3(Image.width / imageSettings.ScaleReducion, Image.height / imageSettings.ScaleReducion, 0f);
            for (int Y = 0; Y < Image.height; Y += imageSettings.ScaleReducion)
            {
                for (int X = 0; X < Image.width; X += imageSettings.ScaleReducion)
                {
                    if(Y == 0) { ScaledImageSize.x++; }

                    Color Col = Image.GetPixel(X, Y);
                    if(Col.a < MinAlphaThreshold) { NumDiscarded++; continue; }

                    Vector3 ImagePos = new Vector3(X / imageSettings.ScaleReducion, Y / imageSettings.ScaleReducion, 0f);
                    Vector3 ImageOffset = (imageSettings.AlignFromCenter) ? (ImageSize / 2f) : Vector3.zero;
                    GameObject NewCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    NewCube.name = "Canvas Pixel_" + ImagePos.ToString();
                    NewCube.transform.SetParent(CanvasContainer);
                    NewCube.transform.localPosition = (ImagePos - ImageOffset);
                    ColorManager.Instance.ColorObject(NewCube, Col);
                    if(imageSettings.UsePhysics) { NewCube.AddComponent<CanvasPixelPhysics>().PS = physicsSettings; }
                }
                ScaledImageSize.y++;
            }
        }

        // Update is called once per frame
        void Update()
        {
            if(Input.GetKeyDown(ScreenshotCaptureKey) && !MenuManager.Instance.InMenu)
            {
                StartCoroutine(CaptureScreenshot());
            }
        }
        IEnumerator CaptureScreenshot()
        {
            bool PrevShowStats = StatsManager.ShowStatsText;
            StatsManager.ShowStatsText = false;
            StatsManager.Instance.UpdateTextVisible();
            Camera.main.Render();

            yield return new WaitForEndOfFrame();

            string ScreenshotName = Image.name + "_" + CanvasSettingsManager.Instance.Environments[imageSettings.EnvironmentIndex].name + "_" + StatsManager.Instance.SizeString(ScaledImageSize.x, ScaledImageSize.y);
            string ScreenshotPath = Application.dataPath + "/Screenshots";

            if(!Directory.Exists(ScreenshotPath)) { Directory.CreateDirectory(ScreenshotPath); };
            ScreenCapture.CaptureScreenshot(ScreenshotPath + "/" + ScreenshotName + ".png");
            StatsManager.ShowStatsText = PrevShowStats;

            AudioManager.Instance.InteractWithSFX("Capture Screenshot", SoundEffectBehaviour.Play);
            captureText.SetText("Screenshot Captured! ([Installed Directory]/PixelCube SIM_Data/Screenshots/" + ScreenshotName + ".png)");
            captureText.Appear();
        }
    }
}