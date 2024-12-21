using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTrigger : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other){
        // Player player = other.GetComponent<Player>();
        // if(player !=null){
        //     player.Knockback();
        // }
        if(other.gameObject.CompareTag("Trap"))
        {
            Player player = GetComponent<Player>();
            player.Knockback(transform.position.x);
        }
    }
    void OnCollisionEnter2D(Collision2D other){
        if (other.gameObject.CompareTag("Enemy"))
        {
            Player player = GetComponent<Player>();
            player.Knockback(transform.position.x);
        }
    }
}
