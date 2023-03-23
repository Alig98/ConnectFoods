using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCreator : MonoBehaviour
{
    public Tile TilePrefab;
    public Transform TileParent;

    private void Start()
    {
        CreateGrid();
    }

    private void CreateGrid()
    {
        var gameManager = GameManager.Instance;
        
        var gridStartPoint = gameManager.GridStartPoint;
        var gridEndPoint = gameManager.GridEndPoint;
        var rowCount = gameManager.RowCount;
        var columnCount = gameManager.ColumnCount;
        
        var tileSizeForRow = Mathf.Abs((gridStartPoint.position.x - gridEndPoint.position.x)) / (rowCount-1);
        var tileSizeForColumn = Mathf.Abs((gridStartPoint.position.y - gridEndPoint.position.y)) / (columnCount-1);

        var tileSize = Mathf.Min(tileSizeForColumn,tileSizeForRow);
        tileSize = Mathf.Clamp01(tileSize);

        for (int j = 0; j < columnCount; j++)
        {
            for (int i = 0; i < rowCount; i++)
            {
                var tile = Instantiate(TilePrefab,TileParent);

                tile.InitializeTile(i,j,tileSize);
                
                TileManager.Instance.AddToAllTiles(tile,i,j);
            }
        }

        if (gridStartPoint.position.x+(rowCount-1 * tileSize) < gridEndPoint.position.x)
        {
            var dif = (gridEndPoint.position.x - (gridStartPoint.position.x + ((rowCount-1) * tileSize)))*.5f;

            var pos = TileParent.position;
            pos.x += dif;

            TileParent.position = pos;
        }
    }
}