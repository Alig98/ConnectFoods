using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonBase<GameManager>
{
    public Transform GridStartPoint;
    public Transform GridEndPoint;
    public int RowCount;
    public int ColumnCount;
    public LevelInfos LevelInfos;
    public TileInfos TileInfos;
}

public enum Layer
{
    Tile = 6
}