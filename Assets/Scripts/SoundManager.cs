using UnityEngine;

public class SoundManager : MonoBehaviour
{

    [SerializeField]
    AudioSource bgmAudioSource;
    [SerializeField]
    AudioSource seAudioSource;
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

    GameObject CheckOtherSoundManager()
    {
        return GameObject.FindGameObjectWithTag("SoundManager");
    }

    public void PlayBgm(AudioClip clip)
    {
        bgmAudioSource.clip = clip;

        if(clip == null)
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
        if(clip == null)
        {
            return;
        }

        seAudioSource.PlayOneShot(clip);
    }

    public void SetBgmVolume(float volume)
    {
        bgmAudioSource.volume = volume;
    }

    public void SetSeVolume(float volume)
    {
        seAudioSource.volume = volume;
    }

}
