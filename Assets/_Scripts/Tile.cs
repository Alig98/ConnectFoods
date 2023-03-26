using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class Tile : MonoBehaviour
{
    //Fields
    [SerializeField] private SpriteRenderer m_ObjectModel;
    [SerializeField] private SpriteRenderer m_TileBG;
    [SerializeField] private SpriteRenderer m_TileModel;
    private TileState m_TileState;
    private TileTypes m_TileType;
    private Vector3 m_StartScale;
    private List<Tile> m_NeighbourTiles = new List<Tile>();
    private Vector2Int m_TileIndex;
    private TileInfos m_TileInfos;
    private ParticleSystem m_PopEffect;
    private int m_ObjectModelDefaultSortingOrder;
    private int m_TileBGDefaultSortingOrder;
    private int m_TileodelDefaultSortingOrder;

    //Properties
    public TileTypes TileType => m_TileType;
    public TileState TileState => m_TileState;
    public Vector2Int TileIndex => m_TileIndex;
    public List<Tile> NeighbourTiles => m_NeighbourTiles;

    //Public Methods
    public void InitializeTile(int x, int y, float tileSize)
    {
        transform.localScale = Vector3.one * tileSize;
        SetTileIndex(x,y);
        
        m_TileInfos = GameManager.Instance.TileInfos;
        GiveRandomFruit();
        m_TileState = TileState.Idle;
        m_StartScale = transform.localScale;

        m_ObjectModelDefaultSortingOrder = m_ObjectModel.sortingOrder;
        m_TileBGDefaultSortingOrder = m_TileBG.sortingOrder;
        m_TileodelDefaultSortingOrder = m_TileModel.sortingOrder;
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

    public void ClearNeighbourTiles()
    {
        m_NeighbourTiles.Clear();
    }

    public bool ControlIfNeighbour(Tile tile)
    {
        if (m_NeighbourTiles.Contains(tile))
        {
            return true;
        }

        return false;
    }

    //Private Methods
    private void ShuffleTile()
    {
        var startPosition = transform.position;

        transform.DOMove(Vector3.zero, m_TileInfos.ShuffleMoveTime);
        transform.DOShakeRotation(m_TileInfos.ShuffleRotationTime);
        transform.DOMove(startPosition, m_TileInfos.ShuffleMoveTime)
            .SetDelay(m_TileInfos.ShuffleRotationTime-m_TileInfos.ShuffleMoveTime);
        transform.DORotate(Vector3.zero, m_TileInfos.ShuffleMoveTime)
            .SetDelay(m_TileInfos.ShuffleRotationTime).OnComplete((() =>
            {
                SetState(TileState.Idle);
            }));
        
        GiveRandomFruit();
    }
    
    private void SetTileIndex(int x, int y, bool useTween = false)
    {
        m_TileIndex = new Vector2Int(x, y);
        var gridStartPoint = GameManager.Instance.GridStartPoint;

        var targetPos = new Vector3(gridStartPoint.position.x + (x * transform.localScale.x)
            , gridStartPoint.position.y - (y * transform.localScale.x), 0);
        if (useTween)
        {
            transform.DOKill();
            transform.DOMove(targetPos,m_TileInfos.FallTime);
        }
        else
        {
            transform.position = targetPos;
        }
    }
    
    private void GiveRandomFruit()
    {
        var tileTypeSprites = m_TileInfos.TileSprites;
        var tileTypeIndex = Random.Range(0, tileTypeSprites.Count);
        m_TileType = (TileTypes) tileTypeIndex;
        m_ObjectModel.sprite = tileTypeSprites[tileTypeIndex];
    }
    
    private void TileStateChanged()
    {
        switch (m_TileState)
        {
            case TileState.Idle:
                transform.localScale = m_StartScale;
                SetTileIdle();
                break;
            case TileState.Selected:
                transform.localScale *= 1.15f;
                SetTileSelected();
                break;
            case TileState.Fall:
                SetTileIndex(m_TileIndex.x,m_TileIndex.y+1,true);
                SetState(TileState.Idle);
                break;
            case TileState.Pop:
                transform.localScale = m_StartScale;
                EventManager.TilePopEvent.Invoke(this);
                PlayPopEffect();
                SetState(TileState.StandBy);
                break;
            case TileState.StandBy:
                TileManager.Instance.AddToStandByTiles(this);
                SetTileIndex(m_TileIndex.x,-1);
                GiveRandomFruit();
                break;
            case TileState.Shuffle:
                ShuffleTile();
                break;
        }
    }

    private void PlayPopEffect()
    {
        if (m_PopEffect == null)
        {
            m_PopEffect = Instantiate(m_TileInfos.PopParticle);
            m_PopEffect.transform.localScale = transform.localScale;
            m_PopEffect.transform.parent = GameManager.Instance.EffectParent;
        }

        m_PopEffect.transform.position = transform.position;
        
        m_PopEffect.Play();
    }
    
    private void SetTileSelected()
    {
        m_TileBG.gameObject.SetActive(true);
        m_ObjectModel.sortingOrder =m_ObjectModelDefaultSortingOrder+10;
        m_TileBG.sortingOrder = m_TileBGDefaultSortingOrder+10;
        m_TileModel.sortingOrder = m_TileodelDefaultSortingOrder+10;
    }

    private void SetTileIdle()
    {
        m_TileBG.gameObject.SetActive(false);
        m_ObjectModel.sortingOrder = m_ObjectModelDefaultSortingOrder;
        m_TileBG.sortingOrder = m_TileBGDefaultSortingOrder;
        m_TileModel.sortingOrder = m_TileodelDefaultSortingOrder;
    }
}

//Enums
public enum TileTypes
{
    Pumpkin = 0,
    Banana = 1,
    Apple = 2,
    Berry = 3,
    DragonFruit = 4,
}

public enum TileState
{
    Idle,
    Selected,
    Fall,
    Pop,
    StandBy,
    Shuffle
}