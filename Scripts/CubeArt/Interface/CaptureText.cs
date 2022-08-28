using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Experiments.CubeArt.Interface
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class CaptureText : MonoBehaviour
    {
        public float AppearTime;
        public float DisappearTime;
        float T;
        TMP_Text text;

        // Start is called before the first frame update
        void Start()
        {
            text = GetComponent<TextMeshProUGUI>();
            T = 0f;
        }

        // Update is called once per frame
        void Update()
        {
            if(T > 0f) { T -= Time.deltaTime; }
            float Alpha = Mathf.Clamp(T, 0f, DisappearTime) / DisappearTime;
            text.alpha = Alpha;
        }

        public void Appear()
        {
            T = AppearTime;
        }
        public void SetText(string NewText)
        {
            text.text = NewText;
        }
    }
}