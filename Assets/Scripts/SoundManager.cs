using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class SoundManager : MonoBehaviour
{

    [SerializeField]
    AudioSource bgmAudioSource;
    [SerializeField]
    AudioSource seAudioSource;
    [SerializeField]
    Slider BGMVolumeSlider;
    [SerializeField]
    Slider SEVolumeSlider;
    public float bgmVolume;
    public float seVolume;
    public static SoundManager instance; // インスタンスの定義
    public float BgmVolume
    {
        get
        {
            return bgmAudioSource.volume;
        }
        set
        {
            bgmAudioSource.volume = Mathf.Clamp01(value);
        }
    }

    public float SeVolume
    {
        get
        {
            return seAudioSource.volume;
        }
        set
        {
            seAudioSource.volume = Mathf.Clamp01(value);
        }
    }

    void Awake()
    {
        // シングルトンの呪文
        if (instance == null)
        {
            // 自身をインスタンスとする
            instance = this;
        }
        else
        {
            // インスタンスが複数存在しないように、既に存在していたら自身を消去する
            Destroy(gameObject);
        }
    }

    void Start()
    {
        //BGMの音量をGameSettingsから取得
        bgmVolume = GameSettings.instance.BGMVolume;
        bgmAudioSource.volume = bgmVolume;
        BGMVolumeSlider.gameObject.GetComponent<VolumeSlider>().SetBGMValue(bgmVolume);
        //SEの音量をGameSettingsから取得
        seVolume = GameSettings.instance.SEVolume;
        seAudioSource.volume = seVolume;
        SEVolumeSlider.gameObject.GetComponent<VolumeSlider>().SetSEValue(seVolume);
    }

    GameObject CheckOtherSoundManager()
    {
        return GameObject.FindGameObjectWithTag("SoundManager");
    }

    public void PlayBgm(AudioClip clip)
    {
        bgmAudioSource.clip = clip;

        if (clip == null)
        {
            return;
        }

        bgmAudioSource.Play();
    }

    public void StopBgm()
    {
        bgmAudioSource.Stop();
    }

    public void PlaySe(AudioClip clip)
    {
        if (clip == null)
        {
            return;
        }

        seAudioSource.PlayOneShot(clip);
    }

    public void SetBgmVolume(float bgmvolume)
    {
        bgmAudioSource.volume = bgmvolume;
        GameSettings.instance.BGMVolume = bgmvolume;
    }

    public void SetSeVolume(float sevolume)
    {
        seAudioSource.volume = sevolume;
        GameSettings.instance.SEVolume = sevolume;
    }

}
