using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SomethingData
{
    [SerializeField]
    public int[] JobKaihouJoukyou=new int[12];

    [SerializeField]
    public int[] SyuzokuKaihouJoukyou=new int[12];

    [SerializeField]
    public int NowhaveTabikeiken;

    [SerializeField]
    public bool[] DangeonKouryaku = new bool[20];

    [SerializeField]
    public bool[] EnemySougu = new bool[100];

    [SerializeField]
    public bool[] EnemyGekiha = new bool[100];

    //セッター
    public void SetJobKaihou(int value,int Number)
    {
        JobKaihouJoukyou[Number] = value;
    }

    public void SetSyuzokuKaihou(int value,int Number)
    {
        SyuzokuKaihouJoukyou[Number] = value;
    }

    public void SetTabikeiken(int value)
    {
        NowhaveTabikeiken = value;
    }

    //ゲッター
    public int GetJobKaihou(int Number)
    {
        return JobKaihouJoukyou[Number];
    }

    public int GetSyuzokuKaihou(int Number)
    {
        return SyuzokuKaihouJoukyou[Number];
    }

    public int GetTabiKeiken()
    {
        return NowhaveTabikeiken;
    }
}
