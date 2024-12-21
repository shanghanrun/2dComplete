using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_InGame : MonoBehaviour
{
    public static UI_InGame instance;
    public UI_FadeEffect fadeEffect; // 외부입력 아니다.

    void Awake()
    {
        instance = this;

        fadeEffect = GetComponentInChildren<UI_FadeEffect>();
    }
    void Start(){
        fadeEffect.ScreenFade(0,1.5f); //불투명(255)에서 불투명(0)으로 1.5초간
    }
}
