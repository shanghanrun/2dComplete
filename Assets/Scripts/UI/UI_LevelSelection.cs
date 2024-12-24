using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_LevelSelection : MonoBehaviour
{
    [SerializeField] UI_LevelButton buttonPrefab;
    [SerializeField] Transform buttonsParentTr;

    [SerializeField] bool[] levelsUnlocked;

    void Start(){
        GetLevelInfo();
        CreateLevelButtons();
    }

    void CreateLevelButtons(){
        int levelsAmount = SceneManager.sceneCountInBuildSettings -1; 

        for(int i=1; i<levelsAmount; i++){ //첫번째 MainMenu빼기 위해 i=1부터
            if(IsLevelUnlocked(i) == false) return;
            
            UI_LevelButton newButton = Instantiate(buttonPrefab, buttonsParentTr);
            newButton.SetupButton(i);
        }
    }

    bool IsLevelUnlocked(int levelIndex) => levelsUnlocked[levelIndex];

    void GetLevelInfo(){
        int levelsAmount = SceneManager.sceneCountInBuildSettings - 1;

        levelsUnlocked = new bool[levelsAmount];
        for(int i=1; i<levelsAmount; i++){
            bool isUnlocked = PlayerPrefs.GetInt("Level"+i+"Unlocked", 0) == 1; // 값이 1인지 여부를 unlockedLevel에 전달
            
            if(isUnlocked){
                levelsUnlocked[i] = true;
            }
        //     else{   // 이 부분을 왜 뺐는지 모르겠다.
        //         levelsUnlocked[i] = false;
        //     }
        }

        levelsUnlocked[1] = true;  // 어떤 경우에라도 level1은 열려 있다.

    }
}
