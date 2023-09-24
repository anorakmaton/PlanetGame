using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CircleController : MonoBehaviour
{
    // プレイエリアの設定
    float left;
    float right;
    float top;
    private readonly float offset = 0.01f;
    public float scale;
    public bool isDropped = false;
    public bool isNext = false;
    public GameObject Mask;
    private Vector3 worldPosition;
    private CircleManager circleManager;
    public int Size { get; set;}
    public bool isTimePassed = false;
    
    public static CircleController Init(int size, Vector3 position, bool isDropped, bool isNext = false)
    {
        GameObject myCircle = Resources.Load<GameObject>("CircleSize" + size);
        CircleController circleController = Instantiate(myCircle).GetComponent<CircleController>();
        ////初期値を設定する
        //サイズ
        circleController.Size = size;
        if(isDropped)
        {
            //落下フラグ
            circleController.isDropped = true;
            // Rigidbodyのシミュレーションを有効にする
            circleController.GetComponent<Rigidbody2D>().simulated = true;
        }
        //CircleManagerを親オブジェクトにする
        circleController.transform.parent = GameObject.Find("CircleManager").transform;
        //座標
        circleController.transform.position = position;
        circleController.isNext = isNext;
        return circleController;
    }

    void Start()
    {
        //ステージのサイズを取得
        left = GameManager.instance.playAreaWidth / -2;
        right = GameManager.instance.playAreaWidth / 2;
        top = GameManager.instance.playAreaHeight;
        //タグ名からサイズを取得
        Size = int.Parse(tag);
        //マスクのスケールを0にする
        Mask.transform.localScale = new Vector3(0, 0, 0);
        //親オブジェクトの"CircleManager"を取得
        circleManager = transform.parent.gameObject.GetComponent<CircleManager>();
        //自身のスケールのxを取得
        scale = transform.localScale.x;
        // Rigidbodyのシミュレーションを無効にする
        GetComponent<Rigidbody2D>().simulated = false;

        //落下している場合は
        if (isDropped)
        {
            //生成されてから0.1秒後にRigidBodyのシミュレーションを有効にする
            Invoke("EnableRigidBody", 0.1f);
        }
        //落下していないかつNextでない場合は初期位置に設定する
        else if (!isNext)
        {
            worldPosition = new Vector3(0, 4.5f, 0);
            transform.position = worldPosition;
            ////マウスでの操作
            //マウスの位置を取得
            // Vector3 mousePosition = Input.mousePosition;
            // //z軸修正
            // mousePosition.z = 10f;
            // //ワールド座標に変換
            // worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            // // 座標をプレイエリア内に制限
            // worldPosition.x = Mathf.Clamp(worldPosition.x, left + (scale / 2) + offset, right - (scale / 2) - offset);
            // worldPosition.y = Mathf.Clamp(worldPosition.y, top, top);
            // transform.position = worldPosition;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDropped && !isNext) {
            ////マウスでの操作
            // マウスの位置を取得
            Vector3 mousePosition = Input.mousePosition;
            // z軸修正
            mousePosition.z = 10f;
            // ワールド座標に変換
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            // 座標をプレイエリア内に制限
            worldPosition.x = Mathf.Clamp(worldPosition.x, left + (scale / 2) + offset, right - (scale / 2) -  offset);
            worldPosition.y = Mathf.Clamp(worldPosition.y, top, top);
            transform.position = worldPosition;

            // 左クリックされたら
            /*
            if (Input.GetMouseButtonDown(0))
            {
                isDropped = true;
                GetComponent<Rigidbody2D>().simulated = true;
                circleManager.OnDropCircle(gameObject);
            }
            */
            /*
            // キー操作
            // Aキーが押されているとき
            if(Input.GetKey(KeyCode.A))
            {
                worldPosition.x -= 0.05f;
            }
            // Dキーが押されているとき
            else if(Input.GetKey(KeyCode.D))
            {
                worldPosition.x += 0.05f;
            }
    
            // 座標をプレイエリア内に制限
            worldPosition.x = Mathf.Clamp(worldPosition.x, left + (scale / 2) + offset, right - (scale / 2) -  offset);
            worldPosition.y = Mathf.Clamp(worldPosition.y, top, top);
            transform.position = worldPosition;

            // スペースキーが押されたら
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isDropped = true;
                GetComponent<Rigidbody2D>().simulated = true;
                circleManager.OnDropCircle(this.gameObject);
            }
            */
        }
        
    }

    public void OnClickArea()
    {
        if (!isDropped && !isNext)
        {
            isDropped = true;
            GetComponent<Rigidbody2D>().simulated = true;
            circleManager.OnDropCircle(gameObject);
        }
    }

    void EnableRigidBody()
    {
        // Rigidbodyのシミュレーションを有効にする
        GetComponent<Rigidbody2D>().simulated = true;
    }

    // 衝突判定
    void OnCollisionEnter2D(Collision2D collision)
    {
        // 衝突相手が違う大きさの場合は何もしない
        if (!gameObject.CompareTag(collision.gameObject.tag))
        {
            return;
        }
        //衝突相手がすでにほかのオブジェクトと接触している場合は何もしない
        if (collision.gameObject.CompareTag("Touched"))
        {
            return;
        }

        // 自身と衝突相手のタグを"Touched"に変更
        collision.gameObject.tag = "Touched";
        gameObject.tag = "Touched";
        //同じ大きさの円と衝突した場合の処理
        circleManager.MergeCircle(transform.gameObject, collision.gameObject, Size);
    }

    // マスクを徐々に大きくするコルーチン
    private IEnumerator UpdateMask()
    {
        float delta = transform.localScale.x / 10;
        while (Mask.transform.localScale.x < transform.localScale.x)
        {
            yield return new WaitForSeconds(0.002f);
            // マスクのスケールを徐々に大きくする
            Mask.transform.localScale += new Vector3(delta, delta, 0);
        }
    }
    // マスクを徐々に大きくしてからオブジェクトを削除するコルーチン
    public IEnumerator StartDeleteCircle()
    {
        yield return StartCoroutine(UpdateMask());
        Destroy(gameObject);
    }
    //コルーチンを開始する
    public void DeleteCircle()
    {
        //シミュレーションを無効にする
        GetComponent<Rigidbody2D>().simulated = false;
        StartCoroutine(StartDeleteCircle());
    }
}
