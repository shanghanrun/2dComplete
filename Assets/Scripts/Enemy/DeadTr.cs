using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadTr : MonoBehaviour
{
    EnemyRino rino;
    
    void Start()
    {
        rino = GetComponentInParent<EnemyRino>();
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Player")){
            AudioManager.instance.PlaySFX(1); // EnemyKicked
            Die();
            
        }
    }
    void Die(){
        rino.callFromChild = true;
        rino.Die();
    }
}
