using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonBase<GameManager>
{
    public Transform GridStartPoint;
    public Transform GridEndPoint;
    public int RowCount;
    public int ColumnCount;

    private Tile m_SelectedTile;
    private List<Tile> m_SelectedTiles = new List<Tile>();

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            var origin = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            var hit = Physics2D.Raycast(origin, Vector2.zero);
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.layer ==(int) Layer.Tile)
                {
                    m_SelectedTile = hit.collider.GetComponent<Tile>();
                    m_SelectedTile.SetState(TileState.Selected);
                    
                    m_SelectedTiles.Add(m_SelectedTile);
                }
            }
        }

        if (Input.GetKey(KeyCode.Mouse0))
        {
            var origin = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            var hit = Physics2D.Raycast(origin, Vector2.zero);
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.layer ==(int) Layer.Tile)
                {
                    var hitTile = hit.collider.GetComponent<Tile>();

                    if (hitTile.TileType == m_SelectedTile.TileType && !m_SelectedTiles.Contains(hitTile)
                                                                    && m_SelectedTile.ControlIfNeighbour(hitTile))
                    {
                        hitTile.SetState(TileState.Selected);
                        m_SelectedTiles.Add(hitTile);

                        m_SelectedTile = hitTile;
                    }
                    if (m_SelectedTiles.Contains(hitTile))
                    {
                        var hitTileIndex = m_SelectedTiles.IndexOf(hitTile);
                        var selectedTilesCount = m_SelectedTiles.Count;

                        if (hitTileIndex == selectedTilesCount-1)
                        {
                            return;
                        }

                        for (int i = hitTileIndex+1; i < selectedTilesCount; i++)
                        {
                            m_SelectedTiles[hitTileIndex+1].SetState(TileState.Idle);
                            m_SelectedTiles.Remove(m_SelectedTiles[hitTileIndex+1]);
                        }
                        
                        m_SelectedTile = hitTile;
                    }
                }
            }
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            for (int i = 0; i < m_SelectedTiles.Count; i++)
            {
                m_SelectedTiles[i].SetState(m_SelectedTiles.Count>=3 ? TileState.Pop : TileState.Idle);
            }
                
            m_SelectedTiles.Clear();
            m_SelectedTile = null;
        }
    }
}

public enum Layer
{
    Tile = 6
}
