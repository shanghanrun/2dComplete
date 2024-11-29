using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap_Saw : MonoBehaviour
{
    public float moveSpeed =3;
    public Transform[] wayPoints;
    Vector3[] wayPointPositions;
    public int index =1;
    int endIndex;
    bool isStart =true;
    public float delay = 1;
    bool canMove = true;

    Animator animator => GetComponent<Animator>();
    SpriteRenderer sr => GetComponent<SpriteRenderer>();
    
    void Start()
    {
        // wayPointPositions 배열 초기화
        wayPointPositions = new Vector3[wayPoints.Length];

        for (int i=0; i< wayPoints.Length; i++){
            wayPointPositions[i] = wayPoints[i].position;
        }

        animator.SetBool("active", canMove);
        // transform.position = wayPoints[0].position;
        transform.position = wayPointPositions[0];
        endIndex = wayPointPositions.Length - 1;
    }

    void Update(){
        if(!canMove) return;

        transform.position = Vector2.MoveTowards(transform.position, wayPointPositions[index], moveSpeed *Time.deltaTime);

        // 여러 개의 포인트를 왕복운동하는 방식
        if(Vector2.Distance(transform.position, wayPointPositions[index])< 0.01f){
            UpdateIndex();
        }        
    }

    void UpdateIndex(){
        if (isStart){
            index++;            
            if (index > endIndex){
                isStart = false;
                index -= 2; // 되돌아가기 위해 2 감소
                StartCoroutine(StopMovement(delay));
            }
        }
        else{
            index--;            
            if (index < 0){
                isStart = true;
                index = 1; // 다시 전진하기 위해 1로 설정
                StartCoroutine(StopMovement(delay));
            }
        }
    }

    IEnumerator StopMovement(float delay){
        canMove = false;
        animator.SetBool("active", canMove);
        yield return new WaitForSeconds(delay);
        canMove = true;
        sr.flipX = !sr.flipX; //반대방향으로 돌게
        animator.SetBool("active", canMove);
        
    }

}
