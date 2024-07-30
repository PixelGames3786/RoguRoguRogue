using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Diagnostics;

public class EditorRestartUnity
{
    [MenuItem("File/Restart")]
    static void RestartUnity()
    {
        // •Ê‚ÌUnity‚ğ‹N“®‚µ‚½‚ ‚Æ‚É©g‚ğI—¹
        Process.Start(EditorApplication.applicationPath);
        EditorApplication.Exit(0);
    }
}