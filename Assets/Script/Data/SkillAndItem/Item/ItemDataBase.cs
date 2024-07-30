using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
[CreateAssetMenu(fileName = "ItemDataBase", menuName = "CreateData/CreateItem/ItemDataBase")]
public class ItemDataBase : ScriptableObject
{
    [SerializeField]
    private List<BaseItemData> ItemDataList = new List<BaseItemData>();
    
    public List<BaseItemData> GetItemDataBase()
    {
        return ItemDataList;
    }

    public void SetItemDataBase(List<BaseItemData> value)
    {
        ItemDataList = value;
    }
}
