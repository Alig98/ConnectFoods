using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SingletonBase<GameManager>
{
    public Transform GridStartPoint;
    public Transform GridEndPoint;
    public LevelInfos LevelInfos;
    public TileInfos TileInfos;

    private GameState m_GameState;
    private int m_Score;
    private int m_TotalMove;
    private List<TargetObjective> m_TargetObjectives = new List<TargetObjective>();

    public LevelInfo LevelInfo => LevelInfos.LevelInfoList[Registry.LoadedLevelIndex - 1];

    public GameState GameState => m_GameState;

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
        TileManager.Instance.InitializeTileManager(LevelInfo.RowCount,LevelInfo.ColumnCount,TileInfos);
        
        GridCreator.Instance.CreateGrid(GridStartPoint.position, GridEndPoint.position, LevelInfo.RowCount,
            LevelInfo.ColumnCount);

        Registry.GetHighScoreDatas();
    }

    private void OnEnable()
    {
        EventManager.MoveHasBeenMade.AddListener(OnMoveHasBeenMade);
        EventManager.TilePopEvent.AddListener(OnTilePop);
    }

    private void OnDestroy()
    {
        EventManager.MoveHasBeenMade.RemoveListener(OnMoveHasBeenMade);
        EventManager.TilePopEvent.RemoveListener(OnTilePop);
    }

    //todo:
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

    private void OnMoveHasBeenMade()
    {
        TileManager.Instance.AdjustGrid();

        m_TotalMove -= 1;

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
        
        CheckGameState();
    }

    private void GameOver()
    {
        m_GameState = GameState.GameOver;

        Registry.SaveLastPlayedLevelDatas(LevelInfo.LevelNumber,0);
        Registry.ShowEndLevelSituation = true;
        Registry.Save();
        
        Invoke(nameof(LoadMainMenu),1);
    }

    private void LevelPassed()
    {
        m_GameState = GameState.LevelPassed;

        var isHighScore = ControlHighScore();

        if (Registry.CurrentLevel <= LevelInfo.LevelNumber && Registry.CurrentLevel < LevelInfos.LevelInfoList.Count)
        {
            Registry.CurrentLevel = LevelInfo.LevelNumber+1;
        }
        Registry.SaveLastPlayedLevelDatas(LevelInfo.LevelNumber,1,isHighScore ? 1:0);
        Registry.ShowEndLevelSituation = true;
        Registry.Save();
        
        Invoke(nameof(LoadMainMenu),1);
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