using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillChangeScript : MonoBehaviour
{
    public GameObject SkillSetumeiPrefab,SkillChangePrefab;
    private GameObject SkillSetumeiWindow,SkillChangeWindow;

    private RectTransform ThisRect;

    public LevelUpWindow LevelUpScript;

    [System.NonSerialized]
    public bool InFlag, OutFlag;

    public float Speed;

    [System.NonSerialized]
    public int SyutokuSkill;

    // Start is called before the first frame update
    void Start()
    {
        InFlag = true;

        ThisRect = gameObject.GetComponent<RectTransform>();

        gameObject.transform.GetChild(1).GetComponent<Image>().sprite = AllDataManege.SkillDataList[SyutokuSkill].GetSkillSprite();

        for (int i=0;i<6;i++)
        {
            gameObject.transform.GetChild(i + 2).GetComponent<Image>().sprite = AllDataManege.SkillDataList[AllDataManege.CharactorData.GetSkillMagic()[i]].GetSkillSprite();
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

            if (ThisRect.localScale.y<=0f)
            {
                AllDataManege.DataSave();

                if (LevelUpScript.SkillKoukanKaisu>1&&(LevelUpScript.SkillKoukanKaisu!=LevelUpScript.SkillKoukanJikkou))
                {
                    SkillChangeWindow = Instantiate(SkillChangePrefab, gameObject.transform.parent);

                    SkillChangeWindow.GetComponent<SkillChangeScript>().LevelUpScript = LevelUpScript;
                    SkillChangeWindow.GetComponent<SkillChangeScript>().SyutokuSkill = LevelUpScript.SkillKoukanList[LevelUpScript.SkillKoukanJikkou];

                    LevelUpScript.SkillKoukanJikkou++;
                }

                Destroy(gameObject);
            }
        }
    }

    public void SetOutFlag()
    {
        if (!InFlag&&!SkillSetumeiWindow)
        {
            OutFlag = true;
        }
    }

    public void MakeSetumeiWindow(int value)
    {
        if (value==0)
        {
            SkillSetumeiWindow = Instantiate(SkillSetumeiPrefab,gameObject.transform.parent);

            SkillSetumeiWindow.GetComponent<SkillSyutokuWindow>().WindowType=1;
            SkillSetumeiWindow.GetComponent<SkillSyutokuWindow>().IrekaeSkill = SyutokuSkill;

            SkillSetumeiWindow.GetComponent<SkillSyutokuWindow>().SkillChangeWindow = gameObject;
        }

        if (value>0)
        {
            SkillSetumeiWindow = Instantiate(SkillSetumeiPrefab,gameObject.transform.parent);

            SkillSetumeiWindow.GetComponent<SkillSyutokuWindow>().WindowType = 2;
            SkillSetumeiWindow.GetComponent<SkillSyutokuWindow>().IrekaeSkill = SyutokuSkill;
            SkillSetumeiWindow.GetComponent<SkillSyutokuWindow>().IrekaeMotoSkill = value - 1;

            SkillSetumeiWindow.GetComponent<SkillSyutokuWindow>().SkillChangeWindow = gameObject;
        }
    }
}
