using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinSelection : MonoBehaviour
{
    [SerializeField] int currentIndex =0;    
    [SerializeField] Animator skinDisplay;
    public int maxIndex;

    void Awake(){
        maxIndex = skinDisplay.layerCount -1;
        Debug.Log("currentIndex :"+ currentIndex);
    }

    public void NextSkin(){
        Debug.Log("Next!");
        currentIndex++;
        Debug.Log("index :"+currentIndex);

        if(currentIndex > maxIndex){
            currentIndex =0;
            Debug.Log("index :"+currentIndex);
        }
        UpdateSkinDisplay();
    }

    public void PreviousSkin(){
        Debug.Log("Prev!");
        currentIndex--;
        Debug.Log("index :" + currentIndex);
        if (currentIndex <0){
            currentIndex = maxIndex;
            Debug.Log("index :"+currentIndex);
        }

        UpdateSkinDisplay();
    }

    void UpdateSkinDisplay(){
        for(int i=0; i< skinDisplay.layerCount; i++){        
            skinDisplay.SetLayerWeight(i,0);
        }

        skinDisplay.SetLayerWeight(currentIndex, 1);
    }

    public void SelectSkin(){
        Debug.Log("Select버튼");
        SkinManager.instance.SetSkinId(currentIndex);
    }
}
