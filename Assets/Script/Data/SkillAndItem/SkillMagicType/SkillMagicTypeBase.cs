using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
[CreateAssetMenu(fileName = "SkillMagicTypeBase", menuName = "CreateData/CreateSkillMagic/CreateBase")]
public class SkillMagicTypeBase : ScriptableObject
{

    [SerializeField]
    private List<BaseSkillMagicType> SkillMagicTypeList = new List<BaseSkillMagicType>();

    //ゲッター

    public List<BaseSkillMagicType> GetSkillMagicTypeList()
    {
        return SkillMagicTypeList;
    }

}
