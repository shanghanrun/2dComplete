using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]
public struct Skin{
    public string skinName;
    public int skinPrice;
    public bool unlocked;
}

public class UI_SkinSelection : MonoBehaviour
{
    UI_LevelSelection uiLevelSelection;
    UI_MainMenu uiMainMenu;
    [SerializeField] Skin[] skins;

    [Header("UI details")]
    [SerializeField] int currentIndex =0;    
    [SerializeField] Animator skinDisplay;
    public int maxIndex;

    [SerializeField] TextMeshProUGUI priceText;
    [SerializeField] TextMeshProUGUI bankText;
    [SerializeField] TextMeshProUGUI selectText;

    void Awake(){
        maxIndex = skinDisplay.layerCount -1;
    }
    void Start(){
        LoadSkinUnLocked();
        UpdateSkinDisplay();
        
        uiMainMenu = GetComponentInParent<UI_MainMenu>(); //UI_MainMenu는 Canvas에 있다.
        uiLevelSelection = uiMainMenu.GetComponentInChildren<UI_LevelSelection>(true); // 비활성화되었을때 찾기위해
    }

    void LoadSkinUnLocked(){
        for(int i=0; i < skins.Length; i++){
            string skinName = skins[i].skinName;
            bool skinUnlocked = PlayerPrefs.GetInt(skinName +"Unlocked", 0) ==1;

            if(skinUnlocked || i==0){
                skins[i].unlocked = true;
            }
        }
    }

    public void NextSkin(){
        currentIndex++;

        if(currentIndex > maxIndex){
            currentIndex =0;
        }
        UpdateSkinDisplay();
    }

    public void PreviousSkin(){
        currentIndex--;
        if (currentIndex <0){
            currentIndex = maxIndex;
        }

        UpdateSkinDisplay();
    }

    void UpdateSkinDisplay(){
        bankText.text = "Bank: " + FruitsInBank;

        for(int i=0; i< skinDisplay.layerCount; i++){        
            skinDisplay.SetLayerWeight(i,0);
        }

        skinDisplay.SetLayerWeight(currentIndex, 1);

        if(skins[currentIndex].unlocked){
            priceText.transform.parent.gameObject.SetActive(false);
            selectText.text = "Select";
        }
        else{
            priceText.transform.parent.gameObject.SetActive(true);
            priceText.text = "Price: " + skins[currentIndex].skinPrice;
            selectText.text ="Buy";
        }
    }

    public void SelectSkin(){
        if(skins[currentIndex].unlocked == false){
            BuySkin();
        }
        else{
            SkinManager.instance.SetSkinId(currentIndex);
            uiMainMenu.SwitchUI(uiLevelSelection.gameObject); // GameObject가 들어가야 되므로 클래스의 gameObject로 한다.
        }

        UpdateSkinDisplay();
    }

    int FruitsInBank => PlayerPrefs.GetInt("TotalFruitsAmount"); // 읽기 전용 프로퍼티 (get메소드 생략형)  
    // int FruitsInBank() => PlayerPrefs.GetInt("TotalFruitsAmount"); 

    void BuySkin(){
        if(HaveEnoughFruits(skins[currentIndex].skinPrice) == false){
            Debug.Log("Not Enough fruits");
            return;
        }

        string skinName = skins[currentIndex].skinName;
        skins[currentIndex].unlocked = true;

        PlayerPrefs.SetInt(skinName + "Unlocked", 1);
    }

    bool HaveEnoughFruits(int price){
        if( FruitsInBank > price){
            PlayerPrefs.SetInt("TotalFruitsAmount", FruitsInBank - price);
            return true;
        }
        else return false;
    }
}
