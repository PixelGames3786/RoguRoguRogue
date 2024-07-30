using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class DangeonManeger : MonoBehaviour
{
    public static BaseDangeonData DangeonData;
    public static BaseDangeonKaisouData KaisouData;
    public static BaseItemData TakarabakoData;

    //プレハブ　ウィンドウ系
    public GameObject BagWindowPrefab, StetasWindowPrefab, ItemIrekaePrefab, LevelUpPrefab, SkillWindowPrefab, BaffWindowPrefab;

    //プレハブ　それ以外
    public GameObject SceneChangePrefab, ShiharaiPrefab, CommandPrefab, BattleChangePrefab, IventSpritePrefab, IventTatiePrefab, TyuuiPrefab;

    [System.NonSerialized]
    public GameObject ShiharaiWindow, BagWindow, LevelUpWindow, StetasWindow, SkillWindow, ItemIrekaeWindow, BaffWindow, TyuuiWindow;

    [System.NonSerialized]
    public GameObject SceneChange, EnemySprite, CommandParent, IventSprite;

    //パーティクル
    public GameObject[] Particles;

    //キャンバスとカメラ
    private GameObject UICanvas, IventCanvas, Camera;

    //画像だとか
    public Sprite[] UISprites;
    public Sprite[] IventSprites;

    [System.NonSerialized]
    public Image DangeonHaikei, BattleChangeImage;

    //HPMPバー関連
    private RectTransform HPBerRect, MPBerRect;
    private Text HPBerText, MPBerText;

    private float HPBerFloat, MPBerFloat;

    //ダンジョン攻略に使う
    public static int RandomInt;

    private bool KaisouChangeIn, KaisouChangeOut;

    //モンスターだとか
    public EnemySyuzokuDataBase EnemySyuzokuDataBase;

    [System.NonSerialized]
    public BaseEnemyData EnemyData;

    private List<BaseEnemySyuzokuData> EnemySyuzokuList = new List<BaseEnemySyuzokuData>();

    //戦闘中パラメーター
    public static int TousouKaisuu;

    public static List<int> BattleTokusyu=new List<int>();
    //1 逃走確実

    //メッセージ進行
    private Text MessageText;

    //テキストアセットたち
    //戦闘時のテキストアセットとどのダンジョンでも共通のテキストアセット
    public TextAsset BattleTextAsset, KyoutuTextAsset;

    [System.NonSerialized]
    public static bool CanMesFlag, MesEndFlag;

    private string[] DangeonText,BattleText,KyoutuText;

    private string HyoujiText;

    private int NowMesInt;

    private float KeikaTime;

    //イベント処理とか
    public static int HasseiIvent, IventChangeSinkou;
    //0 なにもなし 1 バトル 2 宝箱 3 旅商人 4 一休み 岩 5 AGI試し 6 一休み　切り株

    //バトルとか
    [System.NonSerialized]
    public bool BattleFlag, BattleEndFlag, HaibokuFlag;

    public static int UseSkillFlag; //0 スキルは使われていない 1 自分のスキルが使われた 2 敵のスキルが使われた
    public static int BossGekiha, BossBattle,SenseiInt, KoudouSpeed, EnemyKoudouSpeed;

    public static string SyouriText;

    [System.NonSerialized]
    public int[] MotoParameter = new int[14];
    [System.NonSerialized]
    public int[] EnemyMotoParameter = new int[14];

    public static BaseItemData DropItem;

    //行商人とか
    [System.NonSerialized]
    public BaseGyousyouData GyousyouData;

    //ボタンを押せるかどうか
    public static bool CanCommandFlag, CanSinkouFlag;

    // Start is called before the first frame update
    void Start()
    {
        AllDataManege.DangeonManeger = this;

        EnemySyuzokuList = EnemySyuzokuDataBase.GetSyuzokuDataList();

        //ダンジョンデータと現在の階層データの取得 //後で変更する
        DangeonData = AllDataManege.DangeonData; //AllDataManege.DangeonDataList[0];
        KaisouData = DangeonData.KaisouList[AllDataManege.DangeonKaisou];

        //階層データから背景を設定
        DangeonHaikei = GameObject.Find("HaikeiCanvas").transform.GetChild(0).GetComponent<Image>();

        UICanvas = GameObject.Find("UICanvas");
        IventCanvas = GameObject.Find("IventCanvas");
        Camera = GameObject.Find("Main Camera");

        //メッセージ進行に使う物の取得
        DangeonText = DangeonData.DangeonText.text.Split('\n');
        BattleText = BattleTextAsset.text.Split('\n');
        KyoutuText = KyoutuTextAsset.text.Split('\n');

        HyoujiText = DangeonText[Random.Range(KaisouData.TextGyousuuMin, KaisouData.TextGyousuuMax)];

        MessageText = UICanvas.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>();

        //ＨＰバーとＭＰバーを取得
        HPBerRect = UICanvas.transform.GetChild(1).transform.GetChild(0).transform.GetChild(1).GetComponent<RectTransform>();
        MPBerRect = UICanvas.transform.GetChild(1).transform.GetChild(1).transform.GetChild(1).GetComponent<RectTransform>();

        //ＨＰとＭＰのテキストを取得
        HPBerText = UICanvas.transform.GetChild(1).transform.GetChild(0).transform.GetChild(3).GetComponent<Text>();
        MPBerText = UICanvas.transform.GetChild(1).transform.GetChild(1).transform.GetChild(3).GetComponent<Text>();

        DangeonHaikei.sprite = DangeonData.KaisouList[AllDataManege.DangeonKaisou].DangeonSprite;

        CanSinkouFlag = true;

        //ここでＢＧＭを止める
        AllAudioManege.StopBGM();
        AllAudioManege.PlayBGM(KaisouData.DangeonBGM);

        //ロードしたときに特殊状況によってUIを変えたりする
        switch (AllDataManege.TokusyuJoukyou)
        {
            //戦闘中の場合
            case 2:
                {
                    HasseiIvent = 1;

                    EnemyData = AllDataManege.EnemyData;

                    //もともとのデータを保存
                    for (int i = 0; i < 14; i++)
                    {
                        MotoParameter[i] = PlayerPrefs.GetInt("MotoParameter" + i);
                        EnemyMotoParameter[i] = PlayerPrefs.GetInt("EnemyMotoParameter" + i);
                    }

                    BattleFlag = true;
                    CanCommandFlag = true;

                    //敵の画像を作る
                    EnemySprite = Instantiate(IventSpritePrefab, IventCanvas.transform);
                    EnemySprite.GetComponent<Image>().sprite = EnemyData.EnemySprite;

                    UICanvas.transform.GetChild(2).transform.GetChild(0).GetComponent<Image>().color = new Color(1, 0, 1);
                    UICanvas.transform.GetChild(2).transform.GetChild(1).GetComponent<Image>().color = new Color(1, 0, 1);
                    UICanvas.transform.GetChild(2).transform.GetChild(2).GetComponent<Image>().color = new Color(1, 0, 1);
                    UICanvas.transform.GetChild(2).transform.GetChild(3).GetComponent<Image>().color = new Color(1, 0, 1);
                    UICanvas.transform.GetChild(0).GetComponent<Image>().color = new Color(1, 0, 1);
                    UICanvas.transform.GetChild(1).GetComponent<Image>().color = new Color(1, 0, 1);

                    UICanvas.transform.GetChild(2).transform.GetChild(2).transform.GetChild(0).GetComponent<Text>().text = "通常攻撃";
                    UICanvas.transform.GetChild(2).transform.GetChild(3).transform.GetChild(0).GetComponent<Text>().text = "逃げる";

                    UICanvas.transform.GetChild(2).transform.GetChild(4).gameObject.SetActive(true);
                    UICanvas.transform.GetChild(2).transform.GetChild(5).gameObject.SetActive(true);

                    HyoujiText = BattleText[Random.Range(0, BattleText.Length)];
                    HyoujiText = HyoujiText.Replace("[魔物名]", EnemyData.EnemyName);

                    BattleParameterReset();

                    //ここでＢＧＭを戦闘のＢＧＭにかえる
                    AllAudioManege.StopBGM();
                    AllAudioManege.PlayBGM(DangeonData.BattleBGM);
                }
                break;

            //宝箱イベントの場合
            case 3:
                {
                    HasseiIvent = 2;

                    TakarabakoData = AllDataManege.ItemDataList[AllDataManege.TakaraBakoItem];

                    UICanvas.transform.GetChild(2).transform.GetChild(2).transform.GetChild(0).GetComponent<Text>().text = "";
                    UICanvas.transform.GetChild(2).transform.GetChild(2).GetComponent<Image>().sprite = UISprites[1];

                    //宝箱の画像を作成
                    IventSprite = Instantiate(IventSpritePrefab, IventCanvas.transform);

                    CommandParent = Instantiate(CommandPrefab, UICanvas.transform);
                    CommandParent.GetComponent<DangeonCommandScript>().CommandType = 1;
                    CommandParent.GetComponent<DangeonCommandScript>().DangeonManeger = this;

                    CanSinkouFlag = false;
                }
                break;

            //行商人イベントの場合
            case 4:
                {
                    HasseiIvent = 3;

                    UICanvas.transform.GetChild(2).transform.GetChild(2).transform.GetChild(0).GetComponent<Text>().text = "";
                    UICanvas.transform.GetChild(2).transform.GetChild(2).GetComponent<Image>().sprite = UISprites[1];

                    //立ち絵画像を作成
                    IventSprite = Instantiate(IventTatiePrefab, IventCanvas.transform);

                    CommandParent = Instantiate(CommandPrefab, UICanvas.transform);
                    CommandParent.GetComponent<DangeonCommandScript>().CommandType = 3;
                    CommandParent.GetComponent<DangeonCommandScript>().DangeonManeger = this;

                    CanSinkouFlag = false;
                }
                break;

            //ボス部屋前イベントの場合
            case 5:
                {
                    CanCommandFlag = true;
                    CanSinkouFlag = false;
                    BossBattle = 1;

                    UICanvas.transform.GetChild(2).transform.GetChild(2).transform.GetChild(0).GetComponent<Text>().text = "";
                    UICanvas.transform.GetChild(2).transform.GetChild(2).GetComponent<Image>().sprite = UISprites[1];

                    //コマンドボタンを作る
                    CommandParent = Instantiate(CommandPrefab, UICanvas.transform);
                    CommandParent.GetComponent<DangeonCommandScript>().CommandType = 2;
                    CommandParent.GetComponent<DangeonCommandScript>().DangeonManeger = this;

                    TokusyuText("奥からプレッシャーを感じる……\nどうしようか？",0);
                }
                break;

            //一休み 岩イベントの場合
            case 6:
                {
                    HasseiIvent = 4;

                    UICanvas.transform.GetChild(2).transform.GetChild(2).transform.GetChild(0).GetComponent<Text>().text = "";
                    UICanvas.transform.GetChild(2).transform.GetChild(2).GetComponent<Image>().sprite = UISprites[1];

                    //イベント画像を作成
                    IventSprite = Instantiate(IventSpritePrefab, IventCanvas.transform);
                    IventSprite.GetComponent<Image>().sprite = IventSprites[1];

                    CommandParent = Instantiate(CommandPrefab, UICanvas.transform);
                    CommandParent.GetComponent<DangeonCommandScript>().CommandType = 4;
                    CommandParent.GetComponent<DangeonCommandScript>().DangeonManeger = this;

                    CanSinkouFlag = false;
                }
                break;

            //DEX試しイベントの場合
            case 7:
                {
                    HasseiIvent = 5;

                    UICanvas.transform.GetChild(2).transform.GetChild(2).transform.GetChild(0).GetComponent<Text>().text = "";
                    UICanvas.transform.GetChild(2).transform.GetChild(2).GetComponent<Image>().sprite = UISprites[1];

                    //イベント画像を作成
                    IventSprite = Instantiate(IventSpritePrefab, IventCanvas.transform);
                    IventSprite.GetComponent<Image>().sprite = IventSprites[2];

                    CommandParent = Instantiate(CommandPrefab, UICanvas.transform);
                    CommandParent.GetComponent<DangeonCommandScript>().CommandType = 5;
                    CommandParent.GetComponent<DangeonCommandScript>().DangeonManeger = this;

                    CanSinkouFlag = false;

                }
                break;

            //一休み 切り株イベントの場合
            case 8:
                {
                    HasseiIvent = 6;

                    UICanvas.transform.GetChild(2).transform.GetChild(2).transform.GetChild(0).GetComponent<Text>().text = "";
                    UICanvas.transform.GetChild(2).transform.GetChild(2).GetComponent<Image>().sprite = UISprites[1];

                    //イベント画像を作成
                    IventSprite = Instantiate(IventSpritePrefab, IventCanvas.transform);
                    IventSprite.GetComponent<Image>().sprite = IventSprites[3];

                    CommandParent = Instantiate(CommandPrefab, UICanvas.transform);
                    CommandParent.GetComponent<DangeonCommandScript>().CommandType = 4;
                    CommandParent.GetComponent<DangeonCommandScript>().DangeonManeger = this;

                    CanSinkouFlag = false;
                }
                break;
        }

        //バーとテキストをリセットする
        HPMPBerReset();

        //AllDataManege.CharactorData.BagItemInt[0] = 142;
    }

    // Update is called once per frame
    void Update()
    {
        //ダンジョンから出る時
        if (SceneChange && SceneChange.transform.GetChild(0).GetComponent<Rigidbody2D>().IsSleeping())
        {
            AllDataManege.TokusyuJoukyou = 0;
            HasseiIvent = 0;

            AllDataManege.DataSave();

            SceneManager.LoadScene("Town");
        }

        //メッセージ進行
        if (CanMesFlag && !MesEndFlag)
        {
            if (KeikaTime >= AllDataManege.MesTime)
            {
                KeikaTime = 0;

                MessageText.text += HyoujiText[NowMesInt];

                NowMesInt++;

                //もし表示し終えたら
                if (NowMesInt >= HyoujiText.Length)
                {
                    MesEndFlag = true;

                    NowMesInt = 0;

                    HyoujiText = "";

                    return;
                }
            }

            KeikaTime += Time.deltaTime;
        }

        //ダンジョン階層チェンジ暗くなるまで
        if (KaisouChangeIn)
        {
            DangeonHaikei.color = new Color(DangeonHaikei.color.r - (5 * Time.deltaTime), DangeonHaikei.color.g - (5 * Time.deltaTime), DangeonHaikei.color.b - (5 * Time.deltaTime));

            //完全に暗くなったら
            if (DangeonHaikei.color.r <= 0f)
            {
                KaisouChangeIn = false;
                KaisouChangeOut = true;

                CanMesFlag = false;
                MesEndFlag = true;

                DangeonHaikei.sprite = KaisouData.DangeonSprite;
            }
        }

        //ダンジョン階層チェンジ明るくなるまで
        if (KaisouChangeOut)
        {
            DangeonHaikei.color = new Color(DangeonHaikei.color.r + (5 * Time.deltaTime), DangeonHaikei.color.g + (5 * Time.deltaTime), DangeonHaikei.color.b + (5 * Time.deltaTime));

            //完全に明るくなったら
            if (DangeonHaikei.color.r >= 1f)
            {
                KaisouChangeOut = false;

                CanCommandFlag = true;

                CanMesFlag = true;
                MesEndFlag = false;

                NowMesInt = 0;

                MessageText.text = "";

                HyoujiText = DangeonText[Random.Range(KaisouData.TextGyousuuMin, KaisouData.TextGyousuuMax)];

                //ここでＢＧＭを次の階のＢＧＭにかえる
                //もし同じBGMじゃなかったら変える
                if (!(KaisouData.DangeonBGM==DangeonData.KaisouList[AllDataManege.DangeonKaisou-1].DangeonBGM))
                {
                    AllAudioManege.PlayBGM(KaisouData.DangeonBGM);
                }
            }
        }

        //バトルに入るとき　真っ暗になったら
        if (IventChangeSinkou == 2 && BattleChangeImage.color.a == 1f)
        {
            BattleFlag = true;
            CanCommandFlag = true;

            IventChangeSinkou = 5;

            //敵の画像を作る
            EnemySprite = Instantiate(IventSpritePrefab, IventCanvas.transform);
            EnemySprite.GetComponent<Image>().sprite = EnemyData.EnemySprite;

            //ウィンドウたちの色を変える
            UICanvas.transform.GetChild(2).transform.GetChild(2).GetComponent<Image>().sprite = UISprites[0];

            UICanvas.transform.GetChild(2).transform.GetChild(0).GetComponent<Image>().color = new Color(1, 0, 1);
            UICanvas.transform.GetChild(2).transform.GetChild(1).GetComponent<Image>().color = new Color(1, 0, 1);
            UICanvas.transform.GetChild(2).transform.GetChild(2).GetComponent<Image>().color = new Color(1, 0, 1);
            UICanvas.transform.GetChild(2).transform.GetChild(3).GetComponent<Image>().color = new Color(1, 0, 1);
            UICanvas.transform.GetChild(0).GetComponent<Image>().color = new Color(1, 0, 1);
            UICanvas.transform.GetChild(1).GetComponent<Image>().color = new Color(1, 0, 1);

            UICanvas.transform.GetChild(2).transform.GetChild(2).transform.GetChild(0).GetComponent<Text>().text = "通常攻撃";
            UICanvas.transform.GetChild(2).transform.GetChild(3).transform.GetChild(0).GetComponent<Text>().text = "逃げる";

            UICanvas.transform.GetChild(2).transform.GetChild(4).gameObject.SetActive(true);
            UICanvas.transform.GetChild(2).transform.GetChild(5).gameObject.SetActive(true);

            //戦闘BGMを流す
            AllAudioManege.StopBGM();
            AllAudioManege.PlayBGM(DangeonData.BattleBGM);

            TokusyuText(" ",0);
        }

        //バトルに入るとき　明るくなったら
        if (IventChangeSinkou==5&&!BattleChangeImage)
        {
            IventChangeSinkou = 0;

            //KoudouSpeedに速度を入れて比較する

            KoudouSpeed += AllDataManege.CharactorData.KihonParameter[11];
            EnemyKoudouSpeed += EnemyData.EnemyParameter[11];

            TokusyuText(EnemyData.EnemyName + "が現れた！", 0);

            //もし敵の方が速かったら
            if (EnemyKoudouSpeed>KoudouSpeed)
            {
                SenseiInt = 1;
                EnemyKoudou();
            }
            //もし同じ速さだったら運で比べる
            else if (EnemyKoudouSpeed==KoudouSpeed)
            {
                //もし敵の方が運がよかったら
                if (EnemyData.EnemyParameter[12]>AllDataManege.CharactorData.KihonParameter[12])
                {
                    SenseiInt = 1;
                    EnemyKoudou();
                }
                //もし同じ運だったらランダムで決める
                else if (EnemyData.EnemyParameter[12]==AllDataManege.CharactorData.KihonParameter[12])
                {
                    int Random = UnityEngine.Random.Range(0,2);

                    if (Random==1)
                    {
                        SenseiInt = 1;
                        EnemyKoudou();
                    }
                }
            }

            //ここでＢＧＭを戦闘ＢＧＭにする
            AllAudioManege.PlayBGM(DangeonData.BattleBGM);

        }

        //逃げてバトルから出る時　真っ暗になったら
        if (IventChangeSinkou == 3 && BattleChangeImage.color.a == 1f)
        {
            BattleFlag = false;
            CanCommandFlag = true;

            IventChangeSinkou = 0;
            TousouKaisuu = 0;

            KoudouSpeed = 0;
            EnemyKoudouSpeed = 0;

            SenseiInt = 0;

            Destroy(EnemySprite);

            UICanvas.transform.GetChild(2).transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1);
            UICanvas.transform.GetChild(2).transform.GetChild(1).GetComponent<Image>().color = new Color(1, 1, 1);
            UICanvas.transform.GetChild(2).transform.GetChild(2).GetComponent<Image>().color = new Color(1, 1, 1);
            UICanvas.transform.GetChild(2).transform.GetChild(3).GetComponent<Image>().color = new Color(1, 1, 1);
            UICanvas.transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1);
            UICanvas.transform.GetChild(1).GetComponent<Image>().color = new Color(1, 1, 1);

            UICanvas.transform.GetChild(2).transform.GetChild(2).transform.GetChild(0).GetComponent<Text>().text = "進む";
            UICanvas.transform.GetChild(2).transform.GetChild(3).transform.GetChild(0).GetComponent<Text>().text = "出る";

            UICanvas.transform.GetChild(2).transform.GetChild(4).gameObject.SetActive(false);
            UICanvas.transform.GetChild(2).transform.GetChild(5).gameObject.SetActive(false);

            TokusyuText("逃げ出せた！",0);

            if (SkillWindow)
            {
                Destroy(SkillWindow);
            }

            AllDataManege.DataSave();

            //ここでＢＧＭをその階のＢＧＭに変える
            AllAudioManege.StopBGM();
            AllAudioManege.PlayBGM(KaisouData.DangeonBGM);

        }

        //勝利してバトルから出る時　真っ暗になったら
        if (IventChangeSinkou == 4 && BattleChangeImage.color.a == 1f)
        {
            BattleFlag = false;
            BattleEndFlag = false;
            CanCommandFlag = true;

            IventChangeSinkou = 0;
            TousouKaisuu = 0;

            Destroy(EnemySprite);

            UICanvas.transform.GetChild(2).transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1);
            UICanvas.transform.GetChild(2).transform.GetChild(1).GetComponent<Image>().color = new Color(1, 1, 1);
            UICanvas.transform.GetChild(2).transform.GetChild(2).GetComponent<Image>().color = new Color(1, 1, 1);
            UICanvas.transform.GetChild(2).transform.GetChild(3).GetComponent<Image>().color = new Color(1, 1, 1);
            UICanvas.transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1);
            UICanvas.transform.GetChild(1).GetComponent<Image>().color = new Color(1, 1, 1);

            UICanvas.transform.GetChild(2).transform.GetChild(2).transform.GetChild(0).GetComponent<Text>().text = "進む";
            UICanvas.transform.GetChild(2).transform.GetChild(3).transform.GetChild(0).GetComponent<Text>().text = "出る";

            UICanvas.transform.GetChild(2).transform.GetChild(4).gameObject.SetActive(false);
            UICanvas.transform.GetChild(2).transform.GetChild(5).gameObject.SetActive(false);

            TokusyuText(DangeonText[Random.Range(KaisouData.TextGyousuuMin, KaisouData.TextGyousuuMax)],0);

            //もしスキル選択ウィンドウが出たら
            if (SkillWindow)
            {
                Destroy(SkillWindow);
            }

            AllDataManege.DataSave();

            //ここでＢＧＭをその階のＢＧＭに変える
            AllAudioManege.StopBGM();
            AllAudioManege.PlayBGM(KaisouData.DangeonBGM);
        }

        //負けた時に冒険終了シーンへ移る
        if (HaibokuFlag && BattleChangeImage &&BattleChangeImage.color.a == 1f)
        {
            //ここでＢＧＭを止める
            AllAudioManege.MajiStopBGM();

            BattleChangeImage.GetComponent<Animation>().enabled = false;

            SceneManager.LoadScene("BoukenEnd");
        }

        //負けた時に押されたときにバトルチェンジを入れる
        if (HaibokuFlag&&!BattleChangeImage&&Input.GetMouseButtonDown(0))
        {
            BattleChangeImage = Instantiate(BattleChangePrefab, GameObject.Find("SceneChangeCanvas").transform).GetComponent<Image>();
        }

        //HPとMPバーの表示
        HPMPBerReset();
    }

    //ボタンとか
    public void ButtonDown(GameObject Button)
    {
        if (!CanCommandFlag)
        {
            return;
        }

        if (Button.name == "SinkouButton" && !CanSinkouFlag)
        {
            return;
        }

        Image ButtonImage = Button.GetComponent<Image>();

        //戦闘中でないなら
        if (!BattleFlag)
        {
            ButtonImage.color = new Color(0.8f, 0.8f, 0.8f);

        }
        else
        {
            ButtonImage.color = new Color(0.8f, 0f, 0.8f);
        }
    }

    public void ButtonUp(GameObject Button)
    {
        if (!CanCommandFlag)
        {
            return;
        }

        if (Button.name == "SinkouButton" && !CanSinkouFlag)
        {
            return;
        }

        Image ButtonImage = Button.GetComponent<Image>();

        //戦闘中でないなら
        if (!BattleFlag)
        {
            ButtonImage.color = new Color(1f, 1f, 1f);
        }
        else
        {
            ButtonImage.color = new Color(1f, 0f, 1f);
        }

    }

    public void KinouButtonDown(int value)
    {
        if (!CanCommandFlag)
        {
            return;
        }

        //押されたボタンによって処理を変える
        switch(value)
        {
            //脱出ボタンが押されたとき
            case 0:

                //戦闘中でないなら
                if (!BattleFlag)
                {
                    ShiharaiWindow = Instantiate(ShiharaiPrefab, UICanvas.transform);
                    ShiharaiWindow.GetComponent<ShiharaiScript>().ShiharaiType = 4;

                    CanCommandFlag = false;
                }
                else
                {
                    //もし戦闘が終わっていなかったら
                    if (!BattleEndFlag)
                    {
                        ShiharaiWindow = Instantiate(ShiharaiPrefab, UICanvas.transform);
                        ShiharaiWindow.GetComponent<ShiharaiScript>().ShiharaiType = 5;

                        ShiharaiWindow.GetComponent<ShiharaiScript>().EscapeFlag = EscapeHantei();

                        CanCommandFlag = false;
                    }
                    //もし終わっていたら
                    else
                    {
                        //注意ウィンドウを出す
                        TyuuiWindow = Instantiate(TyuuiPrefab, UICanvas.transform);
                        TyuuiWindow.GetComponent<KakuninWindowScript>().KakuninType = 2;

                        TyuuiWindow.transform.GetChild(0).GetComponent<Text>().text = "もう戦闘は終わっている！";

                        CanCommandFlag = false;
                    }
                }

                break;

            //バッグボタンが押されたとき
            case 1:

                BagWindow = Instantiate(BagWindowPrefab, UICanvas.transform);

                BagWindow.GetComponent<DangeonBagScript>().DangeonManeger = this;

                CanCommandFlag = false;

                break;

            //ステータスボタンが押されたとき
            case 2:

                StetasWindow = Instantiate(StetasWindowPrefab, UICanvas.transform);

                StetasWindow.GetComponent<DangeonStetasScript>().DangeonManeger = this;

                CanCommandFlag = false;

                break;

            //メッセージボタンが押されたとき
            case 3:

                //メッセージの表示が出来ないと
                if (!CanMesFlag)
                {
                    return;
                }

                //メッセージが表示中だと
                if (!MesEndFlag)
                {
                    MessageText.text += HyoujiText.Substring(NowMesInt);

                    KeikaTime = 0;
                    NowMesInt = 0;

                    MesEndFlag = true;

                    return;
                }

                //メッセージを表示
                if (MesEndFlag)
                {
                    MessageText.text = "";

                    //もしもバトル中なら
                    if (BattleFlag && !BattleEndFlag)
                    {
                        HyoujiText = BattleText[Random.Range(0, BattleText.Length)];
                        HyoujiText = HyoujiText.Replace("[魔物名]", EnemyData.EnemyName);
                    }

                    //今起こっているイベントによって表示するメッセージを変える
                    switch (HasseiIvent)
                    {
                        //何も起こっていない場合
                        case 0:

                            HyoujiText = DangeonText[Random.Range(KaisouData.TextGyousuuMin, KaisouData.TextGyousuuMax)];

                            break;

                        //宝箱の場合
                        case 2:

                            HyoujiText = KyoutuText[Random.Range(0, 2)];

                            break;

                        //行商人の場合
                        case 3:

                            HyoujiText = KyoutuText[Random.Range(3, 5)];

                            break;

                        //一休み 岩の場合
                        case 4:

                            HyoujiText = KyoutuText[Random.Range(6, 9)];
                            
                            break;

                        //DEX試しの場合
                        case 5:

                            HyoujiText = KyoutuText[Random.Range(9,12)];

                            break;

                        //一休み 切り株の場合
                        case 6:

                            HyoujiText = KyoutuText[Random.Range(13,16)];

                            break;
                    }

                    NowMesInt = 0;

                    MesEndFlag = false;

                    return;
                }

                break;

            //進行ボタンが押されたとき
            case 4:

                {
                    if (!CanSinkouFlag)
                    {
                        return;
                    }

                    CanCommandFlag = false;

                    //戦闘中じゃないなら
                    if (!BattleFlag)
                    {
                        if (CommandParent)
                        {
                            CommandParent.GetComponent<DangeonCommandScript>().OutFlag = true;
                        }

                        SinkouHantei();
                    }
                    //戦闘中なら
                    else
                    {
                        //戦闘が終わっていたら
                        if (BattleEndFlag)
                        {
                            BattleChangeImage = Instantiate(BattleChangePrefab, GameObject.Find("SceneChangeCanvas").transform).GetComponent<Image>();

                            IventChangeSinkou = 4;

                            AllDataManege.TokusyuJoukyou = 1;
                        }
                        else
                        {
                            //物理攻撃の場合
                            if (AllDataManege.NowSoubi[0].GetKougekiType() == 0)
                            {
                                AllAudioManege.PlaySE(AllDataManege.NowSoubi[0].GetSound());

                                int Damege = new int();

                                Damege = AllDataManege.CharactorData.KihonParameter[5] - EnemyData.EnemyParameter[6];

                                EnemyDamage(Damege, AllDataManege.NowSoubi[0].GetWeaponType(), AllDataManege.NowSoubi[0].GetZokusei());
                            }
                            //魔法攻撃の場合
                            else if (AllDataManege.NowSoubi[0].GetKougekiType() == 1)
                            {
                                AllAudioManege.PlaySE(AllDataManege.NowSoubi[0].GetSound());

                                int Damege = new int();

                                Damege = AllDataManege.CharactorData.KihonParameter[7] - EnemyData.EnemyParameter[8];

                                EnemyDamage(Damege, AllDataManege.NowSoubi[0].GetWeaponType(), AllDataManege.NowSoubi[0].GetZokusei());
                            }

                            //パーティクルを生成
                            if (AllDataManege.ItemDataList[AllDataManege.CharactorData.NowSoubi[0]].GetParticle())
                            {
                                Instantiate(AllDataManege.ItemDataList[AllDataManege.CharactorData.NowSoubi[0]].GetParticle());
                            }
                        }
                    }

                    break;
                }


            //スキルボタンが押されたとき
            case 5:

                //もし戦闘が終わっていなかったら
                if (!BattleEndFlag)
                {
                    if (!SkillWindow)
                    {
                        SkillWindow = Instantiate(SkillWindowPrefab, UICanvas.transform);
                    }
                    else
                    {
                        SkillWindow.GetComponent<DangeonSkillScript>().OutFlag = true;
                    }
                }
                //もし終わっていたら
                else
                {
                    //注意ウィンドウを出す
                    TyuuiWindow = Instantiate(TyuuiPrefab, UICanvas.transform);
                    TyuuiWindow.GetComponent<KakuninWindowScript>().KakuninType = 2;

                    TyuuiWindow.transform.GetChild(0).GetComponent<Text>().text = "もう戦闘は終わっている！";

                    CanCommandFlag = false;
                }

                break;

            //バフボタンが押されたとき
            case 6:

                BaffWindow = Instantiate(BaffWindowPrefab, UICanvas.transform);

                CanCommandFlag = false;

                break;
        }
    }


    ////判定系////


    //進行度判定
    public void SinkouHantei()
    {
        //もし進行度が100に行ったら
        if (AllDataManege.DangeonSinkou + KaisouData.SinkouParcent >= 100)
        {
            //もし文字が変わっていたり特殊なコマンドがあったら消す
            //宝箱 行商人 一休み岩 DEX試し 一休み切り株の場合
            if (HasseiIvent >=2)
            {
                UICanvas.transform.GetChild(2).transform.GetChild(2).transform.GetChild(0).GetComponent<Text>().text = "進む";
                UICanvas.transform.GetChild(2).transform.GetChild(2).GetComponent<Image>().sprite = UISprites[0];
                IventSprite.GetComponent<ImageInOutScript>().InFlag = false;
                IventSprite.GetComponent<ImageInOutScript>().OutFlag = true;

                CanSinkouFlag = true;

                HasseiIvent = 0;
                AllDataManege.TokusyuJoukyou = 1;
            }

            //もしいまいる階層が最後の階層ならクリア処理
            if (AllDataManege.DangeonKaisou == DangeonData.KaisouList.Length - 1)
            {
                //もしボスデータが存在していたらボス戦に入る
                if (KaisouData.BossData != null && BossGekiha == 0)
                {
                    CanCommandFlag = true;
                    CanSinkouFlag = false;
                    BossBattle = 1;

                    UICanvas.transform.GetChild(2).transform.GetChild(2).transform.GetChild(0).GetComponent<Text>().text = "";
                    UICanvas.transform.GetChild(2).transform.GetChild(2).GetComponent<Image>().sprite = UISprites[1];

                    //コマンドボタンを作る
                    CommandParent = Instantiate(CommandPrefab, UICanvas.transform);
                    CommandParent.GetComponent<DangeonCommandScript>().CommandType = 2;
                    CommandParent.GetComponent<DangeonCommandScript>().DangeonManeger = this;

                    TokusyuText("奥からプレッシャーを感じる……\nどうしようか？",0);

                    return;
                }

                //もし既にボスを撃破していたら普通に先に進ませる

                Sinkou();

            }
            //そうでないなら
            else
            {
                //もしボスデータが存在していたらボス戦に入る
                if (KaisouData.BossData != null && BossGekiha == 0)
                {
                    CanCommandFlag = true;
                    CanSinkouFlag = false;
                    BossBattle = 1;

                    UICanvas.transform.GetChild(2).transform.GetChild(2).transform.GetChild(0).GetComponent<Text>().text = "";
                    UICanvas.transform.GetChild(2).transform.GetChild(2).GetComponent<Image>().sprite = UISprites[1];

                    //コマンドボタンを作る
                    CommandParent = Instantiate(CommandPrefab, UICanvas.transform);
                    CommandParent.GetComponent<DangeonCommandScript>().CommandType = 2;
                    CommandParent.GetComponent<DangeonCommandScript>().DangeonManeger = this;

                    TokusyuText("奥からプレッシャーを感じる……\nどうしようか？",0);

                    AllDataManege.TokusyuJoukyou = 5;

                    AllDataManege.DataSave();

                    return;
                }

                //もしボスを倒していたら階層を進ませる
                AllDataManege.DangeonKaisou++;

                AllDataManege.DangeonSinkou = 0;

                BossBattle = 0;
                BossGekiha = 0;

                KaisouData = DangeonData.KaisouList[AllDataManege.DangeonKaisou];

                AllDataManege.DataSave();

                KaisouChangeIn = true;
            }

            return;
        }
        //もし進行度が100に行っていなかったら普通に先に進ませる
        else
        {
            Sinkou();
        }
    }

    //逃走できるかどうか判定
    private bool EscapeHantei()
    {
        if (BattleTokusyu.Any(value=>value==1))
        {
            return true;
        }

        int TousouKakuritu = (AllDataManege.CharactorData.KihonParameter[11] * 128 / AllDataManege.CharactorData.KihonParameter[11]) + (30 * TousouKaisuu);

        RandomInt = Random.Range(0, 101);

        if (RandomInt <= TousouKakuritu / 8)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    ////進行////

    //通常進行
    private void Sinkou()
    {
        //進行度を足す
        AllDataManege.DangeonSinkou += KaisouData.SinkouParcent;

        //メッセージをリセット
        MessageText.text = "";

        HyoujiText = DangeonText[Random.Range(KaisouData.TextGyousuuMin, KaisouData.TextGyousuuMax)];

        NowMesInt = 0;

        MesEndFlag = false;

        //もし文字が変わっていたり特殊なコマンドがあったら消す
        //宝箱 行商人 一休み岩 DEX試し 一休み切り株の場合
        if (HasseiIvent >=2)
        {
            UICanvas.transform.GetChild(2).transform.GetChild(2).transform.GetChild(0).GetComponent<Text>().text = "進む";
            UICanvas.transform.GetChild(2).transform.GetChild(2).GetComponent<Image>().sprite = UISprites[0];

            IventSprite.GetComponent<ImageInOutScript>().InFlag = false;
            IventSprite.GetComponent<ImageInOutScript>().OutFlag = true;


            CanSinkouFlag = true;

            HasseiIvent = 0;
            AllDataManege.TokusyuJoukyou = 1;
        }

        //発生するイベントを求める
        RandomInt = Random.Range(0, 101);

        for (int i = 0; i < KaisouData.IventKakuritu.Length - 1; i++)
        {
            if (RandomInt > KaisouData.IventKakuritu[i] && RandomInt <= KaisouData.IventKakuritu[i + 1])
            {
                HasseiIvent = KaisouData.HasseiIvent[i];

                break;
            }
        }

        //イベントごとの処理
        switch (HasseiIvent)
        {
            //0なら何も起きない
            case 0:

                CanCommandFlag = true;

                Debug.Log("何も起こらなかった");

                break;

            //1ならバトルに突入
            case 1:

                {
                    CanCommandFlag = false;

                    IventChangeSinkou = 1;

                    //カメラの震え
                    StartCoroutine("CameraHurue");

                    AllDataManege.TokusyuJoukyou = 2;

                    //敵の種族を取得
                    RandomInt = Random.Range(0, 101);

                    for (int i = 0; i < KaisouData.EnemyKakuritu.Length - 1; i++)
                    {
                        if (RandomInt > KaisouData.EnemyKakuritu[i] && RandomInt <= KaisouData.EnemyKakuritu[i + 1])
                        {
                            //敵のデータを作る
                            EnemyData = MakeEnemyData(EnemySyuzokuList[KaisouData.SouguEnemy[i]]);

                            AllDataManege.EnemyData = EnemyData;

                            //まだ遭遇したことがなかったら遭遇フラッグを立てる
                            if (!AllDataManege.SomethingData.EnemySougu[EnemyData.SyuzokuNumber])
                            {
                                AllDataManege.SomethingData.EnemySougu[EnemyData.SyuzokuNumber] = true;
                            }
                        }
                    }

                    //バフだとかが入っていない元のパラメータを取得してセーブ
                    MotoParameter = AllDataManege.CharactorData.KihonParameter.ToArray();
                    EnemyMotoParameter = AllDataManege.EnemyData.EnemyParameter.ToArray();

                    for (int i = 0; i < 14; i++)
                    {
                        PlayerPrefs.SetInt("MotoParameter" + i, MotoParameter[i]);
                        PlayerPrefs.SetInt("EnemyMotoParameter" + i, EnemyMotoParameter[i]);
                    }

                    BattleParameterReset();

                    AllAudioManege.PlaySE(27);
                }

                break;

            //2なら宝箱を作成
            case 2:

                {
                    CanCommandFlag = true;
                    CanSinkouFlag = false;

                    UICanvas.transform.GetChild(2).transform.GetChild(2).transform.GetChild(0).GetComponent<Text>().text = "";
                    UICanvas.transform.GetChild(2).transform.GetChild(2).GetComponent<Image>().sprite = UISprites[1];

                    //宝箱の画像を作成
                    IventSprite = Instantiate(IventSpritePrefab, IventCanvas.transform);

                    AllDataManege.TokusyuJoukyou = 3;

                    //宝箱の中身を取得
                    RandomInt = Random.Range(0, 101);

                    for (int i = 0; i < KaisouData.TakarabakoKakuritu.Length - 1; i++)
                    {
                        if ((KaisouData.TakarabakoKakuritu[i] < RandomInt) && (RandomInt <= KaisouData.TakarabakoKakuritu[i + 1]))
                        {
                            AllDataManege.TakaraBakoItem = KaisouData.TakarabakoItem[i];

                            if (KaisouData.TakarabakoItem[i] >= 0)
                            {
                                TakarabakoData = AllDataManege.ItemDataList[KaisouData.TakarabakoItem[i]];
                            }

                            break;
                        }
                    }

                    //コマンドボタンを作る
                    CommandParent = Instantiate(CommandPrefab, UICanvas.transform);
                    CommandParent.GetComponent<DangeonCommandScript>().CommandType = 1;
                    CommandParent.GetComponent<DangeonCommandScript>().DangeonManeger = this;

                    TokusyuText("宝箱を見つけた！",0);

                    Debug.Log("宝箱を発見！");
                }

                break;

            //3なら行商人を作成
            case 3:

                {
                    CanCommandFlag = true;
                    CanSinkouFlag = false;

                    UICanvas.transform.GetChild(2).transform.GetChild(2).transform.GetChild(0).GetComponent<Text>().text = "";
                    UICanvas.transform.GetChild(2).transform.GetChild(2).GetComponent<Image>().sprite = UISprites[1];

                    //行商人の画像を作成
                    IventSprite = Instantiate(IventTatiePrefab, IventCanvas.transform);

                    AllDataManege.TokusyuJoukyou = 4;

                    //コマンドボタンを作る
                    CommandParent = Instantiate(CommandPrefab, UICanvas.transform);
                    CommandParent.GetComponent<DangeonCommandScript>().CommandType = 3;
                    CommandParent.GetComponent<DangeonCommandScript>().DangeonManeger = this;

                    TokusyuText("行商人に出会った",0);

                    Debug.Log("行商人に遭遇！");
                }

                break;

            //4なら一休み 岩を作成
            case 4:

                {
                    CanCommandFlag = true;
                    CanSinkouFlag = false;

                    UICanvas.transform.GetChild(2).transform.GetChild(2).transform.GetChild(0).GetComponent<Text>().text = "";
                    UICanvas.transform.GetChild(2).transform.GetChild(2).GetComponent<Image>().sprite = UISprites[1];

                    //岩の画像を作成
                    IventSprite = Instantiate(IventSpritePrefab, IventCanvas.transform);
                    IventSprite.GetComponent<Image>().sprite = IventSprites[1];

                    AllDataManege.TokusyuJoukyou = 6;
                    //5はボス部屋前

                    //コマンドボタンを作る
                    CommandParent = Instantiate(CommandPrefab, UICanvas.transform);
                    CommandParent.GetComponent<DangeonCommandScript>().CommandType = 4;
                    CommandParent.GetComponent<DangeonCommandScript>().DangeonManeger = this;

                    TokusyuText("一休みできそうな岩を見つけた！",0);

                    Debug.Log("一休みできそうな岩を見つけた！");
                }

                break;

            //5ならDEX試しを作成
            case 5:

                {
                    CanCommandFlag = true;
                    CanSinkouFlag = false;

                    UICanvas.transform.GetChild(2).transform.GetChild(2).transform.GetChild(0).GetComponent<Text>().text = "";
                    UICanvas.transform.GetChild(2).transform.GetChild(2).GetComponent<Image>().sprite = UISprites[1];

                    //DEX試しの画像を作成
                    IventSprite = Instantiate(IventSpritePrefab, IventCanvas.transform);
                    IventSprite.GetComponent<Image>().sprite = IventSprites[2];

                    AllDataManege.TokusyuJoukyou = 7;

                    //コマンドボタンを作る
                    CommandParent = Instantiate(CommandPrefab, UICanvas.transform);
                    CommandParent.GetComponent<DangeonCommandScript>().CommandType = 5;
                    CommandParent.GetComponent<DangeonCommandScript>().DangeonManeger = this;

                    TokusyuText("弓と的を見つけた！",0);

                    Debug.Log("弓と的を見つけた！");
                }

                break;

            //6なら一休み 切り株を作成
            case 6:

                {
                    CanCommandFlag = true;
                    CanSinkouFlag = false;

                    UICanvas.transform.GetChild(2).transform.GetChild(2).transform.GetChild(0).GetComponent<Text>().text = "";
                    UICanvas.transform.GetChild(2).transform.GetChild(2).GetComponent<Image>().sprite = UISprites[1];

                    //切り株の画像を作成
                    IventSprite = Instantiate(IventSpritePrefab, IventCanvas.transform);
                    IventSprite.GetComponent<Image>().sprite = IventSprites[3];

                    AllDataManege.TokusyuJoukyou = 8;

                    //コマンドボタンを作る
                    CommandParent = Instantiate(CommandPrefab, UICanvas.transform);
                    CommandParent.GetComponent<DangeonCommandScript>().CommandType = 4;
                    CommandParent.GetComponent<DangeonCommandScript>().DangeonManeger = this;

                    TokusyuText("一休みできそうな切り株を見つけた！", 0);

                    Debug.Log("一休みできそうな岩を見つけた！");
                }

                break;
        }

        AllDataManege.DataSave();
    }


    ////バトル＆敵関連////


    //敵のデータをセット
    public BaseEnemyData MakeEnemyData(BaseEnemySyuzokuData SyuzokuData)
    {
        BaseEnemyData ReturnData = new BaseEnemyData();

        ReturnData.SyuzokuNumber = SyuzokuData.GetSyuzokuInt();
        ReturnData.EnemyName = SyuzokuData.GetSyuzokuName();
        ReturnData.EnemySetumei = SyuzokuData.GetSyuzokuSetumei();
        ReturnData.EnemySprite = SyuzokuData.GetSprite();
        ReturnData.KoudouPattern = SyuzokuData.GetKoudouPattern();
        ReturnData.Koudou = SyuzokuData.GetKoudou();
        ReturnData.DropItemKakuritu = SyuzokuData.GetDropKakuritu();
        ReturnData.DropItem = SyuzokuData.GetDropItem();
        ReturnData.KougekiType = SyuzokuData.GetKougekiType();
        ReturnData.SkillAndMagic = SyuzokuData.GetSkillAndMagic();
        ReturnData.KougekiWeapon = SyuzokuData.GetKougekiWeapon();
        ReturnData.KougekiZokusei = SyuzokuData.GetKougekiZokusei();
        ReturnData.WeekWeaponType = SyuzokuData.GetWeekWeapon();
        ReturnData.WeekZokusei = SyuzokuData.GetWeekZokusei();
        ReturnData.StrongWeaponType = SyuzokuData.GetStrongWeapon();
        ReturnData.StrongZokusei = SyuzokuData.GetStrongZokusei();
        ReturnData.SoundEffect = SyuzokuData.GetSoundEffect();

        //ここからランダムで決める系
        ReturnData.GetExp = Random.Range(SyuzokuData.GetGetExp()[0], SyuzokuData.GetGetExp()[1]);
        ReturnData.GetGold = Random.Range(SyuzokuData.GetGetGold()[0], SyuzokuData.GetGetGold()[1]);

        //ステータス
        ReturnData.EnemyParameter[0] = Random.Range(SyuzokuData.GetSaiteiParameter()[0],SyuzokuData.GetSaikouParameter()[0]);
        ReturnData.EnemyParameter[1] = Random.Range(SyuzokuData.GetSaiteiParameter()[1], SyuzokuData.GetSaikouParameter()[1]);
        ReturnData.EnemyParameter[2] = ReturnData.EnemyParameter[1];
        ReturnData.EnemyParameter[3] = Random.Range(SyuzokuData.GetSaiteiParameter()[2], SyuzokuData.GetSaikouParameter()[2]);
        ReturnData.EnemyParameter[4] = ReturnData.EnemyParameter[3];

        for (int i = 3; i < 12; i++)
        {
            ReturnData.EnemyParameter[i + 2] = Random.Range(SyuzokuData.GetSaiteiParameter()[i], SyuzokuData.GetSaikouParameter()[i]);
        }

        return ReturnData;
    }

    //敵の行動
    public void EnemyKoudou()
    {
        //敵の行動
        int TekiKoudou = new int();

        RandomInt = Random.Range(0, 101);

        //敵の行動を取得
        for (int i = 0; i < EnemyData.KoudouPattern.Length - 1; i++)
        {
            if (EnemyData.KoudouPattern[i] < RandomInt && RandomInt <= EnemyData.KoudouPattern[i + 1])
            {
                TekiKoudou = EnemyData.Koudou[i];
            }
        }


        //行動ごとに処理を変化
        //通常攻撃
        if (TekiKoudou==0)
        {
            //敵の通常攻撃が物理攻撃の場合
            if (EnemyData.KougekiType == 0)
            {
                int Damege = new int();

                Damege = EnemyData.EnemyParameter[5] - AllDataManege.CharactorData.KihonParameter[6];

                MyDamage(Damege, EnemyData.KougekiWeapon, EnemyData.KougekiZokusei);
            }
            //敵の通常攻撃が魔法攻撃の場合
            else if (EnemyData.KougekiType == 1)
            {
                int Damege = new int();

                Damege = EnemyData.EnemyParameter[7] - AllDataManege.CharactorData.KihonParameter[8];

                MyDamage(Damege, EnemyData.KougekiWeapon, EnemyData.KougekiZokusei);
            }
        }
        //スキル使用
        if (TekiKoudou>0)
        {
            AllDataManege.EnemyUseSkill(AllDataManege.SkillDataList[EnemyData.SkillAndMagic[TekiKoudou - 1]]);
        }
    }

    //自分がダメージを受ける
    public void MyDamage(int Damege,int KougekiWeapon,int KougekiZokusei)
    {
        //与えるダメージを計算
        int CriticalResult = new int();

        Damege = Mathf.Clamp(Damege, 0, 99999)+1;

        Damege = Random.Range(Damege+Mathf.CeilToInt(Damege/5),Damege-Mathf.CeilToInt(Damege/5));

        //敵の攻撃が自分の防具の弱点武器だったら
        if (AllDataManege.NowSoubi[1].GetWeekWeapon().Any(value => value == KougekiWeapon))
        {
            Damege = Mathf.CeilToInt(Damege * 1.2f);
        }

        //敵の攻撃が自分の防具の弱点属性だったら
        if (AllDataManege.NowSoubi[1].GetWeekZokusei().Any(value=>value==KougekiZokusei))
        {
            Damege = Mathf.CeilToInt(Damege * 1.2f);
        }

        //クリティカル判定
        if (CriticalHantei(1) == 1)
        {
            Damege = Mathf.CeilToInt(Damege * 1.5f);

            CriticalResult = 1;
        }

        Damege = Mathf.Clamp(Damege, 0, 99999);

        AllDataManege.CharactorData.KihonParameter[2] -= Damege;

        MesEndFlag = false;
        CanMesFlag = true;

        NowMesInt = 0;

        if (MessageText.text != null)
        {
            HyoujiText += "\n";
        }

        //敵がスキルを使っていたら～の攻撃は表示しない
        if (UseSkillFlag==2)
        {
            //必殺を出していたら
            if (CriticalResult==1)
            {
                TokusyuText("必殺！" + Damege + "ダメージ！", 1);
            }
            //必殺を出していなかったら
            else
            {
                TokusyuText(Damege + "ダメージ！", 1);
            }

            UseSkillFlag = 0;
            
        }
        //使っていなかったら通常のテキスト表示
        else
        {
            string BattleText="";

            //必殺を出していたら
            if (CriticalResult==1)
            {
                BattleText += "必殺！";
            }

            //先制攻撃をされていたら
            if (SenseiInt == 1)
            {
                BattleText += EnemyData.EnemyName + "の先制攻撃！\n" + Damege + "ダメージ！";
            }
            //二回行動中なら
            else if (SenseiInt == 5)
            {
                BattleText += EnemyData.EnemyName + "の二回行動！\n" + Damege + "ダメージ！";
            }
            //通常行動なら
            else
            {
                BattleText += EnemyData.EnemyName + "の攻撃！\n" + Damege + "ダメージ！";          
            }

            TokusyuText(BattleText,1);
        }

        StartCoroutine("GamenHurue");

        //もし自分が死んだら
        if (AllDataManege.CharactorData.KihonParameter[2] <= 0)
        {
            HaibokuFlag = true;
            CanCommandFlag = false;

            AllDataManege.TokusyuJoukyou = 0;

            AllDataManege.DataSave();

            PlayerPrefs.DeleteKey("NowPlaying");

            return;
        }

        //もし敵の先制行動の場合はターン経過処理を行わない
        if (SenseiInt == 0)
        {
            TurnKeikaSyori();
        }
        else if (SenseiInt == 1)
        {
            SenseiInt = 0;
        }

        //二回行動の終わりに通常状態に戻る
        if (SenseiInt == 5)
        {
            TurnKeikaSyori();
        }

        //もしも敵の二回行動なら
        if (SenseiInt == 4)
        {
            SenseiInt++;
            EnemyKoudou();
        }
    }

    //敵にダメージを与える
    public void EnemyDamage(int Damege, int KougekiWeapon, int KougekiZokusei)
    {
        CanCommandFlag = false;

        int CriticalResult = new int();

        Damege = Mathf.Clamp(Damege, 0, 99999) + 1;

        Damege = Random.Range(Damege + Mathf.CeilToInt(Damege / 5), Damege - Mathf.CeilToInt(Damege / 5));

        //もし自分の攻撃が相手の弱点武器だったら
        if (EnemyData.WeekWeaponType.Any(value => value == KougekiWeapon))
        {
            Damege = Mathf.CeilToInt(Damege * 1.2f);
        }

        //もし自分の攻撃が相手の弱点属性だったら
        if (EnemyData.WeekZokusei.Any(value => value == KougekiZokusei))
        {
            Damege = Mathf.CeilToInt(Damege * 1.2f);
        }

        //もし自分の攻撃が相手の得意武器だったら
        if (EnemyData.StrongWeaponType.Any(value => value == KougekiWeapon))
        {
            Damege = Mathf.CeilToInt(Damege * 0.8f);

        }

        //もし自分の攻撃が相手の得意属性だったら
        if (EnemyData.StrongZokusei.Any(value => value == KougekiZokusei))
        {
            Damege = Mathf.CeilToInt(Damege * 0.8f);
        }

        //クリティカル判定
        if (CriticalHantei(0)==1)
        {
            Damege = Mathf.CeilToInt(Damege*1.5f);

            CriticalResult = 1;
        }

        Damege = Mathf.Clamp(Damege, 0, 99999);

        AllDataManege.EnemyData.EnemyParameter[2] -= Damege;

        //敵にダメージが入った時のメッセージ表示
        //自分がスキルを使っている場合はテキストの上書きをしない
        if (UseSkillFlag==1)
        {
            //必殺を出していたら
            if (CriticalResult==1)
            {
                TokusyuText("\n必殺！" + EnemyData.EnemyName + "に" + Damege + "ダメージを与えた！", 1);

            }
            //出していなかったら
            else
            {
                TokusyuText("\n" + EnemyData.EnemyName + "に" + Damege + "ダメージを与えた！", 1);
            }

            UseSkillFlag = 0;
        }
        else
        {
            string BattleText = "";

            //必殺を出していたら
            if (CriticalResult==1)
            {
                BattleText += "必殺！";
            }

            //二回行動中なら
            if (SenseiInt==3)
            {
                BattleText += "二回行動！ "+EnemyData.EnemyName+"に"+Damege+"ダメージを与えた！";
            }
            else
            {
                BattleText += EnemyData.EnemyName + "に" + Damege + "ダメージを与えた！";
            }

            TokusyuText(BattleText,0);
        }

        //もし敵が死んだら
        if (AllDataManege.EnemyData.EnemyParameter[2] <= 0)
        {
            SyouriSyori();

            return;
        }

        //自分の二回行動の終わりに二回行動状態から戻る
        if (SenseiInt == 3)
        {
            SenseiInt = 0;
        }

        //もし自分が二回行動中なら敵の行動は行わない
        if (SenseiInt != 2)
        {
            EnemyKoudou();
        }
        else
        {
            SenseiInt++;
            CanCommandFlag = true;
        }
    }

    //クリティカル判定
    public int CriticalHantei(int HanteiType)
    {
        int HanteiResult=new int();
        //1だと判定成功

        //判定をするのはこちら側か敵側か
        switch (HanteiType)
        {
            //自分の場合
            case 0:

                {
                    //クリティカル確率計算
                    int CriticalChance=Mathf.CeilToInt((AllDataManege.CharactorData.KihonParameter[12]-EnemyData.EnemyParameter[12])/2);

                    //特殊バフがかかっているときにクリティカル確率上昇
                    if (AllDataManege.CharactorData.NowJoutai.Any(value=>value==42))
                    {
                        CriticalChance = CriticalChance * 3;
                    }

                    //クリティカル判定
                    if (CriticalChance>=Random.Range(0,101))
                    {
                        HanteiResult = 1;
                    }
                }

                break;
            
            //敵の場合
            case 1:

                {
                    //クリティカル確率計算
                    int CriticalChance=Mathf.CeilToInt((EnemyData.EnemyParameter[12] - AllDataManege.CharactorData.KihonParameter[12]) / 2);

                    //特殊バフがかかっているときにクリティカル確率上昇
                    if (EnemyData.NowJoutai.Any(value => value == 42))
                    {
                        CriticalChance = CriticalChance * 3;
                    }
                    
                    //クリティカル判定
                    if (CriticalChance >= Random.Range(0, 101))
                    {
                        HanteiResult = 1;
                    }
                }

                break;
        }

        return HanteiResult;
    }


    ////戦闘時サブ処理////


    //ターン経過時の処理
    public void TurnKeikaSyori()
    {
        CanCommandFlag = true;

        //KoudouSpeed加算
        //もし自分が二回行動中だったら
        if (SenseiInt!=3)
        {
            KoudouSpeed += AllDataManege.CharactorData.KihonParameter[11];

        }
        else
        {
            KoudouSpeed = 0;
        }

        //EnemyKoudouSpeed加算
        //もし敵が二回行動中だったら
        if (SenseiInt!=5)
        {
            EnemyKoudouSpeed += EnemyData.EnemyParameter[11];
        }
        else
        {
            SenseiInt = 0;
            EnemyKoudouSpeed = 0;
        }

        //主人公のKoudouSpeedが敵のKoudouSpeedを上回っていたら
        if (KoudouSpeed>EnemyKoudouSpeed*2)
        {
            //次に二回行動できるようになる
            SenseiInt = 2;

            KoudouSpeed = 0;

            BaffTurnKeika();
            BattleParameterReset();

            AllDataManege.DataSave();

            return;
        }

        //敵のKoudouSpeedが主人公のKoudouSpeedを上回っていたら
        if (EnemyKoudouSpeed>KoudouSpeed*2)
        {
            //敵が次に二回行動できるようになる
            SenseiInt = 4;

            EnemyKoudouSpeed = 0;

            BaffTurnKeika();
            BattleParameterReset();

            AllDataManege.DataSave();

            return;
        }

        BaffTurnKeika();
        BattleParameterReset();

        AllDataManege.DataSave();

    }

    //ターン経過時のバフについての処理
    public void BaffTurnKeika()
    {
        //自分のバフ経過を減らす
        for (int i = 0; i < AllDataManege.CharactorData.NowJoutai.Count; i++)
        {
            AllDataManege.CharactorData.JoutaiKeika[i]--;

            //もしバフのターン数がおわったら
            if (AllDataManege.CharactorData.JoutaiKeika[i] < 0)
            {
                AllDataManege.CharactorData.JoutaiKeika.RemoveAt(i);
                AllDataManege.CharactorData.NowJoutai.RemoveAt(i);
            }
        }

        //敵のバフ経過を減らす
        for (int i = 0; i < AllDataManege.EnemyData.NowJoutai.Count; i++)
        {
            AllDataManege.EnemyData.JoutaiKeika[i]--;

            if (AllDataManege.EnemyData.JoutaiKeika[i] < 0)
            {
                AllDataManege.EnemyData.JoutaiKeika.RemoveAt(i);
                AllDataManege.EnemyData.NowJoutai.RemoveAt(i);
            }
        }
    }

    //敵と自分の戦闘パラメーター計算
    public void BattleParameterReset()
    {

        //追加パラメータを初期化
        int[] AddParameter = new int[14];
        int[] EnemyAddParameter = new int[14];

        BaseBaffDebaffData BaffData = new BaseBaffDebaffData();

        //武器データを上乗せしたデータを取得
        AddParameter[1] = AllDataManege.NowSoubi[0].GetAddParameter()[0] + AllDataManege.NowSoubi[1].GetAddParameter()[0] + AllDataManege.NowSoubi[2].GetAddParameter()[0];
        AddParameter[3] = AllDataManege.NowSoubi[0].GetAddParameter()[1] + AllDataManege.NowSoubi[1].GetAddParameter()[1] + AllDataManege.NowSoubi[2].GetAddParameter()[1];

        for (int i = 5; i < 14; i++)
        {
            AddParameter[i] = AllDataManege.NowSoubi[0].GetAddParameter()[i - 3] + AllDataManege.NowSoubi[1].GetAddParameter()[i - 3] + AllDataManege.NowSoubi[2].GetAddParameter()[i - 3];
        }

        //武器データを上乗せしたデータにバフの追加パラメータをのせる

        //自分のバフを一つずつ処理
        for (int i = 0; i < AllDataManege.CharactorData.NowJoutai.Count; i++)
        {
            //バフのデータを取得
            BaffData = AllDataManege.BaffDebaffDataList[AllDataManege.CharactorData.NowJoutai[i]];

            //一つのパラメーターずつ処理
            for (int u = 0; u < 14; u++)
            {
                //その分上がる
                if (BaffData.GetKeizokuType() == 0)
                {
                    AddParameter[u] += BaffData.GetAddParameter()[u];
                }

                //継続して上がる
                if (BaffData.GetBaffUpType() == 1)
                {
                    AddParameter[u] += (BaffData.GetKeizokuTurn() - AllDataManege.CharactorData.JoutaiKeika[i]) * BaffData.GetAddParameter()[u];
                }

                //その分下がる
                if (BaffData.GetKeizokuTurn() == 2)
                {
                    AddParameter[u] -= BaffData.GetAddParameter()[u];
                }

                //継続して下がる
                if (BaffData.GetBaffUpType() == 3)
                {
                    AddParameter[u] -= BaffData.GetAddParameter()[u] + ((BaffData.GetKeizokuTurn() - AllDataManege.CharactorData.JoutaiKeika[i]) * BaffData.GetAddParameter()[u]);
                }
            }
        }

        //敵のバフを一つずつ処理
        for (int i = 0; i < AllDataManege.EnemyData.NowJoutai.Count; i++)
        {
            //バフのデータを取得
            BaffData = AllDataManege.BaffDebaffDataList[AllDataManege.EnemyData.NowJoutai[i]];

            //一つのパラメーターずつ処理
            for (int u = 0; u < 14; u++)
            {
                //その分上がる
                if (BaffData.GetKeizokuType() == 0)
                {
                    EnemyAddParameter[u] += BaffData.GetAddParameter()[u];
                }

                //継続して上がる
                if (BaffData.GetBaffUpType() == 1)
                {
                    EnemyAddParameter[u] += (BaffData.GetKeizokuTurn() - AllDataManege.CharactorData.JoutaiKeika[i]) * BaffData.GetAddParameter()[u];
                }

                //その分下がる
                if (BaffData.GetKeizokuTurn() == 2)
                {
                    EnemyAddParameter[u] -= BaffData.GetAddParameter()[u];
                }

                //継続して下がる
                if (BaffData.GetBaffUpType() == 3)
                {
                    EnemyAddParameter[u] -= BaffData.GetAddParameter()[u] + ((BaffData.GetKeizokuTurn() - AllDataManege.CharactorData.JoutaiKeika[i]) * BaffData.GetAddParameter()[u]);
                }
            }
        }

        //追加パラメータを戦闘パラメーターに足す
        AllDataManege.CharactorData.KihonParameter[1] = MotoParameter[1] + AddParameter[1];
        AllDataManege.CharactorData.KihonParameter[2] += AddParameter[2];
        AllDataManege.CharactorData.KihonParameter[3] = MotoParameter[3] + AddParameter[3];
        AllDataManege.CharactorData.KihonParameter[4] += AddParameter[4];

        AllDataManege.EnemyData.EnemyParameter[1] = EnemyMotoParameter[1] + EnemyAddParameter[1];
        AllDataManege.EnemyData.EnemyParameter[2] += EnemyAddParameter[2];
        AllDataManege.EnemyData.EnemyParameter[3] = EnemyMotoParameter[3] + EnemyAddParameter[3];
        AllDataManege.EnemyData.EnemyParameter[4] += EnemyAddParameter[4];

        for (int i = 5; i < 14; i++)
        {
            AllDataManege.CharactorData.KihonParameter[i] = MotoParameter[i] + AddParameter[i];

            AllDataManege.EnemyData.EnemyParameter[i] = EnemyMotoParameter[i] + EnemyAddParameter[i];

            if (AllDataManege.CharactorData.KihonParameter[i] < 0)
            {
                AllDataManege.CharactorData.KihonParameter[i] = 0;
            }

            if (AllDataManege.EnemyData.EnemyParameter[i] < 0)
            {
                AllDataManege.EnemyData.EnemyParameter[i] = 0;
            }
        }

        //HPがHP最大値より上だったら
        if (AllDataManege.CharactorData.KihonParameter[2] > AllDataManege.CharactorData.KihonParameter[1])
        {
            AllDataManege.CharactorData.KihonParameter[2] = AllDataManege.CharactorData.KihonParameter[1];
        }

        if (AllDataManege.EnemyData.EnemyParameter[2] > AllDataManege.EnemyData.EnemyParameter[1])
        {
            AllDataManege.EnemyData.EnemyParameter[2] = AllDataManege.EnemyData.EnemyParameter[1];
        }

        //MPがMP最大値より上だったら
        if (AllDataManege.CharactorData.KihonParameter[4] > AllDataManege.CharactorData.KihonParameter[3])
        {
            AllDataManege.CharactorData.KihonParameter[4] = AllDataManege.CharactorData.KihonParameter[3];
        }

        if (AllDataManege.EnemyData.EnemyParameter[4] > AllDataManege.EnemyData.EnemyParameter[3])
        {
            AllDataManege.EnemyData.EnemyParameter[4] = AllDataManege.EnemyData.EnemyParameter[3];
        }
    }

    //戦闘が終わった時にバフを解除する
    public void ReturnMotoData()
    {
        KoudouSpeed = 0;
        EnemyKoudouSpeed = 0;

        AllDataManege.CharactorData.NowJoutai.Clear();
        AllDataManege.CharactorData.JoutaiKeika.Clear();

        AllDataManege.CharactorData.KihonParameter[1] = MotoParameter[1];
        AllDataManege.CharactorData.KihonParameter[3] = MotoParameter[3];

        for (int i = 5; i < 14; i++)
        {
            AllDataManege.CharactorData.KihonParameter[i] = MotoParameter[i];
        }
    }

    //勝利時処理
    public void SyouriSyori()
    {
        //敵の画像をフェードアウトさせる
        EnemySprite.GetComponent<ImageInOutScript>().OutFlag = true;

        //勝利時の処理をここに
        UICanvas.transform.GetChild(2).transform.GetChild(2).transform.GetChild(0).GetComponent<Text>().text = "進む";

        CanCommandFlag = true;
        BattleEndFlag = true;

        KoudouSpeed = 0;
        EnemyKoudouSpeed = 0;

        SenseiInt = 0;

        AllDataManege.TokusyuJoukyou = 1;
        HasseiIvent = 0;

        TousouKaisuu = 0;

        //戦闘時特殊状況クリア
        BattleTokusyu.Clear();

        //元のデータに戻す
        ReturnMotoData();

        //もしスキルウィンドウがあったらしまう
        if (SkillWindow)
        {
            SkillWindow.GetComponent<DangeonSkillScript>().OutFlag = true;
        }

        //経験値と金を与える
        AllDataManege.CharactorData.NowExp += EnemyData.GetExp;
        AllDataManege.CharactorData.NowSyojikin += EnemyData.GetGold;

        AllDataManege.CharactorData.NowExp = Mathf.Clamp(AllDataManege.CharactorData.NowExp, 0, 9999);
        AllDataManege.CharactorData.NowSyojikin = Mathf.Clamp(AllDataManege.CharactorData.NowSyojikin, 0, 99999);

        //もし経験値がたまっていたらレベルアップ
        int LevelUpKaisu = new int();

        for (int i = AllDataManege.CharactorData.KihonParameter[0]; i < AllDataManege.SyuzokuData.SaikouParameter[0]; i++)
        {
            if (AllDataManege.CharactorData.NowExp >= AllDataManege.SomeIntData.GetLevelUpExp(i - 1))
            {
                LevelUpKaisu++;
            }
            else
            {
                break;

            }
        }

        if (LevelUpKaisu > 0)
        {
            LevelUpWindow = Instantiate(LevelUpPrefab, UICanvas.transform);
            LevelUpWindow.GetComponent<LevelUpWindow>().LevelUpType = 2;
            LevelUpWindow.GetComponent<LevelUpWindow>().LevelUpKaisu = LevelUpKaisu;

            CanCommandFlag = false;
        }

        //もしも敵種族の撃破フラグがたってなかったらたてる
        if (!AllDataManege.SomethingData.EnemyGekiha[EnemyData.SyuzokuNumber])
        {
            AllDataManege.SomethingData.EnemyGekiha[EnemyData.SyuzokuNumber] = true;
        }

        //もしもボス戦だったらボス撃破フラグを立てる
        if (BossBattle == 1)
        {
            BossGekiha = 1;

            //もしも最終階層ならダンジョンクリア
            if (AllDataManege.DangeonKaisou == DangeonData.KaisouList.Length - 1 && !AllDataManege.SomethingData.DangeonKouryaku[DangeonData.DangeonNumber])
            {
                AllDataManege.SomethingData.DangeonKouryaku[DangeonData.DangeonNumber] = true;

                //クリアウィンドウを出す
                TyuuiWindow = Instantiate(TyuuiPrefab, UICanvas.transform);
                TyuuiWindow.GetComponent<KakuninWindowScript>().KakuninType = 2;

                TyuuiWindow.transform.GetChild(0).GetComponent<Text>().text = "ダンジョンをクリアした！";

                CanCommandFlag = false;
            }
        }

        //勝利テキストを設定
        SyouriText = EnemyData.EnemyName + "に勝利した！";

        if (EnemyData.GetExp > 0)
        {
            SyouriText += "\n" + EnemyData.GetExp + "経験値を得た！";
        }

        if (EnemyData.GetGold > 0)
        {
            SyouriText += "\n" + EnemyData.GetGold + "ゴールドを得た！";
        }

        //ドロップアイテムを与える
        RandomInt = Random.Range(0, 101);

        //ドロップアイテムを取得
        for (int i = 0; i < EnemyData.DropItemKakuritu.Length - 1; i++)
        {
            if (RandomInt > EnemyData.DropItemKakuritu[i] && RandomInt <= EnemyData.DropItemKakuritu[i + 1])
            {
                DropItem = AllDataManege.ItemDataList[EnemyData.DropItem[i]];

                //ドロップアイテムがあったら
                if (EnemyData.DropItem[i] != 0)
                {
                    //勝利テキストを設定
                    SyouriText += "\n" + DropItem.GetItemName() + "を入手した！";

                    //バッグに空きがあったら
                    if (AllDataManege.CharactorData.BagItemInt.Any(value => value == 0))
                    {
                        for (int u = 0; u < 9; u++)
                        {
                            if (AllDataManege.CharactorData.BagItemInt[u] == 0)
                            {

                                AllDataManege.CharactorData.BagItemInt[u] = DropItem.GetItemNumber();

                                break;
                            }
                        }
                    }
                    //空きがなかったら
                    else
                    {
                        //空きがなかった場合入れ替え処理に入る
                        ItemIrekaeWindow = Instantiate(ItemIrekaePrefab, UICanvas.transform);
                        ItemIrekaeWindow.GetComponent<ItemChangeScript>().SyutokuItem = DropItem.GetItemNumber();

                        CanSinkouFlag = true;
                        CanCommandFlag = false;
                    }
                }

                break;
            }
        }

        //勝利テキストを表示
        TokusyuText(SyouriText,0);

        AllAudioManege.StopBGM();

        AllDataManege.DataSave();
    }


    ////その他いろいろ////


    //HPMPバーリセット
    public void HPMPBerReset()
    {
        int HPBer = Mathf.Clamp(AllDataManege.CharactorData.KihonParameter[2],0,99999);
        int MPBer = Mathf.Clamp(AllDataManege.CharactorData.KihonParameter[4],0,99999);

        HPBerFloat = (float)HPBer / AllDataManege.CharactorData.KihonParameter[1];
        MPBerFloat = (float)MPBer / AllDataManege.CharactorData.KihonParameter[3];

        HPBerText.text = HPBer.ToString();
        MPBerText.text = MPBer.ToString();

        HPBerRect.localScale = new Vector3(HPBerFloat, 1, 1);
        MPBerRect.localScale = new Vector3(MPBerFloat, 1, 1);
    }

    //宝箱を開けたら
    public void OpenTakarabako()
    {
        IventSprite.GetComponent<Image>().sprite = IventSprites[0];
        IventSprite.GetComponent<ImageInOutScript>().InFlag = false;
        IventSprite.GetComponent<ImageInOutScript>().OutFlag = true;

        AllAudioManege.PlaySE(28);

        //特殊な宝箱イベントによって分岐する
        switch (AllDataManege.TakaraBakoItem)
        {
            //罠だったら
            case -1:

                TokusyuText("しまった！　爆発罠だった……",0);

                UICanvas.transform.GetChild(2).transform.GetChild(2).transform.GetChild(0).GetComponent<Text>().text = "進む";
                UICanvas.transform.GetChild(2).transform.GetChild(2).GetComponent<Image>().sprite = UISprites[0];

                Instantiate(Particles[0]);

                //ダメージ処理
                AllDataManege.StetasDamege(Random.Range(KaisouData.TakarabakoDamege[0], KaisouData.TakarabakoDamege[1]));

                //もし死んだら
                if (AllDataManege.CharactorData.KihonParameter[2] <= 0)
                {
                    HaibokuFlag = true;
                    CanCommandFlag = false;

                    BattleChangeImage = Instantiate(BattleChangePrefab, GameObject.Find("SceneChangeCanvas").transform).GetComponent<Image>();

                    return;
                }

                HasseiIvent = 0;
                AllDataManege.TokusyuJoukyou = 1;

                CanSinkouFlag = true;

                AllDataManege.DataSave();

                break;

            //ゴールドだったら
            case 0:

                int GetGold = Random.Range(KaisouData.TakarabakoGold[0], KaisouData.TakarabakoGold[1]);

                //ゴールドを取得
                AllDataManege.CharactorData.NowSyojikin += GetGold;
                AllDataManege.CharactorData.NowSyojikin = Mathf.Clamp(AllDataManege.CharactorData.NowSyojikin, 0, 99999);

                TokusyuText(GetGold + "Ｇを手に入れた！",0);

                HasseiIvent = 0;
                AllDataManege.TokusyuJoukyou = 1;

                CanSinkouFlag = true;

                UICanvas.transform.GetChild(2).transform.GetChild(2).transform.GetChild(0).GetComponent<Text>().text = "進む";
                UICanvas.transform.GetChild(2).transform.GetChild(2).GetComponent<Image>().sprite = UISprites[0];

                AllDataManege.DataSave();

                break;
        }

        //アイテムだったら
        if (AllDataManege.TakaraBakoItem>0)
        {
            //アイテムを取得

            //空きがあるかどうかを判定
            if (AllDataManege.CharactorData.BagItemInt.Any(value => value == 0))
            {
                //空きがある場合宝箱のアイテムを取得する
                for (int i = 0; i < 9; i++)
                {
                    if (AllDataManege.CharactorData.BagItemInt[i] == 0)
                    {
                        AllDataManege.CharactorData.BagItemInt[i] = AllDataManege.TakaraBakoItem;

                        HasseiIvent = 0;
                        AllDataManege.TokusyuJoukyou = 1;

                        CanSinkouFlag = true;

                        UICanvas.transform.GetChild(2).transform.GetChild(2).transform.GetChild(0).GetComponent<Text>().text = "進む";
                        UICanvas.transform.GetChild(2).transform.GetChild(2).GetComponent<Image>().sprite = UISprites[0];

                        TokusyuText(AllDataManege.ItemDataList[AllDataManege.TakaraBakoItem].GetItemName() + "を手に入れた！",0);

                        AllDataManege.DataSave();

                        break;
                    }
                }
            }
            //空きがなかったら
            else
            {
                //空きがなかった場合入れ替え処理に入る
                ItemIrekaeWindow = Instantiate(ItemIrekaePrefab, UICanvas.transform);
                ItemIrekaeWindow.GetComponent<ItemChangeScript>().SyutokuItem = AllDataManege.TakaraBakoItem;

                HasseiIvent = 0;
                AllDataManege.TokusyuJoukyou = 1;

                CanSinkouFlag = true;
                CanCommandFlag = false;

                UICanvas.transform.GetChild(2).transform.GetChild(2).transform.GetChild(0).GetComponent<Text>().text = "進む";
                UICanvas.transform.GetChild(2).transform.GetChild(2).GetComponent<Image>().sprite = UISprites[0];
            }
        }

    }

    //特殊なテキストを表示するときに使うやつ
    public void TokusyuText(string TokusyuText,int AddType)
    {
        //AddType
        //0 前のテキストを全消しして表示する 1 前のテキストを消さずに追加する

        if (AddType == 0)
        {
            MessageText.text = "";

            HyoujiText = TokusyuText;

            NowMesInt = 0;
        }
        else if (AddType == 1)
        {
            //もしこの時点で表示が終わっていたら
            if (MesEndFlag)
            {
                NowMesInt = 0;

                HyoujiText = TokusyuText;

            }
            else
            {
                HyoujiText += TokusyuText;
            }
        }

        CanMesFlag = true;
        MesEndFlag = false;

    }

    //バトルに入るときにカメラを震わすコルーチン
    private IEnumerator CameraHurue()
    {
        for (int i = 0; i < 10; i++)
        {
            //偶数の時
            if (i % 2 == 0)
            {
                Camera.transform.Translate(Random.Range(0.1f, 0.15f), Random.Range(-0.15f, -0.1f), 0);
            }
            //奇数の時
            if (i % 2 == 1)
            {
                Camera.transform.Translate(Random.Range(-0.15f, -0.1f), Random.Range(0.1f, 0.15f), 0);
            }

            yield return new WaitForSeconds(0.05f);
        }

        Camera.transform.position = new Vector3(0, 0, 0);

        IventChangeSinkou++;

        BattleChangeImage = Instantiate(BattleChangePrefab, GameObject.Find("SceneChangeCanvas").transform).GetComponent<Image>();
    }

    //ワンポイントで画面を揺らす
    private IEnumerator GamenHurue()
    {
        for (int i = 0; i < 10; i++)
        {
            //偶数の時
            if (i % 2 == 0)
            {
                Camera.transform.Translate(Random.Range(0.025f, 0.05f), Random.Range(-0.05f, -0.025f), 0);
            }
            //奇数の時
            if (i % 2 == 1)
            {
                Camera.transform.Translate(Random.Range(-0.05f, -0.025f), Random.Range(0.025f, 0.05f), 0);
            }

            yield return new WaitForSeconds(0.05f);
        }

        Camera.transform.position = new Vector3(0, 0, 0);
    }
}
