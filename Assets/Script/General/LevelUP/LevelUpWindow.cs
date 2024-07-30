using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpWindow : MonoBehaviour
{
    private RectTransform ThisRect;

    private Text[] ParameterText = new Text[11];

    public GameObject LevelUpIconPrefab,SkillNyusyuPrefab,SkillChangePrefab;
    private GameObject LevelUpIcon,SkillNyusyuWindow,SkillChangeWindow;

    [System.NonSerialized]
    public bool InFlag, OutFlag;

    public float Speed;

    public int LevelUpType,LevelUpKaisu,SkillKoukanKaisu,SkillKoukanJikkou;
    public List<int> SkillKoukanList;

    private int[] SkillSyutokuKekka=new int[2];

    // Start is called before the first frame update
    void Start()
    {
        AllAudioManege.PlaySE(12);

        for (int i=0;i<LevelUpKaisu;i++)
        {
            //レベルアップ処理
            AllDataManege.CharactorLevelUp();

            //スキル取得処理
            SkillSyutokuKekka = AllDataManege.SkillSyutokuHantei(this);
        }

        //パラメーターオブジェクトを取得
        for (int i=0;i<11;i++)
        {
            ParameterText[i] = gameObject.transform.GetChild(0).transform.GetChild(i).GetComponent<Text>();
        }
        
        //パラメーターを表示
        ParameterText[0].text = "HP " + AllDataManege.CharactorData.GetParameter()[1];

        //もし上がっていたら
        if (AllDataManege.ParameterUpFlag[0])
        {
            LevelUpIcon=Instantiate(LevelUpIconPrefab,gameObject.transform.GetChild(1).transform);

            LevelUpIcon.GetComponent<RectTransform>().localPosition = new Vector3(ParameterText[0].gameObject.GetComponent<RectTransform>().localPosition.x+190, ParameterText[0].gameObject.GetComponent<RectTransform>().localPosition.y - 150, 0);

            AllDataManege.ParameterUpFlag[0] = false;
        }

        ParameterText[1].text = "MP " + AllDataManege.CharactorData.GetParameter()[3];

        //もし上がっていたら
        if (AllDataManege.ParameterUpFlag[1])
        {
            LevelUpIcon = Instantiate(LevelUpIconPrefab, gameObject.transform.GetChild(1).transform);

            LevelUpIcon.GetComponent<RectTransform>().localPosition = new Vector3(ParameterText[1].gameObject.GetComponent<RectTransform>().localPosition.x + 190, ParameterText[1].gameObject.GetComponent<RectTransform>().localPosition.y - 150, 0);

            AllDataManege.ParameterUpFlag[1] = false;
        }

        for (int i=5;i<14;i++)
        {
            ParameterText[i - 3].text += " " + AllDataManege.CharactorData.GetParameter()[i];

            if (AllDataManege.ParameterUpFlag[i - 3])
            {
                LevelUpIcon = Instantiate(LevelUpIconPrefab, gameObject.transform.GetChild(1).transform);

                LevelUpIcon.GetComponent<RectTransform>().localPosition = new Vector3(ParameterText[i-3].gameObject.GetComponent<RectTransform>().localPosition.x + 190, ParameterText[i-3].gameObject.GetComponent<RectTransform>().localPosition.y - 150, 0);

                AllDataManege.ParameterUpFlag[i-3] = false;
            }
        }

        ThisRect = gameObject.GetComponent<RectTransform>();

        InFlag = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (InFlag)
        {
            ThisRect.localPosition = new Vector3(ThisRect.localPosition.x+(Speed*Time.deltaTime),0,0);

            ThisRect.localPosition = new Vector3(Mathf.Clamp(ThisRect.localPosition.x,-1040,-40),0,0);

            if (ThisRect.localPosition.x>=-40)
            {
                InFlag = false;

                //もし普通にスキルを入手出来ていたら
                if (SkillSyutokuKekka[0] == 1)
                {
                    SkillNyusyuWindow = Instantiate(SkillNyusyuPrefab,gameObject.transform.parent);

                    SkillNyusyuWindow.GetComponent<SkillSyutokuWindow>().IrekaeSkill = SkillSyutokuKekka[1];
                }

                //もしスキル交換の必要があったら
                if (SkillKoukanKaisu>0)
                {
                    SkillChangeWindow = Instantiate(SkillChangePrefab,gameObject.transform.parent);

                    SkillChangeWindow.GetComponent<SkillChangeScript>().LevelUpScript = this;
                    SkillChangeWindow.GetComponent<SkillChangeScript>().SyutokuSkill = SkillKoukanList[0];

                    SkillKoukanJikkou++;
                }
            }
        }

        if (OutFlag)
        {
            ThisRect.localPosition = new Vector3(ThisRect.localPosition.x - (Speed * Time.deltaTime), 0, 0);

            ThisRect.localPosition = new Vector3(Mathf.Clamp(ThisRect.localPosition.x, -1040, -40), 0, 0);

            if (ThisRect.localPosition.x <= -1040)
            {
                if (LevelUpType==1)
                {
                    TownManegerScript.CanCommandFlag = true;
                }

                if (LevelUpType==2)
                {
                    DangeonManeger.CanCommandFlag = true;
                }

                AllDataManege.DataSave();

                Destroy(gameObject);
            }
        }
    }

    public void SetOutFlag()
    {
        if (!InFlag&&!SkillNyusyuWindow&&!SkillChangeWindow)
        {
            AllAudioManege.PlaySE(6);

            OutFlag = true;
        }
    }
}
