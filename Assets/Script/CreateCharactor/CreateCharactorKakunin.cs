using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateCharactorKakunin : MonoBehaviour
{
    public int SelectType;
    public int ThisNumber;
    private int WaitHashPath;

    private string HenkanYouStr;

    private float NowCurrentTime;

    public static bool AnimationEndFlag,AnimationPlaying;

    private Animator ThisAnime;

    private GameObject KakuninWindow3,TyuuiWindow;
    public GameObject KakuninWindow3Prefab,KakuninWindow2,KakuninPrefab;

    private GameObject[] Buttons;

    private Text[] KakuninWindow3Text = new Text[4];


    // Start is called before the first frame update
    void Start()
    {
        ThisAnime = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ThisAnime.GetBool("OutFlag"))
        {
            if (AnimationEndFlag)
            {
                CreateCharactorManeger.StopFlag = false;

                AnimationEndFlag = false;
                CreateCharactorManeger.AnimationPlaying = false;

                Destroy(gameObject);
            }
        }

        if (KakuninWindow2&& ThisAnime.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            Destroy(KakuninWindow2);
        }
    }

    public void Yes()
    {
        //アニメーションが終わっていたら
        if (ThisAnime.GetCurrentAnimatorStateInfo(0).normalizedTime>=1.0f)
        {

            ThisAnime.SetBool("OutFlag", true);

            //職業選択の場合
            if (SelectType == 0)
            {

                AllDataManege.JobData = AllDataManege.JobDataList[ThisNumber];

                CreateCharactorDaisu.KihonDataInt[2] = ThisNumber;
                AllDataManege.CharactorData.SetKihonData(CreateCharactorDaisu.KihonDataInt);

                CreateCharactorAcse.CharaDataReset();
            }
            else　//種族選択の場合
            {
                AllDataManege.SyuzokuData = AllDataManege.SyuzokuDataList[ThisNumber];

                CreateCharactorDaisu.KihonDataInt[1] = ThisNumber;
                AllDataManege.CharactorData.SetKihonData(CreateCharactorDaisu.KihonDataInt);

                CreateCharactorAcse.CharaDataReset();
            }
        }
    }

    public void YesSecond()
    {
        //アニメーションが終わっていたら
        if (ThisAnime.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            ThisAnime.SetBool("OutFlag", true);
        }

        KakuninWindow3 = Instantiate(KakuninWindow3Prefab,transform.parent);

        KakuninWindow3.GetComponent<CreateCharactorKakunin>().SelectType=SelectType;
        KakuninWindow3.GetComponent<CreateCharactorKakunin>().ThisNumber = ThisNumber;
        KakuninWindow3.GetComponent<CreateCharactorKakunin>().KakuninWindow2 = gameObject;

        for (int i=0;i<4;i++)
        {
            KakuninWindow3Text[i] = KakuninWindow3.transform.GetChild(i).transform.GetChild(0).GetComponent<Text>();
        }

        //職業選択の場合
        if (SelectType==0)
        {
            HenkanYouStr = AllDataManege.JobDataList[ThisNumber].GetHituyou().ToString();

            KakuninWindow3Text[0].text = KakuninWindow3Text[0].text.Replace("変",HenkanYouStr);
            KakuninWindow3Text[0].text = KakuninWindow3Text[0].text.Replace("換",AllDataManege.JobDataList[ThisNumber].GetJobName())+"\n\n<size=46>旅の経験はプレイ中に手に入ります</size>";
            KakuninWindow3Text[3].text = KakuninWindow3Text[3].text.Replace("変換", HenkanYouStr);

            KakuninWindow3Text[1].text = AllDataManege.JobDataList[ThisNumber].GetJobName();
        }
        else　//種族選択の場合
        {
            HenkanYouStr = AllDataManege.SyuzokuDataList[ThisNumber].GetHituyou().ToString();

            KakuninWindow3Text[0].text = KakuninWindow3Text[0].text.Replace("変",HenkanYouStr);
            KakuninWindow3Text[0].text = KakuninWindow3Text[0].text.Replace("換",AllDataManege.SyuzokuDataList[ThisNumber].GetSyuzokuName())+"\n\n<size=46>旅の経験はプレイ中に手に入ります</size>";
            KakuninWindow3Text[3].text = KakuninWindow3Text[3].text.Replace("変換", HenkanYouStr);

            KakuninWindow3Text[1].text = AllDataManege.SyuzokuDataList[ThisNumber].GetSyuzokuName();
        }
    }

    //開放するかどうか
    public void YesThird()
    {
        if (SelectType==0) //職業の時
        {
            Buttons= new GameObject[GameObject.Find("SelectButtonParentJob(Clone)").transform.childCount];

            for (int i = 0; i < GameObject.Find("SelectButtonParentJob(Clone)").transform.childCount; i++)
            {
                Buttons[i] = GameObject.Find("SelectButtonParentJob(Clone)").transform.GetChild(i).gameObject;
            }

            //もし旅の経験が足りていたら
            if (AllDataManege.SomethingData.GetTabiKeiken() - AllDataManege.JobDataList[ThisNumber].GetHituyou()>=0)
            {
                AllDataManege.SomethingData.SetTabikeiken(AllDataManege.SomethingData.GetTabiKeiken() - AllDataManege.JobDataList[ThisNumber].GetHituyou());
                AllDataManege.SomethingData.SetJobKaihou(1,ThisNumber);

                Buttons[ThisNumber].GetComponent<Image>().color = new Color(1f, 1f, 1f);

                //アニメーションを起動
                if (ThisAnime.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                {
                    CreateCharactorManeger.AnimationPlaying = true;

                    GameObject.Find("SelectButtonParentJob(Clone)").GetComponent<CreateCharactorAcse>().ButtonColorKousin();

                    ThisAnime.SetBool("OutFlag", true);
                }
            }
            else  //旅の経験が足りなかったら
            {
                //注意ウィンドウを出す
                if (!CreateCharactorManeger.TyuuiFlag)
                {
                    TyuuiWindow = Instantiate(KakuninPrefab, gameObject.transform.parent.transform);
                    TyuuiWindow.GetComponent<RectTransform>().localPosition = new Vector3(1080, 0, 0);

                    TyuuiWindow.transform.GetChild(0).GetComponent<Text>().text = "旅の経験が足りないよ！";

                    TyuuiWindow.GetComponent<KakuninWindowScript>().KakuninType = 1;

                    CreateCharactorManeger.TyuuiFlag = true;

                    gameObject.transform.GetChild(3).GetComponent<Image>().color = new Color(1,1,1);
                }
            }
        }
        else //種族の時
        {
            Buttons = new GameObject[GameObject.Find("SelectButtonParentSyuzoku(Clone)").transform.childCount];

            for (int i = 0; i < GameObject.Find("SelectButtonParentSyuzoku(Clone)").transform.childCount; i++)
            {
                Buttons[i] = GameObject.Find("SelectButtonParentSyuzoku(Clone)").transform.GetChild(i).gameObject;
            }

            //旅の経験が足りなかったら
            if (AllDataManege.SomethingData.GetTabiKeiken() - AllDataManege.SyuzokuDataList[ThisNumber].GetHituyou() >= 0)
            {
                AllDataManege.SomethingData.SetTabikeiken(AllDataManege.SomethingData.GetTabiKeiken() - AllDataManege.SyuzokuDataList[ThisNumber].GetHituyou());
                AllDataManege.SomethingData.SetSyuzokuKaihou(1, ThisNumber);

                Buttons[ThisNumber].GetComponent<Image>().color = new Color(1f,1f,1f);

                //アニメーションを起動
                if (ThisAnime.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                {
                    ThisAnime.SetBool("OutFlag", true);

                    GameObject.Find("SelectButtonParentSyuzoku(Clone)").GetComponent<CreateCharactorAcse>().ButtonColorKousin();

                    CreateCharactorManeger.AnimationPlaying = true;
                }
            }
            else //旅の経験が足りなかったら
            {
                //注意ウィンドウを出す
                if (!CreateCharactorManeger.TyuuiFlag)
                {
                    TyuuiWindow = Instantiate(KakuninPrefab, gameObject.transform.parent.transform);
                    TyuuiWindow.GetComponent<RectTransform>().localPosition = new Vector3(1080,0,0);

                    TyuuiWindow.transform.GetChild(0).GetComponent<Text>().text="旅の経験が足りないよ！";

                    TyuuiWindow.GetComponent<KakuninWindowScript>().KakuninType = 1;

                    CreateCharactorManeger.TyuuiFlag = true;

                    gameObject.transform.GetChild(3).GetComponent<Image>().color = new Color(1, 1, 1);
                }
            }
        }
    }

    public void No()
    {
        if (ThisAnime.GetCurrentAnimatorStateInfo(0).normalizedTime>=1.0f&&!CreateCharactorManeger.TyuuiFlag)
        {
            ThisAnime.SetBool("OutFlag", true);
        }
    }
}
