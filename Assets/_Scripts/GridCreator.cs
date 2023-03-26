using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCreator : SingletonBase<GridCreator>
{
    public Tile TilePrefab;
    public Transform TileParent;

    public void CreateGrid(Vector3 gridStartPoint,Vector3 gridEndPoint,int rowCount,int columnCount)
    {
        var tileSizeForRow = Mathf.Abs((gridStartPoint.x - gridEndPoint.x)) / (rowCount-1);
        var tileSizeForColumn = Mathf.Abs((gridStartPoint.y - gridEndPoint.y)) / (columnCount-1);

        var tileSize = Mathf.Min(tileSizeForColumn,tileSizeForRow);
        tileSize = Mathf.Clamp01(tileSize);

        for (int j = 0; j < columnCount; j++)
        {
            for (int i = 0; i < rowCount; i++)
            {
                var tile = Instantiate(TilePrefab,TileParent);
                
                TileManager.Instance.AddToAllTiles(tile,i,j,tileSize);
            }
        }

        if (gridStartPoint.x+(rowCount-1 * tileSize) < gridEndPoint.x)
        {
            var dif = (gridEndPoint.x - (gridStartPoint.x + ((rowCount-1) * tileSize)))*.5f;

            var pos = TileParent.position;
            pos.x += dif;

            TileParent.position = pos;
        }
    }
}