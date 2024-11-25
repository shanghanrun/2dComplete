using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    void Awake(){
        instance = this;
    }

    public void Score(){
        Debug.Log("You have 5 points");
    }
}
