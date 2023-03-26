using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : SingletonBase<MainMenuManager>
{
    public GameObject LevelsContainer;
    public LevelInfos LevelInfos;
    public LevelContainer LevelContainer;
    public LevelEndContainer LevelEndContainer;
    public InfiniteScrollView InfiniteScrollView;
    private int m_SelectedLevel;

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

    private void ShowEndLevelSituation()
    {
        var lastLevelDatas = Registry.GetLastPlayedLevelDatas();
        
        LevelEndContainer.UpdateUI(lastLevelDatas[0],lastLevelDatas[1],lastLevelDatas[2]);
        LevelEndContainer.transform.DOScale(Vector3.one, .1f);
        Registry.ShowEndLevelSituation = false;
        Registry.Save();
    }

    public void OpenLevelList()
    {
        InfiniteScrollView.UpdateScreenData();
        LevelsContainer.transform.DOKill();
        LevelsContainer.transform.DOScale(Vector3.one, .1f);
    }

    public void CloseLevelList()
    {
        LevelsContainer.transform.DOKill();
        LevelsContainer.transform.DOScale(Vector3.zero, .1f);
    }

    public void OpenLevelContainer(int level)
    {
        if (LevelContainer.IsOpen) return;

        m_SelectedLevel = level;
        LevelContainer.UpdateInfos(LevelInfos.LevelInfoList[level-1]);
        LevelContainer.transform.DOScale(Vector3.one, .1f);
    }

    public void PlayLevel()
    {
        Registry.LoadedLevelIndex = m_SelectedLevel;
        Registry.Save();

        SceneManager.LoadScene(1);
    }
}
