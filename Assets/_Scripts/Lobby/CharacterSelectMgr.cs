using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class CharacterSelectMgr : MonoBehaviour
{
    [Header("Gold Info")]
    public UILabel m_labelGoldAmount;

    [Header("Character Btn")]
    public GameObject m_objSelectAdd;
    public GameObject m_objSelectSub;
    public GameObject m_objSelectMul;
    public GameObject m_objSelectDiv;

    [Header("Character Profile")]
    public UILabel m_labelCharName;
    public UILabel m_labelCharStory;
    public UILabel m_labelSkillLv;
    public UILabel m_labelSkillExplanation;
    public UILabel m_labelSkillLvCur;
    public UILabel m_labelSkillBonusCur;
    public UILabel m_labelSkillLvNext;
    public UILabel m_labelSkillBonusNext;
    public UILabel m_labelAquiredCoinAmount;
    private eTABLE_LIST m_eTableChatacter;
    private eTABLE_LIST m_eTableCharacterLv;
    eKEY_TABLEDB m_eKeyCharName;
    eKEY_TABLEDB m_eKeyCharStory;
    eKEY_TABLEDB m_eKeyCharSkillExplanation;

    private void Awake()
    {
        EventListener.AddListener("OnGoldAmountChanged", this);
        EventListener.AddListener("OnLanguageChanged", this);
        EventListener.AddListener("OnCharacterChanged", this);
    }

    void Start()
    {
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
        if (MyGlobals.Language == eLANGUAGE.KOREAN)
        {
            m_eKeyCharName = eKEY_TABLEDB.s_CHAR_NAME_KR;
            m_eKeyCharStory = eKEY_TABLEDB.s_CHAR_STORY_KR;
            m_eKeyCharSkillExplanation = eKEY_TABLEDB.s_SKILL_KR;
        }
        else
        {
            m_eKeyCharName = eKEY_TABLEDB.s_CHAR_NAME_US;
            m_eKeyCharStory = eKEY_TABLEDB.s_CHAR_STORY_US;
            m_eKeyCharSkillExplanation = eKEY_TABLEDB.s_SKILL_US;
        }
    }

    void OnCharacterChanged()
    {
        RefreshCharacterInfo();
    }

    void RefreshCharacterInfo()
    {
        RetargetCharacterSelect();

        m_labelCharName.text = (string)TableDB.Instance.GetData(m_eTableChatacter,
            (int)MyGlobals.UserState.m_eCurCharacter, m_eKeyCharName);

        m_labelCharStory.text = (string)TableDB.Instance.GetData(m_eTableChatacter,
            (int)MyGlobals.UserState.m_eCurCharacter, m_eKeyCharStory);

        m_labelSkillLv.text = MyUtility.GetLevelText(MyGlobals.UserState.GetCurSkillLv());

        int iSkillLevelCur = MyGlobals.UserState.GetCurSkillLv();
        int iSkillLevelNext = iSkillLevelCur + 1;

        m_labelSkillExplanation.text = string.Format(
            (string)TableDB.Instance.GetData(m_eTableChatacter,
                                    (int)MyGlobals.UserState.m_eCurCharacter, m_eKeyCharSkillExplanation),
                                TableDB.Instance.GetData(m_eTableCharacterLv,
                                    iSkillLevelCur, eKEY_TABLEDB.i_SKILL_VALUE));

        m_labelSkillLvCur.text = m_labelSkillLv.text;
        m_labelSkillBonusCur.text = ((int)TableDB.Instance.GetData(m_eTableCharacterLv,
                                    iSkillLevelCur, eKEY_TABLEDB.i_SKILL_VALUE)).ToString();

        if (iSkillLevelNext <= 10)
        {
            m_labelSkillLvNext.text = m_labelSkillLv.text;
            m_labelSkillBonusNext.text = ((int)TableDB.Instance.GetData(m_eTableCharacterLv,
                                    iSkillLevelNext, eKEY_TABLEDB.i_SKILL_VALUE)).ToString();
            m_labelAquiredCoinAmount.text = ((int)TableDB.Instance.GetData(m_eTableCharacterLv,
                                    iSkillLevelNext, eKEY_TABLEDB.i_AMOUNT)).ToString();
        }
        else
        {
            m_labelSkillLvNext.text = "";
            m_labelSkillBonusNext.text = "";
            m_labelAquiredCoinAmount.text = "";
        }

        //tempString = tempString.Replace("\\n", "\n");
    }

    void RetargetCharacterSelect()
    {
        m_objSelectAdd.SetActive(false);
        m_objSelectSub.SetActive(false);
        m_objSelectMul.SetActive(false);
        m_objSelectDiv.SetActive(false);

        switch (MyGlobals.UserState.m_eCurCharacter)
        {
            case eCHARACTER.ADD:
                m_eTableCharacterLv = eTABLE_LIST.CHAR_LEVEL_ADD;
                m_objSelectAdd.SetActive(true);
                break;
            case eCHARACTER.SUB:
                m_eTableCharacterLv = eTABLE_LIST.CHAR_LEVEL_SUB;
                m_objSelectSub.SetActive(true);
                break;
            case eCHARACTER.MUL:
                m_eTableCharacterLv = eTABLE_LIST.CHAR_LEVEL_MUL;
                m_objSelectMul.SetActive(true);
                break;
            case eCHARACTER.DIV:
                m_eTableCharacterLv = eTABLE_LIST.CHAR_LEVEL_DIV;
                m_objSelectDiv.SetActive(true);
                break;
            default:
                m_eTableCharacterLv = eTABLE_LIST.CHAR_LEVEL_ADD;
                m_objSelectAdd.SetActive(true);
                break;
        }
    }

    public void OnPress_Add()
    {
        MyGlobals.UserState.m_eCurCharacter = eCHARACTER.ADD;

        OnCharacterChanged();
    }

    public void OnPress_Sub()
    {
        MyGlobals.UserState.m_eCurCharacter = eCHARACTER.SUB;

        OnCharacterChanged();
    }

    public void OnPress_Mul()
    {
        MyGlobals.UserState.m_eCurCharacter = eCHARACTER.MUL;

        OnCharacterChanged();
    }

    public void OnPress_Div()
    {
        MyGlobals.UserState.m_eCurCharacter = eCHARACTER.DIV;

        OnCharacterChanged();
    }
}
