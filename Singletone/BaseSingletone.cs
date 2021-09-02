using UnityEngine;

/// <summary>
/// * 싱글톤 인터페이스
/// * OnCreateInstance() 로 싱글톤을 생성한다.
/// * OnDestoryInstance() 로 싱글톤을 제거한다.
/// </summary>
public interface IBaseSingleton
{
    //* 생성
    void OnCreateInstance();
    //* 삭제
    void OnDestoryInstance();
}

/// <summary>
/// * BaseSingleton 기본형 싱글톤입니다.
/// * abstract 추상 클래스 BaseSingleton<T> 에 제약이 걸려있습니다.
/// * where T (T 에 대한 제약)
/// * T에 대한 제약 내용은 
/// * 1. class 참조형식이여야한다. 
/// * 2. IBaseSingletone 인터페이스를 반드시 구현해야한다.
/// * 3. new() T는 매개 변수가 없는 생성자가 있어야한다.
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class BaseSingleton<T> where T : class, IBaseSingleton, new()
{
    public static T instance { get; private set; }
   
    /// <summary>
    /// * Singleton 생성
    /// </summary>
    /// <returns>Instance 값</returns>
    public static T CreateInstance()
    {
        //* instance 가 null 일때 instance 를 생성한다.
        if(instance == null)
        {
            instance = new T();
            instance.OnCreateInstance();
        }
        return instance;
    }

    /// <summary>
    /// * Singletone 제거
    /// </summary>
    public static void DestoryInstance()
    {
        instance?.OnDestoryInstance();
        instance = null;
    }
}

/// <summary>
/// * BaseMonoSingleton MonoBehaviour를 상속받는 싱글톤입니다.
/// * abstract 추상 클래스 BaseMonoSingleton<T> 에 제약이 걸려있습니다.
/// * where T (T 에 대한 제약)
/// * T에 대한 제약 내용은 
/// * 1. class 참조형식이여야한다.
/// </summary>
/// <typeparam name="T"> class </typeparam>
public abstract class BaseMonoSingleton<T> : MonoBehaviour where T : class
{
    public static T instance { get; private set; }

    //* insatnce 를 받아올수있다.
    public static T1 GetInstance<T1>() where T1 : class, T
    {
        return instance as T1;
    }
    
    /// <summary>
    /// * Scene이 넘어갈때 Singletone 이 삭제될지 유지할지 체크합니다.
    /// </summary>
    [SerializeField]
    private bool isDontDestroyLoadScene = true;
    public bool IsDontDestroyLoadScene => isDontDestroyLoadScene;

    /// <summary>
    /// * Senec 이 시작될때 체크합니다.
    /// * 1. instance 가 null 이 아닐때 gameObject 삭제
    /// * 2. instance 가 null 일때 instance 에 this 또는 T 값을 넣어줍니다.
    /// * 값을 넣을후 IsDontDestroyLoadScene이 ture일때 오브젝트를 파괴하지않고 유지합니다.
    /// </summary>
    protected virtual void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this as T;
            if(IsDontDestroyLoadScene)
            {
                DontDestroyOnLoad(gameObject);
            }
        }
    }

    // *Instance 에 null을 넣어 instance를 제거합니다.
    private void OnDestroy() 
    {
        instance = null;
    }
}