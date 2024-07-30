using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
[CreateAssetMenu(fileName = "DangeonDataBase", menuName = "CreateData/CreateDangeon/DataBase")]
public class DangeonDataBase : ScriptableObject
{
    [SerializeField]
    private List<BaseDangeonData> DangeonDataList = new List<BaseDangeonData>();

    public List<BaseDangeonData> GetDangeonDataBase()
    {
        return DangeonDataList;
    }
}