using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DangeonBagScript : MonoBehaviour
{
    public DangeonManeger DangeonManeger;

    public GameObject SetumeiWindowPrefab, IconParentPrefab;
    private GameObject SetumeiWindow, IconParent;

    private RectTransform ThisRect;

    private BaseItemData SentakuItem;
    private BaseSkillData SentakuSkill;

    private bool InFlag, OutFlag;

    public float Speed;

    // Start is called before the first frame update
    void Start()
    {
        InFlag = true;

        ThisRect = gameObject.GetComponent<RectTransform>();

        ItemImageKousin();
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
                OutFlag = false;

                DangeonManeger.CanCommandFlag = true;

                Destroy(gameObject);
            }
        }

        ItemImageKousin();
    }

    //ボタン関連
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

    public void BagButtonDown(int value)
    {
        //もし装備欄のアイテムを選択していたら
        if (value < 3)
        {
            AllAudioManege.PlaySE(0);

            SetumeiWindow = Instantiate(SetumeiWindowPrefab, gameObject.transform.parent.transform);

            SentakuItem = AllDataManege.NowSoubi[value];

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

                MakeIcon();

                return;
            }
            else if (SentakuItem.GetItemType() == 0) //無だったら
            {
                Destroy(SetumeiWindow.transform.GetChild(4).gameObject);
                Destroy(SetumeiWindow.transform.GetChild(6).gameObject);

                SetumeiWindow.transform.GetChild(5).GetComponent<RectTransform>().localPosition = new Vector3(0, -400, 0);
            }
        }
        //もしバッグのアイテムを選択していたら
        else if (value >= 3)
        {
            AllAudioManege.PlaySE(0);

            SetumeiWindow = Instantiate(SetumeiWindowPrefab, gameObject.transform.parent.transform);

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
                        SetumeiWindow.GetComponent<StetasSetumeiScript>().DangeonManeger = DangeonManeger;

                        SetumeiWindow.transform.GetChild(4).transform.GetChild(0).GetComponent<Text>().text = "装備する";
                        SetumeiWindow.GetComponent<StetasSetumeiScript>().SoubiOrHazusi = false;

                        //アイコンを作る
                        IconParent = Instantiate(IconParentPrefab, SetumeiWindow.transform);

                        MakeIcon();

                    }

                    return;

                //消費アイテムの場合
                case 4:

                    {
                        SetumeiWindow.GetComponent<StetasSetumeiScript>().SentakuNumber = value;
                        SetumeiWindow.GetComponent<StetasSetumeiScript>().DangeonManeger = DangeonManeger;

                        //アイコンを作る
                        IconParent = Instantiate(IconParentPrefab, SetumeiWindow.transform);

                        MakeIcon();

                        //もし戦闘中にしか使えないアイテムを使おうとしたら
                        if (SentakuItem.GetUseBasyo() == 1 && (!DangeonManeger.BattleFlag || DangeonManeger.BattleEndFlag))
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

    //アイテムの画像更新
    public void ItemImageKousin()
    {
        for (int i = 0; i < 3; i++)
        {
            gameObject.transform.GetChild(0).transform.GetChild(i).GetComponent<Image>().sprite = AllDataManege.NowSoubi[i].GetItemSprite();
        }

        for (int i = 0; i < 9; i++)
        {
            gameObject.transform.GetChild(1).transform.GetChild(i).GetComponent<Image>().sprite = AllDataManege.BagItemList[i].GetItemSprite();
        }
    }

    //アイコンづくり
    //装備アイテムの属性とか表示するやつ
    private void MakeIcon()
    {
        //普通の装備武器アイテム
        if (SentakuItem.GetItemType() == 1)
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

        //防具アイテムだったら
        if (SentakuItem.GetItemType() == 2)
        {
            Destroy(IconParent.gameObject);
        }

        //アクセサリーアイテムだったら
        if (SentakuItem.GetItemType() == 3)
        {
            Destroy(IconParent.gameObject);
        }

        //消費アイテムだったら
        if (SentakuItem.GetItemType() == 4)
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

    }
}
