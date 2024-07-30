using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatiCommandScript : MonoBehaviour
{
    private List<AnimationClip> AnimationList = new List<AnimationClip>();

    private GameObject MiniSceneChange,TatieCanvas,HaikeiCanvas;
    public GameObject MessageWindow,CommandPrefab,MiniChangePrefab,KakuninWindowPrefab;

    private GameObject[] GameObjects = new GameObject[5];
    public GameObject[] Prefabs;

    public Sprite[] TatieSprites,HaikeiSprites;
    
    private Text[] ChildTextMesh = new Text[8];

    private TownMessageWindow TownMessageWindow;

    private int SentakuInt;
    private bool InFlag,OutFlag;

    // Start is called before the first frame update
    void Start()
    {
        InFlag = true;

        TatieCanvas = GameObject.Find("TatieCanvas");
        HaikeiCanvas = GameObject.Find("HaikeiCanvas");;

        TownMessageWindow = GameObject.Find("MessageWindow(Clone)").GetComponent<TownMessageWindow>();

        foreach (AnimationState anim in this.GetComponent<Animation>())
        {
            this.AnimationList.Add(anim.clip);
        }

        for (int i=0;i<8;i++)
        {
            ChildTextMesh[i] = gameObject.transform.GetChild(i).transform.GetChild(0).GetComponent<Text>();

            if (AllDataManege.TownData.GetCommandKaihou()[i]==0)
            {
                ChildTextMesh[i].text = "？？？";
            }
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        //街のコマンド入るアニメーション終わった時
        if (InFlag&&!this.gameObject.GetComponent<Animation>().isPlaying)
        {
            InFlag = false;

            TownMessageWindow.ThisTextMesh.text = "";
            TownMessageWindow.MessageText = new string[1];
            TownMessageWindow.MessageText[0] = "さて、どうしようか？";
            TownMessageWindow.MesType = 1;

            TownManegerScript.MessageWindowReset();
        }

        //コマンドボタンが引っ込んだ後
        if (OutFlag && !this.gameObject.GetComponent<Animation>().isPlaying)
        {

            TownMessageWindow.ThisTextMesh.text = "";
            TownMessageWindow.MessageText = new string[1];
            TownMessageWindow.MessageText = TownMessageWindow.TextAsset.text.Split('\n');
            TownMessageWindow.MesType = 1;
            TownMessageWindow.NowMojiNum = 0;
            TownMessageWindow.NowMojiGyou = 0;
            TownMessageWindow.CanMesFlag = true;
            TownMessageWindow.MesEndFlag = false;

            TownManegerScript.CanCommandFlag = true;

            Instantiate(CommandPrefab, this.gameObject.transform.parent);

            Destroy(this.gameObject);
        }

        //ミニシーンチェンジで画面全体が隠れた時（右から来た時）
        if (MiniSceneChange&&MiniSceneChange.GetComponent<RectTransform>().localPosition.x <= 0)
        {
            switch (SentakuInt)
            {
                //村長の家に行くとき
                case 1:

                    {
                        //立ち絵オブジェクトを作る
                        GameObjects[0] = Instantiate(Prefabs[0], TatieCanvas.transform);

                        //個別背景オブジェクトを作る
                        GameObjects[1] = Instantiate(Prefabs[1], HaikeiCanvas.transform);

                        TownMessageWindow.ThisTextMesh.text = "";

                        Destroy(this.gameObject);
                    }

                    break;

                //宿屋に行くとき
                case 2:

                    {
                        //立ち絵オブジェクトを作る
                        GameObjects[0] = Instantiate(Prefabs[0], TatieCanvas.transform);
                        GameObjects[0].GetComponent<Image>().sprite = TatieSprites[0];

                        //個別背景オブジェクトを作る
                        GameObjects[1] = Instantiate(Prefabs[1], HaikeiCanvas.transform);
                        GameObjects[1].GetComponent<Image>().sprite = HaikeiSprites[0];

                        TownMessageWindow.ThisTextMesh.text = "";

                        Destroy(this.gameObject);
                    }

                    break;

                //路地裏に行くとき
                case 3:

                    {
                        //個別背景オブジェクトを作る
                        GameObjects[1] = Instantiate(Prefabs[1], HaikeiCanvas.transform);
                        GameObjects[1].GetComponent<Image>().sprite = HaikeiSprites[1];

                        TownMessageWindow.ThisTextMesh.text = "";

                        Destroy(this.gameObject);
                    }

                    break;
            }

        }

        //ミニシーンチェンジで画面全体が隠れた時（左から来た時）
        if (MiniSceneChange && MiniSceneChange.GetComponent<RectTransform>().localPosition.x >=0)
        {
            switch (SentakuInt)
            {
                //道具屋に行くとき
                case 5:

                    {
                        //立ち絵オブジェクトを作る
                        GameObjects[0] = Instantiate(Prefabs[0], TatieCanvas.transform);
                        GameObjects[0].GetComponent<Image>().sprite = TatieSprites[1];

                        //個別背景オブジェクトを作る
                        GameObjects[1] = Instantiate(Prefabs[1], HaikeiCanvas.transform);
                        GameObjects[1].GetComponent<Image>().sprite = HaikeiSprites[2];

                        TownMessageWindow.ThisTextMesh.text = "";

                        Destroy(this.gameObject);
                    }

                    break;

                //武器屋に行くとき
                case 6:

                    {
                        //立ち絵オブジェクトを作る
                        GameObjects[0] = Instantiate(Prefabs[0], TatieCanvas.transform);
                        GameObjects[0].GetComponent<Image>().sprite = TatieSprites[2];

                        //個別背景オブジェクトを作る
                        GameObjects[1] = Instantiate(Prefabs[1], HaikeiCanvas.transform);
                        GameObjects[1].GetComponent<Image>().sprite = HaikeiSprites[3];

                        TownMessageWindow.ThisTextMesh.text = "";

                        Destroy(this.gameObject);
                    }

                    break;

                //訓練場に行くとき
                case 7:

                    {
                        //立ち絵オブジェクトを作る
                        GameObjects[0] = Instantiate(Prefabs[0], TatieCanvas.transform);
                        GameObjects[0].GetComponent<Image>().sprite = TatieSprites[3];

                        //個別背景オブジェクトを作る
                        GameObjects[1] = Instantiate(Prefabs[1], HaikeiCanvas.transform);
                        GameObjects[1].GetComponent<Image>().sprite = HaikeiSprites[4];

                        TownMessageWindow.ThisTextMesh.text = "";

                        Destroy(this.gameObject);
                    }

                    break;
            }

        }

    }

    //押されたとき
    public void ButtonMouseDown(int value)
    {
        //もし注意ウィンドウかアニメーションがまだやってたら判定しない
        if (TownManegerScript.KakuninWindow|| (InFlag && this.gameObject.GetComponent<Animation>().isPlaying)||MiniSceneChange)
        {
            return;
        }

        //もしそのコマンドが開放されてなかったら
        if (AllDataManege.TownData.GetCommandKaihou()[value-1]==0)
        {
            TownManegerScript.KakuninWindow = Instantiate(KakuninWindowPrefab,this.transform.parent);

            return;
        }

        //行く場所によって処理を変える
        switch (value)
        {
            //村長の家に行く
            case 1:

                {
                    AllAudioManege.PlaySE(7);

                    SentakuInt = 1;

                    MiniSceneChange = Instantiate(MiniChangePrefab, GameObject.Find("SceneChangeCanvas").transform);
                    MiniSceneChange.GetComponent<MiniSceneChangeScript>().ChangeEx = 1;

                    TownMessageWindow.CanMesFlag = false;
                    
                }

                break;

            //宿屋へ行く
            case 2:

                {
                    AllAudioManege.PlaySE(7);

                    SentakuInt = 2;

                    MiniSceneChange = Instantiate(MiniChangePrefab, GameObject.Find("SceneChangeCanvas").transform);
                    MiniSceneChange.GetComponent<MiniSceneChangeScript>().ChangeEx = 3;

                    TownMessageWindow.CanMesFlag = false;
                    
                }

                break;

            //路地裏へ行く
            case 3:

                {
                    AllAudioManege.PlaySE(7);

                    SentakuInt = 3;

                    MiniSceneChange = Instantiate(MiniChangePrefab, GameObject.Find("SceneChangeCanvas").transform);
                    MiniSceneChange.GetComponent<MiniSceneChangeScript>().ChangeEx = 4;

                    TownMessageWindow.CanMesFlag = false;
                    
                }

                break;

            //戻る
            case 4:

                {
                    if (!OutFlag)
                    {
                        AllAudioManege.PlaySE(6);
                    }

                    OutFlag = true;

                    this.gameObject.GetComponent<Animation>().Play(AnimationList[1].name);
                }

                break;

            //道具屋ヘ行く
            case 5:

                {
                    AllAudioManege.PlaySE(7);

                    SentakuInt = 5;

                    MiniSceneChange = Instantiate(MiniChangePrefab, GameObject.Find("SceneChangeCanvas").transform);
                    MiniSceneChange.GetComponent<MiniSceneChangeScript>().ChangeEx = 5;
                    MiniSceneChange.GetComponent<MiniSceneChangeScript>().ChangeType = 1;
                    MiniSceneChange.GetComponent<RectTransform>().localPosition = new Vector3(-2160, 0, 0);

                    TownMessageWindow.CanMesFlag = false;
                    
                }

                break;

            //武器屋へ行く
            case 6:

                {
                    AllAudioManege.PlaySE(7);

                    SentakuInt = 6;

                    MiniSceneChange = Instantiate(MiniChangePrefab, GameObject.Find("SceneChangeCanvas").transform);
                    MiniSceneChange.GetComponent<MiniSceneChangeScript>().ChangeEx = 6;
                    MiniSceneChange.GetComponent<MiniSceneChangeScript>().ChangeType = 1;
                    MiniSceneChange.GetComponent<RectTransform>().localPosition = new Vector3(-2160, 0, 0);

                    TownMessageWindow.CanMesFlag = false;
                }

                break;

            //訓練場へ行く
            case 7:

                {
                    AllAudioManege.PlaySE(7);

                    SentakuInt = 7;

                    MiniSceneChange = Instantiate(MiniChangePrefab, GameObject.Find("SceneChangeCanvas").transform);
                    MiniSceneChange.GetComponent<MiniSceneChangeScript>().ChangeEx = 7;
                    MiniSceneChange.GetComponent<MiniSceneChangeScript>().ChangeType = 1;
                    MiniSceneChange.GetComponent<RectTransform>().localPosition = new Vector3(-2160, 0, 0);

                    TownMessageWindow.CanMesFlag = false;
                    
                }

                break;

        }
    }
}
