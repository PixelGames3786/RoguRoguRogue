using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
[CreateAssetMenu(fileName = "EnemySyuzokuDataBase", menuName = "CreateData/CreateEnemy/SyuzokuDataBase")]
public class EnemySyuzokuDataBase : ScriptableObject
{
    [SerializeField]
    private List<BaseEnemySyuzokuData> EnemySyuzokuDataList = new List<BaseEnemySyuzokuData>();

    public List<BaseEnemySyuzokuData> GetSyuzokuDataList()
    {
        return EnemySyuzokuDataList;
    }
}
