using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_LevelButton : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI levelNumberText;
    [SerializeField] TextMeshProUGUI bestTimeText;
    [SerializeField] TextMeshProUGUI fruitText;
    public string sceneName; 
    int levelIndex;

    public void SetupButton(int newLevelIndex){
        levelIndex = newLevelIndex;
        levelNumberText.text = "Level " + levelIndex; 

        sceneName = "Level_" + levelIndex;

        bestTimeText.text = TimerInfoText();
        fruitText.text = FruitsInfoText();
    }

    public void LoadLevel(){
        SceneManager.LoadScene(sceneName);
    }

    string TimerInfoText(){
        float timerValue = PlayerPrefs.GetFloat("Level" + levelIndex + "BestTime", 99);
        return "Best Time: " + timerValue.ToString("00") + " s";
    }
    string FruitsInfoText(){
        int totalFruits = PlayerPrefs.GetInt("Level" + levelIndex + "TotalFruits", 0);
        string totalFruitsText = totalFruits == 0 ? "?" : totalFruits.ToString();

        int fruitsCollected = PlayerPrefs.GetInt("Level" + levelIndex + "FruitsCollected", 0);
        
        return "Fruits: " + fruitsCollected + "/" + totalFruitsText;
    }
}
