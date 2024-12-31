using UnityEngine;
using System.Collections;

public class Fruit_DroppedByPlayer : Fruit
{
    bool canPickup;
    [SerializeField] float[] waitTime;
    [SerializeField] Color transparentColor;
    [SerializeField] Vector2 velocity = new Vector2(1.5f, 0.5f);

    protected override void Start(){
        base.Start();
        StartCoroutine(BlinkRoutine());
    }

    void Update(){
        transform.position += new Vector3(velocity.x, velocity.y) * Time.deltaTime;
    }

    IEnumerator BlinkRoutine(){
        animator.speed = 0;

        foreach(float seconds in waitTime){
            ToggleColorAndSpeed(transparentColor);

            yield return new WaitForSeconds(seconds);

            ToggleColorAndSpeed(Color.white);

            yield return new WaitForSeconds(seconds);
        }

        velocity.x = 0;

        animator.speed = 1;
        canPickup = true;
    }

    void ToggleColorAndSpeed(Color color){
        sr.color = color;
        velocity.x = velocity.x * -1; //좌우로 진동하게
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if(!canPickup) return;

        base.OnTriggerEnter2D(other);
    }
}
