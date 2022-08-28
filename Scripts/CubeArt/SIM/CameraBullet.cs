using System.Collections;
using Experiments.Global.Audio;
using UnityEngine;

namespace Experiments.CubeArt.SIM
{
    public class CameraBullet : MonoBehaviour
    {
        private void OnCollisionEnter(Collision other) {
            CanvasPixelPhysics canvasPixelPhysics = other.collider.GetComponent<CanvasPixelPhysics>();
            if(canvasPixelPhysics != null)
            {
                AudioManager.Instance.InteractWithSFX("Hit", SoundEffectBehaviour.Play);
                canvasPixelPhysics.Activate();
                Destroy(gameObject);
            }
        }
    }
}