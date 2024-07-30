using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleManeger : MonoBehaviour
{
    private GameObject CanvasObject,SceneChange;
    public GameObject LightParent,PressStartPrefab,SceneChangePrefab,TitleBG;

    public GameObject VersionText;

    public Sprite[] BGSprites;

    [System.NonSerialized]
    public bool WaitPressFlag;

    // Start is called before the first frame update
    void Start()
    {
        CanvasObject = GameObject.Find("Canvas");

        VersionText.GetComponent<Text>().text = "Ver " + Application.version;

        int RandomBG = Random.Range(0,BGSprites.Length);
        TitleBG.GetComponent<SpriteRenderer>().sprite = BGSprites[RandomBG];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        { 
            //アニメーションをスキップ
            if (LightParent.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime <= 1f&&!WaitPressFlag&&!SceneChange)
            {
                LightParent.GetComponent<Animator>().SetBool("SkipFlag",true);

                Instantiate(PressStartPrefab,CanvasObject.transform);

                AllAudioManege.PlayBGM(0);

                //AllDataManege.AdmobListener.UserChoseToWatchAd();

                WaitPressFlag = true;

                return;
            }

            if (WaitPressFlag&&!SceneChange)
            {
                SceneChange = Instantiate(SceneChangePrefab,CanvasObject.transform);

                WaitPressFlag = false;

                AllAudioManege.PlaySE(0);
                AllAudioManege.StopBGM();
            }
        }

        if (SceneChange&&SceneChange.transform.GetChild(0).GetComponent<Rigidbody2D>().IsSleeping())
        {

            if (!PlayerPrefs.HasKey("NowPlaying"))
            {
                SceneManager.LoadScene("CreateCharactor");
            }
            
            if(PlayerPrefs.HasKey("NowPlaying"))
            {
                if (AllDataManege.TokusyuJoukyou==0)
                {
                    AllAudioManege.StopBGM();

                    SceneManager.LoadScene("Town");

                }else if (AllDataManege.TokusyuJoukyou>=1)
                {
                    AllAudioManege.StopBGM();

                    SceneManager.LoadScene("Dangeon");
                }

            }
        }
    }
}
