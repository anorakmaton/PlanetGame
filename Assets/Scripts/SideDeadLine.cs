using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideDeadLine : MonoBehaviour
{
    public GameObject GameManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(gameObject.name + "が" + other.gameObject.name + "に衝突しました");
        //親オブジェクトのDeadLineTouchedを呼び出す
        transform.parent.gameObject.GetComponent<DeadLineScript>().DeadLineTouched();
        //無効にする
        //gameObject.SetActive(false);
    }
}
