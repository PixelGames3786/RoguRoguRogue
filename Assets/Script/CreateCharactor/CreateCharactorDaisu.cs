using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.EventSystems;

public class CreateCharactorDaisu : MonoBehaviour
{

    public float Speed;

    public GameObject[] ButtonRui;
    public GameObject ParamatorParent,GejiParent;

    public GameObject EventSystem;

    private Text[] ParamatorText = new Text[13];
    public static RectTransform[] GejiRect = new RectTransform[11];

    public Sprite[] ButtonSprites = new Sprite[2];

    public BaseCharactorData CharactorData;
    private BaseSyuzokuData SyuzokuData;
    private BaseJobData JobData;

    private int[] RandomInt = new int[14];
    public static int[] KihonDataInt=new int[5];

    private int[] TypeRandomInt = new int[3];

    private float[] RectTani=new float[11];
    private float[] NowGejiSize = new float[11];
    private float[] TargetGejiSize = new float[11];

    private bool[] GejiInFlag = new bool[11];
    private bool[] GejiOutFlag = new bool[11];
    private bool[] NowIdouFlag = new bool[11];

    public static bool NowDaisuFlag;

    // Start is called before the first frame update
    void Start()
    {
        CharactorData = AllDataManege.CharactorData;
        SyuzokuData = AllDataManege.SyuzokuData;
        JobData = AllDataManege.JobData;

        KihonDataInt[0] = 15;
        KihonDataInt[1] = 0;
        KihonDataInt[2] = 0;

        for (int i=0;i<ParamatorParent.transform.childCount;i++)
        {
            ParamatorText[i] = ParamatorParent.transform.GetChild(i).GetComponent<Text>();
        }

        for (int i=0;i<GejiParent.transform.childCount;i++)
        {
            GejiRect[i] = GejiParent.transform.GetChild(i).transform.GetChild(0).transform.GetChild(0).GetComponent<RectTransform>();
        }

        SetDaisuFlag();
    }

    // Update is called once per frame
    void Update()
    {
        if (NowDaisuFlag)
        { 
            for (int i=0;i<GejiRect.Length;i++)
            {
                if (GejiInFlag[i])
                {
                    GejiRect[i].sizeDelta = new Vector2(GejiRect[i].sizeDelta.x+Speed*Time.deltaTime,55);

                    if (GejiRect[i].sizeDelta.x>=TargetGejiSize[i])
                    {
                        GejiRect[i].sizeDelta = new Vector2(TargetGejiSize[i],55);
                        GejiInFlag[i] = false;
                    }
                }

                if (GejiOutFlag[i])
                {
                    GejiRect[i].sizeDelta = new Vector2(GejiRect[i].sizeDelta.x - Speed*Time.deltaTime, 55);

                    if (GejiRect[i].sizeDelta.x <= TargetGejiSize[i])
                    {
                        GejiRect[i].sizeDelta = new Vector2(TargetGejiSize[i], 55);
                        GejiOutFlag[i] = false;
                    }
                }
            }

            if (GejiInFlag.All(x=>x==false)&& GejiOutFlag.All(x => x == false))
            {
                EventSystem.GetComponent<EventSystem>().enabled = true;

                NowDaisuFlag = false;
            }
            
        }

        //表示
        ParamatorText[0].text = "ATK　" + CharactorData.GetParameter()[5];
        ParamatorText[1].text = "DEF　" + CharactorData.GetParameter()[6];
        ParamatorText[2].text = "INT　" + CharactorData.GetParameter()[7];
        ParamatorText[3].text = "MIN　" + CharactorData.GetParameter()[8];
        ParamatorText[4].text = "DEX　" + CharactorData.GetParameter()[9];
        ParamatorText[5].text = "AGI　" + CharactorData.GetParameter()[10];
        ParamatorText[6].text = "MaxHP　" + CharactorData.GetParameter()[1];
        ParamatorText[7].text = "MaxMP　" + CharactorData.GetParameter()[3];
        ParamatorText[8].text = "SPD　" + CharactorData.GetParameter()[11];
        ParamatorText[9].text = "LUC　" + CharactorData.GetParameter()[12];
        ParamatorText[10].text = "CHR　" + CharactorData.GetParameter()[13];

        CharactorData = AllDataManege.CharactorData;
        SyuzokuData = AllDataManege.SyuzokuData;
        JobData = AllDataManege.JobData;

        //文字の更新
        ParamatorText[11].GetComponent<Text>().text = "種族："+AllDataManege.SyuzokuData.SyuzokuName;
        ParamatorText[12].GetComponent<Text>().text = "職業：" + AllDataManege.JobData.GetJobName();
    }

