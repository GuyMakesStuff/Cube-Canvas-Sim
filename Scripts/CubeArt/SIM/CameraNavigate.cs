using Experiments.Global.Audio;
using Experiments.CubeArt.Managers;
using EasyPlayerController;
using UnityEngine;

namespace Experiments.CubeArt.SIM
{
    public class CameraNavigate : MonoBehaviour
    {
        [HideInInspector]
        public PlayerInputReciever InputReciever;
        public float Sens;
        float XRig;
        float YRig;
        public float BaseSpeed = 6f;
        public float SprintSpeedMult = 2f;
        float Speed;
        public GameObject BulletPrefab;
        public float BulletForce;

        // Start is called before the first frame update
        void Awake()
        {
            InputReciever = PlayerInputReciever.GetPlayerInputRecieverInstance();
        }

        // Update is called once per frame
        void Update()
        {
            if(!MenuManager.Instance.InMenu)
            {
                XRig -= InputReciever.MouseY * Sens * Time.deltaTime;
                YRig += InputReciever.MouseX * Sens * Time.deltaTime;
            }
            transform.localRotation = Quaternion.Euler(XRig, YRig, 0f);

            Vector3 Direction = (transform.forward * InputReciever.Vert) + (transform.right * InputReciever.Hori) + (transform.up * InputReciever.AltHori);
            Speed = (!InputReciever.SprintKeyHold) ? BaseSpeed : BaseSpeed * SprintSpeedMult;
            if(!MenuManager.Instance.InMenu) { transform.position += Direction * (Speed * Time.deltaTime); }

            if(Input.GetMouseButtonDown(0) && !MenuManager.Instance.InMenu)
            {
                GameObject NewBullet = Instantiate(BulletPrefab, transform.position, transform.rotation);
                NewBullet.GetComponent<Rigidbody>().AddForce((transform.forward + Direction) * BulletForce, ForceMode.VelocityChange);
                AudioManager.Instance.InteractWithSFX("Shoot", SoundEffectBehaviour.Play);
            }
        }
    }
}