using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
[CreateAssetMenu(fileName = "EnemySyuzokuData", menuName = "CreateData/CreateEnemy/SyuzokuData")]
public class BaseEnemySyuzokuData : ScriptableObject
{
    [SerializeField]
    private int SyuzokuInt;

    [SerializeField]
    private string SyuzokuName;

    [SerializeField]
    private Sprite SyuzokuSprite;

    [SerializeField]
    [Multiline] private string SyuzokuSetumei;

    [SerializeField]
    private int KougekiType;

    [SerializeField]
    private int[] SaiteiParameter = new int[12];

    [SerializeField]
    private int[] SaikouParameter = new int[12];

    [SerializeField]
    private int[] KoudouPattern;

    [SerializeField]
    private int[] Koudou;
    //0 何もしない　1 通常攻撃

    [SerializeField]
    private int[] DropItemKakuritu;

    [SerializeField]
    private int[] DropItem;

    [SerializeField]
    private int[] SkillAndMagic = new int[6];

    [SerializeField]
    private int KougekiWeapon;

    [SerializeField]
    private int KougekiZokusei;

    [SerializeField]
    private int[] WeekWeaponType;

    [SerializeField]
    private int[] WeekZokusei;

    [SerializeField]
    private int[] StrongWeaponType;

    [SerializeField]
    private int[] StrongZokusei;

    [SerializeField]
    private int[] GetExp = new int[2];
    //0 最低 1 最高

    [SerializeField]
    private int[] GetGold = new int[2];

    //通常攻撃の効果音
    [SerializeField]
    private int SoundEffect;

    //ゲッター
    public int GetSyuzokuInt()
    {
        return SyuzokuInt;
    }

    public int GetKougekiType()
    {
        return KougekiType;
    }

    public int GetSoundEffect()
    {
        return SoundEffect;
    }

    public int GetKougekiWeapon()
    {
        return KougekiWeapon;
    }

    public int GetKougekiZokusei()
    {
        return KougekiZokusei;
    }

    public string GetSyuzokuName()
    {
        return SyuzokuName;
    }

    public string GetSyuzokuSetumei()
    {
        return SyuzokuSetumei;
    }

    public int[] GetSaiteiParameter()
    {
        return SaiteiParameter;
    }

    public int[] GetSaikouParameter()
    {
        return SaikouParameter;
    }

    public int[] GetSkillAndMagic()
    {
        return SkillAndMagic;
    }

    public int[] GetGetExp()
    {
        return GetExp;
    }

    public int[] GetGetGold()
    {
        return GetGold;
    }

    public int[] GetKoudouPattern()
    {
        return KoudouPattern;
    }

    public int[] GetKoudou()
    {
        return Koudou;
    }

    public int[] GetDropKakuritu()
    {
        return DropItemKakuritu;
    }

    public int[] GetDropItem()
    {
        return DropItem;
    }

    public int[] GetWeekWeapon()
    {
        return WeekWeaponType;
    }

    public int[] GetWeekZokusei()
    {
        return WeekZokusei;
    }

    public int[] GetStrongWeapon()
    {
        return StrongWeaponType;
    }

    public int[] GetStrongZokusei()
    {
        return StrongZokusei;
    }

    public Sprite GetSprite()
    {
        return SyuzokuSprite;
    }
}
