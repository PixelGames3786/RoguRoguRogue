using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class MiniSceneChangeScript : MonoBehaviour
{
    private TownMessageWindow TownMessageWindow;
    private RectTransform ThisRect;

    [System.NonSerialized]
    public GameObject CommandButton,SyojikinWindow;

    public GameObject CommandPrefab,CommandPrefab2,SyojikinPrefab, MesWindowPrefab;

    private GameObject UICanvas;

    [System.NonSerialized]
    public int ChangeEx;

    [System.NonSerialized]
    public int ChangeType;
    //0 右から 1 左から

    public float Speed;

    // Start is called before the first frame update
    void Start()
    {
        ThisRect = this.gameObject.GetComponent<RectTransform>();

        UICanvas = GameObject.Find("UICanvas");
    }

    // Update is called once per frame
    void Update()
    {
        //右からくる
        if (ChangeType==0)
        {
            ThisRect.localPosition = new Vector3(ThisRect.localPosition.x-(Speed*Time.deltaTime),0,0);

            //移動が終わった時
            if (ThisRect.localPosition.x<=-2160)
            {
                ExChangeSyori();
                Destroy(gameObject);
            }
        }

        //左からくる
        if (ChangeType==1)
        {
            ThisRect.localPosition = new Vector3(ThisRect.localPosition.x+(Speed*Time.deltaTime),0,0);

            //移動が終わった時
            if (ThisRect.localPosition.x>=2160)
            {
                ExChangeSyori();
                Destroy(gameObject);
            }
        }
    }

    private void ExChangeSyori()
    {
        if (ChangeEx==0)
        {
            return;
        }

        //村長の家
        if (ChangeEx == 1)
        {
            TownMessageWindow = TownManegerScript.MessageWindow.GetComponent<TownMessageWindow>();

            //メッセージウィンドウのリセット
            TownMessageWindow.MessageText = new string[1];
            TownMessageWindow.MessageText[0]=TownManegerScript.MurabitoText[UnityEngine.Random.Range(0,3)];

            TownMessageWindow.MesType = 2;
            TownManegerScript.MessageWindowReset();

            TownMessageWindow.ThisTextMesh.text = "";

            //コマンドボタンを作る
            CommandButton =Instantiate(CommandPrefab,GameObject.Find("UICanvas").transform);
            CommandButton.GetComponent<TownCommandScript>().CommandType=1;

            TownManegerScript.CanCommandFlag = true;
        }

        //村長の家から戻ってきたとき
        if (ChangeEx == 2)
        {
            TownMessageWindow = TownManegerScript.MessageWindow.GetComponent<TownMessageWindow>();

            CommandButton = Instantiate(CommandPrefab2, GameObject.Find("UICanvas").transform);

            TownMessageWindow.ThisTextMesh.text = "";
            TownMessageWindow.CanMesFlag = false;

            TownManegerScript.CanCommandFlag = true;
        }

        //宿屋
        if (ChangeEx == 3)
        {
            TownMessageWindow = TownManegerScript.MessageWindow.GetComponent<TownMessageWindow>();

            
            TownMessageWindow.MessageText = new string[1];
            TownMessageWindow.MessageText[0] = TownManegerScript.TownText[UnityEngine.Random.Range(AllDataManege.SomeIntData.GetTownTextMin(AllDataManege.TownData.HattenLevel) + 6, AllDataManege.SomeIntData.GetTownTextMax(AllDataManege.TownData.HattenLevel) + 6)];

            TownMessageWindow.MesType = 3;
            TownManegerScript.MessageWindowReset();

            TownMessageWindow.ThisTextMesh.text = "";

            CommandButton = Instantiate(CommandPrefab, UICanvas.transform);
            CommandButton.GetComponent<TownCommandScript>().CommandType = 2;

            SyojikinWindow = Instantiate(SyojikinPrefab,UICanvas.transform);

            TownManegerScript.CanCommandFlag = true;
        }

        //路地裏
        if (ChangeEx==4)
        {
            TownMessageWindow = TownManegerScript.MessageWindow.GetComponent<TownMessageWindow>();

            TownMessageWindow.MessageText = new string[1];
            TownMessageWindow.MessageText[0] = TownManegerScript.TownText[UnityEngine.Random.Range(AllDataManege.SomeIntData.GetTownTextMin(AllDataManege.TownData.HattenLevel) + 9, AllDataManege.SomeIntData.GetTownTextMax(AllDataManege.TownData.HattenLevel) + 9)];

            TownMessageWindow.MesType = 4;
            TownManegerScript.MessageWindowReset();

            TownMessageWindow.ThisTextMesh.text = "";

            CommandButton = Instantiate(CommandPrefab, GameObject.Find("UICanvas").transform);
            CommandButton.GetComponent<TownCommandScript>().CommandType = 3;

            TownManegerScript.CanCommandFlag = true;
        }

        //道具屋
        if (ChangeEx==5)
        {
            TownMessageWindow = TownManegerScript.MessageWindow.GetComponent<TownMessageWindow>();

            TownMessageWindow.MessageText = new string[1];
            TownMessageWindow.MessageText[0] = TownManegerScript.TownText[UnityEngine.Random.Range(AllDataManege.SomeIntData.GetTownTextMin(AllDataManege.TownData.HattenLevel) + 12, AllDataManege.SomeIntData.GetTownTextMax(AllDataManege.TownData.HattenLevel) + 12)];

            TownMessageWindow.MesType = 5;
            TownManegerScript.MessageWindowReset();

            TownMessageWindow.ThisTextMesh.text = "";

            CommandButton = Instantiate(CommandPrefab, UICanvas.transform);
            CommandButton.GetComponent<TownCommandScript>().CommandType = 4;

            SyojikinWindow = Instantiate(SyojikinPrefab, UICanvas.transform);

            TownManegerScript.CanCommandFlag = true;
        }

        //武器屋
        if (ChangeEx == 6)
        {
            TownMessageWindow = TownManegerScript.MessageWindow.GetComponent<TownMessageWindow>();

            TownMessageWindow.MessageText = new string[1];
            TownMessageWindow.MessageText[0] = TownManegerScript.TownText[UnityEngine.Random.Range(AllDataManege.SomeIntData.GetTownTextMin(AllDataManege.TownData.HattenLevel) + 15, AllDataManege.SomeIntData.GetTownTextMax(AllDataManege.TownData.HattenLevel) + 15)];

            TownMessageWindow.MesType = 6;
            TownManegerScript.MessageWindowReset();

            TownMessageWindow.ThisTextMesh.text = "";

            CommandButton = Instantiate(CommandPrefab, UICanvas.transform);
            CommandButton.GetComponent<TownCommandScript>().CommandType = 5;

            SyojikinWindow = Instantiate(SyojikinPrefab, UICanvas.transform);

            TownManegerScript.CanCommandFlag = true;
        }

        //訓練場
        if (ChangeEx == 7)
        {
            TownMessageWindow = TownManegerScript.MessageWindow.GetComponent<TownMessageWindow>();

            TownMessageWindow.MessageText = new string[1];
            TownMessageWindow.MessageText[0] = TownManegerScript.TownText[UnityEngine.Random.Range(AllDataManege.SomeIntData.GetTownTextMin(AllDataManege.TownData.HattenLevel) + 18, AllDataManege.SomeIntData.GetTownTextMax(AllDataManege.TownData.HattenLevel) + 18)];

            TownMessageWindow.MesType = 7;
            TownManegerScript.MessageWindowReset();

            TownMessageWindow.ThisTextMesh.text = "";

            CommandButton = Instantiate(CommandPrefab, UICanvas.transform);
            CommandButton.GetComponent<TownCommandScript>().CommandType = 6;

            SyojikinWindow = Instantiate(SyojikinPrefab, UICanvas.transform);

            TownManegerScript.CanCommandFlag = true;
        }

        //ダンジョンマップへ行くとき
        if (ChangeEx==8)
        {

        }

        //ダンジョンマップから戻ってきたとき
        if (ChangeEx==9)
        {
            CommandButton = Instantiate(CommandPrefab, UICanvas.transform);
            CommandButton.GetComponent<TownCommandScript>().CommandType = 0;

            TownManegerScript.CanCommandFlag = true;
        }
    }
}
