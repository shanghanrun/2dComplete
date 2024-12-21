using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySnail : Enemy
{
    [Header("Snail Details")]
    [SerializeField] EnemySnailBody bodyPrefab;
    [SerializeField] float maxSpeed =10;
    // bool hasBody = true;

    
    protected override void Start(){
        base.Start();
        canMove = true; //시작시 플레이어검출 없이도 움직일 수 있게
    }

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
        if (!isForwardGrounded || isWalled || isBumped)
        {
            Flip();
            isBumped = false;
            idleTimer = idleDuration;
        }
    }

    protected override void HandleMovement()
    {
        base.HandleMovement();
        if(!canMove) return;

        if (idleTimer > 0) return;

        rb.velocity = new Vector2(MoveSpeed * facingDir, rb.velocity.y);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        
        if (other.gameObject.CompareTag("Trap") ||
            other.gameObject.CompareTag("Enemy"))
        {
            isBumped = true;
        }        
    }
    protected override void Flip(){
        base.Flip();
        if(!hasBody){ // 껍데기만 있을 경우, 벽에 부딪쳐 플립할 때
            animator.SetTrigger("WallHit"); //충격받는 모습
            //
        }
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Player player = other.gameObject.GetComponent<Player>();
            Rigidbody2D playerRb = other.gameObject.GetComponent<Rigidbody2D>();
            if (playerRb.velocity.y < -0.4f)
            {               
                Die();
            }
        }
    }

    public override void Die(){
        if(hasBody){
            canMove = false;
            hasBody = false;
            animator.SetTrigger("Hit");

            rb.velocity = Vector2.zero;
            idleDuration = 0;
        } 
        else if(!canMove && !hasBody){
            animator.SetTrigger("Hit");
            canMove = true;
            MoveSpeed = maxSpeed;
        }        
        else{
            base.Die();
        }
    }

    void CreateBody(){
        EnemySnailBody newBody = Instantiate(bodyPrefab, transform.position, Quaternion.identity );

        if (Random.Range(0, 100) < 50)
        {
            deathRotationDirection *= -1;
        }

        newBody.SetupBody(deathImpact, deathRotationSpeed *deathRotationDirection, facingDir);

        Destroy(newBody.gameObject, 4);
    }
    

}
