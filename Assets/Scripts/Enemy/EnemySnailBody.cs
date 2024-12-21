using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySnailBody : MonoBehaviour
{
    Rigidbody2D rb;
    SpriteRenderer sr;
    float zRotation;

    public void SetupBody(float yVelocity, float zRotate, int facingDir){
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        rb.velocity = new Vector2(rb.velocity.x, yVelocity);
        
        if(facingDir ==1){
            sr.flipX = true;
        }
        this.zRotation = zRotate;
    }

    void Update(){
        transform.Rotate(0,0,zRotation *Time.deltaTime);
    }
}
