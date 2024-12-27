using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_InGame : MonoBehaviour
{
    public static UI_InGame instance;
    public UI_FadeEffect fadeEffect {get; set;} // read-only
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] TextMeshProUGUI fruitText; 

    [SerializeField] GameObject pauseUI;

    bool isPaused;

    void Awake()
    {
        instance = this;

        fadeEffect = GetComponentInChildren<UI_FadeEffect>();
    }
    void Start(){
        fadeEffect.ScreenFade(0,1.5f); //불투명(255)에서 불투명(0)으로 1.5초간
    }

    void Update(){
        if(Input.GetKeyDown(KeyCode.P)){
            PauseButton();
        }
    }

    public void PauseButton(){
        if(isPaused){
            isPaused = false;
            Time.timeScale =1;
            if (pauseUI != null){
                pauseUI.SetActive(false);
            }
        }
        else{
            isPaused = true;
            Time.timeScale = 0;
            if(pauseUI !=null){
                pauseUI.SetActive(true);
            }
        }
    }
    public void Resume(){
        isPaused = false;
        Time.timeScale = 1;
        if (pauseUI != null){
            pauseUI.SetActive(false);
        }
        AudioManager.instance.PlaySFX(4);
    }
    public void MainMenu(){
        SceneManager.LoadScene("MainMenu");
        AudioManager.instance.PlaySFX(4);
    }

    public void UpdateFruitUI(int collectedFruits, int totalFruits){
        fruitText.text = collectedFruits + "/" + totalFruits;
    }

    public void UpdateTimerUI(float timer){
        timerText.text = timer.ToString("00") + " s";
    }
}
