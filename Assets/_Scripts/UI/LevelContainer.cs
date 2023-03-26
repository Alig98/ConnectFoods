using DG.Tweening;
using TMPro;
using UnityEngine;

public class LevelContainer : MonoBehaviour
{
    //Fields
    [SerializeField] private TextMeshProUGUI m_LevelNumberText;
    [SerializeField] private TextMeshProUGUI m_HighScoreText;
    [SerializeField] private TextMeshProUGUI m_ObjectivesText;
    private bool m_IsOpen;

    //Properties
    public bool IsOpen => m_IsOpen;

    //Public Methods
    public void UpdateInfos(LevelInfo info)
    {
        m_LevelNumberText.text = $"Level {info.LevelNumber}";

        var highScoreData = Registry.GetHighScoreDatas();
        if (highScoreData.Count >= info.LevelNumber)
        {
            m_HighScoreText.text = highScoreData[info.LevelNumber - 1].ToString();
        }
        else
        {
            m_HighScoreText.text = "0";
        }
        
        m_ObjectivesText.text = "";
        var objectives = info.TargetObjectives;
        for (int i = 0; i < objectives.Count; i++)
        {
            m_ObjectivesText.text +=
                $"{objectives[i].TargetCount}x <sprite=\"Foods\" index={(int)objectives[i].TargetType}>    ";
        }

        m_IsOpen = true;
    }

    public void CloseContainer()
    {
        m_IsOpen = false;
        transform.DOScale(Vector3.zero, .1f);
    }
}
