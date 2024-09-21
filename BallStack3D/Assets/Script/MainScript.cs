
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
public class MainScript : MonoBehaviour
{
    [SerializeField]
    GameObject PlayPanel, LoadingPanel, SettingPannel, ExitPanel;
    [SerializeField]
    float Speed;
    bool Flag;
    [SerializeField]
    Slider SliderObj;
    [SerializeField]
    Button Music, Sound;
    [SerializeField]
    AudioSource MusicSource, SoundSource;
    [SerializeField]
    Sprite MusicOn, SoundOn, MusicOff, SoundOff;


    private void Start()
    {
        if (AudioScript.instance.Music)
        {
            Music.GetComponent<Image>().sprite = MusicOn;
            MusicSource.mute = false;
        }
        else
        {
            Music.GetComponent<Image>().sprite = MusicOff;
            MusicSource.mute = true;
        }

        if (AudioScript.instance.Sound)
        {
            Sound.GetComponent<Image>().sprite = SoundOn;
            SoundSource.mute = false;
        }
        else
        {
            Sound.GetComponent<Image>().sprite = SoundOff;
            SoundSource.mute = true;
        }
    }
    public void PlayBtn()
    {
        PlayPanel.SetActive(false);
        SoundSou();
        LoadingPanel.SetActive(true);
        Flag = true;
    }
    void Update()
    {
        if (Flag == true)
        {
            if (SliderObj.value < SliderObj.maxValue)
            {
                SliderObj.value = SliderObj.value + Speed * Time.deltaTime;
            }
            else
            {
                Flag = false;
                SceneManager.LoadScene(1);
            }
        }

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (SettingPannel.activeInHierarchy)
            {
                SettingPannel.SetActive(false);
                PlayPanel.SetActive(true);
            }
            
            else if (ExitPanel.activeInHierarchy)
            {
                ExitPanel.SetActive(false);
            }
            else
            {
                ExitPanel.SetActive(true);
            }
        }
    }

    public void WrongBtn()
    {
        ExitPanel.SetActive(false);
        PlayPanel.SetActive(true);
        SoundSou();
    }
    public void RightBtn()
    {
        Application.Quit();
    }
    public void SettingBtn()
    {
        SoundSou();
        //PlayPanel.SetActive(false);
        SettingPannel.SetActive(true);
    }
   
    public void CancelSett()
    {
        SettingPannel.SetActive(false);
        SoundSou();
        PlayPanel.SetActive(true);
    }
   

    public void SoundSou()
    {
        SoundSource.Play();
    }

    public void MusicMangemnet()
    {
        if (AudioScript.instance.Music)
        {
            Music.GetComponent<Image>().sprite = MusicOff;
            AudioScript.instance.Music = false;
            MusicSource.mute = true;
        }
        else
        {
            Music.GetComponent<Image>().sprite = MusicOn;
            AudioScript.instance.Music = true;
            MusicSource.mute = false;
        }
    }
    public void SoundMangemnet()
    {
        if (AudioScript.instance.Sound)
        {
            Sound.GetComponent<Image>().sprite = SoundOff;
            AudioScript.instance.Sound = false;
            SoundSource.mute = true;
        }
        else
        {
            Sound.GetComponent<Image>().sprite = SoundOn;
            AudioScript.instance.Sound = true;
            SoundSource.mute = false;
        }
    }
}
