using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayItemAmount : MonoBehaviour
{
    public UILabel m_labelAmount_Eraser;
    public UILabel m_labelAmount_Clock;
    public UILabel m_labelAmount_Recovery;

    void OnEnable ()
    {
        m_labelAmount_Eraser.text = PrefsMgr.Instance.GetItemAmount(eITEM_ID.ERASER).ToString();
        m_labelAmount_Clock.text = PrefsMgr.Instance.GetItemAmount(eITEM_ID.CLOCK).ToString();
        m_labelAmount_Recovery.text = PrefsMgr.Instance.GetItemAmount(eITEM_ID.RECOVERY).ToString();
    }
}
