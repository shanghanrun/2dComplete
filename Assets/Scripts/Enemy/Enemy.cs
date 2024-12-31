using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected Animator animator;
    protected Rigidbody2D rb;
    protected Collider2D cd;
    protected Collider2D[] cds;
    SpriteRenderer sr => GetComponent<SpriteRenderer>(); //언제던지 최신값을 받아올 수 있게 한다. (고정된 값이 아니라, 현재 값)

    float moveSpeed = 4f;
    protected virtual float MoveSpeed {
        get => moveSpeed;
        set => moveSpeed = value;
    }
    // protected virtual float MoveSpeed {get; set;}= 4f;
    
    [Header("Enemy Facing Direction")]
    [SerializeField] bool flipDefaultFacingDirection; // 디폴드 디렉션(-1, 왼쪽)과 반대로 할 지 여부 //디폴트 false

    [Header("Collision Check")]
    [SerializeField] protected float groundCheckDistance =1.1f;
    [SerializeField] protected float wallCheckDistance = 1.3f;
    [SerializeField] protected LayerMask whatIsGround;
    [SerializeField] protected Transform groundCheckTr; 

    [Header("Player detect")]
    [SerializeField] protected LayerMask whatIsPlayer;
    protected bool canMove = false; 
    protected Transform playerTr;

    [Header("Death details")]
    [SerializeField] protected float deathImpact =4;
    [SerializeField] protected float deathRotationSpeed = 360;
    protected bool isDead;
    protected int deathRotationDirection =1;
    
    protected int facingDir = -1;  //최초 이동방향(스프라이트방향)
    protected bool facingRight = false;
    protected bool isGrounded;
    protected bool isForwardGrounded;
    protected bool isWalled;
    protected bool isBumped; // 자기편끼리 부딪친 경우
    protected bool playerDetected;
    protected bool canFlip = true;

    protected float idleDuration = 1f;
    protected float idleTimer;
    protected bool hasBody = true; // EnemySnail용

    protected virtual void Awake(){
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<Collider2D>();
        cds = GetComponents<Collider2D>();
    }

    protected virtual void Start(){
        InvokeRepeating(nameof(UpdatePlayerRef), 0,1);
        if(flipDefaultFacingDirection){
            if(!facingRight){ // 기본은 좌측을 바라보고 있다.(-1)
                Flip();
            }
        }

        PlayerManager.OnPlayerRespawn += UpdateNullPlayerRef; //메소드(함수)이름만 넣어주면 된다.(subscribe)
    }
    void UpdatePlayerRef(){  //가장 최근의 player의 Transform 위치를 얻기
        if(playerTr !=null){
            playerTr = PlayerManager.instance.player.transform;
        }
    }

    void UpdateNullPlayerRef(){ // 플레이어가 사라져서 playerTr이 없을 경우 플레이어 Transform 위치얻기
        if(playerTr == null){
            playerTr = PlayerManager.instance.player.transform;
        }
    }
    
    protected virtual void Update(){
        HandleAnimator();
        idleTimer -= Time.deltaTime;

        if (isDead) {
            HandleDeathRotation();
            return;
        } 
        
        HandleMovement();   
    }

    protected virtual void FixedUpdate(){
        HandleCollision();
    }

    protected virtual void HandleAnimator()
    {
        animator.SetFloat("xVelocity", rb.linearVelocity.x);
    }
    protected virtual void HandleMovement(){
        if(idleTimer >0) return;
        if(!canMove) return;

    }

    protected virtual void HandleFlip(float xValue)
    {
        // if(!hasBody) return;
        if (xValue < 0 && facingRight || xValue > 0 && !facingRight)
            Flip();
    }

    protected virtual void Flip()
    {
        transform.Rotate(0, 180, 0);
        facingRight = !facingRight;
        facingDir = facingDir * -1;
    }

    protected virtual void HandleCollision() //움직이는 enemy에서 사용한다.
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
        isForwardGrounded = Physics2D.Raycast(groundCheckTr.position, Vector2.down, groundCheckDistance, whatIsGround);
        isWalled = Physics2D.Raycast(transform.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);

        if(!hasBody){
            isForwardGrounded = true;//그냥 돌진하도록
        }
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - groundCheckDistance));
        Gizmos.DrawLine(transform.position, new Vector2(groundCheckTr.position.x, groundCheckTr.position.y - groundCheckDistance));
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x + (wallCheckDistance * facingDir), transform.position.y));
    }

    protected virtual void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.CompareTag("Player")){
            Player player = other.gameObject.GetComponent<Player>();
            Rigidbody2D playerRb = other.gameObject.GetComponent<Rigidbody2D>();            

            if(playerRb.linearVelocity.y > -0.4) return; //위에서 아래로 내려와야 된다.
            player.Jump();
            AudioManager.instance.PlaySFX(1); // EnemyKicked
            Die();
        }
    }

    public virtual void Die()
    {
        if(rb.bodyType == RigidbodyType2D.Kinematic){
            rb.bodyType = RigidbodyType2D.Dynamic;
        }

        animator.SetTrigger("Hit");
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, deathImpact);
        DisableColliders();
        isDead = true; // Rotate를 위해
        SetRandomDirection();

        PlayerManager.OnPlayerRespawn -= UpdatePlayerRef; // unsubscribe
        Destroy(gameObject, 1f);
    }

    private void SetRandomDirection(){
        if (Random.Range(0, 100) < 50){
            deathRotationDirection *= -1;
        }
    }

    void DisableColliders(){
        // Collider2D[] colls = GetComponents<Collider2D>();
        if(cds != null && cds.Length >1){
            foreach(var cd in cds){
                cd.enabled = false;
            }
        } else if( cd !=null){
            cd.enabled = false;
        }
    }
    void HandleDeathRotation(){
        //! 회전 속도 증가
        deathRotationSpeed += Time.deltaTime * 10; // 속도 증가 계수 조정

        transform.Rotate(0,0, (deathRotationSpeed * deathRotationDirection) *Time.deltaTime);
    }
    
    
}
