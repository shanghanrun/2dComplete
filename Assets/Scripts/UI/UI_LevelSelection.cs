using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_LevelSelection : MonoBehaviour
{
    [SerializeField] UI_LevelButton buttonPrefab;
    [SerializeField] Transform buttonsParentTr;

    void Start(){
        CreateLevelButtons();
    }

    void CreateLevelButtons(){
        int levelsAmount = SceneManager.sceneCountInBuildSettings -1; 

        for(int i=1; i<levelsAmount; i++){ //첫번째 MainMenu빼기 위해 i=1부터
            UI_LevelButton newButton = Instantiate(buttonPrefab, buttonsParentTr);
            newButton.SetupButton(i);
        }
    }
}
