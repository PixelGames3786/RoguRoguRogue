using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
[CreateAssetMenu(fileName = "GyousyouData", menuName = "CreateData/CreateDangeon/GyousyouData")]
public class BaseGyousyouData : ScriptableObject
{
    [SerializeField]
    public List<int> Syouhin;

    [SerializeField]
    public TextAsset SyouninKaiwa;

    [SerializeField]
    public int[] KaiwaGyousu;
}
