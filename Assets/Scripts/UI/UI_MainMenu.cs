using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_MainMenu : MonoBehaviour //Canvas에 어태치한다.
{
    public string startSceneName;
    UI_FadeEffect fadeEffect;

    [SerializeField] GameObject[] uiElements;
    [SerializeField] GameObject continueButton;

    void Awake(){
        fadeEffect = GetComponentInChildren<UI_FadeEffect>(); //자식 FadeImage에 어태치되었다.
    }

    void Start(){
        if(HasLevelProgression()){
            continueButton.SetActive(true);
        }

        fadeEffect.ScreenFade(0, 3f); // 검정색의 투명도를 0으로 만들면서 fadeIn
    }

    public void SwitchUI(GameObject uiToEnable){
        //일단 모든 ui엘리먼트를 비활성화
        foreach(GameObject ui in uiElements){
            ui.SetActive(false);
        }

        uiToEnable.SetActive(true);
    }

    public void NewGame(){
        fadeEffect.ScreenFade(50, 1.5f, LoadLevelScene);
    }

    void LoadLevelScene()=> SceneManager.LoadScene(startSceneName);

    bool HasLevelProgression(){
        bool hasLevelProgression = PlayerPrefs.GetInt("ContinueLevelNumber", 0) > 0;
        return hasLevelProgression;
    }

    public void ContinueGame(){
        int levelIndex = PlayerPrefs.GetInt("ContinueLevelNumber", 0);
        SceneManager.LoadScene("Level_" + levelIndex);
    }
}
