using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap_Trampoline : MonoBehaviour
{
    protected Animator animator;
    public float bounceForce = 30f;
    Vector2 force;
    bool hasTriggered = false;

    void Start(){
        animator = GetComponent<Animator>();
        // force = transform.up * bounceForce;
    }
    
    void OnTriggerEnter2D(Collider2D other){
        if(!hasTriggered && other.gameObject.CompareTag("Player")){
            hasTriggered = true;

            Player player = other.gameObject.GetComponent<Player>();
            Rigidbody2D playerRb = other.gameObject.GetComponent<Rigidbody2D>();
            force = transform.up * bounceForce; // 충돌 당시의 방향벡터값을 받아온다.

            if (player !=null && playerRb !=null){               
                player.SetPassive(true);
                // playerRb.AddForce(new Vector2(0, bounceForce), ForceMode2D.Impulse);
                // playerRb.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);
                // playerRb.AddForce(new Vector2(-bounceForce,0), ForceMode2D.Impulse);
                playerRb.AddForce(force, ForceMode2D.Impulse);
                
            }

            animator.SetTrigger("Active"); //용수철에니메이션
            StartCoroutine(Wait(player));
            
        }
    }
    IEnumerator Wait(Player player){
        yield return new WaitForSeconds(0.2f);
        player.SetPassive(false);// 다시 움직일 수 있게 한다.
        //!이때에는 플레이어가 공중에 있으니, 트램폴린을 다시 활성화시킨다.
        hasTriggered = false;
    }
}
