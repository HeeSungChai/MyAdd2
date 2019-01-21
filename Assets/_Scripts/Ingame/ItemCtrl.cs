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
        if (MyGlobals.StageMgr.IsDeactiveSkill)
            return;

        //화면상에 떨어지는 숫자가 없으면 무시
        if (MyGlobals.DigitSpawner.DigitsCount <= 0)
            return;

        if (m_scriptCoolTime[(int)eITEM_TYPE.ERASER].IsCoolTime())
            return;

        m_scriptCoolTime[(int)eITEM_TYPE.ERASER].OnActivate();
        EventListener.Broadcast("OnCorrectAnswer", true);
    }

    public void OnPress_Clock()
    {
        if (MyGlobals.StageMgr.IsDeactiveSkill)
            return;

        if (m_scriptCoolTime[(int)eITEM_TYPE.CLOCK].IsCoolTime())
            return;

        m_scriptCoolTime[(int)eITEM_TYPE.CLOCK].OnActivate();
        EventListener.Broadcast("OnTimePaused");
    }

    public void OnPress_Heart()
    {
        if (MyGlobals.StageMgr.IsDeactiveSkill)
            return;

        //현재 HP가 꽉 차있으면 무시
        if (MyGlobals.HpBarMgr.IsHpFull())
            return;

        if (m_scriptCoolTime[(int)eITEM_TYPE.RECOVERY].IsCoolTime() ||
            m_scriptCoolTime[(int)eITEM_TYPE.RECOVERY].IsDisable())
            return;

        m_scriptCoolTime[(int)eITEM_TYPE.RECOVERY].OnActivate();
        EventListener.Broadcast("OnRecoverHp");
    }
}
