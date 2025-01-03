using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{    
    GameManager gameManager;
    DifficultyManager difficultyManager;
    [SerializeField] DifficultyType gameDifficulty;

    Rigidbody2D rb;
    Animator animator; 
    CapsuleCollider2D cc;

    [Header("Visuals")]
    [SerializeField] AnimatorOverrideController[] animators;
    [SerializeField] GameObject playerDeathVfx;
    int skinIndex;
    [SerializeField] ParticleSystem dustFx;

    bool canBeControlled = false;

    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce =16;
    public float doubleJumpForce = 18;
    public bool canDoubleJump;
    float xInput;
    float yInput;
    bool facingRight = true;
    public int facingDirection = 1;
    float defaultGravityScale;

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
    // bool canBeKnocked;

    
    [Header("Collision Info")]
    public bool isGrounded;
    bool isAirborne; // isInTheAir
    public bool isWalled;
    public float groundCheckDistance;
    public float wallCheckDistance;
    public LayerMask whatIsGround;

    bool isPassive = false;

    [Header("Player Fruit Drop")]
    [SerializeField] GameObject fruitDropPrefab;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();        
        cc = GetComponent<CapsuleCollider2D>();
        animator = GetComponentInChildren<Animator>();
    }
    void Start()
    {
        gameManager = GameManager.instance;
        UpdateGameDifficulty();

        defaultGravityScale = rb.gravityScale;
        RespawnFinished(false);
        UpdateSkin();
        // Debug.Log(gameDifficulty);
    }

    

    void Update()
    {     
        UpdateAirborneState();   

        if(!canBeControlled) return;    

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

    public void Damage(){
        if(gameDifficulty == DifficultyType.Normal){     

            if(gameManager.FruitsCollected <=0){
                Die();
                // restart level
                gameManager.RestartLevel();
            }
            else{
                ObjectCreator.instance.CreateObject(fruitDropPrefab, transform, true);
                gameManager.RemoveFruit();
            }
            return;
        }

        if(gameDifficulty == DifficultyType.Hard){
            Die();
            //restart level
            gameManager.RestartLevel();
        }
    }
    
    void UpdateGameDifficulty()
    {
        difficultyManager = DifficultyManager.instance;
        if (difficultyManager != null) gameDifficulty = difficultyManager.difficulty;
    }

    public void UpdateSkin(){
        SkinManager skinManager = SkinManager.instance;
        
        if(skinManager == null) return;

        animator.runtimeAnimatorController = animators[skinManager.chosenSkinId];
    }

    public void RespawnFinished(bool finished){

        if(finished){
            rb.gravityScale = defaultGravityScale;
            canBeControlled = true;
            cc.enabled = true;
            AudioManager.instance.PlaySFX(10); // Respawn
        }
        else
        {
            rb.gravityScale =0;
            canBeControlled = false;
            cc.enabled = false;
        }
    }
    public void RespawnFinishedOK(){
        RespawnFinished(true);
    }

    public void Knockback(float damageSourceXPos){
        float knockbackDir =1;
        if(transform.position.x < damageSourceXPos){
            knockbackDir = -1;
        } 

        if(isKnocked) return;

        AudioManager.instance.PlaySFX(9); // Knocked
        CameraManager.instance.ScreenShake(knockbackDir);
        StartCoroutine(KnockbackRoutine());
        rb.linearVelocity = new Vector2(knockbackPower.x * knockbackDir, knockbackPower.y);
    }
    IEnumerator KnockbackRoutine()
    {
        // canBeKnocked = false;
        isKnocked = true;
        animator.SetTrigger("knockback");
        yield return new WaitForSeconds(knockbackDuration);
        // canBeKnocked = true;
        isKnocked = false;
    }
    public void Die() {
        AudioManager.instance.PlaySFX(0); //Death
        GameObject newFx = Instantiate(playerDeathVfx, transform.position, Quaternion.identity);
        Destroy(gameObject);
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
        dustFx.Play();

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

    public void Jump(){
        // dustFx.Play();
        AudioManager.instance.PlaySFX(3); // Jump
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }
    void DoubleJump()
    {
        dustFx.Play();
        AudioManager.instance.PlaySFX(3); // Jump
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, doubleJumpForce);
        canDoubleJump = false; // 더블점프를 했으므로 이제 불가능하게 만든다.
        isWallJumping = false;
    }
    
    void WallJump(){
        // dustFx.Play();
        AudioManager.instance.PlaySFX(12); // WallJump
        canDoubleJump = true; //마치 땅처럼 허용
        rb.linearVelocity = new Vector2(wallJumpForce.x * -facingDirection, wallJumpForce.y);
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
        if(isPassive) return;

        rb.linearVelocity = new Vector2(xInput * moveSpeed, rb.linearVelocity.y); 
    }
    public void SetPassive(bool value){
        isPassive = value;
        //공중에서 점프가능하게 하기 위해
        isGrounded = true;
    }

    void HandleCollision()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
        isWalled = Physics2D.Raycast(transform.position, Vector2.right * facingDirection, wallCheckDistance, whatIsGround);
    }

    void HandleWallSlide()
    {
        bool canWallSlide = isWalled && rb.linearVelocity.y < 0;
        float yModifier = yInput < 0 ? 1 : .05f;
        

        if (!canWallSlide) return;

        dustFx.Play();

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * yModifier);
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
        animator.SetFloat("xVelocity", rb.linearVelocity.x);
        animator.SetFloat("yVelocity", rb.linearVelocity.y);
        animator.SetBool("isGrounded", isGrounded);
        animator.SetBool("isWalled", isWalled);
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - groundCheckDistance));
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x + (wallCheckDistance * facingDirection), transform.position.y));
    }
}
