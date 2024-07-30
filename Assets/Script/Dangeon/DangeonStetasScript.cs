using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DangeonStetasScript : MonoBehaviour
{
    public DangeonManeger DangeonManeger;

    public GameObject SetumeiWindowPrefab, IconParentPrefab, IconParentPrefab2;
    private GameObject SetumeiWindow, IconParent, IconParent2;

    private RectTransform ThisRect;

    private BaseSkillData SentakuSkill;

    private bool InFlag, OutFlag;

    public float Speed;

    // Start is called before the first frame update
    void Start()
    {
        InFlag = true;

        ThisRect = gameObject.GetComponent<RectTransform>();

        //スキルの画像を設定
        for (int i = 0; i < 6; i++)
        {
            gameObject.transform.GetChild(1).transform.GetChild(i).GetComponent<Image>().sprite = AllDataManege.SkillDataList[AllDataManege.CharactorData.SkillAndMagic[i]].GetSkillSprite();
        }

        //ステータスを表示
        StetasHyouji();
    }

    // Update is called once per frame
    void Update()
    {
        if (InFlag)
        {
            ThisRect.localScale = new Vector3(1, ThisRect.localScale.y + (Speed * Time.deltaTime), 1);

            ThisRect.localScale = new Vector3(1, Mathf.Clamp01(ThisRect.localScale.y), 1);

            if (ThisRect.localScale.y >= 1f)
            {
                InFlag = false;
            }
        }

        if (OutFlag)
        {
            ThisRect.localScale = new Vector3(1, ThisRect.localScale.y - (Speed * Time.deltaTime), 1);

            ThisRect.localScale = new Vector3(1, Mathf.Clamp01(ThisRect.localScale.y), 1);

            if (ThisRect.localScale.y <= 0f)
            {
                DangeonManeger.CanCommandFlag = true;

                Destroy(gameObject);
            }
        }
    }

    //ボタン関連//

    public void ButtonDown(GameObject Button)
    {
        if (InFlag || SetumeiWindow)
        {
            return;
        }

        Image ButtonImage = Button.GetComponent<Image>();

        ButtonImage.color = new Color(0.8f, 0.8f, 0.8f);
    }

    public void ButtonUp(GameObject Button)
    {
        if (InFlag || SetumeiWindow)
        {
            return;
        }

        Image ButtonImage = Button.GetComponent<Image>();

        ButtonImage.color = new Color(1f, 1f, 1f);
    }

    public void BackButtonDown()
    {
        if (InFlag || SetumeiWindow)
        {
            return;
        }

        OutFlag = true;
    }

    //スキルの説明を表示
    public void MakeSkillSetumei(int value)
    {
        AllAudioManege.PlaySE(0);

        //ウィンドウが開いている途中には作らない
        if (InFlag || SetumeiWindow)
        {
            return;
        }

        SetumeiWindow = Instantiate(SetumeiWindowPrefab, gameObject.transform.parent.transform);

        SentakuSkill = AllDataManege.SkillDataList[AllDataManege.CharactorData.GetSkillMagic()[value]];

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

        //戦闘中でしか使用できないスキルの場合
        if (SentakuSkill.GetCanUseTown() == 0)
        {
            Destroy(SetumeiWindow.transform.GetChild(4).gameObject);
            Destroy(SetumeiWindow.transform.GetChild(6).gameObject);

            SetumeiWindow.transform.GetChild(5).GetComponent<RectTransform>().localPosition = new Vector3(0, -400, 0);
        }
        else
        {
            Destroy(SetumeiWindow.transform.GetChild(6).gameObject);

            SetumeiWindow.transform.GetChild(4).transform.GetChild(0).GetComponent<Text>().text = "使用する";

            SetumeiWindow.GetComponent<StetasSetumeiScript>().SkillSiyou = true;
            SetumeiWindow.GetComponent<StetasSetumeiScript>().SiyouSkill = SentakuSkill;

        }
    }

    //スキルの属性とか表示するやつ
    private void MakeIcon()
    {
        Destroy(IconParent.transform.GetChild(1).gameObject);
        Destroy(IconParent2.transform.GetChild(0).gameObject);

        IconParent.transform.GetChild(0).GetComponent<RectTransform>().localPosition = new Vector2(-370, 420);

        //属性の表示
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

    //ステータスを表示
    private void StetasHyouji()
    {
        int[] HyoujiParameter = new int[15];

        Transform StetasTrans = gameObject.transform.GetChild(0).transform;

        HyoujiParameter = AllDataManege.HyoujiParameterReset();

        StetasTrans.GetChild(0).GetComponent<Text>().text = "HP " + HyoujiParameter[2] + "/" + HyoujiParameter[1]; //HP表示
        StetasTrans.GetChild(1).GetComponent<Text>().text = "MP " + HyoujiParameter[4] + "/" + HyoujiParameter[3]; //MP表示
        StetasTrans.GetChild(2).GetComponent<Text>().text = "ATK " + HyoujiParameter[5];
        StetasTrans.GetChild(3).GetComponent<Text>().text = "DEF " + HyoujiParameter[6];
        StetasTrans.GetChild(4).GetComponent<Text>().text = "INT " + HyoujiParameter[7];
        StetasTrans.GetChild(5).GetComponent<Text>().text = "MIN " + HyoujiParameter[8];
        StetasTrans.GetChild(6).GetComponent<Text>().text = "DEX " + HyoujiParameter[9];
        StetasTrans.GetChild(7).GetComponent<Text>().text = "AGI " + HyoujiParameter[10];
        StetasTrans.GetChild(8).GetComponent<Text>().text = "SPD " + HyoujiParameter[11];
        StetasTrans.GetChild(9).GetComponent<Text>().text = "LUC " + HyoujiParameter[12];
        StetasTrans.GetChild(10).GetComponent<Text>().text = "CHR " + HyoujiParameter[13];

        StetasTrans.GetChild(11).GetComponent<Text>().text = "LV " + HyoujiParameter[0];
        StetasTrans.GetChild(12).GetComponent<Text>().text = HyoujiParameter[14] + "G";
        StetasTrans.GetChild(13).GetComponent<Text>().text = "<size=65>EXP</size> " + HyoujiParameter[15];
    }
}
