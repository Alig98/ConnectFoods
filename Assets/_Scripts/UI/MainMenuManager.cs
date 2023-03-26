using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : SingletonBase<MainMenuManager>
{
    //Fields
    [SerializeField] private GameObject m_LevelsContainer;
    [SerializeField] private LevelInfos m_LevelInfos;
    [SerializeField] private LevelContainer m_LevelContainer;
    [SerializeField] private LevelEndContainer m_LevelEndContainer;
    [SerializeField] private InfiniteScrollView m_InfiniteScrollView;
    private int m_SelectedLevel;

    //Properties
    public LevelInfos LevelInfos => m_LevelInfos;

    //Override Methods
    protected override void Awake()
    {
        base.Awake();
        if (Registry.CurrentLevel <= 0)
        {
            Registry.CurrentLevel = 1;
            Registry.Save();
        }

        if (Registry.ShowEndLevelSituation)
        {
            ShowEndLevelSituation();
        }
    }
    
    //Private Methods
    private void ShowEndLevelSituation()
    {
        var lastLevelDatas = Registry.GetLastPlayedLevelDatas();
        
        m_LevelEndContainer.UpdateUI(lastLevelDatas[0],lastLevelDatas[1],lastLevelDatas[2]);
        m_LevelEndContainer.transform.DOScale(Vector3.one, .1f);
        Registry.ShowEndLevelSituation = false;
        Registry.Save();
    }

    //Public Methods
    public void OpenLevelList()
    {
        m_InfiniteScrollView.UpdateScreenData();
        m_LevelsContainer.transform.DOKill();
        m_LevelsContainer.transform.DOScale(Vector3.one, .1f);
    }

    public void CloseLevelList()
    {
        m_LevelsContainer.transform.DOKill();
        m_LevelsContainer.transform.DOScale(Vector3.zero, .1f);
    }

    public void OpenLevelContainer(int level)
    {
        if (m_LevelContainer.IsOpen) return;

        m_SelectedLevel = level;
        m_LevelContainer.UpdateInfos(m_LevelInfos.LevelInfoList[level-1]);
        m_LevelContainer.transform.DOScale(Vector3.one, .1f);
    }

    public void PlayLevel()
    {
        Registry.LoadedLevelIndex = m_SelectedLevel;
        Registry.Save();

        SceneManager.LoadScene(1);
    }
}
