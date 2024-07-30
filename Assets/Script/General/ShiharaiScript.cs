using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ShiharaiScript : MonoBehaviour
{
    public GameObject YadoyaPrefab, LevelUpPrefab, SceneChangePrefab;
    private GameObject YadoyaChange, LevelUpWindow;

    private DangeonManeger DangeonManeger;

    private RectTransform ThisRect;

    public int ShiharaiType;
    private int ShiharaiMoney;

    private string ShiharaiText;

    public float Speed;
    private bool InFlag, OutFlag, NebikiFlag;

    public bool EscapeFlag;
    //false 失敗 true 成功

    // Start is called before the first frame update
    void Start()
    {
        InFlag = true;

        ThisRect = this.GetComponent<RectTransform>();

        //どこでこのウィンドウを呼び出したかでテキストを変える
        switch (ShiharaiType)
        {
            //訓練場
            case 3:

                {
                    ShiharaiText = "\n訓練しますか？\n\n<size=40>※" + AllDataManege.SomeIntData.GetKunrenGold(AllDataManege.CharactorData.GetParameter()[0] - 1) + "ゴールドかかります\n※１レベル上がります</size>";

                    gameObject.transform.GetChild(2).GetComponent<Text>().text = ShiharaiText;
                }

                break;

            //ダンジョン内
            case 4:

                {
                    DangeonManeger = GameObject.Find("DangeonManeger").GetComponent<DangeonManeger>();

                    gameObject.transform.GetChild(1).transform.GetChild(0).GetComponent<Text>().text = "出る";

                    ShiharaiText = DangeonManeger.DangeonData.DangeonName + "\nから脱出しますか？\n\n<size=40>※次回入った時は最初からになります</size>";

                    gameObject.transform.GetChild(2).GetComponent<Text>().text = ShiharaiText;
                }

                break;

            //戦闘中
            case 5:

                {
                    DangeonManeger = GameObject.Find("DangeonManeger").GetComponent<DangeonManeger>();

                    gameObject.transform.GetChild(1).transform.GetChild(0).GetComponent<Text>().text = "逃げる";

                    ShiharaiText = "\n戦闘から逃げ出しますか？";

                    gameObject.transform.GetChild(2).GetComponent<Text>().text = ShiharaiText;
                }

                break;

            //引退
            case 6:

                {
                    gameObject.transform.GetChild(2).GetComponent<Text>().text = "\n冒険を終わらせますか？\n\n<size=40>※レベルに対応した旅の経験を取得します</size>";

                    gameObject.transform.GetChild(1).transform.GetChild(0).GetComponent<Text>().text = "引退する";
                }

                break;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (InFlag)
        {
            ThisRect.localScale = new Vector3(ThisRect.localScale.x + (Speed * Time.deltaTime), ThisRect.localScale.y + (Speed * Time.deltaTime), 1);

            if (ThisRect.localScale.x >= 1f)
            {
                InFlag = false;
            }
        }

        if (OutFlag)
        {
            ThisRect.localScale = new Vector3(ThisRect.localScale.x - (Speed * Time.deltaTime), ThisRect.localScale.y - (Speed * Time.deltaTime), 1);

            if (ThisRect.localScale.x <= 0f)
            {
                if (!LevelUpWindow)
                {
                    TownManegerScript.CanCommandFlag = true;
                }

                //ダンジョン内にいる時
                if (ShiharaiType == 4 && !DangeonManeger.SceneChange)
                {
                    AllDataManege.TokusyuJoukyou = 0;

                    DangeonManeger.CanCommandFlag = true;
                }

                //逃走に失敗したとき
                if (ShiharaiType == 5 && !EscapeFlag)
                {
                    DangeonManeger.CanCommandFlag = true; //あとでここを敵の行動に置き換える
                }

                //引退を拒否したとき
                if (ShiharaiType == 6)
                {
                    TownManegerScript.CanCommandFlag = true;
                }

                Destroy(gameObject);
            }
        }

        //広告の表示
        if (YadoyaChange && YadoyaChange.GetComponent<Image>().color.a == 1f)
        {
            //宿屋にいる時
            if (ShiharaiType == 1 || ShiharaiType == 2)
            {
                TownManegerScript.CanCommandFlag = true;

                TownManegerScript.TownMessageWindow.ThisTextMesh.text = "";
                TownManegerScript.TownMessageWindow.MessageText = new string[1];

                //四分の一で広告を表示する
                if (Random.Range(0,4) == 1)
                {
                    Time.timeScale = 0;

                    //AllDataManege.AdsListener.ShowMovieAd();
                    //AllDataManege.AdsListener.AdsType = 1;

                    AllDataManege.AdmobListener.AdmobType = 1;
                    AllDataManege.AdmobListener.UserChoseToWatchAd();

                }
                else
                {
                    TownManegerScript.TownMessageWindow.MessageText[0] = "ぐっすりでしたよ";
                }

                if (NebikiFlag)
                {
                    TownManegerScript.TownMessageWindow.MessageText[0] += "\n（値引きしてもらったようだ）";
                }

                TownManegerScript.TownMessageWindow.MesType = 3;
                TownManegerScript.MessageWindowReset();

                Destroy(this.gameObject);
            }

            //引退するとき
            if (ShiharaiType == 6)
            {
                YadoyaChange.GetComponent<Animation>().enabled = false;

                SceneManager.LoadScene("BoukenEnd");
            }

        }

        ThisRect.localScale = new Vector3(Mathf.Clamp01(ThisRect.localScale.x), Mathf.Clamp01(ThisRect.localScale.x), 1);
    }

    public void MouseDown(int value)
    {
        if (value == 1)
        {
            AllAudioManege.PlaySE(6);

            OutFlag = true;

            //戦闘中ならCanCommandFlagを元に戻す
            if (ShiharaiType==5)
            {
                DangeonManeger.CanCommandFlag = true;
            }
        }

        if (value == 2)
        {
            AllAudioManege.PlaySE(0);

            //どこでこのウィンドウを呼び出したのかで処理を変える
            switch (ShiharaiType)
            {
                //宿屋（汚い部屋）
                case 1:

                    {
                        ShiharaiMoney = AllDataManege.SomeIntData.GetYadoyaGold(AllDataManege.CharactorData.KihonParameter[0] - 1);

                        //値引き
                        if (Random.Range(0, 100) <= AllDataManege.CharactorData.GetParameter()[12])
                        {
                            ShiharaiMoney -= Mathf.CeilToInt(AllDataManege.CharactorData.GetParameter()[1] * 0.05f);

                            NebikiFlag = true;

                            if (ShiharaiMoney < 1)
                            {
                                ShiharaiMoney = 1;
                            }
                        }

                        //もしお金が足りてたら
                        if (AllDataManege.CharactorData.GetSyojikin() - ShiharaiMoney >= 0)
                        {
                            AllDataManege.CharactorData.SetSyojikin(AllDataManege.CharactorData.GetSyojikin() - ShiharaiMoney);

                            AllDataManege.StetasWariaiKaihuku(0.3f, 3);

                            YadoyaChange = Instantiate(YadoyaPrefab, GameObject.Find("SceneChangeCanvas").transform);
                        }
                        else
                        {
                            TownManegerScript.TownMessageWindow.ThisTextMesh.text = "";
                            TownManegerScript.TownMessageWindow.MessageText = new string[1];
                            TownManegerScript.TownMessageWindow.MessageText[0] = "おや、お金が足りないようですよ";

                            TownManegerScript.TownMessageWindow.MesType = 3;
                            TownManegerScript.MessageWindowReset();
                        }

                        AllDataManege.DataSave();

                    }

                    break;

                //宿屋（綺麗な部屋）
                case 2:

                    {
                        ShiharaiMoney = AllDataManege.SomeIntData.GetYadoyaGold(AllDataManege.CharactorData.KihonParameter[0] - 1) * 2;

                        //値引き
                        if (Random.Range(0, 100) <= AllDataManege.CharactorData.GetParameter()[12])
                        {
                            ShiharaiMoney -= Mathf.CeilToInt(AllDataManege.CharactorData.GetParameter()[1] * 0.1f);

                            NebikiFlag = true;

                            if (ShiharaiMoney < 1)
                            {
                                ShiharaiMoney = 1;
                            }
                        }

                        //お金が足りていたら
                        if (AllDataManege.CharactorData.GetSyojikin() - ShiharaiMoney >= 0)
                        {
                            AllDataManege.CharactorData.SetSyojikin(AllDataManege.CharactorData.GetSyojikin() - ShiharaiMoney);

                            AllDataManege.StetasWariaiKaihuku(0.8f, 3);

                            YadoyaChange = Instantiate(YadoyaPrefab, GameObject.Find("SceneChangeCanvas").transform);
                        }
                        else
                        {
                            TownManegerScript.TownMessageWindow.ThisTextMesh.text = "";
                            TownManegerScript.TownMessageWindow.MessageText = new string[1];
                            TownManegerScript.TownMessageWindow.MessageText[0] = "おや、お金が足りないようですよ";

                            TownManegerScript.TownMessageWindow.MesType = 3;
                            TownManegerScript.MessageWindowReset();
                        }

                        AllDataManege.DataSave();
                    }

                    break;

                //訓練場
                case 3:

                    {
                        ShiharaiMoney = AllDataManege.SomeIntData.GetKunrenGold(AllDataManege.CharactorData.GetParameter()[0] - 1);

                        //お金が足りてたら
                        if (AllDataManege.CharactorData.GetSyojikin() - ShiharaiMoney >= 0)
                        {
                            //レベルアップウィンドウを作成
                            LevelUpWindow = Instantiate(LevelUpPrefab, gameObject.transform.parent.transform);

                            LevelUpWindow.GetComponent<LevelUpWindow>().LevelUpType = 1;
                            LevelUpWindow.GetComponent<LevelUpWindow>().LevelUpKaisu = 1;

                            //その分の経験値を与える
                            AllDataManege.CharactorData.NowExp = AllDataManege.SomeIntData.GetLevelUpExp(AllDataManege.CharactorData.GetParameter()[0] - 1);

                            //支払い
                            AllDataManege.CharactorData.SetSyojikin(AllDataManege.CharactorData.GetSyojikin() - ShiharaiMoney);

                            OutFlag = true;
                        }
                        else
                        {
                            TownManegerScript.TownMessageWindow.ThisTextMesh.text = "";
                            TownManegerScript.TownMessageWindow.MessageText = new string[1];
                            TownManegerScript.TownMessageWindow.MessageText[0] = "金が足りねぇぜ？";

                            TownManegerScript.TownMessageWindow.MesType = 7;
                            TownManegerScript.MessageWindowReset();
                        }
                    }

                    break;

                //ダンジョン内
                case 4:

                    {
                        DangeonManeger.SceneChange = Instantiate(SceneChangePrefab, GameObject.Find("SceneChangeCanvas").transform);

                        OutFlag = true;
                    }

                    break;

                //戦闘中
                case 5:

                    {
                        OutFlag = true;

                        DangeonManeger.TousouKaisuu++;

                        //逃走成功した場合
                        if (EscapeFlag)
                        {
                            DangeonManeger.BattleChangeImage = Instantiate(DangeonManeger.BattleChangePrefab, GameObject.Find("SceneChangeCanvas").transform).GetComponent<Image>();

                            DangeonManeger.IventChangeSinkou = 3;

                            AllDataManege.TokusyuJoukyou = 1;
                        }
                        //失敗した場合
                        else
                        {
                            DangeonManeger.TokusyuText("逃げられなかった！", 0);

                            //敵の行動
                            DangeonManeger.EnemyKoudou();
                        }
                    }

                    break;

                //引退
                case 6:

                    {
                        YadoyaChange = Instantiate(YadoyaPrefab, GameObject.Find("SceneChangeCanvas").transform);

                        AllDataManege.CanGetKeiken = true;

                        AllDataManege.TokusyuJoukyou = 0;

                        AllDataManege.SomethingData.NowhaveTabikeiken += AllDataManege.SomeIntData.GetGetTabikeiken(AllDataManege.CharactorData.KihonParameter[0] - 1);
                        AllDataManege.SomethingData.NowhaveTabikeiken = Mathf.Clamp(AllDataManege.SomethingData.NowhaveTabikeiken, 0, 9999);

                        AllDataManege.DataSave();

                        PlayerPrefs.DeleteKey("NowPlaying");
                    }

                    break;
            }
        }
    }

    //少し待って終了シーンに移行するコルーチン
    IEnumerator ToSyuryou()
    {
        yield return new WaitForSeconds(2);

        SceneManager.LoadScene("BoukenEnd");

        Destroy(gameObject);
    }
}
