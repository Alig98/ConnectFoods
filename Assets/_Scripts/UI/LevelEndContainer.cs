using DG.Tweening;
using TMPro;
using UnityEngine;

public class LevelEndContainer : MonoBehaviour
{
    //Fields
    [SerializeField] private TextMeshProUGUI m_LevelText;
    [SerializeField] private TextMeshProUGUI m_NewHighScoreText;
    [SerializeField] private TextMeshProUGUI m_NewHighScoreAmountText;
    [SerializeField] private TextMeshProUGUI m_LevelSituationText;

    //Public Methods
    public void UpdateUI(int levelNumber,int levelPassed,int isHighScore)
    {
        m_LevelText.text = $"Level {levelNumber}";
        if (levelPassed == 1)
        {
            m_LevelSituationText.text = "Level Passed!";
            
            if (isHighScore == 1)
            {
                m_NewHighScoreText.gameObject.SetActive(true);

                var newHighScore = Registry.GetHighScoreDatas();
                m_NewHighScoreAmountText.text = newHighScore[levelNumber - 1].ToString();
                m_NewHighScoreAmountText.gameObject.SetActive(true);
            }
            else
            {
                m_NewHighScoreText.gameObject.SetActive(false);
                m_NewHighScoreAmountText.gameObject.SetActive(false);
            }
        }
        else
        {
            m_NewHighScoreText.gameObject.SetActive(false);
            m_NewHighScoreAmountText.gameObject.SetActive(false);
            m_LevelSituationText.text = "Level Failed!";
        }
    }

    public void CloseContainer()
    {
        transform.DOScale(Vector3.zero, .1f);
    }
}
