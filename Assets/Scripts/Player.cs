using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;
    Animator animator; 

    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce;
    public float doubleJumpForce;
    public bool canDoubleJump;
    float xInput;
    float yInput;
    bool facingRight = true;
    public int facingDirection = 1;

    [Header("Buffer Jump, Coyote Jump")]
    public float jumpBufferTime = .3f;
    public float jumpBufferCounter;
    bool canBufferJump;
    public float hangTime = 0.2f;
    public float hangTimeCounter;

    [Header("Wall interactions")]
    public float wallJumpDuration = .6f;
    public Vector2 wallJumpForce; // 이차원 벡터
    bool isWallJumping;

    [Header("Knockback")]
    public float knockbackDuration =1;
    public Vector2 knockbackPower;
    bool isKnocked;
    bool canBeKnocked;

    
    [Header("Collision Info")]
    public bool isGrounded;
    bool isAirborne; // isInTheAir
    public bool isWalled;
    public float groundCheckDistance;
    public float wallCheckDistance;
    public LayerMask whatIsGround;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {     
        UpdateAirborneState();       

        if (isKnocked) return;

        HandleInput();
        HandleWallSlide();
        HandleMovement();
        HandleFlip();
        HandleCollision();
        HandleAnimations();

        // 버퍼 카운터 감소
        DecreaseJumpBufferCounter();
    }

    public void Knockback(){
        if(isKnocked) return;
        
        StartCoroutine(KnockbackRoutine());
        animator.SetTrigger("knockback");
        rb.velocity = new Vector2(knockbackPower.x * -facingDirection, knockbackPower.y);
    }
    IEnumerator KnockbackRoutine()
    {
        canBeKnocked = false;
        isKnocked = true;
        yield return new WaitForSeconds(knockbackDuration);
        canBeKnocked = true;
        isKnocked = false;
    }    

    void UpdateAirborneState(){
        if (isAirborne && isGrounded)
        { //이전에 공중에 있다가, 땅에 닿았을 경우 -> 더블점브 가능, 그리고 땅이라서 isAirborne은 false로 전환
            HandleLanding();
        }
        if (!isGrounded && !isAirborne)
            BecomeAirborne();
    }
    void HandleLanding(){
        isAirborne = false; //땅이라서 isAirborne을 false로 만들고
        canDoubleJump = true; // 땅에 닿은 상태라면 doubleJump를 할 수 있게 만든다.

        AttemptExtraJump();        
        SetHangTimeCounter(); // 착지 후 여유시간 리셋
    }
    void BecomeAirborne()
    {
        isAirborne = true;
        DecreaseHangTimeCounter();
    }
    
    void HandleInput()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.Space)){
            SetBufferCounter();
            JumpProcess();
        }             
    }

    void JumpProcess(){
        if(isGrounded) Jump(); // 땅에서는 점프 가능하게 
        else if(isWalled ) WallJump();
        else if(canDoubleJump) DoubleJump(); // 땅이 아닌 경우(공중) 이때 canDoubleJump이면 점프가능
    }

    void Jump(){
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }
    void DoubleJump()
    {
        rb.velocity = new Vector2(rb.velocity.x, doubleJumpForce);
        canDoubleJump = false; // 더블점프를 했으므로 이제 불가능하게 만든다.
        isWallJumping = false;
    }
    
    void WallJump(){
        canDoubleJump = true; //마치 땅처럼 허용
        rb.velocity = new Vector2(wallJumpForce.x * -facingDirection, wallJumpForce.y);
        Flip();

        StopAllCoroutines();
        StartCoroutine(WallJumpRoutine());
    }
    IEnumerator WallJumpRoutine()
    {
        isWallJumping = true;
        yield return new WaitForSeconds(wallJumpDuration);
        isWallJumping = false;
    }

    #region JumpBuffer & CoyoteJump
    void SetBufferCounter()
    {
        jumpBufferCounter = jumpBufferTime;
    }
    void SetHangTimeCounter()
    {
        hangTimeCounter = hangTime;
    }
    void DecreaseJumpBufferCounter()
    {
        if (jumpBufferCounter > 0) jumpBufferCounter -= Time.deltaTime;
    }
    void DecreaseHangTimeCounter()
    {
        hangTime -= Time.deltaTime;
    }
    void InitializeAllCounter()
    {
        jumpBufferCounter = 0;
        hangTimeCounter = 0;
    }
    #endregion

    void AttemptExtraJump(){      
        if(jumpBufferCounter >0 && hangTimeCounter >0){
            Jump(); // 실제 점프 실행   
            InitializeAllCounter();
        } 
    }

    void HandleMovement(){
        if(isWalled) return;
        if(isWallJumping) return; // wallJump중에는 아래 코드를 실행하지 않는다.

        rb.velocity = new Vector2(xInput * moveSpeed, rb.velocity.y); 
    }

    void HandleCollision()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
        isWalled = Physics2D.Raycast(transform.position, Vector2.right * facingDirection, wallCheckDistance, whatIsGround);
    }

    void HandleWallSlide()
    {
        bool canWallSlide = isWalled && rb.velocity.y < 0;
        float yModifier = yInput < 0 ? 1 : .05f;

        if (!canWallSlide) return;

        rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * yModifier);
    }

    void HandleFlip(){
        // if(rb.velocity.x <0 && facingRight || rb.velocity.x > 0 && !facingRight)
        //     Flip();
        if(xInput <0 && facingRight || xInput > 0 && !facingRight)
            Flip();
    }
    void Flip()
    {
        transform.Rotate(0, 180, 0);
        facingRight = !facingRight;
        facingDirection = facingDirection * -1;
    }

    void HandleAnimations()
    {
        animator.SetFloat("xVelocity", rb.velocity.x);
        animator.SetFloat("yVelocity", rb.velocity.y);
        animator.SetBool("isGrounded", isGrounded);
        animator.SetBool("isWalled", isWalled);
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - groundCheckDistance));
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x + (wallCheckDistance * facingDirection), transform.position.y));
    }
}
