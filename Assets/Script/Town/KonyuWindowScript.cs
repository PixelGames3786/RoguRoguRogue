using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class KonyuWindowScript : MonoBehaviour
{
    private GameObject ButtonParent, KonyuWindow,IconParent,IconParent2;
    public GameObject KonyuWindowPrefab,IconParentPrefab,IconParentPrefab2,ButtonPrefab;

    //[System.NonSerialized]
    public Image[] KonyuButtonImages;

    private RectTransform ThisRect,ButtonRect;

    private Vector3 MotoMousePosi, NowMousePosi;

    private BaseItemData KonyuItem;

    [System.NonSerialized]
    public List<int> ItemList;

    private int ButtonCount;
    public int KonyuType;
    // 1 道具屋 2 武器屋 3 防具屋 4 売却 5 行商人

    public float Speed;
    private float SaikouFloat;

    private bool InFlag, OutFlag, MouseDownFlag, NowIdouFlag;

    public bool CanCommand;

    //ダンジョンコマンドの保持
    [System.NonSerialized]
    public DangeonCommandScript DangeonCommand;

    // Start is called before the first frame update
    void Start()
    {
        InFlag = true;
        CanCommand = true;

        ButtonParent = gameObject.transform.GetChild(0).transform.GetChild(0).gameObject;

        ThisRect = GetComponent<RectTransform>();
        ButtonRect = ButtonParent.GetComponent<RectTransform>();

        if (ItemList.Count<=9)
        {
            KonyuButtonImages = new Image[9];
        }
        else
        {
            //余りを考慮して2を足す
            KonyuButtonImages = new Image[ItemList.Count+2];
        }

        //ボタンを作成
        for (int i=0;i<ItemList.Count;i++)
        {
            MakeKonyuButton(i,false);

            ButtonCount++;
        }

        //もしボタンが中途半端なら
        if (ButtonCount<9)
        {
            for (int i=ButtonCount;i<9;i++)
            {
                MakeKonyuButton(i,true);
                
            }
        }
        if (ButtonCount>9&&ButtonCount%3!=0)
        {
            for (int i=ButtonCount;i%3!=0;i++)
            {
                MakeKonyuButton(i,true);
            }
        }

        //スライドできる最大値をセット
        if (ButtonParent.transform.childCount > 9)
        {
            SaikouFloat = Mathf.CeilToInt((ButtonParent.transform.childCount-9) / 3) * -280;
        }
        else
        {
            SaikouFloat = 0;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (InFlag)
        {
            ThisRect.localScale = new Vector3(ThisRect.localScale.x+(Speed*Time.deltaTime),ThisRect.localScale.y+(Speed*Time.deltaTime),1);

            if (ThisRect.localScale.x>=1.0f)
            {
                InFlag = false;
            }

            ThisRect.localScale = new Vector3(Mathf.Clamp01(ThisRect.localScale.x), Mathf.Clamp01(ThisRect.localScale.y), 1);
        }

        if (OutFlag)
        {
            ThisRect.localScale = new Vector3(ThisRect.localScale.x - (Speed * Time.deltaTime), ThisRect.localScale.y - (Speed * Time.deltaTime), 1);

            if (ThisRect.localScale.x <= 0.0f)
            {
                //もし街の中にいたら
                if (KonyuType!=5)
                {
                    TownManegerScript.CanCommandFlag = true;
                }
                //行商人の時はダンジョンコマンドを有効にする
                else
                {
                    DangeonCommand.CanCommandFlag = true;
                }


                Destroy(this.gameObject);
            }

            ThisRect.localScale = new Vector3(Mathf.Clamp01(ThisRect.localScale.x), Mathf.Clamp01(ThisRect.localScale.y), 1);
        }

        if (MouseDownFlag)
        {
            NowMousePosi = Input.mousePosition;

            if (NowMousePosi!=MotoMousePosi)
            {
                NowIdouFlag = true;
            }

            if (NowIdouFlag)
            {
                ButtonRect.localPosition = new Vector2(ButtonRect.localPosition.x - ((MotoMousePosi.x - NowMousePosi.x) * 1.3f), 0);

                ButtonRect.localPosition = new Vector2(Mathf.Clamp(ButtonRect.localPosition.x, SaikouFloat, 0), 0);

            }

            MotoMousePosi = Input.mousePosition;
        }
    }

    public void SetOutFlag()
    {
        if (!CanCommand)
        {
            return;
        }

        AllAudioManege.PlaySE(6);

        OutFlag = true;
    }

    public void ButtonDown()
    {
        if (!CanCommand)
        {
            return;
        }

        MouseDownFlag = true;

        MotoMousePosi = Input.mousePosition;
        
    }

    public void ButtonUp(int value)
    {
        if (!CanCommand||value==0)
        {
            MouseDownFlag = false;

            NowIdouFlag = false;

            return;
        }

        MouseDownFlag = false;

        if (NowIdouFlag)
        {
            NowIdouFlag = false;

            return;
        }

        //道具購入時
        if (KonyuType==1)
        {
            if (AllDataManege.ItemDataList[ItemList[value-1]].GetItemNumber()==0)
            {
                TownManegerScript.TownMessageWindow.ThisTextMesh.text = "";
                TownManegerScript.TownMessageWindow.MessageText = new string[1];
                TownManegerScript.TownMessageWindow.MessageText[0] = "悪いな。そこはまだ商品がないんだ";

                TownManegerScript.TownMessageWindow.MesType = 5;
                TownManegerScript.MessageWindowReset();

                AllAudioManege.PlaySE(9);

                return;
            }

            AllAudioManege.PlaySE(0);

            KonyuWindowReset(value-1);

            CanCommand = false;
        }

        //武器と防具購入時
        if (KonyuType==2||KonyuType==3)
        {
            if (AllDataManege.ItemDataList[ItemList[value - 1]].GetItemNumber() == 0)
            {
                TownManegerScript.TownMessageWindow.ThisTextMesh.text = "";
                TownManegerScript.TownMessageWindow.MessageText = new string[1];
                TownManegerScript.TownMessageWindow.MessageText[0] = "わりぃ。そこぁまだ置いてねぇんだ";

                TownManegerScript.TownMessageWindow.MesType = 6;
                TownManegerScript.MessageWindowReset();

                AllAudioManege.PlaySE(9);

                return;
            }

            AllAudioManege.PlaySE(0);

            KonyuWindowReset(value - 1);

            CanCommand = false;
        }

        //売却時
        if (KonyuType == 4)
        {
            if (AllDataManege.ItemDataList[ItemList[value-1]].GetItemNumber() == 0)
            {
                TownManegerScript.TownMessageWindow.ThisTextMesh.text = "";
                TownManegerScript.TownMessageWindow.MessageText = new string[1];
                TownManegerScript.TownMessageWindow.MessageText[0] = "ん？そこには何もないよな？";

                TownManegerScript.TownMessageWindow.MesType = 5;
                TownManegerScript.MessageWindowReset();

                AllAudioManege.PlaySE(9);

                return;
            }

            AllAudioManege.PlaySE(0);

            KonyuWindowReset(value - 1);

            CanCommand = false;
        }

        //行商人購入時
        if (KonyuType==5)
        {
            //もし商品がなかったら
            if (AllDataManege.ItemDataList[ItemList[value - 1]].GetItemNumber() == 0)
            {
                DangeonCommand.DangeonManeger.TokusyuText("ごめん、そこには商品がないんだ",0);

                AllAudioManege.PlaySE(9);

                return;
            }

            AllAudioManege.PlaySE(0);

            KonyuWindowReset(value - 1);

            CanCommand = false;
        }
    }

    //購入ウィンドウの内容をセット
    private void KonyuWindowReset(int value)
    {
        KonyuWindow = Instantiate(KonyuWindowPrefab, gameObject.transform.parent.transform);

        KonyuWindow.GetComponent<StetasSetumeiScript>().KonyuWindowScript = this;

        //道具、武器、防具、行商人購入時
        if (KonyuType==1||KonyuType==2||KonyuType==3||KonyuType==5)
        {
            KonyuItem = AllDataManege.ItemDataList[ItemList[value]];

            KonyuWindow.GetComponent<StetasSetumeiScript>().KonyuItem = KonyuItem;

            KonyuWindow.transform.GetChild(1).GetComponent<Image>().sprite = KonyuItem.GetItemSprite();
            KonyuWindow.transform.GetChild(2).GetComponent<Text>().text = "\n" + KonyuItem.GetItemSetumei();
            KonyuWindow.transform.GetChild(3).GetComponent<Text>().text = KonyuItem.GetItemName();
            KonyuWindow.transform.GetChild(4).transform.GetChild(0).GetComponent<Text>().text = "購入";

            KonyuWindow.GetComponent<StetasSetumeiScript>().KonyuType = 1;

            //アイコン作成
            IconParent = Instantiate(IconParentPrefab, KonyuWindow.transform);
            IconParent2 = Instantiate(IconParentPrefab2, KonyuWindow.transform);

            MakeIcon();
        }

        //売却時
        if (KonyuType == 4)
        {
            KonyuItem = AllDataManege.ItemDataList[ItemList[value]];

            KonyuWindow.GetComponent<StetasSetumeiScript>().KonyuItem = KonyuItem;
            KonyuWindow.GetComponent<StetasSetumeiScript>().BaikyakuNum = value;

            KonyuWindow.transform.GetChild(1).GetComponent<Image>().sprite = KonyuItem.GetItemSprite();
            KonyuWindow.transform.GetChild(2).GetComponent<Text>().text = "\n" + KonyuItem.GetItemSetumei();
            KonyuWindow.transform.GetChild(3).GetComponent<Text>().text = KonyuItem.GetItemName();
            KonyuWindow.transform.GetChild(4).transform.GetChild(0).GetComponent<Text>().text = "売却";

            KonyuWindow.GetComponent<StetasSetumeiScript>().KonyuType = 4;

            //アイコン作成
            IconParent = Instantiate(IconParentPrefab, KonyuWindow.transform);
            IconParent2 = Instantiate(IconParentPrefab2, KonyuWindow.transform);

            MakeIcon();
        }

        Destroy(KonyuWindow.transform.GetChild(6).gameObject);
    }

    //装備アイテムの属性とか表示するやつ
    private void MakeIcon()
    {
        //普通の装備武器アイテム
        if (KonyuItem.GetItemType() == 1)
        {
            //武器タイプの表示
            switch (KonyuItem.GetWeaponType())
            {
                //棒
                case 0:

                    IconParent.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = "棒";

                    break;

                //剣
                case 1:

                    IconParent.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = "剣";

                    break;

                //槍
                case 2:

                    IconParent.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = "槍";

                    break;

                //弓
                case 3:

                    IconParent.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = "弓";

                    break;

                //杖
                case 4:

                    IconParent.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = "杖";

                    break;

                //斧
                case 5:

                    IconParent.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = "斧";

                    break;

                //槌
                case 6:

                    IconParent.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = "槌";

                    break;
            }

            switch (KonyuItem.GetZokusei())
            {
                //無
                case 0:

                    IconParent.transform.GetChild(1).transform.GetChild(0).GetComponent<Text>().text = "無";

                    break;

                //炎
                case 1:

                    IconParent.transform.GetChild(1).transform.GetChild(0).GetComponent<Text>().text = "炎";

                    break;

                //風
                case 2:

                    IconParent.transform.GetChild(1).transform.GetChild(0).GetComponent<Text>().text = "風";

                    break;

                //水
                case 3:

                    IconParent.transform.GetChild(1).transform.GetChild(0).GetComponent<Text>().text = "水";

                    break;

                //地
                case 4:

                    IconParent.transform.GetChild(1).transform.GetChild(0).GetComponent<Text>().text = "地";

                    break;
            }
        }

        //防具アイテムだったら
        if (KonyuItem.GetItemType() == 2)
        {
            Destroy(IconParent.gameObject);
        }

        //アクセサリーアイテムだったら
        if (KonyuItem.GetItemType() == 3)
        {
            Destroy(IconParent.gameObject);
        }

        //消費アイテムだったら
        if (KonyuItem.GetItemType() == 4)
        {
            //消費攻撃アイテムだったら
            if (KonyuItem.GetSyouhiType() == 4 || KonyuItem.GetSyouhiType() == 5)
            {
                Destroy(IconParent.transform.GetChild(1).gameObject);

                //属性表示
                switch (KonyuItem.GetZokusei())
                {
                    //無
                    case 0:

                        IconParent.transform.GetChild(1).transform.GetChild(0).GetComponent<Text>().text = "無";

                        break;

                    //炎
                    case 1:

                        IconParent.transform.GetChild(1).transform.GetChild(0).GetComponent<Text>().text = "炎";

                        break;

                    //風
                    case 2:

                        IconParent.transform.GetChild(1).transform.GetChild(0).GetComponent<Text>().text = "風";

                        break;

                    //水
                    case 3:

                        IconParent.transform.GetChild(1).transform.GetChild(0).GetComponent<Text>().text = "水";

                        break;

                    //地
                    case 4:

                        IconParent.transform.GetChild(1).transform.GetChild(0).GetComponent<Text>().text = "地";

                        break;
                }
            }
            else
            {
                Destroy(IconParent);
            }
        }

        //もし売却の時は売却値を表示
        if (KonyuType==4)
        {
            IconParent2.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = KonyuItem.GetBykyaku() + "G";
        }
        else
        {
            IconParent2.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = KonyuItem.GetKonyu() + "G";
        }

        Destroy(IconParent2.transform.GetChild(1).gameObject);
        Destroy(IconParent2.transform.GetChild(2).gameObject);

    }

    //購入ボタンを作成
    private void MakeKonyuButton(int i,bool Amari)
    {
        GameObject Button;

        Button = Instantiate(ButtonPrefab, ButtonParent.transform);

        Button.GetComponent<RectTransform>().localPosition = new Vector2(-275 + (Mathf.CeilToInt(i / 3) * 275), 275 + ((i + 3) % 3) * -275);

        KonyuButtonImages[i] = Button.GetComponent<Image>();

        if (!Amari)
        {
            Button.GetComponent<Image>().sprite = AllDataManege.ItemDataList[ItemList[i]].GetItemSprite();
        }
        else
        {
            Button.GetComponent<Image>().sprite = AllDataManege.ItemDataList[0].GetItemSprite();
        }


        EventTrigger.Entry entry = new EventTrigger.Entry();

        //ボタンが押されたときのトリガー
        entry.eventID = EventTriggerType.PointerDown;
        entry.callback.AddListener((x) => ButtonDown());

        Button.GetComponent<EventTrigger>().triggers.Add(entry);

        entry = new EventTrigger.Entry();

        //ボタンが離されたときのトリガー
        entry.eventID = EventTriggerType.PointerUp;

        if (!Amari)
        {
            entry.callback.AddListener((x) => ButtonUp(i + 1));
        }
        else
        {
            entry.callback.AddListener((x) => ButtonUp(0));
        }

        Button.GetComponent<EventTrigger>().triggers.Add(entry);

        entry = new EventTrigger.Entry();
    }
}
