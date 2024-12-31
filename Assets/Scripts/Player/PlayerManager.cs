using UnityEngine;
using System;
using System.Collections;

public class PlayerManager : MonoBehaviour
{
    public static event Action OnPlayerRespawn;
    public static PlayerManager instance;

    [Header("Player")]
    public GameObject playerPrefab;
    public Transform respawnPoint; //참조형
    public float respawnDelay = 1f;
    public Player player;
    Transform playerTr;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else Destroy(gameObject);
        playerTr = playerPrefab.transform; //null이 되지 않게 생성되지 마자 초기화
    }
    void Start(){
        if (respawnPoint == null)
        {
            respawnPoint = FindFirstObjectByType<StartPoint>()?.transform;
        }
        if (player == null)
        {
            player = FindFirstObjectByType<Player>();
        }
    }

    public void UpdateRespawnPosition(Transform newPoint)
    {
        respawnPoint = newPoint;
    }

    public void RespawnPlayer()
    {
        StartCoroutine(RespawnRoutine());
    }

    IEnumerator RespawnRoutine()
    {
        yield return new WaitForSeconds(respawnDelay);
        GameObject newPlayer = Instantiate(playerPrefab, respawnPoint.position, Quaternion.identity);
        player = newPlayer.GetComponent<Player>();
        OnPlayerRespawn?.Invoke();
    }
}
