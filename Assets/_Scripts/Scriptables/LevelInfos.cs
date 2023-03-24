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
}

[System.Serializable]
public class TargetObjective
{
    public TileTypes TargetType;
    public int TargetCount;
}
