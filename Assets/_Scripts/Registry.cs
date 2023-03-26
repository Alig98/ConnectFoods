using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public static class Registry
{
    //Fields
    private static readonly string m_CurrentLevelKey = "CurrentLevelKey";
    private static readonly string m_LoadedLevelIndexKey = "LoadedLevelIndexKey";
    private static readonly string m_ShowEndLevelSituationKey = "ShowEndLevelSituation";
    private static readonly string m_LastPlayedLevelDatas = "LastPlayedLevelDatas";
    private static readonly string m_HighScoreDatas = "HighScoreDatas";

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

    public static bool ShowEndLevelSituation
    {
        set => PlayerPrefs.SetInt(m_ShowEndLevelSituationKey, value ? 1:0);
        get => PlayerPrefs.GetInt(m_ShowEndLevelSituationKey) > 0;
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

    public static void SaveHighScoreData(int levelNumber,int score)
    {
        var dataList = GetHighScoreDatas();

        if (dataList.Count == 0 || CurrentLevel == GameManager.Instance.LevelInfo.LevelNumber)
        {
            dataList.Add(score);
        }
        else
        {
            dataList[levelNumber - 1] = score;
        }
        
        StringBuilder stringBuilder = new StringBuilder();

        for (int i = 0; i < dataList.Count; i++)
        {
            stringBuilder.Append(dataList[i]).Append(",");
        }
        
        PlayerPrefs.SetString(m_HighScoreDatas,stringBuilder.ToString());
    }

    public static List<int> GetHighScoreDatas()
    {
        var data = PlayerPrefs.GetString(m_HighScoreDatas);
        List<int> dataList = new List<int>();

        if (!String.IsNullOrEmpty(data))
        {
            var datas = data.Split(",");
            for (int i = 0; i < datas.Length; i++)
            {
                if (int.TryParse(datas[i], out var highScoreData))
                {
                    dataList.Add(highScoreData);
                }
            }
        }

        return dataList;
    }
    
    public static void SaveLastPlayedLevelDatas(int levelNumber, int isLevelPassed, int isHighScore = 0)
    {
        StringBuilder stringBuilder = new StringBuilder();

        stringBuilder.Append(levelNumber).Append(",").Append(isLevelPassed).Append(",").Append(isHighScore);
        
        PlayerPrefs.SetString(m_LastPlayedLevelDatas,stringBuilder.ToString());
    }

    public static List<int> GetLastPlayedLevelDatas()
    {
        var data = PlayerPrefs.GetString(m_LastPlayedLevelDatas);
        List<int> dataList = new List<int>();

        if (!String.IsNullOrEmpty(data))
        {
            var datas = data.Split(",");
            for (int i = 0; i < datas.Length; i++)
            {
                if (int.TryParse(datas[i], out var d))
                {
                    dataList.Add(d);
                }
            }
        }

        return dataList;
    }
}
