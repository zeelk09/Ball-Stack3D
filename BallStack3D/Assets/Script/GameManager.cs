
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    [SerializeField]
    List<GameObject> AllTube;
    [SerializeField]
    List<GameObject> SelectedTube;
    [SerializeField]
    GameObject BAll;
    [SerializeField]
    List<Color> BAllColors;
    [SerializeField]
    List<Color> SelectedColour,ChoiceColour;
    bool Flag;
    [SerializeField]
    List<string> AllTag;
    [SerializeField]
    GameObject WinPanel,SettingPanel;
    [SerializeField]
    List<GameObject> AllfillTube;
    [SerializeField]
    GameObject Tube;
    bool IsWin;
    int noOfTube = 0;
    [SerializeField]
    List<GameObject> Paren;
    int win=0;
    [SerializeField]
    Button Music, Sound;
    [SerializeField]
    AudioSource MusicSource, SoundSource;
    [SerializeField]
    Sprite MusicOn, SoundOn, MusicOff, SoundOff;
    public static GameManager instance;
    int counter = 0;
    private void Start()
    {
        instance = this;
        TubeIns();
        TubeSelect();
 


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

    void TubeIns()
    {
        int counter = PlayerPrefs.GetInt("counter", 0);
        Debug.Log("counter:=" + counter);
        ; for (int i = 0; i < Paren.Count; i++)
        {
            if (counter == i)
            {
                Paren[i].SetActive(true);
            }
            else
            {
                Paren[i].SetActive(false);
            }
        }
        noOfTube = PlayerPrefs.GetInt("level" + counter, 0);
        Debug.Log("no of tube:=" + noOfTube);
        if (noOfTube < 5)
        {
            for (int i = 0; i <= Paren[counter].transform.childCount - 1; i++)
            {
                GameObject GG = Instantiate(Tube, Paren[counter].transform.GetChild(i).position, Quaternion.identity);
                AllTube.Add(GG);
                GG.name = "level" + i;
                //LevelInc();
            }
        }
        else
        {
            counter++;
            PlayerPrefs.SetInt("counter", counter);
            TubeIns();
        }

    }
    public void ScenLod()
    {
        SceneManager.LoadScene(1);
    }
    //void LevelInc()
    //{
    //    if(win==2)
    //    {
    //        noOfTube = noOfTube + 1;
    //        PlayerPrefs.SetInt("level",noOfTube);
    //        Debug.Log("tube"+noOfTube);
    //    }
    //}
    void TubeSelect()
    {
        int random;
        for(int i=0;i<3;i++)
        {
            do
            {
                random =Random.Range(0,AllTube.Count);

            } while (SelectedTube.Contains(AllTube[random]));
            SelectedTube.Add(AllTube[random]);
        }
        
        ColourSelected();
        colourset();
        SetBall();
    }
    void SetBall()
    {
       
        for (int i=0;i<SelectedTube.Count; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                int val = Random.Range(0,ChoiceColour.Count);
                GameObject NewBAll=Instantiate(BAll, SelectedTube[i].gameObject.transform.GetChild(j).position, Quaternion.identity, SelectedTube[i].gameObject.transform);
                NewBAll.GetComponent<MeshRenderer>().material.color = ChoiceColour[val];
                for(int i1=0;i1<SelectedColour.Count;i1++)
                {
                    if (SelectedColour[i1] == ChoiceColour[val])
                    {
                        NewBAll.gameObject.tag = AllTag[i1];
                    }
                }
                ChoiceColour.Remove(ChoiceColour[val]);
            }
        }
    }
    

    void ColourSelected()
    {
        int random;
        for (int i = 0; i < 3; i++)
        {
            do
            {
                random = Random.Range(0, BAllColors.Count);

            } while (SelectedColour.Contains(BAllColors[random]));
            SelectedColour.Add(BAllColors[random]);
            
        }
    }
    void colourset()
    {
        for (int i = 0; i < SelectedTube.Count; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                ChoiceColour.Add(SelectedColour[i]);
            }
        }
    }
    GameObject FirstObj, SceondObj;
    public void TubeLogic(GameObject CliclObject)
    {
        if(!Flag)
        {
            if(CliclObject.transform.childCount>5)
            {
                FirstObj= CliclObject;
                FirstObj.transform.GetChild(FirstObj.transform.childCount-1).DOMove(FirstObj.transform.GetChild(4).position,0.3f);
                Flag = true;
            }
        }
        else
        {
            SceondObj = CliclObject;
            Flag = false;
            // first clicl and second click at same tube
            if(FirstObj.name==SceondObj.name)
            {
                SoundSource.Play();
                FirstObj.transform.GetChild(FirstObj.transform.childCount - 1).DOMove(SceondObj.transform.GetChild(SceondObj.transform.childCount - 6).transform.position, 0.3f).SetEase(Ease.InOutElastic);
            }
            // the tube is full
            else if(SceondObj.transform.childCount == 9)
            {
                SoundSource.Play();
                FirstObj.transform.GetChild(FirstObj.transform.childCount - 1).DOMove(FirstObj.transform.GetChild(FirstObj.transform.childCount - 6).transform.position, 0.3f).SetEase(Ease.InOutElastic);
            }
            // the tube is empty
            else if(SceondObj.transform.childCount == 5)
            {
                SoundSource.Play();
                FirstObj.transform.GetChild(FirstObj.transform.childCount-1).parent = SceondObj.transform;
                SceondObj.transform.GetChild(SceondObj.transform.childCount - 1).DOMove( SceondObj.transform.GetChild(4).transform.position, 0.3f).OnComplete
                (() => SceondObj.transform.GetChild(SceondObj.transform.childCount -1).DOMove(SceondObj.transform.GetChild(0).transform.position,0.2f).SetEase(Ease.InOutElastic));
            }
            // having same tag
            else if(FirstObj.transform.GetChild(FirstObj.transform.childCount - 1).tag== SceondObj.transform.GetChild(SceondObj.transform.childCount - 1).tag)
            {
                SoundSource.Play();
                FirstObj.transform.GetChild(FirstObj.transform.childCount - 1).parent = SceondObj.transform;
               //SceondObj.transform.GetChild(SceondObj.transform.childCount - 1).position = SceondObj.transform.GetChild(SceondObj.transform.childCount - 6).transform.position;
                SceondObj.transform.GetChild(SceondObj.transform.childCount - 1).DOMove(SceondObj.transform.GetChild(4).transform.position, 0.3f).OnComplete
                (() => SceondObj.transform.GetChild(SceondObj.transform.childCount - 1).DOMove(SceondObj.transform.GetChild(SceondObj.transform.childCount-6).transform.position, 0.2f).SetEase(Ease.InOutElastic));

            }
            // having different tag
            else
            {
                SoundSource.Play();
                FirstObj.transform.GetChild(FirstObj.transform.childCount - 1).DOMove( FirstObj.transform.GetChild(FirstObj.transform.childCount - 6).transform.position,0.3f).SetEase(Ease.InOutElastic);
            }
            Win();
            //LevelInc();

        }

    }
   void Win()
    {
        AllfillTube.Clear();
        foreach(GameObject AllTubes in AllTube)
        {
            if (AllTubes.transform.childCount==9)
            {
                AllfillTube.Add(AllTubes);
            }
        }
        if(AllfillTube.Count ==AllTube.Count-2 )
        {
            foreach(GameObject tubeObj in AllfillTube)
            {
                for(int i=5;i<=8;i++)
                {
                    if(tubeObj.transform.GetChild(5).gameObject.tag!=tubeObj.transform.GetChild(i).gameObject.tag)
                    {
                        IsWin= true;
                        break;
                    }
                    else
                    {
                        IsWin = false;
                    }
                }
            }

        }
        else
        {
            IsWin = true;
        }
        if(!IsWin)
        {
            win = win + 1;
            noOfTube = PlayerPrefs.GetInt("level" + counter, 0);
            noOfTube++;
            PlayerPrefs.SetInt("level"+counter, noOfTube);
            WinPanel.SetActive(true);
            Debug.Log("Winnn");
        }
    }
    public void SettingPanelOn()
    {
        Debug.Log("SettingOn");
        SettingPanel.SetActive(true);
    }

    public void CancelBtn()
    {
        SettingPanel.SetActive(false); 
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
    public void BackBtnClickAction()
    {
        SceneManager.LoadScene(0);
    }
}
