using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeCtrl : MonoBehaviour
{
    float m_fDelta;
    //public UICamera m_camera;
    public LevelRouletteCtrl m_scriptRoulette;

    public void OnReleased ()
    {
        //MyUtility.DebugLog("Release Delta : " + UICamera.currentTouch.totalDelta);
        m_fDelta = UICamera.currentTouch.totalDelta.x;

        if (m_fDelta > 100 || m_fDelta < -100)
        {
            int iDeltaLevel = -((int)(m_fDelta / 100));
            m_scriptRoulette.OnSwipe(iDeltaLevel);
        }
    }
}
