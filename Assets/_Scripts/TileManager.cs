using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : SingletonBase<TileManager>
{
    private Tile[,] m_AllTiles;
    private bool m_IsFirstUpdate;
    
    private void Start()
    {
        var gameManager = GameManager.Instance;
        m_AllTiles = new Tile[gameManager.RowCount, gameManager.ColumnCount];
    }

    private void Update()
    {
        if (!m_IsFirstUpdate)
        {
            FindNeighbourTilesForAll();
            m_IsFirstUpdate = true;
        }
    }

    public void AddToAllTiles(Tile tile, int x, int y)
    {
        m_AllTiles[x,y] = tile;
    }
    
    public void FindNeighbourTilesForAll()
    {
        for (int i = 0; i < m_AllTiles.GetLength(0); i++)
        {
            for (int j = 0; j < m_AllTiles.GetLength(1); j++)
            {
                FindNeighbourTiles(m_AllTiles[i,j]);
            }
        }
    }

    public void FindNeighbourTiles(Tile tile)
    {
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                if ( (i == 0 && j == 0)) continue;
                
                var neighbourTile = GetTileFromIndex(tile.TileIndex.x + i, tile.TileIndex.y + j);

                if (neighbourTile)
                {
                    tile.AddToNeighbourTiles(neighbourTile);
                }
            }
        }
    }
    
    public Tile GetTileFromIndex(int x, int y)
    {
        if (x<0 || y<0 || x> m_AllTiles.GetLength(0)-1 || y> m_AllTiles.GetLength(1)-1)
        {
            return null;
        }
        
        return m_AllTiles[x, y];
    }
    
}
