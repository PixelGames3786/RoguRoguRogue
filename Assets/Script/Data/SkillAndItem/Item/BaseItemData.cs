using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
[CreateAssetMenu(fileName = "ItemData", menuName = "CreateData/CreateItem/ItemData")]
public class BaseItemData : ScriptableObject
{
    [SerializeField]
    private string ItemName;

    [SerializeField]
    [Multiline] private string ItemSetumei;

    [SerializeField]
    private int ItemNumber;

    [SerializeField]
    private int ItemType;
    //1 武器 2　防具 3　アクセサリー 4 消費アイテム 5 売却アイテム

    [SerializeField]
    private int WeaponType;
    //0 棒 1 剣 2 槍 3 弓 4 杖 5 斧 6 槌 7 爆弾

    [SerializeField]
    private int ItemZokusei;
    // 0 なし 1 炎 2 風 3 水 4 地 5 闇 6 光 7 雷

    [SerializeField]
    private int[] WeekWeaponType;

    [SerializeField]
    private int[] WeekZokusei;

    [SerializeField]
    private int[] StrongWeaponType;

    [SerializeField]
    private int[] StrongZokusei;

    [SerializeField]
    private int KougekiType;
    //0 物理攻撃 1 魔法攻撃

    [SerializeField]
    private int SyouhiItemType;
    //1 ＨＰのみ回復 2 ＭＰのみ回復 3 どっちも回復 4 HP割合回復 5 MP割合回復 6 どっちも割合回復 7 相手にダメージ 8 敵バフ付与 9 自分バフ付与 10 煙幕

    [SerializeField]
    private Sprite ItemSprite;

    [SerializeField]
    private int[] AddParameter = new int[11];
    //0 最大HP 1 最大MP 2 ATK

    [SerializeField]
    private int KaihukuSuuji;

    [SerializeField]
    private float KaihukuWariai;

    [SerializeField]
    private int AddBaffNumber;

    [SerializeField]
    private int[] ItemDamege;

    [SerializeField]
    private int BukiMeityu;

    [SerializeField]
    private int BukiHissatu;

    [SerializeField]
    private int BykyakuNedan;

    [SerializeField]
    private int KonyuNedan;

    [SerializeField]
    private int TokusyuType;

    [SerializeField]
    private int CanUseBasyo;
    //0 いつでも使える 1 戦闘中

    //攻撃だとか使った時に出るパーティクル
    [SerializeField]
    private GameObject Particle;

    //攻撃だとか使った時に出る効果音
    [SerializeField]
    private int SoundEffect;

    //ゲッター
    public string GetItemName()
    {
        return ItemName;
    }

    public string GetItemSetumei()
    {
        return ItemSetumei;
    }

    public int GetItemNumber()
    {
        return ItemNumber;
    }

    public Sprite GetItemSprite()
    {
        return ItemSprite;
    }

    public int GetItemType()
    {
        return ItemType;
    }

    public int GetSyouhiType()
    {
        return SyouhiItemType;
    }

    public int GetKougekiType()
    {
        return KougekiType;
    }

    public int[] GetAddParameter()
    {
        return AddParameter;
    }

    public int GetAddBaffNumber()
    {
        return AddBaffNumber;
    }

    public int GetKaihukuSuuji()
    {
        return KaihukuSuuji;
    }

    public float GetKaihukuWariai()
    {
        return KaihukuWariai;
    }

    public int[] GetItemDamege()
    {
        return ItemDamege;
    }

    public int GetZokusei()
    {
        return ItemZokusei;
    }

    public int GetWeaponType()
    {
        return WeaponType;
    }

    public int[] GetStrongZokusei()
    {
        return StrongZokusei;
    }

    public int[] GetStrongWeapon()
    {
        return StrongWeaponType;
    }

    public int[] GetWeekZokusei()
    {
        return WeekZokusei;
    }

    public int[] GetWeekWeapon()
    {
        return WeekWeaponType;
    }

    public int GetBukiMeityu()
    {
        return BukiMeityu;
    }

    public int GetBukiHissatu()
    {
        return BukiHissatu;
    }

    public int GetBykyaku()
    {
        return BykyakuNedan;
    }

    public int GetKonyu()
    {
        return KonyuNedan;
    }

    public int GetTokusyuType()
    {
        return TokusyuType;
    }

    public int GetUseBasyo()
    {
        return CanUseBasyo;
    }

    public GameObject GetParticle()
    {
        return Particle;
    }

    public int GetSound()
    {
        return SoundEffect;
    }

}
