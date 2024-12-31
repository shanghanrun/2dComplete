using System.Collections;
using UnityEngine;
using Unity.Cinemachine;

public class EnemyRino : Enemy
{
	[Header("Rino details")]	
	[SerializeField]  float playerDetectRange = 10;
	[SerializeField] Vector2 impactPower = new Vector2(-3.5f, 5);
	[SerializeField] float maxSpeed = 20f;
	[SerializeField] float speedIncreaseRate = 10f;
	[SerializeField] float rinoSpeed = 8f;
	float currentMoveSpeed;
	bool canDetect = true;
	public bool callFromChild = false;

	[Header("Effects")]
	[SerializeField] ParticleSystem dustFx;
	[SerializeField] CinemachineImpulseSource impulseSource;
	[SerializeField] Vector2 cameraImpulseDir = new Vector2(0.6f,0.6f);

	protected override void Start(){
		base.Start();
		MoveSpeed = rinoSpeed;
		currentMoveSpeed = MoveSpeed;
		impulseSource = GetComponent<CinemachineImpulseSource>(); // EnemyRino오브젝트의 컴포넌트
	}

	protected override void Update()
	{
		base.Update();

		DetectPlayer();
	}
	
	protected override void HandleCollision() //Rino, Mushroom처럼 움직이는 enemy에서 사용한다.
	{
		base.HandleCollision();

		//웅덩이를 만났을 경우
		if(!isForwardGrounded){
			rb.linearVelocity = Vector2.zero;
			Flip();
			canMove = false;
			SpeedReset();
		} 

		//벽에 부딪쳤을 경우
		if(isWalled) {
			animator.SetTrigger("HitWall");
			HitWallImpact();
			
			BackOff();
			//잠시 못 움직이게 설정. 그런데 어차피 canMove false해야 된다.
			canMove = false;
			isWalled = false; //상태초기화
			SpeedReset();

			//플립 호출 및 탐지 제한
			FlipWithCooldown(3.5f);
			StartCoroutine(DisableDetectionWhile(3.5f));
		}
	}
	void SpeedReset(){
		currentMoveSpeed = MoveSpeed;
	}
	void FlipWithCooldown(float delay)
	{
		if (canFlip)
		{
			canFlip = false;
			Invoke(nameof(Flip), delay);
			Invoke(nameof(EnableFlip), delay+0.2f);
		}
	}
	IEnumerator DisableDetectionWhile(float seconds)
	{
		canDetect = false;
		yield return new WaitForSeconds(seconds);
		canDetect = true;
	}

	void EnableFlip()
	{
		canFlip = true;
	}

	void BackOff(){
		rb.linearVelocity = Vector2.zero;
		rb.AddForce(new Vector2(impactPower.x *(facingDir), impactPower.y), ForceMode2D.Impulse);
	}
	
	protected override void HandleMovement()
	{		
		base.HandleMovement();
		if(!canMove) return;

		// 플레이어를 발견하고 달리다가, 플레이어가 사라지더라도, 벽을 충돌해야 된다.
		if (!playerDetected)
		{						
			SpeedReset();
			if(!isWalled){
				canMove = true;	
			} else {
				canMove = false;
				HitWallImpact();
			}
		} else{ //플레이어 탐지시 속도증가
			currentMoveSpeed += speedIncreaseRate *Time.deltaTime;
			currentMoveSpeed = Mathf.Min(currentMoveSpeed, maxSpeed);// 최대속도 제한
		}

		rb.linearVelocity = new Vector2(currentMoveSpeed * facingDir, rb.linearVelocity.y);
	}
	
	
	void DetectPlayer()
	{
		//재생된 플레이어의 playerTr null 방지
		if (playerTr == null)
		{
			var playerInstance = PlayerManager.instance.player;
			if(playerInstance !=null){
				playerTr = playerInstance.transform;
			}
			else{
				//플레이어가 존재하지 않으면 공격로직 중단
				playerDetected = false;
				canMove = false;
				return; //! 아래 detect로직 수행 안하게
			}			
		}		

		if(!canDetect || playerTr == null) return;

		playerDetected = Physics2D.Raycast(transform.position, Vector2.right * facingDir, playerDetectRange, whatIsPlayer);

		if (playerDetected && playerTr != null)
		{
			canMove = true;			
		} 
	}
	

	protected override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		Gizmos.DrawLine(transform.position, new Vector2(transform.position.x + (playerDetectRange * facingDir), transform.position.y));
	}
    public override void Die()
    {
		if(!callFromChild){
			base.Die();
		}
        // 그 외에는 아무것도 안한다. 죽지 않게

		callFromChild = false; // 플래그 초기화
    }

	void HitWallImpact(){
		dustFx.Play();
		impulseSource.DefaultVelocity = new Vector2(cameraImpulseDir.x * facingDir, cameraImpulseDir.y);
		impulseSource.GenerateImpulse();
	}
}
