using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_FadeEffect : MonoBehaviour // FadeImage 오브젝트에 어태치
{
    [SerializeField] Image fadeImage;  // 외부에서 FadeImage게임오브젝트(Image 오브젝트)를 넣어준다.

    public void ScreenFade(float targetAlpha, float duration, System.Action onComplete=null)
    {
        StartCoroutine(Fade(targetAlpha, duration, onComplete));
    }

    IEnumerator Fade(float targetAlpha, float duration, System.Action onComplete){

        float time =0;
        Color currentColor = fadeImage.color;

        float startAlpha = currentColor.a;

        while(time < duration){
            time += Time.deltaTime;

            float alpha = Mathf.Lerp(startAlpha, targetAlpha, time/duration);

            fadeImage.color = new Color(currentColor.r, currentColor.g,currentColor.b,alpha);
            yield return null; //다음프레임에 바로 실행되도록
        }

        fadeImage.color = new Color(currentColor.r,currentColor.g,currentColor.b,targetAlpha); //사실 없어도 된다.

        onComplete?.Invoke();
    }
}
