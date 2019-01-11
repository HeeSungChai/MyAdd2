using System.Collections;
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
    int m_iSelectedItemIndex;

    private void Awake()
    {
        EventListener.AddListener("OnLanguageChanged", this);
    }

    void OnEnable ()
    {
        m_eTableItem = eTABLE_LIST.ITEM_TABLE;

        m_iSelectedItemIndex = 305;
        m_objSelectEraser.SetActive(true);
        m_objSelectClock.SetActive(false);
        m_objSelectRecovery.SetActive(false);

        OnLanguageChanged();
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

        if (m_labelItemName)
            m_labelItemName.text = (string)TableDB.Instance.GetData(m_eTableItem,
                                        m_iSelectedItemIndex, m_eKeyItemName);
        if (m_labelExplanation)
            m_labelExplanation.text = (string)TableDB.Instance.GetData(m_eTableItem,
                                        m_iSelectedItemIndex, m_eKeyItemExplanation);

        ResetTypeWriter();
    }


    public void OnPressed_Eraser ()
    {
        m_iSelectedItemIndex = 305;
        if (m_labelItemName)
            m_labelItemName.text = (string)TableDB.Instance.GetData(m_eTableItem,
                                        m_iSelectedItemIndex, m_eKeyItemName);

        if (m_labelExplanation)
            m_labelExplanation.text = (string)TableDB.Instance.GetData(m_eTableItem,
                                        m_iSelectedItemIndex, m_eKeyItemExplanation);
        ResetTypeWriter();

        m_objSelectEraser.SetActive(true);
        m_objSelectClock.SetActive(false);
        m_objSelectRecovery.SetActive(false);
    }

    public void OnPressed_Clock()
    {
        m_iSelectedItemIndex = 306;

        if (m_labelItemName)
            m_labelItemName.text = (string)TableDB.Instance.GetData(m_eTableItem,
                                        m_iSelectedItemIndex, m_eKeyItemName);

        if (m_labelExplanation)
            m_labelExplanation.text = (string)TableDB.Instance.GetData(m_eTableItem,
                                        m_iSelectedItemIndex, m_eKeyItemExplanation);
        ResetTypeWriter();

        m_objSelectEraser.SetActive(false);
        m_objSelectClock.SetActive(true);
        m_objSelectRecovery.SetActive(false);
    }

    public void OnPressed_Recovery()
    {
        m_iSelectedItemIndex = 307;

        if (m_labelItemName)
            m_labelItemName.text = (string)TableDB.Instance.GetData(m_eTableItem,
                                        m_iSelectedItemIndex, m_eKeyItemName);

        if (m_labelExplanation)
            m_labelExplanation.text = (string)TableDB.Instance.GetData(m_eTableItem,
                                        m_iSelectedItemIndex, m_eKeyItemExplanation);
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
