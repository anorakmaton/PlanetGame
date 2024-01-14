using System.Collections;
using System.Collections.Generic;
using Mono.Cecil.Cil;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using unityroom.Api;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    SoundManager soundManager;
    [SerializeField]
    AudioClip Bgmclip;
    [SerializeField]
    AudioClip GameOverSE;
    [SerializeField]
    AudioClip GameOverBGM;
    // GameOverを表示するtext
    public GameObject GameOverText;
    public Toggle toggleBGM;
    public GameObject TitleButton;
    public GameObject RetryButton;
    public float playAreaWidth = 6f;
    public float playAreaHeight = 5.5f; //クリック可能な座標範囲
    public bool isDebugMode = false; //デバッグモードかどうか
    
    public static GameManager instance; // インスタンスの定義
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

    
    // Start is called before the first frame update
    void Start()
    {
        //toggleBGMの状態をGameSettingsから取得
        toggleBGM.isOn = GameSettings.instance.isPlayingBGM;
        if(toggleBGM.isOn)
        {
            //BGMを再生
            soundManager.PlayBgm(Bgmclip);
        }
        else
        {
            //BGMを停止
            soundManager.StopBgm();
        }
        //重力を下向きにする
        Physics2D.gravity = new Vector2(0, -9.81f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GameOver()
    {
        //持っている円を落とす
        CircleManager.instance.DropCircleWhenGameOver();
        //isGameOverをtrueにする
        CircleManager.instance.isGameOver = true;
        //重力を上向きにする
        Physics2D.gravity = new Vector2(0, 9.81f);
        //GameOverを表示
        GameOverText.SetActive(true);
        //BGMをストップ
        soundManager.StopBgm();
        //サウンドを再生
        soundManager.PlaySe(GameOverSE);
        soundManager.PlaySe(GameOverBGM);
        
        //2秒後にリトライボタンを表示
        Invoke("ShowButton", 2.0f);
    }

    private void ShowButton()
    {
        //Retryボタンを表示
        RetryButton.SetActive(true);
    }

    public void OnToggledSound()
    {
        if (toggleBGM.isOn)
        {
            //BGMを再生
            soundManager.PlayBgm(Bgmclip);
            GameSettings.instance.isPlayingBGM = true;
        }
        else
        {
            //BGMをミュート
            soundManager.StopBgm();
            GameSettings.instance.isPlayingBGM = false;
        }
    }
}
