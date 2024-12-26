using UnityEditor.Tilemaps;
using UnityEngine;

public class MenuCharacter : MonoBehaviour
{
    [SerializeField] float speed =4;
    Vector3 destination;
    Animator animator;

    int facingDir = 1;
    bool facingRight = true;

    bool isMoving;

    void Awake(){
        animator = GetComponent<Animator>();
    }

    void Update(){ // 어태치된 게임오브젝트의 위치조정
        animator.SetBool("isMoving", isMoving);

        if(isMoving){
            transform.position = Vector2.MoveTowards(transform.position, destination, Time.deltaTime * speed);

            if(Vector2.Distance(transform.position,destination) < .1f){
                isMoving = false;
            }
        }
    }

    public void MoveTo(Transform newDestination){
        destination = newDestination.position;
        destination.y = transform.position.y; // 높이는 그대로

        isMoving = true;
        HandleFlip(destination.x);
    }

    void HandleFlip(float xValue){
        if(xValue <transform.position.x && facingRight || xValue > transform.position.x  && !facingRight ) Flip(); 
    }
    void Flip(){
        facingDir = facingDir * -1;
        transform.Rotate(0, 180,0);
        facingRight = !facingRight;
    }
}
