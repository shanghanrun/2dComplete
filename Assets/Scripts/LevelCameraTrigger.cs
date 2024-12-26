using UnityEngine;

public class LevelCameraTrigger : MonoBehaviour
{
    LevelCamera levelCamera; // LevelCamera오브젝트에 달린 LevelCamera 클래스(스크립트)의 인스턴스

    void Awake(){
        levelCamera = GetComponentInParent<LevelCamera>();
    }

    void OnTriggerEnter2D(Collider2D other){
        Player player = other.gameObject.GetComponent<Player>();

        if(player !=null){
            levelCamera.EnableCamera(true);
            levelCamera.SetNewTarget(player.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        Player player = other.gameObject.GetComponent<Player>();

        if (player != null)
        {
            levelCamera.EnableCamera(false);
        }
    }
}
