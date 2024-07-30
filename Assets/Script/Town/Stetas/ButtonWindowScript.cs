using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonWindowScript : MonoBehaviour
{
    private GameObject SetumeiWindow, IconParent, IconParent2;
    public GameObject SetumeiWindowPrefab, IconParentPrefab, IconParentPrefab2;

    private StetasWindowScript StetasWindowScript;

    private BaseItemData SentakuItem;
    private BaseSkillData SentakuSkill;

    public float Speed;
    public bool InFlag, OutFlag;

    // Start is called before the first frame update
    void Start()
    {
        InFlag = true;

        StetasWindowScript = gameObject.transform.parent.GetComponent<StetasWindowScript>();

    }

    // Update is called once per frame
    void Update()
    {
        if (InFlag)
        {
            GetComponent<RectTransform>().localScale = new Vector3(GetComponent<RectTransform>().localScale.x + (Speed * Time.deltaTime), 1, 1);

            if (GetComponent<RectTransform>().localScale.x >= 1f)
            {
                InFlag = false;

                GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            }
        }

        if (OutFlag)
        {
            GetComponent<RectTransform>().localScale = new Vector3(GetComponent<RectTransform>().localScale.x - (Speed * Time.deltaTime), 1, 1);

            if (GetComponent<RectTransform>().localScale.x <= 0f)
            {
                Destroy(gameObject);
            }
        }

        if (gameObject.name == "ButtonWindow(Clone)")
        {
            transform.GetChild(0).GetComponent<Image>().sprite = AllDataManege.ItemDataList[AllDataManege.CharactorData.GetNowSoubi()[0]].GetItemSprite();
            transform.GetChild(1).GetComponent<Image>().sprite = AllDataManege.ItemDataList[AllDataManege.CharactorData.GetNowSoubi()[1]].GetItemSprite();
            transform.GetChild(2).GetComponent<Image>().sprite = AllDataManege.ItemDataList[AllDataManege.CharactorData.GetNowSoubi()[2]].GetItemSprite();
        }

        if (gameObject.name == "ButtonWindow2(Clone)")
        {
            for (int i = 0; i < 9; i++)
            {
                transform.GetChild(i).GetComponent<Image>().sprite = AllDataManege.BagItemList[i].GetItemSprite();
            }
        }

        if (gameObject.name == "ButtonWindow3(Clone)")
        {
            for (int i = 0; i < 6; i++)
            {
                transform.GetChild(i).GetComponent<Image>().sprite = AllDataManege.SkillDataList[AllDataManege.CharactorData.GetSkillMagic()[i]].GetSkillSprite();
            }
        }
    }

    public void MakeItemSetumeiWindow(int value)
    {
        //ウィンドウが開いている途中は作らない
        if (!InFlag && !SetumeiWindow)
        {
            SetumeiWindow = Instantiate(SetumeiWindowPrefab, gameObject.transform.parent.transform);

            StetasWindowScript.ItemSetumeiWindow = SetumeiWindow;
        }
        else
        {
            return;
        }

        //装備欄から開いている場合
        if (value <= 2)
        {
            AllAudioManege.PlaySE(0);

            SentakuItem = AllDataManege.ItemDataList[AllDataManege.CharactorData.GetNowSoubi()[value]];

            SetumeiWindow.transform.GetChild(1).GetComponent<Image>().sprite = SentakuItem.GetItemSprite();
            SetumeiWindow.transform.GetChild(2).GetComponent<Text>().text = SentakuItem.GetItemSetumei();
            SetumeiWindow.transform.GetChild(3).GetComponent<Text>().text = SentakuItem.GetItemName();

            //装備系アイテムだったら
            if (SentakuItem.GetItemType() == 1 || SentakuItem.GetItemType() == 2 || SentakuItem.GetItemType() == 3)
            {
                SetumeiWindow.GetComponent<StetasSetumeiScript>().SentakuNumber = value;

                SetumeiWindow.transform.GetChild(4).transform.GetChild(0).GetComponent<Text>().text = "外す";
                SetumeiWindow.GetComponent<StetasSetumeiScript>().SoubiOrHazusi = true;

                //アイコンを作る
                IconParent = Instantiate(IconParentPrefab, SetumeiWindow.transform);

                MakeIcon(1);

                return;
            }
            else if (SentakuItem.GetItemType() == 0) //無だったら
            {
                Destroy(SetumeiWindow.transform.GetChild(4).gameObject);
                Destroy(SetumeiWindow.transform.GetChild(6).gameObject);

                SetumeiWindow.transform.GetChild(5).GetComponent<RectTransform>().localPosition = new Vector3(0, -400, 0);
            }

        }

        //バッグから開いている場合
        if (value > 2)
        {
            AllAudioManege.PlaySE(0);

            SentakuItem = AllDataManege.BagItemList[value - 3];

            SetumeiWindow.transform.GetChild(1).GetComponent<Image>().sprite = SentakuItem.GetItemSprite();
            SetumeiWindow.transform.GetChild(2).GetComponent<Text>().text = SentakuItem.GetItemSetumei();
            SetumeiWindow.transform.GetChild(3).GetComponent<Text>().text = SentakuItem.GetItemName();

            //アイテムの種類によってアイコンの表示などを変えたりする
            switch (SentakuItem.GetItemType())
            {

                //無か売却アイテムの場合
                case 0:
                case 5:

                    {
                        Destroy(SetumeiWindow.transform.GetChild(4).gameObject);
                        Destroy(SetumeiWindow.transform.GetChild(6).gameObject);

                        SetumeiWindow.transform.GetChild(5).GetComponent<RectTransform>().localPosition = new Vector3(0, -400, 0);
                    }

                    break;

                //装備アイテムの場合
                case 1:
                case 2:
                case 3:

                    {
                        SetumeiWindow.GetComponent<StetasSetumeiScript>().SentakuNumber = value;

                        SetumeiWindow.transform.GetChild(4).transform.GetChild(0).GetComponent<Text>().text = "装備する";
                        SetumeiWindow.GetComponent<StetasSetumeiScript>().SoubiOrHazusi = false;

                        //アイコンを作る
                        IconParent = Instantiate(IconParentPrefab, SetumeiWindow.transform);

                        MakeIcon(1);

                    }

                    return;

                //消費アイテムの場合
                case 4:

                    {
                        SetumeiWindow.GetComponent<StetasSetumeiScript>().SentakuNumber = value;

                        //アイコンを作る
                        IconParent = Instantiate(IconParentPrefab, SetumeiWindow.transform);

                        MakeIcon(1);

                        //もし戦闘中にしか使えないアイテムを使おうとしたら
                        if (SentakuItem.GetUseBasyo() == 1)
                        {
                            SetumeiWindow.transform.GetChild(6).GetComponent<RectTransform>().localPosition = new Vector3(-191, -400);

                            Destroy(SetumeiWindow.transform.GetChild(4).gameObject);

                            return;
                        }

                        SetumeiWindow.transform.GetChild(4).transform.GetChild(0).GetComponent<Text>().text = "使用する";
                        SetumeiWindow.GetComponent<StetasSetumeiScript>().SoubiOrHazusi = false;
                    }

                    return;
            }
        }
    }

    //スキル説明ウィンドウ
    public void MakeSkillSetumeiWindow(int value)
    {
        AllAudioManege.PlaySE(0);

        //ウィンドウが開いている途中には作らない
        if (InFlag || SetumeiWindow)
        {
            return;
        }

        SetumeiWindow = Instantiate(SetumeiWindowPrefab, gameObject.transform.parent.transform);

        StetasWindowScript.ItemSetumeiWindow = SetumeiWindow;

        SentakuSkill = AllDataManege.SkillDataList[AllDataManege.CharactorData.GetSkillMagic()[value]];

        SetumeiWindow.transform.GetChild(1).GetComponent<Image>().sprite = SentakuSkill.GetSkillSprite();
        SetumeiWindow.transform.GetChild(2).GetComponent<Text>().text = SentakuSkill.GetSkillSetumei();
        SetumeiWindow.transform.GetChild(3).GetComponent<Text>().text = SentakuSkill.GetSkillName();

        //アイコンを作る
        IconParent = Instantiate(IconParentPrefab, SetumeiWindow.transform);
        IconParent2 = Instantiate(IconParentPrefab2, SetumeiWindow.transform);

        MakeIcon(2);

        //街の中で使用できないスキルの場合
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

    //装備アイテムの属性とか表示するやつ
    private void MakeIcon(int value)
    {

        //アイテムだったら
        if (value == 1)
        {
            //アイテムの種類で処理を変える
            switch (SentakuItem.GetItemType())
            {
                //武器アイテム
                case 1:

                    {
                        //武器タイプに応じて表示を変える
                        switch (SentakuItem.GetWeaponType())
                        {
                            case 0:

                                IconParent.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = "棒";

                                break;

                            case 1:

                                IconParent.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = "剣";

                                break;

                            case 2:

                                IconParent.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = "槍";

                                break;

                            case 3:

                                IconParent.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = "弓";

                                break;

                            case 4:

                                IconParent.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = "杖";

                                break;

                            case 5:

                                IconParent.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = "斧";

                                break;

                            case 6:

                                IconParent.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = "槌";

                                break;
                        }

                        //属性に応じて表示を変える
                        switch (SentakuItem.GetZokusei())
                        {
                            case 0:

                                IconParent.transform.GetChild(1).transform.GetChild(0).GetComponent<Text>().text = "無";

                                break;

                            case 1:

                                IconParent.transform.GetChild(1).transform.GetChild(0).GetComponent<Text>().text = "炎";

                                break;

                            case 2:

                                IconParent.transform.GetChild(1).transform.GetChild(0).GetComponent<Text>().text = "風";

                                break;

                            case 3:

                                IconParent.transform.GetChild(1).transform.GetChild(0).GetComponent<Text>().text = "水";

                                break;

                            case 4:

                                IconParent.transform.GetChild(1).transform.GetChild(0).GetComponent<Text>().text = "地";

                                break;

                            case 5:

                                IconParent.transform.GetChild(1).transform.GetChild(0).GetComponent<Text>().text = "闇";

                                break;

                            case 6:

                                IconParent.transform.GetChild(1).transform.GetChild(0).GetComponent<Text>().text = "光";

                                break;

                            case 7:

                                IconParent.transform.GetChild(1).transform.GetChild(0).GetComponent<Text>().text = "雷";

                                break;
                        }
                    }

                    break;

                //防具アイテムとアクセサリーアイテム
                case 2: case 3:

                    {
                        Destroy(IconParent.gameObject);
                    }

                    break;

                //消費アイテム
                case 4:

                    {
                        //消費攻撃アイテムだったら
                        if (SentakuItem.GetSyouhiType() == 7)
                        {
                            Destroy(IconParent.transform.GetChild(1).gameObject);

                            IconParent.transform.GetChild(0).GetComponent<RectTransform>().localPosition = new Vector2(-370, 420);

                            //属性に応じて表示を変える
                            switch (SentakuItem.GetZokusei())
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
                        }
                        //回復だったりするとアイコンは必要ないので消す
                        else
                        {
                            Destroy(IconParent);
                        }
                    }

                    break;
            }
        }

        //スキルだったら
        if (value == 2)
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
}
