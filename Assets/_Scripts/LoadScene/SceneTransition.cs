using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransition : MonoBehaviour
{
    public string m_strScene = "<Insert scene name>";
    public float m_fDuration = 1.0f;
    public Color m_color = Color.black;
    public bool m_bEnterIngame;
    public INGAME_TYPE m_eGameType;

    public void PerformTransition()
    {
        if(m_bEnterIngame)
        {
            if (MyGlobals.EnteringIngame)
                return;

            MyGlobals.GameType = m_eGameType;

            MyGlobals.EnteringIngame = true;
            MyGlobals.EnterIngameFromOutgame = true;

            EventListener.Broadcast("OnEnterIngame");
        }

        Transition.LoadLevel(m_strScene, m_fDuration, m_color);
    }
}
