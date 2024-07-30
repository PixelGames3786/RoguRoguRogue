using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;//Advertisementクラスを使うのに必要

public class UnityAdsListener : MonoBehaviour,IUnityAdsListener
{
    //=================================================================================
    //初期化
    //=================================================================================

    public int AdsType;

    //動画広告のPlacementID
    private static readonly string VIDEO_PLACEMENT_ID = "video";

    private void Start()
    {

        DontDestroyOnLoad(this.gameObject);

        //ゲームIDをAndroidとそれ以外(iOS)で分ける
#if UNITY_ANDROID
        string gameID = "3557884";
#else
    string gameID = "3557885";
#endif

        //広告の初期化
        Advertisement.Initialize(gameID,false);

        //広告関連のイベントが発生するように登録
        Advertisement.AddListener(this);
    }

    /// <summary>
    /// 動画広告の表示
    /// </summary>
    public void ShowMovieAd()
    {
        //広告全体の準備が出来ているかチェック
        if (!Advertisement.IsReady())
        {
            Debug.LogWarning("動画広告の準備が出来ていません");
            return;
        }

        //表示したい広告の準備が出来ているかチェック
        var state = Advertisement.GetPlacementState(VIDEO_PLACEMENT_ID);
        if (state != PlacementState.Ready)
        {
            Debug.LogWarning($"{VIDEO_PLACEMENT_ID}の準備が出来ていません。現在の状態 : {state}");
            return; ;
        }

        //広告表示
        Advertisement.Show(VIDEO_PLACEMENT_ID);
    }

    //=================================================================================
    //イベント
    //=================================================================================

    //広告の準備完了
    public void OnUnityAdsReady(string placementId)
    {
        Debug.Log($"{placementId}の準備が完了");
    }

    //広告でエラーが発生
    public void OnUnityAdsDidError(string message)
    {
        Debug.Log($"広告でエラー発生 : {message}");
    }

    //広告開始
    public void OnUnityAdsDidStart(string placementId)
    {
        Debug.Log($"{placementId}の広告が開始");

        AllAudioManege.PauseBGM();
    }

    //広告の表示終了
    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        Debug.Log($"{placementId}の表示終了");
        switch (showResult)
        {
            //最後まで視聴完了(リワード広告ならここでリワード付与する感じ)
            case ShowResult.Finished:
                Debug.Log("広告の表示成功");

                AllAudioManege.UnPauseBGM();

                //宿屋での広告だった場合
                if (AdsType==1)
                {
                    AllDataManege.StetasWariaiKaihuku(0.1f, 3);

                    TownManegerScript.TownMessageWindow.MessageText[0] = "ぐっすりでしたよ\n（いつもより回復量が多くなっている……？）";
                }

                //冒険が終わった時の旅の経験倍増時
                if (AdsType==2)
                {
                    AllDataManege.SomethingData.NowhaveTabikeiken += AllDataManege.SomeIntData.GetGetTabikeiken(AllDataManege.CharactorData.KihonParameter[0] - 1);
                    AllDataManege.SomethingData.NowhaveTabikeiken = Mathf.Clamp(AllDataManege.SomethingData.NowhaveTabikeiken, 0, 9999);

                    AllDataManege.DataSave();

                    Destroy(GameObject.Find("EndKoukoku(Clone)"));
                    Destroy(GameObject.Find("KoukokuButton"));

                    BoukenEndScript.NibaiFlag = true;
                    BoukenEndScript.CanSinkouFlag = true;
                    BoukenEndScript.CanMesFlag = true;
                }

                //冒険が終わった時の旅の経験入手可能にするとき
                if (AdsType==3)
                {
                    AllDataManege.SomethingData.NowhaveTabikeiken+= AllDataManege.SomeIntData.GetGetTabikeiken(AllDataManege.CharactorData.KihonParameter[0] - 1);
                    AllDataManege.SomethingData.NowhaveTabikeiken = Mathf.Clamp(AllDataManege.SomethingData.NowhaveTabikeiken, 0, 9999);

                    AllDataManege.DataSave();

                    Destroy(GameObject.Find("EndKoukoku(Clone)"));
                    Destroy(GameObject.Find("KoukokuButton"));

                    BoukenEndScript.CanSinkouFlag = true;
                    BoukenEndScript.CanMesFlag = true;

                    AllDataManege.CanGetKeiken = true;
                }

                Time.timeScale = 1;

                break;
            //スキップされた
            case ShowResult.Skipped:
                Debug.Log("広告スキップ");

                AllAudioManege.UnPauseBGM();

                //宿屋での広告だった場合
                if (AdsType == 1)
                { 
                    TownManegerScript.TownMessageWindow.MessageText[0] = "ぐっすりでしたよ";
                }

                //冒険が終わった時の旅の経験倍増時
                if (AdsType == 2||AdsType==3)
                {
                    Destroy(GameObject.Find("EndKoukoku(Clone)"));

                    BoukenEndScript.CanSinkouFlag = true;
                    BoukenEndScript.CanMesFlag = true;
                }

                Time.timeScale = 1;
                break;
            //表示自体が失敗した
            case ShowResult.Failed:
                Debug.LogWarning("広告の表示失敗");

                AllAudioManege.UnPauseBGM();

                //宿屋での広告だった場合
                if (AdsType == 1)
                {
                    TownManegerScript.TownMessageWindow.MessageText[0] = "ぐっすりでしたよ";
                }

                //冒険が終わった時の旅の経験倍増時
                if (AdsType == 2||AdsType==3)
                {
                    Destroy(GameObject.Find("EndKoukoku(Clone)"));

                    BoukenEndScript.CanSinkouFlag = true;
                    BoukenEndScript.CanMesFlag = true;
                }

                Time.timeScale = 1;
                break;
        }
    }

}
