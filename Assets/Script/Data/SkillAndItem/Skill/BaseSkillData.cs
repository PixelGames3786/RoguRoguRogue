using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
[CreateAssetMenu(fileName = "SkillData", menuName = "CreateData/CreateSkill/SkillData")]
public class BaseSkillData : ScriptableObject
{
    [SerializeField]
    private string SkillName;

    [SerializeField]
    [Multiline] private string SkillSetumei;

    [SerializeField]
    private int SkillNumber;

    [SerializeField]
    private int SkillType;
    //1 物理攻撃 2 魔法攻撃 3 HP回復 4 MP回復 5 割合HP回復 6 割合MP回復 7 HPMP回復 8 HPMP割合回復 9 自分にバフをかける 10 敵にデバフをかける

    [SerializeField]
    private Sprite SkillSprite;

    [SerializeField]
    private int SkillZokusei;
    // 0 なし 1 炎 2 風 3 水 4 地 5 闇 6 光 7 雷

    [SerializeField]
    private int SkillIryoku;

    [SerializeField]
    private int TokusyuType;
    //1 剣を装備していると威力上昇 2 剣を装備していないと発動不可

    [SerializeField]
    private int TaiouParameter;

    [SerializeField]
    private int CanUseTown;
    //0 バトル中 1 いつでも使える

    [SerializeField]
    private int[] MeityuAndHissatu=new int[2];
    //0 命中 1 必殺

    [SerializeField]
    private int[] SkillSyouhi=new int[2];
    //0 HP消費 1 MP消費

    [SerializeField]
    private int AddBaffNum;

    //使用時に出るパーティクル
    [SerializeField]
    private GameObject Particle;

    //攻撃だとか使った時に出る効果音
    [SerializeField]
    private int SoundEffect;

    //ゲッター

    public string GetSkillName()
    {
        return SkillName;
    }

    public string GetSkillSetumei()
    {
        return SkillSetumei;
    }

    public int GetSkillNumber()
    {
        return SkillNumber;
    }

    public int GetSkillType()
    {
        return SkillType;
    }

    public Sprite GetSkillSprite()
    {
        return SkillSprite;
    }

    public int GetSkillIryoku()
    {
        return SkillIryoku;
    }

    public int GetTokusyuType()
    {
        return TokusyuType;
    }

    public int GetTaiouParameter()
    {
        return TaiouParameter;
    }

    public int GetCanUseTown()
    {
        return CanUseTown;
    }

    public int GetZokusei()
    {
        return SkillZokusei;
    }

    public int[] GetMeityuAndHissatu()
    {
        return MeityuAndHissatu;
    }

    public int[] GetSkillSyouhi()
    {
        return SkillSyouhi;
    }

    public int GetAddBaffNum()
    {
        return AddBaffNum;
    }

    public GameObject GetPartile()
    {
        return Particle;
    }

    public int GetSound()
    {
        return SoundEffect;
    }

}
