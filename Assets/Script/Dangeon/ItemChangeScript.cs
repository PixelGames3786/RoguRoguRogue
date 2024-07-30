using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemChangeScript : MonoBehaviour
{
    public GameObject ItemSetumeiPrefab;
    private GameObject ItemSetumeiWindow;

    private RectTransform ThisRect;

    [System.NonSerialized]
    public bool InFlag, OutFlag;

    public float Speed;

    [System.NonSerialized]
    public int SyutokuItem;

    // Start is called before the first frame update
    void Start()
    {
        ThisRect = gameObject.GetComponent<RectTransform>();

        gameObject.transform.GetChild(1).GetComponent<Image>().sprite = AllDataManege.ItemDataList[SyutokuItem].GetItemSprite();

        for (int i=2;i<11;i++)
        {
            gameObject.transform.GetChild(i).GetComponent<Image>().sprite = AllDataManege.BagItemList[i-2].GetItemSprite();
        }

        InFlag = true;
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
            ThisRect.localScale = new Vector3(1, ThisRect.localScale.y - (Speed * Time.deltaTime), 1);

            ThisRect.localScale = new Vector3(1, Mathf.Clamp01(ThisRect.localScale.y), 1);

            if (ThisRect.localScale.y <=0f)
            {
                AllDataManege.DataSave();

                DangeonManeger.CanCommandFlag = true;

                Destroy(gameObject);
                
            }
        }
    }

    public void SetOutFlag()
    {
        if (!InFlag&&!ItemSetumeiWindow)
        {
            OutFlag = true;
        }
    }

    public void MakeSetumeiWindow(int value)
    {
        if (value==0)
        {
            ItemSetumeiWindow = Instantiate(ItemSetumeiPrefab,gameObject.transform.parent);

            ItemSetumeiWindow.GetComponent<SkillSyutokuWindow>().WindowType = 3;
            ItemSetumeiWindow.GetComponent<SkillSyutokuWindow>().IrekaeSkill = SyutokuItem;

            ItemSetumeiWindow.GetComponent<SkillSyutokuWindow>().SkillChangeWindow = gameObject;
        }

        if (value > 0)
        {
            ItemSetumeiWindow = Instantiate(ItemSetumeiPrefab, gameObject.transform.parent);

            ItemSetumeiWindow.GetComponent<SkillSyutokuWindow>().WindowType = 4;
            ItemSetumeiWindow.GetComponent<SkillSyutokuWindow>().IrekaeSkill = SyutokuItem;
            ItemSetumeiWindow.GetComponent<SkillSyutokuWindow>().IrekaeMotoSkill = value - 1;

            ItemSetumeiWindow.GetComponent<SkillSyutokuWindow>().SkillChangeWindow = gameObject;
        }
    }
}
