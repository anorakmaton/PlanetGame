using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class CircleManager : MonoBehaviour
{
    [SerializeField]
    SoundManager soundManager;
    [SerializeField]
    AudioClip MergeSE;
    [SerializeField]
    AudioClip DropSE;
    public GameObject DeadLine;
    public GameObject DropLine;
    public int nextCircleSize = 1;
    //prefab格納する配列
    public GameObject[] circlePrefabArray;
    private CircleController circle;
    private CircleController Nextcircle;
    public bool isGameOver = false;
    public static CircleManager instance; // インスタンスの定義
    private int EnableCircleLayer_num = 7;
    private Vector3 mousePosition;
    private float left;
    private float right;
    private float top;
    private float scale = 0.5f;
    private readonly float offset = 0.01f;

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
        //落とす円を生成
        circle = CircleController.Init(nextCircleSize, new Vector3(0, 4.5f, 0), false, false);
        CreateCircle();
        left = GameManager.instance.playAreaWidth / -2;
        right = GameManager.instance.playAreaWidth / 2;
        top = GameManager.instance.playAreaHeight;
    }

    // Update is called once per frame
    void Update()
    {
        // マウスの位置を取得
        Vector3 mousePosition = Input.mousePosition;
        // z軸修正
        mousePosition.z = 10f;
        // ワールド座標に変換
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        // デバッグモードの場合は制限なし
        if (!GameManager.instance.isDebugMode)
        {
            // 座標をプレイエリア内に制限
            worldPosition.x = Mathf.Clamp(worldPosition.x, left + (scale / 2) + offset, right - (scale / 2) - offset);
            worldPosition.y = Mathf.Clamp(worldPosition.y, top, top);
        }
        DropLine.transform.position = worldPosition;
    }

    private void CreateCircle()
    {
        // NextCircleのisNextをfalseにする
        if(Nextcircle != null)
        {
            Nextcircle.isNext = false;
            mousePosition  = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;
            mousePosition.y = GameManager.instance.playAreaHeight;
            mousePosition.x = Mathf.Clamp(mousePosition.x, GameManager.instance.playAreaWidth / -2 + (Nextcircle.scale / 2), GameManager.instance.playAreaWidth / 2 - (Nextcircle.scale / 2));
            Nextcircle.gameObject.transform.position = mousePosition;
            circle = Nextcircle;
        }
        //1-5からランダムに数字を選ぶ
        nextCircleSize = Random.Range(1, 6);
        //NextBoxに表示する
        Nextcircle = CircleController.Init(nextCircleSize, new Vector3(5f, 1.85f, 0), false, true);
        //Nextcircle = CircleController.Init(10, new Vector3(5f, 1.85f, 0), false, true);
        
        // 子オブジェクトにする
        //circle.transform.parent = transform;
        // DropLineを表示する
        DropLine.SetActive(true);
    }

    // 合体するときの処理
    public void MergeCircle(GameObject circle1, GameObject circle2, int size)
    {
        //ゲームオーバーしているときは処理をしない
        if (isGameOver)
        {
            return;
        }
        
        //sizeが11以外の時
        if (size != 11)
        {
            //サウンドを再生
            soundManager.PlaySe(MergeSE);
            circle1.GetComponent<CircleController>().DeleteCircle();
            circle2.GetComponent<CircleController>().DeleteCircle();
            //circle1とcircle2の座標の中間地点を計算
            Vector3 middlePosition = (circle1.transform.position + circle2.transform.position) / 2;
            //新しいオブジェクトを生成
            CircleController circle = CircleController.Init(size+1, middlePosition, true);
            // 子オブジェクトにする
            circle.transform.parent = transform;
            //レイヤーを変更
            StartCoroutine(ChangeLayer(circle.gameObject));
            
            //スコアを加算する
            ScoreManager.instance.AddScore(size);

            if (size == 10)
            {
                ScoreManager.instance.BlackHoleCount++;
            }
        }
        else 
        {
            //サウンドを再生
            soundManager.PlaySe(MergeSE);
            circle1.GetComponent<CircleController>().DeleteCircle();
            circle2.GetComponent<CircleController>().DeleteCircle();
            //スコアを加算する
            ScoreManager.instance.AddScore(size);
        }
        
        
    }   

    public void OnDropCircle(GameObject circle_obj)
    {
        if(isGameOver)
        {
            return;
        }
        scale = Nextcircle.scale;
        //SEを再生
        soundManager.PlaySe(DropSE);
        //DisableDeadLine();
        // 1秒後に新しい円を生成する
        Invoke("CreateCircle", 1f);
        // レイヤーを変更
        StartCoroutine(ChangeLayer(circle_obj));
        // DropLineを非表示にする
        DropLine.SetActive(false);
    }

    public void OnClickArea()
    {
        circle.OnClickArea();
    }
    public void DropCircleWhenGameOver()
    {
        circle.isDropped = true;
        circle.GetComponent<Rigidbody2D>().simulated = true;
    }

    void EnableDeadLine()
    {
        DeadLine.SetActive(true);
    }
    void DisableDeadLine()
    {
        DeadLine.SetActive(false);
    }

    IEnumerator ChangeLayer(GameObject circle_obj)
    {
        yield return new WaitForSeconds(2.0f);
        if(circle_obj != null)
        circle_obj.layer = EnableCircleLayer_num;
    }
}
