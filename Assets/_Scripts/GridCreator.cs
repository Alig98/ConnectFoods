using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCreator : MonoBehaviour
{
    public int RowCount;
    public int ColumnCount;
    public Transform GridStartPoint;
    public Transform GridEndPoint;
    public Tile TilePrefab;
    public Transform TileParent;

    private void Start()
    {
        CreateGrid();
    }

    private void CreateGrid()
    {
        var tileSizeForRow = Mathf.Abs((GridStartPoint.position.x - GridEndPoint.position.x)) / (RowCount-1);
        var tileSizeForColumn = Mathf.Abs((GridStartPoint.position.y - GridEndPoint.position.y)) / (ColumnCount-1);

        var tileSize = (tileSizeForColumn > tileSizeForRow ? tileSizeForRow : tileSizeForColumn);
        tileSize = tileSize > 1 ? 1 : tileSize;

        for (int i = 0; i < ColumnCount*2; i++)
        {
            for (int j = 0; j < RowCount; j++)
            {
                var tile = Instantiate(TilePrefab,TileParent);

                tile.transform.position = new Vector3(GridStartPoint.position.x+(j * tileSize)
                    , GridStartPoint.position.y+(ColumnCount*tileSize)-(i * tileSize), 0);
                tile.transform.localScale = Vector3.one * tileSize;
            }
        }

        if (GridStartPoint.position.x+(RowCount-1 * tileSize) < GridEndPoint.position.x)
        {
            var dif = (GridEndPoint.position.x - (GridStartPoint.position.x + ((RowCount-1) * tileSize)))*.5f;

            var pos = TileParent.position;
            pos.x += dif;

            TileParent.position = pos;
        }
    }
}