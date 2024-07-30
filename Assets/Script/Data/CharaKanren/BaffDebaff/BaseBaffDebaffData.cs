using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
[CreateAssetMenu(fileName = "BaffDebaffData", menuName = "CreateData/CreateBaffDebaff/Data")]
public class BaseBaffDebaffData : ScriptableObject
{
    [SerializeField]
    private int BaffNumber;

    [SerializeField]
    private string BaffName;

    [SerializeField]
    [Multiline] private string BaffSetumei;

    [SerializeField]
    private Sprite BaffSprite;

    [SerializeField]
    private int BaffKeizokuTurn;

    [SerializeField]
    private int BaffKeizokuType;
    //0 ターン継続 1 バトルが終わるまで継続

    [SerializeField]
    private int BaffUpType;
    //0 その分上がる 1 ターンがたつごとに上乗せ 2 その分減る 3 ターンがたつごとに減る

    [SerializeField]
    private int[] BaffAddParameter = new int[14];

    //ゲッター
    public int GetNumber()
    {
        return BaffNumber;
    }

    public string GetName()
    {
        return BaffName;
    }

    public string GetSetumei()
    {
        return BaffSetumei;
    }

    public Sprite GetSprite()
    {
        return BaffSprite;
    }

    public int GetKeizokuType()
    {
        return BaffKeizokuType;
    }

    public int GetKeizokuTurn()
    {
        return BaffKeizokuTurn;
    }

    public int GetBaffUpType()
    {
        return BaffUpType;
    }

    public int[] GetAddParameter()
    {
        return BaffAddParameter;
    }
}
