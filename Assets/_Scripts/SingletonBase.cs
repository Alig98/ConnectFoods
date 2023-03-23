using UnityEngine;

public abstract class SingletonBase<T>: MonoBehaviour where T: MonoBehaviour
{
    //Fields
    private static T m_Instance;

    //Properties
    public static T Instance => m_Instance;

    //Unity methods
    protected virtual void Awake()
    {
        if (!m_Instance)
        {
            m_Instance = gameObject.GetComponent<T>();
        }
    }
}
