using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SingletonBase<GameManager>
{
    //Fields
    [SerializeField] private Transform m_GridStartPoint;
    [SerializeField] private Transform m_GridEndPoint;
    [SerializeField] private LevelInfos m_LevelInfos;
    [SerializeField] private TileInfos m_TileInfos;
    [SerializeField] private Transform m_EffectParent;
    private GameState m_GameState;
    private int m_Score;
    private int m_TotalMove;
    private List<TargetObjective> m_TargetObjectives = new List<TargetObjective>();

    //Properties
    public LevelInfo LevelInfo => m_LevelInfos.LevelInfoList[Registry.LoadedLevelIndex - 1];
    public GameState GameState => m_GameState;
    public Transform GridStartPoint => m_GridStartPoint;
    public TileInfos TileInfos => m_TileInfos;
    public Transform EffectParent => m_EffectParent;
    public int Score => m_Score;
    public int TotalMove => m_TotalMove;
    public List<TargetObjective> TargetObjectives => m_TargetObjectives;

    //Unity Methods
    protected override void Awake()
    {
        base.Awake();
        
        m_GameState = GameState.Playing;
        m_TotalMove = LevelInfo.TotalMove;
        
        for (int i = 0; i < LevelInfo.TargetObjectives.Count; i++)
        {
            TargetObjective objective = new TargetObjective();
            objective.TargetCount = LevelInfo.TargetObjectives[i].TargetCount;
            objective.TargetType = LevelInfo.TargetObjectives[i].TargetType;
            
            m_TargetObjectives.Add(objective);
        }
    }

    private void Start()
    {
        TileManager.Instance.InitializeTileManager(LevelInfo.RowCount,LevelInfo.ColumnCount,m_TileInfos);
        
        GridCreator.Instance.CreateGrid(m_GridStartPoint.position, m_GridEndPoint.position, LevelInfo.RowCount,
            LevelInfo.ColumnCount);

        Registry.GetHighScoreDatas();
    }

    private void OnEnable()
    {
        EventManager.MoveHasBeenMade.AddListener(OnMoveHasBeenMade);
    }

    private void OnDestroy()
    {
        EventManager.MoveHasBeenMade.RemoveListener(OnMoveHasBeenMade);
    }

    //Private Methods
    private void CheckGameState()
    {
        if (m_GameState != GameState.Playing) return;
        
        var targetsCount = 0;
        for (int i = 0; i < m_TargetObjectives.Count; i++)
        {
            targetsCount += m_TargetObjectives[i].TargetCount;
        }

        if (targetsCount <= 0)
        {
            LevelPassed();
        }
        else if (m_TotalMove <=0)
        {
            GameOver();
        }
    }

    private void OnMoveHasBeenMade(List<Tile> selectedTiles)
    {
        if (selectedTiles.Count <= 0) return;
        
        if (selectedTiles.Count >= 3)
        {
            for (int i = 0; i < selectedTiles.Count; i++)
            {
                var tile = selectedTiles[i];
                OnTilePop(tile);
            }
            
            m_TotalMove -= 1;
            TileManager.Instance.AdjustGrid();
            
            EventManager.StatisticsChanged.Invoke();
        }
        else
        {
            for (int i = 0; i < selectedTiles.Count; i++)
            {
                selectedTiles[i].SetState(TileState.Idle);
            }
        }

        CheckGameState();
    }

    private void OnTilePop(Tile tile)
    {
        m_Score += 1;

        for (int i = 0; i < m_TargetObjectives.Count; i++)
        {
            var targetObjective = m_TargetObjectives[i];
            if (tile.TileType == targetObjective.TargetType && targetObjective.TargetCount >0)
            {
                targetObjective.TargetCount -= 1;
            }
        }
        
        tile.SetState(TileState.Pop);
    }

    private void GameOver()
    {
        m_GameState = GameState.GameOver;

        Registry.SaveLastPlayedLevelDatas(LevelInfo.LevelNumber,0);
        Registry.ShowEndLevelSituation = true;
        Registry.Save();
        
        Invoke(nameof(LoadMainMenu),TileInfos.FallTime+.1f);
    }

    private void LevelPassed()
    {
        m_GameState = GameState.LevelPassed;

        var isHighScore = ControlHighScore();

        if (Registry.CurrentLevel <= LevelInfo.LevelNumber && Registry.CurrentLevel < m_LevelInfos.LevelInfoList.Count)
        {
            Registry.CurrentLevel = LevelInfo.LevelNumber+1;
        }
        Registry.SaveLastPlayedLevelDatas(LevelInfo.LevelNumber,1,isHighScore ? 1:0);
        Registry.ShowEndLevelSituation = true;
        Registry.Save();
        
        Invoke(nameof(LoadMainMenu),TileInfos.FallTime+.1f);
    }

    private bool ControlHighScore()
    {
        var levelNumber = LevelInfo.LevelNumber;
        var highScoreList = Registry.GetHighScoreDatas();

        if (highScoreList.Count >= levelNumber)
        {
            if (m_Score <= highScoreList[levelNumber-1])
            {
                return false;
            }
        }
        
        Registry.SaveHighScoreData(levelNumber,m_Score);
        Registry.Save();

        return true;
    }

    private void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}

//Enums
public enum GameState
{
    Playing,
    LevelPassed,
    GameOver
}

public enum Layer
{
    Tile = 6
}