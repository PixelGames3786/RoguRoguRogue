using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
[CreateAssetMenu(fileName = "SkillDataBase", menuName = "CreateData/CreateSkill/SkillDataBase")]
public class SkillDataBase : ScriptableObject
{
    [SerializeField]
    private List<BaseSkillData> SkillDataList = new List<BaseSkillData>();

    public List<BaseSkillData> GetSkillDataBase()
    {
        return SkillDataList;
    }
}
