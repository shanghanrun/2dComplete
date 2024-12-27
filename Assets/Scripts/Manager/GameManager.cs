using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement; // event Action을 사용하기 위해

public class GameManager : MonoBehaviour
{
    public static GameManager instance;    

    public UI_InGame uiInGame;

    [Header("Level Management")]
    [SerializeField] int currentLevelIndex;
    int nextLevelIndex;
    [SerializeField] float levelTimer;

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
    public Transform checkPoint;
    public bool canBeReActivated = true; //최근에 방문한 체크포인트를 respawn 포인트로 할지 여부

    [Header("Traps")]
    public GameObject arrowPrefab;
    public float delay = 1f;

    [Header("Managers")]
    [SerializeField] AudioManager audioManager; // 프리팹에서 넣어준다.

    void Awake(){
        if(instance == null){
            instance = this;
        }
        else Destroy(gameObject);
        playerTr = playerPrefab.transform; //null이 되지 않게 생성되지 마자 초기화
        // uiInGame = UI_InGame.instance;
    }
    void Start(){
        uiInGame = UI_InGame.instance; // uiInGame인스턴스가 스스로의 Awake에서 생성된 이후에 찾기
        currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        if(currentLevelIndex < (SceneManager.sceneCountInBuildSettings - 2)){
            nextLevelIndex = currentLevelIndex + 1;
        }
        else{
            nextLevelIndex = currentLevelIndex; // 더이상 증가하지 않도록
        }

        if(respawnPoint == null){
            respawnPoint = FindFirstObjectByType<StartPoint>()?.transform;
        }        
        if(checkPoint == null){
            checkPoint = FindFirstObjectByType<Checkpoint>()?.transform;
        }        
        if(player == null){
            player = FindFirstObjectByType<Player>();
        }        
        
        FruitsInfo();
        CreateManagerIfNeeded();
    }

    private object FindObjectByType<T>()
    {
        throw new NotImplementedException();
    }

    void Update(){
        levelTimer += Time.deltaTime;
        uiInGame.UpdateTimerUI(levelTimer);
    }

    void CreateManagerIfNeeded(){
        if(AudioManager.instance == null){
            Instantiate(audioManager);
        }
    }

    void FruitsInfo(){
        // Fruit[] allFruits = FindObjectsOfType<Fruit>(); deprecated
        Fruit[] allFruits = FindObjectsByType<Fruit>(FindObjectsSortMode.None);
        totalFruitsCount = allFruits.Length;
        uiInGame.UpdateFruitUI(fruitsCollected, totalFruitsCount);

        PlayerPrefs.SetInt("Level" + currentLevelIndex + "TotalFruits", totalFruitsCount);
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

    public void AddFruit() {
        fruitsCollected++;
        uiInGame.UpdateFruitUI(fruitsCollected, totalFruitsCount);
    }
    public void RemoveFruit(){
        fruitsCollected--;
        uiInGame.UpdateFruitUI(fruitsCollected, totalFruitsCount);
    }
    public int FruitsCollected => fruitsCollected;

    public bool DoFruitsNeedRandomLook()=> areFruitsRandom;

    public void CreateObject(GameObject prefab, Transform target){
        StartCoroutine(CreateObjectRoutine(prefab, target));
    }

    IEnumerator CreateObjectRoutine(GameObject prefab, Transform target){
        Vector3 newPos = target.position;
        yield return new WaitForSeconds(delay);
        
        GameObject newObject = Instantiate(prefab, newPos, Quaternion.identity );
    }

    void LoadCurrentScene() => SceneManager.LoadScene("Level_" + currentLevelIndex);
    void LoadTheEndScene() => SceneManager.LoadScene("TheEnd");
    void LoadNextScene() => SceneManager.LoadScene("Level_" + nextLevelIndex);

    public void RestartLevel()
    {
        UI_InGame.instance.fadeEffect.ScreenFade(1, .75f, LoadCurrentScene);
    }

    public void LevelFinished(){
        PlayerPrefs.SetInt("Level" + nextLevelIndex +"Unlocked", 1); // "Level2Unlocked" 변수에 1을 넣음.
        PlayerPrefs.SetInt("ContinueLevelNumber", nextLevelIndex);

        // SkinManager는 MainMenu씬에서만 있다. 일반 Level에는 없다.
        SkinManager skinManager = SkinManager.instance;
        if(skinManager != null){ // 일반 레벨에서 LevelFinished 가 될 때는 이것이 실행되지 않도록 해야 에러안난다.
            PlayerPrefs.SetInt("LastUsedSkin", skinManager.GetSkinId());
        }

        SaveBestTime();
        SaveFruitsInfo();
        
        UI_FadeEffect fadeEffect = uiInGame.fadeEffect;

        int lastLevelIndex = SceneManager.sceneCountInBuildSettings -2;
        bool noMoreLevels = currentLevelIndex == lastLevelIndex;

        if(noMoreLevels){
            fadeEffect.ScreenFade(1, 1.5f, LoadTheEndScene);
        }
        else{
            fadeEffect.ScreenFade(1, 1.5f, LoadNextScene);
        }
    }

    void SaveBestTime(){
        float lastTime = PlayerPrefs.GetFloat("Level" + currentLevelIndex + "BestTime", 99);
        
        if(levelTimer < lastTime){
            PlayerPrefs.SetFloat("Level" + currentLevelIndex + "BestTime", levelTimer);
        }
    }
    
    void SaveFruitsInfo(){
        PlayerPrefs.SetInt("Level" + currentLevelIndex + "FruitsCollected", fruitsCollected);

        int fruitsCollectedBefore = PlayerPrefs.GetInt("Level" + currentLevelIndex + "FruitsCollected");

        if(fruitsCollectedBefore < fruitsCollected){ // 나중의 것이 크면 나중것으로
            PlayerPrefs.SetInt("Level" + currentLevelIndex + "FruitsCollected", fruitsCollected);
        }

        int totalFruitsInBank = PlayerPrefs.GetInt("TotalFruitsAmount");
        PlayerPrefs.SetInt("TotalFruitsAmount", totalFruitsInBank + fruitsCollected);

    }
}
