using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GUIManager : MonoBehaviour
{
    //Fields
    [SerializeField] private TextMeshProUGUI m_ObjectivesText;
    [SerializeField] private TextMeshProUGUI m_ScoreText;
    [SerializeField] private TextMeshProUGUI m_TotalMovesText;
    
    //Unity Methods
    private void Start()
    {
        UpdateUI();
    }
    
    //Private Methods
    private void OnEnable()
    {
        EventManager.StatisticsChanged.AddListener(OnStatisticsChanged);
    }

    private void OnDestroy()
    {
        EventManager.StatisticsChanged.RemoveListener(OnStatisticsChanged);
    }

    private void OnStatisticsChanged()
    {
        UpdateUI();
    }
    
    private void UpdateUI()
    {
        var gameManager = GameManager.Instance;
        var totalMove = gameManager.TotalMove;
        var score = gameManager.Score;
        var targetObjectives = gameManager.TargetObjectives;
        
        m_TotalMovesText.text = totalMove.ToString();
        m_ScoreText.text = score.ToString();

        m_ObjectivesText.text = "";

        for (int i = 0; i < targetObjectives.Count; i++)
        {
            m_ObjectivesText.text +=
                $"{targetObjectives[i].TargetCount}x <sprite=\"Foods\" index={(int)targetObjectives[i].TargetType}>    ";
        }
    }
}
