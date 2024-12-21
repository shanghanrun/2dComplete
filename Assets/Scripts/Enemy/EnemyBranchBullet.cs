using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBranchBullet : MonoBehaviour
{
    Rigidbody2D rb;
    Animator animator;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    public void SetVelocity(float gunSpeed, int facingDir) => rb.velocity = new Vector2(gunSpeed * facingDir, 0);

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // player 충격
            other.gameObject.GetComponent<Player>().Knockback(transform.position.x);
            // 총알삭제
            animator.SetTrigger("Depart");
            rb.velocity = Vector2.zero;
            rb.gravityScale = 4f;
            // 4초후에 저절로 삭제가 EnemyTrunk에서 
            if(gameObject !=null) Destroy(gameObject,1.5f);
        }
        if(other.gameObject.CompareTag("Ground")){
            animator.SetTrigger("Depart");
            rb.velocity = Vector2.zero;
            rb.gravityScale = 4f;
            // 4초후에 저절로 삭제가 EnemyTrunk에서 
            if (gameObject != null) Destroy(gameObject, 1.5f);
        }
    }
}

