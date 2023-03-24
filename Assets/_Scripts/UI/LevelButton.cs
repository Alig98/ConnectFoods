using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    public int m_LevelNumber;
    public TextMeshProUGUI LevelText;
    public Button ButtonGraphics;
    public Color AvailableColor;
    public Color NotAvailableColor;
    public GameObject UnderConstructionText;

    private void Start()
    {
        RefreshUI();
    }

    private void RefreshUI()
    {
        LevelText.text = $"Level {m_LevelNumber}";

        if (m_LevelNumber <= Registry.CurrentLevel)
        {
            ButtonGraphics.image.color = AvailableColor;
            ButtonGraphics.interactable = true;
            UnderConstructionText.SetActive(false);
        }
        else if (m_LevelNumber > MainMenuManager.Instance.LevelInfos.LevelInfoList.Count)
        {
            ButtonGraphics.image.color = NotAvailableColor;
            UnderConstructionText.SetActive(true);
            ButtonGraphics.interactable = false;
        }
        else
        {
            UnderConstructionText.SetActive(false);
            ButtonGraphics.interactable = false;
            ButtonGraphics.image.color = NotAvailableColor;
        }
    }

    public void SetLevel(int level)
    {
        m_LevelNumber = level;
        RefreshUI();
    }

    public int GetLevel()
    {
        return m_LevelNumber;
    }

    public void OpenLevelContainer()
    {
        MainMenuManager.Instance.OpenLevelContainer(m_LevelNumber);
    }
}
