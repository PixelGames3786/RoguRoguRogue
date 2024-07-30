using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SkillSyutokuWindow : MonoBehaviour
{
    [System.NonSerialized]
    public GameObject SkillChangeWindow;

    private RectTransform ThisRect;

    [System.NonSerialized]
    public bool InFlag, OutFlag;

    public float Speed;

    [System.NonSerialized]
    public int WindowType,IrekaeSkill,IrekaeMotoSkill;

    // Start is called before the first frame update
    void Start()
    {
        ThisRect = gameObject.GetComponent<RectTransform>();

        InFlag = true;

        //ウィンドウのタイプで表示を変える
        switch (WindowType)
        {
            //スキル通常習得
            case 0:

                {
                    Destroy(gameObject.transform.GetChild(5).gameObject);

                    gameObject.transform.GetChild(1).GetComponent<Image>().sprite = AllDataManege.SkillDataList[IrekaeSkill].GetSkillSprite();
                    gameObject.transform.GetChild(2).GetComponent<Text>().text = AllDataManege.SkillDataList[IrekaeSkill].GetSkillSetumei();
                    gameObject.transform.GetChild(3).GetComponent<Text>().text = AllDataManege.SkillDataList[IrekaeSkill].GetSkillName();
                    gameObject.transform.GetChild(4).GetComponent<Text>().text = "新スキル習得！";

                    gameObject.transform.GetChild(6).GetComponent<RectTransform>().localPosition = new Vector3(0, -350, 0);
                }

                break;

            //入れ替えスキル説明
            case 1:

                {
                    gameObject.transform.GetChild(1).GetComponent<Image>().sprite = AllDataManege.SkillDataList[IrekaeSkill].GetSkillSprite();
                    gameObject.transform.GetChild(2).GetComponent<Text>().text = AllDataManege.SkillDataList[IrekaeSkill].GetSkillSetumei();
                    gameObject.transform.GetChild(3).GetComponent<Text>().text = AllDataManege.SkillDataList[IrekaeSkill].GetSkillName();
                    gameObject.transform.GetChild(4).GetComponent<Text>().text = "新スキル";
                    gameObject.transform.GetChild(5).transform.GetChild(0).GetComponent<Text>().text = "覚えない";
                }

                break;

            //入れ替えられる側スキル説明
            case 2:

                {
                    gameObject.transform.GetChild(1).GetComponent<Image>().sprite = AllDataManege.SkillDataList[AllDataManege.CharactorData.GetSkillMagic()[IrekaeMotoSkill]].GetSkillSprite();
                    gameObject.transform.GetChild(2).GetComponent<Text>().text = AllDataManege.SkillDataList[AllDataManege.CharactorData.GetSkillMagic()[IrekaeMotoSkill]].GetSkillSetumei();
                    gameObject.transform.GetChild(3).GetComponent<Text>().text = AllDataManege.SkillDataList[AllDataManege.CharactorData.GetSkillMagic()[IrekaeMotoSkill]].GetSkillName();
                    gameObject.transform.GetChild(4).GetComponent<Text>().text = "旧スキル 0" + (IrekaeMotoSkill + 1);
                    gameObject.transform.GetChild(5).transform.GetChild(0).GetComponent<Text>().text = "忘れる";
                }

                break;

            //入れ替えアイテム説明
            case 3:

                {
                    gameObject.transform.GetChild(1).GetComponent<Image>().sprite = AllDataManege.ItemDataList[IrekaeSkill].GetItemSprite();
                    gameObject.transform.GetChild(2).GetComponent<Text>().text = AllDataManege.ItemDataList[IrekaeSkill].GetItemSetumei();
                    gameObject.transform.GetChild(3).GetComponent<Text>().text = AllDataManege.ItemDataList[IrekaeSkill].GetItemName();
                    gameObject.transform.GetChild(4).GetComponent<Text>().text = "";
                    gameObject.transform.GetChild(5).transform.GetChild(0).GetComponent<Text>().text = "拾わない";
                }

                break;

            //入れ替えられる側アイテム説明
            case 4:

                {
                    gameObject.transform.GetChild(1).GetComponent<Image>().sprite = AllDataManege.ItemDataList[AllDataManege.CharactorData.BagItemInt[IrekaeMotoSkill]].GetItemSprite();
                    gameObject.transform.GetChild(2).GetComponent<Text>().text = AllDataManege.ItemDataList[AllDataManege.CharactorData.BagItemInt[IrekaeMotoSkill]].GetItemSetumei();
                    gameObject.transform.GetChild(3).GetComponent<Text>().text = AllDataManege.ItemDataList[AllDataManege.CharactorData.BagItemInt[IrekaeMotoSkill]].GetItemName();
                    gameObject.transform.GetChild(4).GetComponent<Text>().text = "アイテム 0" + (IrekaeMotoSkill + 1);
                    gameObject.transform.GetChild(5).transform.GetChild(0).GetComponent<Text>().text = "捨てる";
                }

                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (InFlag)
        {
            ThisRect.localScale = new Vector3(1,ThisRect.localScale.y+(Speed*Time.deltaTime),1);

            ThisRect.localScale = new Vector3(1,Mathf.Clamp01(ThisRect.localScale.y),1);

            if (ThisRect.localScale.y>=1f)
            {
                InFlag = false;
            }
        }

        if (OutFlag)
        {
            ThisRect.localScale = new Vector3(1, ThisRect.localScale.y - (Speed * Time.deltaTime), 1);

            ThisRect.localScale = new Vector3(1, Mathf.Clamp01(ThisRect.localScale.y), 1);

            if (ThisRect.localScale.y <=0f)
            {
                Destroy(gameObject);
            }
        }
    }

    public void SetOutFlag()
    {
        if (!InFlag)
        {
            OutFlag = true;
        }
    }

    public void SkillIrekae()
    {
        if (WindowType==1)
        {
            SkillChangeWindow.GetComponent<SkillChangeScript>().OutFlag=true;

            SetOutFlag();
        }

        if (WindowType==2)
        {
            int[] SetyouInt = new int[6];

            SetyouInt = AllDataManege.CharactorData.GetSkillMagic();

            SetyouInt[IrekaeMotoSkill] = IrekaeSkill;

            AllDataManege.CharactorData.SetSkillMagic(SetyouInt);

            SkillChangeWindow.GetComponent<SkillChangeScript>().OutFlag = true;

            SetOutFlag();
        }

        if (WindowType==3)
        {
            SkillChangeWindow.GetComponent<ItemChangeScript>().OutFlag = true;

            SetOutFlag();
        }

        if (WindowType==4)
        {
            AllDataManege.CharactorData.BagItemInt[IrekaeMotoSkill] = IrekaeSkill;

            SkillChangeWindow.GetComponent<ItemChangeScript>().OutFlag = true;

            SetOutFlag();
        }
    }
}
