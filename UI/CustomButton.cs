using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using TMPro;

public class CustomButton : UIBase, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
    [SerializeField]
    private Vector3 m_OriginScale = new Vector3(1, 1, 1);
    public Vector3 originScale { get { return m_OriginScale; } private set { m_OriginScale = value; } }

    public bool useAnimation;

    [SerializeField]
    private Image m_Image;
    public Image image { get { return m_Image; } set { m_Image = value; } }

    [SerializeField]
    private TextMeshProUGUI m_Label;
    public TextMeshProUGUI label { get { return m_Label; } set { m_Label = value; } }

    public UnityEvent onClick;

    protected virtual void Awake()
    {
        if(originScale == Vector3.zero)
        {
            originScale = rtf.localScale;
        }
    }

    public override void Open()
    {
        base.Open();

        rtf.localScale = originScale;
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        if (useAnimation)
        {
            rtf.localScale = originScale + new Vector3(0.05f, 0.05f, 0.05f);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (useAnimation)
        {
            rtf.localScale = originScale;
        }
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        onClick?.Invoke();
        if(AudioManager.Instance == null)
        {
            return;
        }
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
#elif UNITY_ANDROID || UNITY_IPHONE
        if (AppManager.Instance.isTouchEffect == true)
        {
            var touchPosition = Input.touches[0].position;
            touchPosition = Camera.main.ScreenToWorldPoint(touchPosition);
        }
#endif
    }
}
