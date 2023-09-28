using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class VolumeSlider : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnBgmSliderChanged(float volume)
    {
        SoundManager.instance.SetBgmVolume(volume);
        Debug.Log(volume);
    }

    public void OnSeSliderChanged(float volume)
    {
        SoundManager.instance.SetSeVolume(volume);
        Debug.Log(volume);
    }

    public void SetBGMValue(float value)
    {
        gameObject.GetComponent<Slider>().value = value;
    }

    public void SetSEValue(float value)
    {
        gameObject.GetComponent<Slider>().value = value;
    }
}
