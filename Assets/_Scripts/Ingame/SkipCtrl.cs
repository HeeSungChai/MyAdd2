using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkipCtrl : MonoBehaviour
{
    public void OnTouchForSkip()
    {
        EventListener.Broadcast("OnTouched");
    }
}
