using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPoint : MonoBehaviour
{
    Animator animator => GetComponent<Animator>();

    void OnTriggerExit2D(Collider2D other)
    {
        Player player = other.GetComponent<Player>(); //이것에 충돌하는 것은 Player밖에 없다.
        if (player != null)
        {
            animator.SetTrigger("Activate");
        }
    }
}
