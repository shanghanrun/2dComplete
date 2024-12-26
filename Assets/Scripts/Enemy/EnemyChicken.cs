using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChicken : Enemy
{
    [Header("Chicken details")]
    [SerializeField] float aggroDuration;
    float aggroTimer;
    [SerializeField] float playerDetectRange = 10;

    protected override void Update()
    {
        base.Update();

        // if (isGrounded) HandleTurnAround();
        // HandleCollision();

        DetectPlayer();
    }
    
    void LateUpdate(){ // 현재프레임에서 변경된 변수가 적용된다.
        if(isGrounded) HandleTurnAround();
    }

    void HandleTurnAround()
    {
        canMove = false;
        if (!isForwardGrounded || isWalled || isBumped)
        {
            Flip();
            canMove = false; //끝에서는 돌아서서 멈춤
            isBumped = false;
        }        
    }

    protected override void HandleMovement()
    {
        base.HandleMovement();

        if(playerDetected && playerTr !=null) HandleFlip(playerTr.transform.position.x);

        rb.linearVelocity = new Vector2(MoveSpeed * facingDir, rb.linearVelocity.y);
    }
    protected override void HandleFlip(float xValue)
    {
        if(xValue < transform.position.x && facingRight || xValue >transform.position.x && !facingRight){
            if(canFlip){
                canFlip = false;
                Invoke(nameof(Flip), .3f);
            }
        }
    }
    protected override void Flip()
    {
        base.Flip();
        canFlip = true;
    }

    // void OnCollisionEnter2D(Collision2D other) // 이 기능은 일단 비활성화
    // {
    //     if (other.gameObject.CompareTag("Trap") ||
    //         other.gameObject.CompareTag("Enemy"))
    //     {
    //         isBumped = true;
    //     }
    // }

    void DetectPlayer(){
        //재생된 플레이어의 playerTr null 방지
        if (playerTr == null){
            playerTr = GameManager.instance.player.transform;
        }

        // RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right *facingDir, playerDetectRange, whatIsPlayer);
        // playerDetected = hit.collider !=null;

        // 써클 콜라이더
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, playerDetectRange, whatIsPlayer);
        playerDetected = hits.Length > 0;

        if (playerTr == null && hits.Length > 0){
            playerTr = hits[0].transform; // Get the first detected player
        }

        if(playerDetected && playerTr !=null){
            canMove =true;
            aggroTimer = aggroDuration;
        }       
    }

    protected override void OnDrawGizmos() {
        base.OnDrawGizmos();
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, playerDetectRange);
        // Gizmos.DrawLine(transform.position, new Vector2(transform.position.x + (playerDetectRange * facingDir), transform.position.y));
    }

}
