using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap_Fire : MonoBehaviour
{
    Animator animator;
    CapsuleCollider2D cap;

    void Start()
    {
       animator = GetComponent<Animator>(); 
       cap = GetComponent<CapsuleCollider2D>();
    }

    public void SetFire(bool isActive){
        animator.SetBool("active", isActive);
        if(isActive){
            cap.enabled = true;
        } else{
            cap.enabled = false;
        }
    }
}
