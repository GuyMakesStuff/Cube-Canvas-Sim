using Experiments.CubeArt.Managers;
using Experiments.Global.Audio;
using UnityEngine;

namespace Experiments.CubeArt.SIM
{
    [RequireComponent(typeof(Rigidbody))]
    public class CanvasPixelPhysics : MonoBehaviour
    {
        Rigidbody Body;
        bool IsActivated;
        public CanvasSettingsManager.PhysicsSettings PS;
        [HideInInspector]
        public Vector3 DetractorPos;

        // Start is called before the first frame update
        void Start()
        {
            Body = GetComponent<Rigidbody>();
            Body.isKinematic = true;
        }

        public void Activate()
        {
            if(!IsActivated)
            {
                IsActivated = true;
                Body.isKinematic = false;
                StatsManager.Instance.TotalObjectsDestroyed++;

                Collider[] NearbyObjects = Physics.OverlapSphere(transform.position, PS.ExplotionRadius);
                foreach (Collider Col in NearbyObjects)
                {
                    CanvasPixelPhysics canvasPixelPhysics = Col.GetComponent<CanvasPixelPhysics>();
                    if(canvasPixelPhysics != null && canvasPixelPhysics != this)
                    {
                        canvasPixelPhysics.DetractorPos = transform.position;
                        canvasPixelPhysics.BeginExplode();
                    }
                }

                int ExplotionIndex = Random.Range(1, 3);
                AudioManager.Instance.InteractWithSFX("Explotion " + ExplotionIndex.ToString("0"), SoundEffectBehaviour.Play);
            }
        }
        public void Detract(Vector3 Position)
        {
            Body.AddExplosionForce(-(PS.ExplotionForce * 100f), Position, PS.ExplotionRadius);
        }
        public void BeginExplode()
        {
            Invoke("Explode", PS.ExplotionDelay);
        }
        void Explode()
        {
            Activate();
            Detract(DetractorPos);
        }
    }
}