using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Player")]
    public GameObject playerPrefab;
    public Transform respawnPoint; //참조형
    public float respawnDelay = 1f;
    public Player player;

    [Header("Fruits ManageMent")]
    public bool areFruitsRandom;
    public int fruitsCollected;
    public int totalFruitsCount;
    // public Fruit[] allFruits;

    [Header("Checkpoint")]
    public Transform checkpoint;
    public bool canBeReActivated = true; //최근에 방문한 체크포인트를 respawn 포인트로 할지 여부

    void Awake(){
        if(instance == null){
            instance = this;
        }
        else Destroy(gameObject);
    }
    void Start(){
        FruitsInfo();
    }

    void FruitsInfo(){
        Fruit[] allFruits = FindObjectsOfType<Fruit>();
        totalFruitsCount = allFruits.Length;
    }

    public void UpdateRespawnPosition(Transform newPoint){
        respawnPoint = newPoint;
    }

    public void RespawnPlayer(){
        StartCoroutine(RespawnRoutine());
    }
    IEnumerator RespawnRoutine(){
        yield return new WaitForSeconds(respawnDelay);
        GameObject newPlayer = Instantiate(playerPrefab, respawnPoint.position, Quaternion.identity);
        player = newPlayer.GetComponent<Player>();
    }

    public void AddFruit() => fruitsCollected++;

    public bool DoFruitsNeedRandomLook()=> areFruitsRandom;
}
