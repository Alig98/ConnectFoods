using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    //Fields
    private int m_LevelNumber;
    
    [SerializeField] private TextMeshProUGUI m_LevelText;
    [SerializeField] private Button m_ButtonGraphics;
    [SerializeField] private Color m_AvailableColor;
    [SerializeField] private Color m_NotAvailableColor;
    [SerializeField] private GameObject m_UnderConstructionText;

    //Unity Methods
    private void Start()
    {
        RefreshUI();
    }

    //Private Methods
    private void RefreshUI()
    {
        m_LevelText.text = $"Level {m_LevelNumber}";

        if (m_LevelNumber <= Registry.CurrentLevel)
        {
            m_ButtonGraphics.image.color = m_AvailableColor;
            m_ButtonGraphics.interactable = true;
            m_UnderConstructionText.SetActive(false);
        }
        else if (m_LevelNumber > MainMenuManager.Instance.LevelInfos.LevelInfoList.Count)
        {
            m_ButtonGraphics.image.color = m_NotAvailableColor;
            m_UnderConstructionText.SetActive(true);
            m_ButtonGraphics.interactable = false;
        }
        else
        {
            m_UnderConstructionText.SetActive(false);
            m_ButtonGraphics.interactable = false;
            m_ButtonGraphics.image.color = m_NotAvailableColor;
        }
    }

    //Public Methods
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
