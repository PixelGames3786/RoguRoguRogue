using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DungeonIdouParent : MonoBehaviour
{
    private GameObject MiniSceneChange,SceneChange;
    public GameObject MiniChangePrefab, MesWindowPrefab,SceneChangePrefab;

    private Vector2 MotoPosition, NowPosition;

    private RectTransform MapRect;

    private Text MapNameText;

    private int SentakuInt,NowTextInt;

    private string MapName;

    private bool MapDownFlag, MapIdouFlag;
    private bool TextHyoujiFlag;

    private float KeikaTime;

    // Start is called before the first frame update
    void Start()
    {
        MapRect = gameObject.transform.GetChild(0).GetComponent<RectTransform>();

        MapNameText = gameObject.transform.GetChild(3).transform.GetChild(0).GetComponent<Text>();

        SentakuInt = 1;
    }

    // Update is called once per frame
    void Update()
    {
        //ミニシーンチェンジ　左から
        if (MiniSceneChange && MiniSceneChange.GetComponent<RectTransform>().localPosition.x >= 0)
        {
            TownManegerScript.MessageWindow = Instantiate(MesWindowPrefab, gameObject.transform.parent.transform);

            TownManegerScript.TownMessageWindow = TownManegerScript.MessageWindow.GetComponent<TownMessageWindow>();
            TownManegerScript.TownMessageWindow.MesType = 1;

            TownManegerScript.MessageWindow.GetComponent<RectTransform>().localPosition = new Vector3(0, -660, 0);

            Destroy(gameObject);
        }

        //マップが押されているとき
        if (MapDownFlag)
        {
            NowPosition = Input.mousePosition;

            if (NowPosition != MotoPosition)
            {
                MapIdouFlag = true;
            }
        }

        //移動しているとき
        if (MapIdouFlag)
        {
            NowPosition = Input.mousePosition;

            MapRect.localPosition = new Vector2(MapRect.localPosition.x - ((MotoPosition.x - NowPosition.x) * 1.3f), MapRect.localPosition.y - ((MotoPosition.y - NowPosition.y) * 1.3f));
            MapRect.localPosition = new Vector2(Mathf.Clamp(MapRect.localPosition.x, -804, 804), 0);

            MotoPosition = Input.mousePosition;
        }

        //マップの名前表示
        if (TextHyoujiFlag)
        {
            if (KeikaTime>=AllDataManege.MesTime)
            {
                KeikaTime = 0;

                MapNameText.text += MapName[NowTextInt];

                if (NowTextInt >= MapName.Length - 1)
                {
                    TextHyoujiFlag = false;

                    NowTextInt = 0;

                    return;
                }

                NowTextInt++;
            }

            KeikaTime += Time.deltaTime;
        }

        //シーンチェンジ実行
        if (SceneChange && SceneChange.transform.GetChild(0).GetComponent<Rigidbody2D>().IsSleeping())
        {

            AllDataManege.DangeonData = AllDataManege.DangeonDataList[SentakuInt - 2];
            AllDataManege.TokusyuJoukyou = 1;
            AllDataManege.SentakuDangeon = SentakuInt - 2;
            AllDataManege.DangeonKaisou = 0;
            AllDataManege.DangeonSinkou = 0;

            AllDataManege.DataSave();

            AllAudioManege.MajiStopBGM();

            SceneManager.LoadScene("Dangeon");
            
        }
    }

    public void MapDown()
    {
        MapDownFlag = true;

        MotoPosition = Input.mousePosition;
    }

    public void MapUp()
    {
        MapDownFlag = false;
        MapIdouFlag = false;
    }

    public void ButtonDown(int value)
    {
        MapDownFlag = false;

        if (MapIdouFlag||SceneChange)
        {
            MapIdouFlag = false;

            return;
        }

        switch (value)
        {
            //戻るボタンが押されたとき
            case 0:

                MiniSceneChange = Instantiate(MiniChangePrefab, GameObject.Find("SceneChangeCanvas").transform);

                MiniSceneChange.GetComponent<MiniSceneChangeScript>().ChangeEx = 9;
                MiniSceneChange.GetComponent<MiniSceneChangeScript>().ChangeType = 1;

                MiniSceneChange.GetComponent<RectTransform>().localPosition = new Vector3(-2160, 0, 0);

                AllAudioManege.PlaySE(6);

                break;

            //向かうボタンが押されたとき
            case 1:

                //村が選択されているとき
                if (SentakuInt == 1)
                {
                    MiniSceneChange = Instantiate(MiniChangePrefab, GameObject.Find("SceneChangeCanvas").transform);

                    MiniSceneChange.GetComponent<MiniSceneChangeScript>().ChangeEx = 9;
                    MiniSceneChange.GetComponent<MiniSceneChangeScript>().ChangeType = 1;

                    MiniSceneChange.GetComponent<RectTransform>().localPosition = new Vector3(-2160, 0, 0);
                }

                //洞窟以上が選択されているとき
                if (SentakuInt >=2)
                {
                    SceneChange = Instantiate(SceneChangePrefab, GameObject.Find("SceneChangeCanvas").transform);
                }

                AllAudioManege.PlaySE(0);

                break;

            //ここから各ダンジョンが押されたときの反応

            //村ボタンが押されたとき
            case 2:

                if (SentakuInt == value-1)
                {
                    return;
                }

                SentakuInt = 1;

                MapNameText.text = "";
                MapName = "村";

                TextHyoujiFlag = true;

                break;

            //洞窟ボタンが押されたとき
            case 3:

                if (SentakuInt == value - 1)
                {
                    return;
                }

                SentakuInt = 2;

                MapNameText.text = "";
                MapName = "洞窟";

                TextHyoujiFlag = true;

                break;

            //森ボタンが押されたとき
            case 4:

                if (SentakuInt == value - 1)
                {
                    return;
                }

                SentakuInt = 3;

                MapNameText.text = "";
                MapName = "森";

                TextHyoujiFlag = true;

                break;

            //火山ボタンが押されたとき
            case 5:

                if (SentakuInt == value - 1)
                {
                    return;
                }

                SentakuInt = 4;

                MapNameText.text = "";
                MapName = "火山";

                TextHyoujiFlag = true;

                break;

            //湖ボタンが押されたとき
            case 6:

                if (SentakuInt == value - 1)
                {
                    return;
                }

                SentakuInt = 5;

                MapNameText.text = "";
                MapName = "湖";

                TextHyoujiFlag = true;

                break;

            //洋館ボタンが押されたとき
            case 7:

                if (SentakuInt == value - 1)
                {
                    return;
                }

                SentakuInt = 6;

                MapNameText.text = "";
                MapName = "洋館";

                TextHyoujiFlag = true;

                break;

            //神殿ボタンが押されたとき
            case 8:

                if (SentakuInt == value - 1)
                {
                    return;
                }

                SentakuInt = 7;

                MapNameText.text = "";
                MapName = "神殿";

                TextHyoujiFlag = true;

                break;

            //祠ボタンが押されたとき
            case 9:

                if (SentakuInt == value - 1)
                {
                    return;
                }

                SentakuInt = 8;

                MapNameText.text = "";
                MapName = "祠";

                TextHyoujiFlag = true;

                break;
        }
    }
}
