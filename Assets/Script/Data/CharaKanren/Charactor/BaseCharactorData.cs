using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class BaseCharactorData
{
    [SerializeField]
    public int[] KihonData = new int[5];

    [SerializeField]
    public int[] KihonParameter = new int[14];

    [SerializeField]
    public int[] JukurenParameter = new int[5];

    [SerializeField]
    public int[] NowSoubi = new int[3];

    [SerializeField]
    public int[] SkillAndMagic=new int[6];

    [SerializeField]
    public List<int> NowJoutai = new List<int>();

    [SerializeField]
    public List<int> JoutaiKeika = new List<int>();

    [SerializeField]
    public int[] BagItemInt = new int[9];

    [SerializeField]
    public int NowExp;

    [SerializeField]
    public int MagicType;

    [SerializeField]
    public int SkillType;

    [SerializeField]
    public int SkillSyutoku;

    [SerializeField]
    public int MagicSyutoku;

    //今の所持金
    [SerializeField]
    public int NowSyojikin;

    //今の居場所　データをロードするときとかにつかう
    [SerializeField]
    public int NowIbasyo;

    //セッター

    public void SetKihonData(int[] value)
    {
        KihonData = value;
    }

    public void SetKihonParameter(int[] value)
    {
        KihonParameter = value;
    }

    public void SetJukurenParameter(int[] value)
    {
        JukurenParameter = value;
    }

    public void SetSkillMagic(int[] value)
    {
        SkillAndMagic = value;
    }

    public void SetNowJoutai(List<int> value)
    {
        NowJoutai = value;
    }

    public void SetMagicType(int value)
    {
        MagicType = value;
    }

    public void SetSkillType(int value)
    {
        SkillType = value;
    }

    public void SetSyojikin(int value)
    {
        NowSyojikin = value;
    }

    public void SetKihonDataKobetu(int value, int Number)
    {
        KihonData[Number] = value;
    }

    public void SetKihonParameterKobetu(int value, int Number)
    {
        KihonParameter[Number] = value;
    }

    public void SetNowExp(int value)
    {
        NowExp = value;
    }

    public void SetNowSoubi(int[] value)
    {
        NowSoubi = value;
    }

    public void SetNowIbasyo(int value)
    {
        NowIbasyo = value;
    }

    public void SetSkillSyutoku(int value)
    {
        SkillSyutoku = value;
    }

    public void SetMagicSyutoku(int value)
    {
        MagicSyutoku = value;
    }

    //ゲッター

    public int[] GetKihonData()
    {
        return KihonData;
    }

    public int[] GetParameter()
    {
        return KihonParameter;
    }

    public int[] GetJukurenParameter()
    {
        return JukurenParameter;
    }

    public int[] GetSkillMagic()
    {
        return SkillAndMagic;
    }

    public List<int> GetNowJoutai()
    {
        return NowJoutai;
    }

    public int GetNowExp()
    {
        return NowExp;
    }

    public int GetMagicType()
    {
        return MagicType;
    }

    public int GetSkillType()
    {
        return SkillType;
    }

    public int GetSyojikin()
    {
        return NowSyojikin;
    }

    public int[] GetNowSoubi()
    {
        return NowSoubi;
    }

    public int GetNowIbasyo()
    {
        return NowIbasyo;
    }

    public int GetSkillSyutoku()
    {
        return SkillSyutoku;
    }

    public int GetMagicSyutoku()
    {
        return MagicSyutoku;
    }
}