    public void SetDaisuFlag()
    {
        if (!CreateCharactorManeger.StopFlag)
        {

            NowDaisuFlag = true;

            //ステータスをランダムに
            RandomInt[0] = 1;
            RandomInt[1] = Random.Range(SyuzokuData.SaiteiParameter[1] + Mathf.CeilToInt((SyuzokuData.CreateSaikouParameter[1] - SyuzokuData.SaiteiParameter[1]) * JobData.GetAddParameter()[1]), SyuzokuData.CreateSaikouParameter[1] + Mathf.CeilToInt((SyuzokuData.CreateSaikouParameter[1] - SyuzokuData.SaiteiParameter[1]) * JobData.GetAddParameter()[1]));
            RandomInt[2] = RandomInt[1];
            RandomInt[3] = Random.Range(SyuzokuData.SaiteiParameter[2] + Mathf.CeilToInt((SyuzokuData.CreateSaikouParameter[2] - SyuzokuData.SaiteiParameter[2]) * JobData.GetAddParameter()[2]), SyuzokuData.CreateSaikouParameter[2] + Mathf.CeilToInt((SyuzokuData.CreateSaikouParameter[2] - SyuzokuData.SaiteiParameter[2]) * JobData.GetAddParameter()[2]));
            RandomInt[4] = RandomInt[3];

            for (int i = 3; i < 12; i++)
            {
                RandomInt[i + 2] = Random.Range(SyuzokuData.SaiteiParameter[i] + Mathf.CeilToInt((SyuzokuData.CreateSaikouParameter[i] - SyuzokuData.SaiteiParameter[i]) * JobData.GetAddParameter()[i]), SyuzokuData.CreateSaikouParameter[i] + Mathf.CeilToInt((SyuzokuData.CreateSaikouParameter[i] - SyuzokuData.SaiteiParameter[i]) * JobData.GetAddParameter()[i]));
            }

            //Grow Magic Skill
            TypeRandomInt[0] = Random.Range(0, 5);
            TypeRandomInt[1] = Random.Range(0, 5);

            //ステータスをセットする
            AllDataManege.CharactorData.SetKihonParameter(RandomInt);
            AllDataManege.CharactorData.SetKihonData(KihonDataInt);
            AllDataManege.CharactorData.SetMagicType(TypeRandomInt[0]);
            AllDataManege.CharactorData.SetSkillType(TypeRandomInt[1]);

            //ゲージ関連
            //一本ごとの単位を取得
            for (int i = 0; i < GejiRect.Length; i++)
            {
                RectTani[i] = 490 / Mathf.Ceil((SyuzokuData.CreateSaikouParameter[i + 1] - SyuzokuData.SaiteiParameter[i + 1]) + Mathf.CeilToInt((SyuzokuData.CreateSaikouParameter[i + 1] - SyuzokuData.SaiteiParameter[i + 1]) * JobData.GetAddParameter()[i + 1]) - 1);

                NowGejiSize[i] = GejiRect[i].sizeDelta.x;
            }

            TargetGejiSize[0] = RectTani[0] * (AllDataManege.CharactorData.GetParameter()[1] - SyuzokuData.SaiteiParameter[1]);

            if (TargetGejiSize[0] > NowGejiSize[0])
            {
                GejiInFlag[0] = true;
            }
            else if (NowGejiSize[0] > TargetGejiSize[0])
            {
                GejiOutFlag[0] = true;
            }

            TargetGejiSize[1] = RectTani[1] * (AllDataManege.CharactorData.GetParameter()[3] - SyuzokuData.SaiteiParameter[2]);
            if (TargetGejiSize[1] > NowGejiSize[1])
            {
                GejiInFlag[1] = true;
            }
            else if (NowGejiSize[1] > TargetGejiSize[1])
            {
                GejiOutFlag[1] = true;
            }

            for (int i = 2; i < GejiRect.Length; i++)
            {
                TargetGejiSize[i] = RectTani[i] * (AllDataManege.CharactorData.GetParameter()[i + 3] - SyuzokuData.SaiteiParameter[i + 1]);

                if (TargetGejiSize[i] > NowGejiSize[i])
                {
                    GejiInFlag[i] = true;
                }
                else if (NowGejiSize[i] > TargetGejiSize[i])
                {
                    GejiOutFlag[i] = true;
                }
            }

            //文字数によってボタンの長さを変更
            ButtonRui[0].GetComponent<RectTransform>().sizeDelta = new Vector2(ParamatorText[11].GetComponent<Text>().text.Length * 90, 140);
            ButtonRui[1].GetComponent<RectTransform>().sizeDelta = new Vector2(ParamatorText[12].GetComponent<Text>().text.Length * 90, 140);

            //ボタンの反応を消す
            EventSystem.GetComponent<EventSystem>().enabled = false;

            gameObject.GetComponent<Image>().sprite = ButtonSprites[0];
        }
    }

    public void ButtonOnClick()
    {
        if (!CreateCharactorManeger.StopFlag)
        {
            gameObject.GetComponent<Image>().sprite = ButtonSprites[1];
        }
    }
}
