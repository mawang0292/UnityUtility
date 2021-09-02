using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// * 해당 Sence의 UI를 관리하는 Manager
/// </summary>
public class UIManagerBase : MonoBehaviour
{
    private static UIManagerBase instance;

    /// <summary>
    /// * GetManager로 캐싱한 후 사용한다.
    /// * 제약조건 UIManager를 상속하고있는 Class
    /// </summary>
    public static T GetManager<T>() where T : UIManagerBase
    {
        if(instance == null)
        {
            instance = FindObjectOfType<T>();
        }

        T manager = instance as T;
        if(manager == null)
        {
            manager = FindObjectOfType<T>();
        }

        instance = manager;
        return manager;
    }

    /// <summary>
    /// * UIManager는 TryGetManager로 캐싱해서 사용한다.
    /// * 제약조건 UIManagerBase를 상속하고있는 Class
    /// </summary>
    public static bool TryGetManager<T>(out T manager) where T : UIManagerBase
    {
        if (instance == null)
        {
            instance = FindObjectOfType<T>();
        }

        manager = instance as T;
        if (manager == null)
        {
            manager = FindObjectOfType<T>();
        }

        instance = manager;
        return (instance != null);
    }

    /// <summary>
    /// * gameObject 활성화
    /// </summary>
    public virtual void Open()
    {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// * gameObject 비활성화
    /// </summary>
    public virtual void Close()
    {
        gameObject.SetActive(false);
    }
}
