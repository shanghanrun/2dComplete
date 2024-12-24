using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Difficulty : MonoBehaviour
{
    DifficultyManager difficultyManager;

    void Start(){
        difficultyManager = DifficultyManager.instance;
    }

    public void SetEasyMode() => difficultyManager.SetDifficulty(DifficultyType.Easy);
    public void SetNormalMode() => difficultyManager.SetDifficulty(DifficultyType.Normal);
    public void SetHardMode() => difficultyManager.SetDifficulty(DifficultyType.Hard);

}
