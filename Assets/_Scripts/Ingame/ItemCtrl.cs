using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eITEM_TYPE
{
    ERASER,
    CLOCK,
    RECOVERY
}

public class ItemCtrl : MonoBehaviour
{
    public ItemCoolTimeCtrl[] m_scriptCoolTime;

	void Start ()
    {
		
	}

    public void OnPress_Eraser()
    {
        //일거리. 화면상에 떨어지는 숫자가 없으면 무시


        if (m_scriptCoolTime[(int)eITEM_TYPE.ERASER].IsCoolTime())
            return;

        m_scriptCoolTime[(int)eITEM_TYPE.ERASER].OnActivate();
        EventListener.Broadcast("OnCorrectAnswer");
    }

    public void OnPress_Clock()
    {
        if (m_scriptCoolTime[(int)eITEM_TYPE.CLOCK].IsCoolTime())
            return;

        m_scriptCoolTime[(int)eITEM_TYPE.CLOCK].OnActivate();
        EventListener.Broadcast("OnTimePaused");
    }

    public void OnPress_Heart()
    {
        //일거리. 현재 HP가 꽉 차있으면 무시


        if (m_scriptCoolTime[(int)eITEM_TYPE.RECOVERY].IsCoolTime())
            return;

        m_scriptCoolTime[(int)eITEM_TYPE.RECOVERY].OnActivate();
        EventListener.Broadcast("OnRecoverHp");
    }
}
