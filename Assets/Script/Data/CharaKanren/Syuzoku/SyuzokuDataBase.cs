using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "SyuzokuDataBase", menuName = "CreateData/CreateSyuzoku/DataBase")]
public class SyuzokuDataBase : ScriptableObject
{
    [SerializeField]
    private List<BaseSyuzokuData> SyuzokuDataList = new List<BaseSyuzokuData>();

    public List<BaseSyuzokuData> GetSyuzokuDataList()
    {
        return SyuzokuDataList;
    }
}
