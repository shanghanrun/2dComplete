using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Credits : MonoBehaviour
{
    [SerializeField] RectTransform rectTr;
    [SerializeField] float scrollSpeed = 100;
    [SerializeField] float offScreenPosY = 1500;

    bool creditsSkipped;

    void Update(){
        rectTr.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;
        GoToMainMenu(); // 여기에도 넣어서, 자동으로 MainMenu뜨게 한다.
    }

    public void SkipCredits(){
        if(!creditsSkipped){
            scrollSpeed *= 10;
            creditsSkipped = true;
        }
        else{
            GoToMainMenu();
        }
    }

    void GoToMainMenu(){
        if(rectTr.position.y > offScreenPosY){
            SceneManager.LoadScene("MainMenu");
        }
    }
}
