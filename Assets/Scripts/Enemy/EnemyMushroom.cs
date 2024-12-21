using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMushroom : Enemy
{
    protected override void Update()
    {
        base.Update();

        HandleCollision();
        
    }
    void LateUpdate()
    { // 현재프레임에서 변경된 변수가 적용된다.
        if (isGrounded) HandleTurnAround();
    }

    private void HandleTurnAround()
    {
        if (!isForwardGrounded || isWalled || isBumped){
            Flip();
            isBumped = false;
            idleTimer = idleDuration;
        }
    }

    protected override void HandleMovement()
    {
        base.HandleMovement();

        if (idleTimer > 0) return;

        rb.velocity = new Vector2(MoveSpeed * facingDir, rb.velocity.y);
    }

    void OnCollisionEnter2D(Collision2D other){
        if(other.gameObject.CompareTag("Trap") ||
            other.gameObject.CompareTag("Enemy"))
        {
            isBumped = true; 
        }
    }
    
}
