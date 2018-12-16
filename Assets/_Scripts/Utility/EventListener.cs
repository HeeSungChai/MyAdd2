using System;
using System.Collections.Generic; //List
using System.Reflection; //MethodInfo & Exception
using UnityEngine;

public sealed class EventListener
{
    struct ListenerMethodStruct
    {
        public ListenerMethodStruct(string listenerMethodName, MonoBehaviour listener)
        {
            this.listenerMethodName = listenerMethodName;
            this.listener = listener;
        }

        public string listenerMethodName;
        public MonoBehaviour listener;
    };

    #region Singleton Implementation
    private static readonly EventListener instance = new EventListener();
    public static EventListener Instance
    {
        get { return instance; }
    }
    #endregion
    
    private Multimap<string, ListenerMethodStruct> m_ListenerDatabase = new Multimap<string, ListenerMethodStruct>();

    public static void AddListener(string eventNameAndMethodName, MonoBehaviour listener)
    {
        AddListener(eventNameAndMethodName, eventNameAndMethodName, listener);
    }

    public static void AddListener(string eventNameAndMethodName, string listenerMethodName, MonoBehaviour listener)
    {
        if (string.IsNullOrEmpty(eventNameAndMethodName) ||
            string.IsNullOrEmpty(listenerMethodName) ||
            listener == null)
            return;

        foreach (ListenerMethodStruct value in instance.m_ListenerDatabase[eventNameAndMethodName])
        {
            if (value.listener.GetInstanceID().Equals(listener.GetInstanceID()) == true)
                return;
        }

        Type listenerType = listener.GetType();
        MethodInfo listenerMethod = listenerType.GetMethod(listenerMethodName,        //지정된 메서드를 지정된 바인딩 제약 조건으로 검색
                                                        BindingFlags.Static |
                                                        BindingFlags.NonPublic |
                                                        BindingFlags.Public |
                                                        BindingFlags.Instance);
        ///* (ref. https://msdn.microsoft.com/en-us/library/system.reflection.methodinfo(v=vs.110).aspx) *///
        if (listenerMethod == null) ////리스너에 해당 함수가 있는지 찾아보고 없으면 리턴
            return;

        instance.m_ListenerDatabase.Add(eventNameAndMethodName, new ListenerMethodStruct(listenerMethodName, listener));
    }

    public static void RemoveListener(string eventNameAndMethodName, MonoBehaviour listener)
    {
        RemoveListener(eventNameAndMethodName, eventNameAndMethodName, listener);
    }

    public static void RemoveListener(string eventNameAndMethodName, string listenerMethodName, MonoBehaviour listener)
    {
        if (string.IsNullOrEmpty(eventNameAndMethodName) == false)
            instance.m_ListenerDatabase.Remove(eventNameAndMethodName, new ListenerMethodStruct(listenerMethodName, listener));
    }

    public static void RemoveListener(MonoBehaviour listener)
    {
        foreach (string key in instance.m_ListenerDatabase.Keys)
            foreach (ListenerMethodStruct value in instance.m_ListenerDatabase[key])
                if (listener == value.listener)
                {
                    instance.m_ListenerDatabase.Remove(key, value);
                    break;
                }
    }

    public static void Broadcast(string eventNameAndMethodName, params object[] args)
    {
        if (true == string.IsNullOrEmpty(eventNameAndMethodName))
            return;

        // Avoid C# InvalidOperationException: Collection was modified; enumeration operation may not execute.
        // This is safe way.
        List<ListenerMethodStruct> activeList = new List<ListenerMethodStruct>();
        List<ListenerMethodStruct> deactiveList = new List<ListenerMethodStruct>();
        foreach (ListenerMethodStruct value in instance.m_ListenerDatabase[eventNameAndMethodName])
            if (value.listener)
                activeList.Add(value);
            else
                deactiveList.Add(value);
        foreach (ListenerMethodStruct value in activeList)
        {
            Type listenerType = value.listener.GetType();
            MethodInfo listenerMethod = listenerType.GetMethod(value.listenerMethodName, BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            if (listenerMethod == null)     // Never gonna happen. Just for sure
                return;
            try
            {
                //if (value.listener.gameObject.activeSelf)
                listenerMethod.Invoke(value.listener, args);    ////value.listener의 value.listenerMethodName를 실행

                //NET_INAPP_BUY_RESULT은 무조건 한번만 호출..
                if (value.listenerMethodName == "NET_INAPP_BUY_RESULT")
                    break;
            }
            catch (TargetException)
            {
                //Debug.Log("Target Method: " + listenerMethod.Name + " is wrong.");
            }
            catch (ArgumentException)
            {
                //Debug.Log("Target Method: " + listenerMethod.Name + " has wrong parameter matches.");
            }
            catch (TargetParameterCountException)
            {
                //Debug.Log("Target Method: " + listenerMethod.Name + " has different number of parameters.");
            }
        }
        foreach (ListenerMethodStruct value in deactiveList)
            RemoveListener(value.listener);
    }
}
