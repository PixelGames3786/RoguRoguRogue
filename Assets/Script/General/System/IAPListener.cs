using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class IAPListener : MonoBehaviour, IStoreListener
{

    //購入実行に使用
    IStoreController Controller { get; set; }
    //リストア処理など拡張機能で使用
    IExtensionProvider Extensions { get; set; }

    public int LoadErorrType,KonyuType;

    public GameObject CoverPrefab;
    private GameObject KakinCover;

    void Start()
    {
        Initialize();

        DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    void Initialize()
    {

        Debug.Log("初期化したぜ");

        var googlePlayPublicKey = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA9ig9vPVt6weVkzHmowEtdKKsZaoRO1M1mBI19c5tNG7f72RNaBe8fNvMo2deVmQVgESltHgdq9rOUMFN7yRW8dtrTeYuZiNOAIAh5TOd7TSa96rHOvF3pfuc8L+O+WjRDkKij3eOGZzMKYfUxwR/b9SPZqR3mciv3aEp3qfQepSUtAwcQy6pR1xX7BNoxxL1iV0PPSE439IaOQP2FpkBRRqEv3Ju055aeCQN2HvDtCKuJgj209DvaFYNq2Li7MCKyyq+ekmr53Gsj9grSO6DLmKboBOffl5dSWrbUekrBCDXAJb8RRqHxEghlbHbhSO0VA0Zsw49K1aSX7V7TEe/2wIDAQAB";
        //builderを作成しプロダクトを登録する。
        StandardPurchasingModule module = StandardPurchasingModule.Instance();
        //Uエディタ検証用の表示を作ってくれる
        module.useFakeStoreUIMode = FakeStoreUIMode.StandardUser;
        ConfigurationBuilder builder = ConfigurationBuilder.Instance(module);

        if (!String.IsNullOrEmpty(googlePlayPublicKey))
        {
            builder.Configure<IGooglePlayConfiguration>().SetPublicKey(googlePlayPublicKey);
        }

        //②プロダクト(アイテム)の登録第
        builder.AddProduct("tabinokeiken15", ProductType.Consumable);

        builder.AddProduct("tabinokeiken25", ProductType.Consumable);
        
        builder.AddProduct("tabinokeiken40", ProductType.Consumable);

        builder.AddProduct("tabinokeiken60", ProductType.Consumable);

        //②以下のようにAndroid、iOSで別のproduct_idでもアプリ上で一つのプロダクトとして登録することもできる。
        /* 
         * builder.AddProduct("jp.hogehoge.1yen", ProductType.Consumable, new IDs
                     {
                     { "jp.hogehoge.android.1yen", GooglePlay.Name },
                     { "jp.hogehoge.ios.1yen", AppleAppStore.Name }

         });
         */


        //IAP初期化
        UnityPurchasing.Initialize(this, builder);

    }


    //初期化成功イベント
    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        //成功すると購入やリストアに使用するインターフェースが返ってくるので保持しよう

        Controller = controller;
        Extensions = extensions;
    }

    //初期化失敗イベント
    public void OnInitializeFailed(InitializationFailureReason error)
    {
        //初期化失敗の場合errorには以下の場合がある。
        //エラーによって警告表示やストア表示を変えるなどの分岐、ログ取得などを記述するといい

        //ストアがアプリケーションを認識していない場合
        //InitializationFailureReason.AppNotKnown

        //有効なプロダクトがない場合
        //InitializationFailureReason.NoProductsAvailable

        //端末の購入が許可になっていない場合
        // InitializationFailureReason.PurchasingUnavailable

        if (error == InitializationFailureReason.AppNotKnown)
        {
            LoadErorrType = 1;
        }
        else if (error == InitializationFailureReason.NoProductsAvailable)
        {
            LoadErorrType = 1;
        }
        else if (error == InitializationFailureReason.PurchasingUnavailable)
        {
            LoadErorrType = 1;
        }

    }

    //アプリ内での購入ボタンを押したときなどの処理
    public void Purchasing(int value)
    {
        Time.timeScale = 0;

        //もし初期化に失敗していたら
        if (LoadErorrType!=0)
        {
            KakinCover = Instantiate(CoverPrefab,GameObject.Find("SceneChangeCanvas").transform);

            KakinCover.GetComponent<KakinCoverScript>().TextFlag = false;

            KakinCover.GetComponent<KakinCoverScript>().LoadingText.text = "初期化に失敗しました\n後にお試しください";

            Initialize();

            return;
        }
        else
        {
            KakinCover = Instantiate(CoverPrefab, GameObject.Find("SceneChangeCanvas").transform);

            Initialize();
        }

        KonyuType = value;

        //登録したプロダクトで購入をかける
        if (value==0)
        {
            var product = Controller.products.WithID("tabinokeiken15");
            Controller.InitiatePurchase(product);
        }

        if (value==1)
        {
            var product = Controller.products.WithID("tabinokeiken25");
            Controller.InitiatePurchase(product);
        }

        if (value == 2)
        {
            var product = Controller.products.WithID("tabinokeiken40");
            Controller.InitiatePurchase(product);
        }

        if (value == 3)
        {
            var product = Controller.products.WithID("tabinokeiken60");
            Controller.InitiatePurchase(product);
        }

        //上記はproductを取得し、購入をかけているが実際にはしたのようにproduct}_idの指定だけでも購入はかけられる。
        // Controller.InitiatePurchase("jp.hogehoge.1yen");

    }

    //購入成功イベント
    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e)
    {
        //Productを取得する。
        //レシート情報などが格納されている
        Product product = e.purchasedProduct;
        string receipt = e.purchasedProduct.receipt;
        //通貨コード(Currencyなどが取得可能)
        ProductMetadata metaData = product.metadata;
        //↓通貨コード。サーバで購入時通貨など収集するときは送ってあげたりする
        //metaData.isoCurrencyCode
        //isoCurrencyCodeでの値段
        //metaData.localizedPrice

        //プロダクトの詳細(UnityIAPのID、ストアにおけるID、プロダクトのタイプのみ)
        ProductDefinition definition = product.definition;
        //↓こっちはUnityIAP上のID
        //definition.id   
        //↓こっちは各プラットフォームストアごとのプロダクトのID 
        //definition.storeSpecificId


        //ここらへんでアプリのバックエンドサーバにレシートを渡すなどして、レシート検証とアイテムの付与などを行わせる。

        if (KonyuType==0)
        {
            AllDataManege.SomethingData.SetTabikeiken(AllDataManege.SomethingData.GetTabiKeiken()+15);
        }

        if (KonyuType==1)
        {
            AllDataManege.SomethingData.SetTabikeiken(AllDataManege.SomethingData.GetTabiKeiken() + 25);
        }

        if (KonyuType == 2)
        {
            AllDataManege.SomethingData.SetTabikeiken(AllDataManege.SomethingData.GetTabiKeiken() + 40);
        }

        if (KonyuType == 3)
        {
            AllDataManege.SomethingData.SetTabikeiken(AllDataManege.SomethingData.GetTabiKeiken() + 60);
        }

        AllDataManege.SomethingData.NowhaveTabikeiken = Mathf.Clamp(AllDataManege.SomethingData.NowhaveTabikeiken,0,9999);

        KakinCover.GetComponent<KakinCoverScript>().TextFlag = false;

        KakinCover.GetComponent<KakinCoverScript>().LoadingText.text = "購入に成功しました";

        AllDataManege.NowKakinSentaku = false;

        AllDataManege.DataSave();

        Time.timeScale = 1;

        //PurchaseProcessingResult.Pending とした場合には購入完了扱いにはならない(サーバでのレシート検証を待ってから終了とする場合など)
        //ConfirmPendingPurchase(明示的に消費処理)がかけられるまでProcessPurchaseが呼び出され続けるようになる
        //PurchaseProcessingResult.Completeはすぐに購入消費完了扱いとなりアイテムの消費などが行われる
        return PurchaseProcessingResult.Pending;
    }

    //購入失敗イベント
    public void OnPurchaseFailed(Product i, PurchaseFailureReason p)
    {
        KakinCover.GetComponent<KakinCoverScript>().TextFlag = false;

        KakinCover.GetComponent<KakinCoverScript>().LoadingText.text = "購入に失敗しました\n後にお試しください";

        AllDataManege.NowKakinSentaku = false;

        Time.timeScale = 1;

        //購入処理が失敗した場合に呼び出される
        //以下のようなパターンがある

        //購入機能自体が利用できなかったとき
        //PurchaseFailureReason.PurchasingUnavailable

        //購入処理中に再度購入をかけた場合
        //「無料で復元できます～」の場合もこれだといいなぁ
        //もしそうだった場合消費型アイテムでも事実上productからリストア処理かけられるかも ?(要検証)
        // PurchaseFailureReason.ExistingPurchasePending

        //購入をかけたプロダクトが無効だった場合など
        //PurchaseFailureReason.ProductUnavailable

        //購入情報のsignatureに問題があった場合
        //PurchaseFailureReason.SignatureInvalid

        //ユーザーが購入をキャンセルしたとき
        //PurchaseFailureReason.UserCancelled

        //支払い時に問題があった(決済の失敗など)
        //PurchaseFailureReason.PaymentDeclined)

        //他エラーに該当しない場合これが返る(たぶん)
        //PurchaseFailureReason.Unknown)

    }

}