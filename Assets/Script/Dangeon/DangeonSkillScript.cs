using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DangeonSkillScript : MonoBehaviour
{
    public GameObject SetumeiPrefab,IconParentPrefab,IconParentPrefab2;
    private GameObject SetumeiWindow,IconParent,IconParent2;

    private RectTransform ThisRect;

    [System.NonSerialized]
    public bool InFlag, OutFlag;

    public float Speed;

    private BaseSkillData SentakuSkill;

    // Start is called before the first frame update
    void Start()
    {
        for (int i=0;i<6;i++)
        {
            gameObject.transform.GetChild(i).GetComponent<Image>().sprite = AllDataManege.SkillDataList[AllDataManege.CharactorData.SkillAndMagic[i]].GetSkillSprite();
        }

        ThisRect = gameObject.GetComponent<RectTransform>();

        InFlag = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (InFlag)
        {
            ThisRect.localScale = new Vector3(ThisRect.localScale.x+(Speed*Time.deltaTime),-115);

            ThisRect.localScale = new Vector3(Mathf.Clamp01(ThisRect.localScale.x),1);

            if (ThisRect.localScale.x>=1)
            {
                InFlag = false;
            }
        }

        if (OutFlag)
        {
            ThisRect.localScale = new Vector3(ThisRect.localScale.x - (Speed * Time.deltaTime), -115);

            ThisRect.localScale = new Vector3(Mathf.Clamp01(ThisRect.localScale.x), 1);

            if (ThisRect.localScale.x <=0)
            {
                DangeonManeger.CanCommandFlag = true;

                Destroy(gameObject);
            }
        }
    }

    public void ButtonDown(GameObject Button)
    {
        if (InFlag || OutFlag || SetumeiWindow)
        {
            return;
        }

        Image ButtonImage = Button.GetComponent<Image>();

        ButtonImage.color = new Color(0.8f,0.8f,0.8f);
    }

    public void ButtonUp(GameObject Button)
    {
        if (InFlag || OutFlag || SetumeiWindow)
        {
            return;
        }

        Image ButtonImage = Button.GetComponent<Image>();

        ButtonImage.color = new Color(1f, 1f, 1f);

    }

    public void SkillButtonDown(int value)
    {
        if (InFlag||OutFlag||SetumeiWindow)
        {
            return;
        }

        SentakuSkill = AllDataManege.SkillDataList[AllDataManege.CharactorData.SkillAndMagic[value]];

        SetumeiWindow = Instantiate(SetumeiPrefab,gameObject.transform.parent.transform);

        SetumeiWindow.GetComponent<StetasSetumeiScript>().SkillSiyou=true;
        SetumeiWindow.GetComponent<StetasSetumeiScript>().SiyouSkill = SentakuSkill;

        SetumeiWindow.transform.GetChild(1).GetComponent<Image>().sprite = SentakuSkill.GetSkillSprite();
        SetumeiWindow.transform.GetChild(2).GetComponent<Text>().text = SentakuSkill.GetSkillSetumei();
        SetumeiWindow.transform.GetChild(3).GetComponent<Text>().text = SentakuSkill.GetSkillName();

        if (SentakuSkill.GetSkillNumber()!=0)
        {
            //アイコンを作る
            IconParent = Instantiate(IconParentPrefab, SetumeiWindow.transform);
            IconParent2 = Instantiate(IconParentPrefab2, SetumeiWindow.transform);

            MakeIcon();
        }

        Destroy(SetumeiWindow.transform.GetChild(6).gameObject);

        SetumeiWindow.transform.GetChild(4).transform.GetChild(0).GetComponent<Text>().text = "使用する";
    }

    //スキルの属性とか表示するやつ
    private void MakeIcon()
    {
        Destroy(IconParent.transform.GetChild(1).gameObject);
        Destroy(IconParent2.transform.GetChild(0).gameObject);

        IconParent.transform.GetChild(0).GetComponent<RectTransform>().localPosition = new Vector2(-370, 420);

        //属性に応じて表示を変える
        switch (SentakuSkill.GetZokusei())
        {
            case 0:

                IconParent.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = "無";

                break;

            case 1:

                IconParent.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = "炎";

                break;

            case 2:

                IconParent.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = "風";

                break;

            case 3:

                IconParent.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = "水";

                break;

            case 4:

                IconParent.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = "地";

                break;

            case 5:

                IconParent.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = "闇";

                break;

            case 6:

                IconParent.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = "光";

                break;

            case 7:

                IconParent.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = "雷";

                break;
        }

        //消費ＭＰの表示
        if (SentakuSkill.GetSkillSyouhi()[0] > 0 && SentakuSkill.GetSkillSyouhi()[1] > 0)
        {
            IconParent2.transform.GetChild(1).transform.GetChild(0).GetComponent<Text>().text = SentakuSkill.GetSkillSyouhi()[0] + "HP";
            IconParent2.transform.GetChild(2).transform.GetChild(0).GetComponent<Text>().text = SentakuSkill.GetSkillSyouhi()[1] + "MP";
        }
        else if (SentakuSkill.GetSkillSyouhi()[0] > 0)
        {
            Destroy(IconParent2.transform.GetChild(2).gameObject);

            IconParent2.transform.GetChild(1).transform.GetChild(0).GetComponent<Text>().text = SentakuSkill.GetSkillSyouhi()[0] + "HP";
        }
        else if (SentakuSkill.GetSkillSyouhi()[1] > 0)
        {
            Destroy(IconParent2.transform.GetChild(1).gameObject);

            IconParent2.transform.GetChild(2).GetComponent<RectTransform>().localPosition = new Vector2(325, 430);

            IconParent2.transform.GetChild(2).transform.GetChild(0).GetComponent<Text>().text = SentakuSkill.GetSkillSyouhi()[1] + "MP";
        }

    }
}
