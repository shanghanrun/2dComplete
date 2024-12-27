using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPoint : MonoBehaviour
{
    Animator animator =>GetComponent<Animator>();

    void OnTriggerEnter2D(Collider2D other)
    {
        AudioManager.instance.PlaySFX(2); // Finish
        Player player = other.GetComponent<Player>(); //이것에 충돌하는 것은 Player밖에 없다.
        if (player != null){
            animator.SetTrigger("Activate");
            // GameManager.instance.LoadTheEndScene();
            GameManager.instance.LevelFinished();
        }
    }
}
