using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DifficultyType{ Easy=1, Normal, Hard} // 원래는 0부터인데, 이렇게 하면 1부터

public class DifficultyManager : MonoBehaviour
{
    public static DifficultyManager instance;

    public DifficultyType difficulty;

    void Awake(){
        DontDestroyOnLoad(this.gameObject);

        if(instance == null){
            instance = this;
        }
        else{
            Destroy(gameObject);
        }
    }

    public void SetDifficulty(DifficultyType newDifficulty)=> difficulty = newDifficulty;

    public void LoadDifficulty(int difficultyIndex){   // 인덱스가 아닌 enum타입으로, difficulty에 저장하기
        difficulty = (DifficultyType)difficultyIndex;
    }
}
