using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : SingletonBase<TileManager>
{
    private List<Tile> m_AllTiles = new List<Tile>();
    private List<Tile> m_SeenTiles = new List<Tile>();
    private bool m_IsFirstUpdate;

    private void Update()
    {
        if (!m_IsFirstUpdate)
        {
            FindSeenTiles();
            FindNeighbourTilesForAll();
            m_IsFirstUpdate = true;
        }
    }

    public void AddToAllTiles(Tile tile)
    {
        m_AllTiles.Add(tile);
    }

    public void FindSeenTiles()
    {
        var gridStartPoint = GameManager.Instance.GridStartPoint;
        
        for (int i = 0; i < m_AllTiles.Count; i++)
        {
            if (m_AllTiles[i].transform.position.y <= gridStartPoint.position.y)
            {
                m_SeenTiles.Add(m_AllTiles[i]);
            }
        }
    }

    public void FindNeighbourTilesForAll()
    {
        for (int i = 0; i < m_AllTiles.Count; i++)
        {
            FindNeighbourTiles(m_AllTiles[i]);
        }
    }

    public void FindNeighbourTiles(Tile tile)
    {
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                var neighbourTile = GetTileFromIndex(tile.TileIndex.x + i, tile.TileIndex.y + j);

                if (!neighbourTile || (i == 0 && j == 0))
                {
                    continue;
                }
                
                tile.AddToNeighbourTiles(neighbourTile);
            }
        }
    }

    public Transform GetTileTransform(int x, int y)
    {
        for (int i = 0; i < m_AllTiles.Count; i++)
        {
            if (m_AllTiles[i].TileIndex.x == x && m_AllTiles[i].TileIndex.y == y)
            {
                return m_AllTiles[i].transform;
            }
        }

        return null;
    }

    public Tile GetTileFromIndex(int x, int y)
    {
        for (int i = 0; i < m_AllTiles.Count; i++)
        {
            if (m_AllTiles[i].TileIndex.x == x && m_AllTiles[i].TileIndex.y == y)
            {
                return m_AllTiles[i];
            }
        }

        return null;
    }
}
