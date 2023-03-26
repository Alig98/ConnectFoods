using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GUIManager : MonoBehaviour
{
    public LevelInfos LevelInfos;
    public TextMeshProUGUI ObjectivesText;
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI TotalMovesText;

    private int m_Score;
    private int m_TotalMove;
    private List<TargetObjective> m_TargetObjectives = new List<TargetObjective>();

    public LevelInfo LevelInfo => LevelInfos.LevelInfoList[Registry.LoadedLevelIndex - 1];
    
    private void Awake()
    {
        m_TotalMove = LevelInfo.TotalMove;
        
        for (int i = 0; i < LevelInfo.TargetObjectives.Count; i++)
        {
            TargetObjective objective = new TargetObjective();
            objective.TargetCount = LevelInfo.TargetObjectives[i].TargetCount;
            objective.TargetType = LevelInfo.TargetObjectives[i].TargetType;
            
            m_TargetObjectives.Add(objective);
        }
        
        UpdateUI();
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
    
    private void OnMoveHasBeenMade()
    {
        m_TotalMove -= 1;
        UpdateUI();
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
        
        UpdateUI();
    }

    private void UpdateUI()
    {
        TotalMovesText.text = m_TotalMove.ToString();
        ScoreText.text = m_Score.ToString();

        ObjectivesText.text = "";

        for (int i = 0; i < m_TargetObjectives.Count; i++)
        {
            ObjectivesText.text +=
                $"{m_TargetObjectives[i].TargetCount}x <sprite=\"Foods\" index={(int)m_TargetObjectives[i].TargetType}>    ";
        }
    }
}
