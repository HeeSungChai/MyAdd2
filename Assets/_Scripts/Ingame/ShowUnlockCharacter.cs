using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowUnlockCharacter : MonoBehaviour
{
    public GameObject m_objSub;
    public GameObject m_objMul;
    public GameObject m_objDiv;
    public UILabel m_labelSkill;
    public UILabel m_labelExplanation;

    public void init(eCHARACTER eChar, string strSkill, string strExplanation)
    {
        m_objSub.SetActive(false);
        m_objMul.SetActive(false);
        m_objDiv.SetActive(false);

        switch (eChar)
        {
            case eCHARACTER.SUB:
                m_objSub.SetActive(true);
                break;
            case eCHARACTER.MUL:
                m_objMul.SetActive(true);
                break;
            case eCHARACTER.DIV:
                m_objDiv.SetActive(true);
                break;
            default:                
                break;
        }

        m_labelSkill.text = strSkill;
        m_labelExplanation.text = strExplanation;
    }
}
