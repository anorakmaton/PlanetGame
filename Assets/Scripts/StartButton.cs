using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class StartButton : MonoBehaviour
{
    public SoundManager soundManager;
    public AudioClip StartSE;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnClickStartButton()
    {
        //シーンをロード
        SceneManager.LoadScene("Game");
        //サウンドを再生
        soundManager.PlaySe(StartSE);
    }
}
