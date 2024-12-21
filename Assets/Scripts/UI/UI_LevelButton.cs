using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_LevelButton : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI levelNumberText;
    public string sceneName; 
    int levelIndex;

    public void SetupButton(int newLevelIndex){
        levelIndex = newLevelIndex;
        levelNumberText.text = "Level " + levelIndex; 

        sceneName = "Level_" + levelIndex;
    }

    public void LoadLevel(){
        SceneManager.LoadScene(sceneName);
    }
}
