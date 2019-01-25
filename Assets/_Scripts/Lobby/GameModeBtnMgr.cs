using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModeBtnMgr : MonoBehaviour
{
    public GameObject m_objOnlyAdventureBtn;
    public GameObject m_objAdventureAndInfinityBtn;

    void Start ()
    {
		if(PrefsMgr.Instance.GetMaxSelectableLv() > 40)
        {
            m_objOnlyAdventureBtn.SetActive(false);
            m_objAdventureAndInfinityBtn.SetActive(true);
        }
        else
        {
            m_objOnlyAdventureBtn.SetActive(true);
            m_objAdventureAndInfinityBtn.SetActive(false);
        }
	}
}
