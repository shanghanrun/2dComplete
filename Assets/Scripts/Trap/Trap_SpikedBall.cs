using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap_SpikedBall : MonoBehaviour
{
    public Rigidbody2D spikedRb;
    public float pushForce;

    void Start(){
        Vector2 pushVector = new Vector2(pushForce, 0);
        spikedRb.AddForce(pushVector, ForceMode2D.Impulse);
    }
}
