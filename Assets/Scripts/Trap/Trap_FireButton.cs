using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap_FireButton : MonoBehaviour
{
    Trap_Fire trapFire;
    Animator animator;
    bool isActive = false;

    void Start()
    {
        trapFire = GetComponentInParent<Trap_Fire>();
        animator = GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.CompareTag("Player")){
            isActive = !isActive;
            trapFire.SetFire(isActive);
            animator.SetTrigger("Active");
        }
    }
}
