using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GUIManager : MonoBehaviour
{
    //Fields
    [SerializeField] private LevelInfos m_LevelInfos;
    [SerializeField] private TextMeshProUGUI m_ObjectivesText;
    [SerializeField] private TextMeshProUGUI m_ScoreText;
    [SerializeField] private TextMeshProUGUI m_TotalMovesText;

    private int m_Score;
    private int m_TotalMove;
    private List<TargetObjective> m_TargetObjectives = new List<TargetObjective>();

    public LevelInfo LevelInfo => m_LevelInfos.LevelInfoList[Registry.LoadedLevelIndex - 1];
    
    //Unity Methods
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
    
    //Private Methods
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
        m_TotalMovesText.text = m_TotalMove.ToString();
        m_ScoreText.text = m_Score.ToString();

        m_ObjectivesText.text = "";

        for (int i = 0; i < m_TargetObjectives.Count; i++)
        {
            m_ObjectivesText.text +=
                $"{m_TargetObjectives[i].TargetCount}x <sprite=\"Foods\" index={(int)m_TargetObjectives[i].TargetType}>    ";
        }
    }
}
