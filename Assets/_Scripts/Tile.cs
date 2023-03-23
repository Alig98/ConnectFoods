using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class Tile : MonoBehaviour
{
    public List<Sprite> TileTypeSprites;
    public SpriteRenderer ObjectModel;
    private TileState m_TileState;
    private TileTypes m_TileType;
    private Vector3 m_StartScale;
    private List<Tile> m_NeighbourTiles = new List<Tile>();
    private Vector2Int m_TileIndex;

    public TileTypes TileType => m_TileType;
    public TileState TileState => m_TileState;
    public Vector2Int TileIndex => m_TileIndex;

    private void Start()
    {
        GiveRandomFruit();
        m_TileState = TileState.Idle;
        m_StartScale = transform.localScale;
    }

    public void GiveRandomFruit()
    {
        var tileTypeIndex = Random.Range(0, TileTypeSprites.Count);
        m_TileType = (TileTypes) tileTypeIndex;
        ObjectModel.sprite = TileTypeSprites[tileTypeIndex];
    }

    private void TileStateChanged()
    {
        switch (m_TileState)
        {
            case TileState.Idle:
                transform.localScale = m_StartScale;
                break;
            case TileState.Selected:
                transform.localScale *= 1.2f;
                break;
            case TileState.Fall:
                SetTileIndex(m_TileIndex.x,m_TileIndex.y+1,true);
                SetState(TileState.Idle);
                break;
            case TileState.Pop:
                transform.localScale = m_StartScale;
                SetState(TileState.StandBy);
                break;
            case TileState.StandBy:
                TileManager.Instance.AddToStandByTiles(this);
                SetTileIndex(m_TileIndex.x,-1);
                GiveRandomFruit();
                break;
        }
    }

    public void InitializeTile(int x, int y, float tileSize)
    {
        transform.localScale = Vector3.one * tileSize;
        SetTileIndex(x,y);
    }

    public void SetState(TileState state)
    {
        m_TileState = state;
        TileStateChanged();
    }

    public void AddToNeighbourTiles(Tile tile)
    {
        m_NeighbourTiles.Add(tile);
    }

    public void SetTileIndex(int x, int y, bool useTween = false)
    {
        m_TileIndex = new Vector2Int(x, y);
        var gridStartPoint = GameManager.Instance.GridStartPoint;

        var targetPos = new Vector3(gridStartPoint.position.x + (x * transform.localScale.x)
            , gridStartPoint.position.y - (y * transform.localScale.x), 0);
        if (useTween)
        {
            transform.DOKill();
            transform.DOMove(targetPos,.2f);
        }
        else
        {
            transform.position = targetPos;
        }
    }

    public bool ControlIfNeighbour(Tile tile)
    {
        if (m_NeighbourTiles.Contains(tile))
        {
            return true;
        }

        return false;
    }
}
public enum TileTypes
{
    Pumpkin = 0,
    Banana = 1,
    Apple = 2,
    Berry = 3,
    DragonFruit = 4
}

public enum TileState
{
    Idle,
    Selected,
    Fall,
    Pop,
    StandBy
}