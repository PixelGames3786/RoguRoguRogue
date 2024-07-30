using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
[CreateAssetMenu(fileName = "SkillMagicType", menuName = "CreateData/CreateSkillMagic/CreateType")]
public class BaseSkillMagicType : ScriptableObject
{

    [SerializeField]
    private int[] SkillLevel;

    [SerializeField]
    private int[] SyutokuSkill;

    //ゲッター
    public int[] GetSkillLevel()
    {
        return SkillLevel;
    }

    public int[] GetSyutokuSkill()
    {
        return SkillLevel;
    }
}
