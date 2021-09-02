using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// * UIBase
/// * UI 스크립트 작성시 상속하는 class
/// </summary>
public class UIBase : MonoBehaviour
{
    //* activeInHierarchy : SetActive(bool) 에 따라 변화, 또한 부모가 있으면 부모의 SetActive(bool)에 영향을 받는다.
    public bool isOpend => gameObject.activeInHierarchy;

    //* rtf : 자신의 RectTransform 을 가져온다.
    public RectTransform rtf { get { return rtf; } set { rtf = value; } }

    //* GameObject 활성화
    public virtual void Open()
    {
        gameObject.SetActive(true);
    }
    
    //* GameObject 비활성화
    public virtual void Close()
    {
        gameObject.SetActive(false);
    }
}
