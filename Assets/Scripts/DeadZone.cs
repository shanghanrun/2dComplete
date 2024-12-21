using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.CompareTag("Player")){
            Player player = other.gameObject.GetComponent<Player>();
            player.Die();
           
            GameManager.instance.RespawnPlayer();
        }
        if(other.gameObject.CompareTag("Enemy")){
            Destroy(other.gameObject, 2);
        }
    }
}
