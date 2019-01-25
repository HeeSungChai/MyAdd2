using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBtnCtrl : MonoBehaviour
{
    public eCHARACTER m_eCharacter;
    public string m_strSprNameOpen;
    public string m_strSprNameClose;
    bool m_bIsActivated = false;
    public UISprite m_sprBtn;
    public TweenScale m_tweenScale;

    private void Awake()
    {
        //EventListener.AddListener("OnCharacterChanged", this);
    }

    void Start ()
    {
        //캐릭터 오픈 여부에 따라 스프라이트 바꿔주기
        if (m_eCharacter != eCHARACTER.ADD)
        {
            if (PrefsMgr.Instance.GetCharacterOpen(m_eCharacter))
                m_sprBtn.spriteName = m_strSprNameOpen;
            else
                m_sprBtn.spriteName = m_strSprNameClose;
        }
    }
	
    void OnCharacterChanged(eCHARACTER eCharacter)
    {
        if (m_eCharacter != eCHARACTER.ADD && 
            PrefsMgr.Instance.GetCharacterOpen(m_eCharacter) == false)
            return;

        if (eCharacter == m_eCharacter)
            OnActivate();
        else
            OnDeactivate();
    }

    public void OnActivate()
    {
        //m_tweenScale.enabled = true;
        //m_tweenScale.ResetToBeginning();
        m_tweenScale.PlayForward();

        m_bIsActivated = true;
    }

    public void OnDeactivate()
    {
        if (!m_bIsActivated)
            return;
        //m_tweenScale.enabled = true;
        //m_tweenScale.ResetToBeginning();
        m_tweenScale.PlayReverse();
    }
}
