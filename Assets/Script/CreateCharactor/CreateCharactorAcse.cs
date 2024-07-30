using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CreateCharactorAcse : MonoBehaviour
{
    //0 職業 1 種族
    public int SelectType;
    public int ThisNumber;

    public GameObject KakuninWindowPrefab,KakuninWindowPrefab2;
    private GameObject KakuninWindow;

    private Text[] KakuninWindowText = new Text[2];

    // Start is called before the first frame update
    void Start()
    {
        ButtonColorKousin();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ButtonColorKousin()
    {
        if (SelectType == 0)
        {
            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                if (AllDataManege.SomethingData.GetJobKaihou(i) == 0)
                {
                    gameObject.transform.GetChild(i).GetComponent<Image>().color = new Color(0.7f, 0.7f, 0.7f);
                    gameObject.transform.GetChild(i).transform.GetChild(0).GetComponent<Text>().color = new Color(0, 0, 0);
                }
                else
                {
                    gameObject.transform.GetChild(i).GetComponent<Image>().color = new Color(1f, 1f, 1f);
                    gameObject.transform.GetChild(i).transform.GetChild(0).GetComponent<Text>().color = new Color(0.3f, 0.3f, 0.3f, 0.8f);
                }
            }
        }

        if (SelectType == 1)
        {
            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                if (AllDataManege.SomethingData.GetSyuzokuKaihou(i) == 0)
                {
                    gameObject.transform.GetChild(i).GetComponent<Image>().color = new Color(0.7f, 0.7f, 0.7f);
                    gameObject.transform.GetChild(i).transform.GetChild(0).GetComponent<Text>().color = new Color(0, 0, 0);
                }
                else
                {
                    gameObject.transform.GetChild(i).GetComponent<Image>().color = new Color(1f, 1f, 1f);
                    gameObject.transform.GetChild(i).transform.GetChild(0).GetComponent<Text>().color = new Color(0.3f, 0.3f, 0.3f, 0.8f);
                }
            }
        }
    }

    public void PointerUp(int value)
    {

        //もし移動が終わっていて中止フラグが立っていなかったら
        if (!CreateCharactorManeger.SelectIdouFlag&&!CreateCharactorManeger.StopFlag && !CreateCharactorManeger.TyuuiFlag)
        {
            ThisNumber = value;

            //職業を選択する場合
            if (SelectType == 0)
            {
                //既に開放していた場合
                if (AllDataManege.SomethingData.GetJobKaihou(ThisNumber) == 1)
                {
                    KakuninWindow = Instantiate(KakuninWindowPrefab,transform.parent.transform.parent.transform);

                    KakuninWindow.GetComponent<CreateCharactorKakunin>().SelectType = 0;
                    KakuninWindow.GetComponent<CreateCharactorKakunin>().ThisNumber = ThisNumber;

                    KakuninWindowText[0]=KakuninWindow.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>();
                    KakuninWindowText[1] = KakuninWindow.transform.GetChild(1).transform.GetChild(0).GetComponent<Text>();

                    KakuninWindowText[0].text = AllDataManege.JobDataList[ThisNumber].GetJobSetumei();
                    KakuninWindowText[1].text = AllDataManege.JobDataList[ThisNumber].GetJobName();

                    CreateCharactorManeger.StopFlag = true;
                }
                else //開放していなかったら
                {
                    KakuninWindow = Instantiate(KakuninWindowPrefab2, transform.parent.transform.parent.transform);

                    KakuninWindow.GetComponent<CreateCharactorKakunin>().SelectType = 0;
                    KakuninWindow.GetComponent<CreateCharactorKakunin>().ThisNumber = ThisNumber;

                    KakuninWindowText[0] = KakuninWindow.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>();
                    KakuninWindowText[1] = KakuninWindow.transform.GetChild(1).transform.GetChild(0).GetComponent<Text>();

                    KakuninWindowText[0].text = KakuninWindowText[0].text.Replace("変更用","職業");
                    KakuninWindowText[1].text = AllDataManege.JobDataList[ThisNumber].GetJobName();

                    CreateCharactorManeger.StopFlag = true;
                }
            }

            //種族を選択する場合
            if (SelectType == 1)
            {
                //既に開放されている場合
                if (AllDataManege.SomethingData.GetSyuzokuKaihou(ThisNumber) == 1)
                {
                    KakuninWindow = Instantiate(KakuninWindowPrefab,transform.parent.transform.parent.transform);

                    KakuninWindow.GetComponent<CreateCharactorKakunin>().SelectType = 1;
                    KakuninWindow.GetComponent<CreateCharactorKakunin>().ThisNumber = ThisNumber;

                    KakuninWindowText[0] = KakuninWindow.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>();
                    KakuninWindowText[1] = KakuninWindow.transform.GetChild(1).transform.GetChild(0).GetComponent<Text>();

                    KakuninWindowText[0].text = AllDataManege.SyuzokuDataList[ThisNumber].GetSyuzokuSetumei();
                    KakuninWindowText[1].text = AllDataManege.SyuzokuDataList[ThisNumber].GetSyuzokuName();

                    CreateCharactorManeger.StopFlag = true;
                }
                else //開放していなかったら
                {
                    KakuninWindow = Instantiate(KakuninWindowPrefab2,transform.parent.transform.parent.transform);

                    KakuninWindow.GetComponent<CreateCharactorKakunin>().SelectType = 1;
                    KakuninWindow.GetComponent<CreateCharactorKakunin>().ThisNumber = ThisNumber;

                    KakuninWindowText[0] = KakuninWindow.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>();
                    KakuninWindowText[1] = KakuninWindow.transform.GetChild(1).transform.GetChild(0).GetComponent<Text>();

                    KakuninWindowText[0].text = KakuninWindowText[0].text.Replace("変更用", "種族");
                    KakuninWindowText[1].text = AllDataManege.SyuzokuDataList[ThisNumber].GetSyuzokuName();

                    CreateCharactorManeger.StopFlag = true;
                }
            }
        }
    }

    public static void CharaDataReset()
    {
        AllDataManege.CharactorData.SetKihonParameterKobetu(AllDataManege.SyuzokuData.GetSaiteiParameter()[1] + Mathf.CeilToInt((AllDataManege.SyuzokuData.CreateSaikouParameter[1] - AllDataManege.SyuzokuData.SaiteiParameter[1]) * AllDataManege.JobData.GetAddParameter()[1]), 1);
        AllDataManege.CharactorData.SetKihonParameterKobetu(AllDataManege.SyuzokuData.GetSaiteiParameter()[1] + Mathf.CeilToInt((AllDataManege.SyuzokuData.CreateSaikouParameter[1] - AllDataManege.SyuzokuData.SaiteiParameter[1]) * AllDataManege.JobData.GetAddParameter()[1]), 2);
        AllDataManege.CharactorData.SetKihonParameterKobetu(AllDataManege.SyuzokuData.GetSaiteiParameter()[2] + Mathf.CeilToInt((AllDataManege.SyuzokuData.CreateSaikouParameter[2] - AllDataManege.SyuzokuData.SaiteiParameter[2]) * AllDataManege.JobData.GetAddParameter()[2]), 3);
        AllDataManege.CharactorData.SetKihonParameterKobetu(AllDataManege.SyuzokuData.GetSaiteiParameter()[2] + Mathf.CeilToInt((AllDataManege.SyuzokuData.CreateSaikouParameter[2] - AllDataManege.SyuzokuData.SaiteiParameter[2]) * AllDataManege.JobData.GetAddParameter()[2]), 4);

        for (int i=6;i<14;i++)
        {
            AllDataManege.CharactorData.SetKihonParameterKobetu(AllDataManege.SyuzokuData.GetSaiteiParameter()[i-3]+ Mathf.CeilToInt((AllDataManege.SyuzokuData.CreateSaikouParameter[i-3] - AllDataManege.SyuzokuData.SaiteiParameter[i-3]) * AllDataManege.JobData.GetAddParameter()[i-3]), i-1);
        }

        for (int i=0;i<CreateCharactorDaisu.GejiRect.Length;i++)
        {
            CreateCharactorDaisu.GejiRect[i].sizeDelta = new Vector2(0,55);
        }
    }
}
