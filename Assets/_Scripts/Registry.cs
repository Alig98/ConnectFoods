using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public static class Registry
{
    //Fields
    private static readonly string m_CurrentLevelKey = "CurrentLevelKey";
    private static readonly string m_LoadedLevelIndexKey = "LoadedLevelIndexKey";

    //Properties
    public static int CurrentLevel
    {
        set => PlayerPrefs.SetInt(m_CurrentLevelKey, value);
        get => PlayerPrefs.GetInt(m_CurrentLevelKey);
    }
    
    public static int LoadedLevelIndex
    {
        set => PlayerPrefs.SetInt(m_LoadedLevelIndexKey, value);
        get => PlayerPrefs.GetInt(m_LoadedLevelIndexKey);
    }

    //Public methods
    public static void Save()
    {
        PlayerPrefs.Save();
    }

    public static void ClearRegistry()
    {
        PlayerPrefs.DeleteAll();
    }
}
