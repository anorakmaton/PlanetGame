using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadLineScript : MonoBehaviour
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
        DeadLineTouched();
    }

    public void DeadLineTouched()
    {
        GameManager.GetComponent<GameManager>().GameOver();
        //無効にする
        gameObject.SetActive(false);
    }
}
