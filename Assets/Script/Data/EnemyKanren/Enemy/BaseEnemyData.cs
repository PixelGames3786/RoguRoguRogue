using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemyData
{
    [SerializeField]
    public int SyuzokuNumber;

    [SerializeField]
    public string EnemyName;

    [SerializeField]
    [Multiline] public string EnemySetumei;

    [SerializeField]
    public Sprite EnemySprite;

    [SerializeField]
    public int[] EnemyParameter=new int[14];

    [SerializeField]
    public int[] KoudouPattern;

    [SerializeField]
    public int[] Koudou;

    [SerializeField]
    public int[] DropItemKakuritu;

    [SerializeField]
    public int[] DropItem;

    [SerializeField]
    public int[] SkillAndMagic = new int[6];

    [SerializeField]
    public List<int> NowJoutai = new List<int>();

    [SerializeField]
    public List<int> JoutaiKeika = new List<int>();

    [SerializeField]
    public int GetExp;

    [SerializeField]
    public int GetGold;

    [SerializeField]
    public int KougekiType;
    //0 物理攻撃 1 魔法攻撃

    [SerializeField]
    public int KougekiWeapon;

    [SerializeField]
    public int KougekiZokusei;

    [SerializeField]
    public int[] WeekWeaponType;

    [SerializeField]
    public int[] WeekZokusei;
    
    [SerializeField]
    public int[] StrongWeaponType;

    [SerializeField]
    public int[] StrongZokusei;

    //通常攻撃の効果音
    [SerializeField]
    public int SoundEffect;

    //セッター
    public void SetParameter(int[] value)
    {
        EnemyParameter = value;
    }

    //ゲッター
    public int[] GetParameter()
    {
        return EnemyParameter;
    }
}
