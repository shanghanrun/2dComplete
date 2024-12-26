using UnityEngine;
using Unity.Cinemachine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;

    [Header("Screen Shake")]
    [SerializeField] Vector2 shakeVelocity;

    CinemachineImpulseSource impulseSource;

    void Awake(){
        instance = this;

        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    public void ScreenShake(float shakeDir){
        impulseSource.DefaultVelocity = new Vector2(shakeVelocity.x * shakeDir, shakeVelocity.y);
        impulseSource.GenerateImpulse();
    }
}
