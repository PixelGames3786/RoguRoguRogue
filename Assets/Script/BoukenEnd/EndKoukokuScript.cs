using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndKoukokuScript : MonoBehaviour
{
    [System.NonSerialized]
    public BoukenEndScript BoukenEndScript;

    private bool CanButton;

    [System.NonSerialized]
    public bool CantKoukoku;

    // Start is called before the first frame update
    void Start()
    {
        CanButton = false;

        //もし旅の経験をゲット出来るようになる系広告なら
        if (BoukenEndScript.GetChangeFlag)
        {
            gameObject.transform.GetChild(0).GetComponent<Text>().text="戦闘中に死亡した場合は旅の経験が得られません\n\n広告を見ると特別に得ることができます";
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (CantKoukoku&&Input.GetMouseButton(0))
        {
            BoukenEndScript.CanSinkouFlag = true;
            BoukenEndScript.CanMesFlag = true;

            Destroy(gameObject);
        }
    }

    //入りアニメーションが終わる時に実行
    public void InAnimation()
    {
        CanButton = true;
    }

    //終わりアニメーションが終わるときに実行
    public void EndAnimation()
    {
        BoukenEndScript.CanSinkouFlag = true;
        BoukenEndScript.CanMesFlag = true;

        Destroy(gameObject);
    }

    //ボタン類
    public void ButtonDown(int value)
    {
        //ボタンを押せるか
        if (!CanButton)
        {
            return;
        }

        //はいボタン
        if (value==0)
        {
            if (BoukenEndScript.GetChangeFlag)
            {
                //GameObject.Find("AdsListener").GetComponent<UnityAdsListener>().ShowMovieAd();
                //GameObject.Find("AdsListener").GetComponent<UnityAdsListener>().AdsType = 3;

                GameObject.Find("AdmobListener").GetComponent<AdmobListener>().AdmobType=3;
                GameObject.Find("AdmobListener").GetComponent<AdmobListener>().UserChoseToWatchAd();
            }
            else
            {
                //GameObject.Find("AdsListener").GetComponent<UnityAdsListener>().ShowMovieAd();
                //GameObject.Find("AdsListener").GetComponent<UnityAdsListener>().AdsType = 2;

                GameObject.Find("AdmobListener").GetComponent<AdmobListener>().AdmobType = 2;
                GameObject.Find("AdmobListener").GetComponent<AdmobListener>().UserChoseToWatchAd();
            }

            Time.timeScale = 0;
        }

        //いいえボタン
        if (value==1)
        {
            gameObject.GetComponent<Animator>().SetTrigger("OutTrigger");

            CanButton = false;
        }
    }
}
