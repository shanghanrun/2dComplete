using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap_Trampoline : MonoBehaviour
{
    Animator animator;
    public float bounceForce = 30f;
    Vector2 force;

    void Start(){
        animator = GetComponent<Animator>();
        force = transform.up * bounceForce;
    }
    
    void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.CompareTag("Player")){
            Player player = other.gameObject.GetComponent<Player>();
            Rigidbody2D playerRb = other.gameObject.GetComponent<Rigidbody2D>();
            if(player !=null && playerRb !=null){               
                player.SetPassive(true);
                // playerRb.AddForce(new Vector2(0, bounceForce), ForceMode2D.Impulse);
                // playerRb.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);
                // playerRb.AddForce(new Vector2(-bounceForce,0), ForceMode2D.Impulse);
                playerRb.AddForce(force, ForceMode2D.Impulse);
                
            }

            animator.SetTrigger("Active");
            StartCoroutine(Wait(player));
            
        }
    }
    IEnumerator Wait(Player player){
        yield return new WaitForSeconds(2);
        player.SetPassive(false);
    }
}
