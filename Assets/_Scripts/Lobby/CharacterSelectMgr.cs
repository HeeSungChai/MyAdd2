using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectMgr : MonoBehaviour
{
    [Header("Gold Info")]
    public UILabel m_labelGoldAmount;

    [Header("Title Info")]
    private eTABLE_LIST m_eTableTitle;
    public UILabel m_labelTitle;
    public UILabel m_labelGrade;
    eKEY_TABLEDB m_eKeyTitleName;
    eKEY_TABLEDB m_eKeyGradeName;

    [Header("Character Info")]
    private eTABLE_LIST m_eTableChatacter;
    private eTABLE_LIST m_eTableCharacterLv;
    public UILabel m_labelCharName;
    public UILabel m_labelCharAbility;
    eKEY_TABLEDB m_eKeyCharName;
    eKEY_TABLEDB m_eKeyCharAbility;
    eCHARACTER m_eCurCharacter;
    int m_iCurLevel;
    //string tempString;

    private void Awake()
    {
        EventListener.AddListener("OnGoldAmountChanged", this);
        EventListener.AddListener("OnLanguageChanged", this);
        EventListener.AddListener("OnCharacterChanged", this);
    }

    void Start()
    {
        m_eTableTitle = eTABLE_LIST.TITLE_MARK;
        m_eTableChatacter = eTABLE_LIST.CHAR_INFO;

        OnGoldAmountChanged();
        OnLanguageChanged();
        OnCharacterChanged();
    }

    void OnGoldAmountChanged()
    {
        m_labelGoldAmount.text = MyUtility.CommaSeparateDigit(MyGlobals.UserState.m_iCoinAmount);
    }

    void OnLanguageChanged()
    {
        //언어설정에 따라 다른 키값 사용
        if (MyGlobals.Language == eLANGUAGE.KOREAN)
        {
            m_eKeyTitleName = eKEY_TABLEDB.s_TITLE_NAME_KR;
            m_eKeyGradeName = eKEY_TABLEDB.s_GRADE_NAME_KR;
            m_eKeyCharName = eKEY_TABLEDB.s_CHAR_NAME_KR;
            m_eKeyCharAbility = eKEY_TABLEDB.s_SKILL_KR;
        }
        else
        {
            m_eKeyTitleName = eKEY_TABLEDB.s_TITLE_NAME_US;
            m_eKeyGradeName = eKEY_TABLEDB.s_GRADE_NAME_US;
            m_eKeyCharName = eKEY_TABLEDB.s_CHAR_NAME_US;
            m_eKeyCharAbility = eKEY_TABLEDB.s_SKILL_US;
        }
    }

    void OnCharacterChanged()
    {
        RefreshGradeInfo();

        RefreshCharacterInfo();
    }

    void RefreshGradeInfo()
    {
        m_labelTitle.text = (string)TableDB.Instance.GetData(m_eTableTitle,
            (int)MyGlobals.UserState.m_eTitle, m_eKeyTitleName);

        string tempStr = (string)TableDB.Instance.GetData(m_eTableTitle,
            (int)MyGlobals.UserState.m_eTitle, m_eKeyGradeName);

        if (tempStr == null || tempStr == "noText")
            m_labelGrade.text = "";
        else
            m_labelGrade.text = tempStr;
    }

    void RefreshCharacterInfo()
    {
        UpdateCharacterTable();

        m_labelCharName.text = (string)TableDB.Instance.GetData(m_eTableChatacter,
            (int)MyGlobals.UserState.m_eCurCharacter, m_eKeyCharName);

        m_labelCharAbility.text = string.Format(
            (string)TableDB.Instance.GetData(m_eTableChatacter,
                (int)m_eCurCharacter, m_eKeyCharAbility),
            TableDB.Instance.GetData(m_eTableCharacterLv,
                (int)m_iCurLevel, eKEY_TABLEDB.i_SKILL_VALUE));

        //tempString = tempString.Replace("\\n", "\n");
    }

    void UpdateCharacterTable()
    {
        m_eCurCharacter = MyGlobals.UserState.m_eCurCharacter;
        switch (m_eCurCharacter)
        {
            case eCHARACTER.ADD:
                m_eTableCharacterLv = eTABLE_LIST.CHAR_LEVEL_ADD;
                m_iCurLevel = MyGlobals.UserState.m_iLvAdd;
                break;
            case eCHARACTER.SUB:
                m_eTableCharacterLv = eTABLE_LIST.CHAR_LEVEL_SUB;
                m_iCurLevel = MyGlobals.UserState.m_iLvMi;
                break;
            case eCHARACTER.MUL:
                m_eTableCharacterLv = eTABLE_LIST.CHAR_LEVEL_MUL;
                m_iCurLevel = MyGlobals.UserState.m_iLvDoubleRobo;
                break;
            case eCHARACTER.DIV:
                m_eTableCharacterLv = eTABLE_LIST.CHAR_LEVEL_DIV;
                m_iCurLevel = MyGlobals.UserState.m_iLvDividivi;
                break;
            default:
                m_eTableCharacterLv = eTABLE_LIST.CHAR_LEVEL_ADD;
                m_iCurLevel = MyGlobals.UserState.m_iLvAdd;
                break;
        }
    }
}
