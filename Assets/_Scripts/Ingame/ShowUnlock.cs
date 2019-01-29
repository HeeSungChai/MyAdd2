using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowUnlock : MonoBehaviour
{
    public UISprite m_sprIcon;
    public UILabel m_labelName;
    public UILabel m_labelExplanation;

	public void init (string strIconName, string strName, string strExplanation)
    {
        m_sprIcon.spriteName = strIconName;
        m_sprIcon.MakePixelPerfect();
        m_labelName.text = strName;
        m_labelExplanation.text = strExplanation;
    }
}
