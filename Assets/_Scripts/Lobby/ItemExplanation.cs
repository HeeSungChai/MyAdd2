﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemExplanation : MonoBehaviour
{
    [Header("Item Explanation")]
    public GameObject m_objSelectEraser;
    public GameObject m_objSelectClock;
    public GameObject m_objSelectRecovery;
    public UILabel m_labelItemName;
    public UILabel m_labelExplanation;
    private eTABLE_LIST m_eTableItem;
    eKEY_TABLEDB m_eKeyItemName;
    eKEY_TABLEDB m_eKeyItemExplanation;
    public TypewriterEffect m_typeWriter;

    void Start ()
    {
        EventListener.AddListener("OnLanguageChanged", this);
        m_eTableItem = eTABLE_LIST.ITEM_TABLE;

        m_objSelectEraser.SetActive(true);
        m_objSelectClock.SetActive(false);
        m_objSelectRecovery.SetActive(false);

        OnLanguageChanged();

        if (m_labelItemName)
            m_labelItemName.text = (string)TableDB.Instance.GetData(m_eTableItem,
                                        305, m_eKeyItemName);
    }

    void OnLanguageChanged()
    {
        if (LanguageMgr.Instance.GetLanguage() == eLANGUAGE.KOREAN)
        {
            m_eKeyItemName = eKEY_TABLEDB.s_ITEM_NAME_KR;
            m_eKeyItemExplanation = eKEY_TABLEDB.s_ITEM_INFO_KR;
        }
        else
        {
            m_eKeyItemName = eKEY_TABLEDB.s_ITEM_NAME_US;
            m_eKeyItemExplanation = eKEY_TABLEDB.s_ITEM_INFO_US;
        }
    }


    public void OnPressed_Eraser ()
    {
        if(m_labelItemName)
            m_labelItemName.text = (string)TableDB.Instance.GetData(m_eTableItem,
                                        305, m_eKeyItemName);

        if (m_labelExplanation)
            m_labelExplanation.text = (string)TableDB.Instance.GetData(m_eTableItem,
                                        305, m_eKeyItemExplanation);
        ResetTypeWriter();

        m_objSelectEraser.SetActive(true);
        m_objSelectClock.SetActive(false);
        m_objSelectRecovery.SetActive(false);
    }

    public void OnPressed_Clock()
    {
        if (m_labelItemName)
            m_labelItemName.text = (string)TableDB.Instance.GetData(m_eTableItem,
                                        306, m_eKeyItemName);

        if (m_labelExplanation)
            m_labelExplanation.text = (string)TableDB.Instance.GetData(m_eTableItem,
                                        306, m_eKeyItemExplanation);
        ResetTypeWriter();

        m_objSelectEraser.SetActive(false);
        m_objSelectClock.SetActive(true);
        m_objSelectRecovery.SetActive(false);
    }

    public void OnPressed_Recovery()
    {
        if (m_labelItemName)
            m_labelItemName.text = (string)TableDB.Instance.GetData(m_eTableItem,
                                        307, m_eKeyItemName);

        if (m_labelExplanation)
            m_labelExplanation.text = (string)TableDB.Instance.GetData(m_eTableItem,
                                        307, m_eKeyItemExplanation);
        ResetTypeWriter();

        m_objSelectEraser.SetActive(false);
        m_objSelectClock.SetActive(false);
        m_objSelectRecovery.SetActive(true);
    }

    void ResetTypeWriter()
    {
        if (m_typeWriter)
        {
            m_typeWriter.gameObject.SetActive(true);
            m_typeWriter.StartNewWrite();
        }
    }
}
