using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DangeonJoutaiScript : MonoBehaviour
{
    public GameObject JoutaiButtonPrefab,SetumeiPrefab;
    private GameObject SetumeiWindow;

    private RectTransform ThisRect, ButtonParentRect;

    private Vector3 MotoPosi, NowPosi;

    private Image ButtonImage;

    private RaycastHit2D hit;

    //バフの数を入れる
    private int JoutaiCount;

    private bool InFlag, OutFlag, HoldFlag, IdouFlag;
    public bool CanButton;

    private float SaikouFloat;
    public float Speed, DrugSpeed;

    // Start is called before the first frame update
    void Start()
    {
        InFlag = true;

        ThisRect = gameObject.GetComponent<RectTransform>();
        ButtonParentRect = gameObject.transform.GetChild(0).transform.GetChild(0).GetComponent<RectTransform>();

        JoutaiCount = AllDataManege.CharactorData.NowJoutai.Count;

        //もしバフがあったらバフテキストを消してボタンを作る
        if (JoutaiCount > 0)
        {
            //バフテキストを消す
            Destroy(gameObject.transform.GetChild(2).gameObject);

            //ボタンを作る
            for (int i = 0; i < JoutaiCount; i++)
            {
                GameObject Button = Instantiate(JoutaiButtonPrefab, ButtonParentRect.gameObject.transform);

                //位置を設定
                Button.GetComponent<RectTransform>().localPosition = new Vector2(0, 480-(i * 180));

                //名前を設定
                Button.name = "JoutaiButton"+i;

                //イベントトリガーを設定
                {
                    EventTrigger.Entry entry = new EventTrigger.Entry();

                    //ボインターを押した時のイベントトリガー
                    entry.eventID = EventTriggerType.PointerClick;
                    entry.callback.AddListener((x) => ButtonKinou(i));

                    Button.GetComponent<EventTrigger>().triggers.Add(entry);
                    
                    entry = new EventTrigger.Entry();

                    //ボタンが押されたときのイベントトリガー
                    entry.eventID = EventTriggerType.PointerDown;
                    entry.callback.AddListener((x) => ButtonDown(Button));

                    Button.GetComponent<EventTrigger>().triggers.Add(entry);

                    entry = new EventTrigger.Entry();

                    //ボタンから離した時のイベントトリガー
                    entry.eventID = EventTriggerType.PointerUp;
                    entry.callback.AddListener((x) => ButtonUp(Button));

                    Button.GetComponent<EventTrigger>().triggers.Add(entry);

                    entry = new EventTrigger.Entry();

                    //ボタンからポインターが出た時のイベントトリガー
                    entry.eventID = EventTriggerType.PointerExit;
                    entry.callback.AddListener((x) => ButtonExit(Button));

                    Button.GetComponent<EventTrigger>().triggers.Add(entry);
                }

                //画像だとかテキストを設定
                Button.transform.GetChild(0).GetComponent<Image>().sprite = AllDataManege.BaffDebaffDataList[AllDataManege.CharactorData.NowJoutai[i]].GetSprite();
                
                Button.transform.GetChild(1).GetComponent<Text>().text = AllDataManege.BaffDebaffDataList[AllDataManege.CharactorData.NowJoutai[i]].GetName();
                Button.transform.GetChild(2).GetComponent<Text>().text = AllDataManege.CharactorData.JoutaiKeika[i].ToString();
            }
        }

        //最高位置の設定
        //もし表示外にボタンが出たら最高位置を上げる
        if (JoutaiCount > 5)
        {
            SaikouFloat = 120 + ((JoutaiCount - 7) * 180);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //メインカメラ上のマウスカーソルのある位置からRayを飛ばす
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //Rayの長さ
        float maxDistance = 10;

        hit = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction, maxDistance);

        if (InFlag)
        {
            ThisRect.localScale = new Vector3(1, ThisRect.localScale.y + (Speed * Time.deltaTime), 1);

            ThisRect.localScale = new Vector3(1, Mathf.Clamp01(ThisRect.localScale.y), 1);

            if (ThisRect.localScale.y >= 1f)
            {
                InFlag = false;
                CanButton = true;
            }
        }

        if (OutFlag)
        {
            ThisRect.localScale = new Vector3(1, ThisRect.localScale.y - (Speed * Time.deltaTime), 1);

            ThisRect.localScale = new Vector3(1, Mathf.Clamp01(ThisRect.localScale.y), 1);

            if (ThisRect.localScale.y <= 0f)
            {
                OutFlag = false;

                DangeonManeger.CanCommandFlag = true;

                Destroy(gameObject);
            }
        }

        if (HoldFlag)
        {
            NowPosi = Input.mousePosition;

            if (NowPosi != MotoPosi)
            {
                IdouFlag = true;

                ButtonImage.color = new Color(1,1,1);
            }
        }

        if (IdouFlag)
        {
            NowPosi = Input.mousePosition;

            ButtonParentRect.localPosition = new Vector2(0, ButtonParentRect.localPosition.y - (MotoPosi.y - NowPosi.y) * DrugSpeed);
            ButtonParentRect.localPosition = new Vector2(0, Mathf.Clamp(ButtonParentRect.localPosition.y, 0, SaikouFloat));

            MotoPosi = Input.mousePosition;
        }
    }

    public void ButtonDown(GameObject Button)
    {
        if (!CanButton)
        {
            return;
        }

        ButtonImage = Button.GetComponent<Image>();

        ButtonImage.color = new Color(0.8f,0.8f,0.8f);

        HoldFlag = true;

        MotoPosi = Input.mousePosition;
    }

    public void ButtonExit(GameObject Button)
    {
        if (!CanButton)
        {
            return;
        }

        ButtonImage = Button.GetComponent<Image>();

        ButtonImage.color = new Color(1f, 1f, 1f);
    }

    public void ButtonUp(GameObject Button)
    {
        if (!CanButton)
        {
            return;
        }

        ButtonImage = Button.GetComponent<Image>();

        ButtonImage.color = new Color(1f, 1f, 1f);

        if ((hit.collider&&hit.collider.name!=Button.name)||Button.name=="ButtonMask")
        {
            HoldFlag = false;
            IdouFlag = false;
        }
    }

    public void ButtonKinou(int value)
    {
        if (!CanButton||IdouFlag)
        {
            HoldFlag = false;
            IdouFlag = false;

            return;
        }

        HoldFlag = false;
        IdouFlag = false;

        //戻るボタンの時
        if (value == 0)
        {
            OutFlag = true;
            CanButton = false;
        }

        //バフボタンの時
        if (value>0)
        {

            CanButton = false;

            //バフの説明ウィンドウを作る
            SetumeiWindow = Instantiate(SetumeiPrefab,gameObject.transform.parent.transform);
            
            SetumeiWindow.transform.GetChild(1).GetComponent<Image>().sprite = AllDataManege.BaffDebaffDataList[AllDataManege.CharactorData.NowJoutai[value-1]].GetSprite();

            SetumeiWindow.transform.GetChild(2).GetComponent<Text>().text= AllDataManege.BaffDebaffDataList[AllDataManege.CharactorData.NowJoutai[value-1]].GetSetumei();
            SetumeiWindow.transform.GetChild(3).GetComponent<Text>().text= AllDataManege.BaffDebaffDataList[AllDataManege.CharactorData.NowJoutai[value-1]].GetName();

            SetumeiWindow.transform.GetChild(5).GetComponent<RectTransform>().localPosition = new Vector2(0,-400);

            Destroy(SetumeiWindow.transform.GetChild(4).gameObject);
            Destroy(SetumeiWindow.transform.GetChild(6).gameObject);

            SetumeiWindow.GetComponent<StetasSetumeiScript>().JoutaiScript=this;
        }
    }
}
