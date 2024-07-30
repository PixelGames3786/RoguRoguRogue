using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadScript : MonoBehaviour
{
    //キャラクターデータの保存
    public static void CharactorSave(BaseCharactorData SaveYouData)
    {
        PlayerPrefs.SetString("CharactorData", JsonUtility.ToJson(SaveYouData));
    }

    //キャラクターデータのロード
    public static BaseCharactorData CharactorLoad()
    {
        BaseCharactorData LoadYouData = new BaseCharactorData();

        LoadYouData = JsonUtility.FromJson<BaseCharactorData>(PlayerPrefs.GetString("CharactorData"));

        return LoadYouData;
    }

    //町データの保存
    public static void TownSave(BaseTownData SaveYouData)
    {
        PlayerPrefs.SetString("TownData",JsonUtility.ToJson(SaveYouData));
    }

    //町データのロード
    public static BaseTownData TownLoad()
    {
        BaseTownData LoadYouData = new BaseTownData();

        LoadYouData=JsonUtility.FromJson<BaseTownData>(PlayerPrefs.GetString("TownData"));

        return LoadYouData;
    }

    //そのほか諸々の保存
    public static void SomethingSave(SomethingData SaveYouData)
    {
        PlayerPrefs.SetString("SomethingData",JsonUtility.ToJson(SaveYouData));
    }

    //そのほか諸々のロード
    public static SomethingData SomethingLoad()
    {
        SomethingData LoadYouData = new SomethingData();

        LoadYouData = JsonUtility.FromJson<SomethingData>(PlayerPrefs.GetString("SomethingData"));

        return LoadYouData;
    }

    //ダンジョンデータの保存
    public static void DangeonSave(BaseDangeonData SaveYouData)
    {
        PlayerPrefs.SetString("DangeonData",JsonUtility.ToJson(SaveYouData));
    }

    //ダンジョンデータのロード
    public static BaseDangeonData DangeonLoad()
    {
        BaseDangeonData LoadYouData = new BaseDangeonData();

        LoadYouData = JsonUtility.FromJson<BaseDangeonData>(PlayerPrefs.GetString("DangeonData"));

        return LoadYouData;
    }

    //エネミーデータの保存
    public static void EnemySave(BaseEnemyData SaveYouData)
    {
        PlayerPrefs.SetString("EnemyData",JsonUtility.ToJson(SaveYouData));
    }

    //エネミーデータのロード
    public static BaseEnemyData EnemyLoad()
    {
        BaseEnemyData LoadYouData = new BaseEnemyData();

        LoadYouData = JsonUtility.FromJson<BaseEnemyData>(PlayerPrefs.GetString("EnemyData"));

        return LoadYouData;
    }
}
