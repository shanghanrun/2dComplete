using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class UI_Settings : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] float mixerMultiplier = 25;

    [Header("SFX settings")]
    [SerializeField] Slider sfxSlider;
    [SerializeField] TextMeshProUGUI sfxSliderText;
    [SerializeField] string sfxParameter;

    [Header("BGM settings")]
    [SerializeField] Slider bgmSlider;
    [SerializeField] TextMeshProUGUI bgmSliderText;
    [SerializeField] string bgmParameter;

    public void SFXSliderValue(float value){
        sfxSliderText.text = Mathf.RoundToInt(value *100) + " %";
        float newValue = Mathf.Log10(value) * mixerMultiplier;
        audioMixer.SetFloat(sfxParameter, newValue);
    }
    public void BGMSliderValue(float value){
        bgmSliderText.text = Mathf.RoundToInt(value * 100) + " %";
        float newValue = Mathf.Log10(value) * mixerMultiplier;
        audioMixer.SetFloat(bgmParameter, newValue);
    }

    void OnDisable(){
        PlayerPrefs.SetFloat(sfxParameter, sfxSlider.value);
        PlayerPrefs.SetFloat(bgmParameter, bgmSlider.value);
    }
    void OnEnable(){
        sfxSlider.value = PlayerPrefs.GetFloat(sfxParameter, 0.7f);
        bgmSlider.value = PlayerPrefs.GetFloat(bgmParameter, 0.3f);
    }
}
