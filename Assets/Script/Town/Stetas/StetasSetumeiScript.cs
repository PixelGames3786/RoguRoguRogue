using System.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public class StetasSetumeiScript : MonoBehaviour
{
    public DangeonManeger DangeonManeger;

    [System.NonSerialized]
    public bool InFlag, OutFlag, SoubiOrHazusi, SkillSiyou;
    // falseが装備 trueは外し

    [System.NonSerialized]
    public int SentakuNumber, KonyuType, BaikyakuNum;
    private int[] SoubiInt = new int[3];

    [System.NonSerialized]
    public int[] SkillKekka = new int[2];

    private BaseItemData IttanOkiba;

    [System.NonSerialized]
    public BaseItemData KonyuItem;

    [System.NonSerialized]
    public BaseSkillData SiyouSkill;

    public GameObject TyuuiPrefab;
    private GameObject TyuuiWindow;

    [System.NonSerialized]
    public KonyuWindowScript KonyuWindowScript;

    [System.NonSerialized]
    public DangeonJoutaiScript JoutaiScript;

    public float Speed;

    // Start is called before the first frame update
    void Start()
    {
        InFlag = true;

        SoubiInt = AllDataManege.CharactorData.GetNowSoubi();
    }

    // Update is called once per frame
    void Update()
    {
        if (InFlag)
        {
            this.GetComponent<RectTransform>().localScale = new Vector3(1, this.GetComponent<RectTransform>().localScale.y + (Speed * Time.deltaTime), 1);

            if (GetComponent<RectTransform>().localScale.y >= 1f)
            {
                InFlag = false;

                GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            }
        }

        if (OutFlag)
        {
            GetComponent<RectTransform>().localScale = new Vector3(1, this.GetComponent<RectTransform>().localScale.y - (Speed * Time.deltaTime), 1);

            if (GetComponent<RectTransform>().localScale.y <= 0f)
            {

                if (KonyuType == 1 || KonyuType == 4)
                {
                    KonyuWindowScript.CanCommand = true;
                }

                if (JoutaiScript)
                {
                    JoutaiScript.CanButton = true;
                }

                Destroy(gameObject);
            }
        }
    }

    public void SetOutFlag()
    {
        if (TyuuiWindow)
        {
            return;
        }

        OutFlag = true;

        AllAudioManege.PlaySE(6);
    }

    //アイテムを外したり装備したりする処理
    public void ItemHazushiOrSoubi()
    {
        if (TyuuiWindow)
        {
            return;
        }

        //スキル使用時の処理
        if (SkillSiyou)
        {
            SkillKekka = AllDataManege.UseSkill(SiyouSkill);

            //スキル使用に失敗した
            if (SkillKekka[0] == 1)
            {
                //注意ウィンドウがなかったら
                if (!TyuuiWindow)
                {
                    TyuuiWindow = Instantiate(TyuuiPrefab, GameObject.Find("UICanvas").transform);

                    if (SiyouSkill.GetSkillSyouhi()[0] > 0 && SiyouSkill.GetSkillSyouhi()[1] == 0)
                    {
                        TyuuiWindow.transform.GetChild(0).GetComponent<Text>().text = "HPが足りない！";
                    }
                    else if (SiyouSkill.GetSkillSyouhi()[1] > 0 && SiyouSkill.GetSkillSyouhi()[0] == 0)
                    {
                        TyuuiWindow.transform.GetChild(0).GetComponent<Text>().text = "MPが足りない！";
                    }
                    else
                    {
                        TyuuiWindow.transform.GetChild(0).GetComponent<Text>().text = "HPとMPが足りない！";
                    }

                    OutFlag = true;
                }

            }
            //スキル使用に必要な条件を満たしていなかった
            else if (SkillKekka[0] == 2)
            {
                TyuuiWindow = Instantiate(TyuuiPrefab, GameObject.Find("UICanvas").transform);

                TyuuiWindow.transform.GetChild(0).GetComponent<Text>().text = "発動条件を満たしていない！";

                OutFlag = true;
            }
            //スキル使用に成功した
            else if (SkillKekka[0] == 0)
            {
                OutFlag = true;
            }

            return;
        }

        //購入時の処理
        if (KonyuType == 1)
        {
            //もし所持金が足りなかったら
            if (KonyuItem.GetKonyu() > AllDataManege.CharactorData.GetSyojikin())
            {
                AllAudioManege.PlaySE(9);

                //もしダンジョンにいたら（行商人から購入する場合）
                if (SceneManager.GetActiveScene().name == "Dangeon")
                {
                    KonyuWindowScript.DangeonCommand.DangeonManeger.TokusyuText("あれ？お金が足りないみたいだよ",0);

                    MakeTyuiWindow("お金が足りない！");

                }
                //普通に街にいる時
                else
                {
                    TownManegerScript.TownMessageWindow.ThisTextMesh.text = "";
                    TownManegerScript.TownMessageWindow.MessageText = new string[1];
                    TownManegerScript.TownMessageWindow.MessageText[0] = "おっと、所持金が足りないみたいだぜ";

                    TownManegerScript.TownMessageWindow.MesType = 5;
                    TownManegerScript.MessageWindowReset();

                    MakeTyuiWindow("お金が足りない！");
                }

                return;
            }

            //もしバッグに空きがなかったら
            if ((AllDataManege.CharactorData.BagItemInt.All(BagInt => BagInt != 0)))
            {
                AllAudioManege.PlaySE(9);

                //もしダンジョンにいたら（行商人から購入する場合）
                if (SceneManager.GetActiveScene().name == "Dangeon")
                {
                    KonyuWindowScript.DangeonCommand.DangeonManeger.TokusyuText("バッグがいっぱいだからまた来てね",0);

                    MakeTyuiWindow("バッグがいっぱいだ……");
                }
                //普通に街にいる時
                else
                {
                    TownManegerScript.TownMessageWindow.ThisTextMesh.text = "";
                    TownManegerScript.TownMessageWindow.MessageText = new string[1];
                    TownManegerScript.TownMessageWindow.MessageText[0] = "おっと、バッグに空きがないみたいだな\nまた来てくれ";

                    TownManegerScript.TownMessageWindow.MesType = 5;
                    TownManegerScript.MessageWindowReset();

                    MakeTyuiWindow("バッグがいっぱいだ……");

                }

                return;
            }

            //購入処理
            for (int i = 0; i < 9; i++)
            {
                if (AllDataManege.CharactorData.BagItemInt[i] == 0)
                {
                    AllDataManege.CharactorData.NowSyojikin -= KonyuItem.GetKonyu();

                    AllDataManege.CharactorData.BagItemInt[i] = KonyuItem.GetItemNumber();

                    //もしダンジョンにいたら（行商人から購入する場合）
                    if (SceneManager.GetActiveScene().name == "Dangeon")
                    {
                        KonyuWindowScript.DangeonCommand.DangeonManeger.TokusyuText("またごひいきに～",0);
                    }
                    //普通に街にいる時
                    else
                    {
                        TownManegerScript.TownMessageWindow.ThisTextMesh.text = "";
                        TownManegerScript.TownMessageWindow.MessageText = new string[1];
                        TownManegerScript.TownMessageWindow.MessageText[0] = "まいどあり！";

                        TownManegerScript.TownMessageWindow.MesType = 5;
                        TownManegerScript.MessageWindowReset();
                    }

                    AllAudioManege.PlaySE(10);

                    AllDataManege.DataSave();

                    return;
                }
            }

            return;
        }

        //売却時の処理
        if (KonyuType == 4)
        {
            BaseItemData BaikyakuItem = AllDataManege.ItemDataList[AllDataManege.CharactorData.BagItemInt[BaikyakuNum]];

            AllDataManege.CharactorData.SetSyojikin(AllDataManege.CharactorData.GetSyojikin() + KonyuItem.GetBykyaku());

            KonyuWindowScript.KonyuButtonImages[BaikyakuNum].sprite = AllDataManege.ItemDataList[0].GetItemSprite();

            AllDataManege.CharactorData.BagItemInt[BaikyakuNum] = 0;
            KonyuWindowScript.ItemList = new List<int>(AllDataManege.CharactorData.BagItemInt);

            TownManegerScript.TownMessageWindow.ThisTextMesh.text = "";
            TownManegerScript.TownMessageWindow.MessageText = new string[1];
            TownManegerScript.TownMessageWindow.MessageText[0] = "ほらよ、金だ";

            TownManegerScript.TownMessageWindow.MesType = 5;
            TownManegerScript.MessageWindowReset();

            AllAudioManege.PlaySE(10);

            AllDataManege.DataSave();

            OutFlag = true;

            return;
        }

        //装備する場合(使用する場合)
        if (!SoubiOrHazusi)
        {
            BaseItemData SentakuItem = AllDataManege.BagItemList[SentakuNumber - 3];

            OutFlag = true;

            //アイテムの種類に応じて処理を変える 
            switch (SentakuItem.GetItemType())
            {
                //武器の場合
                case 1:

                    {
                        //もし何も武器を装備していなかったら
                        if (AllDataManege.NowSoubi[0].GetItemNumber() == 0)
                        {
                            SoubiInt[0] = AllDataManege.BagItemList[SentakuNumber - 3].GetItemNumber();

                            AllDataManege.CharactorData.BagItemInt[SentakuNumber - 3] = 0;
                        }
                        else //装備をしていたら
                        {
                            //装備中のアイテムのデータを一旦ここに置いておく
                            IttanOkiba = AllDataManege.NowSoubi[0];

                            SoubiInt[0] = AllDataManege.BagItemList[SentakuNumber - 3].GetItemNumber();

                            AllDataManege.CharactorData.BagItemInt[SentakuNumber - 3] = IttanOkiba.GetItemNumber();
                        }

                        AllDataManege.CharactorData.SetNowSoubi(SoubiInt);

                        AllAudioManege.PlaySE(11);

                        AllDataManege.DataSave();
                        
                    }

                    return;

                //防具の場合
                case 2:

                    {
                        //もし何も防具を装備していなかったら
                        if (AllDataManege.NowSoubi[1].GetItemNumber() == 0)
                        {
                            SoubiInt[1] = AllDataManege.BagItemList[SentakuNumber - 3].GetItemNumber();

                            AllDataManege.CharactorData.BagItemInt[SentakuNumber - 3] = 0;
                        }
                        else
                        {
                            IttanOkiba = AllDataManege.NowSoubi[1];

                            SoubiInt[1] = AllDataManege.BagItemList[SentakuNumber - 3].GetItemNumber();

                            AllDataManege.CharactorData.BagItemInt[SentakuNumber - 3] = IttanOkiba.GetItemNumber();
                        }

                        AllDataManege.CharactorData.SetNowSoubi(SoubiInt);

                        AllAudioManege.PlaySE(11);

                        AllDataManege.DataSave();
                    }

                    return;

                //アクセサリーの場合
                case 3:

                    {
                        //もし何もアクセサリーを装備していなかったら
                        if (AllDataManege.NowSoubi[2].GetItemNumber() == 0)
                        {
                            SoubiInt[2] = AllDataManege.BagItemList[SentakuNumber - 3].GetItemNumber();

                            AllDataManege.CharactorData.BagItemInt[SentakuNumber - 3] = 0;
                        }
                        else
                        {
                            IttanOkiba = AllDataManege.NowSoubi[2];

                            SoubiInt[2] = IttanOkiba.GetItemNumber();

                            AllDataManege.CharactorData.BagItemInt[SentakuNumber - 3] = IttanOkiba.GetItemNumber();
                        }

                        AllDataManege.CharactorData.SetNowSoubi(SoubiInt);

                        AllAudioManege.PlaySE(11);

                        AllDataManege.DataSave();

                    }

                    return;

                //消費アイテムの場合
                case 4:

                    {
                        //パーティクルがあったら生成
                        if (SentakuItem.GetParticle())
                        {
                            Instantiate(SentakuItem.GetParticle());
                        }

                        //効果音があったら作る
                        if (SentakuItem.GetSound() != 0)
                        {
                            AllAudioManege.PlaySE(SentakuItem.GetSound());
                        }

                        //アイテムの種類によって実行するスクリプトを変える
                        switch (SentakuItem.GetSyouhiType())
                        {
                            //HP回復系だったら
                            case 1:

                                AllDataManege.StetasKaihuku(SentakuItem.GetKaihukuSuuji(), 1);

                                break;

                            //MP回復系だったら
                            case 2:

                                AllDataManege.StetasKaihuku(SentakuItem.GetKaihukuSuuji(), 2);

                                break;

                            //HPMP回復系だったら
                            case 3:

                                AllDataManege.StetasKaihuku(SentakuItem.GetKaihukuSuuji(), 3);

                                break;

                            //HP割合回復系だったら
                            case 4:

                                AllDataManege.StetasWariaiKaihuku(SentakuItem.GetKaihukuWariai(), 1);

                                break;

                            //MP割合回復系だったら
                            case 5:

                                AllDataManege.StetasWariaiKaihuku(SentakuItem.GetKaihukuWariai(), 2);

                                break;

                            //HPMP割合回復系だったら
                            case 6:

                                AllDataManege.StetasWariaiKaihuku(SentakuItem.GetKaihukuWariai(), 3);

                                break;

                            //相手にダメージを与える系だったら
                            case 7:

                                int ItemDamege = Random.Range(SentakuItem.GetItemDamege()[0], SentakuItem.GetItemDamege()[1]);

                                DangeonManeger.EnemyDamage(ItemDamege, SentakuItem.GetWeaponType(), SentakuItem.GetZokusei());

                                break;

                            //相手にバフを付与する系だったら
                            case 8:

                                AllDataManege.EnemyData.NowJoutai.Add(SentakuItem.GetAddBaffNumber());
                                AllDataManege.EnemyData.JoutaiKeika.Add(AllDataManege.BaffDebaffDataList[SentakuItem.GetAddBaffNumber()].GetKeizokuTurn());

                                break;

                            //自分にバフを付与する系だったら
                            case 9:

                                //バフをセットする
                                AllDataManege.CharactorData.NowJoutai.Add(SentakuItem.GetAddBaffNumber());
                                AllDataManege.CharactorData.JoutaiKeika.Add(AllDataManege.BaffDebaffDataList[SentakuItem.GetAddBaffNumber()].GetKeizokuTurn());

                                break;

                            //確実に逃げれるようになるアイテムなら
                            case 10:

                                DangeonManeger.BattleTokusyu.Add(1);

                                break;
                        }

                        //消費後アイテムを消す
                        AllDataManege.CharactorData.BagItemInt[SentakuNumber - 3] = 0;

                        AllDataManege.DataSave();
                    }

                    break;
            }
        }

        //外す場合
        if (SoubiOrHazusi)
        {
            //もし空きがなかったら
            if ((AllDataManege.BagItemList.All(BagItemList => BagItemList.GetItemNumber() != 0)))
            {
                OutFlag = true;

                if (!TyuuiWindow)
                {
                    TyuuiWindow = Instantiate(TyuuiPrefab, GameObject.Find("UICanvas").transform);

                    TyuuiWindow.transform.GetChild(0).GetComponent<Text>().text = "バッグがいっぱい！";

                    AllAudioManege.PlaySE(9);
                }

                return;
            }

            for (int i = 0; i < 9; i++)
            {
                if (AllDataManege.CharactorData.BagItemInt[i] == 0)
                {
                    AllDataManege.CharactorData.BagItemInt[i] = AllDataManege.NowSoubi[SentakuNumber].GetItemNumber();

                    SoubiInt[SentakuNumber] = 0;

                    AllDataManege.CharactorData.SetNowSoubi(SoubiInt);

                    AllAudioManege.PlaySE(11);

                    AllDataManege.DataSave();

                    OutFlag = true;

                    return;
                }
            }
        }
    }

    //アイテムを捨てる時
    public void ItemSuteru()
    {
        if (TyuuiWindow)
        {
            return;
        }

        if (SentakuNumber <= 2)
        {
            SoubiInt[SentakuNumber] = 0;

            AllDataManege.CharactorData.SetNowSoubi(SoubiInt);
        }

        if (SentakuNumber > 2)
        {
            AllDataManege.CharactorData.BagItemInt[SentakuNumber - 3] = 0;
        }

        AllDataManege.DataSave();

        OutFlag = true;
    }

    //注意ウィンドウを出す
    private void MakeTyuiWindow(string TyuiText)
    {
        //注意ウィンドウがなかったら
        if (!TyuuiWindow)
        {
            TyuuiWindow = Instantiate(TyuuiPrefab, GameObject.Find("UICanvas").transform);

            TyuuiWindow.transform.GetChild(0).GetComponent<Text>().text = TyuiText;
        }
    }
}
