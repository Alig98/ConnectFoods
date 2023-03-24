using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class InfiniteScrollView : StandaloneInputModule,IDragHandler
{
    //fields
    [SerializeField] private List<LevelButton> m_LevelsRectTransformList;
    private Vector2 m_FirstSibblingStartPos;
    private Vector2 m_LastSibblingStartPos;
    private int m_LastScreenWidth = 0;
    private int m_LastScreenHeight = 0;
    
    //unity methods
    protected override void Start()
    {
        //execution order'ın hiçbir aşamasında veri güncellenmediği için böyle bir çözüm buldum.
        Invoke(nameof(OnScreenSizeChanged),.01f);

        var currentLevel = Registry.CurrentLevel;
        for (int i = 0; i < m_LevelsRectTransformList.Count; i++)
        {
            m_LevelsRectTransformList[i].SetLevel(currentLevel+i);
        }
    }

    //private methods
    private void OnScreenSizeChanged()
    {
        m_LastScreenWidth = Screen.width;
        m_LastScreenHeight = Screen.height;
        
        m_FirstSibblingStartPos = m_LevelsRectTransformList[0].transform.localPosition;
        m_LastSibblingStartPos = m_LevelsRectTransformList[m_LevelsRectTransformList.Count - 1].transform.localPosition;
    }

    //public methods
    public void OnDrag(PointerEventData data)
    {
        var firstUnit = m_LevelsRectTransformList[0];

        if (firstUnit.GetLevel() == 1 && firstUnit.transform.localPosition.y <= m_FirstSibblingStartPos.y && data.delta.y < 0)
        {
            return;
        }
        
        var lastUnit = m_LevelsRectTransformList[m_LevelsRectTransformList.Count-1];

        //Eğer resolution değişirse start pozisyonları tekrar alıyor
        if (m_LastScreenWidth != Screen.width || m_LastScreenHeight != Screen.height)
        {
            OnScreenSizeChanged();
        }
        
        //Scroll yapıyor.
        for (int i = 0; i < m_LevelsRectTransformList.Count; i++)
        {
            m_LevelsRectTransformList[i].transform.position += Vector3.up*data.delta.y;
        }

        if (m_LevelsRectTransformList[1].transform.localPosition.y >= m_FirstSibblingStartPos.y)
        {
            firstUnit.transform.localScale = Vector3.zero;
            firstUnit.DOKill();
            firstUnit.transform.DOScale(Vector3.one, .1f);
            firstUnit.transform.SetAsLastSibling();
            firstUnit.SetLevel(lastUnit.GetLevel()+1);
            
            m_LevelsRectTransformList.Remove(firstUnit);
            m_LevelsRectTransformList.Add(firstUnit);
        }
        

        if (m_LevelsRectTransformList[m_LevelsRectTransformList.Count-2].transform.localPosition.y <= m_LastSibblingStartPos.y)
        {
            lastUnit.transform.localScale = Vector3.zero;
            lastUnit.DOKill();
            lastUnit.transform.DOScale(Vector3.one, .1f);
            lastUnit.transform.SetAsFirstSibling();
            lastUnit.SetLevel(firstUnit.GetLevel()-1);
            
            m_LevelsRectTransformList.Remove(lastUnit);
            m_LevelsRectTransformList.Insert(0,lastUnit);
        }
    }
}