using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
[CreateAssetMenu(fileName = "BaffDebaffDataBase", menuName = "CreateData/CreateBaffDebaff/DataBase")]
public class BaffDebaffDataBase : ScriptableObject
{
    [SerializeField]
    private List<BaseBaffDebaffData> BaffDebaffDataList = new List<BaseBaffDebaffData>();

    public List<BaseBaffDebaffData> GetDataBase()
    {
        return BaffDebaffDataList;
    }
}
