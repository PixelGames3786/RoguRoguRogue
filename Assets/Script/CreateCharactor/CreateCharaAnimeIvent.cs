using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CreateCharaAnimeIvent : MonoBehaviour
{

    private GameObject Camera,SceneChange;

    public GameObject SelectMask,SceneChangeCanvas,SceneChangePrefab;
    public GameObject[] Prefabs;

    public int SelectType;

    // Start is called before the first frame update
    void Start()
    {
        Camera = GameObject.Find("Main Camera");
    }

    void Update()
    {
        if (SceneChange&&SceneChange.transform.GetChild(0).GetComponent<Rigidbody2D>().IsSleeping())
        {
            PlayerPrefs.SetInt("NowPlaying",1);

            SceneManager.LoadScene("Town");
        }
    }


    public void ParaToTips()
    {
        if (!CreateCharactorDaisu.NowDaisuFlag && !CreateCharactorManeger.StopFlag)
        {
            AllAudioManege.PlaySE(0);

            Camera.GetComponent<Animator>().SetBool("IdouFlag", true);
        }
    }

    public void TipsToPara()
    {
        if (!CreateCharactorDaisu.NowDaisuFlag && !CreateCharactorManeger.StopFlag)
        {
            AllAudioManege.PlaySE(0);

            Camera.GetComponent<Animator>().SetBool("IdouFlag", false);
        }
        
    }

    public void ParaToSelect(int value)
    {
        if (!CreateCharactorDaisu.NowDaisuFlag&&!CreateCharactorManeger.StopFlag)
        {
            AllAudioManege.PlaySE(0);

            Camera.GetComponent<Animator>().SetBool("IdouFlag2", true);

            if (SelectMask.transform.childCount>0)
            {
                Destroy(SelectMask.transform.GetChild(0).gameObject);
            }

            if (value==1)
            {
                Instantiate(Prefabs[0], SelectMask.transform);

            }
            else if (value==2)
            {
                Instantiate(Prefabs[1], SelectMask.transform);
            }
            
        }
    }

    public void SelectToPara()
    {
        if (!CreateCharactorDaisu.NowDaisuFlag&&!CreateCharactorManeger.StopFlag && !CreateCharactorManeger.TyuuiFlag)
        {
            AllAudioManege.PlaySE(0);

            Camera.GetComponent<Animator>().SetBool("IdouFlag2", false);
        }
    }


    public void DestroyMe()
    {
        CreateCharactorManeger.TyuuiFlag = false;

        Destroy(gameObject);
    }

    public void ButtonOnClick()
    {
        if (!CreateCharactorManeger.AnimationPlaying && !CreateCharactorManeger.TyuuiFlag)
        {
            AllAudioManege.PlaySE(0);

            this.gameObject.GetComponent<Image>().color = new Color(this.gameObject.GetComponent<Image>().color.r - 0.2f, this.gameObject.GetComponent<Image>().color.g - 0.2f, this.gameObject.GetComponent<Image>().color.b - 0.2f);
        }
    }

    public void ButtonPointerUp()
    {
        if (!CreateCharactorManeger.AnimationPlaying && !CreateCharactorManeger.TyuuiFlag)
        {
            this.gameObject.GetComponent<Image>().color = new Color(this.gameObject.GetComponent<Image>().color.r + 0.2f, this.gameObject.GetComponent<Image>().color.g + 0.2f, this.gameObject.GetComponent<Image>().color.b + 0.2f);
        }
    }

    public void ButtonDown(GameObject Button)
    {
        if (!CreateCharactorManeger.AnimationPlaying && !CreateCharactorManeger.TyuuiFlag)
        {
            AllAudioManege.PlaySE(0);

            Image ButtonImage = Button.GetComponent<Image>();

            ButtonImage.color = new Color(ButtonImage.color.r - 0.2f, ButtonImage.color.g - 0.2f, ButtonImage.color.b - 0.2f);
        }
    }

    public void ButtonUp(GameObject Button)
    {
        if (!CreateCharactorManeger.AnimationPlaying && !CreateCharactorManeger.TyuuiFlag)
        {
            Image ButtonImage = Button.GetComponent<Image>();

            ButtonImage.color = new Color(ButtonImage.color.r + 0.2f, ButtonImage.color.g + 0.2f, ButtonImage.color.b + 0.2f);
        }
    }

    public void AnimationEnd()
    {
        CreateCharactorKakunin.AnimationEndFlag = true;
    }

    //シーンチェンジをつくる
    public void CreateSceneChange()
    {
        AllAudioManege.PlaySE(2);
        AllAudioManege.MajiStopBGM();

        CreateCharactorManeger.StopFlag = true;

        SceneChange = Instantiate(SceneChangePrefab,SceneChangeCanvas.transform);

        AllDataManege.DataSave();
    }
}
