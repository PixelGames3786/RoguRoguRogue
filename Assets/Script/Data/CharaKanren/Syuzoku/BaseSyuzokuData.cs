using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
[CreateAssetMenu(fileName = "SyuzokuData", menuName = "CreateData/CreateSyuzoku/SyuzokuData")]
public class BaseSyuzokuData :ScriptableObject
{
    public string SyuzokuName;
    [Multiline] public string SyuzokuSetumei;

    public int HituyouTabiKeiken;

    public int[] SaiteiParameter=new int[12];

    public int[] SaikouParameter = new int[12];

    public int[] CreateSaikouParameter = new int[12];

    public int[] GrowParameter = new int[12];

    public int[] GrowSaiteiPara = new int[12];

    public int[] GrowSaikouPara = new int[12];

    //ゲッター
    public int[] GetSaiteiParameter()
    {
        return SaiteiParameter;
    }

    public int[] GetMaxParameter()
    {
        return SaikouParameter;
    }

    public int[] GetCreateMaxParameter()
    {
        return CreateSaikouParameter;
    }

    public int[] GetGrowParameter()
    {
        return GrowParameter;
    }

    public int[] GetGrowSaiteiParameter()
    {
        return GrowSaiteiPara;
    }

    public int[] GetGrowSaikouParameter()
    {
        return GrowSaikouPara;
    }

    public string GetSyuzokuName()
    {
        return SyuzokuName;
    }

    public string GetSyuzokuSetumei()
    {
        return SyuzokuSetumei;
    }

    public int GetHituyou()
    {
        return HituyouTabiKeiken;
    }
}
