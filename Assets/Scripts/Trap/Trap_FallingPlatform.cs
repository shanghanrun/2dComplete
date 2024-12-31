using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap_FallingPlatform : MonoBehaviour
{
    Animator animator;
    Rigidbody2D rb;
    BoxCollider2D[] colls;

    [Header("Platform fall details")]
    public float impactSpeed = 3;
    public float impactDuration = .1f;
    float impactTimer;  // onTrigger시 impactDuration을 받아서, 줄여나가는 것
    bool impactHappened = false;
    bool canMove = false; //wayPoint간에 이동 가능 여부
    public float fallDelay = 1f;

    public float speed = .75f;
    public float travelDistance = 1;
    public Vector3[] wayPoints; 
    int index =0;

    float XPos => transform.position.x;
    float YPos => transform.position.y;
    float ZPos => transform.position.z;

    

    void Awake(){
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        colls = GetComponents<BoxCollider2D>();
    }

    void Start(){
        SetupWayPoints();

        float randomDelay = Random.Range(0, 4f);
        speed = Random.Range(0.5f, 1.5f);
        Invoke(nameof(ActivatePlatform), randomDelay); // 플랫폼마다 달라지도록
    }

    void ActivatePlatform() => canMove = true;

    void SetupWayPoints(){
        wayPoints = new Vector3[2]; //배열크기 초기화
        float yOffset = travelDistance /2;

        wayPoints[0] = new Vector3(XPos, YPos + yOffset, ZPos); 
        wayPoints[1] = new Vector3(XPos, YPos - yOffset, ZPos);
    }

    void Update(){
        HandleImpact();
        HandleMovement();
    }

    void HandleMovement(){
        if(! canMove) return;

        transform.position = Vector2.MoveTowards(transform.position, wayPoints[index], speed *Time.deltaTime);

        if(Vector2.Distance(transform.position, wayPoints[index]) < .01f){
            index++;
            if(index >= wayPoints.Length){
                index =0;
            }
        }
    }

    void HandleImpact(){
        if(impactTimer <0) return;

        impactTimer -= Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, transform.position + (Vector3.down *10), impactSpeed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other){
        if(impactHappened) return; // 충격은 한번만 검출하고 그 이후로 안되게

        Player player = other.gameObject.GetComponent<Player>();
        if(player){
            Invoke("SwitchOffPlatform", fallDelay);
            impactTimer = impactDuration;
            impactHappened = true;
        }
    }

    void SwitchOffPlatform(){
        canMove = false; // HandleMovement의 위치조정을 하지 않게 해서, 더디게 떨어지는 것 방지
        animator.SetTrigger("Deactivate");
        // rb.isKinematic = false; 이것 deprecated
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 3.5f;
        // rb.drag = .5f; 이것 deprecated
        rb.linearDamping = .5f; 
        

        foreach( BoxCollider2D coll in colls){
            coll.enabled = false;
        }
    }

}
