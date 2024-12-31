using UnityEngine;

public class EnemyBat : Enemy
{
    [Header("Bat details")]
    Vector3 originalPos;
    Vector3 destination;
    // [SerializeField] float playerDetectRange = 17; // 사실상 attackRadius랑 같은 의미...
    [SerializeField] float attackRadius = 17; // agrroRadius
    [SerializeField] float batMoveSpeed = 2;
    [SerializeField] float attackSpeed = 4;
    [SerializeField] float chaseDuration = 1;
    float chaseTimer;

    public bool canDetectPlayer = true; 
    public Collider2D target; // OverlapCircle에서 감지한 대상 콜라이더이다.

    override protected void Awake() {
        base.Awake();

        originalPos = transform.position;
        canMove = false;
        MoveSpeed = batMoveSpeed; // Bat의 스피드를 넣어주기
    }

    override protected void Update() {
        base.Update(); 
        //이 안에 
        // HandleAnimator();
        // idleTimer -= Time.deltaTime; 있다.
        chaseTimer -= Time.deltaTime;

        if(idleTimer <0) {
            canDetectPlayer = true;
            // target = null; 이걸 하면 플레이어를 끝까지 추적...(??)
        }

        HandleMovement();
        HandlePlayerDetection();
    }

    protected override void HandleAnimator()
    {
        // base.HandleAnimator();
    }
    protected override void HandleMovement()
    {
        // base.HandleMovement();
        if(!canMove) return;

        HandleFlip(destination.x);
        transform.position = Vector2.MoveTowards(transform.position, destination, MoveSpeed * Time.deltaTime);

        if(chaseTimer >0 && target !=null){
            target = Physics2D.OverlapCircle(transform.position, attackRadius, whatIsPlayer);
            MoveSpeed = attackSpeed;
            
            if(target !=null){ //제자리갔을 경우 target디텍트 안되는 경우도 있다.
                destination = target.transform.position; //최근 타겟위치
            }
        }else{
            destination = originalPos;
            MoveSpeed = batMoveSpeed;
        }

        // 플레이어 근처에 오면 속도를 줄이기
        if(Vector2.Distance(transform.position, destination)< 2){
            MoveSpeed = batMoveSpeed;
        }

        if (Vector2.Distance(transform.position, destination) < .1f){
            if(transform.position == originalPos){

                idleTimer = idleDuration *3; //3초로 만들어 보자.
                chaseTimer = -1;
              
                canMove = false;                
                animator.SetBool("isMoving", false);
                // 아래 두 조건을 만족하게 해주어야, 다시 detect할 수 있다.
                canDetectPlayer = false; // idleTimer를 통해 true로 만들게
                target = null; 
            }
            else { // 플레이어 target에 근접했을 경우
                destination = originalPos;
                chaseTimer = chaseDuration+ idleDuration;
            }
        }        
    }

    void AllowMovement() => canMove = true;

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos(); // wallCheck, groundCheck등을 위한 기즈모

        Gizmos.DrawWireSphere(transform.position, attackRadius); //Bat의 공격범위 기즈모
    }

    void HandlePlayerDetection(){
        if(target == null && canDetectPlayer){
            target = Physics2D.OverlapCircle(transform.position, attackRadius, whatIsPlayer);

            if(target !=null){
                destination = target.transform.position;
                // chaseTimer = chaseDuration;
                canDetectPlayer = false;
                animator.SetBool("isMoving", true);
            }
        }
    }
}
