using UnityEditor;
using UnityEngine;

public static class ResetPrefs
{
    [MenuItem("Tools/Reset PlayerPrefs")]
    public static void Reset()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("PlayerPrefs cleared");
    }
}
