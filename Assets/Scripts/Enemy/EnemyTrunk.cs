using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTrunk : Enemy
{
    [Header("Trunk Detail")]
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform gunPoint;
    [SerializeField] float gunSpeed = 10;
    public float lastTimeAttacked;
    [SerializeField] float attackCooldown = 1.5f;
    [SerializeField] float playerDetectRange = 10f;
    [SerializeField] float trunkSpeed = 2f;

    EnemyBranchBullet newBullet;

    bool canAttack = false;

    protected override void Start()
    {
        base.Start();
        MoveSpeed = trunkSpeed;
    }

    protected override void Update()
    {
        base.Update();

        canAttack = Time.time > lastTimeAttacked + attackCooldown;
        if (playerDetected && canAttack) Attack();
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
        
        if (idleTimer > 0) return;

        if (playerDetected)
        {
            canMove = false;
        }
        else { canMove = true; }
        if (!canMove) return;

        rb.velocity = new Vector2(MoveSpeed * facingDir, rb.velocity.y);
    }

    void Attack()
    {
        lastTimeAttacked = Time.time;
        animator.SetTrigger("Attack"); //이 안에서 이벤트로 CreateBullet 실행한다.
    }
    void CreateBullet()
    {
        var newBulletObject = Instantiate(bulletPrefab, gunPoint.position, Quaternion.identity);
        // newBullet.transform.SetParent(transform); 불필요
        newBullet = newBulletObject.GetComponent<EnemyBranchBullet>();
        if (newBullet != null) newBullet.SetVelocity(gunSpeed, facingDir);
        Destroy(newBulletObject, 4f); //4초후 저절로 삭제(플레이어 안맞아도)
    }

    protected override void HandleAnimator()
    {
        // 아무것도 안한다. 안움직여서 xVelocity 불필요
    }

    protected override void HandleCollision()
    { // 여기에 playerDetect 를 추가한다.
        base.HandleCollision();
        playerDetected = Physics2D.Raycast(transform.position, Vector2.right * facingDir, playerDetectRange, whatIsPlayer);
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x + playerDetectRange * facingDir, transform.position.y));
    }
}
