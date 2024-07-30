using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KakinScript : MonoBehaviour
{
    private RectTransform ThisRect;

    public float Speed;

    private bool InFlag, OutFlag;

    // Start is called before the first frame update
    void Start()
    {
        InFlag = true;

        ThisRect = gameObject.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {

        //メインカメラ上のマウスカーソルのある位置からRayを飛ばす
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //Rayの長さ
        float maxDistance = 10;

        RaycastHit2D hit = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction, maxDistance);

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
            ThisRect.localScale = new Vector3(1, ThisRect.localScale.y - (Speed * Time.deltaTime), 1);

            ThisRect.localScale = new Vector3(1, Mathf.Clamp01(ThisRect.localScale.y), 1);

            if (ThisRect.localScale.y <= 0f)
            {
                OutFlag = false;

                CreateCharactorManeger.TyuuiFlag = false;

                Destroy(gameObject);
            }
        }

        //ウィンドウを閉じる処理
        if (!InFlag&&!OutFlag)
        {
            if (Application.isEditor)
            {
                if (Input.GetMouseButtonDown(0)&& !hit.collider)
                {
                    OutFlag = true;
                }
            }
            else
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began && !hit.collider)
                {
                    OutFlag = true;
                }
            }
        }
    }

    //ボタンが押されたとき
    public void ButtonDown(GameObject Button)
    {
        Image ButtonImage = Button.GetComponent<Image>();
        Text Text = Button.transform.GetChild(0).GetComponent<Text>();

        ButtonImage.color = new Color(0.8f,0.8f,0.8f);
        Text.color = new Color(0.8f,0.8f,0.8f);
        
    }

    //ボタンが離されたとき
    public void ButtonUp(GameObject Button)
    {
        Image ButtonImage = Button.GetComponent<Image>();
        Text Text = Button.transform.GetChild(0).GetComponent<Text>();

        ButtonImage.color = new Color(1f, 1f, 1f);
        Text.color = new Color(0f, 0f, 0f);

    }

    //課金処理
    public void KakinButtonDown(int value)
    {
        if (AllDataManege.NowKakinSentaku)
        {
            return;
        }

        if (value==0)
        {
            AllDataManege.IAPListener.Purchasing(0);
        }

        if (value==1)
        {
            AllDataManege.IAPListener.Purchasing(1);
        }

        AllDataManege.NowKakinSentaku = true;
    }
}
