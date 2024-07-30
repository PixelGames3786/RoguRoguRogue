using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelpWindow : MonoBehaviour
{
    private RectTransform ThisRect,TextRect;

    private Vector2 MotoPosi, NowPosi;

    public float Speed;

    private bool InFlag, OutFlag, HoldFlag, IdouFlag;

    // Start is called before the first frame update
    void Start()
    {
        ThisRect = gameObject.GetComponent<RectTransform>();
        TextRect = gameObject.transform.GetChild(1).transform.GetChild(0).GetComponent<RectTransform>();

        InFlag = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (InFlag)
        {
            ThisRect.localScale = new Vector3(ThisRect.localScale.x + (Speed * Time.deltaTime), 1, 1);
            ThisRect.localScale = new Vector3(Mathf.Clamp01(ThisRect.localScale.x),1,1);

            if (ThisRect.localScale.x>=1f)
            {
                InFlag = false;
            }
        }

        if (OutFlag)
        {
            ThisRect.localScale = new Vector3(ThisRect.localScale.x - (Speed * Time.deltaTime), 1, 1);
            ThisRect.localScale = new Vector3(Mathf.Clamp01(ThisRect.localScale.x), 1, 1);

            if (ThisRect.localScale.x <= 0f)
            {
                Destroy(gameObject);
            }
        }

        if (HoldFlag)
        {
            NowPosi = Input.mousePosition;

            if (NowPosi!=MotoPosi)
            {
                IdouFlag = true;
            }
        }

        if (IdouFlag)
        {
            NowPosi = Input.mousePosition;

            TextRect.localPosition = new Vector2(0, TextRect.localPosition.y - (MotoPosi.y - NowPosi.y) * 1.3f);
            TextRect.localPosition = new Vector2(0, Mathf.Clamp(TextRect.localPosition.y, -250, 250));

            MotoPosi = Input.mousePosition;
        }
    }

    public void MouseDown()
    {
        HoldFlag = true;

        MotoPosi = Input.mousePosition;
    }

    public void MouseUp()
    {
        HoldFlag = false;
        IdouFlag = false;
    }

    public void SetOutFlag()
    {
        AllAudioManege.PlaySE(6);

        OutFlag = true;
    }
}
