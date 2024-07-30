using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PlayerPrefsDeleteTool
{
    [MenuItem("Tools/PlayerPrefs/DeleteAll")]

    static void DeleteAll()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("Delete All Data Of PlayerPrefs!!");
    }
}