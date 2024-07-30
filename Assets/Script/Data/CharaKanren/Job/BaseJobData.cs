using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
[CreateAssetMenu(fileName = "JobData", menuName = "CreateData/CreateJob/JobData")]
public class BaseJobData :ScriptableObject
{
    [SerializeField]
    private string JobName;

    [SerializeField]
    [Multiline]private string JobSetumei;

    [SerializeField]
    private int HituyouTabikeiken;

    [SerializeField]
    private float[] CreateAddParameter=new float[12];

    [SerializeField]
    private int[] GrowParameter = new int[12];

    [SerializeField]
    private int[] SkillLevel = new int[10];

    [SerializeField]
    private int[] SyutokuSkill = new int[10];
    //991 初級魔法四種ランダム取得 992 中級魔法四種ランダム取得 993 上級魔法四種ランダム取得 994 特級魔法四種ランダム取得

    //ゲッター
    public float[] GetAddParameter()
    {
        return CreateAddParameter;
    }

    public int[] GetGrowParameter()
    {
        return GrowParameter;
    }

    public int[] GetSkillLevel()
    {
        return SkillLevel;
    }

    public int[] GetSyutokuSkill()
    {
        return SyutokuSkill;
    }

    public string GetJobName()
    {
        return JobName;
    }

    public string GetJobSetumei()
    {
        return JobSetumei;
    }

    public int GetHituyou()
    {
        return HituyouTabikeiken;
    }

}
