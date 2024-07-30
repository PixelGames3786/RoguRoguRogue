using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TownMessageWindow : MonoBehaviour
{

    public GameObject CommandButtonPrefab,SceneChangePrefab;
    
    [System.NonSerialized]
    public Text ThisTextMesh;

    public TextAsset TextAsset;

    [System.NonSerialized]
    public int NowMojiNum, NowMojiGyou;

    [System.NonSerialized]
    public string[] MessageText;

    [System.NonSerialized]
    public bool InFlag,CanMesFlag,MesEndFlag;

    private float NowMesTime;
    public float MesTime,Speed;

    public int MesType;

    // Start is called before the first frame update
    void Start()
    {

        NowMojiNum = 0;
        NowMojiGyou = 0;

        MesTime = AllDataManege.MesTime;

        InFlag = true;

        MessageText = TextAsset.text.Split('\n');

        ThisTextMesh = gameObject.transform.GetChild(0).GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (InFlag)
        {
            GetComponent<RectTransform>().localPosition = new Vector2(0,GetComponent<RectTransform>().localPosition.y+(Speed*Time.deltaTime));

            if (GetComponent<RectTransform>().localPosition.y>=-660)
            {
                GetComponent<RectTransform>().localPosition = new Vector2(0,-660);

                InFlag = false;
                CanMesFlag = true;
            }
        }

        if (CanMesFlag)
        {
            if (!MesEndFlag)
            {
                if (NowMesTime>MesTime)
                {
                    if (NowMojiNum<MessageText[NowMojiGyou].Length)
                    {
                        ThisTextMesh.text += MessageText[NowMojiGyou][NowMojiNum];

                        NowMojiNum++;

                        AllAudioManege.PlaySE(8);

                    }
                    else
                    {
                        MesEndFlag = true;
                    }

                    NowMesTime = 0;

                }

                NowMesTime += Time.deltaTime;

            }
        }
    }

    public void PointerDown()
    {
        //メッセージ表示中に飛ばす
        if (!MesEndFlag&&!InFlag)
        {
            ThisTextMesh.text += MessageText[NowMojiGyou].Substring(NowMojiNum);

            MesEndFlag = true;

            return;
        }

        //メッセージを表示
        if (MesEndFlag&&CanMesFlag)
        {
            if (NowMojiGyou==0&&MesType==0)
            {
                TownManegerScript.CommandButton = Instantiate(CommandButtonPrefab,gameObject.transform.parent.transform);
                
                MesType = 1;

                return;
            }

            //村の通常
            if (MesType==1)
            {
                this.ThisTextMesh.text = "";
                this.MessageText = new string[1];

                this.MessageText[0] = TownManegerScript.TownText[UnityEngine.Random.Range(AllDataManege.SomeIntData.GetTownTextMin(AllDataManege.TownData.HattenLevel), AllDataManege.SomeIntData.GetTownTextMax(AllDataManege.TownData.HattenLevel))];

                this.MesType = 1;
                this.NowMojiNum = 0;
                this.NowMojiGyou = 0;
                this.CanMesFlag = true;
                this.MesEndFlag = false;
            }

            //村長の通常
            if (MesType==2)
            {

                this.ThisTextMesh.text = "";
                this.MessageText = new string[1];

                this.MessageText[0] = TownManegerScript.TownText[UnityEngine.Random.Range(AllDataManege.SomeIntData.GetTownTextMin(AllDataManege.TownData.HattenLevel)+3, AllDataManege.SomeIntData.GetTownTextMax(AllDataManege.TownData.HattenLevel)+3)];

                this.MesType = 2;
                this.NowMojiNum = 0;
                this.NowMojiGyou = 0;
                this.CanMesFlag = true;
                this.MesEndFlag = false;
            }

            //宿屋の通常
            if (MesType == 3)
            {

                this.ThisTextMesh.text = "";
                this.MessageText = new string[1];

                this.MessageText[0] = TownManegerScript.TownText[UnityEngine.Random.Range(AllDataManege.SomeIntData.GetTownTextMin(AllDataManege.TownData.HattenLevel) + 6, AllDataManege.SomeIntData.GetTownTextMax(AllDataManege.TownData.HattenLevel) + 6)];

                this.MesType = 3;
                this.NowMojiNum = 0;
                this.NowMojiGyou = 0;
                this.CanMesFlag = true;
                this.MesEndFlag = false;
            }

            //路地裏の通常
            if (MesType == 4)
            {

                this.ThisTextMesh.text = "";
                this.MessageText = new string[1];

                this.MessageText[0] = TownManegerScript.TownText[UnityEngine.Random.Range(AllDataManege.SomeIntData.GetTownTextMin(AllDataManege.TownData.HattenLevel) + 9, AllDataManege.SomeIntData.GetTownTextMax(AllDataManege.TownData.HattenLevel) + 9)];

                this.MesType = 4;
                this.NowMojiNum = 0;
                this.NowMojiGyou = 0;
                this.CanMesFlag = true;
                this.MesEndFlag = false;
            }

            //道具屋の通常
            if (MesType == 5)
            {
                this.ThisTextMesh.text = "";
                this.MessageText = new string[1];

                this.MessageText[0] = TownManegerScript.TownText[UnityEngine.Random.Range(AllDataManege.SomeIntData.GetTownTextMin(AllDataManege.TownData.HattenLevel) + 12, AllDataManege.SomeIntData.GetTownTextMax(AllDataManege.TownData.HattenLevel) + 12)];

                this.MesType = 5;
                this.NowMojiNum = 0;
                this.NowMojiGyou = 0;
                this.CanMesFlag = true;
                this.MesEndFlag = false;
            }

            //武器屋の通常
            if (MesType == 6)
            {
                this.ThisTextMesh.text = "";
                this.MessageText = new string[1];

                this.MessageText[0] = TownManegerScript.TownText[UnityEngine.Random.Range(AllDataManege.SomeIntData.GetTownTextMin(AllDataManege.TownData.HattenLevel) + 15, AllDataManege.SomeIntData.GetTownTextMax(AllDataManege.TownData.HattenLevel) + 15)];

                this.MesType = 6;
                this.NowMojiNum = 0;
                this.NowMojiGyou = 0;
                this.CanMesFlag = true;
                this.MesEndFlag = false;
            }

            //訓練場の通常
            if (MesType == 7)
            {
                ThisTextMesh.text = "";
                MessageText = new string[1];

                MessageText[0] = TownManegerScript.TownText[UnityEngine.Random.Range(AllDataManege.SomeIntData.GetTownTextMin(AllDataManege.TownData.HattenLevel) + 18, AllDataManege.SomeIntData.GetTownTextMax(AllDataManege.TownData.HattenLevel) + 18)];

                MesType = 7;
                NowMojiNum = 0;
                NowMojiGyou = 0;
                CanMesFlag = true;
                MesEndFlag = false;
            }
        }
    }
}
