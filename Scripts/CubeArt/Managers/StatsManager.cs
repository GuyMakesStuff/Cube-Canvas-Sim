using Experiments.CubeArt.SIM;
using Experiments.Global.Managers;
using Experiments.Global.Audio;
using UnityEngine;
using TMPro;

namespace Experiments.CubeArt.Managers
{
    public class StatsManager : Manager<StatsManager>
    {
        public TMP_Text StatsText;
        public CanvasCreator canvas;
        int NumFrames = 0;
        float FrameTime = 0.0f;
        float FPS = 0.0f;
        public float FPSUpdateIntervals = 0.5f;
        public KeyCode StatsTextToggleKey;
        [HideInInspector]
        public int TotalObjectsDestroyed;
        public static bool ShowStatsText = true;

        // Start is called before the first frame update
        void Start()
        {
            Init(this);
        }

        // Update is called once per frame
        void Update()
        {
            if(Input.GetKeyDown(StatsTextToggleKey)) { ShowStatsText = !ShowStatsText; AudioManager.Instance.PlaySelectSound(); }
            UpdateTextVisible();

            UpdateFPS();
            StatsText.text = "Approximate FPS:" + FPS.ToString("0.0");
            if(!CanvasSettingsManager.Instance.ErrorEncountered)
            {
                int NumTotalObjects = canvas.ScaledImageSize.x * canvas.ScaledImageSize.y;
                int NumCubesLeft = (NumTotalObjects - canvas.NumDiscarded);

                string DownscaleText = (canvas.imageSettings.ScaleReducion > 1) ? " (Downscaled x" + canvas.imageSettings.ScaleReducion + ") " : " ";
                StatsText.text += "\nCanvas Size" + DownscaleText + "- " + SizeString(canvas.ScaledImageSize.x, canvas.ScaledImageSize.y) + " (" + NumTotalObjects + " Total Cube Objects)";
                if(canvas.NumDiscarded > 0) { StatsText.text += "\n" + canvas.NumDiscarded + " Cubes Discarded From Transparency. (" + NumCubesLeft + " Cubes Left)"; }
                StatsText.text += "\nOriginal Image Size (" + canvas.Image.name + ") - " + SizeString(canvas.Image.width, canvas.Image.height);
                if(canvas.imageSettings.UsePhysics) { StatsText.text += "\n" + TotalObjectsDestroyed + " Out Of " + NumCubesLeft + " Cubes Destroyed"; }
            }
        }
        void UpdateFPS()
        {
            if( FrameTime < FPSUpdateIntervals )
            {
                FrameTime += Time.deltaTime;
                NumFrames++;
            }
            else
            {
                //This code will break if you set your m_refreshTime to 0, which makes no sense.
                FPS = (float)NumFrames/FrameTime;
                NumFrames = 0;
                FrameTime = 0.0f;
            }
        }
        public void UpdateTextVisible()
        {
            StatsText.enabled = (ShowStatsText && !MenuManager.Instance.InMenu);
        }

        public string SizeString(int X, int Y)
        {
            return X + "x" + Y;
        }
    }
}