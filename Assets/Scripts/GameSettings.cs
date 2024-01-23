using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    //toggleBGMの状態を保持する変数
    public bool isPlayingBGM = true;
    public static GameSettings instance; // インスタンスの定義
    public int BestScore = 0;
    public float BGMVolume = 0.3f;
    public float SEVolume = 0.3f;
    void Awake()
    {
        // シングルトンの呪文
        if (instance == null)
        {
            // 自身をインスタンスとする
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // インスタンスが複数存在しないように、既に存在していたら自身を消去する
            Destroy(gameObject);
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
