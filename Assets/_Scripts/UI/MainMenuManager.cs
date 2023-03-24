using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MainMenuManager : SingletonBase<MainMenuManager>
{
    public GameObject LevelsButton;
    public GameObject LevelsContainer;
    public LevelInfos LevelInfos;
    public LevelContainer LevelContainer;

    protected override void Awake()
    {
        base.Awake();
        if (Registry.CurrentLevel <= 0)
        {
            Registry.CurrentLevel = 1;
            Registry.Save();
        }
    }

    public void OpenLevelList()
    {
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
        
        LevelContainer.UpdateInfos(LevelInfos.LevelInfoList[level-1]);
        LevelContainer.transform.DOScale(Vector3.one, .1f);
    }
}
