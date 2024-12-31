using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    Animator animator;
    bool active;
    [SerializeField] bool canBeReActivated; // 최근에 방문한 체크포인트를 respawn포인트로 만드는 기능
    void Awake(){
        animator = GetComponent<Animator>();
    }

    void Start(){
        canBeReActivated = GameManager.instance.canBeReActivated;
    }

    void OnTriggerEnter2D(Collider2D other){
        if(active && canBeReActivated ==false) return;

        AudioManager.instance.PlaySFX(2); // Finish

        Player player = other.GetComponent<Player>(); //이것에 충돌하는 것은 Player밖에 없다.
        if(player !=null) ActivateCheckpoint();
    }
    
    void ActivateCheckpoint(){
        active = true;
        animator.SetTrigger("Activate");
        PlayerManager.instance.UpdateRespawnPosition(transform);
    }

}
