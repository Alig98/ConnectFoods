using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class LevelContainer : MonoBehaviour
{
    public TextMeshProUGUI LevelNumberText;
    public TextMeshProUGUI HighScoreText;
    public TextMeshProUGUI ObjectivesText;
    public bool IsOpen;

    public void UpdateInfos(LevelInfo info)
    {
        LevelNumberText.text = $"Level {info.LevelNumber}";

        var highScoreData = Registry.GetHighScoreDatas();
        if (highScoreData.Count >= info.LevelNumber)
        {
            HighScoreText.text = highScoreData[info.LevelNumber - 1].ToString();
        }
        else
        {
            HighScoreText.text = "0";
        }
        
        ObjectivesText.text = "";
        var objectives = info.TargetObjectives;
        for (int i = 0; i < objectives.Count; i++)
        {
            ObjectivesText.text +=
                $"{objectives[i].TargetCount}x <sprite=\"Foods\" index={(int)objectives[i].TargetType}>    ";
        }

        IsOpen = true;
    }

    public void CloseContainer()
    {
        IsOpen = false;
        transform.DOScale(Vector3.zero, .1f);
    }
}
