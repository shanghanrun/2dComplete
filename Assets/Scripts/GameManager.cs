using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement; // event Action을 사용하기 위해

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Player")]
    public GameObject playerPrefab;
    public Transform respawnPoint; //참조형
    public float respawnDelay = 1f;
    public Player player;
    Transform playerTr;

    [Header("Fruits ManageMent")]
    public bool areFruitsRandom;
    public int fruitsCollected;
    public int totalFruitsCount;
    // public Fruit[] allFruits;

    [Header("Checkpoint")]
    public Transform checkpoint;
    public bool canBeReActivated = true; //최근에 방문한 체크포인트를 respawn 포인트로 할지 여부

    [Header("Traps")]
    public GameObject arrowPrefab;
    public float delay = 1f;

    void Awake(){
        if(instance == null){
            instance = this;
        }
        else Destroy(gameObject);
        playerTr = playerPrefab.transform; //null이 되지 않게 생성되지 마자 초기화
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

    public void CreateObject(GameObject prefab, Transform target){
        StartCoroutine(CreateObjectRoutine(prefab, target));
    }

    IEnumerator CreateObjectRoutine(GameObject prefab, Transform target){
        Vector3 newPos = target.position;
        yield return new WaitForSeconds(delay);
        
        GameObject newObject = Instantiate(prefab, newPos, Quaternion.identity );
    }

    public void LoadTheEndScene() => SceneManager.LoadScene("TheEnd");

    public void LevelFinished(){
        UI_InGame.instance.fadeEffect.ScreenFade(1, 1.5f, LoadTheEndScene); // 투명도1은 어둡게 만드는 것.
    }
}
