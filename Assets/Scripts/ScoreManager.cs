using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using unityroom.Api;

public class ScoreManager : MonoBehaviour
{
    public GameObject ScoreText;
    public GameObject BestScoreText;
    public int score = 0;
    public int BlackHoleCount = 0;
    public int[] scoreList = new int[12] { 1, 3, 6, 10, 15, 21, 28, 36, 45, 55, 66 , 78};
    public static ScoreManager instance; // インスタンスの定義

    private void Awake()
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
        ScoreText.GetComponent<Text>().text = "0";
        BestScoreText.GetComponent<Text>().text = GameSettings.instance.BestScore.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddScore(int size)
    {
        score += scoreList[size - 1];
        ScoreText.GetComponent<Text>().text = score.ToString();
        if(GameSettings.instance.BestScore < score)
        {
            GameSettings.instance.BestScore = score;
        }
       
        //ランキングを送信
        //総合ランキング
        UnityroomApiClient.Instance.SendScore(1, this.score, ScoreboardWriteMode.HighScoreDesc);
        //ブラックホールランキング
        if (this.BlackHoleCount > 0)
        {
            UnityroomApiClient.Instance.SendScore(3, this.BlackHoleCount, ScoreboardWriteMode.HighScoreDesc);
        }
    }
}
