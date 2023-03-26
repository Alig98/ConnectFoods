using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class LevelEndContainer : MonoBehaviour
{
    public TextMeshProUGUI LevelText;
    public TextMeshProUGUI NewHighScoreText;
    public TextMeshProUGUI NewHighScoreAmountText;
    public TextMeshProUGUI LevelSituationText;

    public void UpdateUI(int levelNumber,int levelPassed,int isHighScore)
    {
        LevelText.text = $"Level {levelNumber}";
        if (levelPassed == 1)
        {
            LevelSituationText.text = "Level Passed!";
            
            if (isHighScore == 1)
            {
                NewHighScoreText.gameObject.SetActive(true);

                var newHighScore = Registry.GetHighScoreDatas();
                NewHighScoreAmountText.text = newHighScore[levelNumber - 1].ToString();
                NewHighScoreAmountText.gameObject.SetActive(true);
            }
            else
            {
                NewHighScoreText.gameObject.SetActive(false);
                NewHighScoreAmountText.gameObject.SetActive(false);
            }
        }
        else
        {
            NewHighScoreText.gameObject.SetActive(false);
            NewHighScoreAmountText.gameObject.SetActive(false);
            LevelSituationText.text = "Level Failed!";
        }
    }

    public void CloseContainer()
    {
        transform.DOScale(Vector3.zero, .1f);
    }
}
