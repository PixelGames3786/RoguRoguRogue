using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AllDataManege : MonoBehaviour
{
    //データベース
    public SyuzokuDataBase SyuzokuDataBase;
    public JobDataBese JobDataBase;
    public ItemDataBase ItemDataBase;
    public SkillDataBase SkillDataBase;
    public SomethingData SomethingDataBase;
    public BaseTownData TownDataBase;
    public SkillMagicTypeBase SkillMagicType;
    public SomeIntData SomeIntDataBase;
    public DangeonDataBase DangeonDataBase;
    public BaffDebaffDataBase BaffDataBase;

    //色々なリストを取得
    public static List<BaseSyuzokuData> SyuzokuDataList = new List<BaseSyuzokuData>();
    public static List<BaseJobData> JobDataList = new List<BaseJobData>();
    public static List<BaseItemData> ItemDataList = new List<BaseItemData>();
    public static BaseItemData[] BagItemList = new BaseItemData[9];
    public static List<BaseSkillData> SkillDataList = new List<BaseSkillData>();
    public static List<BaseSkillMagicType> SkillMagicTypeList = new List<BaseSkillMagicType>();
    public static List<BaseDangeonData> DangeonDataList = new List<BaseDangeonData>();
    public static List<BaseBaffDebaffData> BaffDebaffDataList = new List<BaseBaffDebaffData>();

    //他のスクリプトに渡すためのデータ
    public static BaseCharactorData CharactorData;
    public static BaseSyuzokuData SyuzokuData;
    public static BaseJobData JobData;
    public static SomethingData SomethingData;
    public static BaseTownData TownData;
    public static BaseSkillMagicType SyutokuType;
    public static SomeIntData SomeIntData;
    public static BaseDangeonData DangeonData;
    public static BaseEnemyData EnemyData;

    //レベルアップの時に使う
    public static bool LevelUpFlag, SkillSyutokuFlag;
    public static bool[] ParameterUpFlag = new bool[11];

    private static int[] LevelUpParameter = new int[14];

    private static int SkillSyutokuKekka;

    //スキル処理時に使用
    private static int[] SkillKaeri = new int[2];
    private static int[] EnemySkillKaeri = new int[2];
    //0 スキル実行の結果 1 スキルの威力

    //スキル処理時&戦闘時に使用
    private static int[] SomeStetas = new int[10];
    //0 物理命中率 1 魔法命中率 2 物理回避率 3 魔法回避率 4 必殺率 

    //汎用
    private static int[] SetParameter = new int[14];

    public static BaseItemData[] NowSoubi = new BaseItemData[3];

    //広告用スクリプト
    public static UnityAdsListener AdsListener;

    //課金用スクリプト
    public static IAPListener IAPListener;
    public static AdmobListener AdmobListener;

    //課金の二重を防ぐため
    public static bool NowKakinSentaku;

    //ダンジョンやバトルしているときにセーブするため
    public static int TokusyuJoukyou;
    //1 ダンジョン内 2 戦闘 3 宝箱 4 行商人 5 ボス部屋前 6 一休み 岩 7 AGI試し 8 一休み 切り株 

    //ダンジョン内にいるとき
    public static DangeonManeger DangeonManeger;

    public static int DangeonKaisou, DangeonSinkou, SentakuDangeon, TakaraBakoItem, TousouKaisu;

    public static float MesTime;

    //ゲーム終了時
    public static bool CanGetKeiken;


    //一番最初に行われる処理
    void Awake()
    {
        //フレームレートをセット
        Application.targetFrameRate = 60;

        //ランダムのシードを日時から取得
        Random.InitState(System.DateTime.Now.Second + System.DateTime.Now.Millisecond + System.DateTime.Now.Hour);

        SomeIntData = SomeIntDataBase;
        SyuzokuDataList = SyuzokuDataBase.GetSyuzokuDataList();        //種族データベースを取得
        JobDataList = JobDataBase.GetJobDataList();                    //職業データベースを取得
        ItemDataList = ItemDataBase.GetItemDataBase();                 //アイテムデータベースを取得
        SkillDataList = SkillDataBase.GetSkillDataBase();              //スキルデータベースを取得
        SkillMagicTypeList = SkillMagicType.GetSkillMagicTypeList();   //習得スキル型を取得
        DangeonDataList = DangeonDataBase.GetDangeonDataBase();        //ダンジョンを取得
        BaffDebaffDataList = BaffDataBase.GetDataBase();               //バフデバフデータベースを取得

        TownData = new BaseTownData();
        CharactorData = new BaseCharactorData();
        SomethingData = new SomethingData();

        //本当に最初にプレイする時
        if (!PlayerPrefs.HasKey("FirstPlay"))
        {
            DefaultSet();

            PlayerPrefs.SetInt("FirstPlay", 1);
        }
        else
        {
            DataLoad();
        }

        //CharactorData.NowSyojikin += 50000;
        CharactorData.KihonParameter[0] += 10;

        //バッグの中身を取得
        for (int i = 0; i < 9; i++)
        {
            BagItemList[i] = ItemDataList[CharactorData.BagItemInt[i]];
        }

        //種族と職業とスキル習得タイプを取得
        SyuzokuData = SyuzokuDataList[CharactorData.GetKihonData()[1]];
        JobData = JobDataList[CharactorData.GetKihonData()[2]];
        SyutokuType = SkillMagicTypeList[CharactorData.GetSkillType()];

        //現在の装備を取得
        NowSoubi[0] = ItemDataList[CharactorData.GetNowSoubi()[0]];
        NowSoubi[1] = ItemDataList[CharactorData.GetNowSoubi()[1]];
        NowSoubi[2] = ItemDataList[CharactorData.GetNowSoubi()[2]];

        //広告用を取得
        AdsListener = GameObject.Find("AdsListener").GetComponent<UnityAdsListener>();
        AdmobListener = GameObject.Find("AdmobListener").GetComponent<AdmobListener>();

        //課金用を取得
        IAPListener = GameObject.Find("IAPListener").GetComponent<IAPListener>();

        //もしも一つのゲームをプレイ中じゃないと
        if (!PlayerPrefs.HasKey("NowPlaying"))
        {
            CharactorDataReset();
        }

        //毎回最初からの時
        if (!PlayerPrefs.HasKey("TextSpeed"))
        {
            PlayerPrefs.SetFloat("TextSpeed", 0.08f);
        }

        MesTime = PlayerPrefs.GetFloat("TextSpeed");
    }

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    //常に呼び出される
    void Update()
    {
        //バッグの中身を取得
        for (int i = 0; i < 9; i++)
        {
            BagItemList[i] = ItemDataList[CharactorData.BagItemInt[i]];
        }

        //現在の装備
        NowSoubi[0] = ItemDataList[CharactorData.GetNowSoubi()[0]];
        NowSoubi[1] = ItemDataList[CharactorData.GetNowSoubi()[1]];
        NowSoubi[2] = ItemDataList[CharactorData.GetNowSoubi()[2]];

        //命中率とかを常時更新
        SomeStetas[0] = CharactorData.GetParameter()[9] + NowSoubi[0].GetBukiMeityu();
        SomeStetas[1] = Mathf.CeilToInt((CharactorData.GetParameter()[9] + CharactorData.GetParameter()[12]) / 2) + NowSoubi[0].GetBukiMeityu();
        SomeStetas[2] = CharactorData.GetParameter()[10];
        SomeStetas[3] = Mathf.CeilToInt(CharactorData.GetParameter()[11] + CharactorData.GetParameter()[12]);
        SomeStetas[4] = Mathf.CeilToInt((CharactorData.GetParameter()[9] + CharactorData.GetParameter()[12]) / 2) + NowSoubi[0].GetBukiHissatu();
    }


    ////パラメーター////


    //最初から始めた時にキャラのデータをリセット
    public void CharactorDataReset()
    {
        CharactorData = new BaseCharactorData();

        //色々なものをセット
        CharactorData.KihonParameter[0] = 1;
        CharactorData.KihonParameter[1] = 10;
        CharactorData.KihonParameter[2] = 10;
        CharactorData.KihonParameter[3] = 3;
        CharactorData.KihonParameter[4] = 3;
        CharactorData.KihonParameter[5] = 3;
        CharactorData.KihonParameter[6] = 3;
        CharactorData.KihonParameter[7] = 3;
        CharactorData.KihonParameter[8] = 3;
        CharactorData.KihonParameter[9] = 2;
        CharactorData.KihonParameter[10] = 2;
        CharactorData.KihonParameter[11] = 2;
        CharactorData.KihonParameter[12] = 4;
        CharactorData.KihonParameter[13] = 2;

        //ダンジョンの攻略状況をリセット
        for (int i=0;i<DangeonDataList.Count;i++)
        {
            SomethingData.DangeonKouryaku[i] = false;
        }

        //町の発展に応じてGoldを与える
        CharactorData.SetSyojikin(SomeIntData.GetHattenGetGold(TownData.HattenLevel));

        //後で
        CharactorData.NowSyojikin = 99999;


        for (int i = 0; i < 9; i++)
        {
            CharactorData.BagItemInt[i] = 0;
        }

    }

    //一番最初に始めた時にいろんなとこのデフォルトデータをセットする
    public void DefaultSet()
    {
        //後で直す
        TownData.CommandKaihou[0] = 1;
        TownData.CommandKaihou[1] = 1;
        //TownData.CommandKaihou[2] = 1;
        TownData.CommandKaihou[3] = 1;
        TownData.CommandKaihou[4] = 1;
        //TownData.CommandKaihou[5] = 1;
        //TownData.CommandKaihou[6] = 1;

        TownData.DouguyaItemInt.Add(71);
        TownData.BukiyaItemInt.Add(1);
        TownData.BouguyaItemInt.Add(41);

        CharactorData.KihonParameter[0] = 1;
        CharactorData.KihonParameter[1] = 10;
        CharactorData.KihonParameter[2] = 10;
        CharactorData.KihonParameter[3] = 3;
        CharactorData.KihonParameter[4] = 3;
        CharactorData.KihonParameter[5] = 3;
        CharactorData.KihonParameter[6] = 3;
        CharactorData.KihonParameter[7] = 3;
        CharactorData.KihonParameter[8] = 3;
        CharactorData.KihonParameter[9] = 2;
        CharactorData.KihonParameter[10] = 2;
        CharactorData.KihonParameter[11] = 2;
        CharactorData.KihonParameter[12] = 4;
        CharactorData.KihonParameter[13] = 2;


        //後で治す
        SomethingData.NowhaveTabikeiken = 999;
        CharactorData.NowSyojikin = 99999;

        //後で改修する
        CharactorData.SkillAndMagic[0] = 71;

        SomethingData.JobKaihouJoukyou[0] = 1;
        SomethingData.SyuzokuKaihouJoukyou[0] = 1;

        //後で消す
        //CharactorData.BagItemInt[0] = 142;
        //CharactorData.BagItemInt[1] = 81;

        DataSave();

    }

    //レベルアップ時の処理
    public static void CharactorLevelUp()
    {
        //ランダムにパラメーターが増える
        LevelUpParameter = CharactorData.GetParameter();

        //LVアップ
        LevelUpParameter[0]++;

        //HPアップ
        if (SyuzokuData.GetGrowParameter()[1] + JobData.GetGrowParameter()[1] >= Random.Range(0, 101))
        {
            LevelUpParameter[1] += Random.Range(SyuzokuData.GetGrowSaiteiParameter()[1],SyuzokuData.GetGrowSaikouParameter()[1]);

            ParameterUpFlag[0] = true;

            //もし上限を超えていたら
            if (LevelUpParameter[1] >= SyuzokuData.GetMaxParameter()[1])
            {
                LevelUpParameter[1] = SyuzokuData.GetMaxParameter()[1];
            }

            LevelUpParameter[2] = CharactorData.KihonParameter[2];
        }

        //MPアップ
        if (SyuzokuData.GetGrowParameter()[2] + JobData.GetGrowParameter()[2] >= Random.Range(0, 101))
        {
            LevelUpParameter[3] += Random.Range(SyuzokuData.GetGrowSaiteiParameter()[2], SyuzokuData.GetGrowSaikouParameter()[2]);

            ParameterUpFlag[1] = true;

            //もし上限を超えていたら
            if (LevelUpParameter[3] >= SyuzokuData.GetMaxParameter()[2])
            {
                LevelUpParameter[3] = SyuzokuData.GetMaxParameter()[2];
            }

            LevelUpParameter[4] = CharactorData.KihonParameter[4];
        }

        //そのほかのパラメータアップ
        for (int i = 3; i < 12; i++)
        {
            if (SyuzokuData.GetGrowParameter()[i] + JobData.GetGrowParameter()[i] >= Random.Range(0, 101))
            {
                LevelUpParameter[i + 2] += Random.Range(SyuzokuData.GetGrowSaiteiParameter()[i], SyuzokuData.GetGrowSaikouParameter()[i]);

                ParameterUpFlag[i - 1] = true;

                //上限に達していたら
                if (LevelUpParameter[i + 2] >= SyuzokuData.GetMaxParameter()[i])
                {
                    LevelUpParameter[i + 2] = SyuzokuData.GetMaxParameter()[i];

                    //ParameterUpFlag[i - 1] = false;
                }
            }
        }

        //レベルアップ全回復
        LevelUpParameter[2] = LevelUpParameter[1];
        LevelUpParameter[4] = LevelUpParameter[3];

        CharactorData.SetKihonParameter(LevelUpParameter);

        DataSave();
    }

    //装備の効果を足したパラメーターを求める
    public static int[] HyoujiParameterReset()
    {
        int[] HyoujiParameter = new int[16];
        int[] AddParameter = new int[14];

        //武器による追加パラメータを求める
        AddParameter[1] = NowSoubi[0].GetAddParameter()[0] + NowSoubi[1].GetAddParameter()[0] + NowSoubi[2].GetAddParameter()[0];
        AddParameter[3] = NowSoubi[0].GetAddParameter()[1] + NowSoubi[1].GetAddParameter()[1] + NowSoubi[2].GetAddParameter()[1];

        for (int i = 5; i < 14; i++)
        {
            AddParameter[i] = NowSoubi[0].GetAddParameter()[i - 3] + NowSoubi[1].GetAddParameter()[i - 3] + NowSoubi[2].GetAddParameter()[i - 3];
        }

        HyoujiParameter[0] = CharactorData.KihonParameter[0];

        //追加パラメータと元のパラメータを足す
        for (int i = 1; i < 14; i++)
        {
            HyoujiParameter[i] = CharactorData.KihonParameter[i] + AddParameter[i];
            HyoujiParameter[i] = Mathf.Clamp(HyoujiParameter[i],0,9999);
        }

        HyoujiParameter[14] = CharactorData.GetSyojikin();
        HyoujiParameter[15] = CharactorData.GetNowExp();

        return HyoujiParameter;
    }

    //敵のバフやら装備の効果を足したパラメーターを求める
    public static int[] EnemyBattleParameter()
    {
        int[] ReturnParameter = new int[14];

        //バフによる追加パラメータを求める

        return ReturnParameter;
    }


    ////回復&ダメージ////


    //ステータスを入力した割合分回復
    public static void StetasWariaiKaihuku(float Wariai, int KaihukuType)
    {
        int[] SetYouInt = new int[14];

        if (KaihukuType<=3)
        {
            SetYouInt = CharactorData.GetParameter();
        }
        else
        {
            SetYouInt = EnemyData.GetParameter();
        }

        //タイプによって回復するパラメーターを変える
        switch (KaihukuType)
        {
            //HP回復
            case 1:
            case 4:

                SetYouInt[2] += Mathf.CeilToInt(SetYouInt[1] * Wariai);

                SetYouInt[2] = Mathf.Clamp(SetYouInt[2], 0, SetYouInt[1]);

                break;

            //MP回復
            case 2 :
            case 5 :

                SetYouInt[4] += Mathf.CeilToInt(SetYouInt[3] * Wariai);

                SetYouInt[4] = Mathf.Clamp(SetYouInt[4], 0, SetYouInt[3]);

                break;

            //HPMPどっちも回復
            case 3:
            case 6:

                SetYouInt[2] += Mathf.CeilToInt(SetYouInt[1] * Wariai);
                SetYouInt[4] += Mathf.CeilToInt(SetYouInt[3] * Wariai);

                SetYouInt[2] = Mathf.Clamp(SetYouInt[2], 0, SetYouInt[1]);
                SetYouInt[4] = Mathf.Clamp(SetYouInt[4], 0, SetYouInt[3]);

                break;
        }

        if (KaihukuType<=3)
        {
            CharactorData.SetKihonParameter(SetYouInt);
        }
        else
        {
            EnemyData.SetParameter(SetYouInt);
        }
    }

    //ステータスを入力した数字分回復
    public static void StetasKaihuku(int value, int KaihukuType)
    {
        int[] SetYouInt = new int[14];

        if (KaihukuType <= 3)
        {
            SetYouInt = CharactorData.GetParameter();
        }
        else
        {
            SetYouInt = EnemyData.GetParameter();
        }

        //タイプによって回復するパラメーターを変える
        switch (KaihukuType)
        {
            //HP回復
            case 1:
            case 4:

                SetYouInt[2] += value;

                SetYouInt[2] = Mathf.Clamp(SetYouInt[2], 0, SetYouInt[1]);

                break;

            //MP回復
            case 2:
            case 5:

                SetYouInt[4] += value;

                SetYouInt[4] = Mathf.Clamp(SetYouInt[4], 0, SetYouInt[3]);

                break;

            //HPMPどっちも回復
            case 3:
            case 6:

                SetYouInt[2] += value;
                SetYouInt[4] += value;

                SetYouInt[2] = Mathf.Clamp(SetYouInt[2], 0, SetYouInt[1]);
                SetYouInt[4] = Mathf.Clamp(SetYouInt[4], 0, SetYouInt[3]);

                break;
        }

        //タイプによって表示するテキストを変える
        switch (KaihukuType)
        {
            case 1:

                DangeonManeger.TokusyuText("\nHPが"+value+"回復した！",1);

                break;

            case 2:

                DangeonManeger.TokusyuText("\nMPが" + value + "回復した！", 1);

                break;

            case 3:

                DangeonManeger.TokusyuText("\nHPとMPが" + value + "回復した！", 1);

                break;

            case 4:

                DangeonManeger.TokusyuText("\n敵のHPが" + value+"回復した！",1);

                break;

            case 5:

                DangeonManeger.TokusyuText("\n敵のMPが" + value + "回復した！", 1);

                break;

            case 6:

                DangeonManeger.TokusyuText("\n敵のHPとMPが" + value + "回復した！", 1);

                break;
        }


        if (KaihukuType <= 3)
        {
            CharactorData.SetKihonParameter(SetYouInt);
        }
        else
        {
            EnemyData.SetParameter(SetYouInt);
        }
    }

    //HPを入力した数字分減らす
    public static void StetasDamege(int value)
    {
        CharactorData.KihonParameter[2] -= value;

        CharactorData.KihonParameter[2] = Mathf.Clamp(CharactorData.KihonParameter[2], 0, CharactorData.KihonParameter[1]);

        DataSave();
    }


    ////スキル関連////


    //自分がスキルを使用
    public static int[] UseSkill(BaseSkillData SiyouSkill)
    {
        //SkillKaeriti
        //0 スキル使用の結果 0 成功 1 HPorMPが足りない 2 使用条件を満たしていない
        //1 スキルの威力

        SetParameter = CharactorData.GetParameter();

        //使用条件を満たしていない場合ここで戻す
        switch (SiyouSkill.GetTokusyuType())
        {
            //剣装備でないと使用できない場合
            case 2:

                //剣を装備していなかったら戻す
                if (NowSoubi[0].GetWeaponType() != 1)
                {
                    SkillKaeri[0] = 2;

                    return SkillKaeri;
                }

                break;
        }

        //MPまたはHPが足りない場合
        if (SiyouSkill.GetSkillSyouhi()[0] > SetParameter[2] || SiyouSkill.GetSkillSyouhi()[1] > SetParameter[4])
        {
            SkillKaeri[0] = 1;

            return SkillKaeri;
        }
        //足りていたらMPとHPを消費する
        else
        {
            SetParameter[2] -= SiyouSkill.GetSkillSyouhi()[0];
            SetParameter[4] -= SiyouSkill.GetSkillSyouhi()[1];

            CharactorData.SetKihonParameter(SetParameter);

            SkillKaeri[0] = 0;
        }


        DangeonManeger.UseSkillFlag = 1;

        //もしあったらパーティクルの生成
        if (SiyouSkill.GetPartile())
        {
            Instantiate(SiyouSkill.GetPartile());
        }

        //効果音があったら流す
        if (SiyouSkill.GetSound()!=0)
        {
            AllAudioManege.PlaySE(SiyouSkill.GetSound());
        }

        //スキル名表示
        DangeonManeger.TokusyuText(SiyouSkill.GetSkillName() + "！", 0);

        //スキルのタイプによって処理を変える
        switch (SiyouSkill.GetSkillType())
        {
            //物理攻撃だったら
            case 1:

                SkillKaeri[1] = CharactorData.GetParameter()[5] + NowSoubi[0].GetAddParameter()[2] + SiyouSkill.GetSkillIryoku();

                //スキルのタイプによって威力を変えたりする
                switch (SiyouSkill.GetTokusyuType())
                {
                    //剣を装備していると威力上昇タイプだと
                    case 1:

                        //もし剣を装備していたら
                        if (NowSoubi[0].GetWeaponType() == 1)
                        {
                            SkillKaeri[1] = CharactorData.GetParameter()[5] + NowSoubi[0].GetAddParameter()[2] + Mathf.CeilToInt(SiyouSkill.GetSkillIryoku() * 1.5f);
                        }

                        break;

                    //斧を装備していたら威力上昇タイプだと
                    case 3:

                        //もし斧を装備していたら
                        if (NowSoubi[0].GetWeaponType() == 5)
                        {
                            SkillKaeri[1] = CharactorData.GetParameter()[5] + NowSoubi[0].GetAddParameter()[2] + Mathf.CeilToInt(SiyouSkill.GetSkillIryoku() * 1.5f);
                        }

                        break;

                    //槍を装備していたら威力上昇タイプだと
                    case 5:

                        //もし槍を装備していたら
                        if (NowSoubi[0].GetWeaponType() == 2)
                        {
                            SkillKaeri[1] = CharactorData.GetParameter()[5] + NowSoubi[0].GetAddParameter()[2] + Mathf.CeilToInt(SiyouSkill.GetSkillIryoku() * 1.5f);
                        }

                        break;

                    //弓を装備していたら威力上昇タイプだと
                    case 7:

                        //もし弓を装備していたら
                        if (NowSoubi[0].GetWeaponType()==3)
                        {
                            SkillKaeri[1] = CharactorData.GetParameter()[5] + NowSoubi[0].GetAddParameter()[2] + Mathf.CeilToInt(SiyouSkill.GetSkillIryoku() * 1.5f);
                        }

                        break;

                    //槌を装備していたら威力上昇タイプだと
                    case 9:

                        //もし槌を装備していたら
                        if (NowSoubi[0].GetWeaponType()==6)
                        {
                            SkillKaeri[1] = CharactorData.GetParameter()[5] + NowSoubi[0].GetAddParameter()[2] + Mathf.CeilToInt(SiyouSkill.GetSkillIryoku() * 1.5f);
                        }

                        break;

                    //特殊なタイプではなかったらそのままの威力
                    default:

                        SkillKaeri[1] = CharactorData.GetParameter()[5] + NowSoubi[0].GetAddParameter()[2] + SiyouSkill.GetSkillIryoku();

                        break;
                }

                DangeonManeger.EnemyDamage(SkillKaeri[1] - EnemyData.EnemyParameter[6], NowSoubi[0].GetWeaponType(), SiyouSkill.GetZokusei());

                break;

            //魔法攻撃だったら
            case 2:

                SkillKaeri[1] = CharactorData.GetParameter()[7] + NowSoubi[0].GetAddParameter()[4] + SiyouSkill.GetSkillIryoku();

                //スキルのタイプによって威力を変えたりする
                switch (SiyouSkill.GetTokusyuType())
                {
                    //剣を装備していると威力上昇タイプだと
                    case 1:

                        //もし剣を装備していたら
                        if (NowSoubi[0].GetWeaponType() == 1)
                        {
                            SkillKaeri[1] = CharactorData.GetParameter()[7] + NowSoubi[0].GetAddParameter()[4] + Mathf.CeilToInt(SiyouSkill.GetSkillIryoku() * 1.5f);
                        }
                        //装備していなかったら通常の威力
                        else
                        {
                            SkillKaeri[1] = CharactorData.GetParameter()[7] + NowSoubi[0].GetAddParameter()[4] + SiyouSkill.GetSkillIryoku();
                        }

                        break;

                    //斧を装備していたら威力上昇タイプだと
                    case 3:

                        //もし斧を装備していたら
                        if (NowSoubi[0].GetWeaponType() == 5)
                        {
                            SkillKaeri[1] = CharactorData.GetParameter()[7] + NowSoubi[0].GetAddParameter()[4] + Mathf.CeilToInt(SiyouSkill.GetSkillIryoku() * 1.5f);
                        }
                        //装備していなかったら通常の威力
                        else
                        {
                            SkillKaeri[1] = CharactorData.GetParameter()[7] + NowSoubi[0].GetAddParameter()[4] + SiyouSkill.GetSkillIryoku();
                        }

                        break;

                    //特殊なタイプではなかったらそのままの威力
                    default:

                        SkillKaeri[1] = CharactorData.GetParameter()[7] + NowSoubi[0].GetAddParameter()[2] + SiyouSkill.GetSkillIryoku();

                        break;
                }

                DangeonManeger.EnemyDamage(SkillKaeri[1] - EnemyData.EnemyParameter[8], NowSoubi[0].GetWeaponType(), SiyouSkill.GetZokusei());

                break;

            //HP回復系だったら
            case 3:

                StetasKaihuku(SiyouSkill.GetSkillIryoku(), 1);
                
                break;

            //MP回復系だったら
            case 4:

                StetasKaihuku(SiyouSkill.GetSkillIryoku(), 2);
                
                break;

            //どっちも回復系だったら
            case 5:

                StetasKaihuku(SiyouSkill.GetSkillIryoku(), 3);
                
                break;

            //割合HP回復系だったら
            case 6:

                StetasWariaiKaihuku(SiyouSkill.GetSkillIryoku() * 0.1f, 1);

                break;

            //割合MP回復系だったら
            case 7:

                StetasWariaiKaihuku(SiyouSkill.GetSkillIryoku() * 0.1f, 2);
                
                break;

            //割合どっちも回復系だったら
            case 8:

                StetasWariaiKaihuku(SiyouSkill.GetSkillIryoku() * 0.1f, 3);
                
                break;

            //自分にバフを付ける系だったら
            case 9:

                //バフをセットする
                CharactorData.NowJoutai.Add(SiyouSkill.GetAddBaffNum());
                CharactorData.JoutaiKeika.Add(BaffDebaffDataList[SiyouSkill.GetAddBaffNum()].GetKeizokuTurn());
                
                break;

            //敵にバフを付ける系だったら
            case 10:

                //バフをセットする
                EnemyData.NowJoutai.Add(SiyouSkill.GetAddBaffNum());
                EnemyData.JoutaiKeika.Add(BaffDebaffDataList[SiyouSkill.GetAddBaffNum()].GetKeizokuTurn());
                
                break;
        }

        //攻撃スキルはEnemyDamage()で二回行動時のターン経過処理を行うけどそれ以外のスキルはここで行う
        if (SiyouSkill.GetSkillType()>=3&&DangeonManeger.BattleFlag)
        {
            //二回行動の終わりに二回行動状態から戻る
            if (DangeonManeger.SenseiInt == 3)
            {
                DangeonManeger.SenseiInt = 0;
            }

            //もし自分が二回行動中なら敵の行動は行わない
            if (DangeonManeger.SenseiInt != 2)
            {
                DangeonManeger.EnemyKoudou();
            }
            else
            {
                DangeonManeger.SenseiInt++;
            }
        }

        return SkillKaeri;
    }

    //敵がスキルを使用
    public static int[] EnemyUseSkill(BaseSkillData SiyouSkill)
    {
        //EnemySkillKaeriti
        //0 スキル使用の結果 0 成功 1 HPorMPが足りない 2 使用条件を満たしていない
        //1 スキルの威力

        SetParameter = EnemyData.GetParameter();

        //MPまたはHPが足りない場合は通常攻撃に移行する
        if (SiyouSkill.GetSkillSyouhi()[0] > SetParameter[2] || SiyouSkill.GetSkillSyouhi()[1] > SetParameter[4])
        {
            SkillKaeri[0] = 1;

            //敵の通常攻撃が物理攻撃の場合
            if (EnemyData.KougekiType == 0)
            {
                int Damege = new int();

                Damege = EnemyData.EnemyParameter[5] - CharactorData.KihonParameter[6];

                DangeonManeger.MyDamage(Damege, EnemyData.KougekiWeapon, EnemyData.KougekiZokusei);
            }
            //敵の通常攻撃が魔法攻撃の場合
            else if (EnemyData.KougekiType == 1)
            {
                int Damege = new int();

                Damege = EnemyData.EnemyParameter[7] - CharactorData.KihonParameter[8];

                DangeonManeger.MyDamage(Damege, EnemyData.KougekiWeapon, EnemyData.KougekiZokusei);
            }

            return SkillKaeri;
        }
        //足りていたらMPとHPを消費する
        else
        {
            SetParameter[2] -= SiyouSkill.GetSkillSyouhi()[0];
            SetParameter[4] -= SiyouSkill.GetSkillSyouhi()[1];

            EnemyData.SetParameter(SetParameter);

            SkillKaeri[0] = 0;
        }


        DangeonManeger.UseSkillFlag = 2;

        //スキル名表示
        //敵二回行動の時
        if (DangeonManeger.SenseiInt==5)
        {
            DangeonManeger.TokusyuText(EnemyData.EnemyName + "の二回行動！　" + SiyouSkill.GetSkillName() + "！", 0);
        }
        else
        {
            DangeonManeger.TokusyuText(EnemyData.EnemyName + "の" + SiyouSkill.GetSkillName() + "！", 0);
        }


        //スキルのタイプによって処理を変える
        switch (SiyouSkill.GetSkillType())
        {
            //物理攻撃だったら
            case 1:

                SkillKaeri[1] = EnemyData.EnemyParameter[3] + SiyouSkill.GetSkillIryoku();

                DangeonManeger.MyDamage(SkillKaeri[1]-CharactorData.KihonParameter[6], NowSoubi[0].GetWeaponType(), SiyouSkill.GetZokusei());

                break;

            //魔法攻撃だったら
            case 2:

                SkillKaeri[1] = EnemyData.EnemyParameter[5] + SiyouSkill.GetSkillIryoku();

                DangeonManeger.MyDamage(SkillKaeri[1] - CharactorData.KihonParameter[8], NowSoubi[0].GetWeaponType(), SiyouSkill.GetZokusei());

                break;

            //HP回復系だったら
            case 3:

                StetasKaihuku(SiyouSkill.GetSkillIryoku(), 4);

                break;

            //MP回復系だったら
            case 4:

                StetasKaihuku(SiyouSkill.GetSkillIryoku(), 5);

                break;

            //どっちも回復系だったら
            case 5:

                StetasKaihuku(SiyouSkill.GetSkillIryoku(), 6);

                break;

            //割合HP回復系だったら
            case 6:

                StetasWariaiKaihuku(SiyouSkill.GetSkillIryoku() * 0.1f, 4);

                break;

            //割合MP回復系だったら
            case 7:

                StetasWariaiKaihuku(SiyouSkill.GetSkillIryoku() * 0.1f, 5);

                break;

            //割合どっちも回復系だったら
            case 8:

                StetasWariaiKaihuku(SiyouSkill.GetSkillIryoku() * 0.1f, 6);

                break;

            //自分にバフを付ける系だったら
            case 9:

                //バフをセットする
                EnemyData.NowJoutai.Add(SiyouSkill.GetAddBaffNum());
                EnemyData.JoutaiKeika.Add(BaffDebaffDataList[SiyouSkill.GetAddBaffNum()].GetKeizokuTurn());

                break;

            //敵にバフを付ける系だったら
            case 10:

                //バフをセットする
                CharactorData.NowJoutai.Add(SiyouSkill.GetAddBaffNum());
                CharactorData.JoutaiKeika.Add(BaffDebaffDataList[SiyouSkill.GetAddBaffNum()].GetKeizokuTurn());

                break;
        }

        //攻撃スキルはEnemyKoudou()で二回行動時のターン経過処理を行うけどそれ以外のスキルはここで行う
        if (SiyouSkill.GetSkillType() >= 3)
        {
            //もし敵の先制行動の場合はターン経過処理を行わない
            if (DangeonManeger.SenseiInt == 0)
            {
                DangeonManeger.TurnKeikaSyori();
            }
            else if (DangeonManeger.SenseiInt == 1)
            {
                DangeonManeger.SenseiInt = 0;
            }

            //二回行動の終わりに通常状態に戻る
            if (DangeonManeger.SenseiInt == 5)
            {
                DangeonManeger.SenseiInt = 0;
                DangeonManeger.TurnKeikaSyori();
            }

            //もしも敵の二回行動なら
            if (DangeonManeger.SenseiInt == 4)
            {
                DangeonManeger.SenseiInt++;
                DangeonManeger.EnemyKoudou();
            }
        }

        return EnemySkillKaeri;
    }

    //特殊スキルの処理(スキル処理の別スクリプト)
    private static void UseTokusyuSkill(BaseSkillData SiyouSkill,int SiyouType)
    {
        //SiyouType
        //0 自分が使用 1 敵が使用
    }

    //スキル入手判定
    public static int[] SkillSyutokuHantei(LevelUpWindow LevelUpScript)
    {
        int[] Kaeriti = new int[2];
        //0 スキル取得の結果 1 取得するスキル

        //レベルに到達していたら
        if (CharactorData.GetParameter()[0]==JobData.GetSkillLevel()[CharactorData.GetSkillSyutoku()])
        {
            //スキル取得処理
            Kaeriti[0] = SkillSyutoku(JobData.GetSyutokuSkill()[CharactorData.GetSkillSyutoku()],LevelUpScript);
            Kaeriti[1] = JobData.GetSyutokuSkill()[CharactorData.GetSkillSyutoku() - 1];

            return Kaeriti;
        }

        //旧職業と種族交互入手時のスクリプト
        {
            /*

            //スキルを取得するかどうか
            if (CharactorData.GetSkillSyutoku() % 2 == 0)
            {
                //職業スキル

                //レベルに到達していたら(初回)
                if (CharactorData.GetSkillSyutoku() == 0 && CharactorData.GetParameter()[0] == JobData.GetSkillLevel()[CharactorData.GetSkillSyutoku()])
                {
                    //スキル取得処理
                    Kaeriti[0] = SkillSyutoku(SkillDataList[JobData.GetSyutokuSkill()[CharactorData.GetSkillSyutoku()]].GetSkillNumber());
                    Kaeriti[1] = SkillDataList[JobData.GetSyutokuSkill()[CharactorData.GetSkillSyutoku() - 1]].GetSkillNumber();

                    return Kaeriti;
                }

                //レベルに到達していたら(初回以降)
                if (CharactorData.GetSkillSyutoku() > 0 && CharactorData.GetParameter()[0] == JobData.GetSkillLevel()[CharactorData.GetSkillSyutoku() - 1])
                {
                    //スキル取得処理
                    Kaeriti[0] = SkillSyutoku(SkillDataList[JobData.GetSyutokuSkill()[CharactorData.GetSkillSyutoku() - 1]].GetSkillNumber());
                    Kaeriti[1] = SkillDataList[JobData.GetSyutokuSkill()[CharactorData.GetSkillSyutoku() - 2]].GetSkillNumber();

                    return Kaeriti;
                }

            }
            else
            {
                //種族スキル

                //レベルに到達していたら
                if (CharactorData.GetParameter()[0] == SyutokuType.GetSkillLevel()[CharactorData.GetSkillSyutoku() - 1])
                {
                    //スキル取得処理
                    Kaeriti[0] = SkillSyutoku(SkillDataList[SyutokuType.GetSyutokuSkill()[CharactorData.GetSkillSyutoku() - 1]].GetSkillNumber());
                    Kaeriti[1] = SkillDataList[SyutokuType.GetSyutokuSkill()[CharactorData.GetSkillSyutoku() - 2]].GetSkillNumber();

                    return Kaeriti;
                }

            }

        */
        }

        return Kaeriti;
    }

    //スキルを入手処理
    public static int SkillSyutoku(int SyutokuNumber,LevelUpWindow LevelUpScript)
    {
        int Kaeriti = new int();
        //0の場合はスキル取得に失敗
        //1の場合は円満にスキルを取得できた
        //2の場合はスキルに空きがなかったため入れ替え処理を行う

        int[] SkillSet = new int[6];

        SkillSet = CharactorData.GetSkillMagic();

        CharactorData.SetSkillSyutoku(CharactorData.GetSkillSyutoku() + 1);

        //ランダム系のスキルだったら
        switch (SyutokuNumber)
        {
            //ミニ魔法シリーズ
            case 991:

                {
                    int RandomSkill = Random.Range(0,4);

                    //ランダムの結果によって取得スキルを変える
                    switch (RandomSkill)
                    {
                        //ミニファイア
                        case 0:

                            SyutokuNumber = 51;

                            break;

                        //ミニアクア
                        case 1:

                            SyutokuNumber = 55;

                            break;

                        //ミニガイア
                        case 2:

                            SyutokuNumber = 59;

                            break;

                        //ミニウィンド
                        case 3:

                            SyutokuNumber = 63;

                            break;
                    }
                }

                break;

            //通常魔法シリーズ
            case 992:

                {
                    int RandomSkill = Random.Range(0,4);

                    //ランダムの結果によって取得スキルを変える
                    switch (RandomSkill)
                    {
                        //ファイア
                        case 0:

                            SyutokuNumber = 52;

                            break;

                        //アクア
                        case 1:

                            SyutokuNumber = 56;

                            break;

                        //ガイア
                        case 2:

                            SyutokuNumber = 60;

                            break;

                        //ウィンド
                        case 3:

                            SyutokuNumber = 64;

                            break;
                    }
                }

                break;

            //バ魔法シリーズ
            case 993:

                {
                    int RandomSkill = Random.Range(0,4);

                    //ランダムの結果によって取得スキルを変える
                    switch (RandomSkill)
                    {
                        //バファイア
                        case 0:

                            SyutokuNumber = 53;

                            break;

                        //バアクア
                        case 1:

                            SyutokuNumber = 57;

                            break;

                        //バガイア
                        case 2:

                            SyutokuNumber = 61;

                            break;

                        //バウィンド
                        case 3:

                            SyutokuNumber = 65;

                            break;
                    }
                }

                break;

            //バイ魔法シリーズ
            case 994:

                {
                    int RandomSkill = Random.Range(0,4);

                    //ランダムの結果によって取得スキルを変える
                    switch (RandomSkill)
                    {
                        //バイファイア
                        case 0:

                            SyutokuNumber = 54;

                            break;

                        //バイアクア
                        case 1:

                            SyutokuNumber = 58;

                            break;

                        //バイガイア
                        case 2:

                            SyutokuNumber = 62;

                            break;

                        //バイウィンド
                        case 3:

                            SyutokuNumber = 66;

                            break;
                    }
                }

                break;

            //矢シリーズ
            case 995:

                {
                    int RandomSkill = Random.Range(0,3);

                    //火炎矢電光矢疾風矢は番号が並んでいるので18を足すだけでおｋ
                    SyutokuNumber = RandomSkill + 18;
                }

                break;
        }

        //もし空きがなかったら
        if (SkillSet.All(value => value != 0))
        {
            //もし覚えていなかったら
            if (SkillSet.All(value => value != SyutokuNumber))
            {
                Kaeriti = 2;

                LevelUpScript.SkillKoukanKaisu++;
                LevelUpScript.SkillKoukanList.Add(SyutokuNumber);

                return Kaeriti;
            }
            else //すでに覚えていたら
            {
                Kaeriti = 0;

                return Kaeriti;
            }
        }
        //空きがあったら
        else
        {
            //もし覚えていなかったら
            if (SkillSet.All(value => value != SyutokuNumber))
            {
                //空きに当たるまで判定
                for (int i = 0; i < 6; i++)
                {
                    //空きに当たったら
                    if (SkillSet[i] == 0)
                    {

                        SkillSet[i] = SyutokuNumber;

                        CharactorData.SetSkillMagic(SkillSet);

                        Kaeriti = 1;

                        break;
                    }
                }

                return Kaeriti;
            }
            else //すでに覚えていたら
            {
                Kaeriti = 0;

                return Kaeriti;
            }
        }
    }


    ////データ関連////


    //データセーブ
    public static void DataSave()
    {
        //場所や状態によってセーブするデータを変える
        switch (TokusyuJoukyou)
        {
            //もし村にいる状態なら
            case 0:

                SaveLoadScript.CharactorSave(CharactorData);
                SaveLoadScript.TownSave(TownData);
                SaveLoadScript.SomethingSave(SomethingData);

                break;

            //ダンジョンにいるなら
            case 1: case 4: case 5: case 6: case 7: case 8:

                SaveLoadScript.CharactorSave(CharactorData);
                SaveLoadScript.TownSave(TownData);
                SaveLoadScript.SomethingSave(SomethingData);

                PlayerPrefs.SetInt("DangeonKaisou", DangeonKaisou);
                PlayerPrefs.SetInt("DangeonSinkou", DangeonSinkou);
                PlayerPrefs.SetInt("SentakuDangeon", SentakuDangeon);
                PlayerPrefs.SetInt("BossGekiha", DangeonManeger.BossGekiha);
                PlayerPrefs.SetInt("BossBattle", DangeonManeger.BossBattle);

                break;

            //戦闘中なら
            case 2:

                SaveLoadScript.CharactorSave(CharactorData);
                SaveLoadScript.TownSave(TownData);
                SaveLoadScript.SomethingSave(SomethingData);
                SaveLoadScript.EnemySave(EnemyData);

                PlayerPrefs.SetInt("DangeonKaisou", DangeonKaisou);
                PlayerPrefs.SetInt("DangeonSinkou", DangeonSinkou);
                PlayerPrefs.SetInt("SentakuDangeon", SentakuDangeon);

                PlayerPrefs.SetInt("TousouKaisu", DangeonManeger.TousouKaisuu);
                PlayerPrefs.SetInt("BossGekiha", DangeonManeger.BossGekiha);
                PlayerPrefs.SetInt("BossBattle", DangeonManeger.BossBattle);

                PlayerPrefs.SetInt("KoudouSpeed",DangeonManeger.KoudouSpeed);
                PlayerPrefs.SetInt("EnemyKoudouSpeed",DangeonManeger.EnemyKoudouSpeed);
                PlayerPrefs.SetInt("SenseiInt",DangeonManeger.SenseiInt);

                //戦闘中の特殊状況を保存
                PlayerPrefs.SetString("BattleTokusyu", JsonUtility.ToJson(new Serialization<int>(DangeonManeger.BattleTokusyu)));

                //自分のバフデバフを保存
                PlayerPrefs.SetString("NowJoutai", JsonUtility.ToJson(new Serialization<int>(CharactorData.NowJoutai)));
                PlayerPrefs.SetString("JoutaiKeika", JsonUtility.ToJson(new Serialization<int>(CharactorData.JoutaiKeika)));

                //敵のバフデバフを保存
                PlayerPrefs.SetString("EnemyNowJoutai", JsonUtility.ToJson(new Serialization<int>(EnemyData.NowJoutai)));
                PlayerPrefs.SetString("EnemyJoutaiKeika", JsonUtility.ToJson(new Serialization<int>(EnemyData.JoutaiKeika)));

                break;

            //宝箱を見つけていたなら
            case 3:

                SaveLoadScript.CharactorSave(CharactorData);
                SaveLoadScript.TownSave(TownData);
                SaveLoadScript.SomethingSave(SomethingData);

                PlayerPrefs.SetInt("DangeonKaisou", DangeonKaisou);
                PlayerPrefs.SetInt("DangeonSinkou", DangeonSinkou);
                PlayerPrefs.SetInt("SentakuDangeon", SentakuDangeon);
                PlayerPrefs.SetInt("TakaraBakoItem", TakaraBakoItem);

                break;

        }

        PlayerPrefs.SetInt("TokusyuJoukyou", TokusyuJoukyou);
    }

    //データロード
    public static void DataLoad()
    {
        TokusyuJoukyou = PlayerPrefs.GetInt("TokusyuJoukyou");

        //場所や状態によってセーブするデータを変える
        switch (TokusyuJoukyou)
        {
            //もし村にいる状態なら
            case 0:

                TownData = SaveLoadScript.TownLoad();
                CharactorData = SaveLoadScript.CharactorLoad();
                SomethingData = SaveLoadScript.SomethingLoad();

                break;

            //ダンジョンにいるなら
            case 1: case 4: case 5: case 6: case 7: case 8:

                TownData = SaveLoadScript.TownLoad();
                CharactorData = SaveLoadScript.CharactorLoad();
                SomethingData = SaveLoadScript.SomethingLoad();

                DangeonKaisou = PlayerPrefs.GetInt("DangeonKaisou");
                DangeonSinkou = PlayerPrefs.GetInt("DangeonSinkou");
                SentakuDangeon = PlayerPrefs.GetInt("SentakuDangeon");
                DangeonManeger.BossBattle = PlayerPrefs.GetInt("BossBattle");
                DangeonManeger.BossGekiha = PlayerPrefs.GetInt("BossGekiha");

                DangeonData = DangeonDataList[SentakuDangeon];

                break;

            //戦闘中なら
            case 2:

                TownData = SaveLoadScript.TownLoad();
                CharactorData = SaveLoadScript.CharactorLoad();
                SomethingData = SaveLoadScript.SomethingLoad();
                EnemyData = SaveLoadScript.EnemyLoad();

                DangeonKaisou = PlayerPrefs.GetInt("DangeonKaisou");
                DangeonSinkou = PlayerPrefs.GetInt("DangeonSinkou");
                SentakuDangeon = PlayerPrefs.GetInt("SentakuDangeon");

                DangeonManeger.TousouKaisuu = PlayerPrefs.GetInt("TousouKaisu");
                DangeonManeger.BossBattle = PlayerPrefs.GetInt("BossBattle");
                DangeonManeger.BossGekiha = PlayerPrefs.GetInt("BossGekiha");

                DangeonManeger.KoudouSpeed = PlayerPrefs.GetInt("KoudouSpeed");
                DangeonManeger.EnemyKoudouSpeed = PlayerPrefs.GetInt("EnemyKoudouSpeed");

                DangeonManeger.SenseiInt = PlayerPrefs.GetInt("SenseiInt");

                //戦闘中の特殊状況を取得
                DangeonManeger.BattleTokusyu= JsonUtility.FromJson<Serialization<int>>(PlayerPrefs.GetString("BattleTokusyu")).ToList();

                //自分のバフデバフを取得
                CharactorData.NowJoutai = JsonUtility.FromJson<Serialization<int>>(PlayerPrefs.GetString("NowJoutai")).ToList();
                CharactorData.JoutaiKeika = JsonUtility.FromJson<Serialization<int>>(PlayerPrefs.GetString("JoutaiKeika")).ToList();

                //敵のバフデバフを取得
                EnemyData.NowJoutai = JsonUtility.FromJson<Serialization<int>>(PlayerPrefs.GetString("EnemyNowJoutai")).ToList();
                EnemyData.JoutaiKeika = JsonUtility.FromJson<Serialization<int>>(PlayerPrefs.GetString("EnemyJoutaiKeika")).ToList();

                DangeonData = DangeonDataList[SentakuDangeon];

                break;

            //宝箱を見つけていたなら
            case 3:

                TownData = SaveLoadScript.TownLoad();
                CharactorData = SaveLoadScript.CharactorLoad();
                SomethingData = SaveLoadScript.SomethingLoad();

                DangeonKaisou = PlayerPrefs.GetInt("DangeonKaisou");
                DangeonSinkou = PlayerPrefs.GetInt("DangeonSinkou");
                SentakuDangeon = PlayerPrefs.GetInt("SentakuDangeon");
                TakaraBakoItem = PlayerPrefs.GetInt("TakarabakoItem");

                DangeonData = DangeonDataList[SentakuDangeon];

                break;
        }
    }

    //ゲームオーバー処理
    public static void GameOverSyori()
    {

    }
}
