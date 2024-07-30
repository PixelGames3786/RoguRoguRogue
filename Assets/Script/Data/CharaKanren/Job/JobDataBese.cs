using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
[CreateAssetMenu(fileName = "JobDataBase", menuName = "CreateData/CreateJob/DataBase")]
public class JobDataBese : ScriptableObject
{
    [SerializeField]
    private List<BaseJobData> JobDataList = new List<BaseJobData>();

    public List<BaseJobData> GetJobDataList()
    {
        return JobDataList;
    }
}
