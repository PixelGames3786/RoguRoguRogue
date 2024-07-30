using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TownCommandScript : MonoBehaviour
{
    private TownMessageWindow TownMessageWindow;

    //立ち絵と背景
    private GameObject Tatie, Haikei;

    private GameObject StetasParent,ToushiParent,MiniSceneChange,ShiharaiWindow,KonyuWindow,KakuninWindow;
    public GameObject StetasPrefab,ToushiPrefab,TownCommandPrefab,MiniChangePrefab,ShiharaiPrefab,KonyuWindowPrefab,HelpPrefab,KakuninPrefab,DangeonMapPrefab;

    private GameObject[] CommandButtons = new GameObject[6];

    private List<AnimationClip> Animations = new List<AnimationClip>();

    public int[] CommandKaihou;

    private string ShiharaiText;

    [System.NonSerialized]
    public int ButtonType,CommandType,SentakuInt;
    //1 街メニューを出す 2 街メニューに戻る

    private bool KakuninWindowFlag;

    // Start is called before the first frame update
    void Start()
    {
        CommandKaihou = AllDataManege.TownData.GetCommandKaihou();

        TownMessageWindow = TownManegerScript.MessageWindow.GetComponent<TownMessageWindow>();

        for (int i=1;i<gameObject.transform.childCount;i++)
        {
            CommandButtons[i] = gameObject.transform.GetChild(i).gameObject;
        }

        foreach (AnimationState anim in this.GetComponent<Animation>())
        {
            this.Animations.Add(anim.clip);
        }

        //いる場所によってコマンドとかを変える
        switch (CommandType)
        {
            //村長の家にいる時
            case 1:

                CommandButtons[1].transform.GetChild(0).GetComponent<Text>().text = "やるべき事";
                CommandButtons[2].transform.GetChild(0).GetComponent<Text>().text = "無駄話";
                CommandButtons[3].transform.GetChild(0).GetComponent<Text>().text = "引退";
                CommandButtons[4].transform.GetChild(0).GetComponent<Text>().text = "戻る";

                Destroy(CommandButtons[5]);

                break;

            //宿屋にいる時
            case 2:

                CommandButtons[1].transform.GetChild(0).GetComponent<Text>().text = "無駄話";
                CommandButtons[2].transform.GetChild(0).GetComponent<Text>().text = "汚い部屋";
                CommandButtons[3].transform.GetChild(0).GetComponent<Text>().text = "綺麗な部屋";
                CommandButtons[4].transform.GetChild(0).GetComponent<Text>().text = "戻る";

                Destroy(CommandButtons[5]);

                break;

            //路地裏にいる時
            case 3:

                CommandButtons[1].transform.GetChild(0).GetComponent<Text>().text = "無駄話";
                CommandButtons[2].transform.GetChild(0).GetComponent<Text>().text = "汚い部屋";
                CommandButtons[3].transform.GetChild(0).GetComponent<Text>().text = "綺麗な部屋";
                CommandButtons[4].transform.GetChild(0).GetComponent<Text>().text = "戻る";

                Destroy(CommandButtons[5]);

                break;

            //道具屋にいる時
            case 4:

                CommandButtons[1].transform.GetChild(0).GetComponent<Text>().text = "無駄話";
                CommandButtons[2].transform.GetChild(0).GetComponent<Text>().text = "商品";
                CommandButtons[3].transform.GetChild(0).GetComponent<Text>().text = "売却";
                CommandButtons[4].transform.GetChild(0).GetComponent<Text>().text = "戻る";

                Destroy(CommandButtons[5]);

                break;

            //武器屋にいる時
            case 5:

                CommandButtons[1].transform.GetChild(0).GetComponent<Text>().text = "無駄話";
                CommandButtons[2].transform.GetChild(0).GetComponent<Text>().text = "武器商品";
                CommandButtons[3].transform.GetChild(0).GetComponent<Text>().text = "防具商品";
                CommandButtons[4].transform.GetChild(0).GetComponent<Text>().text = "売却";
                CommandButtons[5].transform.GetChild(0).GetComponent<Text>().text = "戻る";

                break;

            //訓練場にいる時
            case 6:

                CommandButtons[1].transform.GetChild(0).GetComponent<Text>().text = "無駄話";
                CommandButtons[2].transform.GetChild(0).GetComponent<Text>().text = "訓練";
                CommandButtons[3].transform.GetChild(0).GetComponent<Text>().text = "戻る";

                Destroy(CommandButtons[4]);
                Destroy(CommandButtons[5]);

                break;
        }

    }

    // Update is called once per frame
    void Update()
    {
        //街選択コマンドを出す
        if (!this.gameObject.GetComponent<Animation>().isPlaying)
        {
            if (ButtonType == 1)
            {
                Instantiate(TownCommandPrefab, gameObject.transform.parent);
                Destroy(gameObject);
            }
        }

        //ミニシーンチェンジ　左から
        if (MiniSceneChange&&MiniSceneChange.GetComponent<RectTransform>().localPosition.x >= 0)
        {
            //村長の家
            if (SentakuInt == 1)
            {
                Destroy(GameObject.Find("Tatie(Clone)").gameObject);
                Destroy(GameObject.Find("KobetuHaikei(Clone)").gameObject);

                TownMessageWindow.ThisTextMesh.text = "";

                Destroy(gameObject);
            }

            //宿屋
            if (SentakuInt == 2)
            {

                Destroy(GameObject.Find("Tatie(Clone)").gameObject);
                Destroy(GameObject.Find("KobetuHaikei(Clone)").gameObject);
                Destroy(GameObject.Find("TownSyojikinWindow(Clone)").gameObject);

                TownMessageWindow.ThisTextMesh.text = "";

                Destroy(gameObject);
            }

            //路地裏
            if (SentakuInt == 3)
            {
                Destroy(GameObject.Find("KobetuHaikei(Clone)").gameObject);

                TownMessageWindow.ThisTextMesh.text = "";

                Destroy(gameObject);
            }

        }

        //ミニシーンチェンジ　右から
        if (MiniSceneChange && MiniSceneChange.GetComponent<RectTransform>().localPosition.x <= 0)
        {
            //道具屋
            if (SentakuInt == 5)
            {
                Destroy(GameObject.Find("Tatie(Clone)").gameObject);
                Destroy(GameObject.Find("KobetuHaikei(Clone)").gameObject);
                Destroy(GameObject.Find("TownSyojikinWindow(Clone)").gameObject);

                TownMessageWindow.ThisTextMesh.text = "";

                Destroy(gameObject);
            }
            
            //ダンジョンマップ
            if (SentakuInt == 6)
            {
                Instantiate(DangeonMapPrefab,gameObject.transform.parent.transform);

                Destroy(TownManegerScript.MessageWindow);

                Destroy(gameObject);
            }
        }

        //確認ウィンドウ
        if (KakuninWindowFlag)
        {
            if (!KakuninWindow)
            {
                KakuninWindowFlag = false;
                TownManegerScript.CanCommandFlag = true;
            }
           
        }
    }

    public void ButtonsMouseDown(int value)
    {
        if (!TownManegerScript.CanCommandFlag)
        {
            return;
        }

        //通常の時
        if (CommandType==0)
        {
            if (value==2)
            {
                MiniSceneChange = Instantiate(MiniChangePrefab, GameObject.Find("SceneChangeCanvas").transform);

                MiniSceneChange.GetComponent<MiniSceneChangeScript>().ChangeEx = 8;
                MiniSceneChange.GetComponent<MiniSceneChangeScript>().ChangeType = 0;

                MiniSceneChange.GetComponent<RectTransform>().localPosition = new Vector3(2160, 0, 0);

                SentakuInt = 6;

                TownMessageWindow.CanMesFlag = false;

                TownManegerScript.CanCommandFlag = false;

                AllAudioManege.PlaySE(7);
            }

            if (value==3)
            {

                AllAudioManege.PlaySE(0);

                StetasParent = Instantiate(StetasPrefab, gameObject.transform.parent.transform);
            }

            if (value==4)
            {
                AllAudioManege.PlaySE(0);

                gameObject.GetComponent<Animation>().Play(Animations[1].name);

                ButtonType = 1;
            }

            if (value==5)
            {
                AllAudioManege.PlaySE(0);

                if (AllDataManege.TownData.GetHattenLevel()<12)
                {
                    ToushiParent = Instantiate(ToushiPrefab, gameObject.transform.parent.transform);
                }
                else
                {
                    KakuninWindow = Instantiate(KakuninPrefab,gameObject.transform.parent.transform);

                    KakuninWindow.transform.GetChild(0).GetComponent<Text>().text = "これ以上投資する必要はない";

                    KakuninWindowFlag = true;
                    TownManegerScript.CanCommandFlag = false;
                }

            }

            if (value==6)
            {
                AllAudioManege.PlaySE(0);

                Instantiate(HelpPrefab,gameObject.transform.parent.transform);
            }

            TownManegerScript.CanCommandFlag = false;
        }

        //村長の家にいる場合
        if (CommandType == 1)
        {
            if (value==2)
            {
                AllAudioManege.PlaySE(0);

                TownMessageWindow.ThisTextMesh.text = "";
                TownManegerScript.MessageWindowReset();
                TownMessageWindow.MessageText[0] = TownManegerScript.MurabitoText[UnityEngine.Random.Range(AllDataManege.SomeIntData.GetTownTextMin(AllDataManege.TownData.HattenLevel), AllDataManege.SomeIntData.GetTownTextMax(AllDataManege.TownData.HattenLevel))];

                TownMessageWindow.MesType = 2;
            }

            if (value == 3)
            {
                AllAudioManege.PlaySE(0);

                TownMessageWindow.ThisTextMesh.text = "";
                TownManegerScript.MessageWindowReset();

                TownMessageWindow.MessageText[0] = TownManegerScript.MurabitoText[UnityEngine.Random.Range(AllDataManege.SomeIntData.GetTownTextMin(AllDataManege.TownData.HattenLevel)+3, AllDataManege.SomeIntData.GetTownTextMax(AllDataManege.TownData.HattenLevel)+3)];

                TownMessageWindow.MesType = 2;
            }

            if (value==4)
            {
                ShiharaiWindow = Instantiate(ShiharaiPrefab,gameObject.transform.parent.transform);

                ShiharaiWindow.GetComponent<ShiharaiScript>().ShiharaiType = 6;

                TownManegerScript.CanCommandFlag = false;
            }

            if (value == 5)
            {
                AllAudioManege.PlaySE(7);

                MiniSceneChange = Instantiate(MiniChangePrefab, GameObject.Find("SceneChangeCanvas").transform);

                MiniSceneChange.GetComponent<MiniSceneChangeScript>().ChangeEx = 2;
                MiniSceneChange.GetComponent<MiniSceneChangeScript>().ChangeType = 1;

                MiniSceneChange.GetComponent<RectTransform>().localPosition = new Vector3(-2160, 0, 0);

                SentakuInt = 1;

                TownMessageWindow.CanMesFlag = false;

                TownManegerScript.CanCommandFlag = false;
            }
            
        }

        //宿屋にいる場合
        if (CommandType==2)
        {
            if (value == 2)
            {
                AllAudioManege.PlaySE(0);

                TownMessageWindow = TownManegerScript.MessageWindow.GetComponent<TownMessageWindow>();

                TownMessageWindow.ThisTextMesh.text = "";
                TownMessageWindow.MessageText = new string[1];
                TownMessageWindow.MessageText[0] = TownManegerScript.MurabitoText[UnityEngine.Random.Range(AllDataManege.SomeIntData.GetTownTextMin(AllDataManege.TownData.HattenLevel) + 6, AllDataManege.SomeIntData.GetTownTextMax(AllDataManege.TownData.HattenLevel) + 6)];

                TownMessageWindow.MesType = 3;
                TownManegerScript.MessageWindowReset();

            }

            if (value == 3)
            {
                AllAudioManege.PlaySE(0);

                TownManegerScript.CanCommandFlag = false;

                ShiharaiText = "\n汚い部屋に泊まりますか？\n\n<size=40>※" + AllDataManege.SomeIntData.GetYadoyaGold(AllDataManege.CharactorData.KihonParameter[0]-1) + "ゴールドかかります\n※全体の三割回復します </size>";

                ShiharaiWindow = Instantiate(ShiharaiPrefab,this.gameObject.transform.parent.transform);

                ShiharaiWindow.GetComponent<ShiharaiScript>().ShiharaiType = 1;
                ShiharaiWindow.transform.GetChild(2).GetComponent<Text>().text=ShiharaiText;

            }

            if (value == 4)
            {
                AllAudioManege.PlaySE(0);

                TownManegerScript.CanCommandFlag = false;

                ShiharaiText = "\n綺麗な部屋に泊まりますか？\n\n<size=40>※" + AllDataManege.SomeIntData.GetYadoyaGold(AllDataManege.CharactorData.KihonParameter[0] - 1)*2 + "ゴールドかかります\n※全体の八割回復します </size>";

                ShiharaiWindow = Instantiate(ShiharaiPrefab, this.gameObject.transform.parent.transform);

                ShiharaiWindow.GetComponent<ShiharaiScript>().ShiharaiType = 2;
                ShiharaiWindow.transform.GetChild(2).GetComponent<Text>().text = ShiharaiText;

            }

            if (value == 5)
            {
                AllAudioManege.PlaySE(7);

                MiniSceneChange = Instantiate(MiniChangePrefab, GameObject.Find("SceneChangeCanvas").transform);

                MiniSceneChange.GetComponent<MiniSceneChangeScript>().ChangeEx = 2;
                MiniSceneChange.GetComponent<MiniSceneChangeScript>().ChangeType = 1;

                MiniSceneChange.GetComponent<RectTransform>().localPosition = new Vector3(-2160, 0, 0);

                SentakuInt = 2;

                TownMessageWindow.CanMesFlag = false;

                TownManegerScript.CanCommandFlag = false;
            }
        }

        //路地裏にいる場合
        if (CommandType==3)
        {
            if (value == 2)
            {
                AllAudioManege.PlaySE(7);

                MiniSceneChange = Instantiate(MiniChangePrefab, GameObject.Find("SceneChangeCanvas").transform);

                MiniSceneChange.GetComponent<MiniSceneChangeScript>().ChangeEx = 2;
                MiniSceneChange.GetComponent<MiniSceneChangeScript>().ChangeType = 1;

                MiniSceneChange.GetComponent<RectTransform>().localPosition = new Vector3(-2160, 0, 0);

                SentakuInt = 3;

                TownMessageWindow.CanMesFlag = false;

                TownManegerScript.CanCommandFlag = false;
            }
        }

        //道具屋にいる場合
        if (CommandType == 4)
        {
            if (value == 2)
            {
                AllAudioManege.PlaySE(0);

                TownMessageWindow = TownManegerScript.MessageWindow.GetComponent<TownMessageWindow>();

                TownMessageWindow.ThisTextMesh.text = "";
                TownMessageWindow.MessageText = new string[1];
                TownMessageWindow.MessageText[0] = TownManegerScript.MurabitoText[UnityEngine.Random.Range(AllDataManege.SomeIntData.GetTownTextMin(AllDataManege.TownData.HattenLevel) + 9, AllDataManege.SomeIntData.GetTownTextMax(AllDataManege.TownData.HattenLevel) + 9)];

                TownMessageWindow.MesType = 5;
                TownManegerScript.MessageWindowReset();

            }

            if (value == 3)
            {
                AllAudioManege.PlaySE(0);

                TownMessageWindow = TownManegerScript.MessageWindow.GetComponent<TownMessageWindow>();

                TownMessageWindow.ThisTextMesh.text = "";
                TownMessageWindow.MessageText = new string[1];
                TownMessageWindow.MessageText[0] = "どうぞ見ていってくれ";

                TownMessageWindow.MesType = 5;
                TownManegerScript.MessageWindowReset();

                KonyuWindow = Instantiate(KonyuWindowPrefab,gameObject.transform.parent.transform);
                KonyuWindow.GetComponent<KonyuWindowScript>().KonyuType=1;
                KonyuWindow.GetComponent<KonyuWindowScript>().ItemList = AllDataManege.TownData.DouguyaItemInt;

                TownManegerScript.CanCommandFlag = false;
            }

            if (value == 4)
            {
                AllAudioManege.PlaySE(0);

                TownMessageWindow = TownManegerScript.MessageWindow.GetComponent<TownMessageWindow>();

                TownMessageWindow.ThisTextMesh.text = "";
                TownMessageWindow.MessageText = new string[1];
                TownMessageWindow.MessageText[0] = "何を売るんだ？";

                TownMessageWindow.MesType = 5;
                TownManegerScript.MessageWindowReset();

                KonyuWindow = Instantiate(KonyuWindowPrefab, gameObject.transform.parent.transform);
                KonyuWindow.GetComponent<KonyuWindowScript>().KonyuType = 4;
                KonyuWindow.GetComponent<KonyuWindowScript>().ItemList = new List<int>(AllDataManege.CharactorData.BagItemInt);

                TownManegerScript.CanCommandFlag = false;
            }

            if (value == 5)
            {
                AllAudioManege.PlaySE(7);

                MiniSceneChange = Instantiate(MiniChangePrefab, GameObject.Find("SceneChangeCanvas").transform);

                MiniSceneChange.GetComponent<MiniSceneChangeScript>().ChangeEx = 2;
                MiniSceneChange.GetComponent<MiniSceneChangeScript>().ChangeType = 0;

                MiniSceneChange.GetComponent<RectTransform>().localPosition = new Vector3(2160, 0, 0);

                SentakuInt = 5;

                TownMessageWindow.CanMesFlag = false;

                TownManegerScript.CanCommandFlag = false;
            }
        }

        //武器屋にいる場合
        if (CommandType == 5)
        {
            if (value == 2)
            {
                AllAudioManege.PlaySE(0);

                TownMessageWindow = TownManegerScript.MessageWindow.GetComponent<TownMessageWindow>();

                TownMessageWindow.ThisTextMesh.text = "";
                TownMessageWindow.MessageText = new string[1];
                TownMessageWindow.MessageText[0] = TownManegerScript.MurabitoText[UnityEngine.Random.Range(AllDataManege.SomeIntData.GetTownTextMin(AllDataManege.TownData.HattenLevel) + 12, AllDataManege.SomeIntData.GetTownTextMax(AllDataManege.TownData.HattenLevel) + 12)];

                TownMessageWindow.MesType = 6;
                TownManegerScript.MessageWindowReset();

            }

            if (value == 3)
            {
                AllAudioManege.PlaySE(0);

                TownMessageWindow = TownManegerScript.MessageWindow.GetComponent<TownMessageWindow>();

                TownMessageWindow.ThisTextMesh.text = "";
                TownMessageWindow.MessageText = new string[1];
                TownMessageWindow.MessageText[0] = "おう！見てけ！";

                TownMessageWindow.MesType = 6;
                TownManegerScript.MessageWindowReset();

                KonyuWindow = Instantiate(KonyuWindowPrefab, gameObject.transform.parent.transform);
                KonyuWindow.GetComponent<KonyuWindowScript>().KonyuType = 2;
                KonyuWindow.GetComponent<KonyuWindowScript>().ItemList = AllDataManege.TownData.BukiyaItemInt;

                TownManegerScript.CanCommandFlag = false;
            }

            if (value==4)
            {
                AllAudioManege.PlaySE(0);

                TownMessageWindow = TownManegerScript.MessageWindow.GetComponent<TownMessageWindow>();

                TownMessageWindow.ThisTextMesh.text = "";
                TownMessageWindow.MessageText = new string[1];
                TownMessageWindow.MessageText[0] = "おう！見てけ！";

                TownMessageWindow.MesType = 6;
                TownManegerScript.MessageWindowReset();

                KonyuWindow = Instantiate(KonyuWindowPrefab, gameObject.transform.parent.transform);
                KonyuWindow.GetComponent<KonyuWindowScript>().KonyuType = 3;
                KonyuWindow.GetComponent<KonyuWindowScript>().ItemList = AllDataManege.TownData.BouguyaItemInt;

                TownManegerScript.CanCommandFlag = false;
            }

            if (value == 5)
            {
                AllAudioManege.PlaySE(0);

                TownMessageWindow = TownManegerScript.MessageWindow.GetComponent<TownMessageWindow>();

                TownMessageWindow.ThisTextMesh.text = "";
                TownMessageWindow.MessageText = new string[1];
                TownMessageWindow.MessageText[0] = "何を持ってきたんだ？";

                TownMessageWindow.MesType = 6;
                TownManegerScript.MessageWindowReset();

                KonyuWindow = Instantiate(KonyuWindowPrefab, gameObject.transform.parent.transform);
                KonyuWindow.GetComponent<KonyuWindowScript>().KonyuType = 4;
                KonyuWindow.GetComponent<KonyuWindowScript>().ItemList = new List<int>(AllDataManege.CharactorData.BagItemInt);

                TownManegerScript.CanCommandFlag = false;
            }

            if (value == 6)
            {
                AllAudioManege.PlaySE(7);

                MiniSceneChange = Instantiate(MiniChangePrefab, GameObject.Find("SceneChangeCanvas").transform);

                MiniSceneChange.GetComponent<MiniSceneChangeScript>().ChangeEx = 2;
                MiniSceneChange.GetComponent<MiniSceneChangeScript>().ChangeType = 0;

                MiniSceneChange.GetComponent<RectTransform>().localPosition = new Vector3(2160, 0, 0);

                SentakuInt = 5;

                TownMessageWindow.CanMesFlag = false;

                TownManegerScript.CanCommandFlag = false;
            }
        }

        //訓練場にいる場合
        if (CommandType==6)
        {
            if (value==2)
            {
                AllAudioManege.PlaySE(0);

                TownMessageWindow = TownManegerScript.MessageWindow.GetComponent<TownMessageWindow>();

                TownMessageWindow.ThisTextMesh.text = "";
                TownMessageWindow.MessageText = new string[1];
                TownMessageWindow.MessageText[0] = TownManegerScript.MurabitoText[UnityEngine.Random.Range(AllDataManege.SomeIntData.GetTownTextMin(AllDataManege.TownData.HattenLevel) + 15, AllDataManege.SomeIntData.GetTownTextMax(AllDataManege.TownData.HattenLevel) + 15)];

                TownMessageWindow.MesType = 7;
                TownManegerScript.MessageWindowReset();

            }

            if (value==3)
            {
                AllAudioManege.PlaySE(0);

                //レベル上限に達していたら
                if (AllDataManege.CharactorData.GetParameter()[0]>=AllDataManege.SyuzokuData.SaikouParameter[0])
                {
                    TownMessageWindow = TownManegerScript.MessageWindow.GetComponent<TownMessageWindow>();

                    TownMessageWindow.ThisTextMesh.text = "";
                    TownMessageWindow.MessageText = new string[1];
                    TownMessageWindow.MessageText[0] = "ん？　もうレベルはマックスみたいだな";

                    TownMessageWindow.MesType = 7;
                    TownManegerScript.MessageWindowReset();

                    return;
                }

                TownManegerScript.CanCommandFlag = false;

                ShiharaiText = "\n訓練しますか？\n\n<size=40>※" + AllDataManege.SomeIntData.GetKunrenGold(AllDataManege.CharactorData.GetParameter()[0]-1) + "ゴールドかかります\n※１レベル上がります</size>";

                ShiharaiWindow = Instantiate(ShiharaiPrefab, this.gameObject.transform.parent.transform);

                ShiharaiWindow.GetComponent<ShiharaiScript>().ShiharaiType = 3;
                ShiharaiWindow.transform.GetChild(2).GetComponent<Text>().text = ShiharaiText;
            }

            if (value == 4)
            {
                AllAudioManege.PlaySE(7);

                MiniSceneChange = Instantiate(MiniChangePrefab, GameObject.Find("SceneChangeCanvas").transform);

                MiniSceneChange.GetComponent<MiniSceneChangeScript>().ChangeEx = 2;
                MiniSceneChange.GetComponent<MiniSceneChangeScript>().ChangeType = 0;

                MiniSceneChange.GetComponent<RectTransform>().localPosition = new Vector3(2160, 0, 0);

                SentakuInt = 5;

                TownMessageWindow.CanMesFlag = false;

                TownManegerScript.CanCommandFlag = false;
            }
        }
    }
}
