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
        var gridStartPoint = GameManager.Instance.GridStartPoint;
        var gridEndPoint = GameManager.Instance.GridEndPoint;
        var rowCount = GameManager.Instance.RowCount;
        var columnCount = GameManager.Instance.ColumnCount;
        
        var tileSizeForRow = Mathf.Abs((gridStartPoint.position.x - gridEndPoint.position.x)) / (rowCount-1);
        var tileSizeForColumn = Mathf.Abs((gridStartPoint.position.y - gridEndPoint.position.y)) / (columnCount-1);

        var tileSize = (tileSizeForColumn > tileSizeForRow ? tileSizeForRow : tileSizeForColumn);
        tileSize = tileSize > 1 ? 1 : tileSize;

        for (int i = 0; i < columnCount*2; i++)
        {
            for (int j = 0; j < rowCount; j++)
            {
                var tile = Instantiate(TilePrefab,TileParent);

                tile.transform.position = new Vector3(gridStartPoint.position.x+(j * tileSize)
                    , gridStartPoint.position.y+(columnCount*tileSize)-(i * tileSize), 0);
                tile.transform.localScale = Vector3.one * tileSize;
                tile.SetTileIndex(j,i);
                
                TileManager.Instance.AddToAllTiles(tile);
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