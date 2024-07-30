using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
[CreateAssetMenu(fileName = "DangeonData", menuName = "CreateData/CreateDangeon/DangeonData")]
public class BaseDangeonData : ScriptableObject
{
    [SerializeField]
    public BaseDangeonKaisouData[] KaisouList;

    [SerializeField]
    public int DangeonNumber;

    [SerializeField]
    public string DangeonName;

    [SerializeField]
    public TextAsset DangeonText;

    [SerializeField]
    public int BattleBGM;
}
