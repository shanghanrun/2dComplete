using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    Rigidbody2D rb;    

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void SetVelocity(float gunSpeed, int facingDir) => rb.velocity = new Vector2(gunSpeed * facingDir, 0);

    void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.CompareTag("Player")){
            // player 충격
            other.gameObject.GetComponent<Player>().Knockback(transform.position.x);
            // 총알삭제
            Destroy(gameObject);
        }
        if (other.gameObject.CompareTag("Ground"))
        {
            rb.velocity = Vector2.zero;
            rb.gravityScale = 4f;
            if (gameObject != null) Destroy(gameObject, 1.5f);
        }
    }
    
}
