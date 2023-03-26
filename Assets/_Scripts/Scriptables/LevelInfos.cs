using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class LevelInfos : ScriptableObject
{
    public List<LevelInfo> LevelInfoList;
}

[System.Serializable]
public class LevelInfo
{
    public int LevelNumber;
    public int TotalMove;
    public int RowCount;
    public int ColumnCount;
    public List<TargetObjective> TargetObjectives;
    public String m_IsCompleted;

    public void SetLevelSituation(bool value)
    {
        m_IsCompleted = $"{value}";
    }
}

[System.Serializable]
public class TargetObjective
{
    public TileTypes TargetType;
    public int TargetCount;
}
