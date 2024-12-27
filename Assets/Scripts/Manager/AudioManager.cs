using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Audio Source")]
    [SerializeField] AudioSource[] sfx;
    [SerializeField] AudioSource[] bgm;
    [SerializeField] int bgmIndex;

    void Awake(){
        DontDestroyOnLoad(this.gameObject);

        if(instance == null){
            instance = this;
        }
        else{
            Destroy(this.gameObject);
        }

        if(bgm.Length <=0) return;

        InvokeRepeating(nameof(PlayMusicIfNeeded), 0, 2);
    }

    public void PlaySFX(int sfxIndex, bool randomPitch =true){
        if(sfxIndex >= sfx.Length) return;

        if(randomPitch) sfx[sfxIndex].pitch = Random.Range(.9f, 1.1f);

        sfx[sfxIndex].Play();
    }

    public void StopSFX(int sfxIndex){
        sfx[sfxIndex].Stop();
    }

    public void PlayBGM(int index){
        if (bgm.Length <= 0) {
            Debug.LogWarning("You have no music audio!");
            return;
        }

        for (int i=0; i< bgm.Length; i++){
            bgm[i].Stop(); // 일단 모든 bgm을 정지시키고
        }

        bgmIndex = index;
        bgm[index].Play();
    }
    public void PlayRandomBGM(){
        bgmIndex = Random.Range(0, bgm.Length);
        PlayBGM(bgmIndex);
    }
    public void PlayMusicIfNeeded(){
        if(bgm[bgmIndex].isPlaying == false){
            PlayRandomBGM();
        }
    }
}
