using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class TownManegerScript : MonoBehaviour
{
    //村人の会話など
    public TextAsset MurabitoKaiwa,TownKaiwa;

    public static TownMessageWindow TownMessageWindow;

    //キャンバス系
    private GameObject UICanvas,SceneChangeCanvas;

    //普通のオブジェクト系
    private GameObject HattenChange;

    //プレハブ系
    public GameObject SceneChange,MessageWindowPrefab,HattenChangePrefab;

    //他のスクリプトにも渡すオブジェクト
    public static GameObject BanaTyuui, CommandButton,KakuninWindow, MessageWindow;

    //街の背景
    public Sprite[] HaikeiSprites;

    private Image HaikeiImage;

    public static int KaiwaType;
    public static bool CanCommandFlag, IkkaikagiriFlag, HattenFlag;

    private bool MakeMesWindow;
    
    private static int[] CommandKaihouInt = new int[8];

    public static string[] MurabitoText,TownText;


    void Awake()
    {
        IkkaikagiriFlag = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        UICanvas = GameObject.Find("UICanvas");
        SceneChangeCanvas = GameObject.Find("SceneChangeCanvas");

        SceneChange = SceneChangeCanvas.transform.GetChild(0).gameObject;

        HaikeiImage = GameObject.Find("TownHaikei").GetComponent<Image>();

        MurabitoText = MurabitoKaiwa.text.Split('\n');
        TownText = TownKaiwa.text.Split('\n');

        //背景の更新
        HaikeiImage.sprite = HaikeiSprites[AllDataManege.TownData.GetHattenLevel()];

    }

    // Update is called once per frame
    void Update()
    {

        //一番最初にメッセージウィンドウを作る
        if (!SceneChange && !MakeMesWindow)
        {

            MessageWindow = Instantiate(MessageWindowPrefab, UICanvas.transform);

            TownMessageWindow = MessageWindow.GetComponent<TownMessageWindow>();

            AllAudioManege.PlayBGM(1);

            MakeMesWindow = true;
        }

        if (CommandButton&&!CommandButton.GetComponent<Animation>().isPlaying&&!IkkaikagiriFlag)
        {
            IkkaikagiriFlag = true;
            CanCommandFlag = true;
        }

        //発展したときに暗転する
        if (HattenFlag && !HattenChange)
        {
            HattenChange = Instantiate(HattenChangePrefab,SceneChangeCanvas.transform);
        }

        if (HattenChange)
        {
            //完全に暗転したとき
            if (HattenChange.GetComponent<Image>().color.a == 1f)
            {
                HaikeiImage.sprite = HaikeiSprites[AllDataManege.TownData.GetHattenLevel()];

                HattenFlag = false;
                CanCommandFlag = true;
            }
        }
    }

    //メッセージウィンドウの内容リセット
    public static void MessageWindowReset()
    {
        TownMessageWindow.NowMojiNum = 0;
        TownMessageWindow.NowMojiGyou = 0;
        TownMessageWindow.CanMesFlag = true;
        TownMessageWindow.MesEndFlag = false;
    }

    //投資の結果判定
    public static void ToushiHantei()
    {
        for (int i=AllDataManege.TownData.GetHattenLevel();i<12;i++)
        {
            if (AllDataManege.TownData.GetToushiMoney()>=AllDataManege.SomeIntData.GetHattenHituyou()[i])
            {
                AllDataManege.TownData.SetHattenLevel(AllDataManege.TownData.GetHattenLevel()+1);

                HattenSyori();

                HattenFlag = true;
                CanCommandFlag = false;
            }
            else
            {
                break;
            }
        }
    }

    //発展したときの処理
    public static void HattenSyori()
    {
        int[] SetCommandInt = new int[8];

        SetCommandInt = AllDataManege.TownData.GetCommandKaihou();

        //発展レベルに応じて出来るコマンドとかを増やしていく
        switch (AllDataManege.TownData.GetHattenLevel())
        {
            //最初に発展したとき : 路地裏と鍛冶屋を開放
            case 1:

                SetCommandInt[2] = 1;
                SetCommandInt[5] = 1;

                break;

            //二番目に発展したとき : 泉の水、木の剣、Ｔシャツを開放
            case 2:

                AllDataManege.TownData.DouguyaItemInt.Add(76);
                AllDataManege.TownData.BukiyaItemInt.Add(2);
                AllDataManege.TownData.BouguyaItemInt.Add(42);

                break;

            //三番目に発展したとき : 薬草の束、聖水、木の装備シリーズ(ランダムで三つ)、若木の鎧、木の鎧開放
            case 3:

                AllDataManege.TownData.DouguyaItemInt.Add(72);
                AllDataManege.TownData.DouguyaItemInt.Add(77);

                for (int i=0;i<3;i++)
                {
                    int RandomBuki = Random.Range(3, 8);

                    while (AllDataManege.TownData.BukiyaItemInt.Any(value=>value==RandomBuki))
                    {
                        RandomBuki = Random.Range(3,8);
                    }

                    AllDataManege.TownData.BukiyaItemInt.Add(RandomBuki);
                }

                AllDataManege.TownData.BouguyaItemInt.Add(43);
                AllDataManege.TownData.BouguyaItemInt.Add(44);

                break;

            //四番目に発展したとき : 上薬草、聖酒、鉄の棒、鉄の剣、鎖帷子開放
            case 4:

                AllDataManege.TownData.DouguyaItemInt.Add(73);
                AllDataManege.TownData.DouguyaItemInt.Add(78);

                AllDataManege.TownData.BukiyaItemInt.Add(8);
                AllDataManege.TownData.BukiyaItemInt.Add(9);

                AllDataManege.TownData.BouguyaItemInt.Add(45);

                break;

            //五番目に発展したとき : 鉄の装備シリーズ(ランダムで三つ)、鉄の鎧、錠剤、訓練場を開放
            case 5:

                SetCommandInt[6] = 1;

                AllDataManege.TownData.DouguyaItemInt.Add(91);

                for (int i=0;i<3;i++)
                {
                    int RandomBuki = Random.Range(10,15);

                    while (AllDataManege.TownData.BukiyaItemInt.Any(value => value == RandomBuki))
                    {
                        RandomBuki = Random.Range(10,15);
                    }
                }

                AllDataManege.TownData.BouguyaItemInt.Add(46);

                break;

            //六番目に発展したとき : 世界樹の露、魔力石、金の棒、金の剣、鋼鉄の鎧、ローブを開放
            case 6:

                AllDataManege.TownData.DouguyaItemInt.Add(81);
                AllDataManege.TownData.DouguyaItemInt.Add(86);

                AllDataManege.TownData.BukiyaItemInt.Add(15);
                AllDataManege.TownData.BukiyaItemInt.Add(16);

                AllDataManege.TownData.BouguyaItemInt.Add(47);
                AllDataManege.TownData.BouguyaItemInt.Add(49);

                break;

            //七番目に発展したとき : 特上薬草、魔力飴、カプセル、狩人の服、金の装備シリーズ(ランダムで三つ)を開放
            case 7:

                AllDataManege.TownData.DouguyaItemInt.Add(74);
                AllDataManege.TownData.DouguyaItemInt.Add(79);
                AllDataManege.TownData.DouguyaItemInt.Add(92);

                for (int i=0;i<3;i++)
                {
                    int RandomBuki = Random.Range(17,22);

                    while (AllDataManege.TownData.BukiyaItemInt.Any(value => value == RandomBuki))
                    {
                        RandomBuki = Random.Range(17,22);
                    }

                    AllDataManege.TownData.BukiyaItemInt.Add(RandomBuki);
                }

                AllDataManege.TownData.BouguyaItemInt.Add(51);

                break;

            //八番目に発展したとき : 世界樹の樹液、魔力結晶、マグマの棒を開放
            case 8:

                AllDataManege.TownData.DouguyaItemInt.Add(82);
                AllDataManege.TownData.DouguyaItemInt.Add(87);

                AllDataManege.TownData.BukiyaItemInt.Add(22);

                break;

            //九番目に発展したとき : 魔女のローブ、鉄の重鎧、漢方薬を開放
            case 9:

                AllDataManege.TownData.DouguyaItemInt.Add(93);

                AllDataManege.TownData.BouguyaItemInt.Add(48);
                AllDataManege.TownData.BouguyaItemInt.Add(50);

                break;

            //十番目の発展したとき : 天使の服、悪魔の服、マグマの剣を開放
            case 10:

                AllDataManege.TownData.BukiyaItemInt.Add(23);

                AllDataManege.TownData.BouguyaItemInt.Add(53);
                AllDataManege.TownData.BouguyaItemInt.Add(55);

                break;

            //十一番目の発展したとき : 世界樹の葉、世界樹の雫、マンドラゴラを開放
            case 11:

                AllDataManege.TownData.DouguyaItemInt.Add(75);
                AllDataManege.TownData.DouguyaItemInt.Add(80);
                AllDataManege.TownData.DouguyaItemInt.Add(94);

                break;

            //十二番目の発展したとき : 天使の鎧、悪魔の鎧、マグマの装備シリーズ(ランダムで三つ)を開放
            case 12:

                for (int i=0;i<3;i++)
                {
                    int RandomBuki = Random.Range(24,29);

                    while (AllDataManege.TownData.BukiyaItemInt.Any(value => value == RandomBuki))
                    {
                        RandomBuki = Random.Range(24,29);
                    }

                    AllDataManege.TownData.BukiyaItemInt.Add(RandomBuki);
                }

                AllDataManege.TownData.BouguyaItemInt.Add(54);
                AllDataManege.TownData.BouguyaItemInt.Add(56);

                break;

        }

        AllDataManege.TownData.DouguyaItemInt.Sort();
        AllDataManege.TownData.BukiyaItemInt.Sort();
        AllDataManege.TownData.BouguyaItemInt.Sort();

        AllDataManege.TownData.SetCommandKaihou(SetCommandInt);

        AllDataManege.DataSave();
    }
}
