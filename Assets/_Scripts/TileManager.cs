using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : SingletonBase<TileManager>
{
    private Tile[,] m_AllTiles;
    private bool m_IsFirstUpdate;
    private List<Tile> m_StandByTiles = new List<Tile>();

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

        if (Input.GetKeyDown(KeyCode.R))
        {
            for (int i = 0; i < m_AllTiles.GetLength(0); i++)
            {
                for (int j = 0; j < m_AllTiles.GetLength(1); j++)
                {
                    m_AllTiles[i,j].GiveRandomFruit();
                }
            }
        }
        
        if (Input.GetKeyDown(KeyCode.F))
        {
            ControlForFall();
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            for (int i = m_AllTiles.GetLength(0)-1; i >= 0 ; i--)
            {
                for (int j = m_AllTiles.GetLength(1)-2; j >= 0; j--)
                {
                    Debug.Log($"i = {i} , j = {j} , {m_AllTiles[i,j]}");
                }
            }
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

    public void AddToStandByTiles(Tile tile)
    {
        m_StandByTiles.Add(tile);
        m_AllTiles[tile.TileIndex.x, tile.TileIndex.y] = null;
    }

    public void ControlForFall()
    {
        if (m_StandByTiles.Count > 0)
        {
            AdjustGrid();
        }
    }
    
    private void AdjustGrid()
    {
        foreach (var standByTile in m_StandByTiles)
        {
            ColumnFall(standByTile.TileIndex.x);
            m_AllTiles[standByTile.TileIndex.x, 0] = standByTile;
            standByTile.SetState(TileState.Fall);
        }
        m_StandByTiles.Clear();
        
        FindNeighbourTilesForAll();
    }

    private void ColumnFall(int x)
    {
        for (int i = m_AllTiles.GetLength(1)-2 ; i >= 0 ; i--)
        {
            if (m_AllTiles[x, i+1] == null)
            {
                if (!m_AllTiles[x,i])
                {
                    continue;
                }
                m_AllTiles[x,i].SetState(TileState.Fall);
                m_AllTiles[x, i+1] = m_AllTiles[x, i];
                m_AllTiles[x, i] = null;
            }
        }
    }
}