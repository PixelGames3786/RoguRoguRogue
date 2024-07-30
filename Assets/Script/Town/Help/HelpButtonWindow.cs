using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelpButtonWindow : MonoBehaviour
{
    private RectTransform ButtonRect,ThisRect;

    private Text ButtonText;

    public GameObject HelpWindowPrefab;
    private GameObject HelpWindow;

    private Vector3 MotoPosition, NowPosition;

    public TextAsset HelpTextAsset;

    private string[] HelpTexts;

    private Image[] ButtonImages;
    
    public float Speed;
    private bool InFlag, OutFlag, HoldFlag, IdouFlag;

    // Start is called before the first frame update
    void Start()
    {
        ThisRect = gameObject.GetComponent<RectTransform>();
        ButtonRect = gameObject.transform.GetChild(1).transform.GetChild(0).GetComponent<RectTransform>();

        InFlag = true;

        TownManegerScript.CanCommandFlag = false;

        ButtonImages = new Image[ButtonRect.childCount];

        HelpTexts = HelpTextAsset.text.Split('@');

        for (int i=0;i<10;i++)
        {
            ButtonImages[i] = ButtonRect.transform.GetChild(i).GetComponent<Image>();

            if (AllDataManege.SomeIntData.GetHelpKaihou(i)==0)
            {
                ButtonImages[i].color = new Color(0.7f,0.7f,0.7f);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (InFlag)
        {
            ThisRect.localScale = new Vector3(1,ThisRect.localScale.y+(Speed*Time.deltaTime),1);
            ThisRect.localScale = new Vector3(1,Mathf.Clamp01(ThisRect.localScale.y),1);

            if (ThisRect.localScale.y>=1f)
            {
                InFlag = false;
            }
        }

        if (OutFlag)
        {
            ThisRect.localScale = new Vector3(1,ThisRect.localScale.y-(Speed*Time.deltaTime),1);
            ThisRect.localScale = new Vector3(1,Mathf.Clamp01(ThisRect.localScale.y),1);

            if (ThisRect.localScale.y<=0)
            {
                TownManegerScript.CanCommandFlag = true;

                Destroy(this.gameObject);
            }
        }

        if (HoldFlag)
        {
            NowPosition = Input.mousePosition;

            if (NowPosition!=MotoPosition)
            {
                IdouFlag = true;
            }
        }

        if (IdouFlag)
        {
            NowPosition = Input.mousePosition;

            ButtonRect.localPosition = new Vector2(0, ButtonRect.localPosition.y-(MotoPosition.y-NowPosition.y)*1.3f);
            ButtonRect.localPosition = new Vector2(0,Mathf.Clamp(ButtonRect.localPosition.y,-75,560));

            MotoPosition = Input.mousePosition;
        }
    }

    public void SetOutFlag()
    {
        AllAudioManege.PlaySE(6);

        OutFlag = true;
    }

    public void MouseDown()
    {
        HoldFlag = true;

        MotoPosition = Input.mousePosition;
    }

    public void MouseUp(int value)
    {
        //もし移動していたらボタンの判定をしない
        if (IdouFlag||value==0)
        {
            HoldFlag = false;
            IdouFlag = false;

            return;
        }

        //もし開放されていなかったら
        if (AllDataManege.SomeIntData.GetHelpKaihou(value-1)==0)
        {
            AllAudioManege.PlaySE(9);

            HoldFlag = false;
            IdouFlag = false;

            return;
        }

        if (!HelpWindow)
        {
            AllAudioManege.PlaySE(0);

            HoldFlag = false;
            IdouFlag = false;

            HelpWindow = Instantiate(HelpWindowPrefab, gameObject.transform.parent.transform);

            HelpWindow.transform.GetChild(1).transform.GetChild(0).GetComponent<Text>().text = HelpTexts[value - 1];
        }

    }
}
