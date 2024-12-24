using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTrigger : MonoBehaviour //Player에 부착된다.
{
    bool hasTriggered = false;
    bool hasCollision = false;

    void OnTriggerEnter2D(Collider2D other){
        if(hasTriggered) return;

        if(other.gameObject.CompareTag("Trap"))
        {
            hasTriggered = true;
            Transform otherTr = other.transform;
            Player player = GetComponent<Player>();
            player.Damage();
            player.Knockback(otherTr.position.x);

            Invoke("ResetTrigger", 0.5f);
        }
    }
    void OnCollisionEnter2D(Collision2D other){
        if(hasCollision) return;

        if (other.gameObject.CompareTag("Enemy"))
        {
            hasCollision = true;
            Transform otherTr = other.transform;
            Player player = GetComponent<Player>();
            player.Damage();
            player.Knockback(otherTr.position.x);

            Invoke("ResetCollision", 0.5f);
        }
    }

    void ResetTrigger(){
        hasTriggered = false;
    }
    void ResetCollision(){
        hasCollision = false;
    }
}
