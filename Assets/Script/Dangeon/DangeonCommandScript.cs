using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class DangeonCommandScript : MonoBehaviour
{
    [System.NonSerialized]
    public DangeonManeger DangeonManeger;

    //プレハブ
    public GameObject KonyuWindowPrefab;
    private GameObject KonyuWindow,UICanvas;

    [System.NonSerialized]
    public int CommandType;

    private RectTransform ThisRect;

    public float Speed;

    private string[] GyousyoText;

    [System.NonSerialized]
    public bool InFlag, OutFlag, CanCommandFlag;

    private bool SinkouFlag, NotSentaku;

    // Start is called before the first frame update
    void Start()
    {
        InFlag = true;
        CanCommandFlag = true;

        ThisRect = gameObject.GetComponent<RectTransform>();

        UICanvas = GameObject.Find("UICanvas");

        //イベントによってコマンドを変えたりする
        switch (CommandType)
        {
            //宝箱の場合
            case 1:

                Destroy(gameObject.transform.GetChild(0).gameObject);
                Destroy(gameObject.transform.GetChild(1).gameObject);
                Destroy(gameObject.transform.GetChild(2).gameObject);

                gameObject.transform.GetChild(3).transform.GetChild(0).GetComponent<Text>().text = "開ける";
                gameObject.transform.GetChild(4).transform.GetChild(0).GetComponent<Text>().text = "無視する";

                break;

            //ボス部屋前の場合
            case 2:

                Destroy(gameObject.transform.GetChild(0).gameObject);
                Destroy(gameObject.transform.GetChild(1).gameObject);
                Destroy(gameObject.transform.GetChild(2).gameObject);

                gameObject.transform.GetChild(3).transform.GetChild(0).GetComponent<Text>().text = "進む";
                gameObject.transform.GetChild(4).transform.GetChild(0).GetComponent<Text>().text = "一旦退く";

                break;

            //行商人の場合
            case 3:

                Destroy(gameObject.transform.GetChild(0).gameObject);
                Destroy(gameObject.transform.GetChild(1).gameObject);

                gameObject.transform.GetChild(2).transform.GetChild(0).GetComponent<Text>().text = "無駄話";
                gameObject.transform.GetChild(3).transform.GetChild(0).GetComponent<Text>().text = "買う";
                gameObject.transform.GetChild(4).transform.GetChild(0).GetComponent<Text>().text = "買わない";

                break;

            //一休み 岩の場合
            case 4:

                Destroy(gameObject.transform.GetChild(0).gameObject);
                Destroy(gameObject.transform.GetChild(1).gameObject);
                Destroy(gameObject.transform.GetChild(2).gameObject);

                gameObject.transform.GetChild(3).transform.GetChild(0).GetComponent<Text>().text = "休む";
                gameObject.transform.GetChild(4).transform.GetChild(0).GetComponent<Text>().text = "無視する";

                break;

            //DEX試しの場合
            case 5:

                Destroy(gameObject.transform.GetChild(0).gameObject);
                Destroy(gameObject.transform.GetChild(1).gameObject);
                Destroy(gameObject.transform.GetChild(2).gameObject);

                gameObject.transform.GetChild(3).transform.GetChild(0).GetComponent<Text>().text = "試す";
                gameObject.transform.GetChild(4).transform.GetChild(0).GetComponent<Text>().text = "無視する";

                break;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (InFlag)
        {
            ThisRect.localPosition = new Vector3(ThisRect.localPosition.x - (Speed * Time.deltaTime), 0, 0);
            ThisRect.localPosition = new Vector3(Mathf.Clamp(ThisRect.localPosition.x, 0, 350), 0, 0);

            if (ThisRect.localPosition.x <= 0)
            {
                InFlag = false;
            }
        }

        if (OutFlag)
        {
            ThisRect.localPosition = new Vector3(ThisRect.localPosition.x + (Speed * Time.deltaTime), 0, 0);
            ThisRect.localPosition = new Vector3(Mathf.Clamp(ThisRect.localPosition.x, 0, 350), 0, 0);

            if (ThisRect.localPosition.x >= 350)
            {
                //戦闘に入らない
                if (CommandType == 2 && NotSentaku)
                {
                    GameObject.Find("UICanvas").transform.GetChild(2).transform.GetChild(2).transform.GetChild(0).GetComponent<Text>().text = "進む";
                    GameObject.Find("UICanvas").transform.GetChild(2).transform.GetChild(2).GetComponent<Image>().sprite = DangeonManeger.UISprites[0];

                    DangeonManeger.CanSinkouFlag = true;
                }
                //戦闘に入る
                else if (CommandType == 2 && !NotSentaku)
                {
                    AllDataManege.TokusyuJoukyou = 2;

                    DangeonManeger.CanCommandFlag = false;
                    DangeonManeger.CanSinkouFlag = true;

                    DangeonManeger.IventChangeSinkou = 1;

                    //カメラの震え
                    DangeonManeger.StartCoroutine("CameraHurue");

                    //敵のデータを作る
                    DangeonManeger.EnemyData = DangeonManeger.MakeEnemyData(DangeonManeger.KaisouData.BossData);

                    AllDataManege.EnemyData = DangeonManeger.EnemyData;

                    //まだ遭遇したことがなかったら遭遇フラッグを立てる
                    if (!AllDataManege.SomethingData.EnemySougu[DangeonManeger.EnemyData.SyuzokuNumber])
                    {
                        AllDataManege.SomethingData.EnemySougu[DangeonManeger.EnemyData.SyuzokuNumber] = true;
                    }

                    //バフだとかが入っていない元のパラメータを取得してセーブ
                    DangeonManeger.MotoParameter = AllDataManege.CharactorData.KihonParameter.ToArray();
                    DangeonManeger.EnemyMotoParameter = AllDataManege.EnemyData.EnemyParameter.ToArray();

                    for (int i = 0; i < 14; i++)
                    {
                        PlayerPrefs.SetInt("MotoParameter" + i, DangeonManeger.MotoParameter[i]);
                        PlayerPrefs.SetInt("EnemyMotoParameter" + i, DangeonManeger.EnemyMotoParameter[i]);
                    }

                    DangeonManeger.BattleParameterReset();

                    AllAudioManege.PlaySE(27);

                }

                AllDataManege.DataSave();

                Destroy(gameObject);
            }
        }
    }

    //ボタンとか
    public void ButtonDown(GameObject Button)
    {
        if (InFlag || OutFlag || !CanCommandFlag)
        {
            return;
        }

        Image ButtonImage = Button.GetComponent<Image>();

        ButtonImage.color = new Color(0.8f, 0.8f, 0.8f);
    }

    public void ButtonUp(GameObject Button)
    {
        if (InFlag || OutFlag || !CanCommandFlag)
        {
            return;
        }

        Image ButtonImage = Button.GetComponent<Image>();

        ButtonImage.color = new Color(1f, 1f, 1f);
    }

    //コマンドボタンが押されたら
    public void CommandDown(int value)
    {
        if (InFlag || OutFlag || !CanCommandFlag)
        {
            return;
        }

        //どのコマンドが押されたかで処理を変える
        switch (value)
        {
            //コマンド１が押されたら
            case 1:

                {

                }

                break;

            //コマンド２が押されたら
            case 2:

                {

                }

                break;

            //コマンド３が押されたら
            case 3:

                {
                    //現在発生しているイベントによってコマンドが押されたときの反応を変える
                    switch (CommandType)
                    {
                        //行商人の場合
                        case 3:

                            int RandomInt = UnityEngine.Random.Range(DangeonManeger.KaisouData.GyousyouData.KaiwaGyousu[0], DangeonManeger.KaisouData.GyousyouData.KaiwaGyousu[1]);

                            GyousyoText = DangeonManeger.KaisouData.GyousyouData.SyouninKaiwa.text.Split('\n');

                            DangeonManeger.TokusyuText(GyousyoText[RandomInt], 0);

                            break;
                    }
                }

                break;

            //コマンド４が押されたら
            case 4:

                {
                    //現在発生しているイベントによってコマンドが押されたときの反応を変える
                    switch (CommandType)
                    {

                        //宝箱の場合
                        case 1:

                            DangeonManeger.OpenTakarabako();

                            OutFlag = true;

                            break;

                        //ボス部屋前の場合
                        case 2:

                            OutFlag = true;

                            break;

                        //行商人の場合
                        case 3:

                            DangeonManeger.TokusyuText("ああ、見ていってよ", 0);

                            KonyuWindow = Instantiate(KonyuWindowPrefab, gameObject.transform.parent.transform);
                            KonyuWindow.GetComponent<KonyuWindowScript>().KonyuType = 5;
                            KonyuWindow.GetComponent<KonyuWindowScript>().ItemList = DangeonManeger.KaisouData.GyousyouData.Syouhin;

                            KonyuWindow.GetComponentInChildren<KonyuWindowScript>().DangeonCommand = this;

                            gameObject.transform.GetChild(1).GetComponent<Image>().color = new Color(1, 1, 1);

                            CanCommandFlag = false;

                            break;

                        //一休み 岩の場合
                        case 4:

                            int KaihukuSuuti = UnityEngine.Random.Range(DangeonManeger.KaisouData.HitoyasumiKaihuku[0], DangeonManeger.KaisouData.HitoyasumiKaihuku[1]);

                            AllDataManege.StetasKaihuku(KaihukuSuuti, 3);

                            DangeonManeger.TokusyuText("HPとMPが" + KaihukuSuuti + "回復した！", 0);

                            OutFlag = true;

                            DangeonManeger.CanSinkouFlag = true;

                            UICanvas.transform.GetChild(2).transform.GetChild(2).transform.GetChild(0).GetComponent<Text>().text = "進む";
                            UICanvas.transform.GetChild(2).transform.GetChild(2).GetComponent<Image>().sprite = DangeonManeger.UISprites[0];

                            break;

                        //AGI試しの場合
                        case 5:

                            int HanteiSuuti = (AllDataManege.CharactorData.KihonParameter[12] + AllDataManege.CharactorData.KihonParameter[9]) / AllDataManege.CharactorData.KihonParameter[0] * 10;
                            int GetGold = UnityEngine.Random.Range(DangeonManeger.KaisouData.TakarabakoGold[0], DangeonManeger.KaisouData.TakarabakoGold[1]) * UnityEngine.Random.Range(1, 4);

                            AllAudioManege.PlaySE(16);

                            //判定
                            //成功したら
                            if (UnityEngine.Random.Range(0, 101) <= HanteiSuuti)
                            {
                                DangeonManeger.TokusyuText("やった！当てれたぞ！\nどこからともなく" + GetGold + "Gが落ちてきた……", 0);

                                //ゴールドを取得
                                AllDataManege.CharactorData.NowSyojikin += GetGold;
                                AllDataManege.CharactorData.NowSyojikin = Mathf.Clamp(AllDataManege.CharactorData.NowSyojikin, 0, 99999);

                            }
                            //失敗したら
                            else
                            {
                                DangeonManeger.TokusyuText("当てられなかった……", 0);
                            }

                            OutFlag = true;
                            DangeonManeger.CanSinkouFlag = true;

                            UICanvas.transform.GetChild(2).transform.GetChild(2).transform.GetChild(0).GetComponent<Text>().text = "進む";
                            UICanvas.transform.GetChild(2).transform.GetChild(2).GetComponent<Image>().sprite = DangeonManeger.UISprites[0];

                            break;
                    }
                }

                break;

            //コマンド５が押されたら
            case 5:

                {
                    //現在発生しているイベントによってコマンドが押されたときの反応を変える
                    switch (CommandType)
                    {
                        //宝箱と行商人と一休み 岩の場合
                        case 1:
                        case 3:
                        case 4:

                            OutFlag = true;

                            DangeonManeger.SinkouHantei();

                            break;

                        //ボス部屋前の場合
                        case 2:

                            OutFlag = true;

                            NotSentaku = true;

                            DangeonManeger.BossBattle = 0;
                            DangeonManeger.HasseiIvent = 0;

                            AllDataManege.DangeonSinkou = 0;
                            AllDataManege.TokusyuJoukyou = 1;

                            break;
                    }
                }

                break;
        }
    }
}
