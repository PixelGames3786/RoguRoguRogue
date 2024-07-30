using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
[CreateAssetMenu(fileName = "SomeIntData", menuName = "CreateData/CreateSomeIntData")]
public class SomeIntData : ScriptableObject
{
    [SerializeField]
    private int[] HattenHituyou;

    [SerializeField]
    private int[] KunrenGold;

    [SerializeField]
    private int[] HelpKaihou;

    [SerializeField]
    private int[] GetTabikeiken;

    [SerializeField]
    private int[] LevelUpExp;

    [SerializeField]
    private int[] HattenGetGold;

    [SerializeField]
    private int[] YadoyaGold;

    //発展レベルに応じての村のテキスト表示の変更
    [SerializeField]
    private int[] TownTextMin;

    [SerializeField]
    private int[] TownTextMax;

    //ゲッター

    public int[] GetHattenHituyou()
    {
        return HattenHituyou;
    }

    public int GetKunrenGold(int value)
    {
        return KunrenGold[value];
    }

    public int GetHelpKaihou(int value)
    {
        return HelpKaihou[value];
    }

    public int GetGetTabikeiken(int value)
    {
        return GetTabikeiken[value];
    }

    public int GetYadoyaGold(int value)
    {
        return YadoyaGold[value];
    }

    public int GetHattenGetGold(int value)
    {
        return HattenGetGold[value];
    }

    public int GetLevelUpExp(int value)
    {
        return LevelUpExp[value];
    }

    public int GetTownTextMin(int value)
    {
        return TownTextMin[value];
    }

    public int GetTownTextMax(int value)
    {
        return TownTextMax[value];
    }
}
