using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class KakuninWindowScript : MonoBehaviour
{
    private GameObject KakinYobidashi,KakinWindow;
    public GameObject YobidashiPrefab,KakinPrefab;

    private RectTransform ThisRect;

    public float Speed;

    private bool InFlag,OutFlag;

    public int KakuninType;

    // Start is called before the first frame update
    void Start()
    {
        InFlag = true;

        ThisRect = this.gameObject.GetComponent<RectTransform>();

        if (KakuninType==1)
        {
            KakinYobidashi = Instantiate(YobidashiPrefab,gameObject.transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (InFlag)
        {
            ThisRect.localScale = new Vector3(ThisRect.localScale.x+(Speed*Time.deltaTime),ThisRect.localScale.y+(Speed*Time.deltaTime),1);

            if (ThisRect.localScale.x>=1f)
            {
                InFlag = false;
            }
        }

        if (OutFlag)
        {
            ThisRect.localScale = new Vector3(ThisRect.localScale.x - (Speed * Time.deltaTime), ThisRect.localScale.y - (Speed * Time.deltaTime), 1);

            if (ThisRect.localScale.x<=0f)
            {
                //もし職業・種族選択時の旅経験足りないとき
                if (KakuninType==1&&!GameObject.Find("KakinParent(Clone)"))
                {
                    CreateCharactorManeger.TyuuiFlag = false;
                }

                //もしダンジョンクリアの時
                if (KakuninType==2)
                {
                    DangeonManeger.CanCommandFlag = true;
                }

                Destroy(this.gameObject);
            }
        }

        if (!InFlag&&!OutFlag)
        {
            if (Application.isEditor)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    OutFlag = true;
                }
            }
            else
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    OutFlag = true;
                }
            }
        }

        ThisRect.localScale = new Vector3(Mathf.Clamp01(ThisRect.localScale.x),Mathf.Clamp01(ThisRect.localScale.y),1);

    }

    public void MakeKakinWindow()
    {

        KakinWindow = Instantiate(KakinPrefab,GameObject.Find("SelectCanvas").transform);

        GameObject.Find("SelectKakuninParent3(Clone)").GetComponent<Animator>().SetBool("OutFlag", true);
    }
}
