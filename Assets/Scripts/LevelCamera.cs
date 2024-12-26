using UnityEngine;
using Unity.Cinemachine;

public class LevelCamera : MonoBehaviour
{
    CinemachineCamera cinemachineCamera; // 게임오브젝트와 유사한 카메라(오브젝트)

    void Awake(){
        cinemachineCamera = GetComponentInChildren<CinemachineCamera>(true);
        EnableCamera(false);
    }

    public void SetNewTarget(Transform newTarget){
        cinemachineCamera.Follow = newTarget;
    }

    public void EnableCamera(bool enable){
        cinemachineCamera.gameObject.SetActive(enable);
    }
}
