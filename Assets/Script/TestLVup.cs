using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestLVup : MonoBehaviour
{

    public GameObject TextCanvas;
    private GameObject ParameterParent;

    //データベース
    public SyuzokuDataBase SyuzokuBase;
    public JobDataBese JobBase;

    //色々なリストを取得
    public static List<BaseSyuzokuData> SyuzokuList = new List<BaseSyuzokuData>();
    public static List<BaseJobData> JobList = new List<BaseJobData>();

    public int SyuzokuNum,JobNum,Level;

    void Awake()
    {
        Random.InitState(System.DateTime.Now.Second + System.DateTime.Now.Millisecond + System.DateTime.Now.Hour);


        SyuzokuList = SyuzokuBase.GetSyuzokuDataList();
        JobList = JobBase.GetJobDataList();

        ParameterParent = TextCanvas.transform.GetChild(2).gameObject;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //レベルアップ検証用メソッド
    public void LevelUpKensyou()
    {
        //種族・職業を取得
        BaseSyuzokuData Syuzoku = SyuzokuList[SyuzokuNum];
        BaseJobData Job = JobList[JobNum];

        //パラメーター
        int[] Parameter = new int[14];

        //最低パラメーターを取得
        Parameter[0] = 1;
        Parameter[1] = Syuzoku.GetSaiteiParameter()[1];
        Parameter[2] = Parameter[1];
        Parameter[3] = Syuzoku.GetSaiteiParameter()[2];
        Parameter[4] = Parameter[3];

        for (int i=5;i<14;i++)
        {
            Parameter[i] = Syuzoku.GetSaiteiParameter()[i-2];
        }

        //パラメーター上昇シークエンス
        for (int i=1;i<Level;i++)
        {
            //レベル上昇
            Parameter[0]++;

            //HP上昇
            if (Syuzoku.GetGrowParameter()[1]+Job.GetGrowParameter()[1]>=Random.Range(0,101))
            {
                Parameter[1] += Random.Range(Syuzoku.GetGrowSaiteiParameter()[1],Syuzoku.GetGrowSaikouParameter()[1]);

                //もし上限を超えていたら
                if (Parameter[1]>Syuzoku.GetMaxParameter()[1])
                {
                    Parameter[1] = Syuzoku.GetMaxParameter()[1];
                }
            }

            //MP上昇
            if (Syuzoku.GetGrowParameter()[2] + Job.GetGrowParameter()[2] >= Random.Range(0,101))
            {
                Parameter[3] += Random.Range(Syuzoku.GetGrowSaiteiParameter()[2], Syuzoku.GetGrowSaikouParameter()[2]);

                //もし上限を超えていたら
                if (Parameter[3] > Syuzoku.GetMaxParameter()[2])
                {
                    Parameter[3] = Syuzoku.GetMaxParameter()[2];
                }
            }

            //その他上昇
            for (int u=3;u<12;u++)
            {
                if (Syuzoku.GetGrowParameter()[u] + Job.GetGrowParameter()[u] >= Random.Range(0, 101))
                {
                    Parameter[u+2] += Random.Range(Syuzoku.GetGrowSaiteiParameter()[u], Syuzoku.GetGrowSaikouParameter()[u]);

                    //もし上限を超えていたら
                    if (Parameter[u+2] > Syuzoku.GetMaxParameter()[u])
                    {
                        Parameter[u+2] = Syuzoku.GetMaxParameter()[u];
                    }
                }
            }
        }

        Parameter[2] = Parameter[1];
        Parameter[4] = Parameter[3];

        //テキストに反映
        TextCanvas.transform.GetChild(0).GetComponent<Text>().text = "種族:" + Syuzoku.GetSyuzokuName();
        TextCanvas.transform.GetChild(1).GetComponent<Text>().text = "職業:" + Job.GetJobName();

        ParameterParent.transform.GetChild(0).GetComponent<Text>().text = "LV:" + Parameter[0];
        ParameterParent.transform.GetChild(1).GetComponent<Text>().text = "MaxHP:" + Parameter[1];
        ParameterParent.transform.GetChild(2).GetComponent<Text>().text = "HP:" + Parameter[2];
        ParameterParent.transform.GetChild(3).GetComponent<Text>().text = "MaxMP:" + Parameter[3];
        ParameterParent.transform.GetChild(4).GetComponent<Text>().text = "MP:" + Parameter[4];
        ParameterParent.transform.GetChild(5).GetComponent<Text>().text = "ATK:" + Parameter[5];
        ParameterParent.transform.GetChild(6).GetComponent<Text>().text = "DEF:" + Parameter[6];
        ParameterParent.transform.GetChild(7).GetComponent<Text>().text = "INT:" + Parameter[7];
        ParameterParent.transform.GetChild(8).GetComponent<Text>().text = "MIN:" + Parameter[8];
        ParameterParent.transform.GetChild(9).GetComponent<Text>().text = "DEX:" + Parameter[9];
        ParameterParent.transform.GetChild(10).GetComponent<Text>().text = "AGI:" + Parameter[10];
        ParameterParent.transform.GetChild(11).GetComponent<Text>().text = "SPD:" + Parameter[11];
        ParameterParent.transform.GetChild(12).GetComponent<Text>().text = "LUC:" + Parameter[12];
        ParameterParent.transform.GetChild(13).GetComponent<Text>().text = "CHR:" + Parameter[13];


    }
}
