using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    Player player;
    void Awake()
    {
        player = GetComponentInParent<Player>();
    }

    public void FinishRespawn() => player.RespawnFinished(true);
}
