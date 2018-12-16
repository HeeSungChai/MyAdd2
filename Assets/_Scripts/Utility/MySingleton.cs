using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MySingleton<T> : MonoBehaviour where T : MySingleton<T>
{
    private static T m_Instance = null;
    private static bool m_ShouldDestroy = false;

    public static bool isInstanced
    {
        get
        {
            return m_Instance != null;
        }
    }

    public static T instance
    {
        get
        {
            if (m_ShouldDestroy)
                return null;

            if (m_Instance == null)
            {
                m_Instance = GameObject.FindObjectOfType(typeof(T)) as T;

                if (m_Instance == null)
                {
                    m_Instance = new GameObject(typeof(T).ToString(), typeof(T)).GetComponent<T>();

                    if (m_Instance == null)
                        Debug.LogError("Problem occured during the creation of " + typeof(T).ToString());

                }
            }
            return m_Instance;
        }
    }

    // If no other monobehaviour request the instance in an awake function
    // executing before this one, no need to search the object.
    private void Awake()
    {
        if (m_Instance == null)
        {
            m_Instance = this as T;
            m_Instance.Init();
        }
    }

    // This function is called when the instance is used the first time
    // Put all the initializations you need here, as you would do in Awake
    public virtual void Init() { }

    // Make sure the instance isn't referenced anymore when the user quit, just in case.
    public virtual void OnApplicationQuit()
    {
        m_Instance = null;
        m_ShouldDestroy = true;
    }
}
