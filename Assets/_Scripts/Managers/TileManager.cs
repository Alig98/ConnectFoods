using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : SingletonBase<TileManager>
{
    //Fields
    private Tile[,] m_AllTiles;
    private bool m_IsFirstUpdate;
    private List<Tile> m_StandByTiles = new List<Tile>();
    private TileInfos m_TileInfos;
    
    //Unity Methods
    private void Update()
    {
        if (!m_IsFirstUpdate)
        {
            FindNeighbourTilesForAll();
            m_IsFirstUpdate = true;
            ControlForAnyMove();
        }
    }
    
    //Public Methods
    public void InitializeTileManager(int rowCount, int columnCount,TileInfos tileInfos)
    {
        m_AllTiles = new Tile[rowCount,columnCount];
        m_TileInfos = tileInfos;
    }

    public void AddToAllTiles(Tile tile, int x, int y, float tileSize)
    {
        m_AllTiles[x,y] = tile;
        
        tile.InitializeTile(x,y,tileSize);
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
        tile.ClearNeighbourTiles();
        
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

    public void AdjustGrid()
    {
        foreach (var standByTile in m_StandByTiles)
        {
            ColumnFall(standByTile.TileIndex.x);
            m_AllTiles[standByTile.TileIndex.x, 0] = standByTile;
            standByTile.SetState(TileState.Fall);
        }
        m_StandByTiles.Clear();
        
        FindNeighbourTilesForAll();
        
        Invoke(nameof(ControlForAnyMove),m_TileInfos.FallTime);
    }

    //Private Methods
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

    private void ControlForAnyMove()
    {
        for (int i = 0; i < m_AllTiles.GetLength(0); i++)
        {
            for (int j = 0; j < m_AllTiles.GetLength(1); j++)
            {
                List<Tile> visitedTiles = new List<Tile>();
                var control = BFS(m_AllTiles[i, j], 1,visitedTiles);
                if (control >= 3)
                {
                    return;
                }
            }
        }

        StartCoroutine(ShuffleTiles());
    }
    
    private int BFS(Tile currentTile,int count,List<Tile> visitedTiles)
    {
        if (count>=3) return count;

        visitedTiles.Add(currentTile);
        var firstNodeNeighbours = currentTile.NeighbourTiles;
        var targetFood = currentTile.TileType;
        
        for (int i = 0; i < firstNodeNeighbours.Count; i++)
        {
            if (firstNodeNeighbours[i].TileType == targetFood && !visitedTiles.Contains(firstNodeNeighbours[i]))
            {
                visitedTiles.Add(firstNodeNeighbours[i]);
                
                count = BFS(firstNodeNeighbours[i], count+1,visitedTiles);
            }
        }

        return count;
    }

    private IEnumerator ShuffleTiles()
    {
        for (int i = 0; i < m_AllTiles.GetLength(0); i++)
        {
            for (int j = 0; j < m_AllTiles.GetLength(1); j++)
            {
                m_AllTiles[i,j].SetState(TileState.Shuffle);
            }
        }

        yield return new WaitForSeconds(m_TileInfos.ShuffleTime);
        
        ControlForAnyMove();
    }
}