using System;
using UnityEngine;
using GoogleMobileAds.Api;
using UnityEngine.UI;

public class AdmobListener : MonoBehaviour
{
    private RewardedAd rewardedAd;

    public int AdmobType;

    private bool IsRewarded;

    public void Start()
    {
        string adUnitId;
#if UNITY_ANDROID
        adUnitId = "ca-app-pub-4482801318215672/7740669352";
#elif UNITY_IPHONE
            adUnitId = "ca-app-pub-3940256099942544/1712485313";
#else
            adUnitId = "unexpected_platform";
#endif

        this.rewardedAd = new RewardedAd(adUnitId);

        // Called when an ad request has successfully loaded.
        this.rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
        // Called when an ad request failed to load.
        this.rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
        // Called when an ad is shown.
        this.rewardedAd.OnAdOpening += HandleRewardedAdOpening;
        // Called when an ad request failed to show.
        this.rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
        // Called when the user should be rewarded for interacting with the ad.
        this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        // Called when the ad is closed.
        this.rewardedAd.OnAdClosed += HandleRewardedAdClosed;

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the rewarded ad with the request.
        this.rewardedAd.LoadAd(request);
    }

    public void HandleRewardedAdLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardedAdLoaded event received");
    }

    public void HandleRewardedAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        MonoBehaviour.print(
            "HandleRewardedAdFailedToLoad event received with message: "
                             + args.LoadAdError);
    }

    public void HandleRewardedAdOpening(object sender, EventArgs args)
    {
        Time.timeScale = 0;

        AllAudioManege.PauseBGM();

        MonoBehaviour.print("HandleRewardedAdOpening event received");
    }

    public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
    {
        Time.timeScale = 1;

        AllAudioManege.UnPauseBGM();

        //宿屋での広告だった場合
        if (AdmobType == 1)
        {
            TownManegerScript.TownMessageWindow.MessageText[0] = "ぐっすりでしたよ";
        }

        //冒険が終わった時の旅の経験倍増時
        if (AdmobType == 2 || AdmobType == 3)
        {
            GameObject.Find("EndKoukoku(Clone)").GetComponent<EndKoukokuScript>().CantKoukoku = true;

            GameObject.Find("EndKoukoku(Clone)").transform.GetChild(0).GetComponent<Text>().text="広告の再生に失敗しました\n\n後ほどお試しください";

            Destroy(GameObject.Find("EndKoukoku(Clone)").transform.GetChild(1).gameObject);
            Destroy(GameObject.Find("EndKoukoku(Clone)").transform.GetChild(2).gameObject);

            
        }

        MonoBehaviour.print(
            "HandleRewardedAdFailedToShow event received with message: "
                             + args.Message);
    }

    public void CreateAndLoadRewardedAd()
    {
#if UNITY_ANDROID
        string adUnitId = "ca-app-pub-4482801318215672/7740669352";
#elif UNITY_IPHONE
            string adUnitId = "ca-app-pub-3940256099942544/1712485313";
#else
            string adUnitId = "unexpected_platform";
#endif

        this.rewardedAd = new RewardedAd(adUnitId);

        this.rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
        this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        this.rewardedAd.OnAdClosed += HandleRewardedAdClosed;

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the rewarded ad with the request.
        this.rewardedAd.LoadAd(request);

        IsRewarded = false;
    }

    public void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        Time.timeScale = 1;

        AllAudioManege.UnPauseBGM();

        if (!IsRewarded)
        {
            //宿屋での広告だった場合
            if (AdmobType == 1)
            {
                TownManegerScript.TownMessageWindow.MessageText[0] = "ぐっすりでしたよ";
            }

            //冒険が終わった時の旅の経験倍増時
            if (AdmobType == 2 || AdmobType == 3)
            {
                Destroy(GameObject.Find("EndKoukoku(Clone)"));

                BoukenEndScript.CanSinkouFlag = true;
                BoukenEndScript.CanMesFlag = true;
            }
        }

        CreateAndLoadRewardedAd();

        MonoBehaviour.print("HandleRewardedAdClosed event received");
    }

    public void HandleUserEarnedReward(object sender, Reward args)
    {
        string type = args.Type;
        double amount = args.Amount;
        MonoBehaviour.print(
            "HandleRewardedAdRewarded event received for "
                        + amount.ToString() + " " + type);

        IsRewarded = true;

        //宿屋での広告だった場合
        if (AdmobType == 1)
        {
            AllDataManege.StetasWariaiKaihuku(1f, 3);

            TownManegerScript.TownMessageWindow.MessageText[0] = "ぐっすりでしたよ\n（いつもより回復量が多くなっている……？）";
        }

        //冒険が終わった時の旅の経験倍増時
        if (AdmobType == 2)
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
        if (AdmobType == 3)
        {
            AllDataManege.SomethingData.NowhaveTabikeiken += AllDataManege.SomeIntData.GetGetTabikeiken(AllDataManege.CharactorData.KihonParameter[0] - 1);
            AllDataManege.SomethingData.NowhaveTabikeiken = Mathf.Clamp(AllDataManege.SomethingData.NowhaveTabikeiken, 0, 9999);

            AllDataManege.DataSave();

            Destroy(GameObject.Find("EndKoukoku(Clone)"));
            Destroy(GameObject.Find("KoukokuButton"));

            BoukenEndScript.CanSinkouFlag = true;
            BoukenEndScript.CanMesFlag = true;

            AllDataManege.CanGetKeiken = true;
        }
    }

    public void UserChoseToWatchAd()
    {
        if (this.rewardedAd.IsLoaded())
        {
            this.rewardedAd.Show();
        }
    }
}